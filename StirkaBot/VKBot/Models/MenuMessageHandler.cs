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

            string nodeId = payload["command"] != null ? "0" : (string)payload["node"];
            string linkId = payload["command"] != null ? "0" : (string)payload["link"];
            //string address = payload["command"] != null ? "0" : (string)payload["address"];
            string conversationId = payload["command"] != null ? "0" : (string)payload["conversationId"];

            var nextNode = _flow.getNextNode(nodeId, linkId);

            var currentLink = _flow.getCurrentLink(nodeId, linkId);
            string userName = await getUserNameAsync(message.from_id, bot);
            if (currentLink.type != null && currentLink.type == "address")
            {
                conversationId = await getConversationIdAync(message.text, bot);
            }

            if (conversationId != null) {
                var conversationMessage = new OutgoingMessage()
                {
                    peer_id = conversationId,
                    message = userName  + ": " + message.text
                };

                await bot.sendMessageAsync(conversationMessage);
            }

            var keyboard = Services.FlowService.convertToKeyboard(nextNode, conversationId);

            var outgoingMessage = new OutgoingMessage()
            {
                peer_id = message.peer_id,
                message = "> " + nextNode.label,
                keyboard = keyboard.ToString()
            };

            await bot.sendMessageAsync(outgoingMessage);
        }

        public async Task<string> getUserNameAsync(string userId, IVKBot bot)
        {
            var usersRequest = new VKBot.UsersRequest
            {
                user_ids = userId,
                name_case = "nom",
                //fields = 
            };
            var usersResponse = await bot.usersGetAsync(usersRequest);
            var users = ((JArray)JObject.Parse(usersResponse)["response"]).FirstOrDefault();
            var userName = users["first_name"] + " " + users["last_name"];
            return userName;
        }

        public async Task<string> getConversationIdAync(string conversationName, IVKBot bot)
        {
            var conversationRequest = new VKBot.ConversationRequest
            {
                offset = 0,
                count = 100,
                filter = "all"
            };
            var conversationResponse = await bot.messagesGetConversationsAsync(conversationRequest);
            var conversation = ((JArray)JObject.Parse(conversationResponse)["response"]["items"])
                .Where(t => t["conversation"]["chat_settings"] != null && (string)t["conversation"]["chat_settings"]["title"] == conversationName).FirstOrDefault();

            string converstionId = (string)conversation["conversation"]["peer"]["id"];
            return converstionId;
        }
    }
}
