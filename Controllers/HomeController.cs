using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChefsNDishes.Models;
using Microsoft.EntityFrameworkCore;

namespace ChefsNDishes.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("/")]
        public IActionResult Chefs()
        {
            List<Chef> AllChefs = dbContext.Chefs
                                    .Include(c => c.CreatedDishes)
                                    .OrderByDescending(c => c.ChefRating)
                                    .ToList();
                                    return View(AllChefs);
        }

        [HttpGet("dishes")]
        public IActionResult Dishes()
        {
            List<Dish> AllDishes = dbContext.Dishes
                                    .Include(c => c.Creator)
                                    .OrderByDescending(d => d.CreatedAt)
                                    .ToList();
                                    return View(AllDishes);
        }

        [HttpGet("dish/add")]
        public IActionResult DishAdd()
        {
            ViewBag.AllChefs = dbContext.Chefs
                                    .OrderBy(c => c.FirstName)
                                    .ToList();
                                    return View();
        }

        [HttpPost("dish/process")]
        public IActionResult DishProcess(Dish newDish)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Dishes");
            }
            else
            {
                return View("DishAdd");
            }
            
        }

        [HttpGet("dish/{DishId}")]
        public IActionResult DishShow(int DishId)
        {
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == DishId);
            ViewBag.Chef = dbContext.Chefs.FirstOrDefault(c => c.ChefId == dish.ChefId);
            return View(dish);
        }

        [HttpGet("dish/{DishId}/edit")]
        public IActionResult DishEdit(int DishId)
        {
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == DishId);
            ViewBag.AllChefs = dbContext.Chefs
                                    .OrderBy(c => c.FirstName)
                                    .ToList();
                                    return View(dish);
        }

        [HttpPost("dish/{DishId}/update")]
        public IActionResult DishUpdate(int DishId, Dish dUpdated)
        {
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == DishId);
            dish.ChefId = dUpdated.ChefId;
            ViewBag.Chef = dbContext.Chefs.FirstOrDefault(c => c.ChefId == dish.ChefId);
            dish.Name = dUpdated.Name;
            dish.Tastiness = dUpdated.Tastiness;
            dish.Calories = dUpdated.Calories;
            dish.Description = dUpdated.Description;
            dish.DataImageUrlField = dUpdated.DataImageUrlField;
            dish.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges();
            return View("DishShow", dish);
        }

        [HttpGet("dish/{DishId}/delete")]
        public IActionResult DishDelete(int DishId)
        {
            Dish dish = dbContext.Dishes.FirstOrDefault(c => c.DishId == DishId);
            dbContext.Dishes.Remove(dish);
            dbContext.SaveChanges();
            return Redirect("/dishes");
        }

        [HttpGet("chef/add")]
        public IActionResult ChefAdd()
        {
            return View();
        }

        [HttpPost("chef/process")]
        public IActionResult ChefProcess(Chef newChef)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newChef);
                dbContext.SaveChanges();
                return RedirectToAction("Chefs");
            }
            else
            {
                return View("ChefAdd");
            }
            
        }

        [HttpGet("chef/{ChefId}")]
        public IActionResult ChefShow(int ChefId)
        {
            Chef chef = dbContext.Chefs.FirstOrDefault(c => c.ChefId == ChefId);
            return View(chef);
        }

        [HttpGet("chef/{ChefId}/edit")]
        public IActionResult ChefEdit(int ChefId)
        {
            Chef chef = dbContext.Chefs.FirstOrDefault(c => c.ChefId == ChefId);
            return View(chef);
        }

        [HttpPost("chef/{ChefId}/update")]
        public IActionResult ChefUpdate(int ChefId, Chef dUpdated)
        {
            Chef chef = dbContext.Chefs.FirstOrDefault(d => d.ChefId == ChefId);
            chef.FirstName = dUpdated.FirstName;
            chef.LastName = dUpdated.LastName;
            chef.DOB = dUpdated.DOB;
            chef.ChefRating = dUpdated.ChefRating;
            chef.Description = dUpdated.Description;
            chef.DataImageUrlField = dUpdated.DataImageUrlField;
            chef.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges();
            return View("ChefShow", chef);
        }

        [HttpGet("chef/{ChefId}/delete")]
        public IActionResult ChefDelete(int ChefId)
        {
            Chef chef = dbContext.Chefs.FirstOrDefault(c => c.ChefId == ChefId);
            dbContext.Chefs.Remove(chef);
            dbContext.SaveChanges();
            return Redirect("/chefs");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
