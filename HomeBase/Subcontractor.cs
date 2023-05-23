using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net.Mail;

namespace HomeBase
{
    public class Subcontractor
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        private string _emailAddress;

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (!IsValidEmail(value))
                {
                    throw new ArgumentException("Invalid email address.");
                }

                _emailAddress = value;
            }
        }

        public string SnsInfo { get; set; }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }


    public class SubcontractorRepository
    {
        private readonly DBManager _dbManager;

        public SubcontractorRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void AddSubcontractor(Subcontractor subcontractor)
        {
            string query = @"
            INSERT INTO Subcontractor (company_name, address, occupation, phone_number, email_address, sns_info)
            VALUES (@CompanyName, @Address, @Occupation, @PhoneNumber, @EmailAddress, @SnsInfo);
        ";

            using (var connection = _dbManager.GetConnection())
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                command.Parameters.AddWithValue("@Address", subcontractor.Address);
                command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateSubcontractor(Subcontractor subcontractor)
        {
            string query = @"
            UPDATE Subcontractor
            SET company_name = @CompanyName, address = @Address, occupation = @Occupation,
                phone_number = @PhoneNumber, email_address = @EmailAddress, sns_info = @SnsInfo
            WHERE subcontractor_id = @Id;
        ";

            using (var connection = _dbManager.GetConnection())
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                command.Parameters.AddWithValue("@Address", subcontractor.Address);
                command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);
                command.Parameters.AddWithValue("@Id", subcontractor.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteSubcontractor(int subcontractorId)
        {
            string query = @"
            DELETE FROM Subcontractor WHERE subcontractor_id = @Id;
        ";

            using (var connection = _dbManager.GetConnection())
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", subcontractorId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Subcontractor> GetAllSubcontractors()
        {
            List<Subcontractor> subcontractors = new List<Subcontractor>();

            string query = "SELECT * FROM Subcontractor;";

            using (var connection = _dbManager.GetConnection())
            using (var command = new SQLiteCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subcontractors.Add(new Subcontractor
                        {
                            Id = Convert.ToInt32(reader["subcontractor_id"]),
                            CompanyName = reader["company_name"].ToString(),
                            Address = reader["address"].ToString(),
                            Occupation = reader["occupation"].ToString(),
                            PhoneNumber = reader["phone_number"].ToString(),
                            EmailAddress = reader["email_address"].ToString(),
                            SnsInfo = reader["sns_info"].ToString()
                        });
                    }
                }
            }

            return subcontractors;
        }
    }
}
