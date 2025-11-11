using System.Diagnostics;
using Dapper;
using Npgsql;
using Warehouse.Models;

namespace Warehouse.DAL
{
    public class DbContextDapper : ICrud<Product>
    {
        private const string ConnectionString =
            "Server=127.0.0.1;Port=5432;Database=warehouse_db;User Id=postgres;Password=1234;";

        private readonly NpgsqlConnection _connection;

        public DbContextDapper()
        {
            _connection = new NpgsqlConnection(ConnectionString);

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public IEnumerable<Product> GetAll()
        {
            _connection.Open();

            const string sql = "SELECT * FROM view_products";
            var products = _connection.Query<Product>(sql);

            _connection.Close();

            return products;
        }

        public bool InsertProduct(Product entity)
        {
            const string sql = """
                                INSERT INTO table_products (name, product_type_id,supplier_id,cost_price)
                                VALUES (@name,
                                        (SELECT id FROM table_product_types WHERE name = @productTypeName),
                                        (SELECT id FROM table_suppliers WHERE name = @supplierName ),
                                        @costPrice);
                                """;
            var result = Exec(sql, entity);
            if (!result)
            {
                // Проверим, существует ли тип товара
                var typeExists = _connection.ExecuteScalar<bool>(
                    "SELECT EXISTS(SELECT 1 FROM table_product_types WHERE name = @name)",
                    new { name = entity.ProductTypeName });

                if (!typeExists)
                    throw new InvalidOperationException(
                        $"Product type '{entity.ProductTypeName}' not found in table_product_types.");

                var supplierExists = _connection.ExecuteScalar<bool>(
                    "SELECT EXISTS(SELECT 1 FROM table_suppliers WHERE name = @name)",
                    new { name = entity.SupplierName });

                if (!supplierExists)
                    throw new InvalidOperationException(
                        $"Supplier '{entity.SupplierName}' not found in table_suppliers.");
            }

            return result;
        }

        public bool InsertType(string name)
        {
            const string sql = """
                               INSERT INTO table_product_types (name)
                               VALUES ( @name);
                               """;
            _connection.Open();
            var result = _connection.Execute(sql, new { name });
            _connection.Close();

            return result > 0;
        }

        public bool InsertSupplier(string name)
        {
            const string sql = """
                               INSERT INTO table_suppliers (name)
                               VALUES ( @name);
                               """;
            _connection.Open();
            var result = _connection.Execute(sql, new { name });
            _connection.Close();

            return result > 0;
        }

        public bool UpdateProduct(Product entity)
        {
            const string sql = """
                               UPDATE table_products
                               SET name = @name,
                                   product_type_id = (SELECT id FROM table_product_types WHERE name = @productTypeName),
                                   supplier_id = (SELECT id from table_suppliers WHERE name = @supplierName),
                                   cost_price = @costPrice
                               WHERE id = @id;
                               """;
            return Exec(sql, entity);
        }

        public bool DeleteProduct(Product entity)
        {
            const string sql = "DELETE FROM table_products WHERE id = @id;";
            return Exec(sql, entity);
        }

        private bool Exec(string sql, Product entity)
        {
            _connection.Open();
            var result = _connection.Execute(sql, entity);
            _connection.Close();

            return result > 0;
        }

        public IEnumerable<string> GetAllTypes()
        {
            _connection.Open();
            var types = _connection.Query<string>("SELECT name FROM table_product_types");
            _connection.Close();
            return types;
        }

        public IEnumerable<string> GetAllSuppliers()
        {
            _connection.Open();
            var suppliers = _connection.Query<string>("SELECT name FROM table_suppliers");
            _connection.Close();
            return suppliers;
        }
    }
}
