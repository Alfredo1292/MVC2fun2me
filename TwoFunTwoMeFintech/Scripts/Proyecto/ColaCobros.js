
$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    e.target // newly activated tab
    e.relatedTarget // previous active tab
    switch (e.target.innerHTML) {
        case "secondTab":
            break;
    }
    //if (!e.target.innerHTML == 'Nuevo')
    //    $('#fisrtTab').css("display", "inline");
    //else $('#fisrtTab').css("display", "none");
    //if (e.target.innerHTML == 'Usuarios')
    //    $('#fisrtTab').css("display", "inline");
});


$(document).ready(function () {
    CargarPendientesMontoCola();
    CargarProcesadoMontoCola();
});

$("#btnSaltar").click(function () {
    alertify.confirm("¿Desea Saltar la cola?.",
        function () {
            actualizarColaAutomatica();
        });
});
$("#btnIniciarCola").click(function () {
    iniciarCola();
});

function buscar() {
    if ($("#txIdentificacion").val() == "") {
        alertify.set('notifier', 'position', 'top-center');
        alertify
            .warning("Digite la identificación.");

        $("#txIdentificacion").focus();
        return false;
    }
    if ($("#ColaIniciada").val() == "1") {
        DetenerCola();
    } else {

        TraeEncabezado($("#txIdentificacion").val(), null);
    }
}

function iniciarCola() {
    alertify.confirm("¿Desea Iniciar la cola?.",
        function () {
            $("#ColaIniciada").val("1");
            buscarCola();
        },
        function () {
            $("#ColaIniciada").val("0");
        });
}

function guardar() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        FechaPP: $("#txtFechaPP").val(),
        MontoPP: $("#txtMontoPP").val(),
        Detalle: $("#txtDetalle").val(),
        AccionLlamada: $("#ddlAccionLlamada option:selected").val(),
        RespuestaGestion: $("#ddlResultadoLlamada option:selected").val(),
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/GuardarGestionCobro",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            alertify.confirm("¡¡Gestión Realizada!!, ¿Desea continuar?.",
                function () {
                    if ($("#ColaIniciada").val() == "1") {
                        actualizarColaAutomatica();
                        clearAll();
                        CargarPendientesMontoCola();
                        CargarProcesadoMontoCola();
                    }
                },
                function () {
                    $("#ColaIniciada").val("0");
                });

        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function actualizarColaAutomatica() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito")
    };

    localStorage.removeItem("IdCredito");

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/ActualizaColaAutomatica",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if ($("#ColaIniciada").val() == "1") {
                buscarCola();
            }
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function DetenerCola() {
    alertify.confirm("¿Desea Detener la cola?.",
        function () {
            $("#ColaIniciada").val("0");

            TraeEncabezado($("#txIdentificacion").val(), null);
        },
        function () {
            $("#ColaIniciada").val("1");
        });
}

function buscarCola() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/BuscarCola",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.length==0) {
                alertify
                    .alert("La cola está vacía, favor ingresar datos en la cola.", function () {
                        $("#ColaIniciada").val("0");
                    });
            } else {
                TraeEncabezado(result[0].Identificacion, result[0].IdCredito);
            }

        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function TraeEncabezado(ident, idCred) {
    var empObj = {
        Identificacion: ident,
        IdCredito: idCred
    };
    localStorage.setItem("IdCredito", idCred);
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/BuscarCliente",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#txtNombre").val(result.Nombre);
            $("#txIdentificacion").val(result.Identificacion);
            $("#txtDirecion").val(result.DetalleDireccion);
            $("#lblCodCliente").text(result.cod_cliente);
            $("#lblApertura").text(result.Apertura);
            $("#lblTelMovil").text(result.TelefonoCel);
            $("#lblTelCasa").text(result.TelefonoFijo);
            $("#lblEstadoCivil").text(result.EstadoCivil);
            $("#lblTrabajoNombre").val(result.TrabajoNombre);
            $("#lblTelTrabajo").text(result.TelefonoLaboral);
            $("#txtDirecionTrabajo").val(result.DireccionRelacionada);
            $("#lblFecMod").text(result.FechaModificacion);
            $("#lblUsuarioMod").text(result.UsrModifica);
            $("#lblConyugue").text(result.Conyugue);
            $("#lblidentificacion").text(result.Identificacion);
            $("#lblNombre").text(result.Nombre);
            ResultLlamada();
            comboTipos();
            cargarContactos(result.Identificacion);
            cargarHistoricosGestiones(result.IdCredito);
            cargarPromesasPagos(result.IdCredito);
            cargarPagos(result.IdCredito);
            cargarDatosCredito(result.IdCredito);
            cargarImage(result.Identificacion);
            cargarGridCobro(result.IdCredito);
            cargarPrestamoCliente(result.Identificacion);
            cargarPrestamoParientes(result.Identificacion);
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function comboTipos() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/comboTipo",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlAccionLlamada").html("");
            $("#ddlAccionLlamada").append(new Option("--------SELECIONE--------", "-1"));
            $.each(result, function (key, item) {
                $("#ddlAccionLlamada").append(new Option(item.Descripcion, item.IdAccion));
            });
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });

}

