let validacionesFotosCnt = 0;
const fillContractData = (tipoCedula, cedula) => {
    getContractData(tipoCedula, cedula).then((data) => {
        let el = $('#paso9');
        const replacement = utils.parseTemplate(data, el.html());
        el.html(replacement);
        mostrar_paso(9);
        $('#paso8').hide();
        //  document.querySelector('#steps-wrap').innerHTML = components.step9(data);
    }).catch(e => {
        console.error(e);
    });
}

/////////////////////////////////MODELO///////////////////////////////////////////////////////


let credito_aprobado = false; //credito aprobado o rechazado
let paso4Req = {

}
let solicitud_cliente = {

}; //conserva todos los datos de la solicitud de la pagina

let page = 1; //numero de pagina actual

let tipo_cedula_cliente = null;


//FORMULARIO PASO 2 SOLICITUD****************************************************************************

var tipo_ced = null; //tipo de cedula cliente, para el padron

var is_mobile = false; //para cambiar algunas imagenes dependiendo de la resolucion del dispositivo

var datos_cliente = {}; // auxiliar ejemp: {cedula: "207060611", nombre: "JOSE", segnombre: "AHIAS", apellido1: "VARGAS", apellido2: "PACHECO"}

var banco_digitos = 1; //cantidad de digitos de cada banco 

var pin_retornado = null; //pin contra pin

//////////////////////////////////////////////////////////////////////////////////////////////////

var signaturePad = null; //la de la firma;

var monto_nac = 0;
var plazo_nac = 0;
var frecuencia_nac = 0


//determinamos si es un dispositivo movil, si no lo es mostramos el campo de subida de cedula desde almacenamiento

$(document).ready(function () {
    var mobile = (/iphone|ipod|android|blackberry|mini|windows\sce|palm/i.test(navigator.userAgent.toLowerCase()));

    if (mobile) {
        console.log("es un dispositivo movil");

        /*configuramos algunas imagenes por defecto para dispositivos moviles*/

        $("#preview-ced-img-selfie").attr('src', 'assets/img/cedula_selfie_movil.png');

        is_mobile = true;

    }

    else {

        is_mobile = false;

        $("#back-button").attr('src', 'assets/img/back-pc.png');

    }
});

function mostrarConsentimientoInformado() {
    let modal = $('#modal-consentimiento');
    modal.modal();
    let d = $("#cedula_solicitud").data('solicitud');
    if (d) {
        let fecha = new Date();
        let options = { year: 'numeric', month: 'long', day: 'numeric' };
        $('.span-nombre-completo').text(d['nombreCompleto']);
        $('.span-cedula').text(d['cedula']);
        $('#fecha-consentimiento').text(`${fecha.toLocaleDateString("es-ES", options)}.`);
    }
}

//****************************VALIDACIONES*****************************************************

//input cedula solo numeros *CEDULA*

function cedulaSolicitudKeyDown(evt, el) {
    return utils.validateNumberOnType(evt);
}

$('#cedula_solicitud-dimex').on('keydown', function (evt) {

	if (solicitud_cliente.IdTipoIdentificacion == 1) { //cedula nacional, 9 digitos


        var ref = $('#cedula_solicitud-dimex'),
            val = ref.val();
        if (val.length >= 9) {
            ref.val(function () {
                //console.log(val.substr(0, 8))
                return val.substr(0, 8);
            });
        }



	}
	else if (solicitud_cliente.IdTipoIdentificacion == 2) { //extranjera, 12 digitos


        var ref = $('#cedula_solicitud-dimex'),
            val = ref.val();
        if (val.length >= 12) {
            ref.val(function () {
                //console.log(val.substr(0, 11))
                return val.substr(0, 11);
            });
        }


    }


});



//limitamos los caracteres del telefono, esto es necesario ya que el input number no soporta maxlenght y el max no funciona en algunos navegadores

$('#telefono_solicitud').on('keydown', function (evt) {


    var ref = $('#telefono_solicitud'),
        val = ref.val();
    if (val.length >= 8) {
        ref.val(function () {
            //console.log(val.substr(0, 8))
            return val.substr(0, 7);
        });
    }


});


$('#telefono_solicitud-dimex').on('keydown', function (evt) {


    var ref = $('#telefono_solicitud-dimex'),
        val = ref.val();
    if (val.length >= 8) {
        ref.val(function () {
            //console.log(val.substr(0, 8))
            return val.substr(0, 7);
        });
    }


});





//input telefono solo numeros *TELEFONO*



$('#telefono_solicitud').on('keydown', function (evt) {
    var key = evt.charCode || evt.keyCode || 0;

    return (key == 8 ||
        key == 9 ||
        key == 46 ||
        key == 110 ||
        key == 190 ||
        (key >= 35 && key <= 40) ||
        (key >= 48 && key <= 57) ||
        (key >= 96 && key <= 105));
});


$('#telefono_solicitud-dimex').on('keydown', function (evt) {
    var key = evt.charCode || evt.keyCode || 0;

    return (key == 8 ||
        key == 9 ||
        key == 46 ||
        key == 110 ||
        key == 190 ||
        (key >= 35 && key <= 40) ||
        (key >= 48 && key <= 57) ||
        (key >= 96 && key <= 105));
});






//correo electronico valido *CORREO*


function correo_valido() {

    //si 1 es el input nacional de lo contrario dimex

    var email = tipo_ced == 1 ? $('#correo_solicitud').val() : $('#correo_solicitud-dimex').val();

    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

    if (!emailReg.test(email)) {



        $('#correo_solicitud').val('');
        $('#correo_solicitud-dimex').val('');

        return false;


    }

    return true;


}



//input pin modal solo numeros *4*

$('#celu-pin').on('keydown', function (evt) {
    var key = evt.charCode || evt.keyCode || 0;

    return (key == 8 ||
        key == 9 ||
        key == 46 ||
        key == 110 ||
        key == 190 ||
        (key >= 35 && key <= 40) ||
        (key >= 48 && key <= 57) ||
        (key >= 96 && key <= 105));
});



//limitamos los caracteres del pin, esto es necesario ya que el input number no soporta maxlenght y el max no funciona en algunos navegadores

$('#celu-pin').on('keydown', function (evt) {


    var ref = $('#celu-pin'),
        val = ref.val();
    if (val.length >= 4) {
        ref.val(function () {
            //console.log(val.substr(0, 8))
            return val.substr(0, 3);
        });
    }


});



/*

//limitamos la cuenta_cliente a solo digitos

$('#cuenta_cliente').on('keydown', function(evt) {
    var key = evt.charCode || evt.keyCode || 0;

    return (key == 8 ||
            key == 9 ||
            key == 46 ||
            key == 110 ||
            key == 190 ||
            (key >= 35 && key <= 40) ||
            (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105));
});


//BN 15 digitos, BCR 11 digitos, BAC 9 digitos



$('#cuenta_cliente').on('keydown', function(evt) {

    var ref = $('#cuenta_cliente'),
        val = ref.val();
    if ( val.length >= banco_digitos){
        ref.val(function() {
            //console.log(val.substr(0, 8))
            return val.substr(0, banco_digitos-1);       
        });
    }


});

*/



//limitamos el telefono referencia a solo digitos

$('#telefono_referencia_laboral').on('keydown', function (evt) {
    var key = evt.charCode || evt.keyCode || 0;

    return (key == 8 ||
        key == 9 ||
        key == 46 ||
        key == 110 ||
        key == 190 ||
        (key >= 35 && key <= 40) ||
        (key >= 48 && key <= 57) ||
        (key >= 96 && key <= 105));
});


