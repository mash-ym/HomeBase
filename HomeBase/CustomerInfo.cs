using System;
using System.Data.SQLite;

namespace HomeBase
{

    public class CustomerInfo
    {
        public int CustomerInfoId { get; set; }
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

                string query = @"INSERT INTO CustomerInfo (customer_info_id, name, building_info_id, phone_number, 
                            email_address, project_history, rating)
                            VALUES (@CustomerInfoId, @Name, @BuildingInfoId, @PhoneNumber, 
                            @EmailAddress, @ProjectHistory, @Rating)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerInfoId", customerInfo.CustomerInfoId);
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
    }

}
