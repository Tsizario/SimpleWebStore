namespace BookWebStore.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration);

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app);

            if (args.Length == 1 && args[0].ToLower() == "/seed")
            {
                // RunSeeding(app);
            }
            else
            {
                app.Run();
            }
        }
    }
}
