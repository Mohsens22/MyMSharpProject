-- Apartments Table ========================
CREATE TABLE Apartments (
    Id uniqueidentifier PRIMARY KEY NONCLUSTERED,
    Name nvarchar(200)  NOT NULL,
    BuildTime datetime2  NULL
);

