//icortes 2018-12-19
$(document).ready(function () {

    updateDateInputs();
    ConsultarDashBoard();
    ConsultarDashBoardDetallado();
    // $("#dtBasicExample").DataTable();
    $('body').css("background-image", "none");
});

function updateDateInputs() {
    var date = new Date(), // The Date object lets you work with dates.
        year = date.getFullYear(), // This method gets the four digit year.
        month = date.getMonth() + 1, // This method gets the month and Jan is 0.
        day = date.getDate(), // This method gets the day of month as a number.
        hour = date.getHours(), // This method gets the hour 
        min = date.getMinutes(); // This method gets the minutes
    month = (month < 10 ? '0' + month : month);
    day = (day < 10 ? '0' + day : day);
    hour = (hour < 10 ? '0' + hour : hour); // It adds a 0 to number less than 10 because input[type=time] only accepts 00:00 format. 
    min = (min < 10 ? '0' + min : min);

    $('#txtFechaBusqueda').val(year + '-' + month + '-' + day); // This line sets the value.

}
function ConsultarDashBoard() {
    var empObj = {
        FechaAsignacion: $('#txtFechaBusqueda').val()
    };

    $('.DashBoard').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/ConsultaDashBoardGeneral",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.AgenteAsignado + '</td>';
                html += '<td>' + item.nombre + '</td>';
                html += '<td>' + item.Cantidad + '</td>';
                if (item.Acion == "Procesado")
                    html += '<td class="accion-procesada">' + item.Acion + '</td>';
                else
                    html += '<td class="accion-pendiente">' + item.Acion + '</td>';
                html += '<td>' + item.FechaAsignacion + '</td>';
                //html += '<td>' + "<a href='#' class='btn btn-primary' onclick= TraeEncabezado('" + item.Identificacion + "','" + item.IdCredito + "'); >Procesar</a>" + '</td>';
                html += '</tr>';
            });
            $('#DashBoardTable').dataTable().fnClearTable();
            $('#DashBoardTable').dataTable().fnDestroy();
            $('.DashBoard').html(html);

            $("#DashBoardTable").DataTable({ "ordering": false });
            $("select[name*='DashBoardTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}


function ConsultarDashBoardDetallado() {


    $('.DashBoardDetallado').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/ConsultaDashBoardDetallado",
        type: "POST",
        data: JSON.stringify(),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.OrdenAsignacion + '</td>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.Identificacion + '</td>';
                html += '<td>' + item.DiasMora + '</td>';
                html += '<td>' + numFormat(item.TotalMora, 4, true) + '</td>';
                if (item.Procesado == "Procesado")
                    html += '<td class="accion-procesada">' + item.Procesado + '</td>';
                else
                    html += '<td class="accion-pendiente">' + item.Procesado + '</td>';
                html += '<td>' + item.FechaUltimaPromesaPago + '</td>';
                html += '<td>' + item.FechaUltimaGestion + '</td>';
                html += '<td>' + item.Agenteasignado + '</td>';
                //html += '<td>' + "<a href='#' class='btn btn-primary' onclick= TraeEncabezado('" + item.Identificacion + "','" + item.IdCredito + "'); >Procesar</a>" + '</td>';
                html += '</tr>';
            });
            $('#DashBoardDetalladoTable').dataTable().fnClearTable();
            $('#DashBoardDetalladoTable').dataTable().fnDestroy();
            $('.DashBoardDetalaldo').html(html);

            $("#DashBoardDetalladoTable").DataTable({ "ordering": false });
            $("select[name*='DashBoardDetalladoTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
