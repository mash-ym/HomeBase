﻿using System;
using System.Data;
using System.Data.SQLite;

namespace HomeBase
{
    public class DBInitializer
    {
        private DBManager _dbManager;

        public DBInitializer(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InitializeDatabase()
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            {
                _dbManager.BeginTransaction();

                try
                {
                    CreateTables(connection);
                    CreateRepositories(connection);

                    _dbManager.CommitTransaction();
                }
                catch
                {
                    _dbManager.RollbackTransaction();
                    throw;
                }
            }
        }

        public void CreateTables(SQLiteConnection connection)
        {
            CreateCustomerInfoTable(connection);
            CreateBuildingInfoTable(connection);
            CreateConstructionTable(connection);
            CreateEstimateTable(connection);
            CreateEstimateDetailTable(connection);
            CreateRequestInfoTable(connection);
            CreateScheduleTable(connection);
            CreateSubcontractorTable(connection);
            CreateProjectTable(connection);
        }



        private void CreateRepositories(SQLiteConnection connection)
        {
            CustomerInfoRepository customerRepository = new CustomerInfoRepository(_dbManager);
            BuildingInfoRepository buildingRepository = new BuildingInfoRepository(_dbManager);
            ConstructionRequestRepository constructionRequestRepository = new ConstructionRequestRepository(_dbManager);
            EstimateRepository estimateRepository = new EstimateRepository(_dbManager);
            EstimateDetailRepository estimateDetailRepository = new EstimateDetailRepository(_dbManager);
            RequestInfoRepository requestInfoRepository = new RequestInfoRepository(_dbManager);
            ScheduleRepository scheduleRepository = new ScheduleRepository(_dbManager);
            SubcontractorRepository subcontractorRepository = new SubcontractorRepository(_dbManager);
            ProjectRepository projectRepository = new ProjectRepository(_dbManager);
        }


        private void CreateCustomerInfoTable(SQLiteConnection connection)
        {
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

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateBuildingInfoTable(SQLiteConnection connection)
        {
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

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateConstructionTable(SQLiteConnection connection)
        {
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

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateEstimateTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Estimate (
                    EstimateId INTEGER PRIMARY KEY AUTOINCREMENT,
                    SiteName TEXT,
                    ProjectId INTEGER,
                    SiteAddress TEXT,
                    CreatedAt DATETIME,
                    RequestInfoId INTEGER,
                    CustomerInfoId INTEGER,
                    BuildingInfoId INTEGER,
                    IssueDate DATETIME,
                    CreatorId INTEGER,
                    TotalAmount DECIMAL,
                    DueDate DATETIME,
                    ChangeHistory TEXT,
                    DeliveryLocation TEXT,
                    DrawingPdf BLOB,
                    FOREIGN KEY (ProjectId) REFERENCES Project (ProjectId)
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }


        private void CreateEstimateDetailTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS EstimateDetail (
                    EstimateDetailID INTEGER PRIMARY KEY AUTOINCREMENT,
                    EstimateID INTEGER,
                    ItemName TEXT,
                    Quantity INTEGER,
                    UnitPrice REAL,
                    Amount REAL,
                    FOREIGN KEY (EstimateID) REFERENCES Estimate (EstimateID)
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateRequestInfoTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS RequestInfo (
                    RequestInfoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerName TEXT,
                    CustomerAddress TEXT,
                    RequestDate DATE,
                    RequestContent TEXT,
                    CreatorID INTEGER,
                    FOREIGN KEY (CreatorID) REFERENCES Creator (CreatorID)
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateScheduleTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Schedule (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProjectId INTEGER,
                    SiteName TEXT,
                    SiteDuration TEXT,
                    GroupName TEXT,
                    StartDate DATETIME,
                    EndDate DATETIME,
                    WorkHours DECIMAL,
                    SubcontractorId INTEGER,
                    FOREIGN KEY (ProjectId) REFERENCES Project (ProjectId)
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }


        private void CreateSubcontractorTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Subcontractor (
                    SubcontractorID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    ContactPerson TEXT,
                    PhoneNumber TEXT,
                    EmailAddress TEXT
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }

        private void CreateProjectTable(SQLiteConnection connection)
        {
            string createQuery = @"
                CREATE TABLE IF NOT EXISTS Project (
                    ProjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProjectName TEXT,
                    StartDate TEXT,
                    EndDate TEXT,
                    Status TEXT
                );
            ";

            ExecuteNonQuery(connection, createQuery);
        }

        private void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
