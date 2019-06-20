//function cargarResultadoScore() {
//    var id = $("#identificacionId").val();
//    alert(id + "");
//    var empObj = {
//        Identificacion : id   
//    };
//    alert(empObj + "");
//    $('.ResultadoScore').html('');
//    processing: true; // for show progress bar  
//    serverSide: true; // for process server side  
//    filter: true; // this is for disable filter (search box)  
//    orderMulti: false; // for disable multiple column at once
//    paging: false;
//    $.ajax({
//        url: baseUrl + "Solicitudes/CargarResultadoScore",
//        type: "POST",
//        data: JSON.stringify(empObj),
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            var html = '';
//            $.each(data, function (key, item) {
//                html += '<tr>';
//                html += '<td>' + item.IdRule + '</td>';
//                html += '<td>' + item.Evaluar + '</td>';
//                html += '<td>' + item.Procesado + '</td>';
//                html += '<td>' + item.Puntaje + '</td>';
//                html += '<td>' + item.FechaProceso + '</td>';
//                html += '<td>' + item.Descripcion + '</td>';
//                html += '</tr>';
//            });
//            $('#TblResultadoScore').dataTable().fnClearTable();
//            $('#TblResultadoScore').dataTable().fnDestroy();
//            $('.ResultadoScore').html(html);
//            $(".TblResultadoScore").DataTable();
//            $("select[name*='TblResultadoScore_length']").change();
//        },
//        error: function (errormessage) {
//            alertify.error('Ocurrion un Error');
//        }
//    });
//}
$(document).ready(function () {
    $('body').css("background-image", "none");
});
function ActualizarSolicitud(ID_SOLICITUD, ID_Productos, ID_Estados, montoMaximo) {
    $.ajax(
        {
            url: baseUrl + "Solicitudes/ActualizaSolicitud",
            type: "POST",
            data: {
                Id: ID_SOLICITUD,
                MontoMaximo: montoMaximo,
                IdProducto: ID_Productos,
                Status: ID_Estados
            },
            success: function (result) {
                $("#spnMensaje").html(result.Mensaje);
            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                $("#spnMensajeError").html(result.Mensaje);
            }
        });

}
function Descargar(ident) {

    $.ajax(
        {
            url: baseUrl + "Solicitudes/DescargarXmlBuros",
            type: "POST",
            data: {
                identificacion: ident
            },
            success: function (result) {

                $("#spnMensaje").html(result.Mensaje);

                if (result.DATATUCA !== null) {
                    download(result.DATATUCA, result.NOMBRETUCA, result.MIMETYPE);
                }
                if (result.DATACREDDIT !== null) {
                    download(result.DATACREDDIT, result.NOMBRECREDDIT, result.MIMETYPE);
                }
                if (result.DATAEQUIFAX !== null) {
                    download(result.DATAEQUIFAX, result.NOMBREEQUIFAX, result.MIMETYPE);
                }
                if (result.DATAGINI!== null) {
                    download(result.DATAEQUIFAX, result.NOMBREGINI, result.MIMETYPE);
				}
				if (result.DATACREDISERVER !== null) {
					download(result.DATACREDISERVER, result.NOMBRECREDISERVER, result.MIMETYPE);
				}
                return false;
            },

            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                $("#spnMensajeError").html(result.Mensaje);
            }
        });

}