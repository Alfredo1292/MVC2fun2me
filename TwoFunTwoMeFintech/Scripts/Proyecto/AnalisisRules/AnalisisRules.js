//Expresiones regulares para validar la cedula nacional y la DIMEX
var ValidarCedula = '^[1-9]{1}[0-9]{8}$';// /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var ValidarCedulaDimex = '^[0-9]{1}[0-9]{11}$';
var ValidarIdSolicitud = /^\d+$/;
$(document).ready(function () {
    //Eventos de los botones 
    $('#btnCerrarRules').click(function () {
        cerrarTablaRules();
    });
    $('#btnBuscarRules').click(function () {
        var textoBuscar = $('#txtBusquedaRules').val();
        BuscarRules(textoBuscar);
    });
});

function cerrarTablaRules() {
    $('#contenido-tabla-rules').hide('slow');
    $('#txtIdentificacion').val("");
}
function BuscarRules(textoBuscar) {
    //validar si se ingreso numero de cedula o idSolicitud
    if (textoBuscar != null) {
        if (textoBuscar == '') {
            alert("Favor ingresar Id Solicitud o Cedula del Cliente");
        }
        else {
            if (textoBuscar.match(ValidarCedula) || textoBuscar.match(ValidarCedulaDimex)) {
                var objBuscar = {
                    Identificacion: textoBuscar
                };
            }
            else {
                if (textoBuscar.match(ValidarIdSolicitud)) {
                    var objBuscar = {
                        IdSolicitud: textoBuscar
                    };
                }
            }
            $.ajax({
                url: "/AnalisisRules/ConsultaRules",
                type: "POST",
                data: JSON.stringify(objBuscar),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.length == 0) {
                        alertify.error("No existen rules para el criterio de busqueda ingresado");
                    }
                    else {
                        var html = '';
                        $.each(result, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.IdSolicitud + '</td>';
                            html += '<td>' + item.Identificacion + '</td>';
                            html += '<td>' + item.MotivoRechazo + '</td>';
                            html += '<td>' + item.DescripcionMotivoRechazo + '</td>';
                            html += '<td>' + item.FechaIngreso + '</td>';
                            html += '<td>' + item.Descripcion + '</td>';
                            html += '</tr>';
                        });
                        $('#TableRules').dataTable().fnClearTable();
                        $('#TableRules').dataTable().fnDestroy();
                        $('#tblrules').html(html);
                        //setear la tabla en español
                        $('#TableRules').dataTable({
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
                        $('select[name*="tblrules_length"]').change();
                        $('#contenido-tabla-rules').show('slow');
                    }
                },
                error: function (errormessage) {
                    alertify.error("Buscar productos\n" + errormessage.responseText);
                }
            });
        }
    } else {

    }
}
