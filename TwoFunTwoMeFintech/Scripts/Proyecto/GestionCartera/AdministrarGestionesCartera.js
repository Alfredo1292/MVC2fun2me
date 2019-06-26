$(document).ready(function () {
    cargarGestionesPendientes();
});

function cargarGestionesPendientes() {
    $('#tblAdminGestionCartera').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/AdministrarGestionesCartera/ConsultaGestionesPendientes",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.TipoGestion + '</td>';
                html += '<td>' + item.FechaIngreso + '</td>';
                html += '<td>' + item.UsuarioIngreso + '</td>';
                html += '<td>' + item.EstadoGestion + '</td>';
                html += '<td style="width: 200px;"><form class="form-inline"><div role="group"><button style="margin-right: 5px;" type="button" id="btnAprobarGestion-' + item.IdGestion + '" class="btn btn-primary btn-primary-editar" onClick="AprobarGestion(' + item.IdGestion + ',' + item.IdCredito + ',\'' + item.TipoGestion + '\')"><span class="fas fa-check"></span>  Aprobar</button><button type="button" id="btnAprobarGestion-' + item.IdGestion + '" class="btn btn-primary-eliminar" onClick="RechazarGestion(' + item.IdGestion + ',' + item.IdCredito + ',\'' + item.TipoGestion + '\')"><span class="fas fa-window-close"></span>  Rechazar</button></div></form></td>';
                html += '</tr>';
            });

            $('#tableAdminGestionCartera').dataTable().fnClearTable();
            $('#tableAdminGestionCartera').dataTable().fnDestroy();
            $('#tblAdminGestionCartera').html(html);
            $("#tableAdminGestionCartera").dataTable({
                "order": [[0, "asc"]],
                language: {
                    "decimal": "",
                    "emptyTable": "No hay información",
                    "info": "Mostrando _START_ a _END_ de _TOTAL_ Registros",
                    "infoEmpty": "Mostrando 0 Entradas",
                    "infoFiltered": "(Filtrado de _MAX_ total Registros)",
                    "infoPostFix": "",
                    "thousands": ",",
                    "lengthMenu": "Mostrar _MENU_ Registros",
                    "loadingRecords": "Cargando...",
                    "processing": "Procesando...",
                    "search": "Buscar:",
                    "zeroRecords": "Sin resultados encontrados",
                    "paginate": {
                        "first": "Primero",
                        "last": "Ultimo",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                }
            });
            $("select[name*='tblAdminGestionCartera_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function AprobarGestion(IdGestion,IdCredito,TipoGestion) {
    var empObj = {
        IdGestion: IdGestion,
        IdCredito: IdCredito,
        TipoGestion: TipoGestion
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AdministrarGestionesCartera/AprobarGestion",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result.Mensaje == 'COMPLETO') {
                    alertify.success('Gestion Aprobada');
                    cargarGestionesPendientes();
                }
                else {
                    alert('BD Error al aprobar la gestion:  ' + result.Mensaje);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function RechazarGestion(IdGestion, IdCredito, TipoGestion) {
    var empObj = {
        IdGestion: IdGestion,
        IdCredito: IdCredito,
        TipoGestion: TipoGestion
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AdministrarGestionesCartera/RechazarGestion",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result.Mensaje == 'COMPLETO') {
                    alertify.success('Gestion Rechazada');
                    cargarGestionesPendientes();
                }
                else {
                    alert('BD Error al aprobar la gestion:  ' + result.Mensaje);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}