//longitud maxima



$('#telefono_referencia_laboral').on('keydown', function (evt) {

    var ref = $('#telefono_referencia_laboral'),
        val = ref.val();
    if (val.length >= 8) {
        ref.val(function () {
            //console.log(val.substr(0, 8))
            return val.substr(0, 7);
        });
    }


});





function tamanio_permitido(size, max_size) {


    if (size > max_size) { return false; }

    else { return true }


}


//******************************FIN VALIDACIONES***************************************************************




function dimiss_espere(opcion, success, err) {

    try {

        switch (opcion) {

            case 'frontal':

                $('.espere-frontal').hide();


                $('#button-upload-cedula-mobiles').attr("disabled", false);

                $('#button-upload-cedula-mobile').attr("disabled", false);

                success();

                break;

            case 'trasera':

                $('.espere-dorso').hide();

                $('button-upload-cedula-mobiles-dorso').attr("disabled", false);

                $('button-upload-cedula-mobile-dorso').attr("disabled", false);

                success();


                break;

            case 'selfie':

                $('.espere-selfie').hide();

                $('button-upload-cedula-mobiles-selfie').attr("disabled", false);

                $('button-upload-cedula-mobile-selfie').attr("disabled", false);

                success();


                break;

            case 'formulario':

                $('.espere-formulario').hide();

                $('#solicitud #continuar').attr("disabled", false);

                success();


                break;

            case 'formulario-dimex':

                $('.espere-formulario-dimex').hide();

                $('.solicitud-aprobacion-dimex').attr("disabled", false);


                success();


                break;


            case 'firma':

                $('.espere-firma').hide();

                $('#btn-firma-continuar').attr("disabled", false);

                success();


                break;

        }

    } catch (ex) { success(ex); }

}






//para los pasos de subida de cedula (imagenes), presenta un please wait


function porfavor_espere(opcion, hide, disabled, success, err) {


    try {

        switch (opcion) {

            case 'frontal':

                if (hide) {

                    $('.espere-frontal').hide();

                }
                else {

                    $('.espere-dorso').show();

                }

                $('#button-upload-cedula-mobiles').attr("disabled", disabled);

                $('#button-upload-cedula-mobile').attr("disabled", disabled);

                success();

                break;

            case 'trasera':


                if (hide) {

                    $('.espere-dorso').hide();

                }
                else {

                    $('.espere-dorso').show();

                }

                $('button-upload-cedula-mobiles-dorso').attr("disabled", disabled);

                $('button-upload-cedula-mobile-dorso').attr("disabled", disabled);

                success();


                break;

            case 'selfie':


                if (hide) {

                    $('.espere-selfie').hide();

                }
                else {

                    $('.espere-selfie').show();

                }

                $('button-upload-cedula-mobiles-selfie').attr("disabled", disabled);

                $('button-upload-cedula-mobile-selfie').attr("disabled", disabled);

                success();


                break;

            case 'formulario':

                if (hide) {

                    $('.espere-formulario').hide();

                }
                else {

                    $('.espere-formulario').show();

                }

                $('.solicitud-aprobacion-contacto').attr("disabled", disabled);

                success();


                break;


            case 'formulario-dimex':

                if (hide) {

                    $('.espere-formulario-dimex').hide();


                }
                else {


                    $('.espere-formulario-dimex').show();

                }

                $('.solicitud-aprobacion-dimex').attr("disabled", disabled);

                success();


                break;


            case 'firma':

                if (hide) {

                    $('.espere-firma').hide();

                }
                else {

                    $('.espere-firma').show();

                }

                $('#btn-firma-continuar').attr("disabled", disabled);
                success();


                break;

        }


    } catch (ex) { err(ex); }

}



//resetea el paso

function reset_paso(page) {


    switch (page) {

        case 1:

            break;

        case 2: //formulario solicitud


            $('.label-bienvenida-nacional').empty();


            $('.corregir-ced-nacional').hide(); //boton de corregir lo escondemos

            $('.solicitud-cedula-nacional').hide(); //boton de aceptar ced lo escondemos


            $('.label-bienvenida-nacional').append('Hola, necesitamos verificar tu identidad');


            tipo_ced = null;

            datos_cliente = {};


            $('.form-credito-nacional').hide();

            $('.form-credito-nacional-contacto').hide();

            $('.form-credito-nacional').hide();

            $('.form-credito-dimex').hide();

            $('#celu-pin').val('');

            $('#tipo_ident').show();

            break;

        case 3: //video

            $('.rechazado_credito').hide();
            $('.aprobado_credito').show();


            break;

        case 4: //Aprobado o rechazado
            break;

        case 5: //cedula frontal

            $("#preview-ced-img").remove();
            $('#button-upload-cedula-desktop').val('');
            $('#button-upload-cedula-mobile').val('');
            $('#cedula-form').show();

            //agregamos la cedula por defecto dinamicamente

            var responsive = "img-responsive";

            var elid = "preview-ced-img";

            var mystyle = "width:100%; max-height: 200px";

            var dir = "assets/img/cedula_frente.png";

            $('.img-ced-frente').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + dir + '"/>');

            $('#cedula-preview-now').hide();

            break;

        case 6: //cedula dorso
            $("#preview-ced-img-dorso").remove();

            $('#button-upload-cedula-desktop-dorso').val('');

            $('#button-upload-cedula-mobile-dorso').val('');
            $('#cedula-form-dorso').show();

            //agregamos la cedula por defecto dinamicamente

            var responsive = "img-responsive";

            var elid = "preview-ced-img-dorso";

            var mystyle = "width:100%; max-height: 200px";

            var dir = "assets/img/cedula_dorso.png";

            $('.img-ced-dorso').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + dir + '"/>');

            $('#cedula-preview-dorso').hide();

            break;

        case 7: //cedula selfie
            $("#preview-ced-img-selfie").remove();

            $('#button-upload-cedula-desktop-selfie').val('');

            $('#button-upload-cedula-mobile-selfie').val('');
            $('#cedula-form-selfie').show();


            //agregamos la cedula por defecto dinamicamente selfie

            var responsive = "img-responsive";

            var elid = "preview-ced-img-selfie";

            var mystyle = "width:100%; max-height: 200px";

            var dir = is_mobile == true ? "assets/img/cedula_selfie_movil.png" : "assets/img/cedula_selfie.png";

            $('.img-ced-selfie').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + dir + '"/>');


            $('#cedula-preview-selfie').hide();

            break;

        case 8: //cuenta banco
            break;

        case 9:  //terminos
            break;

        case 10: //firma
            break;

        case 11: //referencia
            break;


    }



}






//utilizada para mostrar un paso y ocultar los demas 

