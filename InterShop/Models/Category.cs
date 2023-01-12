namespace InterShop.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public ICollection<Product> products { get; set; }
		public Category() 
		{
			products = new List<Product>();
		}
	}
}
