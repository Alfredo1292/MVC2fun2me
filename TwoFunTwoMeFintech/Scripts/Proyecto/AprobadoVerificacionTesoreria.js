$(document).ready(function () {

    getSolicitudesTesoreriaPendianteTranferencia();

});

function getSolicitudesTesoreriaPendianteTranferencia() {
    var empObj = {
        Status: 115,
        Status2: 116,
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
            url: "/AprobadoVerificacionTesoreria/GetAprobadoVerificacionTesoreria",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += '<tr>';
                    html += '<td>' + item.Id + '</td>';
                    html += '<td>' + item.Cedula + '</td>';
                    html += '<td>' + item.Nombre + '</td>';
                    html += '<td>' + item.Producto + '</td>';
                    html += '<td>' + item.Monto + '</td>';
                    html += '<td>' + item.Banco + '</td>';
                    html += '<td style="width: 250px;">' + item.Cuenta + '</td>';
                    html += '<td>' + item.Sinpe + '</td>';
                    html += '<td>' + item.Correo + '</td>';
                    html += '<td><a class="btn-tesoreria btn btn-primary btn-primary-asignar" onclick="mostrarImagenSinpe(' + item.Cedula + ')">Imagen</a></td>';
                    html += '<td style="width: 590px;"><div style="padding: 0 5px"><select style="font-size: 12px; padding: 0;" id="ddEstatusSolicitudes-' + item.Id + '" class="dropdown-tesoreria form-control" ><option value = "0" ></option><option value = "1" >Cuenta incorrecta</option><option value="2" > Cuenta no pertenece al cliente</option><option value="3">Cuenta inactiva</option><option value="4">Verificado OK</option></select ></div ></td > ';
                    html += '<td><a class="btn-tesoreria btn btn-primary btn-primary-editar" onclick="CambioEstado(' + item.Id + ')">Guardar</a></td>';
                    html += '</tr>';
                });
                $("#tableAprobadoVerificacionTesoreria").dataTable().fnClearTable();
                $("#tableAprobadoVerificacionTesoreria").dataTable().fnDestroy();
                $('#tblAprobadoVerificacionTesoreria').html(html);

                $("#tableAprobadoVerificacionTesoreria").dataTable({
                    "order": [[5, "asc"]]
                } );
                $("select[name*='tblAprobadoVerificacionTesoreria_length']").change();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}

//Funciones del mantenimiento
function CambioEstado(id) {
    var nuevoStatus = $('#ddEstatusSolicitudes-' + id + ' option:selected').val();
    if (nuevoStatus > 0) {
        var empObj = {
            id: id,
            nuevoStatus: nuevoStatus
        };
        processing: true; // for show progress bar  
        serverSide: true; // for process server side  
        filter: true; // this is for disable filter (search box)  
        orderMulti: false; // for disable multiple column at once
        paging: false;
        $.ajax(
            {
                url: "/AprobadoVerificacionTesoreria/CambiarEstadoSolicitud",
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
    else {
        getSolicitudesTesoreriaPendianteTranferencia();
        $('#mensajeAccion').text("!!! Debe Selecionar un estado de gestion");
        $('#mensajeAccion').addClass('alert-warning');
        $('#mensajeAccion').css("display", "block");
    }

}
function mostrarImagenSinpe(Identificacion) {
    var empObj = {
        Identificacion: Identificacion
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AprobadoVerificacionTesoreria/GetImagenSinpe",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result.Mensaje != "Completo") {
                    alertify.error(result.Mensaje);
                } else {
                    $("#imagen-sinpe img").attr("src", result.UrlImagenSinpe);
                    $('#ImagenSinpe').modal('show');
                }              
            },
            error: function (errormessage) {
                alertify.error(result.Mensaje);
            }
        });
}

