var state = 'stop';
var v_RegCedula = '^[1-9]{1}[0-9]{8}$';// /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var v_RegTexto = /^[-_\w\d\.(áéíóúñÑ)\s(!¡\?¿%\(\)\#-$\/\*;@:,\+)]*$/i;
var v_RegCedulaDimex = '^[1-9]{1}[0-9]{11}$'; //'^[1-9]{1}[0-9]{11}$' usar esta en caso que permita 11 o 12 digitos
var v_RegEntero = /^\d+$/;
var IdSolicitudActiva = 0;
var busquedaActiva = 0; //bandera para determinar si el agente esta buscando alguna solicitud
//valores por defecto para los dropdwon de informacion de producto, dependiendo del producto de la solicitud
var montoCreditoSolicutud = -1;
var plazoSolicutud = -1;
var descripcionPlazoSolicutud = '';
var montoFrecuenciaSolicutud = -1;
var statusSolicitud = 0;
var objSolicitudActual; //vaiable global con info importante de la solicitud
var next = false;
var tiempoSolicitud = 0;
var tiempo_corriendo;
var alarma;

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    e.target // newly activated tab
    e.relatedTarget // previous active tab
    switch (e.target.innerHTML) {
        case "secondTab":
            break;
    }
    ocultarBloqueo();
    //if (!e.target.innerHTML == 'Nuevo')
    //    $('#fisrtTab').css("display", "inline");
    //else $('#fisrtTab').css("display", "none");
    //if (e.target.innerHTML == 'Usuarios')
    //    $('#fisrtTab').css("display", "inline");
});


$(document).ready(function () {
    bs_input_file_frontal();
    bs_input_file_Trasera();
    bs_input_file_Domiciliacion();
    bs_input_file_Selfie();
    bs_input_file_Firma();
    buscarCatalogos();
    CargaStatusSol();
    //getPasosAgente();
    //AVARGAS 19/02/2019 Cargo el status del asesor 
    getStatusAsesor();
    //Funcion Oculatar los completos
    $('#ddHideAll').click(function () {
        oculatarPasos();
    });
    $('#tab1').click(function () {
        $("#Infotab1").addClass("active");
        $("#InfocontentTab1").addClass("active");
    });
    //pintar tabs dependiendo del progreso o el tab activo
    $('#ul-tabs-ventas li a').click(function () {
        var indiceTab = $(this).parent().index();
        $('.line').removeClass('linea-activada');
        $('.line:lt(' + indiceTab + ')').addClass('linea-activada');

        $('#ul-tabs-ventas li').removeClass('tab-activo');
        $('#ul-tabs-ventas li:lt(' + indiceTab + ')').addClass('tab-activo');
    });
    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
    $("#ColaIniciada").val("1");
    next = false;
    buscarCola();
    ocultarBloqueo();
    //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
    $("#txtCorreo").change(function () {
        validateEmail($("#txtCorreo").val());
    });

    var d = new Date();
    var date1 = (d.getDate() < 10 ? "0" + d.getDate() : d.getDate()) + '/' + ((d.getMonth() + 1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1)) + '/' + d.getFullYear();
    var date2 = d.getFullYear() + '-' + ((d.getMonth() + 1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1)) + '-' + (d.getDate() < 10 ? "0" + d.getDate() : d.getDate());
    document.getElementById("txtFechaPlanPago").setAttribute("min", date1);
    document.getElementById("txtFechaPlanPago").setAttribute("value", date2);
    //document.getElementById("txtFechaPlanPago").setAttribute("minDate", date2);
    //$("#txtFechaPlanPago").datepicker({ minDate: date2 });
    //document.getElementById("txtFechaPlanPago").setAttribute("value", date1);


});

function getProvincia(Provincia = null) {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaProvincia",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlProvincia").html("");
            $("#ddlProvincia").append(new Option("--------Seleccione--------", "-1"));

            $("#ddlCanton").html("");
            $("#ddlCanton").append(new Option("--------Seleccione--------", "-1"));

            $("#ddlDistrito").html("");
            $("#ddlDistrito").append(new Option("--------Seleccione--------", "-1"));

            $.each(result, function (key, item) {
                $("#ddlProvincia").append(new Option(item.Nombre, item.IdProvincia));
            });
                if (Provincia != null) {
                    $("#ddlProvincia").val(Provincia);
                }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("getProvincia\n" + errormessage.responseText);
        }
        });
}

function getCanton(Canton = null, Provincia = null) {

    if (Provincia == null) {
    var objBuscar = {
        IdProvincia: $("#ddlProvincia").val()
    };
    }
    else {
        var objBuscar = {
            IdProvincia: Provincia
        };
    }
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaCanton",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            $("#ddlCanton").html("");
            $("#ddlCanton").append(new Option("--------Seleccione--------", "-1"));

                $("#ddlDistrito").html("");
                $("#ddlDistrito").append(new Option("--------Seleccione--------", "-1"));
            $.each(result, function (key, item) {
                $("#ddlCanton").append(new Option(item.Nombre, item.IdCanton));
            });
                if (Canton != null) {
                    $("#ddlCanton").val(Canton);
                }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("getCanton\n" + errormessage.responseText);
        }
    });
}


function getDistrito(Distrito = null, Canton = null, Provincia = null) {
    if (Canton == null) {
    var objBuscar = {
        IdProvincia: $("#ddlProvincia").val(),
        IdCanton: $("#ddlCanton").val()
    };
    }
    else {
        var objBuscar = {
            IdProvincia: Provincia,
            IdCanton: Canton
        };
    }   
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaDistrito",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            $("#ddlDistrito").html("");
            $("#ddlDistrito").append(new Option("--------Seleccione--------", "-1"));

            $.each(result, function (key, item) {
                $("#ddlDistrito").append(new Option(item.Nombre, item.IdDistrito));
            });
                if (Distrito != null) {
                    $("#ddlDistrito").val(Distrito);
                }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("getDistrito\n" + errormessage.responseText);
        }
    });
}
function CargaDetalleProducto() {
    var objBuscar = {
        IdSolicitud: $("#txtSolicitud").val()
    };
    $.ajax({
        async: false,
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaProductos",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                $("#ddlProducto").val(result[0].NombreProducto);
                $("#textCuota").val(result[0].Cuota);
            }
            else {
                $("#ddlProducto").val('');
                $("#textCuota").val('');
            }
            //Validamos si se lleno el dropDown de MontoCredito
            if ($('#ddlMontoCredito').val() != null) {
                $('#ddlMontoCredito').val(result[0].IdMontoCredito);
            }
            cargarComboboxPlazoProductoDefault();//llenamos el combo del plazo de producto
            if ($('#ddlPlazoPago').val() != null) {
                $('#ddlPlazoPago').val(result[0].IdPlazoCredito);
            }
            if ($('#ddlFrecuencia').val() != null) {
                if (result[0].IdFrecuencia == 1) {
                    $('#ddlFrecuencia').append(new Option("Semanal", "1"));
                }
                $('#ddlFrecuencia').val(result[0].IdFrecuencia);
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("CargaProductos\n" + errormessage.responseText);
        }
    });
}
function CargaStatusSol() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaStatusSolicitud",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlEstadoSol").html("");
            //$("#ddlNacionalidad").append(new Option(item.Desc, item.IdTipo));
            $('#ddlEstadoSol').append(new Option("--------Seleccione--------", "-1"));


            $.each(result, function (key, item) {
                $("#ddlEstadoSol").append(new Option(item.NombreEstado, item.IdEstado));
            });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("CargaStatusSol\n" + errormessage.responseText);
        }
    });
}

function buscarCatalogos() {

    var objBuscar = {
        Modulo: 'Colocacion'
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaCatalogos",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlEstadoCivil").html("");
            $("#ddlEstadoCivil").append(new Option("--------Seleccione--------", "-1"));
            $("#ddlGenero").html("");
            $("#ddlGenero").append(new Option("--------Seleccione--------", "-1"));

            $("#ddlNacionalidad").html("");
            $("#ddlNacionalidad").append(new Option("--------Seleccione--------", "-1"));
            $("#ddlTipoReferencia").html("");
            $("#ddlTipoReferencia").append(new Option("--------Seleccione--------", "-1"));

            $("#ddlSoporte").html("");
            $("#ddlSoporte").append(new Option("--------Seleccione--------", "-1"));

            $.each(result[0], function (key, item) {
                $("#ddlEstadoCivil").append(new Option(item.Desc, item.IdTipo));
            });

            $.each(result[1], function (key, item) {
                $("#ddlGenero").append(new Option(item.Desc, item.IdTipo));
            });

            $.each(result[3], function (key, item) {
                $("#ddlTipoReferencia").append(new Option(item.Desc, item.IdTipo));
            });

            $.each(result[4], function (key, item) {
                $("#ddlSoporte").append(new Option(item.Desc, item.IdTipo));
            });

            $.each(result[5], function (key, item) {
                $("#ddlNacionalidad").append(new Option(item.Desc, item.IdTipo));
            });

            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error(errormessage.responseText);
        }
    });
}

function iniciarCola() {
    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
    if ($("#ColaIniciada").val() == '0' && $("#ddlStatusAsesor").val() != '1') {
        $("#ddlStatusAsesor").val('1');
        SelectChangeStatusAsesor();
        ocultarBloqueo();
    }
    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
    $("#ColaIniciada").val("1");
    buscarCola();
    ocultarBloqueo();
}

function buscarCola() {
    //IdSolicitud
    //IS_NEXT
    var objBuscar = {
        IdSolicitud: $("#txtSolicitud").val(),
        IS_NEXT: next
    };
    if (tiempoSolicitud > 10) {
        if (busquedaActiva == 0) {
            GuardarTiempo('Cola Ventas');
        } else {
            GuardarTiempo('Busqueda');
        }    
    }
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaColaVentas",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.length == 0) {
                alertify
                    .alert("La cola está vacía, favor ingresar datos en la cola.", function () {
                        $("#ColaIniciada").val("0");
                        clearAll();
                        reiniciarTabs();
                        ocultarBloqueo();
                    });
            } else {
                if (IdSolicitudActiva == result[0].IdSolicitud && busquedaActiva == 0) {
                    alertify.error("Debe Ingresar Alguna Gestión para Pasar a la Siguiente Solicitud en Cola");
                }
                else {
                    $("#txtBusqueda").val(result[0].IdSolicitud);//icortes id solicitud inicio de cola
                    buscarSolicitudesXAgentes();
                    //INICIO FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
                    aplicar_regla_controles(result[0].Tipo);
                    localStorage.removeItem("TipoGestion");
                    localStorage.setItem("TipoGestion", result[0].Tipo);
                    $("#lblDetalleOrigen").text(result[0].SourceCode);
                    reiniciarTabs();
                    ocultarBloqueo();
                    $("#ddlStatusAsesor").val('1');
                    SelectChangeStatusAsesor();
                    IdSolicitudActiva = result[0].IdSolicitud;
                    busquedaActiva = 0;
                    cronometro('Iniciar'); //Iniciamos el cronometro
                    //FIN FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
                }
                next = false;
            }
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("buscarCola\n" + errormessage.responseText);
        }
    });
}

function buscarSolicitudesXAgentes() {
    if ($("#txtBusqueda").val() != null) {
        if ($("#txtBusqueda").val() == '') {
            alertify.alert("Favor ingresar Id Solicitud o Cedula del Cliente", function () {
                ocultarBloqueo();
                return false;
            });
        }
        else {
            if ($("#txtBusqueda").val().match(v_RegCedula)) {
                var objBuscar = {
                    Identificacion: $("#txtBusqueda").val()
                };
            }
            else {
                if ($("#txtBusqueda").val().match(v_RegEntero)) {
                    var objBuscar = {
                        IdSolicitud: $("#txtBusqueda").val()
                    };
                }
            }

            $.ajax({
                processing: true, // for show progress bar
                serverSide: true, // for process server side
                filter: true, // this is for disable filter (search box)
                orderMulti: false, // for disable multiple column at once
                url: "/Colocacion/ListarSolicitudesXAcesor",
                type: "POST",
                data: JSON.stringify(objBuscar),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    llenarDatos(result);
                    cronometro('Iniciar');
                    ocultarBloqueo();
                },
                error: function (errormessage) {
                    ocultarBloqueo();
                    alertify.error("Error al cargar la solicitud");
                }
            });
        }
    }
}

//INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
function buscarSolicitudManual(idSolicitudBuscar = null) {
    if (tiempoSolicitud > 10) {
        if (busquedaActiva == 0) {
            GuardarTiempo('Cola Ventas');
        } else {
            GuardarTiempo('Busqueda');
        }
    }
    var validarFormatoBusqueda = false;
    var objBuscar = null;
    //validamos si viene el parametro de busqueda l asignamos al texto de busqueda
    if (idSolicitudBuscar != null) {
        $("#txtBusqueda").val(idSolicitudBuscar);
    }
    if ($("#txtBusqueda").val() == '') {
        alert("Favor ingresar Id Solicitud o Cedula del Cliente");
    }
    else {
        if ($("#txtBusqueda").val().match(v_RegCedula)) { //numero de cedula Nacional
            validarFormatoBusqueda = true;
            objBuscar = {
                Identificacion: $("#txtBusqueda").val()
            };
        }
        else {
            if ($("#txtBusqueda").val().match(v_RegCedulaDimex)) { //numero de cedula Dimex
                validarFormatoBusqueda = true;
                objBuscar = {
                    Identificacion: $("#txtBusqueda").val()
                };
            }
            else {
                if ($("#txtBusqueda").val().match(v_RegEntero)) { //Id de solicitud
                    validarFormatoBusqueda = true;
                    objBuscar = {
                        IdSolicitud: $("#txtBusqueda").val()
                    };
                }
                else {
                    validarFormatoBusqueda = false;
                    alertify.error("El valor ingresado para la busqueda no coincide con ningun criterio de busqueda, Cedula, Dimex o Id Solicitd");
                }
            }
        }
        ocultarBloqueo();
        if (validarFormatoBusqueda) { //Si el criterio de busqueda es correcto (Cedula, Dimex o IdSolicitud)
            if ($("#ColaIniciada").val() == 1) {
                $("#ColaIniciada").val("0");
                state = 'stop';
                var button = $("#button_play");//d3.select("#button_play").classed('btn-success', false);
                $("#button_play_i").attr('class', "fa fa-play");
                clearAll();
                $("#ddlStatusAsesor").val('10');
                SelectChangeStatusAsesor();

                $('#button_reiniciarCola').show();
                //$('#button_fw').hide();
                busquedaActiva = 1;
            }

            $.ajax({
                processing: true, // for show progress bar
                serverSide: true, // for process server side
                filter: true, // this is for disable filter (search box)
                orderMulti: false, // for disable multiple column at once
                url: "/Colocacion/ListarSolicitudesXAcesor",
                type: "POST",
                data: JSON.stringify(objBuscar),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.IdSolicitud > 0) {
                        $("#txtBusqueda").val(result.IdSolicitud);
                        IdSolicitudActiva = result.IdSolicitud;
                        llenarDatos(result);
                        cronometro('Iniciar');
                        ocultarBloqueo();
                    } else {
                        clearAll();
                        ocultarBloqueo();
                        if (objBuscar.IdSolicitud != null) {
                            alertify.error("La Solicitud ingresada no existe");
                        }
                        else {
                            alertify.error("La identificacion no tiene ninguna solicitud ingresada");
                        }
                    }
                },
                error: function (errormessage) {
                    ocultarBloqueo();
                    alertify.error("Error al cargar la solicitud");
                }
            });
        }
    }
}
function mostrarDatosLoop(idSolicitud) {

    var objBuscar = {
        Solicitud: idSolicitud,
        tipo: "1",
        Identificacion: $("#txtCedula").val()
    };

    $('.loop').html('');
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        beforeSend: function () {
            mostrarBloqueo();
        },
        url: "/Colocacion/BuscarSolicitudesLoop",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            var d = new Date();
            ocultarBloqueo();
            $.each(result, function (key, item) {

                html += '<tr>';

                //<td><img src="@Url.Content(item.UrlFotoCedula)" alt="Image" width="100px" height="100px" /><br /><a class="btn btn-success" href="@Url.Content(item.UrlFotoCedula)" target="_blank">Descargar</a></td>
                //    <td><img src="@Url.Content(item.UrlFotoSelfie)" alt="Image" width="100px" height="100px" /><br /><a class="btn btn-success" href="@Url.Content(item.UrlFotoSelfie)" target="_blank">Descargar</a></td>
                //    <td><img src="@Url.Content(item.UrlFotoCedulaTrasera)" alt="Image" width="100px" height="100px" /><br /><a class="btn btn-success" href="@Url.Content(item.UrlFotoCedulaTrasera)" target="_blank">Descargar</a></td>
                //    <td><img src="@Url.Content(item.UrlFotoFirma)" alt="Image" width="50px" height="50px" /><br /><a class="btn btn-success" href="@Url.Content(item.UrlFotoFirma)" target="_blank">Descargar</a></td>
                //    <td><a href="@Url.Content(item.UrlDirectorioPagare) " class="btn btn-success" target="_blank">Descargar</a></td>
                //imagen cedula frontal
                $('#imagen-cedula-frontal').html('<img id="UrlFotoCedula" src=' + item.UrlFotoCedula.substring(1) + '?' + d.getTime() + ' alt="Image" width="100px" height="100px" />');
                $('#descargar-frontal').html('<a class="btn btn-primary btn-primary-eliminar" href=' + item.UrlFotoCedula.substring(1) + '?' + d.getTime() + ' target="_blank"><span class="glyphicon glyphicon-download-alt"></span><span class="tooltiptexMenu">Descargar</span></a>');
                $('#girar-frontal').html('<a class="btn btn-primary" target="_blank" onclick="GirarFotoCedula();"><span class="glyphicon glyphicon-repeat"></span><span class="tooltiptexMenu">Girar</span></a>');

                $('#imagen-selfie').html('<img id="UrlFotoSelfie" src=' + item.UrlFotoSelfie.substring(1) + '?' + d.getTime() + ' alt="Image" width="100px" height="100px" />');
                $('#descargar-selfie').html('<a class="btn btn-primary btn-primary-eliminar" href=' + item.UrlFotoSelfie.substring(1) + '?' + d.getTime() + ' target="_blank"><span class="glyphicon glyphicon-download-alt"></span><span class="tooltiptexMenu">Descargar</span></a>');
                $('#girar-selfie').html('<a class="btn btn-primary" target="_blank" onclick="GirarFotoSelfie();"><span class="glyphicon glyphicon-repeat"></span><span class="tooltiptexMenu">Girar</span></a>');

                $('#imagen-cedula-trasera').html('<img id="UrlFotoCedulaTrasera" src=' + item.UrlFotoCedulaTrasera.substring(1) + '?' + d.getTime() + ' alt="Image" width="100px" height="100px" />');
                $('#descargar-trasera').html('<a class="btn btn-primary btn-primary-eliminar" href=' + item.UrlFotoCedulaTrasera.substring(1) + '?' + d.getTime() + ' target="_blank"><span class="glyphicon glyphicon-download-alt"></span><span class="tooltiptexMenu">Descargar</span></a>');
                $('#girar-trasera').html('<a class="btn btn-primary" target="_blank" onclick="GirarFotoCedulaDorso();"><span class="glyphicon glyphicon-repeat"></span><span class="tooltiptexMenu">Girar</span></a>');

                $('#imagen-firma').html('<img id="UrlFotoFirma" src=' + item.UrlFotoFirma.substring(1) + '?' + d.getTime() + ' alt="Image" width="100px" height="100px" />');
                $('#descargar-firma').html('<a class="btn btn-primary btn-primary-eliminar" href=' + item.UrlFotoFirma.substring(1) + '?' + d.getTime() + ' target="_blank"><span class="glyphicon glyphicon-download-alt"></span><span class="tooltiptexMenu">Descargar</span></a>');
                $('#girar-firma').html('<a class="btn btn-primary" target="_blank" onclick="GiraFotoFirma();"><span class="glyphicon glyphicon-repeat"></span><span class="tooltiptexMenu">Girar</span></a>');

                //$('#imagen-pagare').html('<img id="UrlDirectorioPagare" src=' + item.UrlDirectorioPagare.substring(1) + ' alt="Image" width="100px" height="100px" />');
                //$('#descargar-pagare').html('<a class="btn btn-primary-eliminar" href=' + item.UrlDirectorioPagare.substring(1) + ' target="_blank">Descargar</a>');
                //$('#girar-pagare').html('<a class="btn btn-primary" target="_blank" onclick="CreaContratoPagare();">Generar Nuevo</a>');
                ////html += '<td>' + item.Identificacion + '</td>';
                //html += '<td>' + item.AgenteAsignado + '</td>';
                //html += '<td>' + item.Fecha + '</td>';
                //html += '<td>' + "<a href='#' class='btn btn-primary' onclick= TraeEncabezado('" + item.Identificacion + "','" + item.IdCredito + "'); >Procesar</a>" + '</td>';
                $('#imagen-pagare').html('<object id="UrlDirectorioPagare" data=' + item.UrlDirectorioPagare.substring(1) + '?' + d.getTime() + ' type="application/pdf" <embed src=' + item.UrlDirectorioPagare.substring(1) + '?' + d.getTime() + 'type="application/pdf">  &nbsp; </embed>  alt:<a href=' + item.UrlDirectorioPagare.substring(1) + '?' + d.getTime() + ' > </object>');
                $('#descargar-dom').html('<a id="btnDescargarDocDom" onclick="CreaDocumentoDom();" class="btn btn-primary-eliminar"><span id="icono-carga-domiciliacion" class=""></span>Domiciliacion<span class="tooltiptexMenu">Descargar Domiciliacion</span></a>');
                $('#descargar-pagare').html('<a class="btn btn-primary-eliminar" href=' + item.UrlDirectorioPagare.substring(1) + '?' + d.getTime() + ' target="_blank"><span class="glyphicon glyphicon-download-alt"></span><span class="tooltiptexMenu">Descargar Pagare</span></a>');
                $('#girar-pagare').html('<a id="CreaContratoPagare" class="btn btn-primary" target="_blank" onclick="CreaContratoPagare();"><span class="glyphicon glyphicon-refresh"></span><span class="tooltiptexMenu">Generar Nuevo</span></a>');

                if (item.Status != 39 && item.Status != 38 && item.Status != 12 && item.Status != 112 && item.Status != 113 && item.Status != 116 && item.Status != 115) {
                    $("#CreaContratoPagare").hide();
                    //document.getElementById('CreaContratoPagare').style.display = 'none';
                }
                else {
                    $("#CreaContratoPagare").show();
                }
            });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alert("mostrarDatosLoop\n" + errormessage.responseText)
            ocultarBloqueo();
        }, complete: function (data) {
            ocultarBloqueo();
        }
    });
}

//function buttonBackPress() {
//    console.log("button back invoked.");
//}

//function buttonForwardPress() {
//    console.log("button forward invoked.");
//}

//function buttonRewindPress() {
//    console.log("button rewind invoked.");
//}

function buttonForwardPress() {
    alertify.confirm("¿Desea Saltar la cola?.",
        function () {
            next = true;
            buscarCola();
            ocultarBloqueo();
        },
        function () {
            ocultarBloqueo();
            //return false;
        });
}

function reiniciarCola() {
    $('#button_reiniciarCola').hide();
    //$('#button_fw').show();
    iniciarCola();
}
function buttonPlayPress() {
    if (state == 'stop') {
        state = 'play';
        //var button = $("#button_play"); //d3.select("#button_play").classed('btn-success', true);
        $("#button_play_i").attr('class', "fa fa-pause");
        iniciarCola();
        ocultarBloqueo();
    }
    else if (state == 'play' || state == 'resume') {
        state = 'pause';
        $("#button_play_i").attr('class', "fa fa-play");
        //d3.select("#button_play i").attr('class', "fa fa-play");
        alertify.confirm("¿Desea Pausar la cola?.",
            function () {
                $("#ColaIniciada").val("0");
            },
            function () {
                $("#ColaIniciada").val("1");
            });
    }
    else if (state == 'pause') {
        state = 'resume';
        $("#button_play_i").attr('class', "fa fa-pause");
        //d3.select("#button_play i").attr('class', "fa fa-pause");
        iniciarCola();
        ocultarBloqueo();
    }
}

function buttonStopPress() {

    alertify.confirm("¿Desea Detener la cola?.",
        function () {
            $("#ColaIniciada").val("0");
            state = 'stop';
            var button = $("#button_play");//d3.select("#button_play").classed('btn-success', false);
            $("#button_play_i").attr('class', "fa fa-play");
            clearAll();
            ocultarBloqueo();
        },
        function () {
            $("#ColaIniciada").val("1");
            ocultarBloqueo();
        });
}


function bs_input_file_frontal() {
    $(".input-file-Frontal").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input id='fileImageFrontal'  onchange='handleFileSelectFrontal(this);' type='file' class='input-ghost' accept='.jpg, .png' style='visibility:hidden; height:0'>");
                element.attr("name", $(this).attr("name"));
                element.change(function () {
                    element.next(element).find('input').val((element.val()).split('\\').pop());
                });
                $(this).find("button.btn-choose").click(function () {
                    element.click();
                });
                $(this).find("a.btn-reset").click(function () {
                    element.val(null);
                    $(this).parents(".input-file-Frontal").find('input').val('');
                });
                $(this).find('input').css("cursor", "pointer");
                $(this).find('input').mousedown(function () {
                    $(this).parents('.input-file-Frontal').prev().click();
                    return false;
                });
                return element;
            }
        }
    );
    ocultarBloqueo();
}

function bs_input_file_Trasera() {
    $(".input-file-Trasera").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input id='fileImageTrasera'  type='file' onchange='handleFileSelectTrasera(this);' class='input-ghost' accept='.jpg, .png' style='visibility:hidden; height:0'>");
                element.attr("name", $(this).attr("name"));
                element.change(function () {
                    element.next(element).find('input').val((element.val()).split('\\').pop());
                });
                $(this).find("button.btn-choose").click(function () {
                    element.click();
                });
                $(this).find("a.btn-reset").click(function () {
                    element.val(null);
                    $(this).parents(".input-file-Trasera").find('input').val('');
                });
                $(this).find('input').css("cursor", "pointer");
                $(this).find('input').mousedown(function () {
                    $(this).parents('.input-file-Trasera').prev().click();
                    return false;
                });
                return element;
            }
        }
    );
    ocultarBloqueo();
}

function bs_input_file_Domiciliacion() {
    $(".input-file-Domiciliacion").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input id='fileDomiciliacion' type='file' onchange='handleFileSelectFile(this);' class='input-ghost' accept='.pdf' style='visibility:hidden; height:0'>");
                element.attr("name", $(this).attr("name"));
                element.change(function () {
                    element.next(element).find('input').val((element.val()).split('\\').pop());
                });
                $(this).find("button.btn-choose").click(function () {
                    element.click();
                });
                $(this).find("a.btn-reset").click(function () {
                    element.val(null);
                    $(this).parents(".input-file-Domiciliacion").find('input').val('');
                });
                $(this).find('input').css("cursor", "pointer");
                $(this).find('input').mousedown(function () {
                    $(this).parents('.input-file-Domiciliacion').prev().click();
                    return false;
                });
                return element;
            }
        }
    );
    ocultarBloqueo();
}
function bs_input_file_Selfie() {
    $(".input-file-Selfie").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input id='fileSelfie' type='file' onchange='handleFileSelectSelfie(this);' class='input-ghost' accept='.jpg, .png' style='visibility:hidden; height:0'>");
                element.attr("name", $(this).attr("name"));
                element.change(function () {
                    element.next(element).find('input').val((element.val()).split('\\').pop());
                });
                $(this).find("button.btn-choose").click(function () {
                    element.click();
                });
                $(this).find("a.btn-reset").click(function () {
                    element.val(null);
                    $(this).parents(".input-file-Selfie").find('input').val('');
                });
                $(this).find('input').css("cursor", "pointer");
                $(this).find('input').mousedown(function () {
                    $(this).parents('.input-file-Selfie').prev().click();
                    return false;
                });
                return element;
            }
        }
    );
    ocultarBloqueo();
}
function bs_input_file_Firma() {
    $(".input-file-Firma").before(
        function () {
            if (!$(this).prev().hasClass('input-ghost')) {
                var element = $("<input id='fileFirma' type='file' onchange='handleFileSelectFirma(this);' class='input-ghost' accept='.jpg, .png' style='visibility:hidden; height:0'>");
                element.attr("name", $(this).attr("name"));
                element.change(function () {
                    element.next(element).find('input').val((element.val()).split('\\').pop());
                });
                $(this).find("button.btn-choose").click(function () {
                    element.click();
                });
                $(this).find("a.btn-reset").click(function () {
                    element.val(null);
                    $(this).parents(".input-file-Firma").find('input').val('');
                });
                $(this).find('input').css("cursor", "pointer");
                $(this).find('input').mousedown(function () {
                    $(this).parents('.input-file-Firma').prev().click();
                    return false;
                });
                return element;
            }
        }
    );
    ocultarBloqueo();
}
//icortes
var ObjecFile = null;
var ObjecImageFrontal = null;
var ObjecImageSelfie = null;
var ObjecImageTrasera = null;
var ObjecImageFirma = null;


