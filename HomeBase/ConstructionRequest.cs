using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace HomeBase
{
    public class ConstructionRequest
    {
        public int RequestId { get; set; }
        public string EstimateId { get; set; }
        public string ItemName { get; set; }
        public double CostQuantity { get; set; }
        public string CostUnit { get; set; }
        public double CostUnitPrice { get; set; }
        public double WorkHours { get; set; }
        public DateTime StartDate { get; set; }
        public int SubcontractorId { get; set; }
        public string SiteContact { get; set; }
        public string SalesContact { get; set; }
        public byte[] DrawingPDF { get; set; }
    }

    public class ConstructionRequestRepository
    {
        private readonly DBManager _dbManager;
        private readonly ErrorHandler _errorHandler;

        public ConstructionRequestRepository(DBManager dbManager, ErrorHandler errorHandler)
        {
            _dbManager = dbManager;
            _errorHandler = errorHandler;
        }

        public void InsertConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO ConstructionRequest (EstimateId, ItemName, CostQuantity, CostUnit, CostUnitPrice, WorkHours, StartDate, SubcontractorId, SiteContact, SalesContact, DrawingPDF)" +
                                          "VALUES (@EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPDF)";
                    command.Parameters.AddWithValue("@EstimateId", constructionRequest.EstimateId);
                    command.Parameters.AddWithValue("@ItemName", constructionRequest.ItemName);
                    command.Parameters.AddWithValue("@CostQuantity", constructionRequest.CostQuantity);
                    command.Parameters.AddWithValue("@CostUnit", constructionRequest.CostUnit);
                    command.Parameters.AddWithValue("@CostUnitPrice", constructionRequest.CostUnitPrice);
                    command.Parameters.AddWithValue("@WorkHours", constructionRequest.WorkHours);
                    command.Parameters.AddWithValue("@StartDate", constructionRequest.StartDate);
                    command.Parameters.AddWithValue("@SubcontractorId", constructionRequest.SubcontractorId);
                    command.Parameters.AddWithValue("@SiteContact", constructionRequest.SiteContact);
                    command.Parameters.AddWithValue("@SalesContact", constructionRequest.SalesContact);
                    command.Parameters.AddWithValue("@DrawingPDF", constructionRequest.DrawingPDF);

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

        public void UpdateConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE ConstructionRequest SET EstimateId = @EstimateId, ItemName = @ItemName, CostQuantity = @CostQuantity, CostUnit = @CostUnit, CostUnitPrice = @CostUnitPrice, " +
                                          "WorkHours = @WorkHours, StartDate = @StartDate, SubcontractorId = @SubcontractorId, SiteContact = @SiteContact, SalesContact = @SalesContact, DrawingPDF = @DrawingPDF " +
                                          "WHERE RequestId = @RequestId";
                    command.Parameters.AddWithValue("@EstimateId", constructionRequest.EstimateId);
                    command.Parameters.AddWithValue("@ItemName", constructionRequest.ItemName);
                    command.Parameters.AddWithValue("@CostQuantity", constructionRequest.CostQuantity);
                    command.Parameters.AddWithValue("@CostUnit", constructionRequest.CostUnit);
                    command.Parameters.AddWithValue("@CostUnitPrice", constructionRequest.CostUnitPrice);
                    command.Parameters.AddWithValue("@WorkHours", constructionRequest.WorkHours);
                    command.Parameters.AddWithValue("@StartDate", constructionRequest.StartDate);
                    command.Parameters.AddWithValue("@SubcontractorId", constructionRequest.SubcontractorId);
                    command.Parameters.AddWithValue("@SiteContact", constructionRequest.SiteContact);
                    command.Parameters.AddWithValue("@SalesContact", constructionRequest.SalesContact);
                    command.Parameters.AddWithValue("@DrawingPDF", constructionRequest.DrawingPDF);
                    command.Parameters.AddWithValue("@RequestId", constructionRequest.RequestId);

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

        public void DeleteConstructionRequest(int requestId)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM ConstructionRequest WHERE RequestId = @RequestId";
                    command.Parameters.AddWithValue("@RequestId", requestId);

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

        public List<ConstructionRequest> GetAllConstructionRequests()
        {
            List<ConstructionRequest> constructionRequests = new List<ConstructionRequest>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ConstructionRequest";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConstructionRequest constructionRequest = new ConstructionRequest
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            EstimateId = Convert.ToString(reader["EstimateId"]),
                            ItemName = Convert.ToString(reader["ItemName"]),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = Convert.ToString(reader["CostUnit"]),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = Convert.ToString(reader["SiteContact"]),
                            SalesContact = Convert.ToString(reader["SalesContact"]),
                            DrawingPDF = (byte[])reader["DrawingPDF"]
                        };

                        constructionRequests.Add(constructionRequest);
                    }
                }
            }

            return constructionRequests;
        }

        public ConstructionRequest GetConstructionRequestById(int requestId)
        {
            ConstructionRequest constructionRequest = null;

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ConstructionRequest WHERE RequestId = @RequestId";
                command.Parameters.AddWithValue("@RequestId", requestId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        constructionRequest = new ConstructionRequest
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            EstimateId = Convert.ToString(reader["EstimateId"]),
                            ItemName = Convert.ToString(reader["ItemName"]),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = Convert.ToString(reader["CostUnit"]),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = Convert.ToString(reader["SiteContact"]),
                            SalesContact = Convert.ToString(reader["SalesContact"]),
                            DrawingPDF = (byte[])reader["DrawingPDF"]
                        };
                    }
                }
            }

            return constructionRequest;
        }

        public List<ConstructionRequest> SearchConstructionRequests(string keyword)
        {
            List<ConstructionRequest> results = new List<ConstructionRequest>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ConstructionRequest WHERE ItemName LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConstructionRequest constructionRequest = new ConstructionRequest
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            EstimateId = Convert.ToString(reader["EstimateId"]),
                            ItemName = Convert.ToString(reader["ItemName"]),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = Convert.ToString(reader["CostUnit"]),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = Convert.ToString(reader["SiteContact"]),
                            SalesContact = Convert.ToString(reader["SalesContact"]),
                            DrawingPDF = (byte[])reader["DrawingPDF"]
                        };

                        results.Add(constructionRequest);
                    }
                }
            }

            return results;
        }
    }
}
