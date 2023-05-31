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
        private readonly DBManager _dbManager;
        private readonly ErrorHandler _errorHandler;

        public CustomerInfoRepository(DBManager dbManager, ErrorHandler errorHandler)
        {
            _dbManager = dbManager;
            _errorHandler = errorHandler;
        }

        public void InsertCustomerInfo(CustomerInfo customerInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO CustomerInfo (Name, BuildingInfoId, PhoneNumber, EmailAddress, ProjectHistory, Rating)" +
                                          "VALUES (@Name, @BuildingInfoId, @PhoneNumber, @EmailAddress, @ProjectHistory, @Rating)";
                    command.Parameters.AddWithValue("@Name", customerInfo.Name);
                    command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                    command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@Rating", customerInfo.Rating);

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

        public void UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE CustomerInfo SET Name = @Name, BuildingInfoId = @BuildingInfoId, PhoneNumber = @PhoneNumber, " +
                                          "EmailAddress = @EmailAddress, ProjectHistory = @ProjectHistory, Rating = @Rating " +
                                          "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Name", customerInfo.Name);
                    command.Parameters.AddWithValue("@BuildingInfoId", customerInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@PhoneNumber", customerInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@EmailAddress", customerInfo.EmailAddress);
                    command.Parameters.AddWithValue("@ProjectHistory", customerInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@Rating", customerInfo.Rating);
                    command.Parameters.AddWithValue("@Id", customerInfo.Id);

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

        public void DeleteCustomerInfo(int customerId)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM CustomerInfo WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", customerId);

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

        public List<CustomerInfo> GetAllCustomerInfo()
        {
            List<CustomerInfo> customerInfos = new List<CustomerInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CustomerInfo";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerInfo customerInfo = new CustomerInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                            EmailAddress = Convert.ToString(reader["EmailAddress"]),
                            ProjectHistory = Convert.ToString(reader["ProjectHistory"]),
                            Rating = Convert.ToInt32(reader["Rating"])
                        };

                        customerInfos.Add(customerInfo);
                    }
                }
            }

            return customerInfos;
        }

        public CustomerInfo GetCustomerInfoById(int customerId)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CustomerInfo WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", customerId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CustomerInfo customerInfo = new CustomerInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                            EmailAddress = Convert.ToString(reader["EmailAddress"]),
                            ProjectHistory = Convert.ToString(reader["ProjectHistory"]),
                            Rating = Convert.ToInt32(reader["Rating"])
                        };

                        return customerInfo;
                    }
                }
            }

            return null;
        }

        public List<CustomerInfo> SearchCustomerInfo(string keyword)
        {
            List<CustomerInfo> results = new List<CustomerInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CustomerInfo WHERE Name LIKE @Keyword OR ProjectHistory LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerInfo customerInfo = new CustomerInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                            EmailAddress = Convert.ToString(reader["EmailAddress"]),
                            ProjectHistory = Convert.ToString(reader["ProjectHistory"]),
                            Rating = Convert.ToInt32(reader["Rating"])
                        };

                        results.Add(customerInfo);
                    }
                }
            }

            return results;
        }
    }


}
