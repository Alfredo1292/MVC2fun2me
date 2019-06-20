var v_RegCedula = '^[1-9]{1}[0-9]{8}$';// /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var v_RegCedulaDimex = '^[1-9]{1}[0-9]{11}$'; //'^[1-9]{1}[0-9]{11}$' usar esta en caso que permita 11 o 12 digitos
var v_IdCredito = /^\d+$/;


$(document).ready(function () {
    //llamamos al metodo para cargar la tabla por defecto

    //boton buscar
    $("#btnBurcarHistorialCobros").click(function () {
        setParametrosBusqueda();
    });
    //Boton Limpiar
    $("#btnLimpiarBusqueda").click(function () {
        limpiarBusqueda();
    });
    //Boton descargar
    $("#btnDescargarExcel").click(function () {
        descargarExcel();
    });

    //cuando seleccione la fecha de inicio ponemos en null el texto de busqueda
    $("#txtFechaInicio").change(function () {     
        $('#txtBusquedaCobros').val("");
    });
});
function limpiarTabla() {
                        var html = '';
                    $('#tableHistorialCobros').dataTable().fnClearTable();
                    $('#tableHistorialCobros').dataTable().fnDestroy();
                    $('#tblHistorialCobros').html(html);
                        //setear la tabla en español
                    $('#tableHistorialCobros').dataTable({
                            "order": [[1, "desc"]],
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
                    $('select[name*="tblHistorialCobros_length"]').change();
}
function BuscarHistorialCobros(fechaInicio, fechaFinal, identificacion, idCredito) {
    var objBuscar = {
        FechaInicio: fechaInicio,
        FechaFinal: fechaFinal,
        Identificacion: identificacion,
        IdCredito: idCredito
    };
    $.ajax({
        url: "/HistorialCobros/ConsultaHistorialCobros",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(objBuscar),
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Identificacion + '</td>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.Banco + '</td>';
                html += '<td>' + item.Referencia + '</td>';
                html += '<td>' + item.TotalCobro + '</td>';
                html += '<td>' + item.FechaCobro + '</td>';
                html += '</tr>';
            });
            $('#tableHistorialCobros').dataTable().fnClearTable();
            $('#tableHistorialCobros').dataTable().fnDestroy();
            $('#tblHistorialCobros').html(html);
            //setear la tabla en español
            $('#tableHistorialCobros').dataTable({
                "order": [[1, "desc"]],
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
            $('select[name*="tblHistorialCobros_length"]').change();
            tablaRespaldoExcel(result);
        },
        error: function (errormessage) {
            alertify.error("Error al cargar el historial de cobros.");
        }
    });
}
function setParametrosBusqueda(){
    var fInicio = $('#txtFechaInicio').val(), fFinal = $('#txtFechaFinal').val(), textBuscar = $('#txtBusquedaCobros').val();
    if (fInicio == "" && fFinal == "" && textBuscar == "") {
        alertify.error("Debe ingresar un criterio de busqueda");
    }
    else {
        if (textBuscar != "") {
            if (textBuscar.match(v_RegCedula)) {
                BuscarHistorialCobros(null, null, textBuscar, 0);
                $('#txtFechaInicio').val("");
                $('#txtFechaFinal').val("");
            }
            else {
                if (textBuscar.match(v_RegCedulaDimex)) {
                    BuscarHistorialCobros(null, null, textBuscar, 0);
                    $('#txtFechaInicio').val("");
                    $('#txtFechaFinal').val("");
                }
                else {
                    if (textBuscar.match(v_IdCredito)) {
                        BuscarHistorialCobros(null, null, null, textBuscar);
                        $('#txtFechaInicio').val("");
                        $('#txtFechaFinal').val("");
                    }
                }
            }
        }      
        else {
            if (fInicio != "" && fFinal != "") {
                //validamos que el rango de fecha sea el correcto
                if (fInicio <= fFinal) {
                    BuscarHistorialCobros(fInicio, fFinal, null, 0);
                }
                else {
                    alertify.error("El rango de fechas no es correcto");
                }
            }
            else {
                alertify.error("No se encuentra ningun parametro de busqueda");
            }
        }
    }
}
function limpiarBusqueda() {
    $('#historial-cobros-container input').val("");
    limpiarTabla();
}

function descargarExcel() {
    var d = new Date();
    var tmpElemento = document.createElement('a');
    // obtenemos la información desde el div que lo contiene en el html
    // Obtenemos la información de la tabla
    var data_type = 'data:application/vnd.ms-excel';
    var tabla_div = document.getElementById('tableHistorialCobros-exportar-excel');
    var tabla_html = tabla_div.outerHTML.replace(/ /g, '%20');
    tmpElemento.href = data_type + ', ' + tabla_html;
    //Asignamos el nombre a nuestro EXCEL
    tmpElemento.download = 'historial-cobros-' + d.getDate() + '-' + d.getMonth() + '-' + d.getFullYear() + '_' + d.getHours() + '_' + d.getMinutes() + '_' + d.getSeconds() +'.xls';
    // Simulamos el click al elemento creado para descargarlo
    tmpElemento.click();
}

//Este metodo sera para crear una copia de la tabla sin paginacion.
//Esto con el fin de exportar al excel esta tabla sin que se pierdan datos en la paginacion
function tablaRespaldoExcel(result) {
    var html = '';
    $.each(result, function (key, item) {
        html += '<tr>';
        html += '<td>' + item.Identificacion + '</td>';
        html += '<td>' + item.IdCredito + '</td>';
        html += '<td>' + item.Nombre + '</td>';
        html += '<td>' + item.Banco + '</td>';
        html += '<td>' + item.Referencia + '</td>';
        html += '<td>' + item.TotalCobro + '</td>';
        html += '<td>' + item.FechaCobro + '</td>';
        html += '</tr>';
    });
    $('#tblHistorialCobros-exportar-excel').html(html);

}