using FredsWorkmate.Database;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;

namespace FredsWorkmate
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            Util.Python.Venv.EnsureVenvAndPackages(".venv", "pikepdf");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddRazorPagesOptions((o) => {
                //extra routes go here
            });
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(DatabaseContext.GetConnectionString()));
            builder.Services.AddControllers();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
