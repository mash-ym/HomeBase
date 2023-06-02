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
        public void InsertSubcontractor(Subcontractor subcontractor)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO Subcontractor (company_name, address, occupation, phone_number, email_address, sns_info)" +
                                          "VALUES (@CompanyName, @Address, @Occupation, @PhoneNumber, @EmailAddress, @SnsInfo)";
                    command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                    command.Parameters.AddWithValue("@Address", subcontractor.Address);
                    command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                    command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                    command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                }
            }
        }
        public void UpdateSubcontractor(Subcontractor subcontractor)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE Subcontractor SET company_name = @CompanyName, address = @Address, " +
                                          "occupation = @Occupation, phone_number = @PhoneNumber, email_address = @EmailAddress, " +
                                          "sns_info = @SnsInfo WHERE id = @Id";
                    command.Parameters.AddWithValue("@CompanyName", subcontractor.CompanyName);
                    command.Parameters.AddWithValue("@Address", subcontractor.Address);
                    command.Parameters.AddWithValue("@Occupation", subcontractor.Occupation);
                    command.Parameters.AddWithValue("@PhoneNumber", subcontractor.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", subcontractor.EmailAddress);
                    command.Parameters.AddWithValue("@SnsInfo", subcontractor.SnsInfo);
                    command.Parameters.AddWithValue("@Id", subcontractor.Id);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの更新エラー", ex);
                }
            }
        }
        public void DeleteSubcontractor(int subcontractorId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM Subcontractor WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", subcontractorId);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの削除エラー", ex);
                }
            }
        }
        public List<Subcontractor> GetAllSubcontractors()
        {
            List<Subcontractor> subcontractors = new List<Subcontractor>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM Subcontractor";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subcontractor subcontractor = new Subcontractor
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                CompanyName = Convert.ToString(reader["company_name"]),
                                Address = Convert.ToString(reader["address"]),
                                Occupation = Convert.ToString(reader["occupation"]),
                                PhoneNumber = Convert.ToString(reader["phone_number"]),
                                EmailAddress = Convert.ToString(reader["email_address"]),
                                SnsInfo = Convert.ToString(reader["sns_info"])
                            };

                            subcontractors.Add(subcontractor);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データの取得エラー", ex);
                }
            }

            return subcontractors;
        }
        public Subcontractor GetSubcontractorById(int id)
        {
            Subcontractor subcontractor = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM Subcontractor WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subcontractor = new Subcontractor
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                CompanyName = Convert.ToString(reader["company_name"]),
                                Address = Convert.ToString(reader["address"]),
                                Occupation = Convert.ToString(reader["occupation"]),
                                PhoneNumber = Convert.ToString(reader["phone_number"]),
                                EmailAddress = Convert.ToString(reader["email_address"]),
                                SnsInfo = Convert.ToString(reader["sns_info"])
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データの取得エラー", ex);
                }
            }

            return subcontractor;
        }
        public List<Subcontractor> SearchSubcontractors(string keyword)
        {
            List<Subcontractor> subcontractors = new List<Subcontractor>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM Subcontractor WHERE id = @Keyword OR company_name LIKE @Keyword OR occupation LIKE @Keyword";
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subcontractor subcontractor = new Subcontractor
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                CompanyName = Convert.ToString(reader["company_name"]),
                                Address = Convert.ToString(reader["address"]),
                                Occupation = Convert.ToString(reader["occupation"]),
                                PhoneNumber = Convert.ToString(reader["phone_number"]),
                                EmailAddress = Convert.ToString(reader["email_address"]),
                                SnsInfo = Convert.ToString(reader["sns_info"])
                            };

                            subcontractors.Add(subcontractor);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("検索エラー", ex);
                }
            }

            return subcontractors;
        }

    }
}
