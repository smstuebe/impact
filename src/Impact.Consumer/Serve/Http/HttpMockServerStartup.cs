﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Impact.Consumer.Serve.Http
{
    public class HttpMockServerStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}