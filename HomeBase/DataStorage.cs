using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace HomeBase
{
    public class DataStorage
    {
        private DBManager _dbManager;

        public DataStorage(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void SaveInitialSetupData(InitialSetupData data)
        {
            // 暗号化マネージャーを作成
            EncryptionManager encryptionManager = new EncryptionManager();

            // データを暗号化する
            string encryptedCompanyName = encryptionManager.Encrypt(data.CompanyName);
            string encryptedCompanyAddress = encryptionManager.Encrypt(data.CompanyAddress);
            string encryptedUserEmail = encryptionManager.Encrypt(data.UserEmail);
            string encryptedUsername = encryptionManager.Encrypt(data.Username);
            string encryptedPassword = encryptionManager.Encrypt(data.Password);
            string encryptedRole = encryptionManager.Encrypt(data.Role);
            List<string> encryptedSecurityGroups = data.SecurityGroups.Select(group => encryptionManager.Encrypt(group)).ToList();
            string encryptedApiKey = encryptionManager.Encrypt(data.ApiKey);
            string encryptedAuthToken = encryptionManager.Encrypt(data.AuthToken);
            string encryptedEndpointUrl = encryptionManager.Encrypt(data.EndpointUrl);

            // 暗号化されたデータをデータベースに保存する
            using (SQLiteConnection connection = _dbManager.Connection)
            {
                _dbManager.BeginTransaction();

                try
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO InitialSetup (CompanyName, CompanyAddress, UserEmail, Username, Password, PasswordComplexity, PasswordExpiration, Role, SecurityGroups, ApiKey, AuthToken, EndpointUrl) " +
                                              "VALUES (@CompanyName, @CompanyAddress, @UserEmail, @Username, @Password, @PasswordComplexity, @PasswordExpiration, @Role, @SecurityGroups, @ApiKey, @AuthToken, @EndpointUrl)";
                        command.Parameters.AddWithValue("@CompanyName", encryptionManager.Encrypt(data.CompanyName));
                        command.Parameters.AddWithValue("@CompanyAddress", encryptionManager.Encrypt(data.CompanyAddress));
                        command.Parameters.AddWithValue("@UserEmail", encryptionManager.Encrypt(data.UserEmail));
                        command.Parameters.AddWithValue("@Username", encryptionManager.Encrypt(data.Username));
                        command.Parameters.AddWithValue("@Password", encryptionManager.Encrypt(data.Password));
                        command.Parameters.AddWithValue("@PasswordComplexity", data.PasswordComplexity);
                        command.Parameters.AddWithValue("@PasswordExpiration", data.PasswordExpiration);
                        command.Parameters.AddWithValue("@Role", encryptionManager.Encrypt(data.Role));
                        command.Parameters.AddWithValue("@SecurityGroups", string.Join(",", data.SecurityGroups.Select(group => encryptionManager.Encrypt(group))));
                        command.Parameters.AddWithValue("@ApiKey", encryptionManager.Encrypt(data.ApiKey));
                        command.Parameters.AddWithValue("@AuthToken", encryptionManager.Encrypt(data.AuthToken));
                        command.Parameters.AddWithValue("@EndpointUrl", encryptionManager.Encrypt(data.EndpointUrl));
                        command.ExecuteNonQuery();
                    }

                    _dbManager.CommitTransaction();
                }
                catch
                {
                    _dbManager.RollbackTransaction();
                    throw;
                }
            }

        }


        public InitialSetupData LoadInitialSetupData()
        {
            InitialSetupData data = new InitialSetupData();
            // 暗号化マネージャーを作成
            EncryptionManager encryptionManager = new EncryptionManager();

            using (SQLiteConnection connection = _dbManager.Connection)
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT CompanyName, CompanyAddress, UserEmail, Username, Password, PasswordComplexity, PasswordExpiration, Role, SecurityGroups, ApiKey, AuthToken, EndpointUrl " +
                                          "FROM InitialSetup";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.CompanyName = encryptionManager.Decrypt(reader.GetString(0));
                            data.CompanyAddress = encryptionManager.Decrypt(reader.GetString(1));
                            data.UserEmail = encryptionManager.Decrypt(reader.GetString(2));
                            data.Username = encryptionManager.Decrypt(reader.GetString(3));
                            data.Password = encryptionManager.Decrypt(reader.GetString(4));
                            data.PasswordComplexity = reader.GetBoolean(5);
                            data.PasswordExpiration = reader.GetBoolean(6);
                            data.Role = encryptionManager.Decrypt(reader.GetString(7));
                            data.SecurityGroups = reader.GetString(8).Split(',').Select(group => encryptionManager.Decrypt(group)).ToList();
                            data.ApiKey = encryptionManager.Decrypt(reader.GetString(9));
                            data.AuthToken = encryptionManager.Decrypt(reader.GetString(10));
                            data.EndpointUrl = encryptionManager.Decrypt(reader.GetString(11));
                        }
                    }
                }
            }

            return data;
        }

    }

}
