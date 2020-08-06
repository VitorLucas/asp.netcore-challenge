using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Data;
using challenge.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace challenge.Controllers
{
    [Route("api/products")]
    [ApiController]

    public class ProductController : Controller
    {
        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.Include(product => product.Category).ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                .Include(product => product.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(prod => prod.id == id);

            return products;
        }


        [HttpGet]
        [Route("categories/{id:int}")]

        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                .Include(product => product.Category)
                .AsNoTracking()
                .Where(prod => prod.CategoryId == id)
                .ToListAsync();

            return products;
        }

        [HttpPost]
        [Route("")]

        public async Task<ActionResult<Product>> Post(
                    [FromServices] DataContext context,
                    [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Product>> Put( [FromServices] DataContext context, [FromBody] Product model)
        {

            if (ModelState.IsValid)
            {
                context.Products.Update(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Delete( [FromServices] DataContext context, int? id)
        {
            try
            {
                var employee = await context.Products.FindAsync(id);
                context.Products.Remove(employee);
                await context.SaveChangesAsync();
                return employee;
            }
            catch (Exception)
            {

                return BadRequest("Erro ao deletar item");
            }
           
        }
    }
}
