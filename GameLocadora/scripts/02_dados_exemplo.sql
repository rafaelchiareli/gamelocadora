PRAGMA foreign_keys = ON;
BEGIN;
INSERT INTO Jogo(Titulo,Plataforma,Genero,ValorDiariaCentavos,Ativo)
VALUES ('Super Mario Odyssey','Nintendo Switch','Aventura',1200,1),
       ('Forza Horizon 5','Xbox','Corrida',1500,1),
       ('God of War Ragnarok','PlayStation 5','Acao',1800,1);
INSERT INTO Cliente(Nome,Email,Telefone,Ativo)
VALUES ('Ana Silva','ana@example.com','11999990001',1),
       ('Pedro Souza','pedro@example.com','11999990002',1);
COMMIT;
