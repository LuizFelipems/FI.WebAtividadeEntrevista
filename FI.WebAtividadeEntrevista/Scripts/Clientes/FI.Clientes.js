$(document).ready(function () {
    $('#CPF, #CPFBeneficiarioIncluir').mask('000.000.000-00', { reverse: true });


    $('#IncluirBeneficiario').on('click', function () {
        IncluirBeneficiario($('#CPFBeneficiarioIncluir').val(), $('#NomeBeneficiarioIncluir').val());
    });

    $(document).on('click', '#AlterarBeneficiario', function () {
        cpfAnterior = $(this).data('cpf');
        const nome = $(this).data('nome');

        $('#CPFBeneficiarioIncluir').val(cpfAnterior);
        $('#NomeBeneficiarioIncluir').val(nome);
    });

    $(document).on('click', '#ExcluirBeneficiario', function () {
        $(this).closest('tr').remove();
    });

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

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
                "CPF": $(this).find("#CPF").val(),
                "Beneficiarios": BuscarBeneficiarios()
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
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                    $('#tableBeneficiario tbody').empty();
                }
        });
    })

});


let cpfAnterior = '';

function BuscarBeneficiarios() {
    const listaBeneficiarios = [];

    $('#tableBeneficiario tbody tr').each(function () {
        const cpf = $(this).find('td.cpf').text().trim();
        const nome = $(this).find('td.nome').text().trim();

        listaBeneficiarios.push({ cpf, nome });
    });

    return listaBeneficiarios;
}

function IncluirBeneficiario(cpf, beneficiario) {
    const cpfExisteGrid = $('#tableBeneficiario tbody tr td.cpf').filter(function () {
        return $(this).text() == cpf
    }).length > 0;

    if ((cpf != undefined && cpf != "" && cpf != null) && (beneficiario != undefined && beneficiario != "" && beneficiario != null)) {
        if (!cpfExisteGrid) {
            $('#tableBeneficiario tbody').append('<tr>' +
                '<td class="col-md-4 cpf">' + cpf + '</td>' +
                '<td class="col-md-5 nome">' + beneficiario + '</td>' +
                '<td class="col-md-3" style="display: flex; gap: 5px;">' +
                '<button type="button" class="btn btn-primary" id="AlterarBeneficiario" data-cpf="' + cpf + '" data-nome="' + beneficiario + '">Alterar</button>' +
                '<button type="button" class="btn btn-primary" id="ExcluirBeneficiario" data-cpf="' + cpf + '">Excluir</button>' +
                '</td>' +
                '</tr>');
        }
        else {
            const linhaAlterar = $(`#tableBeneficiario tbody tr`).filter(function () {
                return $(this).find('td.cpf').text().trim() === cpfAnterior;
            });

            linhaAlterar.find('td.cpf').text(cpf);
            linhaAlterar.find('td.nome').text(beneficiario);

            linhaAlterar.find('#AlterarBeneficiario').data('cpf', cpf).data('nome', beneficiario);
            linhaAlterar.find('#ExcluirBeneficiario').data('cpf', cpf);

            cpfAnterior = '';
        }

        $('#CPFBeneficiarioIncluir').val(null);
        $('#NomeBeneficiarioIncluir').val(null);
    }
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
