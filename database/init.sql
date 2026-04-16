USE[CMS_Database];
GO

-- 1) Roles
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Roles') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Roles
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
  );
END
GO

-- Ensure basic roles
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'User') INSERT INTO dbo.Roles(RoleName) VALUES('User');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'Admin') INSERT INTO dbo.Roles(RoleName) VALUES('Admin');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'SuperAdmin') INSERT INTO dbo.Roles(RoleName) VALUES('SuperAdmin');
GO

-- 2) ContractStatuses
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.ContractStatuses') AND type = N'U')
BEGIN
  CREATE TABLE dbo.ContractStatuses
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
  );
  INSERT INTO dbo.ContractStatuses(StatusName) VALUES('NotStarted'),('Active'),('Finished');
END
GO

-- 3) Companies
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Companies') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Companies
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(250) NOT NULL,
    Address NVARCHAR(500) NULL,
    TaxNumber NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );
  CREATE INDEX IX_Companies_Name ON dbo.Companies(Name);
END
GO

-- 4) Employees
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Employees') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Employees
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NULL,
    FirstName NVARCHAR(150) NOT NULL,
    LastName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(250) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );
  ALTER TABLE dbo.Employees
    ADD CONSTRAINT FK_Employees_Companies FOREIGN KEY (CompanyId) REFERENCES dbo.Companies(Id) ON DELETE NO ACTION;
  CREATE INDEX IX_Employees_CompanyId ON dbo.Employees(CompanyId);
END
GO

-- 5) Contracts
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Contracts') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Contracts
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL,
    EmployeeId INT NOT NULL,
    ContractStatusId INT NOT NULL,
    Position NVARCHAR(200) NULL,
    Description NVARCHAR(MAX) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Wage DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );
  ALTER TABLE dbo.Contracts
    ADD CONSTRAINT FK_Contracts_Companies FOREIGN KEY (CompanyId) REFERENCES dbo.Companies(Id) ON DELETE NO ACTION;
  ALTER TABLE dbo.Contracts
    ADD CONSTRAINT FK_Contracts_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(Id) ON DELETE NO ACTION;
  ALTER TABLE dbo.Contracts
    ADD CONSTRAINT FK_Contracts_Statuses FOREIGN KEY (ContractStatusId) REFERENCES dbo.ContractStatuses(Id) ON DELETE NO ACTION;
  CREATE INDEX IX_Contracts_EmployeeId ON dbo.Contracts(EmployeeId);
  CREATE INDEX IX_Contracts_CompanyId ON dbo.Contracts(CompanyId);
  CREATE INDEX IX_Contracts_StartDate ON dbo.Contracts(StartDate);
END
GO

-- 6) Users
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Users') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Users
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    RoleId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );

  ALTER TABLE dbo.Users
    ADD CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE NO ACTION;
   
  CREATE INDEX IX_Users_UserName ON dbo.Users(UserName);
END
GO

-- Junction: allows many-to-many, prevents duplicate pairs
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.UserEmployees') AND type = N'U')
BEGIN
  CREATE TABLE dbo.UserEmployees
  (
    UserId INT NOT NULL,
    EmployeeId INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_UserEmployees PRIMARY KEY (UserId, EmployeeId)
  );

  ALTER TABLE dbo.UserEmployees
    ADD CONSTRAINT FK_UserEmployees_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE;

  ALTER TABLE dbo.UserEmployees
    ADD CONSTRAINT FK_UserEmployees_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(Id) ON DELETE CASCADE;

  CREATE INDEX IX_UserEmployees_EmployeeId ON dbo.UserEmployees(EmployeeId);
END
GO

-- Employee + Date idx
CREATE NONCLUSTERED INDEX IX_Contracts_EmployeeId_Start_End
ON dbo.Contracts (EmployeeId, StartDate, EndDate)
INCLUDE (CompanyId, Position, Wage, Id);
GO

-- Company + Date idx
CREATE NONCLUSTERED INDEX IX_Contracts_CompanyId_Start_End
ON dbo.Contracts (CompanyId, StartDate, EndDate)
INCLUDE (EmployeeId, Position, Wage, Id);
GO

-- Startdate idx
CREATE NONCLUSTERED INDEX IX_Contracts_StartDate_Cover
ON dbo.Contracts (StartDate)
INCLUDE (EmployeeId, CompanyId, Id);
GO




USE CMS_Database;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Companies' AND xtype='U')
CREATE TABLE Companies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Address NVARCHAR(MAX) NULL,
    TaxNumber NVARCHAR(50) NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NULL,
    CompanyId INT NULL,
    CONSTRAINT FK_Employees_Companies FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserEmployees' AND xtype='U')
CREATE TABLE UserEmployees (
    UserId INT NOT NULL,
    EmployeeId INT NOT NULL,
    PRIMARY KEY (UserId, EmployeeId),
    CONSTRAINT FK_UserEmployees_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserEmployees_Employees FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE
);

INSERT INTO Companies (Name, Address) VALUES ('IT Solutions Ltd', 'New York');
INSERT INTO Employees (FirstName, LastName, Email, CompanyId)
VALUES ('Ivan', 'Ivanov', 'ivan@test.com', 1);


-- DataBase Creation
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'CmsDb')
BEGIN
    CREATE DATABASE CMS_Database;
END
GO

USE CmsDb;
GO

-- Roles table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Roles') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Roles
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
  );
END
GO

-- Ensure basic roles
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'User') INSERT INTO dbo.Roles(RoleName) VALUES('User');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'Admin') INSERT INTO dbo.Roles(RoleName) VALUES('Admin');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = 'SuperAdmin') INSERT INTO dbo.Roles(RoleName) VALUES('SuperAdmin');
GO

-- Users table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Users') AND type = N'U')
BEGIN
  CREATE TABLE dbo.Users
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    RoleId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
  );

  ALTER TABLE dbo.Users
    ADD CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE NO ACTION;
   
  CREATE INDEX IX_Users_UserName ON dbo.Users(UserName);
END
GO

-- ContractStatuses table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.ContractStatuses') AND type = N'U')
BEGIN
  CREATE TABLE dbo.ContractStatuses
  (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
  );
  INSERT INTO dbo.ContractStatuses(StatusName) VALUES('NotStarted'),('Active'),('Finished');
END
GO

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'User')
    INSERT INTO Roles (RoleName) VALUES ('User');

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Admin')
    INSERT INTO Roles (RoleName) VALUES ('Admin');

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'SuperAdmin')
    INSERT INTO Roles (RoleName) VALUES ('SuperAdmin');
