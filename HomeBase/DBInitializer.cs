using System;
using System.Data.SQLite;

namespace HomeBase
{
    public class DBInitializer
    {
        private string connectionString;

        public DBInitializer(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }
        public void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                CreateTables(connection);
                CreateRepositories(connection);

                connection.Close();
            }
        }
        public void CreateTables(SQLiteConnection connection)
        {
            // テーブルの作成処理
            CreateCustomerInfoTable(connection);
            CreateBuildingInfoTable(connection);
            CreateConstructionTable(connection);
            CreateEstimateTable(connection);
            CreateEstimateDetailTable(connection);
            CreateRequestInfoTable(connection);
            CreateScheduleTable(connection);
            CreateSubcontractorTable(connection);
        }
        private void CreateRepositories(SQLiteConnection connection)
        {
            CustomerInfoRepository customerRepository = new CustomerInfoRepository(connection);
            BuildingInfoRepository buildingRepository = new BuildingInfoRepository(connection);
            ConstructionRequestRepository constructionRequestRepository = new ConstructionRequestRepository(connection);
            EstimateRepository estimateRepository = new EstimateRepository(connection);
            EstimateDetailRepository estimateDetailRepository = new EstimateDetailRepository(connection);
            RequestInfoRepository requestInfoRepository = new RequestInfoRepository(connection);
            ScheduleRepository scheduleRepository = new ScheduleRepository(connection);
            SubcontractorRepository subcontractorRepository = new SubcontractorRepository(connection);
        }
        private void CreateCustomerInfoTable(SQLiteConnection connection)
        {
            // CustomerInfoテーブルの作成処理
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS CustomerInfo (
                    customer_info_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    building_info_id INTEGER,
                    phone_number TEXT,
                    email_address TEXT,
                    project_history TEXT,
                    rating INTEGER,
                    FOREIGN KEY (building_info_id) REFERENCES BuildingInfo (building_info_id)
                );
            ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateBuildingInfoTable(SQLiteConnection connection)
        {
            // BuildingInfoテーブルの作成処理
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS BuildingInfo (
                    building_info_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    building_name TEXT NOT NULL,
                    room_number TEXT,
                    structure TEXT,
                    address TEXT,
                    area REAL,
                    project_history TEXT,
                    drawing_pdf BLOB
                );
            ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateConstructionTable(SQLiteConnection connection)
        {
            // Constructionテーブルの作成処理
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Construction (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RequestID INTEGER NOT NULL,
                    EstimateID INTEGER NOT NULL,
                    StartDate DATE,
                    SubcontractorID INTEGER,
                    SiteContact TEXT,
                    SalesContact TEXT,
                    DrawingPDF BLOB,
                    FOREIGN KEY (RequestID) REFERENCES RequestInfo (RequestInfoID),
                    FOREIGN KEY (EstimateID) REFERENCES Estimate (EstimateID),
                    FOREIGN KEY (SubcontractorID) REFERENCES Subcontractor (SubcontractorID)
                    );
            ";
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateEstimateTable(SQLiteConnection connection)
        {
            // Estimateテーブルの作成処理
            string createQuery = @"
            CREATE TABLE IF NOT EXISTS Estimate (
                EstimateID INTEGER PRIMARY KEY AUTOINCREMENT,
                SiteName TEXT,
                SiteAddress TEXT,
                CreatedAt DATETIME,
                RequestInfoID INTEGER,
                CustomerInfoID INTEGER,
                BuildingInfoID INTEGER,
                IssuedDate DATE,
                CreatorID INTEGER,
                TotalAmount REAL,
                Deadline DATE,
                RevisionHistory TEXT,
                DeliveryLocation TEXT,
                DrawingPDF BLOB,
                FOREIGN KEY (RequestInfoID) REFERENCES RequestInfo (RequestInfoID),
                FOREIGN KEY (CustomerInfoID) REFERENCES CustomerInfo (CustomerInfoID),
                FOREIGN KEY (BuildingInfoID) REFERENCES BuildingInfo (BuildingInfoID),
                FOREIGN KEY (CreatorID) REFERENCES Creator (CreatorID)
            );
        ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateEstimateDetailTable(SQLiteConnection connection)
        {
            // EstimateDetailテーブルの作成処理
            string createQuery = @"
            CREATE TABLE IF NOT EXISTS EstimateDetail (
                DetailID INTEGER PRIMARY KEY AUTOINCREMENT,
                EstimateID INTEGER,
                ItemName TEXT,
                CostQuantity INTEGER,
                CostUnit TEXT,
                CostUnitPrice REAL,
                EstimateQuantity INTEGER,
                EstimateUnit TEXT,
                EstimateUnitPrice REAL,
                Specification TEXT,
                MarkupRate REAL,
                Remarks TEXT,
                WorkHours REAL,
                SubcontractorID INTEGER,
                GroupNumber INTEGER,
                GroupName TEXT,
                Dependency TEXT,
                FOREIGN KEY (EstimateID) REFERENCES Estimate (EstimateID),
                FOREIGN KEY (SubcontractorID) REFERENCES Subcontractor (SubcontractorID)
            );
        ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateRequestInfoTable(SQLiteConnection connection)
        {
            // RequestInfoテーブルの作成処理
            string createQuery = @"
            CREATE TABLE IF NOT EXISTS RequestInfo (
                RequestInfoID INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerID INTEGER,
                ReferrerID INTEGER,
                BuildingInfoID INTEGER,
                RequestContent TEXT,
                EstimateDeadline DATE,
                OnSiteSurvey INTEGER,
                PhotoData BLOB,
                FOREIGN KEY (CustomerID) REFERENCES CustomerInfo (CustomerInfoID),
                FOREIGN KEY (ReferrerID) REFERENCES Referrer (ReferrerID),
                FOREIGN KEY (BuildingInfoID) REFERENCES BuildingInfo (BuildingInfoID)
            );
        ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateScheduleTable(SQLiteConnection connection)
        {
            // Scheduleテーブルの作成処理
            string createQuery = @"
            CREATE TABLE IF NOT EXISTS Schedule (
                ScheduleID INTEGER PRIMARY KEY AUTOINCREMENT,
                SiteName TEXT,
                SiteDuration TEXT,
                GroupName TEXT,
                StartDate DATE,
                EndDate DATE,
                WorkHours REAL,
                SubcontractorID INTEGER,
                FOREIGN KEY (SubcontractorID) REFERENCES Subcontractor (SubcontractorID)
            );
        ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }
        private void CreateSubcontractorTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Subcontractor (
                    SubcontractorID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CompanyName TEXT,
                    Address TEXT,
                    Occupation TEXT,
                    PhoneNumber TEXT,
                    EmailAddress TEXT,
                    SNSInfo TEXT
                );
            ";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 例外の処理
                Console.WriteLine("データベースの操作中にエラーが発生しました: " + ex.Message);
                // 例外の再スローまたはログの記録など、適切な処理を行ってください。
            }

        }

    }

}