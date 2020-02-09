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
        private StirkaBot.Models.Flow _flow;

        public TextMessageHandler(StirkaBot.Models.Flow flow) {
            _flow = flow;
        }

        //todo:move statics to bot
        static string[] _messageTypes = new[] { "message_new", "message_reply" };

        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return _messageTypes.Contains(message.MessageType.ToLowerInvariant())
                && string.IsNullOrWhiteSpace(message.payload)
                && !string.IsNullOrWhiteSpace(message.text) 
                && message.text.ToLower() == "начать";
        }
        public async Task HandleAsync(IIncomingMessage message, IVKBot bot)
        {
            string nodeId = "0";
            string linkId = "0";

            var nextNode = _flow.getNextNode(nodeId, linkId);

            var keyboard = Services.FlowService.convertToKeyboard(nextNode);


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