function mostrar_paso(paso) {
    appStore.set('last-step', paso - 1);
    /*reseteo de pasos boton back*/

    if (paso == 1) { //escondemos el boton back    

        $('.back_button').hide();

        reset_paso(1);
        reset_paso(2);

        $('#paso' + 2).hide(); //ocultamos el paso actual

        $('#tipo_ident').hide(); //escondemos el escoger tipo de cedula

    }


    else {

        $('#video').trigger('load'); //load iniciando de nuevo
        $('#video').trigger('pause'); //pause el video


        $('.back_button').show();

        reset_paso(paso); //reseteamos el paso actual

        reset_paso(paso + 1); //reseteamos el paso anterior

        $('#paso' + (paso + 1)).hide(); //ocultamos el paso actual

    }

    //*********************mostramos pasos*****************


    for (var i = 1; i <= 12; i++) {
        if (paso == i) {
            $('#paso' + i).show(); $('#ex23').slider('refresh'); $('#ex21').slider('refresh'); $('#ex24').slider('refresh'); $('#ex28').slider('refresh');
            page = i; //pagina actual
            if (paso == 10) {//paso especial de firma, debemos configurarlo graficamente cuando esta visible
                inicializar_firma();
                window.addEventListener('resize', inicializar_firma); //necesario para adaptar la firma a cualquier pantalla

            }

            if (paso == 3) { //video validamos el pin
                let solicitud_validar_pin = {
                    IdTipoIdentificacion: tipo_ced,
                    Identificacion: solicitud_cliente.cedula
                };
                validar_pin(solicitud_validar_pin, function (result) {
                    let status = -1;
                    if (Array.isArray(result)) {
                        status = result[0].Status;
                    } else {
                        status = result.Status;
                    }
                    //determinamos si la solicitud es aprobada o rechazada
                    // status 12 para desarrollo
                    if ([39, 12].includes(status)) { //solicitud aprobada 39
						credito_aprobado = true;
						solicitud_cliente.IdTipoIdentificacion = tipo_ced;
                        logPaso3(tipo_ced, solicitud_cliente.cedula);
                    }
                    else {
                        credito_aprobado = false;
                    }

                }, function (err) {
                    swal({
                        type: 'error',
                        title: 'Oops...',
                        text: err
                    });
                });

            }

            break;
        }

        else {

            $('#paso' + i).hide();

        }

    }


}



//muestra campos del paso de la cedula

function cedula_mostrar_preview() {



    $("#preview-ced-img").remove();

    $('#button-upload-cedula-desktop').val('');

    $('#button-upload-cedula-mobile').val('');


}


//muestra campos del paso de la cedula dorso

function cedula_mostrar_preview_dorso() {


    $("#preview-ced-img-dorso").remove();

    $('#button-upload-cedula-desktop-dorso').val('');

    $('#button-upload-cedula-mobile-dorso').val('');


}



//muestra campos del paso de la cedula selfie

function cedula_mostrar_preview_selfie() {


    $("#preview-ced-img-selfie").remove();

    $('#button-upload-cedula-desktop-selfie').val('');

    $('#button-upload-cedula-mobile-selfie').val('');


}

//carga la imagen y la setea en el html

function cargar_imagen(tipo, input, success, err) {


    try {

        console.log(input.files[0]);

        if (input.files && input.files[0]) {
            var reader = new FileReader();



            reader.onload = function (e) {

                var responsive = "img-responsive";

                var mystyle = "width:100%; max-height: 200px;";

                var elid = "";


                switch (tipo) {

                    case 'cedula':
                        elid = "preview-ced-img";
                        $("#preview-ced-img").remove();
                        $('.img-ced-frente').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + e.target.result + '"/>');
                        success(input.files[0]);

                        break;

                    case 'cedula-dorso':
                        elid = "preview-ced-img-dorso";
                        $("#preview-ced-img-dorso").remove();
                        //cedula_mostrar_preview_dorso();
                        $('.img-ced-dorso').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + e.target.result + '"/>');
                        success(input.files[0]);
                        break;

                    case 'selfie':
                        elid = "preview-ced-img-selfie";
                        $("#preview-ced-img-selfie").remove();
                        //cedula_mostrar_preview_selfie();
                        $('.img-ced-selfie').append('<img id="' + elid + '" class="' + responsive + '" style="' + mystyle + '" src="' + e.target.result + '"/>');
                        success(input.files[0]);
                        break;
                }

            }

            reader.readAsDataURL(input.files[0]);
        }
        else {

            err();

        }

    } catch (ex) { err(); console.log(ex); }


}


//vamos a la pagina anterior

$('.paso-anterior').on('click', function () {

    if (!credito_aprobado && page == 4) { page = 3; $('#paso3').hide(); $('#paso4').hide(); }

    mostrar_paso((page - 1));

});





//*******************************PASO 1 CALCULADORA****************************************************



//obtiene los datos del formulario de calculadora (sus ids para generar el codigo de producto)

function obtener_calcu_credito() {

    // 4 es el id del monto 150 para generar el id del producto, conservamos el monto frecuencia y plazo para generar el id del producto


    solicitud_cliente.monto = $(".monto_credit").val() == 3 ? 4 : $(".monto_credit").val();

    solicitud_cliente.frecuencia = $(".frecuencia").val();

    solicitud_cliente.plazo = $(".plazo").val(); //plazo de 3 meses o menos


    monto_nac = solicitud_cliente.monto;
    plazo_nac = solicitud_cliente.plazo;
    frecuencia_nac = solicitud_cliente.frecuencia;

    obtenerIDProducto(solicitud_cliente.monto, solicitud_cliente.plazo, solicitud_cliente.frecuencia, (val => {
        appStore.set("idProducto", val);
    }), (e => {
        throw e;
    }))



    if ($(".monto_credit").val() == 3) {// se esta mostrando el plazo de 5 meses o menos

        solicitud_cliente.plazo = $(".plazo5meses").val(); //agarramos el dato de plazo del input range de 5 meses

        plazo_nac = solicitud_cliente.plazo;
    }

    //-------------------------------------INICIO TRACE EVENTOS
    logEvent({
        traceId: appStore.get('uId'),
        Paso: '0',
        Evento: 'BtnSolicitar_Click',
        Data: '{ "monto": "' + solicitud_cliente.monto + '", "frecuencia": "' + solicitud_cliente.frecuencia + '", "plazo": "' + solicitud_cliente.plazo+'" }'
    }).then(() => {
            console.log('evento logeado');
        }).catch(e => {
            throw e;
        })
     //-------------------------------------FIN TRACE EVENTOS

}


const solicitarBtnClick = () => {
    logPaso0().then().catch(e => {
        throw e;
    });
    obtener_calcu_credito(); //obtenemos los datos del formulario de calcu (paso 1)

    mostrar_paso(2);

    $("#tipo_ident").show();
    // window.history.pushState("object or string", "paso0", "paos-identificacion");

    //document.querySelector("#steps-wrap").innerHTML = components.step2();

}


//****************************************FIN DEL PASO 1*************************************************
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
/**************************************PASO 2 FORMULARIO**************************************************************************/


//para corregir la cedula nacional


$('.corregir-ced-nacional').on('click', function () {

    $('.solicitud-cedula-nacional').hide(); //escondemos el boton de nacional

    $('.corregir-ced-nacional').hide(); //escondemos el boton de corregir

    $("#cedula_solicitud").val('');

    $('.label-bienvenida-nacional').append('Hola, requerimos verificar tu identidad');


});

//obtenemos los datos del padron del usuario, el nombre debe ir en el modal, cedula nacional

