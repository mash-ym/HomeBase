using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{  
    public class Subcontractor
    {
        public int Id { get; set; }
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

        public SubcontractorRepository(SQLiteConnection connection)
        {
            
        }

        public void CreateSubcontractor(Subcontractor subcontractor)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Subcontractor (id, company_name, address, 
                            occupation, phone_number, email_address, sns_info)
                            VALUES (@SubcontractorId, @CompanyName, @Address, @Occupation, 
                            @PhoneNumber, @EmailAddress, @SnsInfo)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubcontractorId", subcontractor.Id);
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
        public void Create(Subcontractor subcontractor)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = @"
                INSERT INTO subcontractor (company_name, address, occupation, phone_number, email_address, sns_info)
                VALUES (@CompanyName, @Address, @Occupation, @PhoneNumber, @EmailAddress, @SnsInfo);
                SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                    command.Parameters.AddWithValue("@Address", subcontractor.Address);
                    command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                    command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                    command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);

                    var id = (int)command.ExecuteScalar();
                    subcontractor.Id = id;
                }
            }
        }
        // Implement other CRUD operations as needed: Read, Update, Delete
        public void Insert(Subcontractor subcontractor)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                    INSERT INTO subcontractor (company_name, address, occupation, phone_number, email_address, sns_info)
                    VALUES (@CompanyName, @Address, @Occupation, @PhoneNumber, @EmailAddress, @SnsInfo)";
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

        public Subcontractor GetById(long id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = "SELECT * FROM subcontractor WHERE id = @Id";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToSubcontractor(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Subcontractor> GetAll()
        {
            var subcontractors = new List<Subcontractor>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = "SELECT * FROM subcontractor";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subcontractor = MapToSubcontractor(reader);
                            subcontractors.Add(subcontractor);
                        }
                    }
                }
            }

            return subcontractors;
        }

        public void Update(Subcontractor subcontractor)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = @"
                UPDATE subcontractor
                SET company_name = @CompanyName, address = @Address, occupation = @Occupation,
                    phone_number = @PhoneNumber, email_address = @EmailAddress, sns_info = @SnsInfo
                WHERE id = @Id";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                    command.Parameters.AddWithValue("@Address", subcontractor.Address);
                    command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                    command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                    command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);
                    command.Parameters.AddWithValue("@Id", subcontractor.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(long id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = "DELETE FROM subcontractor WHERE id = @Id";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<Subcontractor> Search(string keyword)
        {
            var subcontractors = new List<Subcontractor>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = "SELECT * FROM subcontractor WHERE company_name LIKE @Keyword";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subcontractor = MapToSubcontractor(reader);
                            subcontractors.Add(subcontractor);
                        }
                    }
                }
            }

            return subcontractors;
        }

        private Subcontractor MapToSubcontractor(SQLiteDataReader reader)
        {
            var subcontractor = new Subcontractor();
            subcontractor.Id = (int)Convert.ToInt64(reader["id"]);
            subcontractor.CompanyName = Convert.ToString(reader["company_name"]);
            subcontractor.Address = Convert.ToString(reader["address"]);
            subcontractor.Occupation = Convert.ToString(reader["occupation"]);
            subcontractor.PhoneNumber = Convert.ToString(reader["phone_number"]);
            subcontractor.EmailAddress = Convert.ToString(reader["email_address"]);
            subcontractor.SnsInfo = Convert.ToString(reader["sns_info"]);

            return subcontractor;
        }
    }
}
