using Bogus.DataSets;
using Boilerplate.Data;
using Boilerplate.DTOs;
using Boilerplate.Helper;
using Boilerplate.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ProductController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("product-list")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> Get()
        {
            var productList = await _context.Products.Include(p => p.Category).ToListAsync();

            return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(productList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetById(Guid id)
        {

            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(ApiResponse<Product>.ErrorResponse(null, "Product Not Found"));
            }

            return Ok(ApiResponse<Product>.SuccessResponse(product, null));
        }

        [HttpPost("product-create")]
        public async Task<IActionResult> CreateProduct(CreateProductRequest request)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var categoryList = request.Category?.Select(c => c.Name).ToList() ?? new List<string>();
                var existingCategories = await _context.Categories.Where(c => categoryList.Contains(c.Name)).ToListAsync();
                var existingCategoryNames = existingCategories.Select(c => c.Name).ToHashSet();
                var newCategoryNames = categoryList.Where(name => !existingCategoryNames.Contains(name));

                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price

                };

                foreach (var category in existingCategories)
                {
                    product.CategoryId = category.Id;

                }

                foreach (var name in newCategoryNames)
                {
                    var newCategory = new Category { Name = name };
                    _context.Categories.Add(newCategory);

                    await _context.SaveChangesAsync();
                    product.CategoryId = newCategory.Id;

                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(ApiResponse<object>.SuccessResponse(new
                {
                    id = product.Id,
                    product.Name
                }));

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ApiResponse<string>.ErrorResponse(null, e.Message));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequest request)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {
                var findProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (findProduct == null)
                {
                    return NotFound(ApiResponse<Product>.ErrorResponse(null, "Product Not Found"));
                }

                findProduct.Name = request.Name;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(ApiResponse<Product>.SuccessResponse(findProduct, null));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ApiResponse<string>.ErrorResponse(null, e.Message));
            }
        }
    }
}