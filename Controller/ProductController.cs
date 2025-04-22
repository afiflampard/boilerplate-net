using Boilerplate.Data;
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
    public class ProductController: ControllerBase {
        private readonly AppDBContext _context;

        [HttpGet("product-list")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> Get(){
            var productList = await _context.Products.Include(p => p.Category).ToListAsync();

            return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(productList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetById(Guid id){

            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if(product == null){
                return NotFound(ApiResponse<Product>.ErrorResponse(null,"Product Not Found"));
            }

            return Ok(ApiResponse<Product>.SuccessResponse(product,null));
        }
    }
}