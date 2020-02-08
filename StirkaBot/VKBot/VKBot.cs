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
        static string _token { get; set; } = "39f59d6db93fa0db9fe851ba9914f1a7883c3bd9167de02481e854a14956b1a34acc9f5dd8dbbadfa0104";
        static string _groupId { get; set; } = "191795794";
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
