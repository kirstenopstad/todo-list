using System;
using System.Collections.Generic;
// using MySqlConnector;

namespace ToDoList.Models
{

  public class Item
  {
    // Properties
    public string Description { get; set; }
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public bool Status { get; set; }
    public DateTime DueBy { get; set; }
    //  reference navigation property, because it holds a reference to a single related entity
    public Category Category { get; set; }
    // collection navigation property
    public List<ItemTag> JoinEntities { get; }
  }
}
