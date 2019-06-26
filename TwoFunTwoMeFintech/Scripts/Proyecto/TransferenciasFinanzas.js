
function getTransferenciasFinanzas() {
    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFinal = $('#txtFechaFinal').val();

    if (fechaInicio != "" && fechaFinal != "") {
        if (fechaInicio <= fechaFinal) {
            var empObj = {
                FECHA_INICIO: fechaInicio,
                FECHA_FIN: fechaFinal
            };
            processing: true; // for show progress bar  
            serverSide: true; // for process server side  
            filter: true; // this is for disable filter (search box)  
            orderMulti: false; // for disable multiple column at once
            paging: false;
            $.ajax(
                {
                    url: "/TranferenciasFinanzas/GetTranferencias",
                    type: "POST",
                    data: JSON.stringify(empObj),
                    contentType: "application/json;charset=utf-8",
                    success: function (result) {
                        var html = '';
                        $.each(result, function (key, item) {
                            if (item.Producto == 'TOTAL BANCO') {
                                html += '<tr style="color: black; font-weight: bold; background-color: #87b87f;">';
                                html += '<td>' + item.Cedula + '</td>';
                                html += '<td>' + item.Nombre + '</td>';
                                html += '<td>' + item.Producto + '</td>';
                                html += '<td>' + item.Monto + '</td>';
                                html += '<td>' + item.Banco + '</td>';
                                html += '<td>' + item.Cuenta + '</td>';
                                html += '<td>' + item.Sinpe + '</td>';
                                html += '<td></td>';
                                html += '</tr>';
                            } else {
                                if (item.Producto == 'TOTAL MONTO') {
                                    html += '<tr style="color: black; font-weight: bold; background-color: #ec971f;">';
                                    html += '<td>' + item.Cedula + '</td>';
                                    html += '<td>' + item.Nombre + '</td>';
                                    html += '<td>' + item.Producto + '</td>';
                                    html += '<td>' + item.Monto + '</td>';
                                    html += '<td>' + item.Banco + '</td>';
                                    html += '<td>' + item.Cuenta + '</td>';
                                    html += '<td>' + item.Sinpe + '</td>';
                                    html += '<td></td>';
                                    html += '</tr>';
                                } else {
                                    if (item.Producto == 'TOTAL LINEAS') {
                                        html += '<tr style="color: black; font-weight: bold; background-color: #438eb9;">';
                                        html += '<td>' + item.Cedula + '</td>';
                                        html += '<td>' + item.Nombre + '</td>';
                                        html += '<td>' + item.Producto + '</td>';
                                        html += '<td>' + item.Monto + '</td>';
                                        html += '<td>' + item.Banco + '</td>';
                                        html += '<td>' + item.Cuenta + '</td>';
                                        html += '<td>' + item.Sinpe + '</td>';
                                        html += '<td></td>';
                                        html += '</tr>';
                                    } else {
                                        html += '<tr>';
                                        html += '<td>' + item.Cedula + '</td>';
                                        html += '<td>' + item.Nombre + '</td>';
                                        html += '<td>' + item.Producto + '</td>';
                                        html += '<td>' + item.Monto + '</td>';
                                        html += '<td>' + item.Banco + '</td>';
                                        html += '<td>' + item.Cuenta + '</td>';
                                        html += '<td>' + item.Sinpe + '</td>';
                                        html += '<td>' + item.Fecha.substr(0, 10) + '</td>';
                                        html += '</tr>';
                                    }
                                }
                            }
                        });
                                               
                        $("#tableTransferenciasFinanzas").dataTable().fnClearTable();
                        $("#tableTransferenciasFinanzas").dataTable().fnDestroy();
                        $('#tblTransferenciasFinanzas').html(html);
                        $("#tableTransferenciasFinanzas").dataTable({
                            "ordering": false,
                            "paging": false
                        });
                        $("select[name*='tblTransferenciasFinanzas_length']").change();

                        //habilitamos el boton de descargar
                        $('#btnDescargarReporte').removeClass('btn-desabilitado');
                        $('#btnDescargarReporte').addClass('btn-primary-asignar');
                        //Quitamos cualquier mensaje de warning
                        $('#mensajeAccion').text('');
                        $('#mensajeAccion').addClass('alert-warning');
                        $('#mensajeAccion').css("display", "none");
                    },
                    error: function (errormessage) {
                        alert(errormessage.responseText);
                    }
                });
        }
        else {
            $('#mensajeAccion').text('El rango de fechas es incorrecto');
            $('#mensajeAccion').addClass('alert-warning');
            $('#mensajeAccion').css("display", "block");
        }
    }
    else {
        $('#mensajeAccion').text('Debe seleccionar las fechas para generar el reporte');
        $('#mensajeAccion').addClass('alert-warning');
        $('#mensajeAccion').css("display", "block");
    }
}
function descargarReporteExcel() {
    if ($('#btnDescargarReporte').hasClass('btn-desabilitado')) {
        $('#mensajeAccion').text('Debe generar primero el reporte para poder descargarlo');
        $('#mensajeAccion').addClass('alert-warning');
        $('#mensajeAccion').css("display", "block");
    }
    else {
        var tableID = 'tableTransferenciasFinanzas';
        filename = 'reporte-excel';
        var downloadLink;
        var dataType = 'application/vnd.ms-excel';
        var tableSelect = document.getElementById(tableID);
        var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');


        // damos el nombre del archivo
        filename = filename ? filename + '.xls' : 'excel_data.xls';
        // creamos el elemento para descargar 
        downloadLink = document.createElement("a");
        document.body.appendChild(downloadLink);
        if (navigator.msSaveOrOpenBlob) {
            var blob = new Blob(['ufeff', tableHTML], {
                type: dataType
            });
            navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // creamos el link para la descarga
            downloadLink.href = 'data:' + dataType + ', ' + tableHTML;
            // seteamos el nombre del archivo y la descarga
            downloadLink.download = filename;
            downloadLink.click();
        }
    }
}
