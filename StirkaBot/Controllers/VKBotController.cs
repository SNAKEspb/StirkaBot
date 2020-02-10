using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StirkaBot.Common;
using StirkaBot.VKBot.Models;

namespace StirkaBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VKBotController : ControllerBase
    {
        public VKBotController(List<IUpdatesHandler<IIncomingMessage>> updatesHandlers, List<IUpdatesResultHandler<IIncomingMessage>> responseHandlers) {
            _updatesHandlers = updatesHandlers;
            _responseHandlers = responseHandlers;
        }
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static IVKBot bot = VKBot.VKBot.getInstanse(_logger);

        private List<IUpdatesHandler<IIncomingMessage>> _updatesHandlers;
        private List<IUpdatesResultHandler<IIncomingMessage>> _responseHandlers;

        // POST api/values
        [HttpPost]
        public Task<IActionResult> Post()
        {
            string messageBody = Util.getRawBody(HttpContext.Request.Body);
            _logger.Log(NLog.LogLevel.Info, $"Start bot process {messageBody}");
            var process = ProcessMessagesAsync(bot, messageBody);
            _logger.Log(NLog.LogLevel.Info, $"End bot process {process}");
            return process;
        }

        async Task<IActionResult> ProcessMessagesAsync(IVKBot bot, string messageBody)
        {
            try
            {
                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateMessage>(messageBody);
                //check chache
                //if cache already contains the message, then return ok result, else proceed
                ObjectCache cache = MemoryCache.Default;
                var cacheKey = message.peer_id + message.MessageType + message.from_id + message.date + (message.payload != null ? message.text : "");
                if (cache[cacheKey] != null)
                {
                    _logger.Log(NLog.LogLevel.Info, $"cache key found: {cacheKey}");
                    return Ok("ok");
                }
                cache.Add(cacheKey, message, DateTime.Now.AddMinutes(5));

                //handle logic with response to vk
                foreach (var handler in _responseHandlers)
                {
                    if (handler.CanHandle(message, bot))
                    {
                        var result = await handler.HandleAsync(message, bot);
                        return Ok(result.message);
                    }
                }

                //handle bot requests
                foreach (var handler in _updatesHandlers)
                {
                    if (handler.CanHandle(message, bot))
                    {
                        handler.HandleAsync(message, bot);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex, "Error during bot process");
                //return Ok(ex.ToString());
                return Ok("ok");
            }
            return Ok("ok");
        }
    }
}