function cedulaNacionalSolicitudKeyup(evt) {
    let tipo = tipo_ced;

    let cedula = $("#cedula_solicitud").val(); //nacional

    if ((cedula != '' || cedula != 0) && cedula.length === 9) { //nacional

        if (tipo == 1) {
            $.ajax({
                type: "POST",
                url: `${apiUrl}/ConsultaPadron`,
                data: { cedula },
                dataType: 'json',
                error: (err) => {
                    console.error(err);
                }
            }).done(function (res) {
                let data = res[0];
                datos_cliente = data; 
                //aqui podemos establecer la solicitud en el appStore
                if (data.nombre == '') {
                    $('#cedula_solicitud').val('');
                    $('.label-bienvenida-nacional').empty();


                    $('.label-bienvenida-nacional').append('Cedula inválida!');

                    //alert('Digite una cédula valida');                    


                    return false;
                }


                if (data.nombre != '') {
                    data.nombreCompleto = `${data['nombre']} ${data['segundoNombre']} ${data['primerApellido']} ${data['segundoApellido']}`;
                   appStore.set('solicitud',data);
                    $('.label-bienvenida-nacional').empty();

                    let bienvenido = data['nombreCompleto'].toLowerCase() ;
                    $("#cedula_solicitud").data('solicitud', data);
                    //appStore.set('solicitud', data);

                    $('.label-bienvenida-nacional').append('Hola <span style="color:yellow; font-size:1.1em">' + ' ' + bienvenido + '</span>' + '<br /> ¿Eres tú?');

                    $('.corregir-ced-nacional').show(); //boton de corregir cedula nacional

                    $('.solicitud-cedula-nacional').show(); //boton aceptar lo mostramos


                    // datos_cliente = data; //conservamos los datos del cliente
                    $("#modal-verificacion-text").empty();


                    //****************************************************

                    $('#nombre').val(data.nombre);
                    $('#nombre').prop('readonly', true);
                }
                if (data.segnombre != '') {
                    $('#nombre2').val(data.segnombre);
                    $('#nombre2').prop('readonly', true);
                } else {
                    $('#nombre2').prop('readonly', true);
                }
                if (data.apellido1 != '') {
                    $('#apellido').val(data.apellido1);
                    $('#apellido').prop('readonly', true);
                }
                if (data.apellido2 != '') {
                    $('#apellido2').val(data.apellido2);
                    $('#apellido2').prop('readonly', true);
                } else {
                    $('#apellido2').prop('readonly', true);
                }
            });
        }
    } else {
        $("#cedula_solicitud").data('solicitud', '');
        $('.label-bienvenida-nacional').empty();
        $('.label-bienvenida-nacional').append('Hola, requerimos verificar tu identidad');

        $('.solicitud-cedula-nacional').hide();
        $('.corregir-ced-nacional').hide();
        $('#nombre').val('');
        $('#nombre2').val('');
        $('#apellido').val('');
        $('#apellido2').val('');
        $('#nombre').prop('readonly', false);
        $('#nombre2').prop('readonly', false);
        $('#apellido').prop('readonly', false);
        $('#apellido2').prop('readonly', false);
    }

}


//tipo de cedula para nacional

function btnCedulaNacionalClick() {
    tipo_ced = 1;


    $('#tipo_ident').hide();


    $('.form-credito-nacional').show();

    $('.form-credito-dimex').hide();


    //$('#cedula_solicitud').prop("maxlength", 9); //maximo 9 caracteres cedula nacional


    $('#cedula_solicitud').val(''); //resetamos
}


//tipo de cedula para dimex

function bntCedulaDimexClick() {
    tipo_ced = 2;

    //focus

    //$(this).css("border", "3px solid red");

    //$('.ced-nacional').css("border", "");


    $('#tipo_ident').hide();


    $('.form-credito-dimex').show();

    $('.form-credito-nacional').hide();


    $('#cedula_solicitud-dimex').prop("maxlength", 12); //maximo 12 caracteres cedula nacional


    $('#cedula_solicitud').val(''); //resetamos

}


//le dio siguiente cuando estaba en la pantalla de verificacion cedula nacional
const solicitudCedulaNacionalClick = () => {

    var cedula = $('#cedula_solicitud').val();
    var nombre = datos_cliente.nombre;

    var segnombre = datos_cliente.segnombre;

    var apellido1 = datos_cliente.apellido1;

    var apellido2 = datos_cliente.apellido2;
    const s = appStore.get('solicitud');
    if (s && s['Identificacion'] === cedula) {
        console.log('el usuario regresó');
    } else {
        console.log('Es una cedula nueva');
    }

    solicitud_cliente.Identificacion = cedula;
    //solicitud_cliente.IdTipoIdentificacion= 

    //mostramos el formulario de contacto

    if ((cedula.length != 9)) {
        alert("Tu cédula debe tener 9 caracteres"); return false;
    }

    if (nombre && cedula) {

        //conservamos los datos para ir armando la solicitud

        solicitud_cliente = {
            cedula: cedula,
            nombre: nombre,
            segnombre: segnombre,
            apellido1: apellido1,
            apellido2: apellido2,
            id_monto: monto_nac,
            id_plazo: plazo_nac,
            id_frecuencia: frecuencia_nac
        };

        $('.form-credito-nacional').hide();

        $('.form-credito-nacional-contacto').show();

        logPasoCedula(cedula).then(result => {
            console.log('paso cedula logeado');
        }).catch(e => {
            throw e;
        })

    }
    else {

        alert("Necesitamos una cédula válida!");

    }

}



//aprobamos desde el formulario de contacto nacional


function solicitudAprobacionContactoClick(btn) {


    var telefono = $('#telefono_solicitud').val();

    var correo = $('#correo_solicitud').val();

    var nombre = solicitud_cliente.nombre.substr(0, 1).toUpperCase() + (solicitud_cliente.nombre.substr(1, solicitud_cliente.nombre.length)).toLowerCase();


    //validaciones

    if (!telefono) { alert("El campo teléfono está vacío"); return false; }

    if (telefono.substr(0, 1) < 6 || telefono.length != 8) { alert("Tu celular es inválido, solamente Kolbi, Claro, Movistar"); return false; }

    if (!correo) { alert("El campo correo es requerido"); return false; }

    if (!correo_valido()) { alert("Tu correo no es válido"); return false; }


    //seteamos el nombre de la persona en todos los apartados
    $('#modal-verificacion-text').empty();
    $("#modal-verificacion-text").append((nombre) + " digita el pin enviado a tu celular");

    $('.label-ced-frontal').empty();
    $(".label-ced-frontal").append((nombre) + " necesitamos una fotografía de tu cédula por el frente");

    $('.label-ced-dorso').empty();
    $(".label-ced-dorso").append((nombre) + " ahora necesitamos una fotografía de tu cédula por el dorso");

    $('.label-ced-selfie').empty();
    $(".label-ced-selfie").append((nombre) + " ya casi estamos! , envianos una foto tuya como el ejemplo (selfie):");

    $('.label-cuenta').empty();
    $(".label-cuenta").append((nombre) + " indicanos en qué cuenta te tenemos que depositar. Recordá que tenés que ser el titular, no depositamos en cuentas de otras personas.");

    $('.label-firma').empty();
    $(".label-firma").append((nombre) + " por favor ingresa tu firma como aparece en tu cédula.");

    $('.label-referencia').empty();
    $(".label-referencia").append((nombre) + " para finalizar necesitamos una referencia laboral de tu trabajo.");


    $('.txt_aprobado_gracias').empty();
    $('.txt_aprobado_gracias').append('Felicidades ' + nombre + ' tu crédito ha sido aprobado! <br/> A continuación te pediremos unos datos adicionales para enviar tu dinero');

    //conservamos los datos para ir armando la solicitud

    solicitud_cliente.correo = correo;
    solicitud_cliente.telefono = telefono;

    //armamos los datos que seran enviados a guardar_persona/ metodos_api.js  

    let solicitud_envio = {
        id_monto: monto_nac,
        id_plazo: plazo_nac,
        id_frecuencia: frecuencia_nac,
        IdTipoIdentificacion: tipo_ced,
        Identificacion: solicitud_cliente.cedula,
        PrimerNombre: solicitud_cliente.nombre,
        SegundoNombre: solicitud_cliente.segnombre,
        PrimerApellido: solicitud_cliente.apellido1,
        SegundoApellido: solicitud_cliente.apellido2,
        TelefonoCel: solicitud_cliente.telefono,
        Correo: solicitud_cliente.correo,
        UsrModifica: 'WEB'/*, Origen: ''*/
    };


	console.log(solicitud_envio);
	appStore.merge('solicitud', solicitud_envio)



    //mostramos el pleasewait y deshabilitamos

    porfavor_espere('formulario', false, true, function (success) {
        $(btn).attr("disabled", true);
        guardar_cliente(solicitud_envio, function (success) {
            //si todo sale bien y se guardaron los datos del cliente. genera el modal del pin

            var solicitud_validar_pin = { IdTipoIdentificacion: tipo_ced, Identificacion: solicitud_cliente.cedula };

            validar_pin(solicitud_validar_pin, function (success) {

                //determinamos si la solicitud es aprobada o rechazada
                //debugger;
                if (Array.isArray(success)) {

                    credito_aprobado = true;

                    pin_retornado = success[0].PIN;

                    dimiss_espere('formulario', function () {

                        //si todo salio bien muestra el modal de pin de verificacion

                        //boton de reenvio de nuevo pin

                        $('.modal-pin-reenviar').hide();

                        //en 15 segundos muestra el boton de reenvio de pin

                        setTimeout(function () {
                            $('.modal-pin-reenviar').show();
                        }, 15000);


                        $('#modal-pin').modal({ backdrop: 'static', keyboard: false });


                        $('#modal-pin').modal('show');

                    },

                        function () { alert("No se mostro el loading"); });


                }

                else {

                    dimiss_espere('formulario', function () {


                        $('.aprobado_credito').hide();

                        $('.rechazado_credito').show();

                        credito_aprobado = false;

                        mostrar_paso(4);
                        paso4Req.Credito_Aprobado = credito_aprobado;
                        logPaso4(paso4Req);

                        //alert("La solicitud ha sido rechazada");
                    }, function () { alert("No se mostro el loading") });



                }

            }, function (err) {
                $(btn).attr("disabled", false);
                dimiss_espere('formulario', function () {
                    alert(err);
                }, function () { alert("No se mostro el loading"); });


            });


        }, function (err) {

            dimiss_espere('formulario', function () {
                alert(err);
            }, function () { alert("No se mostro el loading"); });



        })//, solicitud.SegundoNombre, solicitud.PrimerApellido, solicitud.SegundoApellido, solicitud.TelefonoCel, solicitud.Correo, solicitud.IdProducto, UsrModifica: WEB, Origen="" 



    }, function (err) {

        dimiss_espere('formulario', function () {
            alert(err);
        }, function () { alert("No se mostro el loading"); });

    });


}



