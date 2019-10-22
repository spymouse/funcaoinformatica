var lstBeneficiarioInclusao = [];
var beneficiarioEditar = -1;

$(document).ready(function () {

    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #Cpf').val(obj.Cpf);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        if (validarCPF($('#Cpf').val())) {
            $.ajax({
                url: urlPost,
                method: "POST",
                data: {
                    "NOME": $(this).find("#Nome").val(),
                    "CEP": $(this).find("#CEP").val(),
                    "Email": $(this).find("#Email").val(),
                    "Sobrenome": $(this).find("#Sobrenome").val(),
                    "Nacionalidade": $(this).find("#Nacionalidade").val(),
                    "Estado": $(this).find("#Estado").val(),
                    "Cidade": $(this).find("#Cidade").val(),
                    "Logradouro": $(this).find("#Logradouro").val(),
                    "Telefone": $(this).find("#Telefone").val(),
                    "Cpf": removerCaracteresEspeciais($(this).find("#Cpf").val()),
                    "LstBeneficiarios": lstBeneficiarioInclusao
                },
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        
                        $("#formCadastro")[0].reset();
                        window.location.href = urlRetorno;
                        ModalDialog("Sucesso!", r);
                    }
            });
        } else {
            ModalDialog('Cliente', 'Algo errado não esta certo: CPF');
        }
    })

    $('#btnIncluirBeneficiario').click(function (e) {
        e.preventDefault();

        if (validarCPF($('#CpfBeneficiario').val())) {

            var objBeneficiario = new Object();
            objBeneficiario.Nome = $('#NomeBeneficiario').val();
            objBeneficiario.Cpf = removerCaracteresEspeciais($('#CpfBeneficiario').val());

            if (beneficiarioEditar != -1) {
                lstBeneficiarioInclusao[beneficiarioEditar].Nome = objBeneficiario.Nome;
                lstBeneficiarioInclusao[beneficiarioEditar].Cpf = objBeneficiario.Cpf;
                lstBeneficiarioInclusao[beneficiarioEditar].Acao = 'A';
            }

            if (beneficiarioEditar == -1) {
                for (var i = 0; i < lstBeneficiarioInclusao.length; i++) {
                    if (lstBeneficiarioInclusao[i].Cpf == objBeneficiario.Cpf) {
                        ModalDialog('Beneficiario', 'CPF Duplicado');
                        return;
                    }
                }
                objBeneficiario.Acao = 'N';
                lstBeneficiarioInclusao.push(objBeneficiario);
            }

            CarregarDados();

            $('#NomeBeneficiario').val('');
            $('#CpfBeneficiario').val('');
            beneficiarioEditar = -1;

        } else {
            ModalDialog('Beneficiario', 'Algo errado não esta certo: CPF');
        }

        console.log(lstBeneficiarioInclusao);

    })

    $('#btnPopupBeneficiarios').click(function (e) {
        $('#modalBeneficiario').modal('show');
        $('#CpfBeneficiario').val('');
        $('#NomeBeneficiario').val('');
    });

    geturlBeneficiaroCliente();
})

function geturlBeneficiaroCliente() {
    $.ajax({
        url: urlBeneficiaroClienteList,
        method: "POST",
        data: {
            "IdCliente": obj.Id,
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (data) {
                console.log(data.Records);
                lstBeneficiarioInclusao = data.Records;
                CarregarDados();
            }
    });
}

function CarregarDados() {
    $('#thBeneficiarioInclusao').html('');
    let bd = $('#thBeneficiarioInclusao');
    var i = 0;
    $.each(lstBeneficiarioInclusao, function (key, entry) {
        if (entry.Acao != 'E') {
            bd.append('<tr>');
            bd.append($('<td>' + entry.Cpf + '</td>'));
            bd.append($('<td>' + entry.Nome + '</td>'));
            bd.append($('<td><button class="btn btn-primary btn-sm" onclick="return AlterarBeneficiario(' + i + ');">Alterar</button></td>'));
            bd.append($('<td><button class="btn btn-primary btn-sm" onclick="return ExcluirBeneficiario(' + i + ');">Excluir</button></td>'));
            bd.append('</tr>');
        }
        i++;
    });
}

function ExcluirBeneficiario(index) {
    lstBeneficiarioInclusao[index].Acao = 'E';
    CarregarDados();
    beneficiarioEditar = -1;
}

function AlterarBeneficiario(index) {
    $('#CpfBeneficiario').val(lstBeneficiarioInclusao[index].Cpf);
    $('#NomeBeneficiario').val(lstBeneficiarioInclusao[index].Nome);
    beneficiarioEditar = index;
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function validarCPF(cpf_) {
    let cpf = '';
    cpf = removerCaracteresEspeciais(cpf_);
    if (cpf == '') return false;
    // Elimina CPFs invalidos conhecidos	
    if (cpf.length != 11 ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999")
        return false;
    // Valida 1o digito	
    add = 0;
    for (i = 0; i < 9; i++)
        add += parseInt(cpf.charAt(i)) * (10 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(9)))
        return false;
    // Valida 2o digito	
    add = 0;
    for (i = 0; i < 10; i++)
        add += parseInt(cpf.charAt(i)) * (11 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(10)))
        return false;
    return true;
}

function removerCaracteresEspeciais(texto_) {
    let _texto = '';
    _texto = texto_.replace(/[^\d]+/g, '');
    return _texto
}