namespace Boilerplate.DTOs
{
    public class CreateProductRequest{
        public string Name {set; get;}
        public decimal Price{set; get;}
        public List<CreateCategoryRequest>? Category {set; get;}
    }
}