$(document).ready(function () {
    getSolicitudesTesoreriaPendianteTranferencia();
});


function getSolicitudesTesoreriaPendianteTranferencia() {
    var empObj = {
        Status: 38,
        Tipo: 'tesoreria',
        Filtro: ' '
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/TesoreriaPendienteTranferencia/GetTesoreriaPendianteTranferencia",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += '<tr id="fila-' + item.Id +'" onclick=selecionaFila('+item.Id+')>';
                    html += '<td>' + item.Id + '</td>';
                    html += '<td>' + item.Cedula + '</td>';
                    html += '<td>' + item.Nombre + '</td>';
                    html += '<td>' + item.Producto + '</td>';
                    html += '<td>' + item.Monto + '</td>';
                    html += '<td>' + item.Banco + '</td>';
                    html += '<td style="width: 150px;">' + item.Cuenta + '</td>';
                    html += '<td>' + item.Sinpe + '</td>';
                    html += '</tr>';
                });
                $("#tableTesoreriaPendienteTranferencia").dataTable().fnClearTable();
                $("#tableTesoreriaPendienteTranferencia").dataTable().fnDestroy();
                $('#tblTesoreriaPendienteTranferencia').html(html);

                $("#tableTesoreriaPendienteTranferencia").dataTable();
                $("select[name*='tblTesoreriaPendienteTranferencia_length']").change();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function accion() {
    //dependiendo de cual item esta selccionado en el dropdown llamamos al metodo de la accion
    var idSolicitud = $('#lbAccion').text().replace('Solicitud id: ',''); 
    var accion = $('#ddAccion option:selected').val();
    switch (accion) {
        case '0':
            $('#mensajeAccion').text("Debe seleccionar una accion en el Drop Down");
            $('#mensajeAccion').addClass('alert-success');
            $('#mensajeAccion').css("display", "block");
            break;
        case '1':
            aprobar(idSolicitud);
            break;
        case '2':
            verificar(idSolicitud);
            break;
        default:
            $('#mensajeAccion').text("Debe seleccionar una accion en el Drop Down");
            $('#mensajeAccion').addClass('alert-success');
            $('#mensajeAccion').css("display", "block");
            break;
    }

}

//Funciones del mantenimiento
function aprobar(id) {
    var empObj = {
        id: id
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            
            url: "/TesoreriaPendienteTranferencia/AprobarSolicitud",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                getSolicitudesTesoreriaPendianteTranferencia();
                $('#mensajeAccion').text(result);
                $('#mensajeAccion').addClass('alert-success');
                $('#mensajeAccion').css("display", "block");
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function verificar(id) {
    var empObj = {
        id: id
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/TesoreriaPendienteTranferencia/VerificarCuenta",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                getSolicitudesTesoreriaPendianteTranferencia();
                $('#mensajeAccion').text(result);
                $('#mensajeAccion').addClass('alert-success');
                $('#mensajeAccion').css("display", "block");            
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}

//funcion para selecionar las columnas
function selecionaFila(id) {
    if ($('#fila-' + id).hasClass('ui-selected')) {
        $('#tblTesoreriaPendienteTranferencia tr').removeClass('ui-selected');
        $('#controles-tesoreria').addClass('hide');
    } else {
        $('#tblTesoreriaPendienteTranferencia tr').removeClass('ui-selected');
        $('#fila-' + id).addClass('ui-selected');
        $('#lbAccion').text('Solicitud id: ' + id);
        $('#ddAccion').val(0);
        $('#controles-tesoreria').removeClass('hide');
    }

   
   
}
