namespace PosApp.Domain
{
    public class ReceiptItem
    {
        public ReceiptItem(Product product, int amount, decimal subPromoted)
        {
            Product = product;
            Amount = amount;
            SubPromoted = subPromoted;
            Total = product.Price * amount;
        }

        public Product Product { get; }
        public int Amount { get; }
        public decimal Total { get; }
        public decimal SubPromoted { get; set; }
    }
}