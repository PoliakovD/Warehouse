namespace Warehouse.DAL;

public interface ICrud<T>
{
    public IEnumerable<T> GetAll();
    public bool InsertProduct(T entity);
    public bool InsertType(string type);
    public bool InsertSupplier(string supplier);
    public bool UpdateProduct(T entity);
    public bool DeleteProduct(T entity);
    public IEnumerable<string> GetAllTypes();
    public IEnumerable<string> GetAllSuppliers();

}
