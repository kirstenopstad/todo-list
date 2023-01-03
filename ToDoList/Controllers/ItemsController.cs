using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    
    private readonly ToDoListContext _db;

    // 'ToDoListContext' is the exact one from Program.cs
    public ItemsController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      // 'Items' matches what's set in ToDoListContext.cs
      List<Item> model = _db.Items
                            .Include(item => item.Category)
                            .OrderBy(item => item.DueBy)
                            .OrderBy(item => item.Status)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
        // SelectList(data that will populate <option> elements, value, displayed text)
        ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public ActionResult Create(Item item)
    {
        // if no category
        if (item.CategoryId == 0)
        {
          return RedirectToAction("Create");
        }
        // Update DbSet  
        _db.Items.Add(item);
        // Update DbContext 
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    // parameter needs to match the property from anonymous obj created in Create.cshtml
    public ActionResult Details(int id)
    {
        Item thisItem = _db.Items
                            .Include(item => item.Category)
                            .Include(item => item.JoinEntities)
                            .ThenInclude(join => join.Tag)
                            .FirstOrDefault(item => item.ItemId == id);
        return View(thisItem);
    }

    public ActionResult Edit(int id)
    {
        Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
                                // SelectList(data, "value", "display text")
        ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item)
    {
        _db.Items.Update(item);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        return View(thisItem);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        _db.Items.Remove(thisItem);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    
    public ActionResult AddTag(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult AddTag(Item item, int tagId)
    {
      #nullable enable
      ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.TagId == tagId && join.ItemId == item.ItemId));
      #nullable disable
      if (joinEntity == null && tagId != 0)
      {
        _db.ItemTags.Add(new ItemTag() { TagId = tagId, ItemId = item.ItemId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = item.ItemId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      ItemTag joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
      _db.ItemTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult StatusComplete(int itemId)
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == itemId);
      thisItem.Status = true;
      _db.Update(thisItem);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}