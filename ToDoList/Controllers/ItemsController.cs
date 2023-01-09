using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // to access UserManager
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // to use async methods
using System.Security.Claims; // to use claim based authorization

namespace ToDoList.Controllers
{
  // allows access only if user is logged in
  [Authorize]
  public class ItemsController : Controller
  {
    
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    // we need an instance of UserManager in order to access the tools that get us data about the signed-in user
    public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // 'Items' matches what's set in ToDoListContext.cs
      List<Item> model = _db.Items
                            .Where(entry => entry.User.Id == currentUser.Id)
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
        if (!ModelState.IsValid)
        {
          ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
          return View(item);
        }
        else
        {
          // Find current user
          string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
          // Associate currentUser with item's User property
          item.User = currentUser;
          // Update DbSet  
          _db.Items.Add(item);
          // Update DbContext 
          _db.SaveChanges();
          return RedirectToAction("Index");
        }
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