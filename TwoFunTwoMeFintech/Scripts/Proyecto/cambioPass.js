
function cambiarPass() {
    //if ((validPassword($("#password"))) && (validPassword($("#PasswordConfirm")))) {
    var empObj = {
        Cod_agente: $('#agentePass').val(),
        Password: $('#password').val(),
        Password2: $('#PasswordConfirm').val(),
    };

    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/Login/CambioPassword",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //}
}