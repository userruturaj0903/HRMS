CREATE DATABASE WorkSphereHRMS;
USE WorkSphereHRMS;

-- ===========================
-- Roles
-- ===========================

CREATE TABLE Roles
(
    RoleId INT AUTO_INCREMENT PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL
);

INSERT INTO Roles(RoleName)
VALUES
('Admin'),
('HR'),
('Employee');

-- ===========================
-- Departments
-- ===========================

CREATE TABLE Departments
(
    DepartmentId INT AUTO_INCREMENT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
);

-- ===========================
-- Employees
-- ===========================

CREATE TABLE Employees
(
    EmployeeId INT AUTO_INCREMENT PRIMARY KEY,

    EmployeeCode VARCHAR(20) UNIQUE,

    FirstName VARCHAR(100),

    LastName VARCHAR(100),

    Email VARCHAR(100),

    Phone VARCHAR(20),

    Gender VARCHAR(20),

    DateOfBirth DATE,

    JoiningDate DATE,

    DepartmentId INT,

    RoleId INT,

    Designation VARCHAR(100),

    Salary DECIMAL(10,2),

    Address TEXT,

    ProfileImage VARCHAR(255),

    Status VARCHAR(20),

    FOREIGN KEY (DepartmentId)
    REFERENCES Departments(DepartmentId),

    FOREIGN KEY (RoleId)
    REFERENCES Roles(RoleId)
);

-- ===========================
-- Attendance
-- ===========================

CREATE TABLE Attendance
(
    AttendanceId INT AUTO_INCREMENT PRIMARY KEY,

    EmployeeId INT,

    AttendanceDate DATE,

    CheckIn TIME,

    CheckOut TIME,

    Status VARCHAR(20),

    FOREIGN KEY (EmployeeId)
    REFERENCES Employees(EmployeeId)
);

-- ===========================
-- Leave Types
-- ===========================

CREATE TABLE LeaveTypes
(
    LeaveTypeId INT AUTO_INCREMENT PRIMARY KEY,

    LeaveTypeName VARCHAR(50),

    TotalDays INT
);

INSERT INTO LeaveTypes
(LeaveTypeName, TotalDays)
VALUES
('Casual Leave',12),
('Sick Leave',10),
('Earned Leave',15);

-- ===========================
-- Leave Requests
-- ===========================

CREATE TABLE LeaveRequests
(
    LeaveRequestId INT AUTO_INCREMENT PRIMARY KEY,

    EmployeeId INT,

    LeaveTypeId INT,

    FromDate DATE,

    ToDate DATE,

    Reason TEXT,

    Status VARCHAR(20),

    FOREIGN KEY(EmployeeId)
    REFERENCES Employees(EmployeeId),

    FOREIGN KEY(LeaveTypeId)
    REFERENCES LeaveTypes(LeaveTypeId)
);

-- ===========================
-- Users
-- ===========================

CREATE TABLE Users
(
    UserId INT AUTO_INCREMENT PRIMARY KEY,

    Username VARCHAR(100),

    Password VARCHAR(255),

    EmployeeId INT,

    FOREIGN KEY(EmployeeId)
    REFERENCES Employees(EmployeeId)
);















INSERT INTO Departments (DepartmentName)
VALUES ('IT');