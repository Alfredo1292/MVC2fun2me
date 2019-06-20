
$(document).ready(function () {
    $('body').css("background-image", "none");
});
function ActualizarSolicitud(ID_SOLICITUD, ID_Estados) {
    $.ajax(
        {
            url: baseUrl = "ActualizarSolicitud",
            type: "POST",
            data: {
                Solicitud: ID_SOLICITUD,
                Estado: ID_Estados
            },
            success: function (result) {
                $("#spnMensaje").html(result[0].Mensaje);

                if (result[0].Mensaje != 'No se realizo el cambio de estado, el monto es mayor al permitido ')
                {
                    window.location.href = "/SolicitudesVentas/BuscarDocumentos?id=" + ID_SOLICITUD;
                }
                

            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                $("#spnMensajeError").html(result.Mensaje);
            }
        });

 

}

