namespace InterShop.Models
{
	public class Photo
	{
		public int Id { get; set; }
		public string? Path { get; set; }
		public int? ProductId { get; set; }
		public virtual Product? GetProduct { get; set; }
	}
}
