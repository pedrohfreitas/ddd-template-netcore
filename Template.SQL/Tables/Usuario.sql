CREATE TABLE [dbo].[Usuario]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[IdPessoa] BIGINT NULL,
    [Login] VARCHAR(80) NOT NULL, 
    [Senha] VARCHAR(80) NOT NULL, 
    [IdUsuarioStatus] INT NOT NULL, 
    [DataCadastro] DATETIME NOT NULL DEFAULT GETDATE(),
    [Imagem] VARBINARY(MAX) NULL, 
    [TokenResetSenha] VARCHAR(500) NULL, 
    CONSTRAINT [FK_Usuario_UsuarioStatus] FOREIGN KEY ([IdUsuarioStatus]) REFERENCES [dbo].[UsuarioStatus] ([Id]),
    CONSTRAINT [FK_Usuario_Pessoa] FOREIGN KEY ([IdPessoa]) REFERENCES [dbo].[Pessoa] ([Id])
)
