using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StirkaBot.VKBot.Models;

namespace StirkaBot
{
    public interface IUpdatesHandler<T>
    {
        bool CanHandle(T message, IVKBot bot);
        Task HandleAsync(T message, IVKBot bot);
    }
}
