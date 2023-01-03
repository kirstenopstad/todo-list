namespace ToDoList.Models
{
  public class ItemTag
  {
    public int ItemTagId { get; set; }
    public int ItemId { get; set; }
    // reference navigation property
    public Item Item { get; set; }
    public int TagId { get; set; }
    // reference navigation property
    public Tag Tag { get; set; }
  }
}