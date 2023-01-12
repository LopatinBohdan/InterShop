namespace InterShop.Models
{
	public class ProductOrder
	{
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public virtual Product? GetProduct { get; set; }
		public int? OrderId { get; set; }
		public virtual Order? GetOrder{ get; set; }
		public int? Amount { get; set; }
		public decimal? TotalPrice { get; set; }

	}
}
