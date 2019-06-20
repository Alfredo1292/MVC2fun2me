
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

              window.location.href = "/Login/Login";
         
        },
        error: function (errormessage) {
            return false;
        }
    });

}
function compare() {
    var str1 = $("#password").val();
    var str2 = $("#PasswordConfirm").val();
    var n = str1.localeCompare(str2);
    if (n == -1) {
        $("#lblError").text("las claves no concuerdan");
        $("#ErrorDiv").css({ "display": "inline" });
        $("#ErrorDiv").attr("style", "display: inline;");
        $("#button1id").prop("disabled", true);
    } else {
        $("#ErrorDiv").css({ "display": "none" });
        $("#ErrorDiv").attr("style", "display: none;");
        $("#button1id").prop("disabled", false);
        $("#lblError").text("");
    }
}

function validarPass(object) {
    var data = $(object).val();
    var regex = /(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
    if (regex.test(data)) {
        $("#ErrorDiv").css({ "display": "none" });
        $("#ErrorDiv").attr("style", "display: none;");
        $("#button1id").prop("disabled", false);
        $(object).attr("backgroundColor", "#fff");
        $("#lblError").text("");
        return true;
    }
    else {
        $("#lblError").text("Debe de contener al menos 8 caracteres,  1 n" + String.fromCharCode(250) + "mero, 1 car" + String.fromCharCode(225) + "cter en min" + String.fromCharCode(250) + "scula (az) , 1 car" + String.fromCharCode(225) + "cter en may" + String.fromCharCode(250) + "scula (AZ)");
        $("#ErrorDiv").css({ "display": "inline" });
        $(object).attr("backgroundColor", "#ff9");
        $("#button1id").prop("disabled", true);
        return false;
    }
}