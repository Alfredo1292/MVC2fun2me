//Expresiones regulares para validar la cedula nacional y la DIMEX
var ValidarCedula = '^[1-9]{1}[0-9]{8}$';// /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var ValidarCedulaDimex = '^[0-9]{1}[0-9]{11}$';

function BuscarProductos() {
    Buscar();
}

//function saludar(nombre) {
//    alert('Hola ' + nombre);
//}

//function procesarEntradaUsuario(callback) {
//    var nombre = prompt('Por favor ingresa tu nombre.');
//    callback(nombre);
//}

//procesarEntradaUsuario(saludar);

function Buscar() {
    if ($("#txtIdentificacion").val().match(ValidarCedula) || $("#txtIdentificacion").val().match(ValidarCedulaDimex)) {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/ConsultaCambioProducto/?Identificacion=" + $("#txtIdentificacion").val(),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var html = '';
            var banderaContinue = true;
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdSolicitud + '</td>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.MontoProducto + '</td>';
                html += '<td>' + item.PlazoDias + '</td>';
                html += '<td>' + item.Frecuencia + '</td>';
                html += '<td>' + item.MontoMaximo + '</td>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + numFormat(item.Cuota, 1, true) + '</td>';
                html += '<td><a id="editarProductoCredito" class="btn btn-primary btn-primary-editar" onclick="EditarProductoCreditro(' + item.IdCredito + ',' +item.IdSolicitud + ',' + item.MontoProducto + ',' + item.MontoMaximo + ',\'' + item.NombreProducto + '\',' + item.Cuota + ',' + item.PlazoDias + ',\'' + item.Frecuencia + '\')"> Editar </a></td>';
                html += '</tr>';
            });
            $('#TableProducto').dataTable().fnClearTable();
            $('#TableProducto').dataTable().fnDestroy();
            $('#tblProducto').html(html);
            //setear la tabla en español
            $('#TableProducto').dataTable({
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
            $('select[name*="tblProducto_length"]').change();
            $('#contenido-tabla-producto').show('slow');
        },
        error: function (errormessage) {
            alertify.error("Buscar productos\n" + errormessage.responseText);
        }
    });
    }
    else {
        alertify.error("La identificación ingresada no coincide con un formato valido");
    }
}
function EditarProductoCreditro(IdCredito, IdSolicitud, MontoProducto, MontoMaximo, NombreProducto, Cuota, PlazoDias , Frecuencia) {
    
    $("#txtIdSolicitud").val(IdSolicitud);
    $("#txtIdCredito").val(IdCredito);

    $("#ddlMontoCredito").val(MontoProducto);
    $("#txtMontoMaximo").val(MontoMaximo);  
    $("#ddlProducto").val(NombreProducto);
    $("#textCuota").val(Cuota);

    llenarComboboxPlazoProducto(PlazoDias, MontoProducto);
    llenarComboboxFrecuencia(Frecuencia);

    $("#modalCambioProductoCredito").modal('show');
}
function cerrarModal() {
    $("#modalCambioProductoCredito").modal('hide'); 
}
function calcularProducto() {
    if ($('#ddlMontoCredito').val() > 0 && $('#ddlPlazoPago').val() > 0 && $('#ddlFrecuencia').val() > 0) {
        var objProcesar = {
            MontoCredito: $('#ddlMontoCredito').val(),
            IdPlazoCredito: $('#ddlPlazoPago').val(),
            IdFrecuenciaCredito: $('#ddlFrecuencia').val()
        };
        $.ajax({
            url: "/Colocacion/getProductoSolicitud",
            data: JSON.stringify(objProcesar),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#ddlProducto').val(result[0].NombreProducto);
                $('#textCuota').val(result[0].Cuota);
                ocultarBloqueo();
            },
            error: function (errormessage) {
                alertify.error("llenarComboboxPlazoProducto\n" + errormessage.responseText);
            }
        });
    }
    else {
        $('#ddlProducto').val('');
        $('#textCuota').val('');
    }
}
//cargar la frecuencia para los comboBox de la ventana de colocacion
function llenarComboboxProducto(IdSolicitud) {
    //llenarComboboxFrecuencia();
    var objProcesar = {
        IdSolicitud: IdSolicitud
    };
    $.ajax({
        url: "/Colocacion/CargaMontoProducto",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlMontoCredito").html("");
            $("#ddlMontoCredito").append(new Option("--- Seleccione ---", "-1"));

            $.each(result, function (key, item) {
                $("#ddlMontoCredito").append(new Option(item.Descripcion, item.IdMontoCredito));
            });

            $("#ddlPlazoPago").html("");
            $("#ddlPlazoPago").append(new Option("--- Seleccione ---", "-1"));

            $("#ddlFrecuencia").html("");
            $("#ddlFrecuencia").append(new Option("--- Seleccione ---", "-1"));

        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("llenarComboboxProducto\n" + errormessage.responseText);
        }
    });
}
function llenarComboboxPlazoProducto(PlazoDias, MontoProducto) {
    var objProcesar = {
        MontoCredito: $("#ddlMontoCredito").val()
    };
    $.ajax({
        url: "/Colocacion/CargaComboPlazoProducto",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlPlazoPago").html("");
            $("#ddlPlazoPago").append(new Option("--- Seleccione ---", -1));

            $.each(result, function (key, item) {
                $("#ddlPlazoPago").append(new Option(item.Descripcion, item.IdPlazoCredito));
            });

            //setamos el plazo por defecto
            setIdPlazoCredito(PlazoDias, MontoProducto);
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxPlazoProducto\n" + errormessage.responseText);
        }
    });
}
function llenarComboboxFrecuencia(Frecuencia) {
    var objProcesar = {
        IdMontoCredito: $("#ddlMontoCredito").val()
    };
    $.ajax({
        url: "/Colocacion/CargaComboFrecuenciaCredito",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlFrecuencia").html("");
            $("#ddlFrecuencia").append(new Option("--- Seleccione ---", "-1"));

            $.each(result, function (key, item) {
                $("#ddlFrecuencia").append(new Option(item.Descripcion, item.IdFrecuencia));
            });
            if ($("#ddlMontoCredito").val() > 6) {
                $("#ddlFrecuencia option[value='1']").remove();
            }
            //obtenemos el id de frecuencia para seleccionarlo por default
            var idFrecuencia = getIdFrecuecia(Frecuencia,);
            $("#ddlFrecuencia").val(idFrecuencia);
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxFrecuencia\n" + errormessage.responseText);
        }
    });
}
//metodo para actualiza/Guardar el nuevo producto
function GuardarProductoSolicitud() {
    if ($('#ddlProducto').val() == '') {
        alert("El nuevo producto no se a calcula, verifique que esten los datos completos");
    }
    else{
        var objProcesar = {
            P_CREDITO: $("#txtIdCredito").val(),
            N_Producto: $("#ddlProducto").val()
        };
        $.ajax({
            url: "/Colocacion/GuardaProductoSolicitud",
            data: JSON.stringify(objProcesar),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                alertify.success(result);
                Buscar();
                cerrarModal();
            },
            error: function (errormessage) {
                alertify.error("GuardaProductoSolicitud:\n" + errormessage.responseText);
            }
        });
    }
}
//metodos para calcular el id de frecuenacia y de plazoCredito
function setIdPlazoCredito(plazoDias, MontoProducto) {
    var objProcesar = {
        PlazoDias: plazoDias,
        MontoCredito: MontoProducto
    };
    $.ajax({
        async: false,
        url: "/Colocacion/getIdPlazoCredito/",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlPlazoPago").val(result[0].IdPlazoCredito);
        },
        error: function (errormessage) {
            return null;
            alertify.error("llenarComboboxFrecuencia\n" + errormessage.responseText);
        }
    });

}
function getIdFrecuecia(Frecuencia) {
    switch (Frecuencia) {
        case 'QUINCENAL':
            return 2;
            break;
        case 'MENSUAL':
            return 3;
            break;
    }
}
function cerrarTablaProductos() {
    $('#contenido-tabla-producto').hide('slow');
    $('#txtIdentificacion').val("");
}