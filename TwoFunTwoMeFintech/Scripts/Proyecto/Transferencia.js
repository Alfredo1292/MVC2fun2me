
$(document).ready(function () {
    buscarTransferencia();
    $('body').css("background-image", "none");
    // $("#dtBasicExample").DataTable();
});



function buscarTransferencia() {


    $('.transfer').html('');

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Transferencias/ConsultarTransferenciasDocumentos",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#TransTable').css("display", "block");
            $('#divTransfer').css("display", "block");
            var html = '';
            var Total = 0;

            $.each(result, function (key, item) {
                Total += parseFloat(item.Monto_transferencia);
                html += '<tr style="width:100%;">';

                html += '<td style="width:40%;">' + item.Nombre + '</td>';
                //html += '<td style="width:5%;">' + item.Producto + '</td>';
                html += '<td style="width:10%;">' + numFormat(item.Monto_transferencia, 1, true) + '</td>';
                html += '<td style="width:15%;">' + item.Descripcion + '</td>';
                html += '<td style="width:5%;">' + item.Cuenta + '</td>';

                html += '<td style="width:15%;">' + item.CuentaSinpe + '</td>';
                html += '<td style="width:15%; ">' + item.FechaTransferencia + '</td>';
                //html += '<td style="width:5%; ">' + item.UsrModifica + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });
            $("#lblTotalTransferencia").text(numFormat(Total, 1, true));
            $('#TransTable').dataTable().fnClearTable();
            $('#TransTable').dataTable().fnDestroy();
            $('.transfer').html(html);

            $("#TransTable").DataTable();
            $("select[name*='TransTable_length']").val("100");
            $("select[name*='TransTable_length']").change();
            TotalxBanco();
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}


function TotalxBanco() {

    $('.transfer').html('');

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Transferencias/ConsultarTotalTransferencias",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';

            $.each(result, function (key, item) {
                html += '<tr style="width:100%;">';

                html += '<td style="width:40%;">' + item.Descripcion + '</td>';
                html += '<td style="width:40%;">' + item.Cantidad + '</td>';
                //html += '<td style="width:5%;">' + item.Producto + '</td>';
                html += '<td style="width:10%;">' + numFormat(item.TotalxBanco, 1, true) + '</td>';

                html += '</tr>';
            });
            $('#TotBancoTable').dataTable().fnClearTable();
            $('#TotBancoTable').dataTable().fnDestroy();
            $('.totalBanco').html(html);

            $("#TotBancoTable").DataTable();
            $("select[name*='TotBancoTable_length']").val("100");
            $("select[name*='TotBancoTable_length']").change();
            $("select[name*='TransTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}


