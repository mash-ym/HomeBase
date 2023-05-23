using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{

    public class CustomerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingInfoId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ProjectHistory { get; set; }
        public int Rating { get; set; }
    }

    public class CustomerInfoRepository
    {
        private string connectionString;

        public CustomerInfoRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddCustomerInfo(CustomerInfo customerInfo)
        {
            string insertQuery = @"
            INSERT INTO CustomerInfo (name, building_info_id, phone_number, email_address, project_history, rating)
            VALUES (@Name, @BuildingInfoId, @PhoneNumber, @EmailAddress, @ProjectHistory, @Rating);
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", customerInfo.Name);
                command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                command.Parameters.AddWithValue("@Rating", customerInfo.Rating);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<CustomerInfo> GetAllCustomerInfo()
        {
            string selectQuery = "SELECT * FROM CustomerInfo;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                connection.Open();

                List<CustomerInfo> customerInfoList = new List<CustomerInfo>();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerInfo customerInfo = new CustomerInfo()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            BuildingInfoId = reader.GetInt32(2),
                            PhoneNumber = reader.GetString(3),
                            EmailAddress = reader.GetString(4),
                            ProjectHistory = reader.GetString(5),
                            Rating = reader.GetInt32(6)
                        };

                        customerInfoList.Add(customerInfo);
                    }
                }

                return customerInfoList;
            }
        }

        public void UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            string updateQuery = @"
            UPDATE CustomerInfo SET
            name = @Name,
            building_info_id = @BuildingInfoId,
            phone_number = @PhoneNumber,
            email_address = @EmailAddress,
            project_history = @ProjectHistory,
            rating = @Rating
            WHERE customer_info_id = @Id;
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", customerInfo.Name);
                command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                command.Parameters.AddWithValue("@Rating", customerInfo.Rating);
                command.Parameters.AddWithValue("@Id", customerInfo.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCustomerInfo(int customerId)
        {
            string deleteQuery = "DELETE FROM CustomerInfo WHERE customer_info_id = @Id;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", customerId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}
