using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StirkaBot.VKBot.Models
{
    public class MenuMessageHandler : IUpdatesHandler<IIncomingMessage>
    {
        private StirkaBot.Models.Flow _flow;

        public MenuMessageHandler(StirkaBot.Models.Flow flow)
        {
            _flow = flow;
        }

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
            JObject payload = JObject.Parse(message.payload);

            string nodeId = payload["command"] != null ? "0" : payload["node"].ToString();
            string linkId = payload["command"] != null ? "0" : payload["link"].ToString();

            var nextNode = _flow.getNextNode(nodeId, linkId);

            var keyboard = Services.FlowService.convertToKeyboard(nextNode);

            var outgoingMessage = new OutgoingMessage()
            {
                peer_id = message.peer_id,
                message = "> " + nextNode.label,
                keyboard = keyboard.ToString()
            };

            await bot.sendMessageAsync(outgoingMessage);
        }
    }
}
