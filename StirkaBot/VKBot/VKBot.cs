using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Services;
using StirkaBot.VKBot.Models;

namespace StirkaBot.VKBot
{
    public class VKBot : IVKBot
    {
        static string _token { get; set; } = "0c29646fefcc442729f323eaf428f999dba1bcc95abfe3da03d0459c7b55fe6b965a59585b14c7a1c24af";
        static string _groupId { get; set; } = "179992947";
        static string _apiVersion { get; set; } = "5.103";
        static string _confirmationCode { get; set; } = "8ff28f3c";

        private NLog.Logger _logger;
        private VKService vkService;

        public VKBot(NLog.Logger logger)
        {
            _logger = logger;
            vkService = new VKService(logger);
        }

        private static VKBot _instanse;
        public static VKBot getInstanse(NLog.Logger logger)
        {
            if (_instanse == null)
            {
                _instanse = new VKBot(logger);
            }
            return _instanse;
        }

        public string confimationCode => _confirmationCode;
        public async Task<bool> SendMessageAsync(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                return await vkService.messagesSendAsync(tmessage, _groupId, _token, _apiVersion);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return false;
            }

        }

        public async Task processTextMessage(IIncomingMessage message)
        {
            try
            {
                //todo
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex, "getMessagesUploadServer Error");
            }
        }


    }
}