//aprobamos desde el formulario de DIMEX

$('.solicitud-aprobacion-dimex').on('click', function () {



    //conservamos los datos del cliente**************************

    var cedula = $('#cedula_solicitud-dimex').val();

    var telefono = $('#telefono_solicitud-dimex').val();

    var correo = $('#correo_solicitud-dimex').val();

    var nombre = $('#nombre-dimex').val();

    var segnombre = $('#nombre2-dimex').val();

    var apellido1 = $('#apellido-dimex').val();

    var apellido2 = $('#apellido2-dimex').val();


    if (nombre) {


        nombre = nombre.substr(0, 1).toUpperCase() + (nombre.substr(1, nombre.length)).toLowerCase();

        //seteamos el nombre de la persona en todos los apartados
        $('#modal-verificacion-text').empty();
        $("#modal-verificacion-text").append((nombre) + " digita el pin enviado a tu celular");

        $('.label-ced-frontal').empty();
        $(".label-ced-frontal").append((nombre) + " necesitamos una fotografía de tu cédula por el frente");

        $('.label-ced-dorso').empty();
        $(".label-ced-dorso").append((nombre) + " ahora necesitamos una fotografía de tu cédula por el dorso");

        $('.label-ced-selfie').empty();
        $(".label-ced-selfie").append((nombre) + " ya casi estamos! , envianos una foto tuya como el ejemplo (selfie):");

        $('.label-cuenta').empty();
        $(".label-cuenta").append((nombre) + " indicanos en qué cuenta te tenemos que depositar. Recordá que tenés que ser el titular, no depositamos en cuentas de otras personas.");

        $('.label-firma').empty();
        $(".label-firma").append((nombre) + " por favor ingresa tu firma como aparece en tu cédula.");

        $('.label-referencia').empty();
        $(".label-referencia").append((nombre) + " para finalizar necesitamos una referencia laboral de tu trabajo.");


        $('.txt_aprobado_gracias').empty();
        $('.txt_aprobado_gracias').append('Felicidades ' + nombre + ' tu crédito ha sido aprobado! <br/> A continuación te pediremos unos datos adicionales para enviar tu dinero');


    }

    //conservamos los datos de la solicitud

    solicitud_cliente = { cedula: cedula, telefono: telefono, correo: correo, nombre: nombre, segnombre: segnombre, apellido1: apellido1, apellido2: apellido2, id_monto: monto_nac, id_plazo: plazo_nac, id_frecuencia: frecuencia_nac };

    //validaciones

    if (!cedula) { alert("El campo cédula está vacío"); return false; }

    if (!telefono) { alert("El campo teléfono está vacío"); return false; }

    if (telefono.substr(0, 1) < 6 || telefono.length != 8) { alert("Tu celular es inválido, solamente Kolbi, Claro, Movistar"); return false; }

    // cedula nacional invalida

    if ((nombre == '' || nombre == null) && tipo_ced != 2) { alert("Se requiere una cedula válida"); return false; }

    //nombre dimex invalido

    if ((nombre == '' || nombre == null) && tipo_ced != 1) { alert("El campo nombre no puede estar vacío"); return false; }


    if ((cedula.length != 9 && tipo_ced == 1)) { alert("La cedula nacional debe tener 9 caracteres"); return false; }



    if ((cedula.length != 12 && tipo_ced == 2)) { alert("La cedula dimex debe tener 12 caracteres"); return false; }


    if ((apellido1 == '' || apellido1 == null) && tipo_ced != 1) { alert("El campo primer apellido no puede estar vacío"); return false; }


    if (!correo) { alert("El campo correo es requerido"); return false; }


    if (!correo_valido()) { alert("El correo no es válido"); return false; }



    //armamos los datos que seran enviados a guardar_persona/ metodos_api.js  

    var solicitud_envio = { id_monto: monto_nac, id_plazo: plazo_nac, id_frecuencia: frecuencia_nac, IdTipoIdentificacion: tipo_ced, Identificacion: solicitud_cliente.cedula, PrimerNombre: solicitud_cliente.nombre, SegundoNombre: solicitud_cliente.segnombre, PrimerApellido: solicitud_cliente.apellido1, SegundoApellido: solicitud_cliente.apellido2, TelefonoCel: solicitud_cliente.telefono, Correo: solicitud_cliente.correo, UsrModifica: 'WEB', Origen: '' };
    console.log('La solicitud de envio aqui es');
    console.log(solicitud_envio)
    //, solicitud.SegundoNombre, solicitud.PrimerApellido, solicitud.SegundoApellido, solicitud.TelefonoCel, solicitud.Correo, solicitud.IdProducto, UsrModifica: WEB, Origen="" 

    //mostramos el cargando

    porfavor_espere('formulario-dimex', false, true, function (success) {

        guardar_cliente(solicitud_envio, function (success) {

            //si todo sale bien y se guardaron los datos del cliente. genera el modal del pin

            var solicitud_validar_pin = {
                IdTipoIdentificacion: tipo_ced,
                Identificacion: solicitud_cliente.cedula
            };

            validar_pin(solicitud_validar_pin, function (success) {


                if (Array.isArray(success)) {

                    pin_retornado = success[0].PIN;

                    credito_aprobado = true;

                    dimiss_espere('formulario-dimex', function () {

                        //boton de reenvio de nuevo pin

                        $('.modal-pin-reenviar').hide();

                        //en 15 segundos muestra el boton de reenvio de pin

                        setTimeout(function () {
                            $('.modal-pin-reenviar').show();
                        }, 30000);


                        //si todo salio bien muestra el modal de pin de verificacion

                        $('#modal-pin').modal({ backdrop: 'static', keyboard: false });

                        $('#modal-pin').modal('show');


                    }, function () { alert("No se mostro el loading"); });




                }
                else { //ha sido rechazado

                    dimiss_espere('formulario-dimex', function () {

                        $('.aprobado_credito').hide();

                        $('.rechazado_credito').show();

                        credito_aprobado = false;

                        mostrar_paso(4);
                        paso4Req.Credito_Aprobado = credito_aprobado;
                        logPaso4(paso4Req);

                        //alert("La solicitud ha sido rechazada");


                    }, function () {

                        alert("No se mostro el loading");

                    });


                }

            }, function (err) {


                dimiss_espere('formulario-dimex', function () {

                    alert(err);


                }, function () {

                    alert("No se mostro el loading");

                });



            });

        }, function (err) {

            dimiss_espere('formulario-dimex', function () {

                alert(err);


            }, function () {

                alert("No se mostro el loading");

            });


        })



    }, function (err) {


        dimiss_espere('formulario-dimex', function () {

            alert(err);


        }, function () {

            alert("No se mostro el loading");

        });



    });


});

