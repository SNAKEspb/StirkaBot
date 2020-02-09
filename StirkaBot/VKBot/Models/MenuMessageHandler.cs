using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StirkaBot.VKBot.Models
{
    public class MenuMessageHandler : IUpdatesHandler<IIncomingMessage>
    {
        
        //FlowClass
        static string[] _messageTypes = new[] { "message_new", "message_reply" };

        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return _messageTypes.Contains(message.MessageType.ToLowerInvariant())
                && !string.IsNullOrWhiteSpace(message.payload)
                && !string.IsNullOrWhiteSpace(message.text);
        }
        public async Task HandleAsync(IIncomingMessage message, IVKBot bot)
        {
            var flow = new StirkaBot.Services.FlowService(null).initFlow();

            JObject payload = JObject.Parse(message.payload);

            string nodeId = payload["command"] != null ? "0" : payload["node"].ToString();
            string linkId = payload["command"] != null ? "0" : payload["link"].ToString();

            var node = flow.nodes[nodeId];
            var link = node.links[linkId];

            var keyboard = JObject.FromObject(new
            {
                one_time = false,
                buttons = link.node.links.Select(t => new[]
                {
                        new
                        {
                            action = new
                            {
                                type = "text",
                                payload = JObject.FromObject(new
                                {
                                    node = link.node.id,
                                    link = t.Value.id,
                                    label = t.Value.label
                                }).ToString(),
                                label = t.Value.label
                            },
                            color = t.Value.color
                        }
                    })
            });


            var outgoingMessage = new OutgoingMessage()
            {
                peer_id = message.peer_id,
                message = DateTime.Now.ToString(),
                keyboard = keyboard.ToString()
            };

            await bot.sendMessageAsync(outgoingMessage);
        }
    }
}