function saveFileDocument() {
    guardarGestion('122', 'Modificación de Documentos', '');
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/GuardarDomiciliacion",
        type: "POST",
        beforeSend: function () {
            mostrarBloqueo();
        },
        data: JSON.stringify(ObjecFile),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alertify
                .alert("Archivo Guardado", function () {
                    mostrarDatosLoop($("#txtSolicitud").val());
                    getPasosAgente();
                    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                    //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("saveFileDocument\n" + errormessage.responseText);
        }
        , complete: function (data) {
            ocultarBloqueo();
        }
    });

}

function saveFileFrontal() {
    guardarGestion('120', 'Modificación de cedula Frontal', '');
    $.ajax({
        type: "POST",
        url: "/Colocacion/GuardarFrontal",
        beforeSend: function (data) {
            mostrarBloqueo();
        },
        data: JSON.stringify(ObjecImageFrontal),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Mensaje == 'SOK02' && result.Result == false) {
                alertify
                    .alert("ERROR:Por favor, ingrese nuevamente la foto de la cédula", function () {
                        mostrarDatosLoop($("#txtSolicitud").val());
                    });
            }
            else {
                alertify
                    .alert("Archivo Guardado", function () {
                        mostrarDatosLoop($("#txtSolicitud").val());
                        getPasosAgente();
                        alertify.success("Proceso Realizado con Exito !!!");
                        //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                        //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                        //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    });
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("saveFileFrontal\n" + errormessage.responseText);
        }, complete: function (data) {
            ocultarBloqueo();
        }
        //complete: function () {
        //    i--;
        //    if (i <= 0) {
        //        ocultarBloqueo();
        //    }
        //}
    });

}

function saveFileSelfie() {

    if ($('#cargar-foto-selfie').val()== '') {
        alertify
            .alert("Debe de seleccionar una selfie para guardarla", function () {
            });
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Colocacion/GuardarSelfie",
            data: JSON.stringify(ObjecImageSelfie),
            beforeSend: function (data) {
                mostrarBloqueo();
            },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (!result.Mensaje == null && !result.PorcentMatched == null && result.Mensaje == 'SOK04' && result.Result == true && result.PorcentMatched == 0) {
                    //callback(result);		
                    alertify
                        .alert("ERR:Por favor, ingrese nuevamente la foto del selfie", function () {
                            mostrarDatosLoop($("#txtSolicitud").val());
                        });
                }
                else {
                    alertify
                        .alert("Archivo Guardado", function () {
                            mostrarDatosLoop($("#txtSolicitud").val());
                            getPasosAgente();
                            alertify.success("Proceso Realizado con Exito !!!");
                            //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                            //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                            //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                        });
                }
                guardarGestion('123', 'Modificación de Selfie', '');
                ocultarBloqueo();
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alert("saveFileSelfie\n" + errormessage.responseText);
            }, complete: function (data) {
                ocultarBloqueo();
            }
        });
    }
}

function saveFileTrasera() {
    guardarGestion('120', 'Modificación de Cedula Trasera', '');
    $.ajax({

        type: "POST",
        url: "/Colocacion/GuardarTrasera",
        data: JSON.stringify(ObjecImageTrasera),
        beforeSend: function (data) {
            mostrarBloqueo();
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Mensaje == 'SOK03' && result.VigenciaCed == false) {
                alertify
                    .alert("Error:Por favor, ingrese nuevamente la foto del dorso de la cédula. La cédula no se encuentra vigente!!!", function () {
                        mostrarDatosLoop($("#txtSolicitud").val());
                        ocultarBloqueo();
                    });
            }
            if (result.Mensaje == 'SOK03' && result.Result == false) {
                alertify
                    .alert("Error:Por favor, ingrese nuevamente la foto del dorso de la cédula, el número de cédula no coincide con el número de cédula de la fotrográfia!!!", function () {
                        mostrarDatosLoop($("#txtSolicitud").val());
                        ocultarBloqueo();
                    });
            }
            else {
                alertify
                    .alert("Archivo Guardado", function () {
                        mostrarDatosLoop($("#txtSolicitud").val());
                        getPasosAgente();
                        ocultarBloqueo();
                        alertify.success("Proceso Realizado con Exito !!!");
                        //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                        //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                        //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    });
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("saveFileTrasera\n" + errormessage.responseText);
        },
        complete: function (data) {
            ocultarBloqueo();
        }
    });

}

function saveFileFirma() {
    guardarGestion('119', 'Modificación de firma', '');
    $.ajax({

        type: "POST",
        url: "/Colocacion/GuardarFirma",
        beforeSend: function (data) {
            mostrarBloqueo();
        },
        data: JSON.stringify(ObjecImageFirma),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alertify
                .alert("Archivo Guardado", function () {
                    mostrarDatosLoop($("#txtSolicitud").val());
                    ocultarBloqueo();
                    getPasosAgente();
                    ocultarBloqueo();
                    alertify.success("Proceso Realizado con Exito !!!");
                    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                    //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("saveFileFirma\n" + errormessage.responseText);
        }, complete: function (data) {
            ocultarBloqueo();
        }
    });

}
//icortes
function handleFileSelectFile(evt) {
    ObjecFile = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();

    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;

            var base64String = window.btoa(binaryData);

            ObjecFile = { Base64: base64String, Name: f.name, Type: f.type, IdSolicitud: $("#txtSolicitud").val(), Identificacion: $("#txtCedula").val() }
        };
    })(f);

    reader.readAsBinaryString(f);
}

//icortes
function handleFileSelectTrasera(evt) {
    ObjecImageTrasera = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();

    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;

            var base64String = window.btoa(binaryData);

            ObjecImageTrasera = { Base64: base64String, Name: f.name, Type: f.type, IdSolicitud: $("#txtSolicitud").val(), Identificacion: $("#txtCedula").val() }
        };
    })(f);

    reader.readAsBinaryString(f);
}

//icortes
function handleFileSelectFrontal(evt) {
    ObjecImageFrontal = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();

    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;

            var base64String = window.btoa(binaryData);

            ObjecImageFrontal = { Base64: base64String, Name: f.name, Type: f.type, IdSolicitud: $("#txtSolicitud").val(), Identificacion: $("#txtCedula").val() }
        };
    })(f);

    reader.readAsBinaryString(f);
}

function handleFileSelectSelfie(evt) {
    ObjecImageSelfie = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();

    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;

            var base64String = window.btoa(binaryData);

            ObjecImageSelfie = { Base64: base64String, Name: f.name, Type: f.type, IdSolicitud: $("#txtSolicitud").val(), Identificacion: $("#txtCedula").val() }
        };
    })(f);

    reader.readAsBinaryString(f);
}

function handleFileSelectFirma(evt) {
    ObjecImageFirma = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();

    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;

            var base64String = window.btoa(binaryData);

            ObjecImageFirma = { Base64: base64String, Name: f.name, Type: f.type, IdSolicitud: $("#txtSolicitud").val(), Identificacion: $("#txtCedula").val() }
        };
    })(f);

    reader.readAsBinaryString(f);
}

function ActualizaPersona() {
    if (validateForm()) {
        var nombreSplit = $("#txtNombre").val().split(' ');
        var nombre = nombreSplit[0];
        var segundoNombre = nombreSplit.length == 4 ? nombreSplit[1] : "";
        var apellido = nombreSplit.length == 4 ? nombreSplit[2] : nombreSplit[1];
        var segundoApellido = nombreSplit.length == 4 ? nombreSplit[3] : nombreSplit[2];

        var objActualiza = {
            IdSolicitud: $("#txtSolicitud").val(),          // --Obligatorio                                                 
            IdTipoIdentificacion: $("#hdfTipoIdentificacion").val(),      //     --Obligatorio
            Identificacion: $("#txtCedula").val(),         //--Obligatorio
            VencimientoIdentificacion: $("#txtCedulaVence").val(),//--Obligatorio
            PrimerNombre: nombre,//--Obligatorio
            SegundoNombre: segundoNombre,
            PrimerApellido: apellido,//-- Obligatorio
            SegundoApellido: segundoApellido,
            TelefonoCel: $("#txtTelefono").val(),//--Obligatorio
            //TelefonoFijo: $("#").val(),
            //TelefonoLaboral: $("#").val(),
            Correo: $("#txtCorreo").val(),//--Obligatorio
            //CorreoOpcional: $("#").val(),
            EstadoCivil: $("#ddlEstadoCivil").val(),
            Sexo: $("#ddlGenero").val(),//--Obligatorio
            FechaNacimiento: $("#txtFechaNacimiento").val(),//--Obligatorio
            Provincia: $("#ddlProvincia").val(),//--Obligatorio
            Canton: $("#ddlCanton").val(),//--Obligatorio
            Distrito: $("#ddlDistrito").val(),//--Obligatorio
            DetalleDireccion: $("#txtDetalle").val(),
            //UsrModifica: $("#").val(),
            IdProducto: $("#ddlProducto").val(),
            UsoCredito: $("#txtUsoCredito").val(),
            //OrdenPatronal: $("#").val(),
            // --> RefereinciaFamiliar
            IdTipRefFamiliar: $("#ddlTipoReferencia").val(),
            NombreCompleto: $("#txtNombreRefFamiliar").val(),
            TelefonoFamiliar: $("#txtTelefonoRefFamiliar").val(),
            // --> ReferenciaLaboral
            Empresa: $("#txtNombreRefEmpresa").val(),
            TelefonoEmpresa: $("#txtTelefonoRefEmpresa").val(),
            SupervisorDirecto: $("#txtNombreRefSupervisor").val(),
            //--> ReferenciaPersonal
            NombreCompletoPersonal: $("#txtNombreRefPersonal").val(),
            TelefonoPersonal: $("#txtTelefonoRefPersonal").val()
        }
        //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
        guardarGestion('128', 'Actualiza Informacion Personal', '');
        //FIN  FCAMACHO  GUARDAR GESTION 13/03/2019
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/Colocacion/ActualizarPersonas",
            type: "POST",
            data: JSON.stringify(objActualiza),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == "OK") {
                    alertify
                        .alert("Gestión  Guardada", function () {
                            if ($("#ColaIniciada").val() == "1") {
                                guardarGestion('120', 'Modificación de cedula Frontal', '');
                                ocultarBloqueo();
                                clearAll();
                                ocultarBloqueo();
                                guardarGestion('125', 'Gestionado', 'Solicitud: ' + objActualiza.IdSolicitud);
                                ocultarBloqueo();
                                buscarCola();
                                ocultarBloqueo();
                                getPasosAgente();
                                ocultarBloqueo();
                            }
                        });
                } else {
                    alertify
                        .error("Ocurrio un Error contacte al Administrador", function () {
                            ocultarBloqueo();
                        });
                }
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alert("ActualizaPersona\n" + errormessage.responseText);
            }
        });
    }
}
function buscarSolicitudesXAgentesRecarga() {

    if ($("#txtCedula").val().match(v_RegCedula)) {
        var objBuscar = {
            Identificacion: $("#txtCedula").val()
        };
    }
    else {
        if ($("#txtSolicitud").val().match(v_RegEntero)) {
            var objBuscar = {
                IdSolicitud: $("#txtSolicitud").val()
            };
        }
    }

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/ListarSolicitudesXAcesor",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            llenarDatos(result);
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("buscarSolicitudesXAgentesRecarga\n" + errormessage.responseText);
        }
    });
}
function GirarFotoCedula() {
    guardarGestion('124', 'Giro de Cedula', '');
    var objBuscar = {
        Identificacion: $("#txtCedula").val(),
        IdSolicitud: $("#txtSolicitud").val(),
        UrlFotoCedula: $('#UrlFotoCedula').attr('src')
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionDocumentos/GirarFotoCedula",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //buscarSolicitudesXAgentesRecarga();
            mostrarDatosLoop($("#txtSolicitud").val());
            ocultarBloqueo();
            alertify.success("Proceso Realizado con Exito !!!");
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("GirarFotoCedula\n" + errormessage.responseText);
        }
    });
}

function GirarFotoCedulaDorso() {
    guardarGestion('124', 'Giro de Cedula Dorso', '');
    var objBuscar = {
        Identificacion: $("#txtCedula").val(),
        IdSolicitud: $("#txtSolicitud").val(),
        UrlFotoCedulaTrasera: $('#UrlFotoCedulaTrasera').attr('src')
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionDocumentos/GirarFotoCedulaDorso",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //buscarSolicitudesXAgentesRecarga();
            mostrarDatosLoop($("#txtSolicitud").val());
            ocultarBloqueo();
            alertify.success("Proceso Realizado con Exito !!!");
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("GirarFotoCedulaDorso\n" + errormessage.responseText);
        }
    });
}

function GirarFotoSelfie() {
    guardarGestion('124', 'Giro de Selfie', '');
    var objBuscar = {
        Identificacion: $("#txtCedula").val(),
        IdSolicitud: $("#txtSolicitud").val(),
        UrlFotoSelfie: $('#UrlFotoSelfie').attr('src')
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionDocumentos/GirarFotoSelfie",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //buscarSolicitudesXAgentesRecarga();
            mostrarDatosLoop($("#txtSolicitud").val());
            alertify.success("Proceso Realizado con Exito !!!");
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("GirarFotoSelfi\n" + errormessage.responseText);
        }
    });
}

