var v_RegPassword = /(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
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
}