//muestra el video con un timeout de 35 segundos para luego mostrar el boton continuar

function show_continuar_gif() {

    $('#video').trigger('play'); //le damos play al video
    // $('#video').webkitExitFullscreen(); //salimos del fullscreen 
    // $('#video').exitFullscreen(); //salimos del fullscreen

    $('#video').bind('ended', function () { //cuando llega al final se desencadena este evento... si el credito fue aprobado pasamos al paso 4

        // $(this).webkitExitFullscreen();  
        // $(this).exitFullscreen();
        paso4Req.Credito_Aprobado = credito_aprobado;
        logPaso4(paso4Req);

        if (credito_aprobado) {

            mostrar_paso(4);


        }
        else {
            //no fue aprobado el credito 

            $('.aprobado_credito').hide();

            $('.rechazado_credito').show();

            mostrar_paso(4);
        }

    });

}

function verificarPin() {
    return $("#celu-pin").val() != null && $("#celu-pin").val() == pin_retornado;
}

function modalPinReenviarClick() {
    $("#celu-pin").val();

    let solicitud_reenvio = {
        IdTipoIdentificacion: tipo_ced,
        Identificacion: solicitud_cliente.cedula
    };

    /*si se reenvia el nuevo pin, no deberia venir con uno nuevo*/

    generar_pin(solicitud_reenvio, function (success) {
        pin_retornado = success[0].PIN;
        alert("El pin ha sido reenviado. Espera ha que llegue a tu celular y digitalo nuevamente");
        $('.modal-pin-reenviar').hide();
    }, function (err) {
        swal(message.swalErrorTitle, err, "error");
    }); 

}

//modal de digite su pin, verificamos que sea el correcto

const modalPinSiguienteClick = (btn) => {

    var pin = $("#celu-pin").val();
    /*VERIFICAR EL ENVIO DE PIN EN EL SERVIDOR con un WS*/

    if (pin && pin.length == 4) {

        //si el pin es valido
        if (verificarPin()) {
            logPinVerificado().then().catch(e => {
                throw e;
            });
            $('#modal-pin').modal('hide'); //escondemos el modal

            mostrar_paso(3); //video

            show_continuar_gif(); //mostramos el boton continuar despues de 45 segundos (para que el cliente se tire el gif)

        } else {
            logPinFallido().then().catch(e => {
                throw e;
            });
            alert(message.pinFallido);
        }

    } else { alert("Digita el pin enviado a tu celular de 4 digitos"); }
}


//****************************************FIN DEL PASO 2****************************************************
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//***************************************PASO 3 PANTALLA DE VIDEO**********************************************************



$(".btn-continuar-gif").on('click', function () { //boton para ir al paso 4
    paso4Req.Credito_Aprobado = credito_aprobado;
    logPaso4(paso4Req);
    if (credito_aprobado) {

        mostrar_paso(4);

    }

    else {

        alert("Lo sentimos tu crédito no fue aprobado");

    }

    $('#ex23').slider('refresh');
    $('#ex21').slider('refresh');
    $('#ex24').slider('refresh');
    $('#ex28').slider('refresh');

});


//*************************************FIN DEL PASO 3*********************************************************************
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//*****************************************PASO 4 CREDITO APROBADO O RECHAZADO**************************


$('.aprobado_credito').on('click', function () {//boton credito aprobado

    mostrar_paso(5);


})





//***************************************FIN DEL PASO 4**************************************************

//****************************************PASO 5 SUBIDA DE CEDULA****************************************

//boton de subir archivo de forma de captura de camara CEDULA FRONTAL (disponible solo para pc y mobile)

$('#button-upload-cedula-mobile').bind('change', function (e) { //dynamic property binding

    //muestra el espere;

    $('.espere-frontal').show();
    $('#button-upload-cedula-mobiles').attr("disabled", true);

    $('#button-upload-cedula-mobile').attr("disabled", true);

    cargar_imagen('cedula', this, function (datafile) {



        enviar_cedulaFrontal(datafile, tipo_ced, solicitud_cliente.cedula, function (data) {


            solicitud_cliente.cedula_frontal = data; //conservamos la cedula file para despues guardarla  

            dimiss_espere('frontal', function () {
                mostrar_paso(6);
            }, function () {
                swal({
                    icon: 'error',
                    title: 'Oops...',
                    text: "No se mostro el loading"
                });

            });

        }, function (err) {
            dimiss_espere('frontal', function () {
                swal({
                    icon: 'error',
                    title: 'Oops...',
                    text: err
                });
            }, function () {
                swal({
                    icon: 'error',
                    title: 'Oops...',
                    text: "No se mostro el loading"
                });
            });
        })



    }, function () {

        dimiss_espere('frontal', function () {
            alert("la imagen no es válida");
        }, function () { alert("No se mostro el loading"); });


    });


});





$('.cedula-aceptar').on('click', function () {//paso 5 continuar al 6 cedula trasera


    mostrar_paso(6);

});


//******************************************FIN DE PASO 5**********************************************
//************************************************************************************************
//******************************************PASO 6 CEDULA TRASERA*********************************************


$('.cedula-aceptar-dorso').on('click', function () {//paso 6 continuar al paso 7

    mostrar_paso(7);

});



//boton de subir archivo de forma de captura de camara CEDULA TRASERA (disponible solo para pc y mobile)

$('#button-upload-cedula-mobile-dorso').bind('change', function (e) { //dynamic property binding


    $('.espere-dorso').show();
    $('button-upload-cedula-mobiles-dorso').attr("disabled", true);
    $('button-upload-cedula-mobile-dorso').attr("disabled", true);


    cargar_imagen('cedula-dorso', this, function (datafile) {


        enviar_cedulaDorso(datafile, tipo_ced, solicitud_cliente.cedula, function (data) {


            solicitud_cliente.cedula_dorso = data; //conservamos la cedula file para despues guardarla  


            dimiss_espere('trasera', function () {
                mostrar_paso(7);
            }, function () { alert("No se mostro el loading"); });

        }, function (err) {

            dimiss_espere('trasera', function () {
                alert(err);
            }, function () { alert("No se mostro el loading"); });



        })


    }, function () {


        dimiss_espere('trasera', function () {
            alert("la imagen no es válida");

        }, function () { alert("No se mostro el loading"); });


    });


});



