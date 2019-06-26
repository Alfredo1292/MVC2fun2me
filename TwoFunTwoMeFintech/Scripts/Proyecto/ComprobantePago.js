//Expresiones regulares para validar la cedula nacional y la DIMEX
var ValidarCedula = '^[1-9]{1}[0-9]{8}$';// /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var ValidarCedulaDimex = '^[0-9]{1}[0-9]{11}$';


//function saludar(nombre) {
//    alert('Hola ' + nombre);
//}

//function procesarEntradaUsuario(callback) {
//    var nombre = prompt('Por favor ingresa tu nombre.');
//    callback(nombre);
//}

//procesarEntradaUsuario(saludar);

function BuscarComprobante() {

    var objProcesar = {
        Identificacion: $("#txtIdentificacion").val()
    };

    if ($("#txtIdentificacion").val().match(ValidarCedula) || $("#txtIdentificacion").val().match(ValidarCedulaDimex)) {
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/ComprobantePago/ConsultarComprobante",
            data: JSON.stringify(objProcesar),
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += '<tr>';
                    html += '<td>' + item.Id + '</td>';
                    html += '<td>' + item.IdCredito + '</td>';
                    html += '<td>' + item.Banco + '</td>';
                    html += '<td>' + item.NumeroComprobante + '</td>';
                    html += '<td>' + item.FechaTransferencia + '</td>';
                    html += '<td>' + item.EstadoNombre + '</td>';
                    html += '<td><a id="editarComprobante" class="btn btn-primary btn-primary-editar" onclick="EditarComPago(' + item.Id + ')"> Editar </a></td>';
                    html += '</tr>';
                });
                $('#TableComprobantePago').dataTable().fnClearTable();
                $('#TableComprobantePago').dataTable().fnDestroy();
                $('.TabComprobantePago').html(html);

                //setear la tabla en español
                $('#TableComprobantePago').dataTable({
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
                $("select[name*='TableComprobantePago_length']").change();
            },
            error: function (errormessage) {
                alertify.error("Buscar Comprobante de Pago\n" + errormessage.responseText);
            }
        });
    }
    else {
        alertify.error("La identificación ingresada no coincide con un formato valido");
    }
}
function EditarComPago(id) {


    var objProcesar = {
        Id: id,
        Identificacion: $("#txtIdentificacion").val()
    };
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/ComprobantePago/EditaComprobante",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                $("#txtId").val(item.Id);
                $("#txtNumComprobante").val(item.NumeroComprobante);
                $("#txtIdentificacionPopup").val(item.Identificacion);
                $("#txtNombreCliente").val(item.NombreCuenta);
                consultaBancos(item.IdBanco);

                $("#txtFechaTransferecia").val(item.FechaTransferencia);
                $("#ddlEstado").val(item.Estado == true ? "1" : "0");

                if (item.ImageComprobante != "" && item.ImageComprobante != null) {
                    var image = "data:image/png;base64," + item.ImageComprobante + "";

                    $("#imgCliente").attr('src', image);


                } else $("#imgCliente").attr('src', "/Images/noImage.jpg");

            });
            //$('#TableComprobantePago').dataTable().fnClearTable();
            //$('#TableComprobantePago').dataTable().fnDestroy();
            //$('.TabComprobantePago').html(html);

            //$("#TableComprobantePago").DataTable();
            //$("select[name*='TableComprobantePago_length']").change();
            $('#myModal').modal('show');
        },
        error: function (errormessage) {
            alertify.error("Buscar Comprobante de Pago\n" + errormessage.responseText);
        }
    });
}
function consultaBancos(id) {
    $("#ddlBanco").html("");
    $("#ddlBanco").append(new Option("------Seleccione------", "-1"));
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/ComprobantePago/ConsultaBancos",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                $("#ddlBanco").append(new Option(item.Descripcion, item.Id));
            });
            $("#ddlBanco").val(id);
        },
        error: function (errormessage) {
            alertify.error("Buscar el Banco\n" + errormessage.responseText);
        }
    });
}


function Update() {


    var objProcesar = {
        Id: $("#txtId").val(),
        Identificacion: $("#txtIdentificacion").val(),
        Estado: $("#ddlEstado").val() == "1" ? true : false
    };
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/ComprobantePago/MantenimientoComprobante",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {


            $('#myModal').modal('hide');

            BuscarComprobante();
        },
        error: function (errormessage) {
            alertify.error("Buscar Comprobante de Pago\n" + errormessage.responseText);
        }
    });
}