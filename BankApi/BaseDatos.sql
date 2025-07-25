IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Personas] (
    [Id] uniqueidentifier NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Genero] nvarchar(max) NOT NULL,
    [Edad] int NOT NULL,
    [Identificacion] nvarchar(max) NOT NULL,
    [Direccion] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [ClienteId] uniqueidentifier NULL,
    [Contraseña] nvarchar(max) NULL,
    [Estado] int NULL,
    CONSTRAINT [PK_Personas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Cuentas] (
    [Id] uniqueidentifier NOT NULL,
    [NumeroCuenta] int NOT NULL,
    [TipoCuenta] nvarchar(max) NOT NULL,
    [SaldoInicial] decimal(18,2) NOT NULL,
    [Estado] nvarchar(max) NOT NULL,
    [ClienteId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Cuentas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cuentas_Personas_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Personas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Movimientos] (
    [Id] uniqueidentifier NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [TipoMovimiento] nvarchar(max) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Saldo] decimal(18,2) NOT NULL,
    [CuentaId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Movimientos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Movimientos_Cuentas_CuentaId] FOREIGN KEY ([CuentaId]) REFERENCES [Cuentas] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Cuentas_ClienteId] ON [Cuentas] ([ClienteId]);
GO

CREATE INDEX [IX_Movimientos_CuentaId] ON [Movimientos] ([CuentaId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722102339_InitialCreate', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722110300_IncludedMissingKeyDecorator', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personas]') AND [c].[name] = N'Telefono');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Personas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Personas] ALTER COLUMN [Telefono] int NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cuentas]') AND [c].[name] = N'Estado');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Cuentas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Cuentas] ALTER COLUMN [Estado] nvarchar(1) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722115651_ModifiedPropertiesOnPersonaAndCuenta', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250723215847_IncludedAutoGenerationOfGUID', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personas]') AND [c].[name] = N'Estado');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Personas] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Personas] ALTER COLUMN [Estado] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250723235507_IncludedMissingConfigOnCliente', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250724095911_IncludedModelValidation', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Personas]') AND [c].[name] = N'ClienteId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Personas] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Personas] DROP COLUMN [ClienteId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250724155615_RemovedUnnecessaryClientIdAttribute', N'7.0.0');
GO

COMMIT;
GO

