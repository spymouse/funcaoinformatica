﻿CREATE PROC FI_SP_ConsDelBeneficiarioCliente
	@ID BIGINT
AS
BEGIN
	DELETE BENEFICIARIOS WHERE ID = @ID
END