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
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static IVKBot bot = VKBot.VKBot.getInstanse(_logger);
        static List<IUpdatesHandler<IIncomingMessage>> updatesHandler = new List<IUpdatesHandler<IIncomingMessage>>()
        {
            new TextMessageHandler(),
            new MenuMessageHandler()
        };

        static List<IUpdatesResultHandler<IIncomingMessage>> responseHandler = new List<IUpdatesResultHandler<IIncomingMessage>>()
        {
            new ConfirmationHandler(),
        };

        // GET: api/<controller>
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Forbid();
        //}

        // POST api/values
        [HttpPost]
        public Task<IActionResult> Post()
        {
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateMessage>(Util.getRawBody(HttpContext.Request.Body));
            _logger.Log(NLog.LogLevel.Info, $"Start bot process {message.text}");
            var process = ProcessMessagesAsync(bot, message);
            _logger.Log(NLog.LogLevel.Info, $"End bot process {process}");
            return process;
        }

        async Task<IActionResult> ProcessMessagesAsync(IVKBot bot, IIncomingMessage message)
        {
            try
            {
                //check chache
                //if cache already contains the message, then return ok result, else proceed
                ObjectCache cache = MemoryCache.Default;
                var cacheKey = message.peer_id + message.MessageType + message.from_id + message.date;
                if (cache[cacheKey] != null)
                {
                    _logger.Log(NLog.LogLevel.Info, $"cache key found: {cacheKey}");
                    return Ok("ok");
                }
                cache.Add(cacheKey, message, DateTime.Now.AddMinutes(5));
                //todo: separate interface for Task<HandlerResult>
                //handle logic with response to vk
                foreach (var handler in responseHandler)
                {
                    if (handler.CanHandle(message, bot))
                    {
                        var result = await handler.HandleAsync(message, bot);
                        return Ok(result.message);
                    }
                }
                //todo: separate interface for Task 
                //handle bot requests
                foreach (var handler in updatesHandler)
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
                return Ok(ex.ToString());
            }
            return Ok("ok");
        }
    }
}