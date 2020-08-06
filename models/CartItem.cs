using System.ComponentModel.DataAnnotations;

namespace challenge.models
{
    public class CartItem
    {
        [Key]

        public int id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]

        public int ProductId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade minima de 1 produto")]

        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
