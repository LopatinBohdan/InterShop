using InterShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InterShop.Controllers
{
    public class CRUDController : Controller
    {
        InterShopContext db;
        List<User> users;
        List<Category> categories;
        List<Product> products;

        class Cart
        {
            public int Id { get; set; }
            public int Amount { get; set; }

        }
        public CRUDController(InterShopContext context)
        {
            this.db = context;
        }


        public IActionResult ToCart()
        {
            Dictionary<Product, int> pairs = new Dictionary<Product, int>();
            var cookie= Request.Cookies["Cart"];
            if (cookie != null)
            {
                List<Cart> carts = JsonSerializer.Deserialize<List<Cart>>(cookie);

                foreach (var item in carts)
                {
                    var product=db.products.Find(item.Id);
                    if (pairs.ContainsKey(product))
                    {
                        pairs[product] += item.Amount;
                    }
                    else
                    {
                        pairs.Add(product, item.Amount);
                    }
                }
                   
                return View(pairs);
            }
            else return NotFound();
        }


        //[Authorize(Roles="admin")]
        [HttpGet]
        public IActionResult UserAdd()
        {
            return View(new User());
        }

        [HttpGet]
        public IActionResult CategoryAdd()
        {
            return View(new Category());
        }
        [HttpGet]
        public IActionResult ProductAdd()
        {
            ViewBag.vb=db.categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Check(User user)
        {
            if (ModelState.IsValid)
            {
                User temp = user;
                db.users.Add(temp);
                await db.SaveChangesAsync();
                return Redirect("/CRUD/Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckCategory(Category category)
        {
            if (category!=null)
            {
                db.categories.Add(category);
                await db.SaveChangesAsync();
                return Redirect("/CRUD/Category");
            }
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> CheckProduct(Product product)
        {
            if (product != null)
            {
                db.products.Add(product);
                await db.SaveChangesAsync();

                var photos = Request.Form.Files;
                if (photos.Count > 0)
                {
                    for (int i = 0; i < photos.Count; i++)
                    {
                        Photo photo = new Photo();
                        photo.ProductId = db.products.Where(p=>p.Id==product.Id).FirstOrDefault().Id;

                        var gallery = $@"{Directory.GetCurrentDirectory()}\wwwroot\Photos\{product.Title.Replace(" ","_")}\Gallery";
                        Directory.CreateDirectory(gallery);
                        photo.Path = $@"{gallery}\{photos[i].FileName}";
                        using (FileStream fs=new FileStream(photo.Path, FileMode.Create))
                        {
                            photos[i].CopyTo(fs);
                        }
                        photo.Path = $@"{gallery}\{photos[i].FileName}".Split("wwwroot")[1];
                        db.photos.Add(photo);
                        db.SaveChanges();
                    }
                }
                return Redirect("/CRUD/Product");
            }
            return View();
        }


        //[Authorize(Roles="admin")]
        public IActionResult Index()
        {
            users = db.users.ToList();
            return View(users);
        }
        public IActionResult Category()
        {
            categories = db.categories.ToList();
            return View(categories);
        }
        public IActionResult Product()
        {
            products = db.products.ToList();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User? temp = await db.users.FirstOrDefaultAsync(u => u.Id == id);
                if (temp != null)
                {
                    db.users.Remove(temp);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id != null)
            {
                Product? temp = await db.products.FirstOrDefaultAsync(u => u.Id == id);
                if (temp != null)
                {
                    db.products.Remove(temp);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Product");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id != null)
            {
                Category? temp = await db.categories.FirstOrDefaultAsync(u => u.Id == id);
                if (temp != null)
                {
                    db.categories.Remove(temp);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Category");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> UserEdit(int id)
        {
            User temp = await db.users.FindAsync(id);
            if (temp != null)
            {
                return View(temp);
            }
            return View("Index");
        }
        public async Task<IActionResult> ProductEdit(int id)
        {
            Product temp = await db.products.FindAsync(id);
            if (temp != null)
            {
                ViewBag.vb = db.categories.ToList();
                return View(temp);
            }
            return View("Product");
        }
        public async Task<IActionResult> CategoryEdit(int id)
        {
            Category temp = db.categories.Find(id);
            if (temp != null)
            {
                return View(temp);
            }
            return View("Category");
        }

        [HttpGet]
        public async Task<IActionResult> UserInfo(int id)
        {
            User temp = await db.users.FindAsync(id);
            if (temp != null)
            {
                ViewData["Role"] = db.roles.Find(temp.RoleId).Name;
                return View(temp);
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ProductInfo(int id)
        {
            Product temp = await db.products.FindAsync(id);
            if (temp != null)
            {
                ViewBag.photos = db.photos.Where(p=>p.ProductId==id).ToList();
                return View(temp);
            }
            return View("ProductInfo");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (user != null)
            {
                db.Update(user);
                await db.SaveChangesAsync();
                return View("Index", db.users.ToList());
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(int id, Category category)
        {
            if (category != null)
            {
                db.Update(category);
                await db.SaveChangesAsync();
                return View("Category", db.categories.ToList());
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(int id, Product product)
        {
            if (product != null)
            {
               
                db.Update(product);
                await db.SaveChangesAsync();
                return View("Product", db.products.ToList());
            }
            return View(product);
        }
    }
}
