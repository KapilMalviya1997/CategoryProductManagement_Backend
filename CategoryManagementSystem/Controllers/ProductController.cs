using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Category_Management_System.Models;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Product_Category_Management_System.DataContracts.GetAllProducts;

namespace Product_Category_Management_System.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductCategoryContext _context;

        public ProductController(ProductCategoryContext context)
        {
            _context = context;
        }

        [Route("GetAllProducts")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult>> GetProducts(int pageNumber, int pageSize)
        {
            if (pageNumber > 0 && pageSize > 0)
            {
                var res = _context.Products.AsQueryable();
                var Result = await res.OrderBy(x => x.CreatedAt).Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToListAsync();
                return new PagedResult
                {
                    TotalRecords = res.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Items = Result
                };
            }
            else
            {
                return BadRequest("Data not found.");
            }

        }
        [Route("GetProduct")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetProductByFilter(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var Result = await _context.Products.Where(x => (filter == x.ProductId.ToString() || filter.Contains(x.Name) || filter.Contains(x.Description))).FirstOrDefaultAsync();
                if (Result != null)
                {
                    return Ok(Result);
                }
                else
                {
                    return BadRequest("Data not found.");
                }
            }
            else
            {
                return BadRequest("Value can not be Null/Empty.");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddProduct")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product != null)
            {
                if (product.Price > 0 && product.Quantity > 0)
                {
                    var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

                    product.CreatedAt = DateTime.Now;
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
                }
                else
                {
                    return BadRequest("Value can not be zero.");
                }
            }
            else
            {
                return BadRequest("Data not found.");
            }
        }


        [HttpPut]
        [Route("EditProduct")]
        [AllowAnonymous]
        public async Task<ActionResult> EditProduct(Product product)
        {
            if (product != null)
            {
                var Result = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
                if (Result != null && (Result.Name != product.Name || Result.Price != product.Price || Result.Quantity != product.Quantity || Result.CategoryId != product.CategoryId))
                {
                    Result.Name = product.Name;
                    Result.Price = product.Price;
                    Result.Quantity = product.Quantity;
                    Result.CategoryId = product.CategoryId;
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
        [Route("DeleteProduct")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteProduct(Guid productId)
        {
            if (!string.IsNullOrEmpty(productId.ToString()))
            {
                var Result = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
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
