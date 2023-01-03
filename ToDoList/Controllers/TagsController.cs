using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Controllers
{
  public class TagsController : Controller

  {
    // property
    private readonly ToDoListContext _db;

    // constructor
    public TagsController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      // pass the ToList method directly to View as argument
      return View(_db.Tags.ToList());
    }

    public ActionResult Details(int id)
    {               // gives us a list of Tag objects from the database
      Tag thisTag = _db.Tags
                    // load the JoinEntities property of each Tag
                    .Include(tag => tag.JoinEntities)
                    // load the Item object associated with each ItemTag
                    .ThenInclude(join => join.Item)
                    .FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Tag tag)
    {
      _db.Tags.Add(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddItem(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      ViewBag.ItemId = new SelectList(_db.Items, "ItemId", "Description");
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult AddItem(Tag tag, int itemId)
    {
      // nullable directives to enable nullable types
      #nullable enable
      // query for first itemtag that contains matching ItemId & TagId
      // if nothing is found, value of joinEntity is null (default return when nothing found)
      // make it a nullable type by including ? after type [i.e. ItemTag?]
      ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.ItemId == itemId && join.TagId == tag.TagId));
      #nullable disable
      if (joinEntity == null && itemId != 0)
      {
        _db.ItemTags.Add(new ItemTag() {ItemId = itemId , TagId = tag.TagId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = tag.TagId});
    }

    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult Edit(Tag tag)
    {
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      return View(thisTag);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed (int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      _db.Tags.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      ItemTag  joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
      _db.ItemTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}