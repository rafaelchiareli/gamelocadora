PRAGMA foreign_keys = ON;
BEGIN;
CREATE TABLE Jogo (
 Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
 Titulo TEXT NOT NULL CHECK(length(trim(Titulo)) BETWEEN 1 AND 120),
 Plataforma TEXT NOT NULL CHECK(length(trim(Plataforma)) BETWEEN 1 AND 40),
 Genero TEXT NOT NULL CHECK(length(trim(Genero)) BETWEEN 1 AND 40),
 ValorDiariaCentavos INTEGER NOT NULL CHECK(ValorDiariaCentavos BETWEEN 1 AND 100000),
 CapaArquivo TEXT NULL,
 Ativo BOOLEAN NOT NULL CHECK(Ativo IN (0,1))
);
CREATE TABLE Cliente (
 Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
 Nome TEXT NOT NULL CHECK(length(trim(Nome)) BETWEEN 1 AND 120),
 Email TEXT NOT NULL CHECK(length(Email) BETWEEN 1 AND 150),
 Telefone TEXT NOT NULL CHECK(length(Telefone) BETWEEN 1 AND 30),
 Ativo BOOLEAN NOT NULL CHECK(Ativo IN (0,1))
);
CREATE TABLE Locacao (
 Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
 ClienteId INTEGER NOT NULL,
 JogoId INTEGER NOT NULL,
 DataLocacao DATETIME NOT NULL,
 DataPrevistaDevolucao DATETIME NOT NULL,
 DataDevolucao DATETIME NULL,
 ValorDiariaCentavos INTEGER NOT NULL CHECK(ValorDiariaCentavos > 0),
 ValorTotalCentavos INTEGER NULL CHECK(ValorTotalCentavos IS NULL OR ValorTotalCentavos > 0),
 FOREIGN KEY(ClienteId) REFERENCES Cliente(Id) ON DELETE RESTRICT,
 FOREIGN KEY(JogoId) REFERENCES Jogo(Id) ON DELETE RESTRICT,
 CHECK(DataPrevistaDevolucao > DataLocacao),
 CHECK(DataDevolucao IS NULL OR DataDevolucao >= DataLocacao),
 CHECK((DataDevolucao IS NULL AND ValorTotalCentavos IS NULL)
    OR (DataDevolucao IS NOT NULL AND ValorTotalCentavos IS NOT NULL))
);
CREATE INDEX IX_Locacao_ClienteId ON Locacao(ClienteId);
CREATE UNIQUE INDEX UX_Locacao_JogoAberto ON Locacao(JogoId)
 WHERE DataDevolucao IS NULL;
COMMIT;
