namespace Boilerplate.Model
{
    public class Product{
        public Guid Id {get; set;} = Guid.NewGuid();
        public required string Name {get; set;}

        public Guid? CategoryId{get; set;}

        public Category? Category{get; set;}
        public decimal Price {get; set;}
    }
}