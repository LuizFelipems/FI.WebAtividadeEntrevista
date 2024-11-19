using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Incluir(beneficiario);
        }

        /// <summary>
        /// Lista os beneficiarios do cliente
        /// </summary>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Listar(idCliente);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id do cliente
        /// </summary>
        /// <param name="idCliente">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.Excluir(idCliente);
        }
    }
}
