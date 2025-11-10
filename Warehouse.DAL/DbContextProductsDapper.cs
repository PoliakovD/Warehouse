
using Dapper;
using Npgsql;
using Warehouse.Models;

namespace Warehouse.DAL
{
    public class DbContextProductsDapper : ICrud<Product>
    {
        private const string ConnectionString =
            "Server=127.0.0.1;Port=5432;Database=warehouse_db;User Id=postgres;Password=1234;";
        private readonly NpgsqlConnection _connection;

        public DbContextProductsDapper()
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

        public bool Insert(Product entity)
        {
            const string sql = """
                               INSERT INTO table_products (name, product_type_id,supplier_id,cost_price)
                               VALUES (@name, (SELECT id FROM table_product_types WHERE name = @type),
                                       (SELECT id from table_suppliers WHERE name = @supplier),@price);
                               """;
            return Exec(sql, entity);
        }

        public bool Update(Product entity)
        {
            const string sql = """
                               UPDATE table_products
                               SET name = @name,
                                   product_type_id = (SELECT id FROM table_product_types WHERE name = @type),
                                   supplier_id = (SELECT id from table_suppliers WHERE name = @supplier),
                                   cost_price = @price
                               WHERE id = @id;
                               """;
            return Exec(sql, entity);
        }

        public bool Delete(Product entity)
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
    }
}


