using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddUserSecrets(Assembly.GetExecutingAssembly());
            })
            .Build().Run();
    }
}
