using SimpleWebStore.DAL.Extensions;

namespace BookWebStore.UI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDalServices(_configuration);
            // services.AddBllServices(_configuration);

            //services.AddNotyf(config =>
            //{
            //    config.DurationInSeconds = 3;
            //    config.IsDismissable = true;
            //    config.Position = NotyfPosition.BottomRight;
            //    config.HasRippleEffect = true;
            //});

            services.AddMvc();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });                  
        }
    }
}
