using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using ebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            var cpfExiste = bo.VerificarExistencia(model.CPF);

            if (!this.ModelState.IsValid || cpfExiste)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                if (cpfExiste)
                    erros.Add("Este CPF já possui cadastrado");

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = RemoverMascaraCPF(model.CPF)
                });

                foreach (var bene in model.Beneficiarios ?? new List<BeneficiarioModel>())
                {
                    boBene.Incluir(new Beneficiario()
                    {
                        CPF = RemoverMascaraCPF(bene.CPF),
                        Nome = bene.Nome,
                        IdCliente = model.Id,
                    });
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            var cpfExiste = bo.VerificarExistencia(model.CPF, model.Id);

            if (!this.ModelState.IsValid || cpfExiste)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                if (cpfExiste)
                    erros.Add("Este CPF já possui cadastrado");

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = RemoverMascaraCPF(model.CPF)
                });

                if (model.Beneficiarios != null && model.Beneficiarios.Any())
                {
                    boBene.Excluir(model.Id);

                    foreach (var bene in model.Beneficiarios)
                    {
                        boBene.Incluir(new Beneficiario()
                        {
                            CPF = RemoverMascaraCPF(bene.CPF),
                            Nome = bene.Nome,
                            IdCliente = model.Id,
                        });
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = AdicionarMascaraCPF(cliente.CPF)
                };

                List<Beneficiario> beneficiarios = boBene.Listar(model.Id);

                if (beneficiarios != null && beneficiarios.Any())
                {
                    model.Beneficiarios = new List<BeneficiarioModel>();

                    foreach (var bene in beneficiarios)
                    {
                        model.Beneficiarios.Add(new BeneficiarioModel()
                        {
                            CPF = AdicionarMascaraCPF(bene.CPF),
                            Nome = bene.Nome
                        });
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public static string RemoverMascaraCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return cpf;

            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        public static string AdicionarMascaraCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return cpf;

            return string.Format("{0:000\\.000\\.000-00}", long.Parse(cpf));
        }
    }
}