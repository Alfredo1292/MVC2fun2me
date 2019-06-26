

//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
    $('body').css("background-image", "none");
    //$('#example').dataTable();
});

//Load Data function
function loadData() {
    $('#UserTable').html();
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/ListaNegra/Detalle",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {

                html += '<tr>';
                html += '<td>' + item.Identificacion + '</td>';
                html += '<td>' + item.Telefono + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.Motivo + '</td>';
                html += '<td>' + ((item.Activo = true) ? 'Activo' : 'Desactivo') + '</td>';
                html += '<td>' + item.Accion + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.Id + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delete(' + item.Id + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('#UserTable').dataTable().fnClearTable();
            $('#UserTable').dataTable().fnDestroy();

            $('.tbody').html(html);

            $("#UserTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //	$("#UserTable").DataTable();
}



//Function for getting the Data Based upon Employee ID
function getbyID(id) {
    if (id != '0') {
        $('#txtIdentificacion').css('border-color', 'lightgrey');
        $('#txtNombre').css('border-color', 'lightgrey');
        $('#txtMotivo').css('border-color', 'lightgrey');
        $('#chkActivo').css('border-color', 'lightgrey');
        $('#ddlAccion').css('border-color', 'lightgrey');
        $('#idEditarAgente strong').text("Editar Prospecto: " + id);
        $.ajax({

            url: "/ListaNegra/Edit/" + id,
            typr: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                $.each(result.list_ListaNegra, function (key, item) {
                    $('#txtIdentificacion').val(item.Identificacion);
                    $('#txtNombre').val(item.Nombre);
                    $('#txtMotivo').val(item.Motivo);
                    $('#chkActivo').prop('checked', item.Activo);
                    $("#ddlAccion").val(item.Accion);
                    $("#hdnId").val(item.Id);
                    $("#txtTelefono").val(item.Telefono);
                    $('#myModal').modal('show');
                    $('#btnUpdate').show();
                    $('#btnAdd').hide();

                });

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    else {
        $('#myModal').modal('show');
        $('#btnUpdate').show();
        $('#btnAdd').show();
    }
    return false;
}
//function for updating employee's record
function Update() {

    var Obj = {
        Id: $('#hdnId').val(),
        Identificacion: $('#txtIdentificacion').val(),
        Nombre: $('#txtNombre').val(),
        Motivo: $('#txtMotivo').val(),
        Accion: $('#ddlAccion').val(),
        Activo: $('#chkActivo[type=checkbox]').prop('checked'),
        Telefono: $("#txtTelefono").val(),
        AccionMantemiento: 'UPDATE'

    };
    $.ajax({
        url: "/ListaNegra/MantenimientoListaNegra",
        data: JSON.stringify(Obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            alertify.alert(result[0].Respuesta, function () {
                var searchTerm = $(".search").val();
                var listItem = $('.results tbody').children('tr');

                $.extend($.expr[':'], {
                    'containsi': function (elem, i, match, array) {
                        return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                    }
                });
                var jobCount = $('.results tbody tr[visible="true"]').length;
                $('.counter').text(jobCount + ' item');

                if (jobCount == '0') { $('.no-result').show(); }
                else { $('.no-result').hide(); }
                loadData();
            });
            //loadData();
            $('#myModal').modal('hide');
            clearTextBox();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Insert() {
    var Obj = {
        Id: $('#hdnId').val(),
        Identificacion: $('#txtIIdentificacion').val(),
        Nombre: $('#txtINombre').val(),
        Motivo: $('#txtIMotivo').val(),
        Accion: $('#ddlIAccion').val(),
        Activo: $('#chkIActivo[type=checkbox]').prop('checked'),
        Telefono: $("#txtITelefono").val(),
        AccionMantemiento: 'INSERT'

    };
    $.ajax({
        url: "/ListaNegra/MantenimientoListaNegra",
        data: JSON.stringify(Obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            alertify.alert(result[0].Respuesta, function () {
                var searchTerm = $(".search").val();
                var listItem = $('.results tbody').children('tr');

                $.extend($.expr[':'], {
                    'containsi': function (elem, i, match, array) {
                        return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                    }
                });
                var jobCount = $('.results tbody tr[visible="true"]').length;
                $('.counter').text(jobCount + ' item');

                if (jobCount == '0') { $('.no-result').show(); }
                else { $('.no-result').hide(); }
                loadData();
            });
            //loadData();
            clearTextBox();

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record
function Delete(ID) {

    alertify.confirm("¿Desea Continuar?",
        function () {
            $.ajax(
                {
                    url: "/ListaNegra/MantenimientoListaNegra",
                    type: "POST",
                    data: {
                        Id: ID,
                        AccionMantemiento: 'DELETE'
                    },
                    success: function (result) {
                        alertify
                            .alert(result[0].Respuesta);
                        $('#UserTable').html();
                        loadData();
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = p3;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).message;
                        alert(err);
                    }
                });
        },
        function () {
            alertify.error('Cancel');
        });

}

//Function for clearing the textboxes
function clearTextBox() {
    $('#txtIdentificacion').val("");
    $('#txtNombre').val("");
    $('#txtMotivo').val("");
    $('#chkActivo').val("");
    $('#ddlAccion').val("");
    $('#txtIIdentificacion').val("");
    $('#txtINombre').val("");
    $('#txtIMotivo').val("");
    $('#chkIActivo').val("");
    $('#ddlIAccion').val("");
    $("#txtTelefono").val("");
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#txtIIdentificacion').val().trim() == "") {
        $('#txtIIdentificacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtIIdentificacion').css('border-color', 'lightgrey');
    }
    if ($('#txtINombre').val().trim() == "") {
        $('#txtINombre').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtINombre').css('border-color', 'lightgrey');
    }
    if ($('#txtMotivo').val().trim() == "") {
        $('#txtMotivo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtMotivo').css('border-color', 'lightgrey');
    }
    return isValid;
}

function validateCreate() {
    var isValid = true;
    if ($('#txtcod_agente').val().trim() == "") {
        $('#txtcod_agente').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtcod_agente').css('border-color', 'lightgrey');
    }
    if ($('#txtnombre').val().trim() == "") {
        $('#txtnombre').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtnombre').css('border-color', 'lightgrey');
    }
    if ($('#txtCorreo').val().trim() == "") {
        $('#txtCorreo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtCorreo').css('border-color', 'lightgrey');
    }
    if (!$('#ddlRoles').val()) {
        $('#ddlRoles').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlRoles').css('border-color', 'lightgrey');
    }
    if ($('#txtpass').val().trim() == "") {
        $('#txtpass').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtpass').css('border-color', 'lightgrey');
    }
    if ($('#txtConfirmPassword').val().trim() == "") {
        $('#txtConfirmPassword').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtConfirmPassword').css('border-color', 'lightgrey');
    }
    return isValid;
}

