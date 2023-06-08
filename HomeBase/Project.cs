using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public Estimate Estimate { get; set; }
        public List<ConstructionRequest> ConstructionRequests { get; set; }
        public List<Estimate> Estimates { get; set; }
        public List<SpecificationDocument> SpecificationDocuments { get; set; }
        public List<Drawing> Drawings { get; set; }
    }
    public class ProjectRepository
    {
        private readonly DBManager _dbManager;

        public ProjectRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertProject(Project project)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO Project (ProjectName, StartDate, EndDate, Status) " +
                                          "VALUES (@ProjectName, @StartDate, @EndDate, @Status)";
                    command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);
                    command.Parameters.AddWithValue("@Status", project.Status);

                    command.ExecuteNonQuery();

                    // プロジェクトIDを取得
                    command.CommandText = "SELECT last_insert_rowid()";
                    int projectId = Convert.ToInt32(command.ExecuteScalar());
                    project.ProjectId = projectId;

                    // プロジェクトに関連する工事依頼を追加
                    InsertConstructionRequests(project.ConstructionRequests, projectId);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                }
            }

        }
        public void UpdateProject(Project project)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE Project SET ProjectName = @ProjectName, StartDate = @StartDate, EndDate = @EndDate, Status = @Status WHERE ProjectId = @ProjectId";
                    command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);
                    command.Parameters.AddWithValue("@Status", project.Status);
                    command.Parameters.AddWithValue("@ProjectId", project.ProjectId);

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
        public void DeleteProject(int projectId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // プロジェクトを削除する前に関連するデータを削除
                    // 例: 関連する見積書を削除するメソッド: DeleteEstimatesByProjectId(projectId)
                    //     関連する修理履歴を削除するメソッド: DeleteRepairHistoriesByProjectId(projectId)
                    //     ...

                    // プロジェクトを削除
                    command.CommandText = "DELETE FROM Project WHERE Id = @ProjectId";
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to delete Project.", ex);
                }
            }
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Project";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project project = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            ProjectName = reader["ProjectName"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"].ToString(),
                            // 他のプロパティを設定する
                        };

                        projects.Add(project);
                    }
                }
            }

            return projects;
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

        public List<Project> SearchProject(string keyword)
        {
            List<Project> projects = new List<Project>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Project WHERE ProjectName LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project project = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            ProjectName = reader["ProjectName"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"].ToString(),
                            // 他のプロパティを設定する
                        };

                        projects.Add(project);
                    }
                }
            }

            return projects;
        }

        private void InsertConstructionRequests(List<ConstructionRequest> constructionRequests, int projectId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                foreach (ConstructionRequest constructionRequest in constructionRequests)
                {
                    try
                    {
                        command.CommandText = "INSERT INTO ConstructionRequest (ProjectId, EstimateId, ItemName, CostQuantity, CostUnit, CostUnitPrice, WorkHours, StartDate, SubcontractorId, SiteContact, SalesContact, DrawingPDF) " +
                                              "VALUES (@ProjectId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPDF)";
                        command.Parameters.AddWithValue("@ProjectId", projectId);
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
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                    }
                }
            }
        }

    }
}
