﻿using System;
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
                    command.CommandText = "INSERT INTO Estimate (SiteName, SiteAddress, CreatedAt, RequestInfoId, CustomerInfoId, BuildingInfoId, " +
                                          "IssueDate, CreatorId, TotalAmount, DueDate, ChangeHistory, DeliveryLocation, DrawingPdf)" +
                                          "VALUES (@SiteName, @SiteAddress, @CreatedAt, @RequestInfoId, @CustomerInfoId, @BuildingInfoId, " +
                                          "@IssueDate, @CreatorId, @TotalAmount, @DueDate, @ChangeHistory, @DeliveryLocation, @DrawingPdf)";
                    command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
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

        public void UpdateEstimate(Estimate estimate)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE Estimate SET SiteName = @SiteName, SiteAddress = @SiteAddress, " +
                                          "CreatedAt = @CreatedAt, RequestInfoId = @RequestInfoId, CustomerInfoId = @CustomerInfoId, " +
                                          "BuildingInfoId = @BuildingInfoId, IssueDate = @IssueDate, CreatorId = @CreatorId, " +
                                          "TotalAmount = @TotalAmount, DueDate = @DueDate, ChangeHistory = @ChangeHistory, " +
                                          "DeliveryLocation = @DeliveryLocation, DrawingPdf = @DrawingPdf " +
                                          "WHERE EstimateId = @EstimateId";
                    command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
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
                command.CommandText = "SELECT * FROM Estimate";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Estimate estimate = new Estimate
                        {
                            EstimateId = Convert.ToInt32(reader["EstimateId"]),
                            SiteName = Convert.ToString(reader["SiteName"]),
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

            return estimates;
        }

        public Estimate GetEstimateById(int estimateId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
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

            return null;
        }

        public List<Estimate> SearchEstimate(string keyword)
        {
            List<Estimate> estimates = new List<Estimate>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
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

            return estimates;
        }

        

    }
}

