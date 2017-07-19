using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnippetApp.Models;

namespace CodeSnippet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            using (var client = new MyDbContext()) {
                client.Database.EnsureCreated();
            }
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<MyDbContext>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Snippets/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Snippets}/{action=Index}/{id?}");
            });

            // seed data. this is not how we would typically do this, but
            // we just need some data for demonstration purposes
            var db = new MyDbContext();
            if (!db.Snippets.Any()) {
                var snippets = new List<Snippet>() {
                    new Snippet() { ID = 1, Title = "Div's for Life", Description = "Shows what a div looks like", ProgrammingLanguage = "HTML", CodeSnippet = "<div>Hello>/div>"},
                    new Snippet() { ID = 2, Title = "C# varible", Description = "Shows what a C# variable looks like", ProgrammingLanguage = "C#", CodeSnippet = "public int . . ."},
                };

                db.Snippets.AddRange(snippets);
                db.SaveChanges();
            }
        }
    }
}
