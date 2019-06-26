
var resultadoBusqueda = null;
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
    $('body').css("background-image", "none");
});

$("#btnSaltar").click(function () {
    alertify.confirm("¿Desea Saltar la cola?.",
        function () {
            actualizarColaAutomaticaSig();
        });
});
$("#btnIniciarCola").click(function () {
    iniciarCola();
});

//$("#txIdentificacion").keyup(function (e) {
//    if (e.keyCode == 13) {
//        buscar();
//    }
//});

function buscarColaKey(e) {
    if (e.keyCode == 13) {
        if (e.currentTarget.id == 'txtPrimerNombre' || e.currentTarget.id == 'txtSegundoNombre' || e.currentTarget.id == 'txtPrimerApellido' || e.currentTarget.id == 'txtSegundoApellido') {
            BuscarNombre('ConsultaCobros');
        }
        if (e.currentTarget.id == 'txIdentificacion') {
            buscar();
        }
        if (e.currentTarget.id == 'txIdentificacionConsulta') {
            buscarConsulta('ConsultaCobros');
        }
    }
}

function buscar(origen=null) {
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

        TraeEncabezado($("#txIdentificacion").val(), null, origen);
    }
}
function buscarConsulta(origen = null) {
    if ($("#txIdentificacionConsulta").val() == "") {
        alertify.set('notifier', 'position', 'top-center');
        alertify
            .warning("Digite la identificación.");
        $("#txIdentificacionConsulta").focus();
        return false;
    }
    if ($("#ColaIniciada").val() == "1") {
        DetenerCola();
    } else {
        TraeEncabezado($("#txIdentificacionConsulta").val(), null, origen);
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

	//if ($("#ddlResultadoLlamada option:selected").val() == "PRP") {
	//	var res = ValidaFecha();
	//	if (res == false) {
	//		alertify.error("Ha seleccionado una fecha menor a la actual!!!");
	//		return;
	//	}
	//}

    var resDetalle = validateDetalle();
    if (resDetalle == false) {
        return;
    }

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        FechaPP: $("#txtFechaPP").val(),
        MontoPP: $("#txtMontoPP").val(),
        Detalle: $("#txtDetalle").val(),
        NotaPermanente: $("#txtComentario").val(),
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
            if (result.Mensaje == "ErrFecDias") {
                alertify.error("Ha seleccionado una fecha de promesa de pago mayor a 40 dias!!!!!!");
            }
            else if (result.Mensaje == "ErrMonto") {
                alertify.error("El monto es incorrecto!!!!!!");
            }
            else if (result.Mensaje == "ErrFec") {
                alertify.error("La fecha ingresada es menor a la actual!!!!!!");
            }
            else if (result.Mensaje == "Err") {
                alertify.error("Error inesperado, Por favor Contacte al administrador del sistema!!!!!!");
            }
            else if (result.Mensaje == "ErrBD") {
                alertify.error("Error inesperado en la Base de Datos, Por favor Contacte al administrador del sistema!!!!!!");
            } else {
                if (result.Resultado == true) {
                    cargarPromesasPagos($('#lblCodCliente').text());
                    alertify.confirm(result.Mensaje +"  ¿Desea continuar?.",
                        function () {
                            if ($("#ColaIniciada").val() == "1") {
                                actualizarColaAutomatica();
                                InfoPersonal("ocultar");
                                clearAll();
                                Reprogramaciones();
                                CargarPendientesMontoCola();
                                CargarProcesadoMontoCola();
                            }
                            else {
                                InfoPersonal("ocultar");
                                clearAll();
                            }
                        },
                        function () {
                            $("#ColaIniciada").val("0");
                        });
                }                
            }
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function actualizarColaAutomaticaSig() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        Bandera: 1
    };

    //localStorage.removeItem("IdCredito");

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
            if (result[0].ResultadoSig == "0") {

                alertify.confirm("Para Continuar, Debe Realizar Gestión a la Cuenta",
                    function () {
                        //actualizarColaAutomaticaSig();
                    });


            }
            else {
                actualizarColaAutomatica();
            }

        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}

