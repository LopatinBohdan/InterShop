using Microsoft.AspNetCore.Server.IIS.Core;

namespace InterShop.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? Login { get; set; }
		public string? Password { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public int? RoleId { get; set; }
		public virtual Role? UserRole{ get; set;}
		public ICollection<Order>? orders { get; set;}

		public User(string login, string password, string email, string phone, int roleId)
		{
			this.Login = login;
			this.Password = password;
			this.Email = email;
			this.Phone = phone;
			this.RoleId = roleId;
		}
        
        public User() { }
	}
}
