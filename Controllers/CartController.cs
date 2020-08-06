using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using challenge.Data;
using challenge.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace challenge.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<CartItem>>> Get([FromServices] DataContext context)
        {
            var cartItems = await context.CartItems.ToListAsync();
            return cartItems;
        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<ActionResult<CartItem>> GetById([FromServices] DataContext context, int id)
        {
            try
            {
                var cartItem = await context.CartItems
               .Include(db => db.Product)
               .AsNoTracking()
               .FirstOrDefaultAsync(prod => prod.ProductId == id);

                return cartItem;
            }
            catch (Exception)
            {
                return BadRequest("Item não encontrado");
            }
           
        }


        [HttpPost]
        [Route("")]

        public async Task<ActionResult<CartItem>> Post(
                    [FromServices] DataContext context,
                    [FromBody] CartItem model)
        {
            if (ModelState.IsValid)
            {

                if (context.Products
                            .AsNoTracking()
                            .FirstOrDefaultAsync(prod => prod.id == model.ProductId).Result.Quantity > model.Quantity)
                {
                    context.CartItems.Add(model);
                    await context.SaveChangesAsync();
                    return model;
                }
                else 
                {
                    return BadRequest("Quantidade indisponivel");
                }
                
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut]
        [Route("")]
        public async Task<ActionResult<CartItem>> Put([FromServices] DataContext context, [FromBody] CartItem model)
        {

            if (ModelState.IsValid)
            {
                context.CartItems.Update(model);
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
        public async Task<ActionResult<CartItem>> Delete([FromServices] DataContext context, int? id)
        {
            try
            {
                var product = await context.CartItems.FindAsync(id);
                context.CartItems.Remove(product);
                await context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                return BadRequest("Erro ao deletar produto");
            }

        }
    }
}
