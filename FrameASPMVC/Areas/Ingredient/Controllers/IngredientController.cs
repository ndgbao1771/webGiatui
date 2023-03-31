using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.AppContext;
using System.Drawing.Printing;
using App.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace doan4.Areas.Ingredients.Controllers
{
    [Area("Ingredient")]
    [Route("admin/ingredient/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "page")]
        public int currentPage { get; set; }
        public int countPage { get; set; }


        public string StatusMessage { get; set; }

        public IngredientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ingredients/Ingredients
        public async Task<IActionResult> Index([FromQuery(Name = "page")] int curentPage, int pagesize)
        {
           var appDbContext =  _context.materials.Include(i => i.brands)
                                                  .Include(i => i.cateIngredients)
                                                  .Include(u => u.unitIngredientAmount)
                                                  .Include(u => u.unitIngredientWeight)
                                                 .ToList();
            int totalIngredient = appDbContext.Count();
            if (pagesize <= 0) pagesize = 8;
            int countPagesS = (int)Math.Ceiling((double)totalIngredient / pagesize);

            if (currentPage > countPagesS) currentPage = countPagesS;
            if (currentPage < 1) currentPage = 1;
            var pagingModelS = new PagingModel()
            {
                countpages = countPagesS,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    page = pageNumber,
                    pagesize = pagesize
                })
            };
            ViewBag.pagingModel = pagingModelS;
            ViewBag.totalorder = totalIngredient;

            var ingredientInPageS = appDbContext.Skip((currentPage - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            return View(ingredientInPageS);
        }

        // GET: Ingredients/Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.materials == null)
            {
                return NotFound();
            }

            var ingredient = await _context.materials
                .Include(i => i.brands)
                .Include(i => i.cateIngredients)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Ingredients/Create
        public IActionResult Create()
        {
            ViewBag.UnitIngredientWeightID = new SelectList(_context.unitIngredients, "Id", "Unit");
            ViewBag.UnitIngredientAmountID = new SelectList(_context.unitIngredients, "Id", "Unit");
            ViewBag.BrandId = new SelectList(_context.brands, "Id", "BrandName");
            ViewBag.CateIngredientId = new SelectList(_context.cateIngredients, "Id", "CategoryName");
            return View();
        }

        // POST: Ingredients/Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([Bind("Id,Name,IngredientPrice,Weight,Amount,Quantity,UnitIngredientWeightID,UnitIngredientAmountID,brandID,CategoryIngredientID")] Material ingredient)
        {
            if (ModelState.IsValid)
            {
                ingredient.TotalWeight = ingredient.Quantity * ingredient.Weight;
                ingredient.CurentWeight = ingredient.TotalWeight; 
                ingredient.ImportDate = DateTime.Now;
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitIngredientWeightID"] = new SelectList(_context.unitIngredients, "Id", "Unit", ingredient.UnitIngredientWeightID);
            ViewData["UnitIngredientAmountID"] = new SelectList(_context.unitIngredients, "Id", "Unit", ingredient.UnitIngredientAmountID);
            ViewData["brandID"] = new SelectList(_context.brands, "Id", "BrandName", ingredient.brandID);
            ViewData["CategoryIngredientID"] = new SelectList(_context.cateIngredients, "Id", "CategoryName", ingredient.CategoryIngredientID);
            return View(ingredient);
        }

        // GET: Ingredients/Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.materials == null)
            {
                return NotFound();
            }

            var ingredient = await _context.materials.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ViewData["brandID"] = new SelectList(_context.brands, "Id", "BrandName", ingredient.brandID);
            ViewData["CategoryIngredientID"] = new SelectList(_context.cateIngredients, "Id", "CategoryName", ingredient.CategoryIngredientID);
            return View(ingredient);
        }

        // POST: Ingredients/Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IngredientPrice,TotalWeight,Amount,brandID,CategoryIngredientID")] Material ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
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
            ViewData["brandID"] = new SelectList(_context.brands, "Id", "BrandName", ingredient.brandID);
            ViewData["CategoryIngredientID"] = new SelectList(_context.cateIngredients, "Id", "CategoryName", ingredient.CategoryIngredientID);
            return View(ingredient);
        }

        // GET: Ingredients/Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.materials == null)
            {
                return NotFound();
            }

            var ingredient = await _context.materials
                .Include(i => i.brands)
                .Include(i => i.cateIngredients)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.materials == null)
            {
                return Problem("Entity set 'AppDbContext.materials'  is null.");
            }
            var ingredient = await _context.materials.FindAsync(id);
            if (ingredient != null)
            {
                _context.materials.Remove(ingredient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
          return (_context.materials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