//******************************************FIN DE PASO 6**********************************************
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//****************************************PASO 7 CEDULA SELFIE******************************************

//paso 7 con camara la subida de cedula para mobile


$('#button-upload-cedula-mobile-selfie').bind('change', function (e) { //dynamic property binding

    //mostramos el loading

    $('.espere-selfie').show();
    $('button-upload-cedula-mobiles-selfie').attr("disabled", true)

    $('button-upload-cedula-mobile-selfie').attr("disabled", true);


    cargar_imagen('selfie', this, function (datafile) {

        enviar_cedulaSelfie(datafile, tipo_ced, solicitud_cliente.cedula, function (data) {

            var solicitud_selfie = {
                IdTipoIdentificacion: tipo_ced,
                Identificacion: solicitud_cliente.cedula
            };

            solicitud_cliente.cedula_selfie = data; //conservamos la cedula file para despues guardarla  


            CompararImagenes(solicitud_selfie, function (result) {

                //comparamos rostros

                if (result.PorcentMatched >= 50 && result.UnMatchedFace != null) { //rostros iguales

                    dimiss_espere('selfie', function () {
                        mostrar_paso(8);


                    }, function () { alert("No se mostro el loading"); });


                }
                else {
                    validacionesFotosCnt++;
                    dimiss_espere('selfie', function () {
                        if (validacionesFotosCnt === 2) {
                            mostrar_paso(8);
                        } else {
                            //alert("Tú rostro no se parece al de tu cédula, por favor toma otra foto");
                            swal({
                                title: "Imagenes no se parecen",
                                text: "Tu foto no se parece a la de la cedula, intentalo de nuevo",
                                icon: "warning",
                                //buttons: true,
                                dangerMode: true,
                                timer: 8000,
                            }).then(() => {
                                console.log("Intentar por segunda vez la carga de las imagenes");
                                $("#paso7").hide();
                                mostrar_paso(5);
                            });
                        }

                    }, function () {
                        swal({
                            type: 'error',
                            title: 'Oops...',
                            text: "No se mostro el loading",
                            timer: 3000,
                            // showConfirmButton: false
                        });

                    });

                }


            }, function (err) {

                dimiss_espere('selfie', function () {
                    alert(err);

                }, function () {
                    //alert("No se mostro el loading"); 
                    swal({
                        type: 'error',
                        title: 'Oops...',
                        text: "No se mostro el loading",
                        timer: 3000,
                        // showConfirmButton: false
                    });
                });



            })//solicitud.IdTipoIdentificacion, solicitud.Identificacion

			

        }, function (err) {
            dimiss_espere('selfie', function () {
                alert(err);

            }, function () {
                // alert("No se mostro el loading"); }
                swal({
                    type: 'error',
                    title: 'Oops...',
                    text: "No se mostro el loading",
                    timer: 3000,
                    // showConfirmButton: false
                });
            });


        })



    }, function () {



        dimiss_espere('selfie', function () {
            //alert("la imagen no es válida");
            swal({
                type: 'error',
                title: 'Oops...',
                text: "la imagen no es válida",
                timer: 3000,
                // showConfirmButton: false
            });

        }, function () {
            swal({
                type: 'error',
                title: 'Oops...',
                text: "No se mostro el loading",
                timer: 3000,
                // showConfirmButton: false
            });
        });

    });

});


//continuar al paso 8

$('.aceptar-selfie').on('click', function () {

    mostrar_paso(8);

});



//**********************************************FIN DE PASO 7*********************************************************
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//**********************************************PASO 8 CUENTA BANCARIA*****************************
function validaNumericos(event) {
	if (event.charCode >= 48 && event.charCode <= 57) {
		return true;
	}
	alert("Debe digitar solo números en el campo de la cuenta!");
}

function mostarCampoCuenta() {

    var banco_seleccionado = $("#banco-select").val();

    //PLUGIN INPUT MASK DE JQUERY

    if (banco_seleccionado == 1) {


        $("#cuenta_cliente").inputmask("999-99-999-999999-9");  //BN normal


    }

    if (banco_seleccionado == 2) {  //BCR normal

        $("#cuenta_cliente").inputmask("999-9999999-9");


    }

    if (banco_seleccionado == 3) { //Popular

        $("#cuenta_cliente").inputmask("9999999");


    }


    if (banco_seleccionado == 4) { //BAC

        $("#cuenta_cliente").inputmask("999999999");

    }

    if (banco_seleccionado == 5) { //Promerica

        $("#cuenta_cliente").inputmask("99999999999999");


    }


    if (banco_seleccionado == 6) { //Davivienda

        $("#cuenta_cliente").inputmask("99999999999");


    }


    if (banco_seleccionado == 7) { //Scotiabank

        $("#cuenta_cliente").inputmask("99999999999");


    }


    if (banco_seleccionado == 8) { //LAFISE

        $("#cuenta_cliente").inputmask("999999999");


    }

    if (banco_seleccionado == 13) { //COOPEALIANZA

        $("#cuenta_cliente").inputmask("99999999999999");


    }


    if (banco_seleccionado == 17) { //COOPESERVIDORES SINPE

        $("#cuenta_cliente").inputmask("99999999999999999");


    }


    if (banco_seleccionado == 20) { //COOPEAMISTAD SINPE

        $("#cuenta_cliente").inputmask("99999999999999999");


    }


    if (banco_seleccionado == 21) { //COOPEANDE SINPE

        $("#cuenta_cliente").inputmask("99999999999999999");


    }


    if (banco_seleccionado == 18) { //MUCAP

        $("#cuenta_cliente").inputmask("999999999999");


    }


    if (banco_seleccionado == 19) { //Mutual Alajuela

        $("#cuenta_cliente").inputmask("9999999");


    }


    if (banco_seleccionado == 10) { //OTROS 17 DIGITOS


        $("#cuenta_cliente").inputmask("99999999999999999");

    }


    $("#cuenta_cliente").val(''); //reseteamos el valor de la cuenta de banco


}

//continuar al paso 9

