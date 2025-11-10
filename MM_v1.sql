-- =========================================
-- Database: MarathonManagement
-- SQL Server Script with Soft Delete
-- =========================================

USE [master];
GO

CREATE DATABASE [MarathonManagement_v1];
GO

USE [MarathonManagement_v1];
GO

-- =========================================
-- 1. Roles
-- =========================================
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL
);
GO

INSERT INTO Roles (RoleName)
VALUES ('Admin'), ('Organizer'), ('Runner');
GO

-- =========================================
-- 2. Users
-- =========================================
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FullName NVARCHAR(255),
    RoleId INT NOT NULL,
    RefreshToken NVARCHAR(MAX) DEFAULT NULL,
    RefreshTokenExpiry DATETIME DEFAULT SYSDATETIME(),
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    IsActive BIT DEFAULT 1,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId)
);
GO

-- =========================================
-- 3. Organizers
-- =========================================
CREATE TABLE Organizers (
    OrganizerId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    OrganizationName NVARCHAR(255) NOT NULL,
    Verified BIT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Organizers_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);
GO

-- =========================================
-- 4. Marathons
-- =========================================
CREATE TABLE Marathons (
    MarathonId INT IDENTITY(1,1) PRIMARY KEY,
    OrganizerId INT NOT NULL,
    MarathonName NVARCHAR(255) NOT NULL,
    ThumbnailLink NVARCHAR(255) DEFAULT 'https://static.vecteezy.com/system/resources/previews/025/681/161/non_2x/marathon-running-continuous-one-line-drawing-woman-run-vector.jpg', 
    Description NVARCHAR(MAX),
    Location NVARCHAR(255),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    RegistrationFee DECIMAL(10,2) NOT NULL CHECK (RegistrationFee >= 0),
    MaxParticipants INT,
    Status NVARCHAR(50) DEFAULT 'Upcoming',
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Marathons_Organizers FOREIGN KEY (OrganizerId)
        REFERENCES Organizers(OrganizerId)
);
GO

-- =========================================
-- 5. Checkpoints
-- =========================================
CREATE TABLE Checkpoints (
    CheckpointId INT IDENTITY(1,1) PRIMARY KEY,
    MarathonId INT NOT NULL,
    Name NVARCHAR(100),
    Latitude DECIMAL(9,6) NOT NULL,
    Longitude DECIMAL(9,6) NOT NULL,
    Sequence INT NOT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Checkpoints_Marathons FOREIGN KEY (MarathonId)
        REFERENCES Marathons(MarathonId)
);
GO

-- =========================================
-- 6. Payments
-- =========================================
CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MarathonId INT NOT NULL,
    VNPayTransactionId NVARCHAR(100),
    Amount DECIMAL(10,2) NOT NULL,
    PaymentStatus NVARCHAR(50) DEFAULT 'Pending',
    PaymentDate DATETIME2 DEFAULT SYSDATETIME(),
    PaymentMethod NVARCHAR(50) DEFAULT 'MoMo',
    ResponseCode NVARCHAR(20),
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Payments_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId),
    CONSTRAINT FK_Payments_Marathons FOREIGN KEY (MarathonId)
        REFERENCES Marathons(MarathonId)
);
GO

-- =========================================
-- 7. Registrations
-- =========================================
CREATE TABLE Registrations (
    RegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MarathonId INT NOT NULL,
    PaymentId INT NULL,
    RegisteredAt DATETIME2 DEFAULT SYSDATETIME(),
    Status NVARCHAR(50) DEFAULT 'Pending',
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Registrations_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId),
    CONSTRAINT FK_Registrations_Marathons FOREIGN KEY (MarathonId)
        REFERENCES Marathons(MarathonId),
    CONSTRAINT FK_Registrations_Payments FOREIGN KEY (PaymentId)
        REFERENCES Payments(PaymentId),
    CONSTRAINT UQ_Registration_UserMarathon UNIQUE (UserId, MarathonId)
);
GO

-- =========================================
-- 8. Audit Logs
-- =========================================
CREATE TABLE AuditLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    ActorId INT NOT NULL,
    Action NVARCHAR(255) NOT NULL,
    Timestamp DATETIME2 DEFAULT SYSDATETIME(),
    TargetEntity NVARCHAR(100),
    TargetId INT,
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (ActorId)
        REFERENCES Users(UserId)
);
GO

