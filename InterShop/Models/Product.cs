namespace InterShop.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public int Amount { get; set; }
		public int? CategoryId { get; set; }
		public virtual Category? Category { get; set; } 
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public ICollection<Photo>? photos { get; set; }
	}
}
