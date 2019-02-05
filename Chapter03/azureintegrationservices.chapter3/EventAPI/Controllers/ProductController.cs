using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Models;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductCatalogueContext _context;

        public ProductController(ProductCatalogueContext context)
        {
            _context = context;

            if (_context.ProductCatalogue.Count() == 0)
            {
                _context.ProductCatalogue.Add(new ProductCatalogue { Id= 001, Name = "Product1",productDescription="event product 1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProductCatalogue>>> GetProductCatalogue()
        {
            return await _context.ProductCatalogue.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCatalogue>> GetProductCatalogueItem(long id)
        {
            var todoItem = await _context.ProductCatalogue.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCatalogue>> DeleteProductCatalogueItem(long id)
        {
            var ProductCatalogueItem = await _context.ProductCatalogue.FindAsync(id);
            if (ProductCatalogueItem == null)
            {
                return NotFound();
            }

            _context.ProductCatalogue.Remove(ProductCatalogueItem);
            await _context.SaveChangesAsync();

            return ProductCatalogueItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCatalogueItem(long id, ProductCatalogue Item)
        {
            if (id != Item.Id)
            {
                return BadRequest();
            }

            _context.Entry(Item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }


}