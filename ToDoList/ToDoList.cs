using System;
using ToDoList.Models;

namespace ToDoList
{
  public class Program
  {
    public static void Main()
    {
      // Welcome
      Console.WriteLine("Welcome to the To Do List. We're happy you're here.");
      // Ask to add or view

      while (true)
      {
        Console.WriteLine("Would you like to add an item to your list or view your list? (Add/View)");
        string userNavInput = Console.ReadLine();
        userNavInput = userNavInput.ToUpper();
        if (userNavInput == "ADD")
        {
          // Prompt user input description
          Console.WriteLine("Please enter the description for the new item.");
          string description = Console.ReadLine();
          // Add item to list
          Item newItem = new Item(description);
          // Confirm item has been added
          Console.WriteLine($"'{newItem.Description}' has been added to your list.");
          // Reprompt add / view
        }
        else if (userNavInput == "VIEW")
        {
          // If no items in list
          if ( Item.GetAll().Count == 0)
          {
            Console.WriteLine("You have no items on your ToDo List! Congrats.");
          }
          else 
          {
            foreach (Item item in Item.GetAll())
            {
              Console.WriteLine(item.ItemToString());
            }
          }
            // Print To Do List to Console
        }
        else
        {
          //Reprompt add / view
        }
        // Reprompt add / view
        
      }
    }
  }
}