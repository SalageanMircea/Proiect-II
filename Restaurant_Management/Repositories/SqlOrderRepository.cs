using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlOrderRepository : IOrderRepository
    {
        public List<ChefOrder> GetAllOrders()
        {
            List<ChefOrder> orders = new List<ChefOrder>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt
                FROM Orders
                ORDER BY SentAt DESC";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ChefOrder order = new ChefOrder();

                order.OrderId = Convert.ToInt32(reader["OrderId"]);
                order.TableNumber = Convert.ToInt32(reader["TableNumber"]);
                order.WaiterName = reader["WaiterName"].ToString() ?? "";
                order.Details = reader["Details"].ToString() ?? "";
                order.Status = reader["Status"].ToString() ?? "";
                order.SentAt = Convert.ToDateTime(reader["SentAt"]);

                orders.Add(order);
            }

            return orders;
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE Orders
                SET Status = @status
                WHERE OrderId = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@status", newStatus);
            command.Parameters.AddWithValue("@id", orderId);

            command.ExecuteNonQuery();
        }
    }
}