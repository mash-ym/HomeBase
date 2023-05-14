using System;
using System.Data.SQLite;

namespace HomeBase
{  

    public class Subcontractor
    {
        public int SubcontractorId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string SnsInfo { get; set; }
    }

    public class SubcontractorRepository
    {
        private string connectionString;

        public SubcontractorRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateSubcontractor(Subcontractor subcontractor)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Subcontractor (subcontractor_id, company_name, address, 
                            occupation, phone_number, email_address, sns_info)
                            VALUES (@SubcontractorId, @CompanyName, @Address, @Occupation, 
                            @PhoneNumber, @EmailAddress, @SnsInfo)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcontractorId", subcontractor.SubcontractorId);
                    command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                    command.Parameters.AddWithValue("@Address", subcontractor.Address);
                    command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                    command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                    command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
    }

}
