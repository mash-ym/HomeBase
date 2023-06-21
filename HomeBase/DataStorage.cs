using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace HomeBase
{
    public class DataStorage
    {
        private DBManager _dbManager;
        private EncryptionManager _encryptionManager;

        public DataStorage(DBManager dbManager)
        {
            _dbManager = dbManager;
            _encryptionManager = new EncryptionManager();
        }

        // ユーザー認証を行うメソッド
        public bool AuthenticateUser(string username, string password)
        {
            // ユーザーの認証処理を実装する
            // 例えば、データベースに保存されたユーザーネームとパスワードを照合するなどの処理を行います
            // 認証が成功した場合は true を返し、失敗した場合は false を返します
            // この例では仮に "admin" というユーザー名とパスワード "password" で認証を行うものとします

            // ユーザーネームとパスワードを暗号化する
            string encryptedUsername = _encryptionManager.Encrypt(username);
            string encryptedPassword = _encryptionManager.Encrypt(password);

            // データベースなどからユーザーネームと暗号化されたパスワードを取得して照合する
            string storedUsername = "admin";  // データベースから取得したユーザーネーム
            string storedEncryptedPassword = _encryptionManager.Encrypt("password");  // データベースから取得した暗号化されたパスワード

            return encryptedUsername == storedUsername && encryptedPassword == storedEncryptedPassword;
        }

        // 初期設定データの保存
        public void SaveInitialSetupData(InitialSetupData data)
        {
            // ユーザー認証を行います
            bool isAuthenticated = AuthenticateUser(data.Username, data.Password);
            if (!isAuthenticated)
            {
                throw new Exception("ユーザー認証に失敗しました。初期設定データの保存が許可されていません。");
            }

            // データを暗号化する
            string encryptedCompanyName = _encryptionManager.Encrypt(data.CompanyName);
            string encryptedCompanyAddress = _encryptionManager.Encrypt(data.CompanyAddress);
            string encryptedUserEmail = _encryptionManager.Encrypt(data.UserEmail);
            string encryptedUsername = _encryptionManager.Encrypt(data.Username);
            string encryptedPassword = _encryptionManager.Encrypt(data.Password);
            string encryptedRole = _encryptionManager.Encrypt(data.Role);
            List<string> encryptedSecurityGroups = data.SecurityGroups.Select(group => _encryptionManager.Encrypt(group)).ToList();
            string encryptedApiKey = _encryptionManager.Encrypt(data.ApiKey);
            string encryptedAuthToken = _encryptionManager.Encrypt(data.AuthToken);
            string encryptedEndpointUrl = _encryptionManager.Encrypt(data.EndpointUrl);

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
                        command.Parameters.AddWithValue("@CompanyName", encryptedCompanyName);
                        command.Parameters.AddWithValue("@CompanyAddress", encryptedCompanyAddress);
                        command.Parameters.AddWithValue("@UserEmail", encryptedUserEmail);
                        command.Parameters.AddWithValue("@Username", encryptedUsername);
                        command.Parameters.AddWithValue("@Password", encryptedPassword);
                        command.Parameters.AddWithValue("@PasswordComplexity", data.PasswordComplexity);
                        command.Parameters.AddWithValue("@PasswordExpiration", data.PasswordExpiration);
                        command.Parameters.AddWithValue("@Role", encryptedRole);
                        command.Parameters.AddWithValue("@SecurityGroups", string.Join(",", encryptedSecurityGroups));
                        command.Parameters.AddWithValue("@ApiKey", encryptedApiKey);
                        command.Parameters.AddWithValue("@AuthToken", encryptedAuthToken);
                        command.Parameters.AddWithValue("@EndpointUrl", encryptedEndpointUrl);
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

        // 初期設定データのバックアップ
        public void BackupInitialSetupData(string backupFilePath)
        {
            // 初期設定データをロードする
            InitialSetupData data = LoadInitialSetupData();

            // 初期設定データをバックアップファイルに保存する
            using (StreamWriter writer = new StreamWriter(backupFilePath))
            {
                // バックアップファイルに初期設定データを書き込む処理を実装する
                writer.WriteLine(data.CompanyName);
                writer.WriteLine(data.CompanyAddress);
                writer.WriteLine(data.UserEmail);
                writer.WriteLine(data.Username);
                writer.WriteLine(data.Password);
                writer.WriteLine(data.PasswordComplexity);
                writer.WriteLine(data.PasswordExpiration);
                writer.WriteLine(data.Role);
                writer.WriteLine(string.Join(",", data.SecurityGroups));
                writer.WriteLine(data.ApiKey);
                writer.WriteLine(data.AuthToken);
                writer.WriteLine(data.EndpointUrl);
            }
        }

        // 初期設定データの復元
        public void RestoreInitialSetupData(string backupFilePath)
        {
            // バックアップファイルから初期設定データを読み込む
            InitialSetupData data = new InitialSetupData();

            using (StreamReader reader = new StreamReader(backupFilePath))
            {
                // バックアップファイルから初期設定データを読み込む処理を実装する
                data.CompanyName = reader.ReadLine();
                data.CompanyAddress = reader.ReadLine();
                data.UserEmail = reader.ReadLine();
                data.Username = reader.ReadLine();
                data.Password = reader.ReadLine();
                data.PasswordComplexity = bool.Parse(reader.ReadLine());
                data.PasswordExpiration = bool.Parse(reader.ReadLine());
                data.Role = reader.ReadLine();
                data.SecurityGroups = reader.ReadLine().Split(',').ToList();
                data.ApiKey = reader.ReadLine();
                data.AuthToken = reader.ReadLine();
                data.EndpointUrl = reader.ReadLine();
            }

            // 読み込んだ初期設定データをデータベースに保存する
            SaveInitialSetupData(data);
        }

        // 初期設定データのロード
        public InitialSetupData LoadInitialSetupData()
        {
            // ユーザー認証を行います
            bool isAuthenticated = AuthenticateUser("admin", "password");
            if (!isAuthenticated)
            {
                throw new Exception("ユーザー認証に失敗しました。初期設定データへのアクセスが許可されていません。");
            }

            InitialSetupData data = new InitialSetupData();

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
                            data.CompanyName = _encryptionManager.Decrypt(reader.GetString(0));
                            data.CompanyAddress = _encryptionManager.Decrypt(reader.GetString(1));
                            data.UserEmail = _encryptionManager.Decrypt(reader.GetString(2));
                            data.Username = _encryptionManager.Decrypt(reader.GetString(3));
                            data.Password = _encryptionManager.Decrypt(reader.GetString(4));
                            data.PasswordComplexity = reader.GetBoolean(5);
                            data.PasswordExpiration = reader.GetBoolean(6);
                            data.Role = _encryptionManager.Decrypt(reader.GetString(7));
                            data.SecurityGroups = reader.GetString(8).Split(',').Select(group => _encryptionManager.Decrypt(group)).ToList();
                            data.ApiKey = _encryptionManager.Decrypt(reader.GetString(9));
                            data.AuthToken = _encryptionManager.Decrypt(reader.GetString(10));
                            data.EndpointUrl = _encryptionManager.Decrypt(reader.GetString(11));
                        }
                    }
                }
            }

            return data;
        }
    }


}
