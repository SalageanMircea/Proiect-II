using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories.Interfaces;

namespace Restaurant_Management.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        //initialise a database connection, using interfaces it won't be covered what types of database source we have

        public OrderRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //it is a new funcion insted of LoadOrders
        public List<Order> GetAllOrders() 
        {
            List<Order> orders = new List<Order>();

            //using - prevent conection leaking (it closes automatically after the code is executed)

            using (SqlConnection conn = _connectionFactory.GetConnection()) 
            {
                conn.Open();

                string sql = "SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt FROM Orders ORDER BY SentAt DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            TableNumber = Convert.ToInt32(reader["TableNumber"]),
                            WaiterName = reader["WaiterName"].ToString(),
                            Details = reader["Details"].ToString(),
                            Status = reader["Status"].ToString(),
                            SentAt = Convert.ToDateTime(reader["SentAt"])
                        });
                    }
                }
            }

            return orders;
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (SqlConnection conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                string sql = "UPDATE Orders SET Status=@status WHERE OrderId=@id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
