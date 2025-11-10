namespace Warehouse.Models;

public record Delivery
{
    public int Id { get; set; }
    public string Supplier { get; set; }
    public DateTime DateTime { get; set; }
}
