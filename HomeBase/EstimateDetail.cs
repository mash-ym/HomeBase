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
        public int DetailId { get; set; }        // ID (detail_id) [Primary Key]
        public int EstimateId { get; set; }      // 見積番号 (estimate_id) [Foreign Key]
        public string ItemName { get; set; }     // 項目名 (item_name)
        public decimal CostQuantity { get; set; }     // 原価数量 (cost_quantity)
        public string CostUnit { get; set; }     // 原価単位 (cost_unit)
        public decimal CostUnitPrice { get; set; }    // 原価単価 (cost_unit_price)
        public decimal EstimateQuantity { get; set; }   // 見積数量 (estimate_quantity)
        public string EstimateUnit { get; set; }   // 見積単位 (estimate_unit)
        public decimal EstimateUnitPrice { get; set; }  // 見積単価 (estimate_unit_price)
        public string Specification { get; set; }  // 仕様 (specification)
        public decimal MarkupRate { get; set; }    // 掛率 (markup_rate)
        public string Remarks { get; set; }        // 備考 (remarks)
        public decimal WorkHours { get; set; }     // 工数 (work_hours)
        public int SubcontractorId { get; set; }   // 下請け業者ID (subcontractor_id)
        public int GroupNumber { get; set; }       // グループ番号 (group_number)
        public string GroupName { get; set; }      // グループ名 (group_name)
        public string Dependency { get; set; }     // 依存関係 (dependency)
    }
    public class EstimateDetailRepository
    {
        private string connectionString;

        public EstimateDetailRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateEstimateDetail(EstimateDetail estimateDetail)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO EstimateDetail (detail_id, estimate_id, item_name, cost_quantity, cost_unit, 
                            cost_unit_price, estimate_quantity, estimate_unit, estimate_unit_price, specification, 
                            markup_rate, remarks, work_hours, subcontractor_id, group_number, group_name, dependency)
                            VALUES (@DetailId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, 
                            @EstimateQuantity, @EstimateUnit, @EstimateUnitPrice, @Specification, @MarkupRate, 
                            @Remarks, @WorkHours, @SubcontractorId, @GroupNumber, @GroupName, @Dependency)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetailId", estimateDetail.DetailId);
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

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
    }
}
