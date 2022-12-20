using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTests : IDisposable
  {

    // we've added a new property
      public IConfiguration Configuration { get; set; }
    
    public void Dispose()
    {
      // Teardown after each test
      Item.ClearAll();
    }

    // we've added a constructor
    public ItemTests()
    {
      // load app config manually
      IConfigurationBuilder builder = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
      // Connects to our test db instead of the develoment one
      DBConfiguration.ConnectionString = Configuration["ConnectionStrings:TestConnection"];
    }

    [TestMethod]
    public void ItemConstructor_CreatesInstanceOfItem_Item()
    {
      Item newItem = new Item("test");
      Assert.AreEqual(typeof(Item), newItem.GetType());
    }

    [TestMethod]
    public void GetDescription_ReturnsDescription_String()
    {
      string description = "Walk the dog.";
      Item newItem = new Item(description);
      string result = newItem.Description;
      Assert.AreEqual(description, result);
    }

    [TestMethod]
    public void SetDescription_SetDescription_String()
    {
      //Arrange
      string description = "Walk the dog.";
      Item newItem = new Item(description);
      //Act
      string updatedDescription = "Do the dishes";
      newItem.Description = updatedDescription;
      string result = newItem.Description;
      //Assert
      Assert.AreEqual(updatedDescription, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ItemList()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");

      //Act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyListFromDB_ItemList()
    {
      //Arrange
      List<Item> newList = new List<Item> { };
      //Act
      List<Item> result = Item.GetAll();

      // foreach (Item thisItem in result)
      // {
      //   Console.WriteLine("Output from empty list GetAll test: " + thisItem.Description);
      // }
      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsItems_ItemList()
    {
      //Arrange
      string description01 = "Walk the dog";
      string description02 = "Wash the dishes";
      Item newItem1 = new Item(description01);
      newItem1.Save(); 
      Item newItem2 = new Item(description02);
      List<Item> newList = new List<Item> { newItem1, newItem2};
      newItem2.Save(); 
      //Act
      List<Item> result = Item.GetAll();
      // foreach (Item thisItem in result)
      // {
      //   Console.WriteLine("Output from second GetAll test: " + thisItem.Description);
      // }
      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    // [TestMethod]
    // public void GetListNumber_ReturnsItemListNumber_ItemListNumber()
    // {
    //   // Arrange
    //   string description01 = "Walk the dog";
    //   Item newItem1 = new Item(description01);
    //   //Act
    //   int item1 = newItem1.ListNumber;
    //   //Assert
    //   Assert.AreEqual(item1, 1);
    // }

    // [TestMethod]
    // public void ItemToString_ReturnsItemString_String()
    // {
    //   string description01 = "Walk the dog";
    //   Item newItem1 = new Item(description01);
    //   Assert.AreEqual("1. Walk the dog", newItem1.ItemToString());
    // }

    // [TestMethod]
    // public void ItemToString_ReturnsListNumberString_String()
    // {
    //   //Arrange
    //   string description01 = "Walk the dog";
    //   string description02 = "Wash the dishes";
    //   //Act
    //   Item newItem1 = new Item(description01);
    //   Item newItem2 = new Item(description02);
    //   //Assert
    //   Assert.AreEqual("2. Wash the dishes", newItem2.ItemToString());
    // }

    // [TestMethod]
    // public void GetId_ItemsInstantiateWithAnIdAndGetterReturns_Int()
    // {
    //   //Arrange
    //   string description = "Walk the dog.";
    //   Item newItem = new Item(description);

    //   //Act
    //   int result = newItem.Id;

    //   //Assert
    //   Assert.AreEqual(1, result);
    // }
    
    [TestMethod]
    public void Find_ReturnsCorrectItemFromDatabase_Item()
    {
      //Arrange
      Item newItem = new Item("Mow the lawn");
      newItem.Save();
      Item newItem2 = new Item("Wash dishes");
      newItem2.Save();

      //Act
      Item foundItem = Item.Find(newItem.Id);
      //Assert
      Assert.AreEqual(newItem, foundItem);
    }
  }
}