function GiraFotoFirma() {
    guardarGestion('124', 'Giro de Firma', '');
    var objBuscar = {
        Identificacion: $("#txtCedula").val(),
        IdSolicitud: $("#txtSolicitud").val(),
        UrlFotoFirma: $('#UrlFotoFirma').attr('src')
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/GestionDocumentos/GiraFotoFirma",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            mostrarDatosLoop($("#txtSolicitud").val());
            ocultarBloqueo();
            alertify.success("Proceso Realizado con Exito !!!");
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("GiraFotoFirma\n" + errormessage.responseText);
        }
    });
}
//ventanas informacion y pasos por agente
function actualizarTabla() {
    getPasosAgente();
    ocultarBloqueo();
}
function getPasosAgente() {
    var empObj = {
        IdSolicitud: $("#txtSolicitud").val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            beforeSend: function () {
                mostrarBloqueo();
            },
            url: "/Colocacion/getPAsosAgente",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var colaIniciada = $("#ColaIniciada").val();
                if (result.length == 1 && result[0].Paso == '5000' && colaIniciada == '1') {
                    //Validamos que la solicitud tenga un producto asignado por medio del ID
                    if (objSolicitudActual.IdProducto != 0 && objSolicitudActual.IdProducto != null) {
                        if (objSolicitudActual.MontoProducto <= objSolicitudActual.MontoMaximo) {
                        //cerramos la ventana de pasos
                        cerrarPanelPAsos();
                        //Seguimos con la cola
                        buscarCola();
                        }
                        else {
                            alertify.error("El Monto del credito es mayor al monto maximo aprobado.");
                            seleccionarTabManual(3);
                        }                           
                    }
                    else {
                        alertify.error("Debe ingresar un producto para continuar.");
                        seleccionarTabManual(3);
                    }
                }
                else {
                    var html = '';
                    //validamos el Idtipo para mostrar cuales tabs tienen pasos pendientes
                    validarPasosAgenteTabsNav(result);
                    $.each(result, function (key, item) {
                        if (item.BitCompleto == 1) {
                            ocultarBloqueo();
                            html += '<tr class="fila-marcada">';
                            html += '<td class="paso-marcado"><a data-toggle="tab" href="#contentTab' + item.IdTipo + '" class="tabTipo-' + item.IdTipo + '" onclick="seleccionarTab(' + item.IdTipo + ')" >' + item.Descripcion + '</a></td>';
                            html += '<td><a id=paso-' + item.Paso + ' class="btn btn-marcado" onclick="cambiarBitCompleto(' + item.Paso + ')"><span class="glyphicon glyphicon-ok"></span></a></td>';
                            html += '</tr>';
                            ocultarBloqueo();
                        }
                        else {
                            ocultarBloqueo();
                            html += '<tr class="fila-pendiente">';
                            html += '<td class="paso-pendiente"><a data-toggle="tab" href="#contentTab' + item.IdTipo + '" class="tabTipo-' + item.IdTipo + '"onclick="seleccionarTab(' + item.IdTipo + ')" >' + item.Descripcion + '</a></td>';
                            html += '<td><a id=paso-' + item.Paso + ' class="btn btn-pendiente" onclick="cambiarBitCompleto(' + item.Paso + ')"><span class="glyphicon glyphicon-remove"></span></a></td>';
                            html += '</tr>';
                        }
                        ocultarBloqueo();
                    });
                    $('#tblPasosXAgentes').html(html);
                    ocultarBloqueo();
                    conteoPendientes();
                    ocultarBloqueo();
                    oculatarPasos();
                    ocultarBloqueo();
                    CargaCuentaBancaria();
                    ocultarBloqueo();
                    seleccionarTabPendiente();
                    ocultarBloqueo();
                }
                ocultarBloqueo();
            },
            error: function (errormessage) {
                ocultarBloqueo();
            }, complete: function (data) {
                ocultarBloqueo();
            }
        });
}
function cambiarBitCompleto(paso) {
    if (verificarPaso(paso)) {
        var elemento = '#paso-' + paso;
        ocultarBloqueo();
        if ($(elemento).find('span').hasClass('glyphicon-ok')) { //El paso ya esta marcado como completo
            var pasoCompletado = true;

        }
        else { //El Paso aun no se a completado
            var pasoCompletado = false;
        }
        if (pasoCompletado) {
            alertify.error("No se Puede Desmarcar un Paso ya Completado");
        }
        else {
            var empObj = {
                IdSolicitud: $("#txtSolicitud").val(),
                CodAgente: $("#txtCedula").val(),
                Paso: paso,
                BitCompleto: true
            };
            processing: true; // for show progress bar  
            serverSide: true; // for process server side  
            filter: true; // this is for disable filter (search box)  
            orderMulti: false; // for disable multiple column at once
            paging: false;
            $.ajax(
                {
                    url: "/Colocacion/cambioBitCompleto",
                    type: "POST",
                    data: JSON.stringify(empObj),
                    contentType: "application/json;charset=utf-8",
                    success: function (result) {
                        ocultarBloqueo();
                        getPasosAgente();
                        ocultarBloqueo();
                    },
                    error: function (errormessage) {
                        ocultarBloqueo();
                        alert(errormessage.responseText);
                    }
                });
        }
    }
}
function getDashAgente() {
    var empObj = {
        cod_agente: $("#txtCedula").val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/Colocacion/getDashboardAgente",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var html = '';
                ocultarBloqueo();
                if (result.length > 0) {
                    $.each(result, function (key, item) {
                        html += '<table class="tableDashboard">';
                        html += '<tr>';
                        html += '<td class="fila-frecuencia">' + item.Frecuencia + '</td>';
                        html += '</tr>';
                        html += '<tr>';
                        if (item.MetaDiaria > item.CantidadVentas) {
                            html += '<td class="fila-cantidad" style="color:#EE8282;">' + item.CantidadVentas + '</td>';
                        }
                        else {
                            html += '<td class="fila-cantidad">' + item.CantidadVentas + '</td>';
                        }
                        html += '</tr>';
                        if (item.Comportamiento == 'S') {
                            html += '<tr>';
                            html += '<td class="fila-comportamientoS"><span class="glyphicon glyphicon-arrow-up"></span>' + item.Porcentaje + '</td>';
                            html += '</tr>';
                        }
                        if (item.Comportamiento == 'B') {
                            html += '<tr>';
                            html += '<td class="fila-comportamientoB"><span class="glyphicon glyphicon-arrow-down"></span>' + item.Porcentaje + '</td>';
                            html += '</tr>';
                        }
                        if (item.Comportamiento == 'I') {
                            html += '<tr>';
                            html += '<td class="fila-comportamientoI"><span class="glyphicon glyphicon-arrow-right"></span>' + item.Porcentaje + '</td>';
                            html += '</tr>';
                        }
                        html += '</table>';
                    });
                    $('#tablas-dashboard').html(html);
                    var htmlMetas = '';

                    htmlMetas += '<tr>';
                    htmlMetas += '<td> Meta Diaria: <span>' + result[0].MetaDiaria + '</span></td>';
                    htmlMetas += '<td> Meta Mensual: <span>' + result[0].MetaMensual + '</span></td>';
                    htmlMetas += '</tr>';

                    $('#tableInfoMetas').html(htmlMetas);
                    ocultarBloqueo();
                }
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alert(errormessage.responseText);
            }
        });
}
function getGuion(idSolicitud) {
    var empObj = {
        IdSolicitud: idSolicitud
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/Colocacion/getGuion",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += '<h1>' + item.Titulo + '</h1>';
                    html += '<p>' + item.Nombre + '</p>';
                    html += '<p>' + item.MontoAprobado + '</p>';
                });
                $('#texto-guion').html(html);
                ocultarBloqueo();
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alert(errormessage.responseText);
            }
        });
}
//reprogramar
function reprogramarSolicitud() {
    if ($('#txtFechaRep').val() == '') {
        alertify.error("Debe ingresar la fecha y la hora de la reprogramacion");
    } else {
        if ($('#txtComentarioRepro').val() == '') {
            alertify.error("Debe ingresar el comentario antes de guardar la reprogramacion");
        } else {
            var fecha = $('#txtFechaRep').val();
            var empObj = {
                IdSolicitud: $("#txtSolicitud").val(),
                FechaReprogramacion: fecha,
                ComentarioReprogramacion: $('#txtComentarioRepro').val()
            };
            processing: true; // for show progress bar  
            serverSide: true; // for process server side  
            filter: true; // this is for disable filter (search box)  
            orderMulti: false; // for disable multiple column at once
            paging: false;
            $.ajax(
                {
                    url: "/Colocacion/GuardaReprogramacion",
                    type: "POST",
                    data: JSON.stringify(empObj),
                    contentType: "application/json;charset=utf-8",
                    success: function (result) {
                        alertify.alert("Reprogramación Guardada", function () {
                            clearAll();
                            $("#ModalComentarioReprogramacion").modal('hide');
                            if ($("#ColaIniciada").val() == "1") {

                                buscarCola();
                                ocultarBloqueo();
                            }

                        });
                    },
                    error: function (errormessage) {
                        ocultarBloqueo();
                        alert(errormessage.responseText);
                    }
                });
        }
    }
}
//acordeon
$(document).ready(function ($) {
    $('#panel-acordion').find('.panel-acordion-toggle').click(function () {
        //Expand or collapse this panel
        $(this).next().slideToggle('fast', 'swing');
        $('#panel-acordion').toggleClass('ocultar');
        $('#panel-acordion').toggleClass('mostrar');
        $('#btn-cerrar-pasos').toggleClass('hide');
        if ($('#btn-cerrar-pasos').hasClass('hide')) {
            $('#panel-acordion img').addClass('hide');
        }
        else {
            $('#panel-acordion img').removeClass('hide');
        }
        //Hide the other panels
        //$("panel-acordion-content").not($(this).next()).slideUp('slow');
    });
    $('#panel-acordion-dashboard').find('.panel-acordion-toggle-dashboard').click(function () {
        //Expand or collapse this panel
        $(this).next().slideToggle('fast', 'swing');
        $('#panel-acordion-dashboard').toggleClass('ocultar-dashboard');
        $('#panel-acordion-dashboard').toggleClass('mostrar-dashboard');
        $('#btn-cerrar-dash').toggleClass('hide');
        if ($('#btn-cerrar-dash').hasClass('hide')) {
            $('#panel-acordion-dashboard img').addClass('hide');
        }
        else {
            $('#panel-acordion-dashboard img').removeClass('hide');
        }
    });
    $('#panel-acordion-guion').find('.panel-acordion-toggle-guion').click(function () {
        //Expand or collapse this panel
        $(this).next().slideToggle('fast', 'swing');
        $('#panel-acordion-guion').toggleClass('ocultar-guion');
        $('#panel-acordion-guion').toggleClass('mostrar-guion');
        $('#btn-cerrar-guion').toggleClass('hide');
        if ($('#btn-cerrar-guion').hasClass('hide')) {
            $('#panel-acordion-guion img').addClass('hide');
        }
        else {
            $('#panel-acordion-guion img').removeClass('hide');
        }
    });
    ocultarBloqueo();
});
//Conteo de tareras pendientes
function conteoPendientes() {
    var pendientes = $('.fila-pendiente').toArray().length;
    var elemento;
    if ($('#btn-cerrar-pasos').hasClass('hide')) {
        elemento = '<span class="glyphicon glyphicon-bell"></span><span id="btn-cerrar-pasos" class="hide glyphicon glyphicon-remove-sign"></span>';
    }
    else {
        elemento = '<span class="glyphicon glyphicon-bell"></span><span id="btn-cerrar-pasos" class="glyphicon glyphicon-remove-sign"></span>';
    }

    $("#titulo-panel").text('');
    $("#titulo-panel").append(elemento + pendientes);
    //if (pendientes > 0) {

    //    $('#titulo-panel').css('color', '#EE8282');
    //}
    //else {
    //    $('#titulo-panel').css('color', '#87b87f');
    //}
    ocultarBloqueo();
}
function oculatarPasos() {
    if ($('#ddHideAll').is(':checked')) {
        $('.fila-marcada').addClass('hide');
    }
    else {
        $('.fila-marcada').removeClass('hide');
    }
    ocultarBloqueo();
}

function llenarDatos(result) {
    $("#txtSolicitud").val(result.IdSolicitud);
    $("#txtFechaSolicitud").val(result.FechaIngreso);
    $("#txtCedula").val(result.Identificacion);
    $("#txtNombre").val(result.Nombre);
    $("#txtTelefono").val(result.TelefonoCel);
    $("#txtFechaNacimiento").val(result.FechaNacimiento);
    $("#txtCedulaVence").val(result.VencimientoIdentificacion);
    $("#txtCorreo").val(result.Correo);
    validateEmail($("#txtCorreo").val());

    //INICIO, ICORTES,02052019, NOMBRE EMPRESA DE CREDDID
    $("#lblEmpresaCreddidNombre").text(result.RazonSocial);


    //FIN, ICORTES,02052019, NOMBRE EMPRESA DE CREDDID

    $("#ddlNacionalidad").val(result.IdTipoIdentificacion);
    $("#ddlGenero").val(result.Sexo);
    $("#ddlEstadoCivil").val(result.EstadoCivil);

    //Titulo de la solicitud
    if ($("#ColaIniciada").val() == "0") {
        $('#lblDetalleOrigen').text(result.SourceCode);
    }
    getProvincia(result.Provincia);
    getCanton(result.Canton, result.Provincia);
    getDistrito(result.Distrito, result.Canton, result.Provincia);
    //icortes tipoIdentificacion
    $("#hdfTipoIdentificacion").val(result.IdTipoIdentificacion);

    //setTimeout(function () {
    //    $('#ddlCanton').val(result.Canton);
    //}, 500);

    //setTimeout(function () {
    //    $("#ddlCanton").trigger("change");
    //}, 500);

    //setTimeout(function () {
    //    $("#ddlDistrito").trigger("change");
    //    $("#ddlDistrito").val(result.Distrito);
    //}, 500);
    localStorage.setItem("DistritoValue", result.Distrito);
    //$('#ddlCanton').val(result.Canton).prop('selected', true);



    //$('#ddlDistrito').val(result.Distrito).prop('selected', true);

    //INICIO FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA
    $("#txtDetalle").val(result.DetalleDireccion);
    //FIN FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA
    //$("#txtDetalle").val(result.DetalleDireccion);


    //icortes modificacion 14-02-2018
    $('#ddlProducto').val(result.IdProducto).prop('selected', true);
    $('#ddlEstadoSol').val(result.Status).prop('selected', true);
    $('#txtMontoMaximo').val(numFormat(result.MontoMaximo, 1, true));
    //numFormat(item.Monto_transferencia, 1, true)
    $('#txtFechaUltimaRepro').val(result.FechaReprogramacion);
    //fin icortes 14-02-2018
    mostrarDatosLoop(result.IdSolicitud);
    ocultarBloqueo();
    //if (result.Origen == "" || result.Origen == "web" || result.Origen == null) {
    //    $("#divOrigenWeb").css("display", "inline");
    //    $("#trWeb").css("display", "inline");
    //    $("#trLoopp").css("display", "none");
    //    $("#divOrigenLoop").css("display", "none");
    //}
    //else {
    //    $("#divOrigenLoop").css("display", "inline");
    //    mostrarDatosLoop(result.IdSolicitud);
    //    $("#divOrigenWeb").css("display", "none");
    //    $("#trWeb").css("display", "none");
    //    $("#trLoopp").css("display", "inline");
    //}
    cargarMontosProductos();
    ocultarBloqueo();
    mostrarGestion();
    ocultarBloqueo();
    getDashAgente();
    ocultarBloqueo();
    getPasosAgente();
    ocultarBloqueo();
    getGuion(result.IdSolicitud);
    ocultarBloqueo();
    cargarHistorialSolicitudes();
    //INICIO FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA
    getReferenciasPersonal(result);
    //FIN FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA
    CargaDetalleProducto(); //Calculamos el detalle del producto 
    //Metodo para cargar la tabla de rules en el tab historial de solicitudes
    //Este metodo esta en el Archivo: AnalisisRules.js
    BuscarRules($('#txtCedula').val());
    cargarContactos($('#txtCedula').val());
    //INICIO FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES 
    cargarContactos_Direccion($('#txtCedula').val());
    //FINAL FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES
    ocultarBloqueo();
    //vamos a guardar la info relevante de la solicitu para usarla cuando sea necesario
    obtenerSolicitudActual(result.IdSolicitud);
    ocultarModalSelecionarConsulta();
    consultaTelefonoLaboral();
}
//INICIO FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA
function getReferenciasPersonal(result) {
    $("#ddlTipoReferencia").val(result.RefFamiliar);
    $("#txtNombreRefFamiliar").val(result.NombreCompleto);
    $("#txtTelefonoRefFamiliar").val(result.TelefonoFamiliar);
    $("#txtNombreRefEmpresa").val(result.Empresa);
    $("#txtTelefonoRefEmpresa").val(result.TelefonoEmpresa);
    $("#txtNombreRefSupervisor").val(result.SupervisorDirecto);
    $("#txtNombreRefPersonal").val(result.NombrePersonal);
    $("#txtTelefonoRefPersonal").val(result.TelefonoPersonal);
}
//FIN FCAMACHO 19/03/2019 SE CORRIJE BUG DE NO MUESTRA INFO DE REFERENCIA

