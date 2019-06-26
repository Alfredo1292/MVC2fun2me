<<<<<<< HEAD
﻿
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
=======
﻿var v_RegPassword = /(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
var colorFalse = '#ff9';
var colorTrue = '#fff';



function validPassword(control) {

    if (v_RegPassword.test($(control).val())) {
        control.style.backgroundColor = colorTrue;
    } else { control.style.backgroundColor = colorFalse; }
}
function passEquals() {

    if ($("#password").val() == $("#PasswordConfirm").val()) {

    }
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
}