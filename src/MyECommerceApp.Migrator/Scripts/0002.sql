GO

CREATE TABLE $schema$.[Clients] (
    [ClientId] UNIQUEIDENTIFIER NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [Address] nvarchar(500) NOT NULL
    CONSTRAINT [PK_Clients] PRIMARY KEY ([ClientId])
);

GO