function CreaContratoPagare() {

    if (!doesFileExist($('#UrlFotoFirma').attr('src'))) {
        alertify
            .alert("Debe de cargar la Firma", function () {
            });
    }
    else {
        var objBuscar = {
            Identificacion: $("#txtCedula").val(),
            IdSolicitud: $("#txtSolicitud").val(),
            UrlFotoFirma: $('#UrlFotoFirma').attr('src')
        };

        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/GestionDocumentos/CreaContratoPagare",
            type: "POST",
            data: JSON.stringify(objBuscar),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                // buscarSolicitudesXAgentesRecarga();
                mostrarDatosLoop($("#txtSolicitud").val());
                ocultarBloqueo();
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alertify.error(errormessage.responseText);
            }
        });
    }
}

var colorFalse = '#ff9';
var colorTrue = '#fff';
function validateForm() {
    if ($("#txtSolicitud").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtSolicitud').attr("placeholder", "Campo Requerido");
        $('#txtSolicitud').focus();
        $('#txtSolicitud').css('background-color', colorFalse);
        return false;
    }
    else {
        $('#txtSolicitud').css('background-color', colorTrue);
    }
    if ($("#txtCedula").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtCedula').attr("placeholder", "Campo Requerido");
        $('#txtCedula').focus();
        $('#txtCedula').css('background-color', colorFalse);
        return false;
    }
    else {
        $('#txtCedula').css('background-color', colorTrue);
    }
    if ($("#txtCedulaVence").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtCedulaVence').attr("placeholder", "Campo Requerido");
        $('#txtCedulaVence').focus();
        $('#txtCedulaVence').css('background-color', colorFalse);
        return false;
    } else {
        $('#txtCedulaVence').css('background-color', colorTrue);
    }
    if ($("#txtNombre").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtNombre').attr("placeholder", "Campo Requerido");
        $('#txtNombre').focus();
        $('#txtNombre').css('background-color', colorFalse);
        return false;
    } else {
        $('#txtNombre').css('background-color', colorTrue);
    }
    if ($("#txtTelefono").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtTelefono').attr("placeholder", "Campo Requerido");
        $('#txtTelefono').focus();
        $('#txtTelefono').css('background-color', colorFalse);
        return false;
    } else {
        $('#txtTelefono').css('background-color', colorTrue);
    }
    if ($("#txtCorreo").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtCorreo').attr("placeholder", "Campo Requerido");
        $('#txtCorreo').focus();
        $('#txtCorreo').css('background-color', colorFalse);
        return false;
    } else {
        $('#txtCorreo').css('background-color', colorTrue);
    }

    if ($("#ddlGenero").val() == "-1") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#ddlGenero').attr("placeholder", "Campo Requerido");
        $('#ddlGenero').focus();
        $('#ddlGenero').css('background-color', colorFalse);
        return false;
    } else {
        $('#ddlGenero').css('background-color', colorTrue);
    }

    if ($("#txtFechaNacimiento").val() == "") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#txtFechaNacimiento').attr("placeholder", "Campo Requerido");
        $('#txtFechaNacimiento').focus();
        $('#txtFechaNacimiento').css('background-color', colorFalse);
        return false;
    } else {
        $('#txtFechaNacimiento').css('background-color', colorTrue);
    }


    if ($("#ddlProvincia").val() == "-1") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#ddlProvincia').attr("placeholder", "Campo Requerido");
        $('#ddlProvincia').focus();
        $('#ddlProvincia').css('background-color', colorFalse);
        return false;
    } else {
        $('#ddlProvincia').css('background-color', colorTrue);
    }

    if ($("#ddlCanton").val() == "-1") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#ddlCanton').attr("placeholder", "Campo Requerido");
        $('#ddlCanton').focus();
        $('#ddlCanton').css('background-color', colorFalse);
        return false;
    } else {
        $('#ddlCanton').css('background-color', colorTrue);
    }

    if ($("#ddlDistrito").val() == "-1") {
        //$('#txtSolicitud').after('<span class="alert alert-danger"> Campo Requerido </span>');
        $('#ddlDistrito').attr("placeholder", "Campo Requerido");
        $('#ddlDistrito').focus();
        $('#ddlDistrito').css('background-color', colorFalse);
        return false;
    } else {
        $('#ddlDistrito').css('background-color', colorTrue);
    }
    return true;
}

function guardarGestion(idGestion, Accion, detalle) {
    var objBuscar = {
        IdSolicitud: $("#txtSolicitud").val(),
        Detalle: detalle,// $('#ddlEstadoSol option:selected').text(),
        ACCION: 'INSERTAR',
        TipoGestion: idGestion,
        Accion_gestion: Accion,
        TipoColaGestion: localStorage.getItem("TipoGestion"),
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/GuardarGestion",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //alertify
            //    .alert("Gestión  Guardada", function () {
            //        if ($("#ColaIniciada").val() == "1") {
            //            buscarCola();
            //        }
            //    });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error(errormessage.responseText);
        }
    });
}

function mostrarGestion() {
    var objBuscar = {
        IdSolicitud: $("#txtSolicitud").val()
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        beforeSend: function () {
            mostrarBloqueo();
        },
        url: "/Colocacion/MonstrarGestion",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.DesTipoGestion + '</td>';
                html += '<td>' + item.IdSolicitud + '</td>';
                html += '<td>' + item.FechaGestion + '</td>';
                html += '<td>' + item.nombre + '</td>';
                html += '<td>' + item.Detalle + '</td>';
                html += '</tr>';
            });
            $("#tableGestiones").dataTable().fnClearTable();
            $("#tableGestiones").dataTable().fnDestroy();
            $('#tblGestiones').html(html);
            $("#tableGestiones").dataTable({
                "order": [[2, "desc"]],
                "paging": false,
                "searching": false
            });
            $("select[name*='tblGestiones_length']").change();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error(errormessage.responseText);
        }, complete: function (data) {
            ocultarBloqueo();
        }
    });
}
//INICIO FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
function aplicar_regla_controles(accion) {

    var objBuscar = {
        Accion: accion
    };
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaContolesXAccionCola",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            $.each(result, function (key, item) {

                //metodos para cambiar setear los controles con propiedades dinamicas
                setPropiedadesCtrls(item);

            });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error(errormessage.responseText);
        }
    });
}
//FIN FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA

//ICORTES ONCHENAGE STATUS SOLICITUD

//function statusSolicutudGestion() {
//    guardarGestion('118', 'Cambio de Estado de la solicitud ', $('#ddlEstadoSol option:selected').text() + ' || ' + $("#txtUsoCredito").val());
//}//function statusSolicutudGestion() {
//    guardarGestion('118', 'Cambio de Estado de la solicitud ', $('#ddlEstadoSol option:selected').text() + ' || ' + $("#txtUsoCredito").val());
//}


function clearAll() {
    jQuery(".clearAllFrm").find(':input').each(function () {
        switch (this.type) {
            case 'password':
            case 'text':
            case 'textarea':
            case 'file':
            case 'select-one':
            case 'select-multiple':
            case 'date':
            case 'number':
            case 'tel':
            case 'email':
            case 'datetime-local':
                jQuery(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
                break;
            case 'select':
                jQuery(this).val('-1');
                break;
        }
    });
}
//FIN ICORTES ONCHANGE SO
//INICIO AVARGAS 19/02/2019 PROCESO DE CAMBIO DE STATUS DEL ASESOR
function SelectChangeStatusAsesor() {
    var res = validateddlStatusAsesor();
    if (res == false) {
        return false;
    }
    ocultarBloqueo();
    var objBuscar = {
        IdCatDisponibilidad: $("#ddlStatusAsesor").val()
    };

    $.ajax({
        url: "/Colocacion/CambioSatusAsesor",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                //alertify.alert(result[0].Respuesta, function () {
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
                //});
                ocultarBloqueo();
            }
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert(errormessage.responseText);
        }
    });
}

function getStatusAsesor() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/CargaStatusAsesor",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlStatusAsesor").html("");
            $.each(result.ListCatDisponibilidad, function (key, item) {
                $("#ddlStatusAsesor").append(new Option(item.Descripcion, item.IdCatDisponibilidad));
            });
            //$("#ddlStatusAsesor").val(result.IdCatDisponibilidad);
            $('#ddlStatusAsesor  option[value="' + result.IdCatDisponibilidad + '"]').prop("selected", true);
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error(errormessage.responseText);
        }
    });
}

//Valdidation using jquery
function validateddlStatusAsesor() {
    var isValid = true;
    if (!$('#ddlStatusAsesor').val()) {
        $('#ddlStatusAsesor').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlStatusAsesor').css('border-color', 'lightgrey');
    }
    return isValid;
}
//Alfredo José Vargas Seinfarth// Final 19/02/2019

//FINAL AVARGAS 19/02/2019 PROCESO DE CAMBIO DE STATUS DEL ASESOR



//INICIO FCAMACHO 20 / 02 / 2019 FRAGMENTACION DEL GURADADO DE LA COLA RQ 02 COLOCACION
function procesarColaPersona() {
    if ($('#txtBusqueda').val() == '') {
        alertify.error("Debe Ingresar una Solicitud Antes de Procesarla");
    } else {
        var objProcesar = {
            IdSolicitud: $("#txtSolicitud").val(),
            IdTipoIdentificacion: $("#ddlNacionalidad").val(),
            Identificacion: $("#txtCedula").val(),
            VencimientoIdentificacion: $("#txtCedulaVence").val() == '1900-01-01 00:00:00.000' ? '01-01-1900' : $("#txtCedulaVence").val(),
            //PrimerNombre: $("#txtSolicitud").val(),
            //SegundoNombre: $("#txtSolicitud").val(),
            //PrimerApellido: $("#txtSolicitud").val(),
            //SegundoApellido: $("#txtSolicitud").val(),
            TelefonoCel: $("#txtTelefono").val(),
            //TelefonoFijo: $("#txtSolicitud").val(),
            //TelefonoLaboral: $("#txtSolicitud").val(),
            Correo: $("#txtCorreo").val(),
            //CorreoOpcional: $("#txtCorreo").val(),
            EstadoCivil: $("#ddlEstadoCivil").val(),
            Sexo: $("#ddlGenero").val(),
            FechaNacimiento: $("#txtFechaNacimiento").val() == '1900-01-01 00:00:00.000' ? '01-01-1900' : $("#txtFechaNacimiento").val(),
            Provincia: $("#ddlProvincia").val(),
            Canton: $("#ddlCanton").val(),
            Distrito: $("#ddlDistrito").val(),
            DetalleDireccion: $("#txtDetalle").val(),
        };
        //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
        guardarGestion('128', 'Mantenimiento Informacion Personal', '');
        //FIN FCAMACHO CAMACHO  GUARDAR GESTION 13/03/2019
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/Colocacion/ProcesarColaPersona",
            type: "POST",
            data: JSON.stringify(objProcesar),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                alertify.success("Proceso Realizado con Exito !!!");
                getPasosAgente();
                //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                //if ($("#ColaIniciada").val() == '0') { iniciarCola(); ocultarBloqueo(); }
                //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alertify.error(errormessage.responseText);
            }
        });
    }
}

function procesarColaReferenciaPersonas() {
    if ($('#txtBusqueda').val() == '') {
        alertify.error("Debe Ingresar una Solicitud Antes de Procesarla");
    } else {
        var objProcesar = {
            IdSolicitud: $("#txtSolicitud").val(),
            Identificacion: $("#txtCedula").val(),
            IdTipRefFamiliar: $("#ddlTipoReferencia").val(),
            NombreCompleto: $("#txtNombreRefFamiliar").val(),
            TelefonoFamiliar: $("#txtTelefonoRefFamiliar").val(),
            Empresa: $("#txtNombreRefEmpresa").val(),
            TelefonoEmpresa: $("#txtTelefonoRefEmpresa").val(),
            SupervisorDirecto: $("#txtNombreRefSupervisor").val(),
            NombreCompletoPersonal: $("#txtNombreRefPersonal").val(),
            TelefonoPersonal: $("#txtTelefonoRefPersonal").val(),
        };
        //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
        guardarGestion('126', 'Mantenimiento Referencias Personales', '');
        //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/Colocacion/ProcesarColaRefrenciaPersona",
            type: "POST",
            data: JSON.stringify(objProcesar),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                getPasosAgente();
                alertify.success("Proceso Realizado con Exito !!!");
                //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                //if ($("#ColaIniciada").val() == '0') { iniciarCola(); ocultarBloqueo(); }
                //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
            },
            error: function (errormessage) {
                ocultarBloqueo();
                alertify.error(errormessage.responseText);
            }
        });
    }
}

