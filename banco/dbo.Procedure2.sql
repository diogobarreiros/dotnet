﻿CREATE PROCEDURE [dbo].[SelectClientesProdutos]
AS
begin
	SELECT * FROM CLIENTES;
	SELECT * FROM PRODUTOS;
end
