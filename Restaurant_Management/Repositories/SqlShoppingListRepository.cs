using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlShoppingListRepository : IShoppingListRepository
    {
        public List<ShoppingItem> GetAllItems()
        {
            List<ShoppingItem> items = new List<ShoppingItem>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT ShoppingItemId, Name, Quantity, Unit, IsPurchased, CreatedAt
                FROM ShoppingItems
                ORDER BY IsPurchased ASC, CreatedAt DESC";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ShoppingItem item = new ShoppingItem();

                item.ShoppingItemId = Convert.ToInt32(reader["ShoppingItemId"]);
                item.Name = reader["Name"].ToString() ?? "";
                item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                item.Unit = reader["Unit"].ToString() ?? "";
                item.IsPurchased = Convert.ToBoolean(reader["IsPurchased"]);
                item.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                items.Add(item);
            }

            return items;
        }

        public void AddItem(ShoppingItem item)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                INSERT INTO ShoppingItems
                (Name, Quantity, Unit, IsPurchased, CreatedAt)
                VALUES
                (@name, @quantity, @unit, @isPurchased, @createdAt)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@quantity", item.Quantity);
            command.Parameters.AddWithValue("@unit", item.Unit);
            command.Parameters.AddWithValue("@isPurchased", item.IsPurchased);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now);

            command.ExecuteNonQuery();
        }

        public void UpdateItem(ShoppingItem item)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE ShoppingItems
                SET Name = @name,
                    Quantity = @quantity,
                    Unit = @unit,
                    IsPurchased = @isPurchased
                WHERE ShoppingItemId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", item.ShoppingItemId);
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@quantity", item.Quantity);
            command.Parameters.AddWithValue("@unit", item.Unit);
            command.Parameters.AddWithValue("@isPurchased", item.IsPurchased);

            command.ExecuteNonQuery();
        }

        public void UpdatePurchasedStatus(int itemId, bool isPurchased)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE ShoppingItems
                SET IsPurchased = @isPurchased
                WHERE ShoppingItemId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", itemId);
            command.Parameters.AddWithValue("@isPurchased", isPurchased);

            command.ExecuteNonQuery();
        }

        public void DeleteItem(int itemId)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                DELETE FROM ShoppingItems
                WHERE ShoppingItemId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", itemId);

            command.ExecuteNonQuery();
        }
    }
}