function procesarColaProducto() {
    if ($('#txtBusqueda').val() == '') {
        alertify.error("Debe Ingresar una Solicitud Antes de Procesarla");
    }
    else {
        //Validamos que la solicitud este en status 39 de lo contrario no procedemos con la gestion
        if (objSolicitudActual.Status == 39) {
            var objProcesar = {
                IdSolicitud: $("#txtSolicitud").val(),
                IdProducto: $("#ddlProducto").val(),
                Soporte: $("#ddlSoporte").val(),
                NombreProducto: $("#ddlProducto").val()
            };
            //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
            guardarGestion('127', 'Modificacion Producto', '');
            //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
            $.ajax({
                processing: true, // for show progress bar
                serverSide: true, // for process server side
                filter: true, // this is for disable filter (search box)
                orderMulti: false, // for disable multiple column at once
                url: "/Colocacion/ProcesarColaProducto",
                type: "POST",
                data: JSON.stringify(objProcesar),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    obtenerSolicitudActual($("#txtSolicitud").val())
                    getPasosAgente();
                    ocultarBloqueo();
                    alertify.success("Proceso Realizado con Exito !!!");
                    //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                    //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                },
                error: function (errormessage) {
                    ocultarBloqueo();
                    alertify.error("Metodo procesarColaProducto\n" + errormessage.responseText);
                }
            });
        }
        else {
            alert("Solo las solicitudes en Status: Aprobado Pendiente de Documentos, pueden proceder con esta gestion.\n\n Status actual de la solicitud: " + objSolicitudActual.DetalleStatus);
        }
    }
}

function procesarColaSolicitud() {
    var objProcesar = {
        IdSolicitud: $("#txtSolicitud").val(),
        UsoCredito: $("#txtUsoCredito").val(),
        Estado: $("#ddlEstadoSol").val(),
    };
    //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
    guardarGestion('129', 'Rechazar Solicitud', $("#txtUsoCredito").val());
    //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Colocacion/ProcesarColaSolicitud",
        type: "POST",
        data: JSON.stringify(objProcesar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ModalProcesarSolicitud").modal('hide');
            alertify.success("Proceso Realizado con Exito !!!");
            var colaIniciada = $("#ColaIniciada").val();
            if (colaIniciada == '1') {
                buscarCola();
                ocultarBloqueo();
            } else {
                clearAll();
                ocultarBloqueo();
            }
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("procesarColaSolicitud\n" + errormessage.responseText);
        }
    });
}

function mostrarModal() {
    if ($('#txtBusqueda').val() == '') {
        alertify.error("Debe Ingresar una Solicitud Antes de Rechazarla");
    } else {
        $("#ModalProcesarSolicitud").modal('show');
    }
    ocultarBloqueo();
}
//FIN FCAMACHO 20 / 02 / 2019 FRAGMENTACION DEL GURADADO DE LA COLA RQ 02 COLOCACION 


//inicio icortes genereacion de link de loop

function GeneracionLinkLoop() {
    if ($('#txtSolicitud').val() == '') {
        alertify.error("Debe Ingresar una Solicitud Antes de Generar el Link");
    } else {
        var objProcesar = {
            IdSolicitud: $("#txtSolicitud").val(),
            Identificacion: $("#txtCedula").val(),
            Telefono: $("#txtTelefono").val()
        };
        $.ajax({
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            orderMulti: false, // for disable multiple column at once
            url: "/Colocacion/GenerarLinkLoop",
            type: "POST",
            data: JSON.stringify(objProcesar),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $("#lblLink").text(result.Uri);
                alertify.success("Link Copiado en el ClipBoard");
                copyToClipboard($("#lblLink"));
                window.open(result.UrlWhatsApp, "Fenix", "Fenix");
                ocultarBloqueo();
            },
            error: function (errormessage) {
                alertify.error("GeneracionLinkLoop\n" + errormessage.responseText);
            }
        });
    }
}
function copyToClipboard(selector) {
    var $temp = $("<div>");
    $("body").append($temp);
    $temp.attr("contenteditable", true)
        .html($(selector).html()).select()
        .on("focus", function () { document.execCommand('selectAll', false, null) })
        .focus();
    document.execCommand("copy");
    $temp.remove();
    ocultarBloqueo();
}

//fin icortes generacion de link loop
//Metodo para reiniciar los tabs
function reiniciarTabs() {
    //Primero quitamos la clase active a todos los tabs.
    $('#ul-tabs-ventas li').removeClass('active');
    $('#ul-tabs-ventas li').removeClass('tab-activo');
    $('.line').removeClass('linea-activada');
    $('.tab-colocacion .tab-pane').removeClass('active');
    //Agregamos la clase active al tab por default
    $('#ul-tabs-ventas:first-child li').first().addClass('in active');
    $('#contentTab1').addClass('in active');
}
function validarPasosAgenteTabsNav(result) {
    ocultarBloqueo();
    //removemos la clase de pendientes para luego pintar las pendientes
    $('#ul-tabs-ventas li').removeClass('tab-pendientes');
    $.each(result, function (key, item) {
        switch (item.IdTipo) {
            case 1:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab1').addClass('tab-pendientes');
                }
                break;
            case 2:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab2').addClass('tab-pendientes');
                }
                break;
            case 3:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab3').addClass('tab-pendientes');
                }
                break;
            case 4:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab4').addClass('tab-pendientes');
                }
                break;
            case 5:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab5').addClass('tab-pendientes');
                }
                break;
            default:
                break;
        }
        ocultarBloqueo();
    });
}
function seleccionarTab(idTipo) {
    ocultarBloqueo();
    $('#ul-tabs-ventas li').removeClass('active');
    $('#tab' + idTipo).addClass('in active');
    ocultarBloqueo();
    //cerrarPanelPAsos();
}
function cerrarPanelPAsos() {
    ocultarBloqueo();
    $('#panel-acordion').find('.panel-acordion-content').slideToggle('fast', 'swing');
    $('#panel-acordion').toggleClass('ocultar');
    $('#panel-acordion').toggleClass('mostrar');
    $('#btn-cerrar-pasos').toggleClass('hide');
    ocultarBloqueo();
}
function seleccionarTabPendiente() {
    ocultarBloqueo();
    var clase = $("#tblPasosXAgentes:first td a").attr('class');
    if (clase == null) {
        clase = "tabTipo-7";
    }
    var idTab = clase.replace('tabTipo-', '');
    if (idTab == 0) {
        if ($('.tabTipo-0').text() == 'VERIFICACION FIRMA') {
            clase = $("#tblPasosXAgentes tr:nth-child(2) td a").attr('class');
            idTab = clase.replace('tabTipo-', '');
            if (idTab == 0) {
        idTab = 7;
            }
        }
        else {
            idTab = 7;
        }
        //Esta validacion la usamos para mostrar por defecto el tab cuando se muestre el tab de info personal
        //Nota: se debe hacer asi, pues al ser una tab-content dentro de otro tab-content, no reconoce la clase active por default
        if (idTab == 1) {
            $("#InfocontentTab1").addClass('active');
        }
    }
    //quitamos la clase active de los tabs y luego agregamos la clase active al tab correcto
    $('#ul-tabs-ventas li').removeClass('active');
    $('#tab' + idTab).addClass('in active');
    //removemos la clase active en los content tabs y luego agregamos la clase active en el content tab correcto
    $('.tab-colocacion.tab-pane.fade').removeClass('active');
    $('#contentTab' + idTab).addClass('in active');
    //agregar active a la info de las cuentas bancarias
    $('#tabCuentaBanc').addClass('in active');
    //pintamos los tabs dependiendo del progreso.
    //pintar tabs dependiendo del progreso o el tab activo
    var indiceTab = $('#tab' + idTab).index();
    $('.line').removeClass('linea-activada');
    $('.line:lt(' + indiceTab + ')').addClass('linea-activada');

    $('#ul-tabs-ventas li').removeClass('tab-activo');
    $('#ul-tabs-ventas li:lt(' + indiceTab + ')').addClass('tab-activo');
    ocultarBloqueo();
}

//Replicamos el metodo de llamar las cuentas bancarias
function CargaCuentaBancaria() {
    if ($('#txtCedula').val() == "") return false;
    $('.transfer').html();
    var empObj = {
        Identificacion: $('#txtCedula').val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/CuentaBancaria/ConsultaCuentasBancarias",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            ocultarBloqueo();
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.DescPredet + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td>' + item.DescCuenta + '</td>';
                html += '<td>' + item.DescMoneda + '</td>';
                html += '<td>' + item.Cuenta + '</td>';
                html += '<td id="cuenta-simpe">' + item.CuentaSinpe + '</td>';
                if (item.Verificacion == 1) {
                    html += '<td width=150px>' + '<p>' + item.VerificacionStatus + '<span id="verificar-mail-icon" class="far fa-check-circle"></span></p>' + '</td>';
                }
                else {
                    html += '<td width=150px>' + '<p> ' + item.VerificacionStatus + ' <span id="verificar-mail-icon" class="verificacionFallida far fa-times-circle"></span></p>' + '</td>';
                }
                html += '<td><a class="btn-tesoreria btn btn-primary btn-primary-asignar" onclick="mostrarImagenSinpe(' + objSolicitudActual.Identificacion + ')">Imagen</a></td>';
                html += '<td width=175px ><form class="form-inline"><div role="group"><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.Id + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.Id + ',' + $('#txtCedula').val() + ')">Eliminar</a></div></form></td>';
                html += '</tr>';
            });

            $("#tblBCO").dataTable().fnClearTable();
            $("#tblBCO").dataTable().fnDestroy();
            //$('.transfer').html(html);
            $("#tblBCO").dataTable({
                "ordering": false,
                "paging": false,
                "searching": false
            });
            $('#bodyTableBCO').html(html);
            $("select[name*='bodyTableBCO_length']").change();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alert("CargaCuentaBancaria\n" + errormessage.responseText);
        }
    });
}

