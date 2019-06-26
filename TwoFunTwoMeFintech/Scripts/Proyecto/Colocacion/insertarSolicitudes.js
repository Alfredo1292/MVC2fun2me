$(document).ready(function () {
    cargarDDLSuborigenes();
});
function cargarDDLSuborigenes() {
    $.ajax({
        url: "/SolicitudesVentas/cargaComboSubOrigenes",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#ddlSubOrigen").html("");
            //$("#ddlSubOrigen").append(new Option("--- Seleccione ---", -1));
            $.each(result, function (key, item) {
                $("#ddlSubOrigen").append(new Option(item.SubOrigen, item.Id));
            });
            cargarComboboxFrecuencia();
            ocultarBloqueo();
        },
        error: function (errormessage) {
            alertify.error("cargarDDLSuborigenes\n" + errormessage.responseText);
        }
    });
}