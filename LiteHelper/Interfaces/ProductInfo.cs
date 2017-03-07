using System;
namespace LiteHelper.Interfaces
{
	public class ProductInfo
	{
		public string Id { get; set; }
		public string PriceString { get; set; }
		public decimal Price { get; set; }
		public string DisplayName { get; set; }
	}
}