function doesFileExist(urlToFile) {
    var xhr = new XMLHttpRequest();
    xhr.open('HEAD', urlToFile, false);
    xhr.send();

    if (xhr.status == "404") {
        return false;
    } else {
        return true;
    }
    ocultarBloqueo();
}
//Validamos si esta info antes de marcar un paso como completado en el dashboard de pasos
function verificarPaso(paso) {
    ocultarBloqueo();
    if ($('#lblDetalleOrigen').text().indexOf("RENOVACION") > -1) { //Si es una renovacion, no hace ninguna validacion devuelve true para q pueda marcar el paso como completado
        return true;
    } else {
        switch (paso) {
            case 9://Foto Cedula Frontal
                if (!doesFileExist($('#UrlFotoCedula').attr('src'))) {
                    alertify.error("No Existe Foto de Cedula Frontal. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 10://Foto Cedula Trasera
                if (!doesFileExist($('#UrlFotoCedulaTrasera').attr('src'))) {
                    alertify.error("No Existe Foto de Cedula Trasera. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 11://Foto selfie
                if (!doesFileExist($('#UrlFotoSelfie').attr('src'))) {
                    alertify.error("No Existe Foto de Selfie. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 14://Foto selfie
                if (!doesFileExist($('#UrlFotoFirma').attr('src'))) {
                    alertify.error("No Existe Foto de la Firma. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 15://Solicita Referencias
                if (
                    (
                        ($('#ddlTipoReferencia').val() != "-1" && $('#ddlTipoReferencia').val() != "0" && $('#ddlTipoReferencia').val() != null) &&
                        ($('#txtNombreRefFamiliar').val() != "") &&
                        ($('#txtTelefonoRefFamiliar').val() != "" && $('#txtTelefonoRefFamiliar').val() != "0")
                    ) ||
                    (
                        ($('#txtNombreRefEmpresa').val() != "") &&
                        ($('#txtTelefonoRefEmpresa').val() != "" && $('#txtTelefonoRefFamiliar').val() != "0") &&
                        ($('#txtNombreRefSupervisor').val() != "")
                    ) ||
                    (
                        ($('#txtNombreRefPersonal').val() != "") &&
                        ($('#txtTelefonoRefPersonal').val() != "" && $('#txtTelefonoRefPersonal').val() != "0")
                    )
                ) {
                    return true;
                }
                else {
                    alertify.error("No Existe Ninguna Referencia Agregada. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                break;
            case 12://Solicita Cuenta Bancaria
                if ($('#cuenta-simpe').text() == '' || $('#cuenta-simpe').text() == null) {
                    alertify.error("No Existe Ninguna Cuenta Bancaria. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 101://VErificacion email
                if ($('#verificar-mail-icon').hasClass('error')) {
                        alertify.error("El Correo no es valido. No Puede Marcar Este Paso como Completo.");
                    return false;
                }
                else {
                    //Si el correo es correcto vamos a guardarlo 
                    procesarColaPersona();
                    return true;
                }
                break;
            default:
                alertify.error("No Existe Ninguna Validacion Para el Paso: " + paso);
                return true;
                break;
        }
    }
    ocultarBloqueo();
}
function InsertaSolicitud() {
    var res = validaInsertSolicitud();
    if (res == false) {
        return false;
    }
    var objProcesar = {
        Identificacion: $("#txtIdentificacion").val(),
        Telefono: $("#txtTelefonoIngresoSolicitudes").val(),
        Mail: $("#txtMailIngresoSolicitudes").val(),
        SubOrigen: $("#ddlSubOrigen option:selected").text()
    };
    $.ajax({
        url: "/SolicitudesVentas/InsertarNuevaSolictitudes",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Status == null || result.Status == 0 || result.Status == -1 || result.Status == 12 || result.Status == 15 || result.Status == 54 || result.Status == 55 || result.Status == 67 || result.Status == 68 || result.Status == 69 || result.Status == 70 || result.Status == 117) {
                //mostramos el panel de rechazo y ocultamos los demas
                $('#lblNombreRechazo').text(result.Nombre);
                $('#lblEstadoSolicitud').text(result.StatusDescripcion);
                $('#solicitudes-panel-3').hide();
                $('#solicitudes-panel-2').hide();
                $('#btnCerrarPanelRechazo').show(); //Boton para cerrar el panel en caso de rechazo
                $('#solicitudes-panel-1').show('slow');
                if (result.Mensaje != null && result.Mensaje != '' && result.Status == -1) $('#lblEstadoSolicitud').text(result.Mensaje);
                else
                    if (result.Status == null || result.Status == 0) {
                        $('#lblEstadoSolicitud').text("Solicitud no Procesada");
                    }
            }
            else {
                //	$("#lblValMontoMaximo").text(result.MontoMaximo);
                llenarComboboxProducto(result.MontoMaximo);
                //Vamos a mostrar los paneles de los controles
                $("label[for='lblValMontoMaximo']").text(result.MontoMaximo);
                $("label[for='lblValIdSolicitud']").text(result.IdSolicitud);
                $('#lblNombreRechazo').text(result.Nombre);
                $('#lblEstadoSolicitud').text(result.StatusDescripcion);
                $('#btnCerrarPanelRechazo').hide(); //Boton para cerrar el panel en caso de rechazo, se esconde porq panel 3 trae un boton con la misma funcion
                $('#solicitudes-panel-1').show('slow');
                $('#solicitudes-panel-2').show('slow');
                $('#solicitudes-panel-3').show('slow');
                $("#btnInsertar").hide();
            }
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("InsertaSolicitud\n" + errormessage.responseText);
        }
    });
}

function llenarComboboxProducto(MontoMaximo) {
    llenarComboboxFrecuencia();
    var res = validateSelectMonto();
    if (res == false) {
        return false;
    }
    var objProcesar = {
        MontoMaximo: MontoMaximo
    };
    $.ajax({
        url: "/SolicitudesVentas/CargaComboProducto",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlMontoCredito").html("");
            $("#ddlMontoCredito").append(new Option("--- Seleccione ---", "-1"));



            $.each(result.ListMontoCredito, function (key, item) {
                $("#ddlMontoCredito").append(new Option(item.Descripcion, item.IdMontoCredito));
            });

            $("#ddlPlazoPago").html("");
            $("#ddlPlazoPago").append(new Option("--- Seleccione ---", "-1"));


            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("llenarComboboxProducto\n" + errormessage.responseText);
        }
    });
}

function llenarComboboxPlazoProducto() {
    var res = validateSelectMonto();
    if (res == false) {
        return false;
    }
    var objProcesar = {
        IdMontoCredito: $("#ddlMontoCredito").val()
    };
    $.ajax({
        url: "/SolicitudesVentas/CargaComboPlazoProducto",
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

            $("#ddlFrecuencia").html("");
            $("#ddlFrecuencia").append(new Option("--- Seleccione ---", -1));
            $("#ddlFrecuencia").append(new Option("Quincenal", 2));
            $("#ddlFrecuencia").append(new Option("Mensual", 3));
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxPlazoProducto\n" + errormessage.responseText);
        }
    });
}
function llenarComboboxFrecuencia() {
    var res = validateSelectMonto();
    if (res == false) {
        return false;
    }
    $.ajax({
        url: "/SolicitudesVentas/CargaComboFrecuenciaCredito",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlFrecuencia").html("");
            $("#ddlFrecuencia").append(new Option("--- Seleccione ---", -1));
            $.each(result, function (key, item) {
                $("#ddlFrecuencia").append(new Option(item.Descripcion, item.IdFrecuencia));
            });
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxFrecuencia\n" + errormessage.responseText);
        }
    });
}
//cargar los montos para los comboBox de la ventana de colocacion
function cargarMontosProductos() {
    var objProcesar = {
        IdSolicitud: $('#txtSolicitud').val()
    };
    $.ajax({
        async: false,
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
            cargarComboboxPlazoProducto();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("llenarComboboxProducto\n" + errormessage.responseText);
        }
    });
}
//cargar la frecuencia para los comboBox de la ventana de colocacion
function cargarComboboxFrecuencia() {
    var objProcesar = {
        IdMontoCredito: $("#ddlMontoCredito").val()
    };
    $.ajax({
        async: false,
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
            //llamamos al metodo para cargar los datos del producto por defecto
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxFrecuencia\n" + errormessage.responseText);
        }
    });
}
//cargar la el plazo para los comboBox de la ventana de colocacion al cambiar el monto de producto
function cargarComboboxPlazoProducto() {
    //cargarComboboxFrecuencia();
    $("#ddlPlazoPago").html("");
    var objProcesar = {
        IdMontoCredito: $("#ddlMontoCredito").val()
    };
    $.ajax({
        async: false,
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
            cargarComboboxFrecuencia();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("llenarComboboxPlazoProducto\n" + errormessage.responseText);
        }
    });
}
//cargar la el plazo para los comboBox de la ventana de colocacion por defecto
function cargarComboboxPlazoProductoDefault() {
    $("#ddlPlazoPago").html("");
    $("#ddlPlazoPago").append(new Option("--- Seleccione ---", "-1"));
    if ($('#ddlMontoCredito').val() == 1) {
        $("#ddlPlazoPago").append(new Option('1 mes', 1));
        $("#ddlPlazoPago").append(new Option('2 meses', 2));
        $("#ddlPlazoPago").append(new Option('3 meses', 3));
    }
    if ($('#ddlMontoCredito').val() == 2) {
        $("#ddlPlazoPago").append(new Option('1 mes', 4));
        $("#ddlPlazoPago").append(new Option('2 meses', 5));
        $("#ddlPlazoPago").append(new Option('3 meses', 6));
        $("#ddlPlazoPago").append(new Option('4 meses', 7));
        $("#ddlPlazoPago").append(new Option('5 meses', 8));
    }
    if ($('#ddlMontoCredito').val() == 3) {
        $("#ddlPlazoPago").append(new Option('1 mes', 9));
        $("#ddlPlazoPago").append(new Option('2 meses', 10));
        $("#ddlPlazoPago").append(new Option('3 meses', 11));
        $("#ddlPlazoPago").append(new Option('4 meses', 12));
        $("#ddlPlazoPago").append(new Option('5 meses', 13));
    }
    if ($('#ddlMontoCredito').val() == 4) {
        $("#ddlPlazoPago").append(new Option('1 mes', 14));
        $("#ddlPlazoPago").append(new Option('2 meses', 15));
        $("#ddlPlazoPago").append(new Option('3 meses', 16));
        $("#ddlPlazoPago").append(new Option('4 meses', 17));
        $("#ddlPlazoPago").append(new Option('5 meses', 18));
        $("#ddlPlazoPago").append(new Option('6 meses', 19));
    }
    if ($('#ddlMontoCredito').val() == 5) {
        $("#ddlPlazoPago").append(new Option('1 mes', 20));
        $("#ddlPlazoPago").append(new Option('2 meses', 21));
        $("#ddlPlazoPago").append(new Option('3 meses', 22));
        $("#ddlPlazoPago").append(new Option('4 meses', 23));
        $("#ddlPlazoPago").append(new Option('5 meses', 24));
        $("#ddlPlazoPago").append(new Option('6 meses', 25));
    }
    if ($('#ddlMontoCredito').val() == 6) {
        $("#ddlPlazoPago").append(new Option('1 mes', 26));
        $("#ddlPlazoPago").append(new Option('2 meses', 27));
        $("#ddlPlazoPago").append(new Option('3 meses', 28));
        $("#ddlPlazoPago").append(new Option('4 meses', 29));
        $("#ddlPlazoPago").append(new Option('5 meses', 30));
        $("#ddlPlazoPago").append(new Option('6 meses', 31));
        $("#ddlPlazoPago").append(new Option('9 meses', 32));
        $("#ddlPlazoPago").append(new Option('12 meses', 33));
    }
    if ($('#ddlMontoCredito').val() == 7) {
        $("#ddlPlazoPago").append(new Option('6 mes', 34));
        $("#ddlPlazoPago").append(new Option('9 meses', 35));
        $("#ddlPlazoPago").append(new Option('12 meses', 36));
    }
    if ($('#ddlMontoCredito').val() == 8) {
        $("#ddlPlazoPago").append(new Option('6 mes', 37));
        $("#ddlPlazoPago").append(new Option('9 meses', 38));
        $("#ddlPlazoPago").append(new Option('12 meses', 39));
    }
    if ($('#ddlMontoCredito').val() == 9) {
        $("#ddlPlazoPago").append(new Option('6 mes', 40));
        $("#ddlPlazoPago").append(new Option('9 meses', 41));
        $("#ddlPlazoPago").append(new Option('12 meses', 42));
    }
    if ($('#ddlMontoCredito').val() == 10) {
        $("#ddlPlazoPago").append(new Option('6 mes', 43));
        $("#ddlPlazoPago").append(new Option('9 meses', 44));
        $("#ddlPlazoPago").append(new Option('12 meses', 45));
    }
    ocultarBloqueo();
}
function calcularProducto() {
    if ($('#ddlMontoCredito').val() > 0 && $('#ddlPlazoPago').val() > 0 && $('#ddlFrecuencia').val() > 0) {
        var objProcesar = {
            IdMontoCredito: $('#ddlMontoCredito').val(),
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
function CerrarPanelIngresoSolicitud() {
    $('#txtIdentificacion').val('');
    $('#txtTelefonoIngresoSolicitudes').val('');
    $('#txtMailIngresoSolicitudes').val('');
    $('#GuardaProductoSolicitud').val('');
    $('#lblNombreRechazo').text('');
    $('#lblEstadoSolicitud').text('');
    $('#lbStatusRechazo').text('');
    $('#lblValMontoMaximo').text('');
    $('#lblValIdSolicitud').text('');

    $("#btnInsertar").show('slow');
    $('#solicitudes-panel-1').hide('slow');
    $('#solicitudes-panel-2').hide('slow');
    $('#solicitudes-panel-3').hide('slow');

}
function GuardaProductoSolicitud() {
    ocultarBloqueo();
    var res = validate();
    if (res == false) {
        return false;
    }
    ocultarBloqueo();
    var objProcesar = {
        IdSolicitud: $("#lblValIdSolicitud").text(),
        IdMontoCredito: $("#ddlMontoCredito").val(),
        IdPlazoCredito: $("#ddlPlazoPago").val(),
        IdFrecuencia: $("#ddlFrecuencia").val()
    };
    $.ajax({
        url: "/SolicitudesVentas/GuardaProductoSolicitud",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alertify.success("Se ha ingresado correctamente el producto de la solicitud !!!" + result.IdSolicitud);
            CerrarPanelIngresoSolicitud();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            alertify.error("GuardaProductoSolicitud\n" + errormessage.responseText);
        }
    });
}
function validaInsertSolicitud() {
    var isValid = true;
    if ($('#txtIdentificacion').val().trim() == "") {
        $('#txtIdentificacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtIdentificacion').css('border-color', 'lightgrey');
    }
    if ($('#txtTelefonoIngresoSolicitudes').val().trim() == "") {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'lightgrey');
    }
    ocultarBloqueo();
    return isValid;
}

function validateSelectMonto() {
    var isValid = true;
    if ($('#txtIdentificacion').val().trim() == "") {
        $('#txtIdentificacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtIdentificacion').css('border-color', 'lightgrey');
    }
    if ($('#txtTelefonoIngresoSolicitudes').val().trim() == "") {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'lightgrey');
    }
    ocultarBloqueo();
    return isValid;
}

function validate() {
    var isValid = true;
    if ($('#txtIdentificacion').val().trim() == "") {
        $('#txtIdentificacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtIdentificacion').css('border-color', 'lightgrey');
    }
    if ($('#txtTelefonoIngresoSolicitudes').val().trim() == "") {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtTelefonoIngresoSolicitudes').css('border-color', 'lightgrey');
    }
    if ($('#lblValIdSolicitud').text().trim() == "") {
        $('#lblValIdSolicitud').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lblValIdSolicitud').css('border-color', 'lightgrey');
    }
    if ($('#lblValMontoMaximo').text().trim() == "") {
        $('#lblValMontoMaximo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lblValMontoMaximo').css('border-color', 'lightgrey');
    }
    if (!$('#ddlMontoCredito').val() || $('#ddlMontoCredito').val() == -1) {
        $('#ddlMontoCredito').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlMontoCredito').css('border-color', 'lightgrey');
    }
    if (!$('#ddlPlazoPago').val() || $('#ddlPlazoPago').val() == -1) {
        $('#ddlPlazoPago').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlPlazoPago').css('border-color', 'lightgrey');
    }
    if (!$('#ddlFrecuencia').val() || $('#ddlFrecuencia').val() == -1) {
        $('#ddlFrecuencia').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlFrecuencia').css('border-color', 'lightgrey');
    }
    ocultarBloqueo();
    return isValid;
}
//Jabarca inicio
function CreaDocumentoDom() {

    //Activamos el spin del boton 
    $('#icono-carga-domiciliacion').addClass('fas fa-spinner fa-pulse');
    var objBuscar = {
        Identificacion: $("#txtCedula").val(),
        IdSolicitud: $("#txtSolicitud").val(),
        UrlFotoFirma: $('#UrlFotoFirma').attr('src'),
        UrlFotoCedulaFrontal: $('#UrlFotoCedula').attr('src'),
        UrlFotoCedulaTrasera: $('#UrlFotoCedulaTrasera').attr('src')
    };

    $.ajax({
        url: "/GestionDocumentos/CrearDocumentoDom",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.Base64 !== null) {
                download(result.Base64, result.Name, result.Type);
                alertify.success(result);
            }
            else {
                alertify.alert("Error al generar el documento de Domiciliacion.\n Favor verificar la siguiente informacion:\n1. Que el cliente tenga una cuenta cliente asignada y predeterminada.\n2.Que el cliente tenga un producto asignado\n\nNota: De estar correcta toda la informacion del cliente, favor comunicarse con el administrador del sistema.");
            }
            //Desactivamos el spin del boton 
            $('#icono-carga-domiciliacion').removeClass('fas fa-spinner fa-pulse');
            ocultarBloqueo();
        },
        error: function (errormessage) {
            ocultarBloqueo();
            //Desactivamos el spin del boton 
            $('#icono-carga-domiciliacion').removeClass('fas fa-spinner fa-pulse');
            alertify.alert("Error al generar el documento de Domiciliacion.\n Favor verificar la siguiente informacion:\n1. Que el cliente tenga una cuenta cliente asignada y predeterminada.\n2.Que el cliente tenga un producto asignado\n\nNota: De estar correcta toda la informacion del cliente, favor comunicarse con el administrador del sistema.");
        }
    });
}
//Jabarca Final
function cargarHistorialSolicitudes() {
    var objProcesar = {
        Identificacion: $('#txtCedula').val()
    };
    $.ajax({
        url: "/Colocacion/getHistorialSolicitudes",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.IdSolicitud + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td>' + item.Origen + '</td>';
                html += '<td>' + item.FechaIngreso + '</td>';
                html += '<td>' + item.UsuarioModificacion + '</td>';
                html += '</tr>';
            });
            $("#tableHistorialSolicitudes").dataTable().fnClearTable();
            $("#tableHistorialSolicitudes").dataTable().fnDestroy();
            $('#tblHistorialSolicitudes').html(html);
            $("#tableHistorialSolicitudes").dataTable();
            $("select[name*='tblHistorialSolicitudes_length']").change();
        },
        error: function (errormessage) {
            alertify.error("cargarHistorialSolicitudes\n" + errormessage.responseText);
        }
    });
}

