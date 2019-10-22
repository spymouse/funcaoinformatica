using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiarios
    {
        /// <summary>
        /// Inclui um novo Beneficiario
        /// </summary>
        /// <param name="cliente">Objeto de Beneficiario</param>
        public long Incluir(DML.Beneficiarios cliente)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Lista os Beneficiarios
        /// </summary>
        public List<DML.Beneficiarios> Listar(long IdCliente)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            return cli.Listar(IdCliente);
        }

        public void Alterar(Beneficiarios beneficiarios)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            cli.Alterar(beneficiarios);
        }

        public void Excluir(long id)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            cli.Excluir(id);
        }
    }
}
