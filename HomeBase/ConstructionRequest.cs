using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace HomeBase
{
    public class ConstructionRequest
    {
        public int RequestId { get; set; }
        public int ProjectId { get; set; } // プロジェクトID
        public Project Project { get; set; } // プロジェクトとの関連
        public int EstimateId { get; set; }
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
        public Estimate Estimate { get; set; }
    }


    public class ConstructionRequestRepository
    {
        private readonly DBManager _dbManager;

        public ConstructionRequestRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO ConstructionRequest (ProjectId, EstimateId, ItemName, CostQuantity, CostUnit, CostUnitPrice, WorkHours, StartDate, SubcontractorId, SiteContact, SalesContact, DrawingPDF)" +
                                          "VALUES (@ProjectId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPDF)";
                    command.Parameters.AddWithValue("@ProjectId", constructionRequest.ProjectId);
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
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE ConstructionRequest SET ProjectId = @ProjectId, EstimateId = @EstimateId, ItemName = @ItemName, CostQuantity = @CostQuantity, CostUnit = @CostUnit, CostUnitPrice = @CostUnitPrice, WorkHours = @WorkHours, StartDate = @StartDate, SubcontractorId = @SubcontractorId, SiteContact = @SiteContact, SalesContact = @SalesContact, DrawingPDF = @DrawingPDF WHERE RequestId = @RequestId";
                    command.Parameters.AddWithValue("@ProjectId", constructionRequest.ProjectId);
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
            using (SQLiteConnection connection = _dbManager.Connection)
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

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ConstructionRequest";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConstructionRequest constructionRequest = new ConstructionRequest()
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            EstimateId = Convert.ToInt32(reader["EstimateId"]),
                            ItemName = reader["ItemName"].ToString(),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = reader["CostUnit"].ToString(),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = reader["SiteContact"].ToString(),
                            SalesContact = reader["SalesContact"].ToString(),
                            DrawingPDF = (byte[])reader["DrawingPDF"],
                            Project = null // 関連するプロジェクト情報は初期値nullとしておく
                        };

                        constructionRequests.Add(constructionRequest);
                    }
                }
            }

            // プロジェクト情報の取得
            foreach (ConstructionRequest constructionRequest in constructionRequests)
            {
                constructionRequest.Project = GetProjectById(constructionRequest.ProjectId);
            }

            return constructionRequests;
        }

        public ConstructionRequest GetConstructionRequestById(int requestId)
        {
            ConstructionRequest constructionRequest = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ConstructionRequest WHERE RequestId = @RequestId";
                command.Parameters.AddWithValue("@RequestId", requestId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        constructionRequest = new ConstructionRequest()
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            EstimateId = Convert.ToInt32(reader["EstimateId"]),
                            ItemName = reader["ItemName"].ToString(),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = reader["CostUnit"].ToString(),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = reader["SiteContact"].ToString(),
                            SalesContact = reader["SalesContact"].ToString(),
                            DrawingPDF = (byte[])reader["DrawingPDF"],
                            Project = null // 関連するプロジェクト情報は初期値nullとしておく
                        };
                    }
                }
            }

            if (constructionRequest != null)
            {
                // プロジェクト情報の取得
                constructionRequest.Project = GetProjectById(constructionRequest.ProjectId);
            }

            return constructionRequest;
        }

        public Project GetProjectById(int projectId)
        {
            Project project = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Project WHERE ProjectId = @ProjectId";
                command.Parameters.AddWithValue("@ProjectId", projectId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        project = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            ProjectName = reader["ProjectName"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"].ToString(),
                            // 他のプロパティを設定する
                        };
                    }
                }
            }

            return project;
        }

        public List<ConstructionRequest> SearchConstructionRequests(string keyword)
        {
            List<ConstructionRequest> results = new List<ConstructionRequest>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT cr.*, p.* FROM ConstructionRequest cr " +
                                      "INNER JOIN Project p ON cr.ProjectId = p.Id " +
                                      "WHERE cr.ItemName LIKE @Keyword OR cr.SiteContact LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConstructionRequest constructionRequest = new ConstructionRequest
                        {
                            RequestId = Convert.ToInt32(reader["RequestId"]),
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            EstimateId = Convert.ToInt32(reader["EstimateId"]),
                            ItemName = reader["ItemName"].ToString(),
                            CostQuantity = Convert.ToDouble(reader["CostQuantity"]),
                            CostUnit = reader["CostUnit"].ToString(),
                            CostUnitPrice = Convert.ToDouble(reader["CostUnitPrice"]),
                            WorkHours = Convert.ToDouble(reader["WorkHours"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                            SiteContact = reader["SiteContact"].ToString(),
                            SalesContact = reader["SalesContact"].ToString(),
                            DrawingPDF = (byte[])reader["DrawingPDF"],
                            Project = new Project
                            {
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"]),
                                Status = reader["Status"].ToString()
                                // プロジェクト情報の他のプロパティを設定する
                            }
                        };

                        results.Add(constructionRequest);
                    }
                }
            }

            return results;
        }


    }
}
