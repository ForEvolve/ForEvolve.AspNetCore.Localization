using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebPagesSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
#if NET2
                .AddMvc()
#else
                .AddRazorPages()
#endif
                .AddForEvolveLocalization()
            ;
        }

        public void Configure(
            IApplicationBuilder app,
#if NET2
            IHostingEnvironment env
#else
            IWebHostEnvironment env
#endif
        )
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRequestLocalization();
#if NET2
            app.UseMvc();
#else
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
#endif
        }
    }
}
