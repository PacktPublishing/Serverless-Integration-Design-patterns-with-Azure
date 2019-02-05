using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Configuration;

namespace GremlinGraphAPI
{
    public class Startup
    {
        private static string hostname = System.Environment.GetEnvironmentVariable("cosmos db gremlin uri");
        private static string port = "443";
        private static string collection = System.Environment.GetEnvironmentVariable("collectionname");
        private static string authKey = System.Environment.GetEnvironmentVariable("Auth Key");
        private static string database = System.Environment.GetEnvironmentVariable("cosmos db database name");

        private GremlinClient client;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var server = new GremlinServer(hostname, 443, enableSsl: true,
                                         username: "/dbs/" + database + "/colls/" + collection,
                                         password: authKey);

            client = new GremlinClient(server, new GraphSON2Reader(),
                     new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }




        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(client);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Cosmos Graph Database", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
