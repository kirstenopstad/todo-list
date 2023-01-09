using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace ToDoList.Models
{

  public class Item
  {
    // Properties
    public int ItemId { get; set; }
    [Required(ErrorMessage = "The item's description can't be empty!")]
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "You must add your item to a category. Have you created a category yet?")] // <-- data annotation / validation attribute
    public int CategoryId { get; set; }
    public bool Status { get; set; }
    public DateTime DueBy { get; set; }
    //  reference navigation property, because it holds a reference to a single related entity
    public Category Category { get; set; }
    // collection navigation property
    public List<ItemTag> JoinEntities { get; }
    public ApplicationUser User { get; set; }
  }
}
