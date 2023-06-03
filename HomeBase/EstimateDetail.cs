using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace HomeBase
{
    public class EstimateDetail
    {
        public int DetailId { get; set; }
        public int ProjectId { get; set; } // プロジェクトID
        public int? ParentDetailId { get; set; }
        public int EstimateId { get; set; }
        public string ItemName { get; set; }
        public decimal CostQuantity { get; set; }
        public string CostUnit { get; set; }
        public decimal CostUnitPrice { get; set; }
        public decimal CostAmount { get; set; }
        public decimal EstimateQuantity { get; set; }
        public string EstimateUnit { get; set; }
        public decimal EstimateUnitPrice { get; set; }
        public decimal EstimateAmount { get; set; }
        public string Specification { get; set; }
        public decimal MarkupRate { get; set; }
        public string Remarks { get; set; }
        public decimal WorkHours { get; set; }
        public int SubcontractorId { get; set; }
        public int GroupNumber { get; set; }
        public string GroupName { get; set; }
        public string Dependency { get; set; }
        public List<EstimateDetail> Children { get; set; }
        public Estimate Estimate { get; set; } // Estimateとの関連

        public EstimateDetail()
        {
            // デフォルトコンストラクタ
            Children = new List<EstimateDetail>();
        }

        public EstimateDetail(int detailId, int estimateId, string itemName, decimal costQuantity, string costUnit, decimal costUnitPrice,
            decimal estimateQuantity, string estimateUnit, decimal estimateUnitPrice, string specification,
            decimal markupRate, string remarks, decimal workHours, int subcontractorId,
            int groupNumber, string groupName, string dependency)
        {
            DetailId = detailId;
            ParentDetailId = null;
            EstimateId = estimateId;
            ItemName = itemName;
            CostQuantity = costQuantity;
            CostUnit = costUnit;
            CostUnitPrice = costUnitPrice;
            EstimateQuantity = estimateQuantity;
            EstimateUnit = estimateUnit;
            EstimateUnitPrice = estimateUnitPrice;
            Specification = specification;
            MarkupRate = markupRate;
            Remarks = remarks;
            WorkHours = workHours;
            SubcontractorId = subcontractorId;
            GroupNumber = groupNumber;
            GroupName = groupName;
            Dependency = dependency;
            Children = new List<EstimateDetail>();
        }

        public void RecalculateAmount()
        {
            EstimateAmount = EstimateQuantity * EstimateUnitPrice;
            CostAmount = CostQuantity * CostUnitPrice;
        }
    }


    public class EstimateDetailRepository
    {
        private readonly DBManager _dbManager;

        public EstimateDetailRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertEstimateDetail(EstimateDetail estimateDetail)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 親階層の見積金額および原価の集計
                    decimal parentEstimateAmount = 0;
                    decimal parentCostAmount = 0;

                    if (estimateDetail.ParentDetailId.HasValue)
                    {
                        EstimateDetail parentDetail = GetEstimateDetail(estimateDetail.ParentDetailId.Value);
                        parentEstimateAmount = parentDetail.EstimateAmount;
                        parentCostAmount = parentDetail.CostAmount;
                    }

                    // 自身の見積金額および原価の計算
                    estimateDetail.RecalculateAmount();
                    decimal estimateAmount = estimateDetail.EstimateAmount;
                    decimal costAmount = estimateDetail.CostAmount;

                    // 親階層の見積金額および原価に加算
                    parentEstimateAmount += estimateAmount;
                    parentCostAmount += costAmount;

                    // 更新された見積金額および原価を設定
                    estimateDetail.EstimateAmount = estimateAmount;
                    estimateDetail.CostAmount = costAmount;

                    // 親階層の見積金額および原価を更新
                    if (estimateDetail.ParentDetailId.HasValue)
                    {
                        EstimateDetail parentDetail = GetEstimateDetail(estimateDetail.ParentDetailId.Value);
                        parentDetail.EstimateAmount = parentEstimateAmount;
                        parentDetail.CostAmount = parentCostAmount;
                        UpdateEstimateDetail(parentDetail); // 親階層の見積明細を更新
                    }

                    // 自身の見積明細を挿入
                    command.CommandText = "INSERT INTO EstimateDetail (DetailId, ParentDetailId, EstimateId, ItemName, CostQuantity, CostUnit, CostUnitPrice, CostAmount, EstimateQuantity, EstimateUnit, EstimateUnitPrice, EstimateAmount, Specification, MarkupRate, Remarks, WorkHours, SubcontractorId, GroupNumber, GroupName, Dependency) " +
                                          "VALUES (@DetailId, @ParentDetailId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, @CostAmount, @EstimateQuantity, @EstimateUnit, @EstimateUnitPrice, @EstimateAmount, @Specification, @MarkupRate, @Remarks, @WorkHours, @SubcontractorId, @GroupNumber, @GroupName, @Dependency)";
                    command.Parameters.AddWithValue("@DetailId", estimateDetail.DetailId);
                    command.Parameters.AddWithValue("@ParentDetailId", estimateDetail.ParentDetailId.HasValue ? (object)estimateDetail.ParentDetailId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@EstimateId", estimateDetail.EstimateId);
                    command.Parameters.AddWithValue("@ItemName", estimateDetail.ItemName);
                    command.Parameters.AddWithValue("@CostQuantity", estimateDetail.CostQuantity);
                    command.Parameters.AddWithValue("@CostUnit", estimateDetail.CostUnit);
                    command.Parameters.AddWithValue("@CostUnitPrice", estimateDetail.CostUnitPrice);
                    command.Parameters.AddWithValue("@CostAmount", estimateDetail.CostAmount);
                    command.Parameters.AddWithValue("@EstimateQuantity", estimateDetail.EstimateQuantity);
                    command.Parameters.AddWithValue("@EstimateUnit", estimateDetail.EstimateUnit);
                    command.Parameters.AddWithValue("@EstimateUnitPrice", estimateDetail.EstimateUnitPrice);
                    command.Parameters.AddWithValue("@EstimateAmount", estimateDetail.EstimateAmount);
                    command.Parameters.AddWithValue("@Specification", estimateDetail.Specification);
                    command.Parameters.AddWithValue("@MarkupRate", estimateDetail.MarkupRate);
                    command.Parameters.AddWithValue("@Remarks", estimateDetail.Remarks);
                    command.Parameters.AddWithValue("@WorkHours", estimateDetail.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorId", estimateDetail.SubcontractorId);
                    command.Parameters.AddWithValue("@GroupNumber", estimateDetail.GroupNumber);
                    command.Parameters.AddWithValue("@GroupName", estimateDetail.GroupName);
                    command.Parameters.AddWithValue("@Dependency", estimateDetail.Dependency);

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

        public EstimateDetail GetEstimateDetail(int detailId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM EstimateDetail WHERE DetailId = @DetailId";
                command.Parameters.AddWithValue("@DetailId", detailId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        EstimateDetail estimateDetail = new EstimateDetail
                        {
                            DetailId = reader.GetInt32(reader.GetOrdinal("DetailId")),
                            ParentDetailId = reader.IsDBNull(reader.GetOrdinal("ParentDetailId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ParentDetailId")),
                            EstimateId = reader.GetInt32(reader.GetOrdinal("EstimateId")),
                            ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                            CostQuantity = reader.GetDecimal(reader.GetOrdinal("CostQuantity")),
                            CostUnit = reader.GetString(reader.GetOrdinal("CostUnit")),
                            CostUnitPrice = reader.GetDecimal(reader.GetOrdinal("CostUnitPrice")),
                            CostAmount = reader.GetDecimal(reader.GetOrdinal("CostAmount")),
                            EstimateQuantity = reader.GetDecimal(reader.GetOrdinal("EstimateQuantity")),
                            EstimateUnit = reader.GetString(reader.GetOrdinal("EstimateUnit")),
                            EstimateUnitPrice = reader.GetDecimal(reader.GetOrdinal("EstimateUnitPrice")),
                            EstimateAmount = reader.GetDecimal(reader.GetOrdinal("EstimateAmount")),
                            Specification = reader.GetString(reader.GetOrdinal("Specification")),
                            MarkupRate = reader.GetDecimal(reader.GetOrdinal("MarkupRate")),
                            Remarks = reader.GetString(reader.GetOrdinal("Remarks")),
                            WorkHours = reader.GetDecimal(reader.GetOrdinal("WorkHours")),
                            SubcontractorId = reader.GetInt32(reader.GetOrdinal("SubcontractorId")),
                            GroupNumber = reader.GetInt32(reader.GetOrdinal("GroupNumber")),
                            GroupName = reader.GetString(reader.GetOrdinal("GroupName")),
                            Dependency = reader.GetString(reader.GetOrdinal("Dependency")),
                            Children = new List<EstimateDetail>()
                        };

                        return estimateDetail;
                    }
                }
            }

            return null;
        }

        public void UpdateEstimateDetail(EstimateDetail estimateDetail)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE EstimateDetail SET EstimateId = @EstimateId, ItemName = @ItemName, CostQuantity = @CostQuantity, " +
                                          "CostUnit = @CostUnit, CostUnitPrice = @CostUnitPrice, EstimateQuantity = @EstimateQuantity, " +
                                          "EstimateUnit = @EstimateUnit, EstimateUnitPrice = @EstimateUnitPrice, Specification = @Specification, " +
                                          "MarkupRate = @MarkupRate, Remarks = @Remarks, WorkHours = @WorkHours, SubcontractorId = @SubcontractorId, " +
                                          "GroupNumber = @GroupNumber, GroupName = @GroupName, Dependency = @Dependency, " +
                                          "CostAmount = @CostAmount, EstimateAmount = @EstimateAmount " +
                                          "WHERE DetailId = @DetailId";
                    command.Parameters.AddWithValue("@EstimateId", estimateDetail.EstimateId);
                    command.Parameters.AddWithValue("@ItemName", estimateDetail.ItemName);
                    command.Parameters.AddWithValue("@CostQuantity", estimateDetail.CostQuantity);
                    command.Parameters.AddWithValue("@CostUnit", estimateDetail.CostUnit);
                    command.Parameters.AddWithValue("@CostUnitPrice", estimateDetail.CostUnitPrice);
                    command.Parameters.AddWithValue("@EstimateQuantity", estimateDetail.EstimateQuantity);
                    command.Parameters.AddWithValue("@EstimateUnit", estimateDetail.EstimateUnit);
                    command.Parameters.AddWithValue("@EstimateUnitPrice", estimateDetail.EstimateUnitPrice);
                    command.Parameters.AddWithValue("@Specification", estimateDetail.Specification);
                    command.Parameters.AddWithValue("@MarkupRate", estimateDetail.MarkupRate);
                    command.Parameters.AddWithValue("@Remarks", estimateDetail.Remarks);
                    command.Parameters.AddWithValue("@WorkHours", estimateDetail.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorId", estimateDetail.SubcontractorId);
                    command.Parameters.AddWithValue("@GroupNumber", estimateDetail.GroupNumber);
                    command.Parameters.AddWithValue("@GroupName", estimateDetail.GroupName);
                    command.Parameters.AddWithValue("@Dependency", estimateDetail.Dependency);
                    command.Parameters.AddWithValue("@CostAmount", estimateDetail.CostAmount);
                    command.Parameters.AddWithValue("@EstimateAmount", estimateDetail.EstimateAmount);
                    command.Parameters.AddWithValue("@DetailId", estimateDetail.DetailId);

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


        public void DeleteEstimateDetail(int detailId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM EstimateDetail WHERE DetailId = @DetailId";
                    command.Parameters.AddWithValue("@DetailId", detailId);

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


        public List<EstimateDetail> GetAllEstimateDetails()
        {
            List<EstimateDetail> results = new List<EstimateDetail>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM EstimateDetail";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EstimateDetail estimateDetail = new EstimateDetail
                        {
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

                        results.Add(estimateDetail);
                    }
                }
            }

            return results;
        }


        public EstimateDetail GetEstimateDetailById(int detailId)
        {
            EstimateDetail estimateDetail = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM EstimateDetail WHERE DetailId = @DetailId";
                command.Parameters.AddWithValue("@DetailId", detailId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estimateDetail = new EstimateDetail
                        {
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
                    }
                }
            }

            return estimateDetail;
        }


        public List<EstimateDetail> SearchEstimateDetails(string keyword)
        {
            List<EstimateDetail> results = new List<EstimateDetail>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM EstimateDetail WHERE ItemName LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EstimateDetail estimateDetail = new EstimateDetail
                        {
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

                        results.Add(estimateDetail);
                    }
                }
            }

            return results;
        }

    }

}
