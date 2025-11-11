using Warehouse.DAL;
using Warehouse.Models;

namespace Warehouse.BLL
{
    public class Service
    {
        private readonly ICrud<Product> _crud;

        public Service()
        {
            _crud = new DbContextDapper();
        }

        public IEnumerable<Product> GetAll() => _crud.GetAll();

        public IEnumerable<Product> GetByName(string name)
        {
            var products = GetAll();
            return products.Where(product => product.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }
        public IEnumerable<string> GetTypesByName(string name)
        {
            var types = GetAllTypes();
            return types.Where(t => t.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<string> GetSuppliersByName(string name)
        {
            var suppliers = GetAllSuppliers();
            return suppliers.Where(t => t.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool Add(Product product) => _crud.InsertProduct(product);
        public bool Delete(Product product) => _crud.DeleteProduct(product);
        public bool Update(Product product) => _crud.UpdateProduct(product);
        public IEnumerable<string> GetAllSuppliers() => _crud.GetAllSuppliers();
        public IEnumerable<string> GetAllTypes() => _crud.GetAllTypes();
        public bool AddType(string type ) => _crud.InsertType(type);
        public bool AddSupplier(string supplier) => _crud.InsertSupplier(supplier);


    }

}

