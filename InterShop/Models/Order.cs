namespace InterShop.Models
{
	public class Order
	{
		public int Id { get; set; }
		public int? UserId { get; set; }
		public virtual User? GetUser { get; set; }
		public ICollection<ProductOrder> ProductOrders { get; set; }
		public int OrderStateId { get; set; }
		public virtual OrderState? State { get; set; }  
		DateTime? Created { get; set; }
		DateTime? Updated { get; set; }

		public Order() 
		{
			ProductOrders= new List<ProductOrder>();
		}
	}
}
