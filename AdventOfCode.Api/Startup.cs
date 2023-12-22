using AdventOfCode.Api.Configuration;
using AdventOfCode.Api.Services;

public class Startup(IHostEnvironment environment, IConfiguration configuration)
{
    private readonly IHostEnvironment _env = environment;
    private readonly IConfiguration _config = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient();
        services.AddControllers();

        services.AddLogging(b => b.AddSimpleConsole());

        //services.AddScoped<LoggingMiddleware>();
        services.AddOptions<UserSecrets>().Bind(configuration);
        services.AddSingleton<IInputProvider, CachingInputProvider>();
        services.AddSingleton<IPuzzleFactory, PuzzleFactory>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.UseEndpoints(x =>
        {
            x.MapControllers();
        });
    }
}