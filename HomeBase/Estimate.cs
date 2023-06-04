using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{
    public class Estimate
    {
        public int EstimateId { get; set; } // 見積もりID
        public string SiteName { get; set; } // 現場名
        public int ProjectId { get; set; } // プロジェクトID
        public string SiteAddress { get; set; } // 現場住所
        public DateTime CreatedAt { get; set; } // 作成日時
        public int RequestInfoId { get; set; } // 依頼情報ID
        public int CustomerInfoId { get; set; } // 顧客情報ID
        public int BuildingInfoId { get; set; } // 建物情報ID
        public DateTime IssueDate { get; set; } // 発行日
        public int CreatorId { get; set; } // 作成者ID
        public decimal TotalAmount { get; set; } // 合計金額
        public DateTime DueDate { get; set; } // 納期
        public string ChangeHistory { get; set; } // 変更履歴
        public string DeliveryLocation { get; set; } // 納品場所
        public byte[] DrawingPdf { get; set; } // 図面のPDFデータ
        public List<EstimateDetail> EstimateDetails { get; set; } // 見積明細との関連
        public List<SpecificationDocument> SpecificationDocuments { get; set; }
    }


    public class EstimateRepository
    {
        private readonly DBManager _dbManager;

        public EstimateRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertEstimate(Estimate estimate)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // Estimate テーブルへの挿入クエリの作成
                    command.CommandText = @"
                INSERT INTO Estimate (SiteName, ProjectId, SiteAddress, CreatedAt, RequestInfoId, CustomerInfoId, BuildingInfoId, IssueDate,
                                      CreatorId, TotalAmount, DueDate, ChangeHistory, DeliveryLocation, DrawingPdf)
                VALUES (@SiteName, @ProjectId, @SiteAddress, @CreatedAt, @RequestInfoId, @CustomerInfoId, @BuildingInfoId, @IssueDate,
                        @CreatorId, @TotalAmount, @DueDate, @ChangeHistory, @DeliveryLocation, @DrawingPdf)";

                    // パラメータの設定
                    command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
                    command.Parameters.AddWithValue("@ProjectId", estimate.ProjectId);
                    command.Parameters.AddWithValue("@SiteAddress", estimate.SiteAddress);
                    command.Parameters.AddWithValue("@CreatedAt", estimate.CreatedAt);
                    command.Parameters.AddWithValue("@RequestInfoId", estimate.RequestInfoId);
                    command.Parameters.AddWithValue("@CustomerInfoId", estimate.CustomerInfoId);
                    command.Parameters.AddWithValue("@BuildingInfoId", estimate.BuildingInfoId);
                    command.Parameters.AddWithValue("@IssueDate", estimate.IssueDate);
                    command.Parameters.AddWithValue("@CreatorId", estimate.CreatorId);
                    command.Parameters.AddWithValue("@TotalAmount", estimate.TotalAmount);
                    command.Parameters.AddWithValue("@DueDate", estimate.DueDate);
                    command.Parameters.AddWithValue("@ChangeHistory", estimate.ChangeHistory);
                    command.Parameters.AddWithValue("@DeliveryLocation", estimate.DeliveryLocation);
                    command.Parameters.AddWithValue("@DrawingPdf", estimate.DrawingPdf);

                    // クエリの実行
                    command.ExecuteNonQuery();

                    // トランザクションのコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // トランザクションのロールバック
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                }
            }
        }

        public void UpdateEstimate(Estimate estimate)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // Estimate テーブルの更新クエリの作成
                    command.CommandText = @"
                UPDATE Estimate
                SET SiteName = @SiteName, ProjectId = @ProjectId, SiteAddress = @SiteAddress, CreatedAt = @CreatedAt,
                    RequestInfoId = @RequestInfoId, CustomerInfoId = @CustomerInfoId, BuildingInfoId = @BuildingInfoId,
                    IssueDate = @IssueDate, CreatorId = @CreatorId, TotalAmount = @TotalAmount, DueDate = @DueDate,
                    ChangeHistory = @ChangeHistory, DeliveryLocation = @DeliveryLocation, DrawingPdf = @DrawingPdf
                WHERE EstimateId = @EstimateId";

                    // パラメータの設定
                    command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
                    command.Parameters.AddWithValue("@ProjectId", estimate.ProjectId);
                    command.Parameters.AddWithValue("@SiteAddress", estimate.SiteAddress);
                    command.Parameters.AddWithValue("@CreatedAt", estimate.CreatedAt);
                    command.Parameters.AddWithValue("@RequestInfoId", estimate.RequestInfoId);
                    command.Parameters.AddWithValue("@CustomerInfoId", estimate.CustomerInfoId);
                    command.Parameters.AddWithValue("@BuildingInfoId", estimate.BuildingInfoId);
                    command.Parameters.AddWithValue("@IssueDate", estimate.IssueDate);
                    command.Parameters.AddWithValue("@CreatorId", estimate.CreatorId);
                    command.Parameters.AddWithValue("@TotalAmount", estimate.TotalAmount);
                    command.Parameters.AddWithValue("@DueDate", estimate.DueDate);
                    command.Parameters.AddWithValue("@ChangeHistory", estimate.ChangeHistory);
                    command.Parameters.AddWithValue("@DeliveryLocation", estimate.DeliveryLocation);
                    command.Parameters.AddWithValue("@DrawingPdf", estimate.DrawingPdf);
                    command.Parameters.AddWithValue("@EstimateId", estimate.EstimateId);

                    // クエリの実行
                    command.ExecuteNonQuery();

                    // トランザクションのコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // トランザクションのロールバック
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの更新エラー", ex);
                }
            }
        }

        public void DeleteEstimate(int estimateId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM Estimate WHERE EstimateId = @EstimateId";
                    command.Parameters.AddWithValue("@EstimateId", estimateId);

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

        public List<Estimate> GetAllEstimates()
        {
            List<Estimate> estimates = new List<Estimate>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    // Estimate テーブルから全てのレコードを取得するクエリ
                    command.CommandText = "SELECT * FROM Estimate";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Estimate estimate = new Estimate
                            {
                                EstimateId = Convert.ToInt32(reader["EstimateId"]),
                                SiteName = reader["SiteName"].ToString(),
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                SiteAddress = reader["SiteAddress"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                RequestInfoId = Convert.ToInt32(reader["RequestInfoId"]),
                                CustomerInfoId = Convert.ToInt32(reader["CustomerInfoId"]),
                                BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                                CreatorId = Convert.ToInt32(reader["CreatorId"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                DueDate = Convert.ToDateTime(reader["DueDate"]),
                                ChangeHistory = reader["ChangeHistory"].ToString(),
                                DeliveryLocation = reader["DeliveryLocation"].ToString(),
                                DrawingPdf = (byte[])reader["DrawingPdf"]
                            };

                            // TODO: EstimateDetails を取得して設定する処理
                            // EstimateDetail テーブルから該当の見積もりIDに紐づくレコードを取得するクエリ
                            command.CommandText = "SELECT * FROM EstimateDetail WHERE EstimateId = @EstimateId";
                            command.Parameters.AddWithValue("@EstimateId", estimate.EstimateId);

                            using (SQLiteDataReader detailReader = command.ExecuteReader())
                            {
                                List<EstimateDetail> estimateDetails = new List<EstimateDetail>();

                                while (detailReader.Read())
                                {
                                    EstimateDetail detail = new EstimateDetail
                                    {
                                        // EstimateDetail のプロパティを設定する処理
                                        DetailId = Convert.ToInt32(reader["DetailId"]),
                                        ParentDetailId = reader["ParentDetailId"] != DBNull.Value ? Convert.ToInt32(reader["ParentDetailId"]) : (int?)null,
                                        EstimateId = Convert.ToInt32(reader["EstimateId"]),
                                        ItemName = Convert.ToString(reader["ItemName"]),
                                        CostQuantity = Convert.ToDecimal(reader["CostQuantity"]),
                                        CostUnit = Convert.ToString(reader["CostUnit"]),
                                        CostUnitPrice = Convert.ToDecimal(reader["CostUnitPrice"]),
                                        CostAmount = Convert.ToDecimal(reader["CostAmount"]),
                                        EstimateQuantity = Convert.ToDecimal(reader["EstimateQuantity"]),
                                        EstimateUnit = Convert.ToString(reader["EstimateUnit"]),
                                        EstimateUnitPrice = Convert.ToDecimal(reader["EstimateUnitPrice"]),
                                        EstimateAmount = Convert.ToDecimal(reader["EstimateAmount"]),
                                        Specification = Convert.ToString(reader["Specification"]),
                                        MarkupRate = Convert.ToDecimal(reader["MarkupRate"]),
                                        Remarks = Convert.ToString(reader["Remarks"]),
                                        WorkHours = Convert.ToDecimal(reader["WorkHours"]),
                                        SubcontractorId = Convert.ToInt32(reader["SubcontractorId"]),
                                        GroupNumber = Convert.ToInt32(reader["GroupNumber"]),
                                        GroupName = Convert.ToString(reader["GroupName"]),
                                        Dependency = Convert.ToString(reader["Dependency"])
                                    };

                                    estimateDetails.Add(detail);
                                }

                                estimate.EstimateDetails = estimateDetails;
                            }


                            estimates.Add(estimate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データの取得エラー", ex);
                }
            }

            return estimates;
        }

        public Estimate GetEstimateById(int estimateId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM Estimate WHERE EstimateId = @EstimateId";
                    command.Parameters.AddWithValue("@EstimateId", estimateId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Estimate estimate = new Estimate
                            {
                                EstimateId = Convert.ToInt32(reader["EstimateId"]),
                                SiteName = Convert.ToString(reader["SiteName"]),
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                SiteAddress = Convert.ToString(reader["SiteAddress"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                RequestInfoId = Convert.ToInt32(reader["RequestInfoId"]),
                                CustomerInfoId = Convert.ToInt32(reader["CustomerInfoId"]),
                                BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                                CreatorId = Convert.ToInt32(reader["CreatorId"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                DueDate = Convert.ToDateTime(reader["DueDate"]),
                                ChangeHistory = Convert.ToString(reader["ChangeHistory"]),
                                DeliveryLocation = Convert.ToString(reader["DeliveryLocation"]),
                                DrawingPdf = (byte[])reader["DrawingPdf"]
                            };

                            return estimate;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データの取得エラー", ex);
                }
                return null;
            }
        }

        public List<Estimate> SearchEstimate(string keyword)
        {
            List<Estimate> estimates = new List<Estimate>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM Estimate WHERE SiteName LIKE @Keyword OR SiteAddress LIKE @Keyword";
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Estimate estimate = new Estimate
                            {
                                EstimateId = Convert.ToInt32(reader["EstimateId"]),
                                SiteName = Convert.ToString(reader["SiteName"]),
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                SiteAddress = Convert.ToString(reader["SiteAddress"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                RequestInfoId = Convert.ToInt32(reader["RequestInfoId"]),
                                CustomerInfoId = Convert.ToInt32(reader["CustomerInfoId"]),
                                BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                                CreatorId = Convert.ToInt32(reader["CreatorId"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                DueDate = Convert.ToDateTime(reader["DueDate"]),
                                ChangeHistory = Convert.ToString(reader["ChangeHistory"]),
                                DeliveryLocation = Convert.ToString(reader["DeliveryLocation"]),
                                DrawingPdf = (byte[])reader["DrawingPdf"]
                            };

                            estimates.Add(estimate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データの検索エラー", ex);
                }
            }

            return estimates;
        }

        

    }
}

