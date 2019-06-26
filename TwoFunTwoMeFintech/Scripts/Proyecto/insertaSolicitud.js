function InsertaSolicitud() {
    var res = validaInsertSolicitud();
    if (res == false) {
        return false;
    }
    var objProcesar = {
        Identificacion: $("#txtIdentificacion").val(),
        Telefono: $("#txtTelefonoIngresoSolicitudes").val()
    };
    $.ajax({
        url: "/SolicitudesVentas/InsertarNuevaSolictitudes",
        data: JSON.stringify(objProcesar),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Status == null || result.Status == 0 || result.Status == 12 || result.Status == 15 || result.Status == 54 || result.Status == 55 || result.Status == 67 || result.Status == 68 || result.Status == 69 || result.Status == 70 || result.Status == 117) {
                //mostramos el panel de rechazo y ocultamos los demas
                $('#lblNombreRechazo').text(result.Nombre);
                $('#lblEstadoSolicitud').text(result.StatusDescripcion);
                $('#solicitudes-panel-3').hide();
                $('#solicitudes-panel-2').hide();
                $('#btnCerrarPanelRechazo').show(); //Boton para cerrar el panel en caso de rechazo
                $('#solicitudes-panel-1').show('slow');
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