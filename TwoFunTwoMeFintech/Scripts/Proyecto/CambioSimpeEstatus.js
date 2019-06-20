
$(document).ready(function () {
    buscarSinpe();
    $('body').css("background-image", "none");
    // $("#dtBasicExample").DataTable();
});



function buscarSinpe() {
    var empObj = {
        Accion: 'CONSULTA',
    };

    $('.Sinpe').html('');

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/SolicitudesVentas/cansultaSimpe",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#SinpeTable').css("display", "block");
            var html = '';


            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdSolicitud + '</td>';
                html += '<td>' + item.Identificacion + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.CuentaSinpe + '</td>';
                html += '<td>' + item.DescripcionStatus + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return EditarSinpe(' + item.IdSolicitud + ')"> Editar </a></td>';
                html += '</tr>';
            });

            $('#SinpeTable').dataTable().fnClearTable();
            $('#SinpeTable').dataTable().fnDestroy();
            $('.Sinpe').html(html);

            $("#SinpeTable").DataTable();
            $("select[name*='SinpeTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function EditarSinpe(id) {
    var empObj = {
        Accion: 'CONSULTA',
        IdSolicitud: id
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/SolicitudesVentas/EditaSimpe",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                $('#ID').val(item.IdSolicitud);
                $('#txtCuentaSinpe').val(item.CuentaSinpe);
                $('#txtEstado').val(item.DescripcionStatus);
                $('#hdfEstadoId').val(item.IdStatus);
                CargaTipos(item.IdStatus);

                $('#myModal').modal('show');
            });
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}


function ModificarSinpe() {
    var empObj = {
        Accion: 'UPDATE',
        IdSolicitud: $('#ID').val(),
        CuentaSinpe: $("#txtCuentaSinpe").val(),
        Status: $("#ddlEstados").val()
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/SolicitudesVentas/ActualizaSimpe",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alertify.alert(result[0].Respuesta);
            $('#myModal').modal('hide');
            buscarSinpe();
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function CargaTipos(idSelected) {
    var empObj = {
        Id: '5'
    };
    var options = [];
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/SolicitudesVentas/CargaTipos",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                options.push('<option value="',
                    item.ID_ESTADO, '">',
                    item.Descripcion, '</option>');
            });
            $("#ddlEstados").html(options.join(''));
            $("#ddlEstados").val(idSelected);
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

