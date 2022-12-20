using System.Collections.Generic;
using MySqlConnector;

namespace ToDoList.Models
{

  public class Item
  {
    // Properties
    public string Description { get; set; }
    public int Id { get; set; }

    // Constructor
    public Item(string description)
    {
      Description = description;
    }
    // Overloaded
    public Item(string description, int id)
    {
      Description = description;
      Id = id;
    }

    // Note that best practice dictates that 
    // this method be below the properties and constructors 
    // but above the other methods in our file:

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        // type cast System.Object into Item object
        Item newItem = (Item) otherItem;
        // if id &descriptions are the same, items are the same
        bool idEquality = (this.Id == newItem.Id);
        bool descriptionEquality = (this.Description == newItem.Description);
        return descriptionEquality;
      }
    }

    // Since their *actual* hashcodes would be different
    // We generate new ones that will match, because items *should* have same id 
    // And .GetHashCode() with return same int given same input
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      // Write query with parameter placeholder
      cmd.CommandText = "INSERT INTO items (description) VALUES (@ItemDescription);";
      // instantiate parameter
      MySqlParameter param = new MySqlParameter();
      // name it to connect with placeholder in query
      param.ParameterName = "@ItemDescription";
      // assign value
      param.Value = this.Description;
      // add parameter to command
      cmd.Parameters.Add(param);    
      // execute
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;

      // End new code

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> { };
      
      // Open db connection
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
      conn.Open();
      // Construct SQL query // using 'as' casts rdr to MySql-compatible command
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "SELECT * FROM items;";
      // Read the query result // using 'as' casts rdr to MySql-compatible command
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      // .Read() returns false when it gets to end of records in db
      while (rdr.Read())
      {
          // extract and record data in obj our app understands
          // the number argument represents the col # starting at 0
          int itemId = rdr.GetInt32(0);
          string itemDescription = rdr.GetString(1);
          Item newItem = new Item(itemDescription, itemId);
          allItems.Add(newItem);
      }
      // Close the connection
      conn.Close();
      // make sure it closed
      if (conn != null)
      {
          conn.Dispose();
      }
      return allItems;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "DELETE FROM items;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int id)
    {
      // We open a connection.
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "SELECT * FROM items WHERE id = @ThisId;";

      MySqlParameter param = new MySqlParameter();
      param.ParameterName = "@ThisId";
      param.Value = id;
      cmd.Parameters.Add(param);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      // deafult values ensure that if record doesn't exist, app doesn't break
      int itemId = 0;
      string itemDescription = "";
      while (rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemDescription = rdr.GetString(1);
      }
      Item foundItem = new Item(itemDescription, itemId);

      // We close the connection.
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundItem;
    }

  }
}