function descargarPagare() {
    download($("#UrlDirectorioPagare").attr('src'), $("#txtNombre").val(), "application/pdf");
    //alert($("#UrlDirectorioPagare").attr('src'));
}

function descargarImagenes(tipo) {
    switch (tipo) {
        case 1:
            download($("#UrlFotoCedula").attr('src'), $("#txtNombre").val(), "image/jpeg");
            break;
        case 2:
            download($("#UrlFotoSelfie").attr('src'), $("#txtNombre").val(), "image/jpeg");
            break;
        case 3:
            download($("#UrlFotoCedulaTrasera").attr('src'), $("#txtNombre").val(), "image/jpeg");
            break;
        case 4:
            download($("#UrlFotoFirma").attr('src'), $("#txtNombre").val(), "image/jpeg");
            break;
    }
}
function mostrarModalComentarioRepro() {
    $("#ModalComentarioReprogramacion").modal('show');
}
//Mediante este metodo vamos a obtener la informacion importante de la solicitud para poder usarla de manera local
function obtenerSolicitudActual(idSolcitud) {
    $.ajax({
        async: false,
        url: "/Colocacion/getSolicitudActual/?idSolicitud=" + idSolcitud,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            objSolicitudActual = result;
        },
        error: function (errormessage) {
            alertify.error("cargarHistorialSolicitudes\n" + errormessage.responseText);
        }
    });
}
function seleccionarTabManual(idTab) {
    if (idTab > 0 && idTab < 8) {
        //quitamos la clase active de los tabs y luego agregamos la clase active al tab correcto
        $('#ul-tabs-ventas li').removeClass('active');
        $('#tab' + idTab).addClass('in active');
        //removemos la clase active en los content tabs y luego agregamos la clase active en el content tab correcto
        $('.tab-colocacion.tab-pane.fade').removeClass('active');
        $('#contentTab' + idTab).addClass('in active');
        //agregar active a la info de las cuentas bancarias
        $('#tabCuentaBanc').addClass('in active');
        //pintamos los tabs dependiendo del progreso.
        //pintar tabs dependiendo del progreso o el tab activo
        var indiceTab = $('#tab' + idTab).index();
        $('.line').removeClass('linea-activada');
        $('.line:lt(' + indiceTab + ')').addClass('linea-activada');
        $('#ul-tabs-ventas li').removeClass('tab-activo');
        $('#ul-tabs-ventas li:lt(' + indiceTab + ')').addClass('tab-activo');
        ocultarBloqueo();
    }
    else {
        alertify.error("El indice del tab a selecionar debe ser mayor a 0 y menor a 8");
    }
}
//INICIO -- AVARGAS -- Se agrega las opciones de contactos y direcciones para Colocación
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
        async: true,
        url: "/Colocacion/CargaContactos",
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
//AVARGAS - 23/01/2019 - Mantenimiento de Teléfonos del cliente --INICIO--
function getCatalogoTelefono() {
    var id = 0;
    $.ajax({
        url: "/Colocacion/ConsultaCatalogoTel/" + id,
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
function getTelefono(id) {
    $('#EditId').css('border-color', 'lightgrey');
    $('#ddlEditPredeterminado').css('border-color', 'lightgrey');
    $('#EditRelacion').css('border-color', 'lightgrey');
    $('#EditTelefono').css('border-color', 'lightgrey');
    getCatalogoTelefono();
    $.ajax({
        url: "/Colocacion/ObtieneTelefono/" + id,
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
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//function for updating employee's record
function ActualizarTelefono() {
    var res = validateTelEdit();
    if (res == false) {
        return;
    }
    var empObj = {
        IdTelefono: $('#EditId').val(),
        Relacion: $('#EditRelacion').val(),
        Telefono: $('#EditTelefono').val(),
        Predeterminado: $('#ddlEditPredeterminado option:selected').val()
    };
    $.ajax({
        url: "/Colocacion/ActualizarTelefono",
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
                cargarContactos($('#txtCedula').val());
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
        Identificacion: $('#txtCedula').val(),
        FuenteDatos: "Ingresado por Asesor",
        Tipo: $('#ddlNuevoTipoTEL option:selected').val(),
        Relacion: $('#NuevoRelacion').val(),
        Telefono: $('#NuevoTelefono').val(),
        Predeterminado: $('#ddlNuevoPredeterminado option:selected').val()
    };
    $.ajax({
        url: "/Colocacion/IngresarTelefono",
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
                cargarContactos($('#txtCedula').val());
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
                    url: "/Colocacion/EliminaTelefono",
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

//AVARGAS - 23/01/2019 - Mantenimiento de Teléfonos del cliente --FINAL--

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
        url: "/Colocacion/ContactosDireccion",
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
        url: "/Colocacion/ConsultaCatalogoDIR/" + id,
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
        url: "/Colocacion/ObtieneDireccion/" + id,
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
        url: "/Colocacion/ActualizarDireccion",
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
                cargarContactos_Direccion($('#txtCedula').val());
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
        Identificacion: $('#txtCedula').val(),
        FuenteDatos: "Ingresado por Asesor",
        Tipo: $('#ddlNuevoTipoDIR option:selected').val(),
        Relacion: $('#NuevoRelacionDIR').val(),
        Direccion: $('#NuevoDireccion').val(),
        Predeterminado: $('#ddlNuevoPredeterminadoDIR option:selected').val()
    };
    $.ajax({
        url: "/Colocacion/CrearDireccion",
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
                cargarContactos_Direccion($('#txtCedula').val());
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
    if ($('#NuevoDireccion').val().trim() == "") {
        $('#NuevoDireccion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NuevoDireccion').css('border-color', 'lightgrey');
    }
    return isValid;
}
//FIN -- AVARGAS -- Se agrega las opciones de contactos y direcciones para Colocación


//INICIO, ICORTES, CALCULA PLAN PAGOS
function CalcularPlanPagos() {
    var empObj = {
        IdSolicitud: $("#txtSolicitud").val(),
        FechaPago: $("#txtFechaPlanPago").val()
    };

    $('.planPago').html('');
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Colocacion/CalculaPlanPago",
        type: "POST",
        data: JSON.stringify(empObj),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.FechaVencimiento + '</td>';
                html += '<td>' + numFormat(item.MontoCuota, 1, true) + '</td>';
                html += '</tr>';
            });
            $("#tblPlanPago").dataTable().fnClearTable();
            $("#tblPlanPago").dataTable().fnDestroy();
            //$("#tblPlanPago").dataTable({
            //    "ordering": false,
            //    "paging": false,
            //    "searching": false
            //});
            $('.planPago').html(html);

            $("#tblPlanPago").DataTable();
            $("select[name*='tblPlanPago_length']").change();
            $('#modalPlanPago').modal('show');

        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error');

        }
    });
}

//FIN, ICORTES, CALCULA PLAN PAGOS
//Inicio Cronometro
function GuardarTiempo(origen) {
    var objBuscar = {
        Tiempo: tiempoSolicitud,
        IdSolicitud: objSolicitudActual.IdSolicitud,
        Origen: origen
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Colocacion/GuardarTiempoSolicitud",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            clearInterval(alarma);
            $('#lblTiempoGestion').text('Tiempo Gestion:');
            $('.titulo-pagina').removeClass('titulo-pagina-alterno');
            alertify.success('Tiempo de Solicitud Guardado').delay(5)
        },
        error: function (errormessage) {
            alertify.error('Ocurrion un Error Al guardar el tiempo');
        }
    });
}
function cronometro(accion){
    if (accion == 'Iniciar') {
        var tiempo = {
            hora: 0,
            minuto: 0,
            segundo: 0
        };
        //reiniciamos el cronometro y la alarma en caso q esten activas.
        tiempoSolicitud = 0;
        clearInterval(tiempo_corriendo);
        activarAlarma('Detener');
        tiempo_corriendo = setInterval(function () {
            // Segundos
            tiempo.segundo++;
            if (tiempo.segundo >= 60) {
                tiempo.segundo = 0;
                tiempo.minuto++;
            }
            $("#lblTiempoMinuto").text(tiempo.minuto < 10 ? '0' + tiempo.minuto : tiempo.minuto);
            $("#lblTiempoSegundo").text(tiempo.segundo < 10 ? '0' + tiempo.segundo : tiempo.segundo);
            //vamos sumando los segundos en la variable global
            tiempoSolicitud++;
            if (tiempoSolicitud == 300) {
                activarAlarma('Activar');
                //validamos si hay tiempo en la solicitud actual para guardarlo en la bd.
                //if (tiempoSolicitud > 0) {
                //    if (busquedaActiva == 0) {
                //        GuardarTiempo('Cola Ventas');
                //    } else {
                //        GuardarTiempo('Busqueda');
                //    }                    
                //}
            }
        }, 1000);
    }
    if (accion == 'Detener') {
        clearInterval(tiempo_corriendo);
    }
}
function activarAlarma(Accion) {
    if (Accion == 'Activar') {
        $('#lblTiempoGestion').text('Tiempo Excedido:');
        alarma = setInterval(function () {
            $('#titulo-alarma').toggleClass('titulo-pagina-alterno');
        }, 500);
    }
    if (Accion == 'Detener') {
        clearInterval(alarma);
        $('#lblTiempoGestion').text('Tiempo Gestion:');
        $('#titulo-alarma').removeClass('titulo-pagina-alterno');
    }
}
//Fin Cronometro
//INICIO Nuevo Version 
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
            url: "/Colocacion/BuscarClienteNombre",
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
function mostrarModalSelecionarConsulta(result) {
    $('#icono-cargando-modal-busqueda').hide();
    resultadoBusqueda = result;
    var html = '';
    var indice = 0;
    $.each(result, function (key, item) {
        html += '<tr id="' + item.IdSolicitud + '" onclick="mostrarConsultaSeleccionada(' + item.IdSolicitud + ')">';
        html += '<td>' + item.Nombre + '</td>';
        html += '<td>' + item.Identificacion + '</td>';
        html += '<td>' + item.IdSolicitud + '</td>';
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
function ocultarModalSelecionarConsulta() {
    $("#ElegirConsulta").modal('hide');
    $('#icono-cargando-modal-busqueda').hide();
}
function mostrarConsultaSeleccionada(IdSolicitudBuscar) {
    //cerramos la ventana del modal de seleccion de solicitud
    $('#icono-cargando-modal-busqueda').show('slow');
    buscarSolicitudManual(IdSolicitudBuscar);
    MostrarFiltros('Ocultar');
}
function buscarColaKey(e) {
    if (e.keyCode == 13) {
        if (e.currentTarget.id == 'txtPrimerNombre' || e.currentTarget.id == 'txtSegundoNombre' || e.currentTarget.id == 'txtPrimerApellido' || e.currentTarget.id == 'txtSegundoApellido') {
            BuscarNombre('ConsultaCobros');
        }
        if (e.currentTarget.id == 'txIdentificacion') {
            buscarSolicitudManual();
        }
    }
}
function validateEmail(email) {
    var objBuscar = {
        Mail: email
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/Colocacion/ValidarEmail",
        type: "POST",
        data: JSON.stringify(objBuscar),
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data.valid == "true") {
                $('#verificar-mail-icon').removeClass('fas fa-times-circle');
                $('#verificar-mail-icon').addClass('fas fa-check-circle');
                $('#verificar-mail-icon').removeClass('error');
                $('#verificar-mail-icon').addClass('correcto');
                procesarColaPersona();
            }
            else {
                $('#verificar-mail-icon').removeClass('fas fa-check-circle');
                $('#verificar-mail-icon').addClass('fas fa-times-circle');
                $('#verificar-mail-icon').removeClass('correcto');
                $('#verificar-mail-icon').addClass('error');
            }
        },
        error: function (errormessage) {
            alertify.error('Error al validar el Email');
        }
    });
}
function mostrarImagenSinpe(Identificacion) {
    var empObj = {
        Identificacion: Identificacion
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/Colocacion/GetImagenSinpe",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result.Mensaje != "Completo") {
                    alertify.error(result.Mensaje);
                } else {
                    $("#imagen-sinpe img").attr("src", result.UrlImagenSinpe);
                    $('#ImagenSinpe').modal('show');
                }
            },
            error: function (errormessage) {
                alertify.error(result.Mensaje);
            }
        });
}
function guardarTelefonoLaboral() {
    var teleofonoLaboral = $('#txtTelefonoLaboral').val();
    var comentarioTeleofonoLaboral = $('#txtTelefonoLaboralComentario').val();
    if (teleofonoLaboral != '' && comentarioTeleofonoLaboral != '') {
        var empObj = {
            Identificacion: $('#txtCedula').val(),
            Telefono: teleofonoLaboral,
            ComentarioTelefonoLaboral: comentarioTeleofonoLaboral,
            IdSolicitud: $('#txtSolicitud').val()
        };
        processing: true; // for show progress bar  
        serverSide: true; // for process server side  
        filter: true; // this is for disable filter (search box)  
        orderMulti: false; // for disable multiple column at once
        paging: false;
        $.ajax(
            {
                url: "/Colocacion/GuardarTelefonoLaboral",
                type: "POST",
                data: JSON.stringify(empObj),
                contentType: "application/json;charset=utf-8",
                success: function (result) {
                    if (result.MensajeSalida != "Completo") {
                        alertify.success('Telefono Laboral Guardado');
                        getPasosAgente();
                    } else {
                        alertify.error('ERROR al Guardar el Telefono Laboral');
                    }
                },
                error: function (errormessage) {
                    alertify.error("Error al Guardar Telefono Laboral :  "+errormessage);
                }
            });
    }
    else {
        alertify.error('Debe ingresar el telefono laboral y el comentario de validacion para guardar estos datos.');
    }   
}
function consultaTelefonoLaboral() {
    var empObj = {
        Identificacion: $('#txtCedula').val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/Colocacion/ConsulatTelefonoLaboral",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result != null) {
                    $('#txtTelefonoLaboral').val(result.Telefono);
                    $('#txtTelefonoLaboralComentario').val(result.ComentarioTelefonoLaboral);
                }
            },
            error: function (errormessage) {
                alertify.error(errormessage);
            }
        });
}
//FIN Nueva Version
