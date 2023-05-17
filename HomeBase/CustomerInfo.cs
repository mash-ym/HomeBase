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

        public CustomerInfoRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateCustomerInfo(CustomerInfo customerInfo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO CustomerInfo (id, name, building_info_id, phone_number, 
                            email_address, project_history, rating)
                            VALUES (@CustomerInfoId, @Name, @BuildingInfoId, @PhoneNumber, 
                            @EmailAddress, @ProjectHistory, @Rating)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", customerInfo.Id);
                    command.Parameters.AddWithValue("@Name", customerInfo.Name);
                    command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                    command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@Rating", customerInfo.Rating);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
        public void InsertCustomerInfo(CustomerInfo customerInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO CustomerInfo (Name, BuildingInfoId, PhoneNumber, EmailAddress, ProjectHistory, Rating) " +
                                          "VALUES (@Name, @BuildingInfoId, @PhoneNumber, @EmailAddress, @ProjectHistory, @Rating)";
                    command.Parameters.AddWithValue("@Name", customerInfo.Name);
                    command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                    command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@Rating", customerInfo.Rating);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<CustomerInfo> GetAllCustomerInfo()
        {
            var customerInfoList = new List<CustomerInfo>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM CustomerInfo";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customerInfo = new CustomerInfo
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
                }
            }

            return customerInfoList;
        }

        public void UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE CustomerInfo SET Name = @Name, BuildingInfoId = @BuildingInfoId, " +
                                          "PhoneNumber = @PhoneNumber, EmailAddress = @EmailAddress, " +
                                          "ProjectHistory = @ProjectHistory, Rating = @Rating WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Name", customerInfo.Name);
                    command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                    command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@Rating", customerInfo.Rating);
                    command.Parameters.AddWithValue("@Id", customerInfo.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomerInfo(int Id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM CustomerInfo WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
