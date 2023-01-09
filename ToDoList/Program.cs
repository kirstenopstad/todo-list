using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore; // imports the UseMySql() method 
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Models; // because we use we use the model ToDoListContext within Program.cs
using Microsoft.AspNetCore.Identity; // to get access to Identity

namespace ToDoList
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      // add EF Core as a service to our To Do List app, specify ToDoListContext as the type
      builder.Services.AddDbContext<ToDoListContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                          )
                        )
                      );

      
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<ToDoListContext>()
             .AddDefaultTokenProviders();

      builder.Services.Configure<IdentityOptions>(options =>
      {
        // Default Password settings.
        // options.Password.RequireDigit = true;
        // options.Password.RequireLowercase = true;
        // options.Password.RequireNonAlphanumeric = true;
        // options.Password.RequireUppercase = true;
        // options.Password.RequiredLength = 6;
        // options.Password.RequiredUniqueChars = 1;
        // Custom settings
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
      });

      // DBConfiguration.ConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

      WebApplication app = builder.Build();

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );

      app.Run();
    }
  }
}