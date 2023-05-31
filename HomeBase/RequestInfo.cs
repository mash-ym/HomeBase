using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{   

    public class RequestInfo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ReferrerId { get; set; }
        public int BuildingInfoId { get; set; }
        public string RequestContent { get; set; }
        public DateTime EstimateDeadline { get; set; }
        public bool OnSiteSurvey { get; set; }
        public byte[] PhotoData { get; set; }
    }

    public class RequestInfoRepository
    {
        private readonly DBManager _dbManager;
        private readonly ErrorHandler _errorHandler;

        public RequestInfoRepository(DBManager dbManager, ErrorHandler errorHandler)
        {
            _dbManager = dbManager;
            _errorHandler = errorHandler;
        }

        public void InsertRequestInfo(RequestInfo requestInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO RequestInfo (customer_id, referrer_id, building_info_id, request_content, " +
                                          "estimate_deadline, on_site_survey, photo_data) " +
                                          "VALUES (@CustomerId, @ReferrerId, @BuildingInfoId, @RequestContent, " +
                                          "@EstimateDeadline, @OnSiteSurvey, @PhotoData)";
                    command.Parameters.AddWithValue("@CustomerId", requestInfo.CustomerId);
                    command.Parameters.AddWithValue("@ReferrerId", requestInfo.ReferrerId);
                    command.Parameters.AddWithValue("@BuildingInfoId", requestInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@RequestContent", requestInfo.RequestContent);
                    command.Parameters.AddWithValue("@EstimateDeadline", requestInfo.EstimateDeadline);
                    command.Parameters.AddWithValue("@OnSiteSurvey", requestInfo.OnSiteSurvey);
                    command.Parameters.AddWithValue("@PhotoData", requestInfo.PhotoData);

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
        public void UpdateRequestInfo(RequestInfo requestInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE RequestInfo SET customer_id = @CustomerId, " +
                                          "referrer_id = @ReferrerId, building_info_id = @BuildingInfoId, " +
                                          "request_content = @RequestContent, estimate_deadline = @EstimateDeadline, " +
                                          "on_site_survey = @OnSiteSurvey, photo_data = @PhotoData " +
                                          "WHERE id = @Id";
                    command.Parameters.AddWithValue("@CustomerId", requestInfo.CustomerId);
                    command.Parameters.AddWithValue("@ReferrerId", requestInfo.ReferrerId);
                    command.Parameters.AddWithValue("@BuildingInfoId", requestInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@RequestContent", requestInfo.RequestContent);
                    command.Parameters.AddWithValue("@EstimateDeadline", requestInfo.EstimateDeadline);
                    command.Parameters.AddWithValue("@OnSiteSurvey", requestInfo.OnSiteSurvey);
                    command.Parameters.AddWithValue("@PhotoData", requestInfo.PhotoData);
                    command.Parameters.AddWithValue("@Id", requestInfo.Id);

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
        public RequestInfo GetRequestInfoById(int id)
        {
            RequestInfo requestInfo = null;

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM RequestInfo WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        requestInfo = new RequestInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            ReferrerId = Convert.ToInt32(reader["ReferrerId"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            RequestContent = Convert.ToString(reader["RequestContent"]),
                            EstimateDeadline = Convert.ToDateTime(reader["EstimateDeadline"]),
                            OnSiteSurvey = Convert.ToBoolean(reader["OnSiteSurvey"]),
                            PhotoData = (byte[])reader["PhotoData"]
                        };
                    }
                }
            }

            return requestInfo;
        }
        public List<RequestInfo> GetAllRequestInfo()
        {
            List<RequestInfo> requestInfoList = new List<RequestInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM RequestInfo";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RequestInfo requestInfo = new RequestInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            ReferrerId = Convert.ToInt32(reader["ReferrerId"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            RequestContent = Convert.ToString(reader["RequestContent"]),
                            EstimateDeadline = Convert.ToDateTime(reader["EstimateDeadline"]),
                            OnSiteSurvey = Convert.ToBoolean(reader["OnSiteSurvey"]),
                            PhotoData = (byte[])reader["PhotoData"]
                        };

                        requestInfoList.Add(requestInfo);
                    }
                }
            }

            return requestInfoList;
        }
        public List<RequestInfo> SearchRequestInfo(string keyword)
        {
            List<RequestInfo> results = new List<RequestInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM RequestInfo WHERE RequestContent LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RequestInfo requestInfo = new RequestInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            ReferrerId = Convert.ToInt32(reader["ReferrerId"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"]),
                            RequestContent = Convert.ToString(reader["RequestContent"]),
                            EstimateDeadline = Convert.ToDateTime(reader["EstimateDeadline"]),
                            OnSiteSurvey = Convert.ToBoolean(reader["OnSiteSurvey"]),
                            PhotoData = (byte[])reader["PhotoData"]
                        };

                        results.Add(requestInfo);
                    }
                }
            }

            return results;
        }
        public void DeleteRequestInfo(int id)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM RequestInfo WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

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

    }

}