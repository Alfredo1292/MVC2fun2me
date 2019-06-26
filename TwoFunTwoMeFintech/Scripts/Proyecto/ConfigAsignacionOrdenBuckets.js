
$(document).ready(function () {
    $('body').css("background-image", "none");
});
function cargarConFiguracion() {

    var empObj = {
        Accion: 'CONSULTA'
    };

    $('.ConfigAsig').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Buckets/MantenimientoConfiguracion",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.EstadoNombre + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return EditaConfiguracion(' + item.Id + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick=" return EliminaConfig(' + item.Id + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('#TblConfigAsig').dataTable().fnClearTable();
            $('#TblConfigAsig').dataTable().fnDestroy();
            $('.ConfigAsig').html(html);

            $("#TblConfigAsig").DataTable();
            $("select[name*='TblConfigAsig_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function cargarConFiguracionReportes() {
    var empObj = {
        Accion: 'CONSULTA'
    };
    $('.ConfigReportes').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url:"/Buckets/ConfiguracionReportes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            var parametros = '';
            $.each(data, function (key, item) {
                parametros = item.Id + ',\'' + item.NombreArchivo + '\'';
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.NombreArchivo + '</td>';
                html += '<td>' + item.Area + '</td>';               
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="insertarColaReportes(' + parametros+')"> Generar </a></td>';
                html += '</tr>';
            });
            $('#TblConfigReportes').dataTable().fnClearTable();
            $('#TblConfigReportes').dataTable().fnDestroy();
            $('.ConfigReportes').html(html);
            $("#TblConfigReportes").DataTable();
            $("select[name*='TblConfigReportes_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
}
function generarColaReporte() {
    var empObj = {
        IdReporte: ''
    };
    $('.ColaReportes').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Buckets/ColaReportes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdReporte + '</td>';
                html += '<td>' + item.NombreArchivo + '</td>';
                if (item.Estado==0) {
                    html += '<td class="accion-pendiente">' + 'Pendiente' + '</td>';
                } else {
                    html += '<td class="accion-procesada">' + 'Procesado' + '</td>';
                }
                html += '<td>' + item.UsuarioCreacion + '</td>';
                html += '</tr>';
            });
            $('#TblColaReportes').dataTable().fnClearTable();
            $('#TblColaReportes').dataTable().fnDestroy();
            $('.ColaReportes').html(html);
            $("#TblColaReportes").DataTable();
            $("select[name*='TblColaReportes']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
}
function insertarColaReportes(idReporte, nombreArchivo) {
    var empObj = {
        IdReporte: idReporte,
        NombreArchivo: nombreArchivo,
        Estado: 0,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Buckets/InsertaColaReportes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            generarColaReporte()
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });

}
function EliminaConfig(id) {
    var empObj = {
        Accion: 'ELIMINA',
        Id: id
    };
    alertify.confirm("¿Desea Eliminar?.",
        function () {
            $.ajax({
                url: "/Buckets/MantenimientoConfiguracion",
                type: "POST",
                data: JSON.stringify(empObj),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alertify.alert("Id: " + id + " Se elimino", function () {
                        cargarConFiguracion();
                    });
                },
                error: function (errormessage) {
                    alertify.error('Ocurrion un Error');

                }
            });

        });
}

function EditaConfiguracion(id) {
    var empObj = {
        Accion: 'CONSULTA',
        Id: id
    };

    $.ajax({
        url: "/Buckets/MantenimientoConfiguracion",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#ID").val(data[0].Id)
            $("#Nombre").val(data[0].Nombre)
            $("#ddlEstado").val(data[0].EstadoNombre == "ACTIVO" ? "1" : "0");
            $('#ModalEditConfig').modal('show');
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
}
function Update() {
    var empObj = {
        Accion: 'ACTUALIZA',
        Id: $("#ID").val(),
        Nombre: $("#Nombre").val(),
        Estado: $("#ddlEstado").val() == "1" ? "true" : "falses"
    };

    $.ajax({
        url: "/Buckets/MantenimientoConfiguracion",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            alertify.alert("Id: " + $("#ID").val() + " Se actualizo", function () {
                $('#ModalEditConfig').modal('hide');
                cargarConFiguracion();
            });
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
}

function GaurdarNuevo() {
    var empObj = {
        Accion: 'INSERTA',
        Nombre: $("#NombreNuevo").val(),
        Estado: $("#ddlEstadoNuevo").val() == "1" ? "true" : "falses"
    };

    $.ajax({
        url: "/Buckets/MantenimientoConfiguracion",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            alertify.alert("Se creo la configuración N#: " + $data[0].Id, function () {
                cargarConFiguracion();
                $("#ConfigAsigTab").addClass('active');
            });
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
}