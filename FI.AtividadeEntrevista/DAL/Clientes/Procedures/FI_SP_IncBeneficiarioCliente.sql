﻿CREATE PROC FI_SP_IncBeneficiarioCliente
    @CPF		   VARCHAR (11),
    @NOME		   VARCHAR (50),
    @IDCLIENTE     BIGINT
AS
BEGIN
	INSERT INTO BENEFICIARIOS (CPF, NOME, IDCLIENTE) 
	VALUES (@CPF, @NOME, @IDCLIENTE)

	SELECT SCOPE_IDENTITY()
END