function actualizarColaAutomatica_Reprogramar() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        Bandera: 3,
        FechaReprogramacion: $("#txtFechaRep").val()
    };

    //localStorage.removeItem("IdCredito");

    $.ajax({
        processing: true, // for show progress bar

        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Cobros/ActualizaReprogramacion",
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

function actualizarColaAutomatica() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        Bandera: 2
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
            //INICIO FCAMACHO 04/12/2018 
            //PROCESO DE GUARDAR EN TABLA LOS DATOS OBTENIDOS PREVIAMENTE ANTES DE DETENER LA COLA 
            if ($("#ColaIniciada").val() == "1")//SI ESTA EN GESTION DE COLA
            {
                guardarTemp();
            }
            //FIN FCAMACHO 04/12/2018
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
            if (result.length == 0) {
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

function TraeEncabezado(ident, idCred,origen=null) {


    $('#Reprogramaciones').modal('hide');

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
        url: "/Cobros/BuscarCliente",
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
    $('input[type="text"]').val('');
    $("#lblidentificacion").text("");
    $("#lblNombre").text("");
    $("#lblNombre").text("");
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

    $("#imgCliente").attr
        ('src', "/Images/noImage.jpg");
}

function Reprogramaciones() {

    //validamos si existen los controles de reprogramaciones
    //este script se usa tambien en la ventana de consulta de cobros y ahi no existen la reprogramaciones
    if ($('#spanbag').length) {
        $('.reprogr').html('');
        processing: true; // for show progress bar  
        serverSide: true; // for process server side  
        filter: true; // this is for disable filter (search box)  
        orderMulti: false; // for disable multiple column at once
        paging: false;
        $.ajax({
            url: "/Cobros/Reprogramaciones",
            type: "POST",
            data: JSON.stringify(),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                var html = '';
                $.each(data, function (key, item) {

                    document.getElementById("spanbag").textContent = item.Cantidad;

                    if (item.IdCredito > 0) {

                        html += '<tr>';
                        html += '<td>' + item.IdCredito + '</td>';
                        html += '<td>' + item.Identificacion + '</td>';
                        html += '<td>' + item.AgenteAsignado + '</td>';
                        html += '<td>' + item.Fecha + '</td>';
                        html += '<td>' + "<a href='#' class='btn btn-primary' onclick= TraeEncabezado('" + item.Identificacion + "','" + item.IdCredito + "'); >Procesar</a>" + '</td>';
                        html += '</tr>';

                    }

                });

                $('.reprogr').html(html);

                //$("#ContacTable2").DataTable();
                $("select[name*='UserTable_length']").change();
            },
            error: function (errormessage) {
            alertify.error('Ocurrio un Error con reprogramaciones');

            }
        });
    }
}
function renderizarTablaTelefonos(arr,cedula) {
    var html = '';
    $.each(arr, function (key, item) {
        console.log('El item es');
        console.log(item);
        html += '<tr>';
        html += '<td>' + item.Categoria + '</td>';
        html += '<td>' + item.FuenteDatos + '</td>';
        html += '<td>' + item.Relacion + '</td>';
        html += '<td>' + item.Telefono + '</td>';
        if (item.FechaDato) {
            html += '<td>' + item.FechaDato.substring(0, 10) + '</td>';
        } else {
            html += '<td></td>';
        }
        let trId = `td-contact-${key}`;
        html += `<td class="estrella-calificacion" style='cursor:pointer;' id="${trId}">`;
        for (let i = 0, max = 5; i < max; i++) {
            if (i < item.calificacion) {
                html += `<i class="fas fa-star" onclick="actualizarCalificacion(${item.IdTelefono},${i + 1},'${trId}','${cedula}')"></i>`;
            } else {
                html += `<i class="far fa-star" onclick="actualizarCalificacion(${item.IdTelefono},${i + 1},'${trId}','${cedula}')"></i>`;
            }
        }
        html += `</td><td><a href="#" class="btn btn-primary" onclick="getTelefono(${item.IdTelefono},${item.calificacion})">Editar</a></td></tr>`;
    });


    $('.contact').html(html);

    $("#ContacTable").DataTable();
    $("select[name*='UserTable_length']").change();
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
        success: function (r) {
            renderizarTablaTelefonos(r, ident);



        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error al Cargar los Contactos');

        }
    });
}
//AVARGAS - 23/01/2019 - Mantenimiento de Teléfonos del cliente --INICIO--
function getCatalogoTelefono() {
    var id = 0;
    $.ajax({
        url: "/Cobros/ConsultaCatalogoTel/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $("#ddlEditPredeterminado").html("");
            $("#ddlEditPredeterminado").append(new Option("--Seleccione--", "0"));

            $("#ddlNuevoPredeterminado").html("");
            $("#ddlNuevoPredeterminado").append(new Option("--Seleccione--", "0"));
            $.each(result, function (key, item) {
                $("#ddlEditPredeterminado").append(new Option(item.Descripcion, item.Id));
            });
            $.each(result, function (key, item) {
                $("#ddlNuevoPredeterminado").append(new Option(item.Descripcion, item.Id));
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
function getTelefono(id,calificacion) {
    $('#EditId').css('border-color', 'lightgrey');
    $('#ddlEditPredeterminado').css('border-color', 'lightgrey');
    $('#EditRelacion').css('border-color', 'lightgrey');
    $('#EditTelefono').css('border-color', 'lightgrey');
    getCatalogoTelefono();
    $.ajax({
        url: "/Cobros/ObtieneTelefono/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $('#EditId').val(result.IdTelefono);
            $('#ddlEditPredeterminado').val(result.Predeterminado);
            $('#EditTelefono').val(result.Telefono);
            $('#EditRelacion').val(result.Relacion);
            $('#myModalEdit').modal('show');
            $('#btnEditUpdate').show();
            $('#btnAdd').hide();
            $('#myModalEdit').data('calificacion', calificacion);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//function for updating employee's record
function ActualizarTelefono(calificacion) {
    var res = validateTelEdit();
    if (res == false) {
        return;
    }
    var empObj = {
        IdTelefono: $('#EditId').val(),
        Relacion: $('#EditRelacion').val(),
        Telefono: $('#EditTelefono').val(),
        Predeterminado: $('#ddlEditPredeterminado option:selected').val(),
        calificacion
    };
    $.ajax({
        url: "/Cobros/ActualizarTelefono",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //loadData();
            $('#myModalEdit').modal('hide');
            $('#EditRelacion').val("");
            $('#EditTelefono').val("");
            $('#ddlEditPredeterminado').val("");
            if (result.Mensaje == "Ok") {
                alertify.alert("Id: " + $('#EditId').val() + " Se actualizo", function () {
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
                    $('#EditId').val("");
                });
                cargarContactos($('#lblidentificacion').text());
            }
            else {
                alertify.error("Error al Actualizar el teléfono!!!!!");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//function for create employee's record
function CrearTelefonoNuevo() {
    var res = validateTelNuevo();
    if (res == false) {
        return;
    }
    var empObj = {
        Identificacion: $('#lblidentificacion').text(),
        FuenteDatos: "Ingresado por Asesor",
        Tipo: $('#ddlNuevoTipoTEL option:selected').val(),
        Relacion: $('#NuevoRelacion').val(),
        Telefono: $('#NuevoTelefono').val(),
        Predeterminado: $('#ddlNuevoPredeterminado option:selected').val()
    };
    $.ajax({
        url: "/Cobros/IngresarTelefono",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myModalNuevo').modal('hide'); btnCrear
            $('#NuevoFuenteDatos').val("");
            $('#ddlNuevoTipoTEL').val("");
            $('#NuevoRelacion').val("");
            $('#ddlNuevoPredeterminado').val("");
            if (result.Mensaje == "Ok") {
                alertify.alert("Se ha creado el registro para el teléfono: " + $('#NuevoTelefono').val() + ".", function () {
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
                $('#NuevoTelefono').val("");
                cargarContactos($('#lblidentificacion').text());
            }
            else {
                alertify.error("El número de teléfono ya existe!!!");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function DeleleTelefono(ID) {
    alertify.confirm("¿Desea Continuar?",
        function () {
            $.ajax(
                {
                    url: "/Cobros/EliminaTelefono",
                    type: "POST",
                    data: {
                        ID
                    },
                    success: function (result) {
                        alertify
                            .alert(result.Mensaje);
                        location.reload();
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
//AVARGAS - 23/01/2019 - Mantenimiento de Teléfonos del cliente --FINAL--
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
                html += '<td>' + item.Usuario + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });


            $('.Histo').html(html);

            $("#HistoTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error con la Carga del Historico de Gestiones');

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
                html += '<td>' + item.Usuario + '</td>';
                html += '<td class="Promesa ' + item.EstadoPromesaPago +'">' + item.EstadoPromesaPago + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

            $('.HistoPag').html(html);

            $("#PromPagTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error con la Carga de Promesas de Pago');

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
            alertify.error('Ocurrio un Error al Cargar los Pagos');

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
            alertify.error('Ocurrio un Error al cargar los Pendientes Monto Cola');

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
            alertify.error('Ocurrio un Error al Procesar Monto Cola');

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
				html += '<td>' + item.MontoCargoMora + '</td>';
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
            alertify.error('Ocurrio un Error con la Carga de Datos de Credito');

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

                $("#imgClienteCartera").attr('src', image);


            } else $("#imgClienteCartera").attr('src', "/Images/noImage.jpg");
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error la Cargar la Imagen');

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
            alertify.error('Ocurrio un Error con la Carga del Grid de Cobro');

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
            $("#PrestamosClientesTable").dataTable().fnClearTable();
            $("#PrestamosClientesTable").dataTable().fnDestroy();

            $('.GridPrestamos').html(html);
            $("#PrestamosClientesTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error al Cargar los Prestamos del Cliente');

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
            alertify.error('Ocurrio un Error al Cargar Los Prestamos de Parientes');

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
        url: "/Cobros/calculaCuota",
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
            alertify.error('Ocurrio un Error al Calcular Cuota');

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
        url: "/Cobros/ReferenciasPersonales",
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
            alertify.error('Ocurrio un Error al Cargar Referencias Personales');

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
        url: "/Cobros/CorreoPersona",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#lblCorreo").text(data[0].Correo);
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error con el Email de la Persona');

        }
    });
}


//Icortes llamada automatica

function IniciaLLamada() {

    var empObj = {
        IdCredito: localStorage.getItem("IdCredito"),
        IdentificacionCliente: $("#lblidentificacion").text(),
        TelefonoCelCliente: $("#lblTelMovil").text()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/LlamadaAutomaticaIniciar",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            alertify
                .alert("Llamando, favor espere...", function () {

                });
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error al Iniciar la Llamada');

        }
    });

}
//INICIO FCAMACHO 04/12/2018  GESTION DE COBROS TEMPORAL

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
        url: "/Cobros/consultarInfoGestionTemp",
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

function guardarTemp() {

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
        url: "/Cobros/GuardarGestionCobroTemp",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });

    $("#txtFechaPP").val("");
    $("#txtMontoPP").val("");
    $("#ddlAccionLlamada").val("-1");
    $("#ddlResultadoLlamada").val("-1");
    $("#txtDetalle").val("");
}

//FIN FCAMACHO 04/12/2018


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
        url: "/Cobros/HistoricoPlanPagos",
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
				html += '<td>' + item.MontoCargoMora + '</td>';
                html += '</tr>';
            });
            $('#PlanPagoTable').dataTable().fnClearTable();
            $('#PlanPagoTable').dataTable().fnDestroy();
            $('.planPagos').html(html);

            $("#PlanPagoTable").DataTable();
            $("select[name*='PlanPagoTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrio un Error al cargar el plan de pagos');

        }
    });
}


//Valdidation using jquery
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
//INICIO FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES 
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
        url: "/Cobros/ContactosDireccion",
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
//FINAL FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES 
//INICIO AVARGAS a 24/04/2019 MANTENIMIENTO DIRECCIONES 
function getCatalogoDireccion() {
    var id = 0;
    $.ajax({
        url: "/Cobros/ConsultaCatalogoDIR/" + id,
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
        url: "/Cobros/ObtieneDireccion/" + id,
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
//function for updating employee's record
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
        url: "/Cobros/ActualizarDireccion",
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
//function for create employee's record
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
        url: "/Cobros/CrearDireccion",
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
//FINAL AVARGAS 24/04/2019 MANTENIMIENTO DIRECCIONES 
//Valdidation using jquery
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
function mostrarInfo(resultado){
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
    $("#txtComentario").val(resultado.NotaPermanente);
    InfoPersonal('mostrar');
    ResultLlamada();
    comboTipos();
    cargarContactos(resultado.Identificacion);
    //INICIO FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES 
    cargarContactos_Direccion(resultado.Identificacion);
    //FINAL FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES
    Reprogramaciones();
    cargarHistoricosGestiones(resultado.IdCredito);
	cargarHistoricosGestionesAutomatica(resultado.IdCredito);
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
        html += '<tr id="' + item.IdCredito + '" onclick="mostrarConsultaSeleccionada('+indice +')">';
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
function mostrarConsultaSeleccionada(indice) {
    TraeEncabezado(resultadoBusqueda[indice].Identificacion, resultadoBusqueda[indice].IdCredito);
}
//INICIO FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO

function imprimirNotifCobro() {

    var total = 0;

    var table = document.getElementById('tblDatosCreditos'),
        rows = table.getElementsByTagName('tr'),
        i, j, cells, customerId;

    for (i = 0, j = rows.length; i < j; ++i) {
        cells = rows[i].getElementsByTagName('td');
        if (!cells.length) {
            continue;
        }
        total = cells[5].innerHTML;
    }

    var Obj = {
        Identificacion: $('#lblidentificacion').text(),
        Nombre: $('#lblNombre').text(),
        TelefonoDomicilio: $('#lblTelCasa').text(),
        DireccionDomicilio: $('#txtDirecion').text(),
        LugarTrabajo: $('#lblTrabajoNombre').text(),
        TelefonoTrab: $('#lblTelTrabajo').text(),
        DireccionTrabajo: $('#txtDirecionTrabajo').text(),
        Total: total,
        IdCredito: $('#lblCodCliente').text()
    };
    $.ajax({
        url: "/Cobros/ImprimirNotificacionCobro",
        data: JSON.stringify(Obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Mensaje == "OK") {
                descargarNotifCobroPDF(result);
                descargarNotifCobroDOCX(result);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function descargarNotifCobroPDF(Obj) {

    var req = new XMLHttpRequest();
    req.open("POST", "/Cobros/DescargarNotifCobroPDF?ruta=" + Obj.RutaTemp.replace('.docx', '.pdf'));
    req.responseType = "blob";
    req.onreadystatechange = function () {
        if (req.readyState === 4 && req.status === 200) {
            if (typeof window.navigator.msSaveBlob === 'function') {
                window.navigator.msSaveBlob(req.response, Obj.RutaTemp);
                //Obj.Arch_Eliminar = 'pdf';
                //EliminarNotifCobro(Obj);
            } else {
                var blob = req.response;
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = Obj.NombrePDF;
                document.body.appendChild(link);
                link.click();
            }
        }
    }; req.send()
}

function descargarNotifCobroDOCX(Obj) {

    var req = new XMLHttpRequest();
    req.open("POST", "/Cobros/DescargarNotifCobroDOCX?ruta=" + Obj.RutaTemp);
    req.responseType = "blob";
    req.onreadystatechange = function () {
        if (req.readyState === 4 && req.status === 200) {
            if (typeof window.navigator.msSaveBlob === 'function') {
                window.navigator.msSaveBlob(req.response, Obj.RutaTemp);
                //Obj.Arch_Eliminar = 'docx';
                //EliminarNotifCobro(Obj);
            } else {
                var blob = req.response;
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = Obj.NombreDOCX;
                document.body.appendChild(link);
                link.click();
            }
        }
    }; req.send()
}

function EliminarNotifCobro(Obj) {
    $.ajax({
        url: "/Cobros/EliminarNotifCobro",
        data: JSON.stringify(Obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            if (result.Mensaje == "OK") {
                alert("Documentos generados correctamente");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });


}//FIN FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO


function InfoPersonal(accion) {
    if (accion == 'mostrar') {
        $("#item-info-personal").show("bounce", { times: 4 }, "slow");
    }
    if (accion == 'ocultar') {
        $('#item-info-personal').hide("bounce", { times: 4 }, "slow");
    }

}
function MostrarFiltros(accion = null) {
    //limpiamos los campos del filtro
    $('#txtPrimerNombre').val('');
    $('#txtSegundoNombre').val('');
    $('#txtPrimerApellido').val('');
    $('#txtSegundoApellido').val('');

    //validamos si el elemento esta oculto 
    if ($('#filtro-busqueda').is(':visible') || accion == 'Ocultar') {
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
function cargarHistoricosGestionesAutomatica(ident) {

    var empObj = {
        IdCredito: ident,
    };
    $('.HistoAuto').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Cobros/HistoricoGestionesAutomaticas",
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
                html += '<td>' + item.Usuario + '</td>';
                // html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });


            $('.HistoAuto').html(html);

            $("#HistoTableAutom").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}
function actualizarCalificacion(idTelefono, calificacion,elId,cedula) {
    let html = '';
    for (let i = 0, max = 5; i < max; i++) {
        if (i < calificacion) {
            html += `<i class="fas fa-star" onclick="actualizarCalificacion(${idTelefono},${i + 1},'${elId}')"></i>`;
        } else {
            html += `<i class="far fa-star" onclick="actualizarCalificacion(${idTelefono},${i + 1},'${elId}')"></i>`;
        }
    }
    $('#' + elId).html(html);
    fetch('/Cobros/actualizarCalificacion', {
        method:'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            idTelefono,
            calificacion
        })
    }).then(res => res.json()).then(res => {
        if (res.ok) {
            alertify.success('calificación actualizada correctamente');
            cargarContactos(cedula);
        } else {
            alertify.error('Error al tratar de actulizar calificación');
            }
        }).catch(e => {
            alertify.error('A ocurrido un error inesperado');
            throw e;
        });
}