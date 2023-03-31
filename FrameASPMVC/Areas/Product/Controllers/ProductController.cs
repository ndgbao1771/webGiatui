using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Product;
using Microsoft.AspNetCore.Authorization;
using App.Data;
using App.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using App.AppContext;

namespace doan4.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/product/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.products.Include(p => p.Author);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs,Price")] CreateProduct products)
        {
            var categories = await _context.categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if(await _context.products.AnyAsync(p => p.Slug == products.Slug))
            {
                ModelState.AddModelError("Slug", "Chuỗi url bị trùng lặp");
                return View(products);
            }

            if(products.Slug == null)
            {
                products.Slug = AppUtilities.GenerateSlug(products.Title);
            }


            if (ModelState.IsValid)
            {
                var userCreate = await _userManager.GetUserAsync(this.User);

                products.DateCreated = products.DateUpdated = DateTime.Now;
                products.AuthorId = userCreate.Id;

                _context.Add(products);

                if (products.CategoryIDs != null)
                {
                    foreach (var CateId in products.CategoryIDs)
                    {
                        _context.Add(new ProductCategory()
                        {
                            CategoryId = CateId,
                            Product = products
                        });
                    }
                }

                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo sản phẩm mới";
                return RedirectToAction(nameof(Index));
            }
            
            return View(products);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            var productEdit = new CreateProduct (){
                Title = products.Title,
                Description = products.Description,
                Slug = products.Slug,
                Content = products.Content,
                Published = products.Published,
                Price = products.Price,
                CategoryIDs = products.ProductCategories.Select(pCate => pCate.CategoryId).ToArray()
            };

            var categories = await _context.categories.ToListAsync();
            ViewData["categories"] =  new MultiSelectList(categories, "Id", "Title");
            return View(productEdit);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,Content,Published,CategoryIDs,Price")] CreateProduct products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            var categories = await _context.categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if(await _context.products.AnyAsync(p => p.Slug == products.Slug  && p.Id != id))
            {
                ModelState.AddModelError("Slug", "Chuỗi url bị trùng lặp");
                return View(products);
            }

            if(products.Slug == null)
            {
                products.Slug = AppUtilities.GenerateSlug(products.Title);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var userCreate = await _userManager.GetUserAsync(this.User);

                    var productUpdate = await _context.products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p=> p.Id == id);
                    if(productUpdate == null)
                    {
                        return NotFound();
                    }

                    productUpdate.Title = products.Title;
                    productUpdate.Description = products.Description;
                    productUpdate.Slug = products.Slug;
                    productUpdate.Content = products.Content;
                    productUpdate.Published = products.Published;
                    productUpdate.Price = products.Price;
                    productUpdate.DateUpdated = DateTime.Now;
                    productUpdate.AuthorId = userCreate.Id;

                    //update ProductCategory
                    if (products.CategoryIDs == null) products.CategoryIDs = new int[] {};

                    var oldCateId = productUpdate.ProductCategories.Select(c => c.CategoryId).ToArray();
                    var newCateId = products.CategoryIDs;

                    var removeCateId = from pCate in productUpdate.ProductCategories
                                        where (!newCateId.Contains(pCate.CategoryId))
                                        select pCate;
                    _context.productCategories.RemoveRange(removeCateId);
                    var addCateId = from pCate in newCateId
                                    where !oldCateId.Contains(pCate)
                                    select pCate;

                    foreach(var cateId in addCateId)
                    {
                        _context.productCategories.Add(new ProductCategory(){
                            ProductId = id,
                            CategoryId = cateId
                        });
                    }

                    _context.Update(productUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Cập nhật sản phẩm thành công";
            return View(products);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.products.FindAsync(id);
            _context.products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.products.Any(e => e.Id == id);
        }


        public class UploadOneFIle 
        {
            // public IFromFile FileUpLoad {get; set;}
        }

        [HttpGet]
        public IActionResult UploadPhoto (int id) 
        {
            var product = _context.products.Where(p => p.Id == id).FirstOrDefault();
            if(product == null)
            {
                return NotFound();
            }
            ViewData["product"] = product;
            return View();
        }
    }
}
