using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Category_Management_System.Models;
using System.Linq.Expressions;

namespace Product_Category_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ProductCategoryContext _context;

        public CategoryController(ProductCategoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.OrderByDescending(x=>x.CreatedAt).ToListAsync();
        }

        [HttpGet]
        [Route("GetCategory")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(string name)
        {
            var res = await _context.Categories.Where(x=> name.ToLower().Contains(x.Name.ToLower())).ToListAsync();
            return res;

        }

        [HttpPost]
        [Route("AddCategory")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    var Res = await _context.Categories
                        .Where(x => category.Name.Contains(x.Name))
                        .FirstOrDefaultAsync();

                    if (Res == null)
                    {
                        category.CreatedAt = DateTime.Now;
                        category.UpdatedAt = DateTime.Now;
                        _context.Categories.Add(category);
                        await _context.SaveChangesAsync();
                        //return CreatedAtAction("GetProduct", new { id = category.CategoryId }, category);
                        return StatusCode(200, "Add Successfully.");
                    }
                    else
                    {
                        return Ok(category);
                    }
                }
                else
                {
                    return BadRequest("Category data is missing.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }

        }

        [HttpGet]
        [Route("EditCategory")]
        [AllowAnonymous]
        public async Task<ActionResult> EditCategory(Guid id, string category)
        {
            if (category != null)
            {
                var Result = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                if (Result != null && Result.Name != category)
                {
                    Result.Name = category;
                    Result.UpdatedAt = DateTime.Now;
                    _context.Entry(Result).State = EntityState.Modified;
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Ok("Updated Successfully");
                    }
                }
                return Ok("Not Updated.");
            }
            else
            {
                return BadRequest("Data not found.");
            }
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteCategory(Guid CategoryId)
        {
            if (!string.IsNullOrEmpty(CategoryId.ToString()))
            {
                var Result = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == CategoryId);
                if (Result != null)
                {
                    _context.Remove(Result);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Ok("Deleted Successfully.");
                    }
                }
                return Ok("Not Deleted.");
            }
            else
            {
                return BadRequest("Id can not be zero.");
            }
        }
    }

}
