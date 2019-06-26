//Ini-------------------------------Declaracion de variables Globales--------------------------------//
var valido = 0;

var colorFalse = '#ff9';
var colorTrue = '#fff';

//var v_RegTexto = /^([A-Z][a-z][0-9])*$/;
//var v_RegTexto = /^[-_\w\d\.]*$/i;
//var v_RegTexto = /^[-_\w\d\.(áéíóú)\s(!¡\?¿%\(\)\$\/\*;:,\+><)]*$/i;
var v_RegTexto = /^[-_\w\d\.(áéíóúñÑ)\s(!¡\?¿%\(\)\#-$\/\*;@:,\+)]*$/i;

var v_texCost = /^[A-Z]$/;

var v_RegCedula = /^([1-9]{1}0?[1-9]{3}0?[1-9]{3})$/;
var v_RegCedulaKP = /^[0-9]$/;

var v_RegEstadoCivil = /^[S,s]|[C,c]|[D,d]|[U,u]$/;

var v_RegGenero = /^[M,m]|[F,f]$/;

var v_RegTelefono = /^(\([0-9]{3}\))?[0-9]{4}-?[0-9]{4}$/;
var v_RegTelefonoKP = /^[0-9\-\(\)]$/;

var v_RegCorreo = /[\w-\.]{3,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
var v_RegCorreoKP = /^[-_@\w\d\.]*$/;

//Original--var v_RegPassword = /(?=^.{8,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
//Original--var v_RegPasswordKP = /^[^aeiouAEIOU][A-Z a-z 0-9 ]*$/; 

//debe tener una mayuscul almenos un numero y permite caracteres especiales(@#$%^&*()]*) y entre 8 y 15 caracteres y no permite espacios en blanco
var v_RegPassword = /(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
var v_RegPasswordKP = /^[0-9a-zA-Z!@#$%^&*()]$/;


var v_RegEntero = /^\d+$/;

var v_RegDecimal = /^(\d)?(\d|,)*\.?\d*$/;
var v_RegDecimalKP = /^[0-9\.]$/;

//Expresiones utilizadas para validar decimales negativos
var v_RegDecimalNeg = /^(-{0,1})(\d)?(\d|,)*\.?\d*$/;
var v_RegDecimalKPNeg = /^[-{0,1}0-9\.]$/;


var v_RegFecha = /^(((0[1-9]|[12][0-9]|3[01])([-])(0[13578]|10|12)([-])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-])(0[469]|11)([-])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-])(02)([-])(\d{4}))|((29)(\.|-|\/)(02)([-])([02468][048]00))|((29)([-])(02)([-])([13579][26]00))|((29)([-])(02)([-])([0-9][0-9][0][48]))|((29)([-])(02)([-])([0-9][0-9][2468][048]))|((29)([-])(02)([-])([0-9][0-9][13579][26])))$/;
var v_RegFechaTransac = /^(((0[1-9]|[12][0-9]|3[01])([-\/])(0[13578]|10|12)([-\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-\/])(0[469]|11)([-\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-\/])(02)([-\/])(\d{4}))|((29)(\.|-|\/)(02)([-\/])([02468][048]00))|((29)([-\/])(02)([-\/])([13579][26]00))|((29)([-\/])(02)([-\/])([0-9][0-9][0][48]))|((29)([-\/])(02)([-\/])([0-9][0-9][2468][048]))|((29)([-\/])(02)([-\/])([0-9][0-9][13579][26])))$/;
var v_RegFechaKP = /^[0-9\-]$/;
var v_RegFechaTransacKP = /^[0-9\/]$/;

var v_RegDefault = /^[-_\w\d\.(áéíóú)\s(!¡\?¿%\(\)\$\/\*;:,\+)]*$/i;

/*var v_ConfCuentas = /^([^X]+)|((X)+[-](X)+)+$/;
var v_ConfCuentasKP = /^[^X]+|[-]*$/;*/
var v_ConfCuentas = /^[-_\w\d\.(áéíóú)\s(!¡\?¿%\(\)\$\/\*;:,\+)]*$/i;
var v_ConfCuentasKP = /^[-_\w\d\.(áéíóú)\s(!¡\?¿%\(\)\$\/\*;:,\+)]*$/i;

var v_CtaContNumer = /^([0-9]+[-]?[0-9]+)*$/;
var v_CtaContNumerKP = /^[0-9\-]$/;

var v_EmptyCharsKP = /^$/;

var v_ConfCat = /^([X]+[-]?)*$/
var v_ConfCatKP = /^[X\-]$/;

//End-------------------------------Declaracion de variables Globales--------------------------------//



//Ini-------------------------------Servicios privados del objeto------------------------------------//

//Se encarga de realizar las validaciones de los controles de un tap

function pb_RecorrerForm(tapName) {
    var sAux = "";
    var RegExPattern = "";
    var procesando = "";
    var frm = document.getElementById(tapName);
    var elementos2 = frm.getElementsByTagName("textarea");
    var elementos = frm.getElementsByTagName("Input");
    var msg;

    for (i = 0; i < elementos2.length; i++) {

        sAux = elementos2[i].className;
        // alert(sAux + '  Formulario');
        procesando = sAux.substring(0, 1);
        sAux = sAux.substring(2, sAux.length);
        sAux = sAux.split(' ')[0];

        switch (sAux) {
            case 'Texto':
                RegExPattern = v_RegTexto;
                msg = "Formato incorrecto";
                break;
        } // fin del case
        if (procesando == "1") {//Campo requerido
            if ((elementos2[i].value.match(RegExPattern)) && (elementos2[i].value != '')) {
                elementos2[i].style.backgroundColor = colorTrue;

            } else {
                elementos2[i].style.backgroundColor = colorFalse;

                valido++;
                alertify.warning(msg);

                //alert(msg);

                elementos2[i].value = '';

                break;
            }
        }
        else if (procesando == "2") {//Campo no requerido
            if ((elementos2[i].value.match(RegExPattern)) || (elementos2[i].value == '')) {
                elementos2[i].style.backgroundColor = colorTrue;
            }
            else {
                elementos2[i].style.backgroundColor = colorFalse;
                elementos2[i].value = '';
                valido++;
                alert(msg);

                break;
            }
        }
    }
    sAux = "";
    if (valido > 0) { valido = 0; return false; }
    else { valido = 0; }
    for (i = 0; i < elementos.length; i++) {

        sAux = elementos[i].className;
        procesando = sAux.substring(0, 1);
        sAux = sAux.substring(2, sAux.length);
        sAux = sAux.split(' ')[0];

        switch (sAux) {
            case 'Texto':
                RegExPattern = v_RegTexto;
                msg = "Formato incorrecto";
                break;
            case 'Cedula':
                RegExPattern = v_RegCedula;
                msg = "Formato incorrecto";
                break;
            case 'EstadoCivil':
                RegExPattern = v_RegEstadoCivil;
                msg = "Formato incorrecto de estado civil (Soltero= S ó s,Casado= C ó c, Divorciado= D ó d Unión Libre= U ó u)";
                break;
            case 'Genero':
                RegExPattern = v_RegGenero;
                msg = "Formato incorrecto de género (Masculino= M ó m, Femenino= F ó f)";
                break;
            case 'Telefono':
                RegExPattern = v_RegTelefono;
                msg = "Formato incorrecto";
                break;
            case 'Correo':
                RegExPattern = v_RegCorreo;
                msg = "Formato incorrecto";
                break;
            case 'Password':
                RegExPattern = v_RegPassword;
                msg = "La contraseña debe tener entre 8 y 15 caracteres, debe tener una mayúscula, al menos un número, permite caracteres especiales(@#$%^&*()]*), y no permite espacios en blanco";
                //(Entre 8 y 10 caracteres, por lo menos un digito y un alfanumérico, y no puede contener caracteres espaciales)
                //(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$
                break;
            case 'Entero':
                RegExPattern = v_RegEntero;
                msg = "Formato incorrecto";
                break;
            case 'Decimal':
                RegExPattern = v_RegDecimal;
                msg = "Formato incorrecto";
                break;
            case 'DecimalNeg':
                RegExPattern = v_RegDecimalNeg;
                msg = "Formato incorrecto";
                break;
            case 'Fecha':
                RegExPattern = v_RegFecha;
                msg = "Formato incorrecto de fechas";
                break;
            case 'FechaTra':
                RegExPattern = v_RegFechaTransac;
                msg = "Formato incorrecto de fechas";
                break;
            case 'ConfCuentas':
                RegExPattern = v_ConfCuentas;
                msg = "Formato incorrecto de configuración";
                break;
            case 'CtaCont':
                RegExPattern = v_CtaContNumer;
                msg = "Formato incorrecto de configuración";
                break;
            case 'CfCuentas':
                RegExPattern = v_ConfCat;
                msg = "Formato incorrecto de configuración";
                break;
            default:
                RegExPattern = v_RegDefault;
                msg = "Formato incorrecto";
                break;
        }

        if (procesando == "1") {//Campo requerido 
            if ((elementos[i].value.match(RegExPattern)) && (elementos[i].value != '')) {
                elementos[i].style.backgroundColor = colorTrue;
            } else {
                elementos[i].style.backgroundColor = colorFalse;
                valido++;
                alert(msg);
                elementos[i].value = '';
                break;
            }
        } else if (procesando == "2") {//Campo no requerido
            if ((elementos[i].value.match(RegExPattern)) || (elementos[i].value == '')) {
                elementos[i].style.backgroundColor = colorTrue;
            } else {
                elementos[i].style.backgroundColor = colorFalse;
                valido++;
                alert(msg);
                elementos[i].value = '';
                break;
            }
        }
    }
    sAux = "";
    if (valido > 0) { valido = 0; return false; }
    else { valido = 0; return true; }
}



//Se encarga de realizar las validaciones de los eventos en un control
function pb_ValidaEvento(evento, control) {
    var sAux = "";
    var RegExPattern = "";
    var keynum, keychar = "";

    sAux = control.className;
    procesando = sAux.substring(0, 1);
    sAux = sAux.substring(2, sAux.length);
    sAux = sAux.split(' ')[0];
    switch (sAux) {
        case 'Texto':
            RegExPattern = v_RegTexto;
            break;
        case 'TextoCost':
            RegExPattern = v_texCost;
            break;
        case 'Cedula':
            RegExPattern = v_RegCedulaKP;
            break;
        case 'EstadoCivil':
            RegExPattern = v_RegEstadoCivil;
            break;
        case 'Genero':
            RegExPattern = v_RegGenero;
            break;
        case 'Telefono':
            RegExPattern = v_RegTelefonoKP;
            break;
        case 'Correo':
            RegExPattern = v_RegCorreoKP;
            break;
        case 'Password':

            RegExPattern = v_RegPasswordKP;
            //(Entre 8 y 10 caracteres, por lo menos un digito y un alfanumérico, y no puede contener caracteres espaciales)
            //(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$
            break;
        case 'Entero':
            RegExPattern = v_RegEntero;
            break;
        case 'Decimal':
            RegExPattern = v_RegDecimalKP;
            break;
        case 'DecimalNeg':
            RegExPattern = v_RegDecimalKPNeg;
            break;
        case 'Fecha':
            RegExPattern = v_RegFechaKP;
            break;
        case 'FechaTra':
            RegExPattern = v_RegFechaTransacKP;
            break;
        case 'ConfCuentas':
            RegExPattern = v_ConfCuentasKP;
            break;
        case 'CtaCont':
            RegExPattern = v_CtaContNumerKP;
            break;
        case 'Empty':
            RegExPattern = v_EmptyCharsKP;
            break;
        case 'CfCuentas':
            RegExPattern = v_ConfCatKP;
            break;
        default:
            RegExPattern = v_RegDefault;
            break;
    }

    if (window.event) { keynum = evento.keyCode; }
    else if (evento.which) { keynum = evento.which; }

    if ((keynum == 8) || !keynum) { return true; }

    keychar = String.fromCharCode(keynum);
    sAux = "";

    if (RegExPattern.test(keychar)) {
        control.style.backgroundColor = colorTrue;
        return true;
    }
    else {
        control.style.backgroundColor = colorFalse;
        return false;
    }
}

         //End-------------------------------Servicios privados del objeto------------------------------------//
