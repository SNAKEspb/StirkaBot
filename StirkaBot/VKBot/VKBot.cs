using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Services;
using StirkaBot.VKBot.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace StirkaBot.VKBot
{
    public class VKBot : IVKBot
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private VKService vkService;
        private VKBotOptions _options;

        public VKBot(IOptions<VKBotOptions> options)
        {
            vkService = new VKService(_logger);
            _options = options.Value;
        }

        public string confimationCode => _options.confirmationCode;
        public async Task<string> sendMessageAsync(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                await messagesGetHistory(tmessage);
                await messagesGetConversations(tmessage);

                return await vkService.sendRequest(tmessage, "messages.send", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return null;
            }

        }

        public class HistoryRequest : IOutgoingMessage
        {
            public int offset { get; set; }
            public int count { get; set; }
            public string user_id { get; set; }
            public string peer_id { get; set; }
            public string start_message_id { get; set; }
            public string rev { get; set; }
            public string extended { get; set; }
            public string fields { get; set; }
            public string group_id { get; set; }
        }

        public async Task messagesGetHistory(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                var request = new HistoryRequest
                {
                    //offset = 0,
                    count = 1,
                    //user_id = "1556462",
                    peer_id = tmessage.peer_id,
                    //start_message_id = "0",
                    //rev = "1",
                    //extended = 
                    //fields = 
                    group_id = _options.groupId,
                };
                await vkService.sendRequest(request, "messages.getHistory", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
            }
        }

        public class Conversation : IOutgoingMessage
        {
            public int offset { get; set; }
            public int count { get; set; }
            public string filter { get; set; }
            public string start_message_id { get; set; }
            public string extended { get; set; }
            public string fields { get; set; }
            public string group_id { get; set; }

            public string major_sort_id { get; set; }
        }

        public async Task messagesGetConversations(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                var request = new Conversation
                {
                    offset = 0,
                    count = 200,
                    filter = "all",
                    //start_message_id = "0",
                    //extended = "1"
                    //fields = 
                    //group_id = _options.groupId,
                    //major_sort_id = 
                };
                await vkService.sendRequest(request, "messages.getConversations", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
            }
        }

    }
}
