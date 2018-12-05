using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {

    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }
    
    [HttpGet("/categories/new")]
    public ActionResult New()
    {
      return View();
    }
    
    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
      Category newCategory = new Category(categoryName);
      newCategory.Save();

      List<Category> allCategories = Category.GetAll();
      return View("Index", allCategories);
    }
    
    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Item> categoryItems = selectedCategory.GetItems();
      model.Add("category", selectedCategory);
      model.Add("items", categoryItems);
      return View(model);
    }

    [HttpGet("/categories/{id}/delete")]
     public ActionResult Delete(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category category = Category.Find(id);
      List<Item> categoryItems = category.GetItems();
      
      foreach(Item item in categoryItems)
      {
        Item.DeleteItem(item.GetId());
      }

      Category.DeleteCategory(id);
     
      model.Add("category", category);
      model.Add("items", categoryItems);
      return View("Delete", model);
   
    }
    
    
    //This one creates new Items within a given Category, not new Categories:
    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemDescription)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription, categoryId);
      newItem.Save();
      List<Item> categoryItems = foundCategory.GetItems();
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

   [HttpGet("/categories/{categoryId}/items/{itemId}/delete")]
    public ActionResult Delete(int categoryId, int itemId)
    {
      Item item = Item.Find(itemId);
      Item.DeleteItem(item.GetId());
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category category = Category.Find(categoryId);
      List<Item> categoryItems = category.GetItems();
      model.Add("category", category);
      model.Add("items", categoryItems);
      return View("Show", model);
   
    }


  }
}
