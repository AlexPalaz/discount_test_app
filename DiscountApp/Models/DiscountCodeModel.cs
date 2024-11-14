namespace DiscountApp.Models
{
    public class DiscountCode
    {
        public required string Code { get; set; }
        public bool Consumed { get; set; }
        public int Id { get; set; }
    };
}
