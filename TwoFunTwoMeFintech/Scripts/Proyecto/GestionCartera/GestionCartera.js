//Inicio Variables de uso global
var resultadoBusqueda = null;
var creditoActivo = 0;
var ObjecPagare = null;
var ObjecLegal = null;
//Fin Variables de uso global

$(document).ready(function () {
    $('#btnMenuAcciones').click(function () {
        //Expand or collapse this panel
        $(this).next().slideToggle('fast', 'swing');
        $(this).toggleClass('activo');
    });
});
//buscar con la tecla enter
function buscarColaKey(e) {
    if (e.keyCode == 13) {
        if (e.currentTarget.id == 'txtPrimerNombre' || e.currentTarget.id == 'txtSegundoNombre' || e.currentTarget.id == 'txtPrimerApellido' || e.currentTarget.id == 'txtSegundoApellido') {
            BuscarNombre('ConsultaCobros');
        }
        if (e.currentTarget.id == 'txIdentificacion') {
            buscar('ConsultaCobros');
        }       
    }
}
function buscar(origen = null) {
    if ($("#txIdentificacion").val() == "") {
        alertify.set('notifier', 'position', 'top-center');
        alertify
            .warning("Digite la identificación.");

        $("#txIdentificacion").focus();
        return false;
    }
    if (isNaN($("#txIdentificacion").val())) {
        alertify.set('notifier', 'position', 'top-center');
        alertify
            .warning("Solo puede buscar por IdCredito o por Identificacion");
    }
    else {
        TraeEncabezado($("#txIdentificacion").val(), null, origen); 
    }
    

}
function TraeEncabezado(ident, idCred, origen = null) {
    var empObj = {
        Identificacion: ident,
        IdCredito: idCred,
        TIPO_COLA: $("#ColaIniciada").val() == '0' ? null : $("#ColaIniciada").val(),
        TipoConsulta: origen
    };
    localStorage.setItem("IdCredito", idCred);
    $.ajax({
        processing: true, // for show progress bar
        // progress=mostrarBloqueo(),
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionesCartera/BuscarCliente",
        async: true,
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result == null || result.length == 0) {
                alertify.error("No existen datos para ese criterio de busqueda.");
                clearAll();
            }
            else {
                if (result.length > 1) {
                    mostrarModalSelecionarConsulta(result);
                    MostrarFiltros('Ocultar');
                }
                else {
                    mostrarInfo(result[0]);
                    MostrarFiltros('Ocultar');
                }
            }          
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        },
        //complete: function () {

        //    ocultarBloqueo();

        //}
    });
}
function comboTipos() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionesCartera/comboTipo",
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
function clearAll() {
    //Mostramos el item de info personal
    InfoPersonal('ocultar');

    $("#txtFechaPP").val("");
    $("#txtMontoPP").val("");
    $('#ddlAccionLlamada option:eq(-1)').attr('selected', 'selected');
    $('#ddlResultadoLlamada option:eq(-1)').attr('selected', 'selected');
    $("#txtDetalle").val("");
    $('input[type="text"]').val('');
    $("#lblidentificacion").text("");
    $("#lblNombre").text("");
    $("#lblCorreo").text("");
    $("#txtComentario").val("");

    $("#txtFechaRep").val("");

    $("#lblCodCliente").text("");
    $("#lblTelMovil").text("");
    $("#lblTelCasa").text("");
    $("#lblApertura").text("");
    $("#lblEstadoCivil").text("");
    $("#lblFecMod").text("");
    $("#lblUsuarioMod").text("");
    $("#lblConyugue").text("");
    $("#lblTelTrabajo").text("");


    $('.HistoFamily').html('');
    $('.Histo').html('');
    $('.GridCredit').html
    $('.GridPrestamos').html('');
    $('.reprogr').html

    $('.contact').html('');
    $('.HistoPromPago').html('');
    $('.HistoPag').html
    $('.HistoDatosCreditos').html('');

    $("#imgClienteCartera").attr
        ('src', "/Images/noImage.jpg");
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
        url: "/GestionesCartera/Contactos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Categoria + '</td>';
                html += '<td>' + item.FuenteDatos + '</td>';
                html += '<td>' + item.Tipo + '</td>';
                html += '<td>' + item.Relacion + '</td>';
                html += '<td>' + item.Telefono + '</td>';
                html += '<td>' + item.FechaDato + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="return getTelefono(' + item.IdTelefono + ')"> Editar </a></td>';
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
        url: "/GestionesCartera/HistoricoGestiones",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';                
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.TipoGestion + '</td>';
                html += '<td>' + item.FechaIngreso + '</td>';
                html += '<td>' + item.UsuarioIngreso + '</td>';
                html += '<td>' + item.FechaAprovacion + '</td>';
                html += '<td>' + item.UsuarioAprobacion + '</td>';

                if (item.EstadoGestion == 'Completada') {
                    html += '<td class="alert alert-success">' + item.EstadoGestion + '</td>';
                }
                else {
                    if (item.EstadoGestion == 'Pendiente') {
                        html += '<td class="alert alert-warning">' + item.EstadoGestion + '</td>';
                    }
                    else {
                        if (item.EstadoGestion == 'Rechazada') {
                            html += '<td class="alert alert-danger">' + item.EstadoGestion + '</td>';
                        }
                        else {
                            html += '<td>' + item.EstadoGestion + '</td>';
                        }
                    }

                }

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
        url: "/GestionesCartera/HistoricoPromesasPagos",
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
                html += '<td>' + item.Usuario + '</td>';
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
        url: "/GestionesCartera/HistoricoPagos",
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
        url: "/GestionesCartera/conultar_Datos_creditos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Producto + '</td>';
                html += '<td>' + item.DiasVenc + '</td>';
                html += '<td>' + item.Cuota + '</td>';
                html += '<td>' + item.originacion + '</td>';
                html += '<td>' + item.SaldoVenc + '</td>';
                html += '<td>' + item.SaldoTotal + '</td>';
                html += '<td>' + item.CapitalPendiente + '</td>';
                html += '<td>' + item.IntCorriente + '</td>';
                html += '<td>' + item.IntMoraPendiente + '</td>';
                html += '<td>' + item.MontoCargoMora + '</td>';
                html += '<td>' + item.MoraDesde + '</td>';
                html += '<td>' + item.FechaUltPago + '</td>';
                html += '<td>' + item.MontoUltPago + '</td>';
                html += '</tr>';
            });
            $('#tblDatosCreditos').dataTable().fnClearTable();
            $('#tblDatosCreditos').dataTable().fnDestroy();
            $('.HistoDatosCreditos').html(html);
            $("select[name*='HistoDatosCreditos_length']").change();
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
        url: "/GestionesCartera/mostrar_imagenes",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            //~/Images/noImage.jpg
            if (data.Fotografia != "" && data.Fotografia != null) {
                var image = "data:image/png;base64," + data.Fotografia + "";

                $("#imgClienteCartera").attr('src', image);


            } else $("#imgClienteCartera").attr('src', "/Images/noImage.jpg");
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
        url: "/GestionesCartera/consultarPrestamosCliente",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
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
        url: "/GestionesCartera/consultarPrestamosParientes",
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
function calcularCuotas() {
    if ($("#txtFechaCalcular").val() == "") {
        $("#txtFechaCalcular").focus();
        return false;
    }
    var empObj = {
        idCredito: localStorage.getItem("IdCredito"),
        Fecha: $("#txtFechaCalcular").val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/calculaCuota",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#lblDiasMora").text(result.Dias_Mora);
            $("#lblMontoCalculado").text(result.TotalMora);
            $("#lblMontoCancela").text(result.TotalCancela);
            $('#CalculadoraModal').modal('show');
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function cargarReferenciasPersonales(ident) {

    var empObj = {
        Identificacion: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/ReferenciasPersonales",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                //html += '<td>' + item.Moneda + '</td>';
                html += '<td>' + item.TipoReferencia + '</td>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.SupervisorDirectoTrabajo + '</td>';
                html += '<td>' + item.Telefono + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.refPersonales').html(html);
            $("#RefPersonalesTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function EmailPersona(ident) {

    var empObj = {
        Identificacion: ident,
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/CorreoPersona",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#lblCorreo").text(data[0].Correo);
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function cargarGestionDetalle(idCred) {

    $("#txtFechaPP").val("");
    $("#txtMontoPP").val("");
    $("#ddlAccionLlamada").val("-1");
    $("#ddlResultadoLlamada").val("-1");
    $("#txtDetalle").val("");

    var empObj = {
        IdCredito: idCred
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionesCartera/consultarInfoGestionTemp",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#txtFechaPP").val(data[0].FechaPP);
            $("#txtMontoPP").val(data[0].MontoPP);
            $("#ddlAccionLlamada").val(data[0].AccionLlamada);
            $("#ddlResultadoLlamada").val(data[0].RespuestaGestion);
            $("#txtDetalle").val(data[0].Detalle);
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });

}
function cargarPlanPagos(ident) {
    var empObj = {
        IdCredito: ident,
    };
    $('.planPagos').html("");
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/HistoricoPlanPagos",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdCredito + '</td>';
                html += '<td>' + item.Cuota + '</td>';
                html += '<td>' + item.FechaVencimiento + '</td>';
                html += '<td>' + item.MontoCapital + '</td>';
                html += '<td>' + item.MontoInteres + '</td>';
                html += '<td>' + item.MontoOriginacion + '</td>';
                html += '<td>' + item.MontoIntMora + '</td>';
                html += '<td>' + item.MontoCobrado + '</td>';
                html += '</tr>';
            });
            $('#PlanPagoTable').dataTable().fnClearTable();
            $('#PlanPagoTable').dataTable().fnDestroy();
            $('.planPagos').html(html);

            $("#PlanPagoTable").DataTable();
            $("select[name*='PlanPagoTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function validateTelEdit() {
    var isValid = true;
    if ($('#EditId').val().trim() == "") {
        $('#EditId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EditId').css('border-color', 'lightgrey');
    }
    if ($('#ddlEditPredeterminado option:selected').val() == "0") {
        $('#ddlEditPredeterminado').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlEditPredeterminado').css('border-color', 'lightgrey');
    }
    if ($('#EditTelefono').val().trim() == "") {
        $('#EditTelefono').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EditTelefono').css('border-color', 'lightgrey');
    }
    return isValid;
}
function validateTelNuevo() {
    var isValid = true;
    if ($('#ddlNuevoPredeterminado option:selected').val() == "0") {
        $('#ddlNuevoPredeterminado').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoPredeterminado').css('border-color', 'lightgrey');
    }
    if ($('#ddlNuevoTipoTEL option:selected').val().trim() == "") {
        $('#ddlNuevoTipoTEL').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoTipoTEL').css('border-color', 'lightgrey');
    }
    if ($('#NuevoTelefono').val().trim() == "") {
        $('#NuevoTelefono').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NuevoTelefono').css('border-color', 'lightgrey');
    }
    return isValid;
}
function MostrarModalNuevoTelefono() {
    getCatalogoTelefono();
    $('#myModalNuevo').modal('show');
    $('#btnCrear').show();
    $('#btnAdd').hide();
}
function cargarContactos_Direccion(ident) {
    var empObj = {
        Identificacion: ident,
    };
    $('.Direccion').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/ContactosDireccion",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Categoria + '</td>';
                html += '<td>' + item.FuenteDatos + '</td>';
                html += '<td>' + item.Tipo + '</td>';
                html += '<td>' + item.Relacion + '</td>';
                html += '<td>' + item.Direccion + '</td>';
                html += '<td>' + item.FechaDato + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="return getDireccion(' + item.IdDireccion + ')"> Editar </a></td>';
                html += '</tr>';
            });
            $('.Direccion').html(html);
            $("#ContacDireccionTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');
        }
    });
} 
function getCatalogoDireccion() {
    var id = 0;
    $.ajax({
        url: "/GestionesCartera/ConsultaCatalogoDIR/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $("#ddlEditPredeterminadoDIR").html("");
            $("#ddlEditPredeterminadoDIR").append(new Option("--Seleccione--", "0"));
            $("#ddlNuevoPredeterminadoDIR").html("");
            $("#ddlNuevoPredeterminadoDIR").append(new Option("--Seleccione--", "0"));
            $.each(result, function (key, item) {
                $("#ddlEditPredeterminadoDIR").append(new Option(item.Descripcion, item.Id));
            });
            $.each(result, function (key, item) {
                $("#ddlNuevoPredeterminadoDIR").append(new Option(item.Descripcion, item.Id));
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
function getDireccion(id) {
    $('#EditIdDIR').css('border-color', 'lightgrey');
    $('#ddlEditPredeterminadoDIR').css('border-color', 'lightgrey');
    $('#EditFuenteDatosDIR').css('border-color', 'lightgrey');
    $('#EditTipoDIR').css('border-color', 'lightgrey');
    $('#EditDireccion').css('border-color', 'lightgrey');
    getCatalogoDireccion();
    $.ajax({
        url: "/GestionesCartera/ObtieneDireccion/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $('#EditIdDIR').val(result.IdDireccion);
            $('#ddlEditPredeterminadoDIR').val(result.Predeterminado);
            $('#EditFuenteDatosDIR').val(result.FuenteDatos);
            //$('#ddlEditTipoDIR').val(result.Tipo);
            $('#EditDireccion').val(result.Direccion);
            $('#EditRelacionDIR').val(result.Relacion);
            $('#myModalEditDIR').modal('show');
            $('#btnEditUpdateDIR').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
function ActualizarDireccion() {
    var res = validateDIREdit();
    if (res == false) {
        return;
    }
    var empObj = {
        IdDireccion: $('#EditIdDIR').val(),
        FuenteDatos: "Ingresado por Asesor",
        //Tipo: $('#EditTipoDIR').val(),
        Relacion: $('#EditRelacionDIR').val(),
        Direccion: $('#EditDireccion').val(),
        Predeterminado: $('#ddlEditPredeterminadoDIR option:selected').val()
    };
    $.ajax({
        url: "/GestionesCartera/ActualizarDireccion",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //loadData();
            $('#myModalEditDIR').modal('hide');
            $('#EditFuenteDatosDIR').val("");
            //$('#EditTipoDIR').val("");
            $('#EditRelacionDIR').val("");
            $('#EditDireccion').val("");
            $('#ddlEditPredeterminadoDIR').val("");
            if (result.Mensaje == "Ok") {
                alertify.alert("Id: " + $('#EditIdDIR').val() + " Se ha actualizado", function () {
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
                    $('#EditIdDIR').val("");
                });
                cargarContactos_Direccion($('#lblidentificacion').text());
            }
            else {
                alertify.error("Error al Actualizar la Dirección!!!!!");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function CrearDiereccionNuevo() {
    var res = validateDIRNuevo();
    if (res == false) {
        return;
    }
    var empObj = {
        Identificacion: $('#lblidentificacion').text(),
        FuenteDatos: "Ingresado por Asesor",
        Tipo: $('#ddlNuevoTipoDIR option:selected').val(),
        Relacion: $('#NuevoRelacionDIR').val(),
        Direccion: $('#NuevoDireccion').val(),
        Predeterminado: $('#ddlNuevoPredeterminadoDIR option:selected').val()
    };
    $.ajax({
        url: "/GestionesCartera/CrearDireccion",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myModalNuevoDIR').modal('hide'); btnCrear
            $('#NuevoFuenteDatosDIR').val("");
            $('#ddlNuevoTipoDIR').val("");
            $('#NuevoRelacionDIR').val("");
            $('#ddlNuevoPredeterminadoDIR').val("");
            if (result.Mensaje == "Ok") {
                alertify.alert("Se ha creado el registro para la Dirección", function () {
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
                });
                $('#NuevoDireccion').val("");
                cargarContactos_Direccion($('#lblidentificacion').text());
            }
            else {
                alertify.error("No se ha podido ingresar la dirección, Por favor contacte al administrador!!!");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function MostrarModalNuevoDireccion() {
    getCatalogoDireccion();
    $('#myModalNuevoDIR').modal('show');
    $('#btnCrearDIR').show();
    $('#btnAdd').hide();
}
function validateDIREdit() {
    var isValid = true;
    if ($('#EditIdDIR').val().trim() == "") {
        $('#EditIdDIR').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EditIdDIR').css('border-color', 'lightgrey');
    }
    if ($('#ddlEditPredeterminadoDIR option:selected').val() == "0") {
        $('#ddlEditPredeterminadoDIR').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlEditPredeterminadoDIR').css('border-color', 'lightgrey');
    }
    //if ($('#EditFuenteDatosDIR').val().trim() == "") {
    //	$('#EditFuenteDatosDIR').css('border-color', 'Red');
    //	isValid = false;
    //}
    //else {
    //	$('#EditTipoDIR').css('border-color', 'lightgrey');
    //}
    //if ($('#EditRelacionDIR').val().trim() == "") {
    //	$('#EditRelacionDIR').css('border-color', 'Red');
    //	isValid = false;
    //}
    //else {
    //	$('#EditRelacionDIR').css('border-color', 'lightgrey');
    //}
    if ($('#EditDireccion').val().trim() == "") {
        $('#EditDireccion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#EditDireccion').css('border-color', 'lightgrey');
    }
    return isValid;
}
function validateDIRNuevo() {
    var isValid = true;
    if ($('#ddlNuevoPredeterminadoDIR option:selected').val() == "0") {
        $('#ddlNuevoPredeterminadoDIR').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoPredeterminadoDIR').css('border-color', 'lightgrey');
    }
    if ($('#ddlNuevoTipoDIR option:selected').val().trim() == "") {
        $('#ddlNuevoTipoDIR option:selected').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoTipoDIR option:selected').css('border-color', 'lightgrey');
    }
    //if ($('#NuevoRelacionDIR').val().trim() == "") {
    //	$('#NuevoRelacionDIR').css('border-color', 'Red');
    //	isValid = false;
    //}
    //else {
    //	$('#NuevoRelacionDIR').css('border-color', 'lightgrey');
    //}
    if ($('#NuevoDireccion').val().trim() == "") {
        $('#NuevoDireccion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NuevoDireccion').css('border-color', 'lightgrey');
    }
    return isValid;
}
function ValidaFecha() {
    var isValid = true;
    var Fecha_aux = $("#txtFechaPP").val().split("-");
    var Fecha1 = new Date(parseInt(Fecha_aux[0]), parseInt(Fecha_aux[1] - 1), parseInt(Fecha_aux[2]));
    var Hoy = new Date();//Fecha actual del sistema
    var AnyoFecha = Fecha1.getFullYear();
    var MesFecha = Fecha1.getMonth();
    var DiaFecha = Fecha1.getDate();

    var AnyoHoy = Hoy.getFullYear();
    var MesHoy = Hoy.getMonth();
    var DiaHoy = Hoy.getDate();

    if (AnyoFecha < AnyoHoy) {
        isValid = false;
    }
    else {
        if (AnyoFecha == AnyoHoy && MesFecha < MesHoy) {
            isValid = false
        }
        else {
            if (AnyoFecha == AnyoHoy && MesFecha == MesHoy && DiaFecha < DiaHoy) {
                isValid = false
            }
            else {
                if (AnyoFecha == AnyoHoy && MesFecha < MesHoy && DiaFecha == DiaHoy) {
                    isValid = false
                }
            }
        }
    }

    return isValid;
}
function validateDetalle() {
    var isValid = true;
    if ($('#txtDetalle').val().trim() == "") {
        $('#txtDetalle').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtDetalle').css('border-color', 'lightgrey');
    }

    return isValid;
}
function mostrarInfo(resultado) {
    //mostrarBloqueo();
    InfoPersonal('ocultar');
    $("#txtNombre").val(resultado.Nombre);
    $("#txIdentificacion").val(resultado.Identificacion);
    $("#txtDirecion").val(resultado.DetalleDireccion);
    $("#lblCodCliente").text(resultado.IdCredito);
    $("#lblApertura").text(resultado.Apertura);
    $("#lblTelMovil").text(resultado.TelefonoCel);
    //IniciaLLamada();
    $("#lblTelCasa").text(resultado.TelefonoFijo);
    $("#lblEstadoCivil").text(resultado.EstadoCivil);
    $("#lblTrabajoNombre").val(resultado.TrabajoNombre);
    $("#lblTelTrabajo").text(resultado.TelefonoLaboral);
    $("#txtDirecionTrabajo").val(resultado.DireccionRelacionada);
    $("#lblFecMod").text(resultado.FechaModificacion);
    $("#lblUsuarioMod").text(resultado.UsrModifica);
    $("#lblConyugue").text(resultado.Conyugue);
    $("#lblidentificacion").text(resultado.Identificacion);
    $("#lblNombre").text(resultado.Nombre);

    //Mostramos el item de info personal
    InfoPersonal('mostrar');
    comboTipos();
    cargarContactos(resultado.Identificacion);
    //INICIO FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES 
    cargarContactos_Direccion(resultado.Identificacion);
    //FINAL FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES
    cargarHistoricosGestiones(resultado.IdCredito);
    cargarPromesasPagos(resultado.IdCredito);
    cargarPagos(resultado.IdCredito);
    cargarPlanPagos(resultado.IdCredito);
    cargarDatosCredito(resultado.IdCredito);
    cargarImage(resultado.Identificacion);
    //cargarGridCobro(result[0].IdCredito);
    cargarPrestamoCliente(resultado.Identificacion);
    cargarPrestamoParientes(resultado.Identificacion);
    cargarReferenciasPersonales(resultado.Identificacion);
    localStorage.setItem("IdCredito", resultado.IdCredito);

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;
    $("#txtFechaCalcular").val(today);
    //calcularCuotas();
    EmailPersona(resultado.Identificacion);

    //INICIO FCAMACHO 04/12/2018  GESTION DE COBROS TEMPORAL
    if ($("#ColaIniciada").val() == 1) {
        cargarGestionDetalle(idCred);
    }
    //FIN FCAMACHO 04/12/2018
    $("#ElegirConsulta").modal('hide');
}
function mostrarModalSelecionarConsulta(result) {
    resultadoBusqueda = result;
    var html = '';
    var indice = 0;
    $.each(result, function (key, item) {
        html += '<tr id="' + item.IdCredito + '" onclick="mostrarConsultaSeleccionada(' + item.IdCredito + ')">';
        html += '<td>' + item.Nombre + '</td>';
        html += '<td>' + item.Identificacion + '</td>';
        html += '<td>' + item.IdCredito + '</td>';
        html += '</tr>';
        indice++;
    });
    $('#tableConsultaCobros').dataTable().fnClearTable();
    $('#tableConsultaCobros').dataTable().fnDestroy();
    $('#tblConsultaCobros').html(html);
    //setear la tabla en español
    $('#tableConsultaCobros').dataTable({
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
    $('select[name*="tblConsultaCobros_length"]').change();
    //Mostramos el modal
    $("#ElegirConsulta").modal('show');
}
function mostrarConsultaSeleccionada(IdCredito) {
    TraeEncabezado(IdCredito);
}
function MostrarFiltros(accion = null) {
    //limpiamos los campos del filtro
    $('#txtPrimerNombre').val('');
    $('#txtSegundoNombre').val('');
    $('#txtPrimerApellido').val('');
    $('#txtSegundoApellido').val('');

    //validamos si el elemento esta oculto 
    if ($('#filtro-busqueda').is(':visible') || accion=='Ocultar') {
        $('#filtro-busqueda').hide('slow');
        $('#btnFiltrar span:first-child').removeClass('fa-angle-double-up');
        $('#btnFiltrar span:first-child').addClass('fa-angle-double-down');
    }
    else {
        $('#filtro-busqueda').show('slow');
        $('#btnFiltrar span:first-child').removeClass('fa-angle-double-down');
        $('#btnFiltrar span:first-child').addClass('fa-angle-double-up');
    }
}
function BuscarNombre() {
    if ($('#txtPrimerNombre').val() == '' && $('#txtSegundoNombre').val() == '' && $('#txtPrimerApellido').val() == '' && $('#txtSegundoApellido').val() == '') {
        alertify.error("Debe igresar algun creterio de busqueda");
    }
    else {
        var empObj = {
            PrimerNombre: $('#txtPrimerNombre').val(),
            SegundoNombre: $('#txtSegundoNombre').val(),
            PrimerApellido: $('#txtPrimerApellido').val(),
            SegundoApellido: $('#txtSegundoApellido').val()
        };
        $.ajax({
            processing: true, // for show progress bar
            // progress=mostrarBloqueo(),
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/GestionesCartera/BuscarClienteNombre",
            async: true,
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == null || result.length == 0) {
                    alertify.error("No existen datos para ese criterio de busqueda.");
                    clearAll();
                }
                else {
                    mostrarModalSelecionarConsulta(result);
                }
            },
            error: function (errormessage) {
                alertify.error(errormessage.responseText);
            }
        });
    }
}
function EsconderMenuAcciones() {
    //Reiniciamos los iconos
    //Reiniciar
    $('#itemReiniciar span').removeClass('fas fa-sync fa-spin');
    $('#itemReiniciar span').addClass('glyphicon glyphicon-refresh');
    //Cancelar
    $('#itemCancelar span').removeClass('fas fa-circle-notch fa-spin');
    $('#itemCancelar span').addClass('glyphicon glyphicon-remove');


        //validamos si el menu esta visible
    if ($('#acciones-cobros').is(':visible')){
        $('#btnMenuAcciones').next().slideToggle('fast', 'swing');
        $('#btnMenuAcciones').removeClass('activo');
    }
        
}
function CancelarCredito() {
    if ($('#lblCodCliente').text() != '') {
        //cambiamos el icono para el efecto de movimiento
        $('#itemCancelar span').removeClass('glyphicon glyphicon-remove');
        $('#itemCancelar span').addClass('fas fa-circle-notch fa-spin');

        var empObj = {
            IdCredito: localStorage.getItem("IdCredito"),
            Accion: 'Cancelar'
        };
        processing: true; // for show progress bar  
        serverSide: true; // for process server side  
        filter: true; // this is for disable filter (search box)  
        orderMulti: false; // for disable multiple column at once
        paging: false;
        $.ajax({
            url: "/GestionesCartera/EjecutarAccionCredito",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.Mensaje == 'COMPLETO') {
                    alertify.success('Credito Cancelado');
                    cargarHistoricosGestiones(localStorage.getItem("IdCredito"));
                }
                else {
                    alert('BD Error al Cancelar Credito:  ' + result.Mensaje);
                }
                EsconderMenuAcciones();
            },
            error: function (errormessage) {
                alertify.error('Ocurrion un Error al Cancelar Credito');
                EsconderMenuAcciones();
            }
        });
    }
    else {
        alertify.error('Debe buscar un credito antes de realizar alguna accion.');
    }
}
function ReiniciarCredito() {
    if ($('#lblCodCliente').text() != '') {
        //cambiamos el icono para el efecto de movimiento
        $('#itemReiniciar span').removeClass('glyphicon glyphicon-refresh');
        $('#itemReiniciar span').addClass('fas fa-sync fa-spin');

        var empObj = {
            IdCredito: localStorage.getItem("IdCredito"),
            Accion: 'REINICIAR'
        };
        processing: true; // for show progress bar  
        serverSide: true; // for process server side  
        filter: true; // this is for disable filter (search box)  
        orderMulti: false; // for disable multiple column at once
        paging: false;
        $.ajax({
            url: "/GestionesCartera/EjecutarAccionCredito",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.Mensaje == 'COMPLETO') {
                    alertify.success('Credito Reiniciado');
                    cargarHistoricosGestiones(localStorage.getItem("IdCredito"));
                }
                else {
                    alert('BD Error al Cancelar Credito:  ' + result.Mensaje);
                }
                EsconderMenuAcciones();
            },
            error: function (errormessage) {
                alertify.error('Ocurrion un Error al Reiniciar Credito');
                EsconderMenuAcciones();
            }
        });
    }
    else {
        alertify.error('Debe buscar un credito antes de realizar alguna accion.');
    }

}
function InfoPersonal(accion) {
    if (accion == 'mostrar') {
        $("#item-info-personal").show("bounce", { times: 3 }, "slow");
    }
    if (accion=='ocultar') {
        $('#item-info-personal').hide("bounce", { times: 3 }, "slow");
    }
}
function cargarAdministradorDocumentos() {
    var html = '';
    //Pagare
    html += '<tr>';
    html += '<td>' + 'Pagare' + '</td>';
    html += '<td><p id="existePagare"></p></td>';
    html += '<td>' + '<form class="form-inline input-folder-pagare" onSubmit="return false;" name="Fichiero" ><button type="button" id="btnCargarPagare" class="btn btn-primary"><span class="fas fa-cloud-upload-alt"></span> Cargar</button></form>' + '</td>';
    html += '<td>' + '<form class="form-inline input-folder-pagare-descarga" onSubmit="return false;" name="Fichiero" ><button type="button" id="btnDescargarPagare" onclick="descargarPagare();" class="btn btn-primary-eliminar"><span class="fas fa-cloud-download-alt"></span> Descargar</button></form>' + '</td>';
    html += '</tr>';
    //Documento legal   
    html += '<tr>';
    html += '<td>' + 'Proceso Legal' + '</td>';
    html += '<td><p id="existeLegal"></p></td>';
    html += '<td>' + '<form class="form-inline input-folder-legal" onSubmit="return false;" name="Fichiero"><button type="button" id="btnCargarLegal" class="btn btn-primary"><span class="fas fa-cloud-upload-alt"></span> Cargar</button></form>' + '</td>';
    html += '<td>' + '<form class="form-inline input-folder-legal-descarga" onSubmit="return false;" name="Fichiero"><button type="button" id="btnDescargarDemanda" onclick="descargarLegal();" class="btn btn-primary-eliminar"><span class="fas fa-cloud-download-alt"></span> Descargar</button></form>' + '</td>';
    html += '</tr>';
    $('#tableAdminDocumentos').dataTable().fnClearTable();
    $('#tableAdminDocumentos').dataTable().fnDestroy();
    $('#tblAdminDocumentos').html(html);
    //setear la tabla en español
    $('#tableAdminDocumentos').dataTable();
  
    //verificamos si existen documentos
    existeDocumento('PAGARE');
    existeDocumento('LEGAL');
    //Metodos para manejar la carga de archivos
    seleccionarArchivoPagare();
    seleccionarArchivoLegal();
}
function existeDocumento(tipoDocumento) {
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/GestionesCartera/ExisteDocumento",
        data: { tipoDocumento: tipoDocumento, identificacion: $('#lblidentificacion').text() },
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (tipoDocumento == 'PAGARE') {
                $('#existePagare').text(result);
            }
            if (tipoDocumento == 'LEGAL') {
                $('#existeLegal').text(result);
            }
        },
        error: function (errormessage) {
            alertify.error('Error al validar l existencia del documento.');
            return 'Error';
        }
    });
}
function descargarPagare() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionesCartera/DescargarDocumentoPagare?identificacion=" + $('#lblidentificacion').text(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "Json",
        success: function (result) {
            if (result.Base64 !== null) {
                if (result == "Pagare no existe") {
                    alertify.error(result);
                }
                else {
                    download(result.Base64, result.Name, result.Type);
                    alertify.success("Descarga Correcta");
                }
            }
            else {
                alertify.alert("Error al descargar el pagare");
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("Error con la descarga");
        }
    });
}
function descargarLegal() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionesCartera/DescargarDocumentoLegal?identificacion=" + $('#lblidentificacion').text(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "Json",
        success: function (result) {
            if (result.Base64 !== null) {
                if (result == "Documento Legal no existe") {
                    alertify.error(result);
                }
                else {
                    download(result.Base64, result.Name, result.Type);
                    alertify.success("Descarga Correcta");
                }
            }
            else {
                alertify.alert("Error al descargar el Documento legal");
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("Error con la descarga");
        }
    });
}
function seleccionarArchivoPagare() {
        $(".input-folder-pagare").before(
            function () {
                if (!$(this).prev().hasClass('seleccionar-pagare')) {
                    if ($('#existePagare').text() == 'SI') {
                        alertify.error('Pagare ya existe. No se puede reemplazar');
                    }
                    else {
                        var element = $("<input id='file-pagare' onchange='handleArchivoPagare(this);' type='file' class='seleccionar-pagare' style='display:none; height:0; with:0;'>");
                        $(this).find("#btnCargarPagare").click(function () {
                            element.click();
                        });
                        return element;
                    }
                }
            }
        );
}
function seleccionarArchivoLegal() {
    $(".input-folder-legal").before(
            function () {
                if (!$(this).prev().hasClass('seleccionar-legal')) {
                    var element = $("<input id='file-legal' onchange='handleArchivoLegal(this);' type='file' class='seleccionar-legal' style='display:none; height:0; with:0;'>");
                    $(this).find("#btnCargarLegal").click(function () {
                        element.click();
                    });
                    return element;
                }
            }
        );
}
function handleArchivoPagare(evt) {
    ObjecPagare = null;
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecPagare = { Base64: base64String, Name: f.name, Type: f.type, Identificacion: $('#lblidentificacion').text() }
            //Subimos el documento
            subirDocPagare(ObjecPagare)
        };
    })(f);
    reader.readAsBinaryString(f);
}
function handleArchivoLegal(evt) {
    ObjecLegal = null;
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecLegal = { Base64: base64String, Name: f.name, Type: f.type, Identificacion: $('#lblidentificacion').text() }
            //Subimos el documento
            subirDocLegal(ObjecLegal)
        };
    })(f);
    reader.readAsBinaryString(f);
}
function subirDocPagare(ObjecPagare) {
    if ($('#existePagare').text() == 'SI') {
        alertify.error('Pagare ya existe. No se puede reemplazar');
    }
    else {
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/GestionesCartera/SubirDocumentoPagare",
            type: "POST",
            data: JSON.stringify(ObjecPagare),
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                //verificamos si existen documentos
                existeDocumento('PAGARE');
                alertify.success(result);
            },
            error: function (errormessage) {
                alertify.error("Error al subir al pagare");
            }
        });
    }
}
function subirDocLegal(ObjecPagare) {
    if ($('#existeLegal').text() == 'SI') {
        alertify.error('Documento Legal ya existe. No se puede reemplazar');
    }
    else {
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/GestionesCartera/SubirDocumentoLegal",
            type: "POST",
            data: JSON.stringify(ObjecPagare),
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                //verificamos si existen documentos
                existeDocumento('LEGAL');
                alertify.success(result);
            },
            error: function (errormessage) {
                alertify.error("Error al subir al Documento Legal");
            }
        });
    }
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
            alertify.error('Ocurrio un Error con la Carga del Grid de Cobro');
        }
    });
}