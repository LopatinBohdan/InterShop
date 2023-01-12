namespace InterShop.Models
{
	public class OrderState
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Order> Orders { get;set; }
		public OrderState()
		{
			Orders = new List<Order>();
		}
	}
}
