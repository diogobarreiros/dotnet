CREATE PROCEDURE [dbo].[InsertCliente]
	@NomeCliente varchar(max),
	@Email varchar(max)
AS
begin
	insert into CLIENTES(NomeCliente,Email) 
	values (@NomeCliente,@Email);
end
