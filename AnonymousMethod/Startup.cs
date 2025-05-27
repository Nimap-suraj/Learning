using System.Threading.Tasks;

namespace AnonymousMethod
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("hello from Use Middleware 1-1 \n");
                await next();
                await context.Response.WriteAsync("hello from Use Middleware 1-2 \n");
            });
            app.Map("/suraj", Customcode);
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("hello from Use Middleware 2-1 \n");
                await next();
                await context.Response.WriteAsync("hello from Use Middleware 2-2 \n");
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Request Complete\n");
                await next(); // Important!
            });

            app.UseRouting();

            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllers(); 
            });
        }

        private void Customcode(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Custome code suraj here\n");
                await next();
            });
        }
    }
}
