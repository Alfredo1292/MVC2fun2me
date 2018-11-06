var v_RegPassword = /(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
var colorFalse = '#ff9';
var colorTrue = '#fff';



function validPassword(control) {

    if (v_RegPassword.test($(control).val())) {
        control.style.backgroundColor = colorTrue;
    } else { control.style.backgroundColor = colorFalse; }
}
function passEquals() {
    var ret = true;
    if ($("#password").val() == $("#PasswordConfirm").val()) {
        $("#ErrorPass").text("Claves incorrectas, favor verificar");
        ret = false;
    }
    else $("#ErrorPass").text("");
    return ret;
}

function cambiarPass() {

}