using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
                                 // IdentityDbContext<ApplicationUser>
                                 //  <ApplicationUser> tells Identity which class contains user acct info it needs to authenticate
  public class ToDoListContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Category> Categories { get; set; }  
    public DbSet<Item> Items { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ItemTag> ItemTags { get; set; }

    public ToDoListContext(DbContextOptions options) : base(options) { }
  }
}