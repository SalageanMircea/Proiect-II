using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlMenuRepository : IMenuRepository
    {
        public List<MenuProduct> GetAllProducts()
        {
            List<MenuProduct> products = new List<MenuProduct>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT MenuId, Name, Category, Price, IsAvailable
                FROM MenuItems
                ORDER BY MenuId DESC";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MenuProduct product = new MenuProduct();

                product.MenuId = Convert.ToInt32(reader["MenuId"]);
                product.Name = reader["Name"].ToString() ?? "";
                product.Category = reader["Category"].ToString() ?? "";
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.IsAvailable = Convert.ToBoolean(reader["IsAvailable"]);

                products.Add(product);
            }

            return products;
        }

        public void AddProduct(MenuProduct product)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                INSERT INTO MenuItems
                (Name, Category, Price, IsAvailable)
                VALUES
                (@name, @category, @price, @available)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@category", product.Category);
            command.Parameters.AddWithValue("@price", product.Price);
            command.Parameters.AddWithValue("@available", product.IsAvailable);

            command.ExecuteNonQuery();
        }

        public void UpdateProduct(MenuProduct product)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE MenuItems
                SET Name = @name,
                    Category = @category,
                    Price = @price,
                    IsAvailable = @available
                WHERE MenuId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", product.MenuId);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@category", product.Category);
            command.Parameters.AddWithValue("@price", product.Price);
            command.Parameters.AddWithValue("@available", product.IsAvailable);

            command.ExecuteNonQuery();
        }

        public void DeleteProduct(int menuId)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                DELETE FROM MenuItems
                WHERE MenuId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", menuId);

            command.ExecuteNonQuery();
        }
    }
}