using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlWaiterRepository : IWaiterRepository
    {
        public List<RestaurantMenuItem> GetAvailableMenuItems()
        {
            List<RestaurantMenuItem> items = new List<RestaurantMenuItem>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT MenuId, Name, Category, Price, IsAvailable
                FROM MenuItems
                WHERE IsAvailable = 1
                ORDER BY Category, Name";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                RestaurantMenuItem item = new RestaurantMenuItem();

                item.MenuId = Convert.ToInt32(reader["MenuId"]);
                item.Name = reader["Name"].ToString() ?? "";
                item.Category = reader["Category"].ToString() ?? "";
                item.Price = Convert.ToDecimal(reader["Price"]);
                item.IsAvailable = Convert.ToBoolean(reader["IsAvailable"]);

                items.Add(item);
            }

            return items;
        }

        public List<WaiterOrder> GetOrdersForWaiter(string waiterName)
        {
            List<WaiterOrder> orders = new List<WaiterOrder>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT OrderId, TableNumber, Details, Status, SentAt
                FROM Orders
                WHERE WaiterName = @waiter
                ORDER BY SentAt DESC";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@waiter", waiterName);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                WaiterOrder order = new WaiterOrder();

                order.OrderId = Convert.ToInt32(reader["OrderId"]);
                order.TableNumber = Convert.ToInt32(reader["TableNumber"]);
                order.Details = reader["Details"].ToString() ?? "";
                order.Status = reader["Status"].ToString() ?? "";
                order.SentAt = Convert.ToDateTime(reader["SentAt"]);

                orders.Add(order);
            }

            return orders;
        }

        public void CreateOrder(int tableNumber, string details, string waiterName)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                INSERT INTO Orders 
                (TableNumber, Details, Status, WaiterName, SentAt)
                VALUES 
                (@table, @details, @status, @waiter, @date)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@table", tableNumber);
            command.Parameters.AddWithValue("@details", details);
            command.Parameters.AddWithValue("@status", RestaurantOrderStatus.Received);
            command.Parameters.AddWithValue("@waiter", waiterName);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            command.ExecuteNonQuery();
        }
    }
}