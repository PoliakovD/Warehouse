namespace Warehouse.Models;

public record DeliveryItem
{
    public int Id { get; set; }
    public string Delivery { get; set; }
    public string Product { get; set; }
}
