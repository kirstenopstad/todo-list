using System.Collections.Generic;

namespace ToDoList.Models
{

  public class Item
  {
    public string Description { get; set; }
    public int ListNumber { get; set; }
    private static List<Item> _instances = new List<Item>{};

    public Item(string description)
    {
      Description = description;
      _instances.Add(this);
      ListNumber = _instances.Count;
    }

    public static List<Item> GetAll()
    {
      return _instances;
    }

    public static void ClearAll()
    {
      _instances.Clear();
    }

    public string ToString() 
    {
      return $"{this.ListNumber}. {this.Description}";
    }
  }
}
