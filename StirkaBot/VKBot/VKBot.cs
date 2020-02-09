using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Services;
using StirkaBot.VKBot.Models;
using Newtonsoft.Json.Linq;

namespace StirkaBot.VKBot
{
    public class VKBot : IVKBot
    {
        static string _token { get; set; } = "39f59d6db93fa0db9fe851ba9914f1a7883c3bd9167de02481e854a14956b1a34acc9f5dd8dbbadfa0104";
        static string _groupId { get; set; } = "191795794";
        static string _apiVersion { get; set; } = "5.103";
        static string _confirmationCode { get; set; } = "8ff28f3c";

        private StirkaBot.Models.Flow flow;

        private NLog.Logger _logger;
        private VKService vkService;
        private FlowService flowService;

        public VKBot(NLog.Logger logger)
        {
            _logger = logger;
            vkService = new VKService(logger);
            flowService = new FlowService(logger);

            flow = flowService.initFlow();
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
        public async Task<bool> sendMessageAsync(IOutgoingMessage message)
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
                //string payload = JObject.FromObject(new
                //{
                //    node = "1",
                //    link = "1"
                //}).ToString();

                //{
                //    JObject payload = JObject.Parse(message.payload);
                //}

                //string nodeId = payload["command"].ToString() == "start" || message.text == "Начать" ? "0" : payload["node"].ToString();
                //string linkId = payload["command"].ToString() == "start" || message.text == "Начать" ? "0" : payload["link"].ToString();

                //var node = flow.nodes[nodeId];
                //var link = node.links[linkId];

                //var keyboard = JObject.FromObject(new
                //{
                //    one_time = false,
                //    buttons = link.node.links.Select(t => new[]
                //    {
                //        new
                //        {
                //            action = new
                //            {
                //                type = "text",
                //                payload = JObject.FromObject(new
                //                {
                //                    node = link.node.id,
                //                    link = t.Value.id,
                //                    label = t.Value.label
                //                }).ToString(),
                //                label = t.Value.label
                //            },
                //            color = t.Value.color
                //        }
                //    })
                //});

                ////new[] {

                ////            new[] {
                ////                new {
                ////                    action = new {
                ////                        type = "text",
                ////                        payload = "{\"button\": \"1\"}",
                ////                        label = "Negative"
                ////                    },
                ////                    color = "negative"
                ////                },
                ////                new {
                ////                    action = new {
                ////                        type = "text",
                ////                        payload = "{\"button\": \"2\"}",
                ////                        label = "Positive"
                ////                    },
                ////                    color = "positive"
                ////                },
                ////                new {
                ////                    action = new {
                ////                        type = "text",
                ////                        payload = "{\"button\": \"3\"}",
                ////                        label = "Primary"
                ////                    },
                ////                    color = "primary"
                ////                },
                ////                new {
                ////                    action = new {
                ////                        type = "text",
                ////                        payload = "{\"button\": \"4\"}",
                ////                        label = "Secondary"
                ////                    },
                ////                    color = "secondary"
                ////                },
                ////            },
                ////    }



                //await vkService.messagesSendAsync(outgoingMessage, _groupId, _token, _apiVersion);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex, "processTextMessage Error");
            }
        }


    }
}
