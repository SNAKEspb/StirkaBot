using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StirkaBot.VKBot.Models
{
    public class TextMessageHandler : IUpdatesHandler<IIncomingMessage>  
    {
        //todo:move statics to bot
        static string[] _messageTypes = new[] { "message_new", "message_reply" };

        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return _messageTypes.Contains(message.MessageType.ToLowerInvariant())
                && string.IsNullOrWhiteSpace(message.payload)
                && !string.IsNullOrWhiteSpace(message.text);
        }
        public async Task HandleAsync(IIncomingMessage message, IVKBot bot)
        {
            var flow = new StirkaBot.Services.FlowService(null).initFlow();

            string nodeId = "0";
            string linkId = "0";

            var node = flow.nodes[nodeId];
            var link = node.links[linkId];
            var nextNode = link.node;

            var keyboard = JObject.FromObject(new
            {
                one_time = false,
                buttons = nextNode.links.Select(t => new[]
                {
                        new
                        {
                            action = new
                            {
                                type = "text",
                                payload = JObject.FromObject(new
                                {
                                    node = nextNode.id,
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
                message = nextNode.label,
                keyboard = keyboard.ToString()
            };
            await bot.sendMessageAsync(outgoingMessage);
        }
    }
}
