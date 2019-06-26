//Formatear Numeros con decimales y miles(True o False)

function numFormat(valor, dec, miles) {
    var num = valor.toString().replace(/,/gi, ""), signo = 3, expr;
    var cad = "" + valor, ceros = "", pos, pdec, i;

    for (i = 0; i < dec; i++) ceros += '0';
    pos = cad.indexOf('.');
    if (pos < 0) {
        cad = cad + "." + ceros;
    } else {
        pdec = cad.length - pos - 1;
        if (pdec <= dec) {
            for (i = 0; i < (dec - pdec); i++) cad += '0';
        } else {
            num = num * Math.pow(10, dec); num = Math.round(num);
            num = num / Math.pow(10, dec); cad = new String(num);
        }
    }
    pos = cad.indexOf('.');
    if (pos < 0) { pos = cad.length; cad = cad + ".00"; }
    if (cad.substr(0, 1) == '-' || cad.substr(0, 1) == '+') signo = 4;
    if (miles && pos > signo) {
        do {
            expr = /([+-]?\d)(\d{3}[\.\,]\d*)/; cad.match(expr);
            cad = cad.replace(expr, RegExp.$1 + ',' + RegExp.$2);
        } while (cad.indexOf(',') > signo)
    }
    if (dec < 0) cad = cad.replace(/\./, '');

    return cad;
}

var submitFormOkay = false;


function SomeThingChanged() {
    submitFormOkay = true;
}

function SomeThingChangedValue() {
    submitFormOkay = false;
}

$(document).ajaxStart(function () {
    // show loader on start
    $('#ModalBlock').modal("show");
    $('.modal-header h3').html('Favor espere...');
}).ajaxSuccess(function () {
    // hide loader on success
    $('#ModalBlock').modal("hide");
}).ajaxError(function () {
    // hide loader on success
    $('#ModalBlock').modal("hide");
});
//$(window).bind('beforeunload', function (event) {
//	if (!submitFormOkay) {
//		event.preventDefault();
//		// alert("Advertencia: Si usted cierra el  navegador de esta manera,\r\n su sesión quedará activa  y no podrá ingresar al sistema durante un lapso de tiempo");
//		return "Debe cerrar la sesi" + String.fromCharCode(243) + "n del sistema correctamente, sino no podr" + String.fromCharCode(225) + " ingresar durante 20 minutos";
//	}

//});


//INICIO FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
function setPropiedadesCtrls(item) {

    switch (item.Tipo) {
        case 'button':
            setPropiedadButton(item);
            break;
        case 'label':
            setPropiedadLabel(item);
            break;
        case 'text':
            setPropiedadTextBox(item);
            break;
        case 'dropdownlist':
            setPropiedadDropdownlist(item);
            break;
        case 'check':
            setPropiedadCheck(item);
            break;
        default:
        // code block
    }

}

function setPropiedadCheck(item) {
    //Visible
    if (item.Visible == 'false') {
        $("#" + item.CodControl + "").hide();
    }
    else {
        $("#" + item.CodControl + "").show();
    }
    //Editable
    if (item.Editable == 'false') {
        $("#" + item.CodControl + "").prop("disabled", true);
    }
    else {
        $("#" + item.CodControl + "").prop("disabled", false);
    }
}
function setPropiedadLabel(item) {

    //Visible
    if (item.Visible == 'false') {
        $("#" + item.CodControl + "").hide();
    }
    else {
        $("#" + item.CodControl + "").show();
    }
    //Texto
    if ((item.Text != null)) {
        $("#" + item.CodControl + "").html(item.Text);
    }
}

function setPropiedadDropdownlist(item) {

    //Visible
    if (item.Visible == 'false') {
        $("#" + item.CodControl + "").hide();
    }
    else {
        $("#" + item.CodControl + "").show();
    }
    //Editable
    if (item.Editable == 'false') {
        $("#" + item.CodControl + "").prop("disabled", true);
    }
    else {
        $("#" + item.CodControl + "").prop("disabled", false);
    }
}

function setPropiedadButton(item) {
    //Editable
    if (item.Editable == 'false') {
        $("#" + item.CodControl + "").prop("disabled", true);
    }
    else {
        $("#" + item.CodControl + "").prop("disabled", false);
    }
    //Texto
    if ((item.Text != null) || (item.Text != '')) {
        $("#" + item.CodControl + "").prop('value', item.Text);
    }
    //Visible
    if (item.Visible == 'false') {
        $("#" + item.CodControl + "").hide();
    }
    else {
        $("#" + item.CodControl + "").show();
    }
}

function setPropiedadTextBox(item) {
    //Editable
    if (item.Editable == 'false') {
        $("#" + item.CodControl + "").prop("readonly", true);
    }
    else {
        $("#" + item.CodControl + "").prop("readonly", false);
    }
    //Texto
    if (item.Text != null) {
        $("#" + item.CodControl + "").prop('value', item.Text);
    }
    //MaxLength
    if (item.MaxLength != null) {
        $("#" + item.CodControl + "").prop('maxlength', item.MaxLength);
    }
    //Visible
    if (item.Visible == 'false') {
        $("#" + item.CodControl + "").hide();
    }
    else {
        $("#" + item.CodControl + "").show();
    }
    //PlaceHolder
    if (item.PlaceHolder != null) {
        $("#" + item.CodControl + "").prop('value', '');
        $("#" + item.CodControl + "").attr("placeholder", item.PlaceHolder);
    }
}
//FIN FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA



