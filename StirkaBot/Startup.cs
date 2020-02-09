using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StirkaBot.VKBot.Models;

namespace StirkaBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<StirkaBot.Models.Flow>(new StirkaBot.Services.FlowService(null).initFlow());

            services.AddSingleton< List<IUpdatesHandler<IIncomingMessage>>>( (p) => new List<IUpdatesHandler<IIncomingMessage>>()
                    {
                        new TextMessageHandler(p.GetService<StirkaBot.Models.Flow>()),
                        new MenuMessageHandler(p.GetService<StirkaBot.Models.Flow>()),
                    });

            services.AddSingleton<List<IUpdatesResultHandler<IIncomingMessage>>>((p) => new List<IUpdatesResultHandler<IIncomingMessage>>()
                    {
                        new ConfirmationHandler(),
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