function ResultLlamada() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/ResultadoLLamada",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlResultadoLlamada").html("");
            $("#ddlResultadoLlamada").append(new Option("--------SELECIONE--------", "-1"));
            $.each(result, function (key, item) {

                $("#ddlResultadoLlamada").append(new Option(item.Descripcion, item.IdRespuestaGestion));
            });
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function clearAll() {
    $("#txtFechaPP").val("");
    $("#txtMontoPP").val("");
    $('#ddlAccionLlamada option:eq(-1)').attr('selected', 'selected');
    $('#ddlResultadoLlamada option:eq(-1)').attr('selected', 'selected');
    $("#txtDetalle").val("");
}

function cargarContactos(ident) {

    var empObj = {
        Identificacion: ident,
    };

    $('.contact').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/Contactos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.FuenteDatos + '</td>';
                html += '<td>' + item.Tipo + '</td>';
                html += '<td>' + item.Relacion + '</td>';
                html += '<td>' + item.Telefono + '</td>';
                html += '<td>' + item.FechaDato + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.contact').html(html);

            $("#ContacTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarHistoricosGestiones(ident) {

    var empObj = {
        IdCredito: ident,
    };
    $('.Histo').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/HistoricoGestiones",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.FechaGestion + '</td>';
                html += '<td>' + item.Detalle + '</td>';
                html += '<td>' + item.Accion + '</td>';
                html += '<td>' + item.RespuestaGestion + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.Histo').html(html);

            $("#HistoTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarPromesasPagos(ident) {

    var empObj = {
        IdCredito: ident,
    };
    $('.HistoPag').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/HistoricoPromesasPagos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.FechaGestion + '</td>';
                html += '<td>' + item.Detalle + '</td>';
                html += '<td>' + item.MontoPromesaPago + '</td>';
                html += '<td>' + item.FechaPromesaPago + '</td>';
                html += '<td>' + item.Accion + '</td>';
                html += '<td>' + item.RespuestaGestion + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.HistoPag').html(html);

            $("#PromPagTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarPagos(ident) {

    var empObj = {
        IdCredito: ident,
    };
    $('.HistoPromPago').html("");
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/HistoricoPagos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.FechaCobro + '</td>';
                html += '<td>' + item.FechaAplica + '</td>';
                html += '<td>' + item.TotalCobro + '</td>';
                html += '<td>' + item.Referencia + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.HistoPromPago').html(html);

            $("#HistoPromPagoTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function CargarPendientesMontoCola() {

    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/conultar_saldo_monto_pendiente",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#lblCantidadPendiente").text(data.CANTIDAD_PENDIENTES);
            $("#lblMontoPendiete").text(data.SALDO_PENDIENTE);
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function CargarProcesadoMontoCola() {

    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/conultar_saldo_monto_procesado",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#lblMontoPocesado").text(data.total_promesas);
            $("#lblCantidadProcesada").text(data.PROCESADOS);
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarDatosCredito(ident) {

    var empObj = {
        IdCredito: ident,
    };
    $('.HistoDatosCreditos').html("");
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/conultar_Datos_creditos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                //html += '<td>' + item.Moneda + '</td>';
                html += '<td>' + item.Producto + '</td>';
                html += '<td>' + item.DiasVenc + '</td>';
                html += '<td>' + item.Cuota + '</td>';
                html += '<td>' + item.originacion + '</td>';
                html += '<td>' + item.SaldoVenc + '</td>';

                html += '<td>' + item.SaldoTotal + '</td>';
                html += '<td>' + item.CapitalPendiente + '</td>';

                html += '<td>' + item.IntCorriente + '</td>';
                html += '<td>' + item.IntMoraPendiente + '</td>';
                html += '<td>' + item.MoraDesde + '</td>';
                html += '<td>' + item.FechaUltPago + '</td>';
                html += '<td>' + item.MontoUltPago + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.HistoDatosCreditos').html(html);

            //$("#tblDatosCreditos").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}


function cargarImage(ident) {

    var empObj = {
        Identificacion: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/mostrar_imagenes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            //~/Images/noImage.jpg
            if (data.Fotografia != "" && data.Fotografia != null) {
                var image = "data:image/png;base64," + data.Fotografia + "";

                $("#imgCliente").attr('src', image);

                $("#imgCliente").prop("alt", data.FiliacionFisica.Nombre);
            } else $("#imgCliente").attr('src', "/Images/noImage.jpg");
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarGridCobro(ident) {

    var empObj = {
        IdCredito: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/consultarGridCreditos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                //html += '<td>' + item.Moneda + '</td>';
                //103890283
                if (item.tipomovimiento == "5") {
                    html += '<td class="success">' + item.tipomovimiento + '</td>';
                    html += '<td class="success">' + item.Fechas + '</td>';
                    html += '<td class="success">' + item.CapitalPendiente + '</td>';
                    html += '<td class="success">' + item.OriginacionPendiente + '</td>';
                    html += '<td class="success">' + item.InteresPendiente + '</td>';

                    html += '<td class="success">' + item.MoraPendiente + '</td>';
                    html += '<td class="success">' + item.SaldoAlDia + '</td>';

                    html += '<td class="success">' + item.totalcobrado + '</td>';
                    html += '<td class="success">' + item.CapitalCobrado + '</td>';
                    html += '<td class="success">' + item.OriginacionCobrado + '</td>';
                    html += '<td class="success">' + item.InteresCobrado + '</td>';
                    html += '<td class="success">' + item.MoraCobrado + '</td>';
                    html += '<td class="success">' + item.SaldoTotal + '</td>';
                } else {

                    html += '<td>' + item.tipomovimiento + '</td>';
                    html += '<td>' + item.Fechas + '</td>';
                    html += '<td>' + item.CapitalPendiente + '</td>';
                    html += '<td>' + item.OriginacionPendiente + '</td>';
                    html += '<td>' + item.InteresPendiente + '</td>';

                    html += '<td>' + item.MoraPendiente + '</td>';
                    html += '<td>' + item.SaldoAlDia + '</td>';

                    html += '<td>' + item.totalcobrado + '</td>';
                    html += '<td>' + item.CapitalCobrado + '</td>';
                    html += '<td>' + item.OriginacionCobrado + '</td>';
                    html += '<td>' + item.InteresCobrado + '</td>';
                    html += '<td>' + item.MoraCobrado + '</td>';
                    html += '<td>' + item.SaldoTotal + '</td>';
                }




                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.GridCredit').html(html);
            $("#GridCreditoTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

function cargarPrestamoCliente(ident) {

    var empObj = {
        Identificacion: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/consultarPrestamosCliente",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                //html += '<td>' + item.Moneda + '</td>';
                html += '<td>' + item.IdSolicitud + '</td>';
                html += '<td>' + item.nombreProducto + '</td>';
                html += '<td>' + item.MontoProducto + '</td>';
                html += '<td>' + item.FechaCredito + '</td>';
                html += '<td>' + item.Interes_Corriente + '</td>';

                html += '<td>' + item.InteresMora + '</td>';
                html += '<td>' + item.MontoCuota + '</td>';

                html += '<td>' + item.CapitalPendiente + '</td>';

                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.GridPrestamos').html(html);
            $("#PrestamosClientesTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function cargarPrestamoParientes(ident) {

    var empObj = {
        Identificacion: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/consultarPrestamosParientes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                //html += '<td>' + item.Moneda + '</td>';
                html += '<td>' + item.Identificacion + '</td>';
                html += '<td>' + item.nombre + '</td>';
                html += '<td>' + item.CORREO + '</td>';
                html += '<td>' + item.TelefonoCel + '</td>';
                html += '<td>' + item.CapitalPendiente + '</td>';

                html += '<td>' + item.InteresMora + '</td>';
                html += '<td>' + item.MontoCuota + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.HistoFamily').html(html);
            $("#HistoFamilyTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}