-- =========================================
-- 1️⃣ Roles (already inserted)
-- =========================================
-- Admin, Organizer, Runner already exist

-- =========================================
-- 2️⃣ Users
-- =========================================
INSERT INTO Users (Username, PasswordHash, Email, FullName, RoleId, IsActive)
VALUES
('admin', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'admin@gmail.com', 'System Admin', 1, 1),
('organizer01', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'org1@example.com', 'John Organizer', 2, 1),
('organizer02', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'org2@example.com', 'Emily Organizer', 2, 1),
('runner01', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'runner1@example.com', 'Alice Runner', 3, 1),
('runner02', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'runner2@example.com', 'Bob Runner', 3, 1),
('runner03', N'$2a$12$Otm5/qH5wkuCv8WBkNEb/eosl5VEuzzbSFpFCW/wVu3uA/l/dLmt2', 'runner3@example.com', 'Charlie Runner', 3, 1);
GO

-- =========================================
-- 3️⃣ Organizers
-- =========================================
INSERT INTO Organizers (UserId, OrganizationName, Verified)
VALUES
(2, 'City Marathon Club', 1),
(3, 'Elite Sports Events', 1);
GO

-- =========================================
-- 4️⃣ Marathons
-- =========================================
INSERT INTO Marathons (OrganizerId, MarathonName, Description, Location, StartDate, EndDate, RegistrationFee, MaxParticipants, Status)
VALUES
(1, 'Hanoi Half Marathon', 'A scenic marathon through the streets of Hanoi.', 'Hanoi', '2025-12-10', '2025-12-10', 300000, 500, 'Upcoming'),
(2, 'Saigon Night Run', 'Night-time run through the heart of Ho Chi Minh City.', 'Ho Chi Minh City', '2025-12-20', '2025-12-20', 350000, 800, 'Upcoming'),
(2, 'Da Nang Charity Run', 'Run for a cause to support local charities.', 'Da Nang', '2025-11-25', '2025-11-25', 200000, 300, 'Open');
GO

-- =========================================
-- 5️⃣ Checkpoints
-- =========================================
INSERT INTO Checkpoints (MarathonId, Name, Latitude, Longitude, Sequence)
VALUES
(1, 'Checkpoint 1', 21.0278, 105.8342, 1),
(1, 'Checkpoint 2', 21.0301, 105.8350, 2),
(1, 'Checkpoint 3', 21.0320, 105.8370, 3),
(2, 'Checkpoint 1', 10.7769, 106.7009, 1),
(2, 'Checkpoint 2', 10.7800, 106.7100, 2),
(2, 'Checkpoint 3', 10.7825, 106.7200, 3),
(3, 'Checkpoint 1', 16.0471, 108.2060, 1),
(3, 'Checkpoint 2', 16.0540, 108.2200, 2);
GO

-- =========================================
-- 6️⃣ Payments
-- =========================================
INSERT INTO Payments (UserId, MarathonId, VNPayTransactionId, Amount, PaymentStatus, PaymentMethod, ResponseCode)
VALUES
(4, 1, 'TXN001', 300000, 'Completed', 'MoMo', '00'),
(5, 2, 'TXN002', 350000, 'Completed', 'MoMo', '00'),
(6, 3, 'TXN003', 200000, 'Pending', 'MoMo', '99');
GO

-- =========================================
-- 7️⃣ Registrations
-- =========================================
INSERT INTO Registrations (UserId, MarathonId, PaymentId, Status)
VALUES
(4, 1, 1, 'Confirmed'),
(5, 2, 2, 'Confirmed'),
(6, 3, 3, 'Pending');
GO

-- =========================================
-- 8️⃣ Audit Logs
-- =========================================
INSERT INTO AuditLogs (ActorId, Action, TargetEntity, TargetId)
VALUES
(1, 'Created Marathon', 'Marathon', 1),
(2, 'Verified Organizer', 'Organizer', 1),
(1, 'Approved Payment', 'Payment', 1),
(3, 'Created Marathon', 'Marathon', 2);
GO
