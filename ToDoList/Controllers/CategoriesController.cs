using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {

    private readonly ToDoListContext _db;

    public CategoriesController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Category> model = _db.Categories.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
        // DbSet method 
        _db.Categories.Add(category);
        // DbContext method
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Category thisCategory = _db.Categories
                              .Include(category => category.Items)
                              // load the JoinEntities of each Item
                              .ThenInclude(item => item.JoinEntities)
                              // load the tag associated with each item
                              .ThenInclude(join => join.Tag)
                              .FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }
  }
}