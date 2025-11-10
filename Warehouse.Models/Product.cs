namespace Warehouse.Models;

public record Product
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string ProductTypeName { get; init; }
    public string SupplierName { get; init; }
    public decimal CostPrice { get; init; }

    public Product(int id, string name, string productTypeName, string supplierName, decimal costPrice)
    {
        this.Id = id;
        this.Name = name;
        this.ProductTypeName = productTypeName;
        this.SupplierName = supplierName;
        this.CostPrice = costPrice;
    }

    public Product(string name, string productTypeName, string supplierName, decimal costPrice)
    {
        this.Name = name;
        this.ProductTypeName = productTypeName;
        this.SupplierName = supplierName;
        this.CostPrice = costPrice;
    }

}


