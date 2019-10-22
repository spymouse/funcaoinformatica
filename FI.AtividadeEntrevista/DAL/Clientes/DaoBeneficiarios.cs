using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiarios
    /// </summary>
    internal class DaoBeneficiarios : AcessoDados
    {
        /// <summary>
        /// Inclui um novo Beneficiarios
        /// </summary>
        /// <param name="Beneficiarios">Objeto de Beneficiarios</param>
        internal long Incluir(DML.Beneficiarios cliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("NOME", cliente.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cliente.Cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", cliente.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiarioCliente", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Lista todos os Beneficiarios por Cliente
        /// </summary>
        internal List<DML.Beneficiarios> Listar(long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiarioCliente", parametros);
            List<DML.Beneficiarios> cli = Converter(ds);

            return cli;
        }

        /// <summary>
        /// Alterar um Beneficiarios
        /// </summary>
        /// <param name="cliente">Objeto de Beneficiarios</param>
        internal void Alterar(DML.Beneficiarios cliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("NOME", cliente.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cliente.Cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", cliente.IdCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", cliente.Id));

            base.Executar("FI_SP_AltBeneficiarioCliente", parametros);
        }


        /// <summary>
        /// Excluir Beneficiarios
        /// </summary>
        /// <param name="cliente">Objeto Beneficiarios</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", Id));

            base.Executar("FI_SP_ConsDelBeneficiarioCliente", parametros);
        }

        private List<DML.Beneficiarios> Converter(DataSet ds)
        {
            List<DML.Beneficiarios> lista = new List<DML.Beneficiarios>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiarios cli = new DML.Beneficiarios();
                    cli.Id = row.Field<long>("ID");
                    cli.Nome = row.Field<string>("NOME");
                    cli.Cpf = row.Field<string>("CPF");
                    cli.IdCliente = row.Field<long>("IDCLIENTE");
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
