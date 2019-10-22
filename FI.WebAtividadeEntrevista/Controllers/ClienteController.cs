﻿using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

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

            if (bo.VerificarExistencia(model.Cpf))
            {
                return Json("CPF ja existente :D");
            }
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

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
                    Cpf = model.Cpf
                });

                if(model.LstBeneficiarios.Count > 0)
                {
                    BoBeneficiarios boBeneficiarios = new BoBeneficiarios();
                    for (int i = 0; i < model.LstBeneficiarios.Count; i++)
                    {
                        boBeneficiarios.Incluir(new Beneficiarios() { 
                        Cpf = model.LstBeneficiarios[i].Cpf,
                         Nome = model.LstBeneficiarios[i].Nome,
                         IdCliente = model.Id
                        });
                    }
                }
                
           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                FI.AtividadeEntrevista.DML.Cliente cliente = bo.Consultar(model.Id);
                if (cliente.Cpf != model.Cpf && bo.VerificarExistencia(model.Cpf))
                {
                    List<string> erros = new List<string>();
                    erros.Add("CPF ja existente :D");
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }

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
                    Cpf = model.Cpf
                });

                if (model.LstBeneficiarios.Count > 0)
                {
                    BoBeneficiarios boBeneficiarios = new BoBeneficiarios();
                    for (int i = 0; i < model.LstBeneficiarios.Count; i++)
                    {
                        Beneficiarios dados = new Beneficiarios()
                        {
                            Cpf = model.LstBeneficiarios[i].Cpf,
                            Nome = model.LstBeneficiarios[i].Nome,
                            IdCliente = model.Id
                        };
                        switch (model.LstBeneficiarios[i].Acao)
                        {
                            
                            case 'N':
                                boBeneficiarios.Incluir(dados); 
                                break;
                            case 'A':
                                boBeneficiarios.Alterar(dados);
                                break;
                            case 'E':
                                boBeneficiarios.Excluir(model.LstBeneficiarios[i].Id);
                                break;
                            default:
                                break;
                        }
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
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
                    Cpf = cliente.Cpf
                };

            
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


        [HttpPost]
        public JsonResult BeneficiaroClienteList(long IdCliente)
        {
            try
            {
                List<Beneficiarios> beneficiarios = new BoBeneficiarios().Listar(IdCliente);

                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = beneficiarios.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}