const cuentaBancariaContinuar = () => {
    let cuentaSimpe = '';
    let tipoCuenta = '';
    var numero_de_cuenta = '' //sin guiones obtiene el valor
    var banco_seleccionado = parseInt($("#banco-select").val());
    switch (banco_seleccionado) {
        case 10:// Otro banco
            {
                cuentaSimpe = $("#cuenta-simpe").val();
                tipoCuenta = '';
                numero_de_cuenta = '';
                break;
            }
        default: {
            cuentaSimpe = '';
            if (banco_seleccionado != 4) {//Si es diferente al BAC
                tipoCuenta = $("#cbo-tipo-cuenta").val();
            }
            numero_de_cuenta = $("#cuenta_cliente").inputmask('unmaskedvalue');
        }
    }

    if (banco_seleccionado == 1 && numero_de_cuenta.length != 15) //BN
    {

        alert("Digita una cuenta bancaria corriente o de ahorros válida de 15 digitos");

        return false;

    }


    else if (banco_seleccionado == 2 && numero_de_cuenta.length != 11) //BCR
    {

        alert("Digita una cuenta bancaria corriente o de ahorros válida de 11 digitos");

        return false;

    }

    else if (banco_seleccionado == 4 && numero_de_cuenta.length != 9) //BAC
    {

        alert("Digita una cuenta bancaria corriente o de ahorros válida de 9 digitos");

        return false;

    }

    else if (banco_seleccionado == 10 && $("#cuenta-simpe").val().length != 17) //OTROS SINPE
    {

        alert("Digita la cuenta cliente SINPE de 17 digitos");

        return false;

    }

    if (numero_de_cuenta || cuentaSimpe) { //si no esta vacia
		let s = appStore.get('solicitud');

		let solicitud_bancaria = {
			IdTipoIdentificacion: solicitud_cliente.IdTipoIdentificacion,
            Identificacion: solicitud_cliente.cedula,
            idBanco: $("#banco-select").val(),
            Cuenta: numero_de_cuenta,
            CuentaSinpe: cuentaSimpe,
            CuentaIban: "",
            UsrModifica: 'WEB',
            Operaciones: "I",
            TipoMoneda: "1",
            TipoCuenta: tipoCuenta
        }

        GuardaCuentaBancaria(solicitud_bancaria, function (success) {

            if (Array.isArray(success) && success[0].result) {
                //metodos_api 
				fillContractData(solicitud_cliente.IdTipoIdentificacion, solicitud_cliente.cedula);

            }
            else {
                swal({
                    icon: 'error',
                    title: 'Oops...',
                    text: "no se pudo procesar la cuenta bancaria",
                    // showConfirmButton: false
                });

            }

        }, function (err) {


            alert(err);

        }) //solicitud.IdTipoIdentificacion, solicitud.Identificacion, solicitud.idBanco, solicitud.Cuenta, solicitud.CuentaSinpe, solicitud.CuentaIban, solicitud.UsrModifica, solicitud.Operaciones, solicitud.TipoMoneda, solicitud.TipoCuenta.


        /*
        solicitud_cliente.cuenta_usuario = numero_de_cuenta;
        
        solicitud_cliente.tipo_cuenta =  $( "#banco-select" ).val(); //id 1 BN, 2 BCR, 4 BAC, 10 OTROS.......
        
        mostrar_paso(9);*/


    }
    else {


        alert("Debe digitar una cuenta válida");

    }


}


//**********************************FIN PASO 8******************************************************
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



//********************************************PASO 9 TERMINOS Y CONDICIONES*****************************************

function aceptarTerminosClick() {
    mostrar_paso(10); //muestra el paso de FIRMA
}

$('.terminos-continuar-declino').on('click', function () {
    swal({
        title: "Advertencia",
        text: "No podemos procesar tu solicitud, sino aceptas los términos y condiciones",
        icon: "warning",
        dangerMode: true,
    })
});


//********************************************FIN PASO 9*****************************************************
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//************************************PASO 10 FIRMA************************************************************

function firmaTerminarClick() {

    if (signaturePad.isEmpty()) {
        return alert("La firma no puede estar vacia!");
    }
    else {

        porfavor_espere('firma', false, true, function (success) {

            var data = signaturePad.toDataURL('image/jpg'); //base64

            console.log(data);

			enviar_cedulaFirma(data, solicitud_cliente.IdTipoIdentificacion, solicitud_cliente.cedula, function (data) {
                mostrar_paso(11);

                solicitud_cliente.firma = data; //conservamos lafirma para despues guardarla  


                dimiss_espere('firma', function () { }, function () { alert("No se mostro el loading"); });


            }, function (err) {
                dimiss_espere('firma', function () {
                    alert(err);

                }, function () { alert("No se mostro el loading"); });
            })



        }, function (err) {


            dimiss_espere('firma', function () {
                alert(err);

            }, function () { alert("No se mostro el loading"); });


        });


    }

}


//importante para que se vea bien la firma (esto debe hacerse cada vez que se redimensione la pantalla).../
function inicializar_firma() {
    console.log("inicialiar firma");

    var canvas = document.getElementById('signature-pad');
    var device = Math.max(window.screen.width, window.innerWidth);

    if (device <= 768) { //es un dispositivo movil


        signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'rgb(225, 249, 4)'
        });


        var parentWidth = $(canvas).parent().outerWidth();
        var parentHeight = $(canvas).parent().outerHeight();

        canvas.setAttribute("width", parentWidth);
        canvas.setAttribute("height", 300);

        var ratio = Math.max(window.devicePixelRatio || 1, 1);

        //canvas.getContext("2d").scale(ratio, ratio);

        this.signaturePad = new SignaturePad(canvas);

        signaturePad.clear();

    }
    else {

        signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'rgb(225, 249, 4)'

        });

        var parentWidth = $(canvas).parent().outerWidth();
        var parentHeight = $(canvas).parent().outerHeight();

        canvas.setAttribute("width", parentWidth);

        //var ratio =  Math.max(window.devicePixelRatio || 1, 1);

        //canvas.getContext("2d").scale(ratio, ratio);

        this.signaturePad = new SignaturePad(canvas);

        signaturePad.clear();


    }



}


/*

document.getElementById('save-png').addEventListener('click', function () {
  if (signaturePad.isEmpty()) {
    return alert("Please provide a signature first.");
  }
  
  var data = signaturePad.toDataURL('image/png');
  console.log(data);
  window.open(data);
});

document.getElementById('save-jpeg').addEventListener('click', function () {
  if (signaturePad.isEmpty()) {
    return alert("Please provide a signature first.");
  }

  var data = signaturePad.toDataURL('image/jpeg');
  console.log(data);
  window.open(data);
});

document.getElementById('save-svg').addEventListener('click', function () {
  if (signaturePad.isEmpty()) {
    return alert("Please provide a signature first.");
  }

  var data = signaturePad.toDataURL('image/svg+xml');
  console.log(data);
  console.log(atob(data.split(',')[1]));
  window.open(data);
});
*/

//borrar firma

$('#clear').on('click', function () {


    signaturePad.clear();


});

//deshacer ultimo cambio

$('#undo').on('click', function () {

    var data = signaturePad.toData();
    if (data) {
        data.pop(); // remove the last dot or line
        signaturePad.fromData(data);
    }


});



//*****************************************FIN PASO 10********************************************
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//********************************************PASO 11 REFERENCIA LABORAL*****************************************


//enviar la solicitud final a guardar
/*
function enviar_solicitud() {





}
*/

 function solicitudReferenciaLaboralClick() {
    solicitud_cliente.nombre_referencia = $('#nombre_referencia_laboral').val();
    solicitud_cliente.empresa_referencia = $('#empresa_referencia_laboral').val();
    solicitud_cliente.tel_referencia = $('#telefono_referencia_laboral').val();

    let solicitud_referencia = {
        IdTipoIdentificacion: tipo_ced,
        Identificacion: solicitud_cliente.cedula,
        Empresa: solicitud_cliente.empresa_referencia,
        Telefono: solicitud_cliente.tel_referencia,
        SupervisorDirecto: solicitud_cliente.empresa_referencia,
        UsrModifica: 'WEB'
    };

    /////solicitud.IdTipoIdentificacion, solicitud.Identificacion, solicitud.Empresa, solicitud.Telefono, solicitud.SupervisorDirecto, solicitud.UsrModifica


    if (solicitud_cliente.nombre_referencia && solicitud_cliente.empresa_referencia && solicitud_cliente.tel_referencia) {


        guardar_referenciaLaboral(solicitud_referencia, function (success) {

            if (validacionesFotosCnt === 0) {
                window.location.href = 'gracias.html';
            } else {
                window.location.href = 'gracias.html?validacion-manual=1';
            }

        }, function (err) {


            alert(err);


        })



    }

    else alert("Ningun campo puede estar vacío");

}