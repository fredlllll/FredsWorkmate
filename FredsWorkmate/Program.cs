using FredsWorkmate.Database;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;

namespace FredsWorkmate
{
    public class Program
    { 

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddRazorPagesOptions((o) => {

                //o.Conventions.AddPageRoute("/Index","");

                //o.Conventions.AddPageRoute("/Customers", "/Customers");
                //o.Conventions.AddPageRoute("/Customer", "/Customers/{id}");

            });
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(DatabaseContext.GetConnectionString()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
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

            app.Run();
        }
    }
}
