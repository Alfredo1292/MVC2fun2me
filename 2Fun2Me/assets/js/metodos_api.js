'use strict';

let appStore = new utils.customStorage("app-store");
appStore.set('origen', 1);

{// block scoped script
    const md = new MobileDetect(window.navigator.userAgent);
    let info = {};
    info.mobile = md.mobile();
    info.phone = md.phone();
    info.tabled = md.tablet();
    info.userAgent = md.userAgent();
    info.os = md.os();
    info.isIphone = md.is('iPhone') ? 1 : 0;
    info.webit = md.version('webkit');
    info.systemBuild = md.versionStr('Build');
    getIpAddress().then(ip => {
        info.publicIpAddress = ip;
        appStore.set("device-info", info);
    }).catch(e => {
        throw e;
    });
}

function logEvent(evtData = {
    Paso,
    Evento,
    Data,
    identificacion,
    Solicitud,
    traceId
}) {
    return new Promise((resolve, reject) => {
        if (!evtData.hasOwnProperty('identificacion')) {
            evtData['identificacion'] = '';
        }
        if (evtData.hasOwnProperty('Data')) {
            evtData['Data'] = JSON.stringify(evtData['Data']);
        }
        $.ajax({
            url: `${apiUrl}/Insertar_TraceEventosLOOP`,
            type: 'POST',
            data: evtData,
            success: function (result) {
                console.log('evento ' + evtData['Evento'] + ' logueado');
                return resolve();
            },
            error: function (xhr, desc, errs) {
                Console.log(errs);
                return reject(errs);
            }
        });

    });
}

appStore.set('uId', utils.uId());
logEvent({
    Evento: 'load',
    traceId: appStore.get('uId'),
    Paso: '-1',
    Data:appStore.get('device-info')
}).then(() => {
}).catch(e => {
    throw e;
})


function base64ToBlob(dataURI, success, err) {
    try {
        // convert base64 to raw binary data held in a string
        // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
        var byteString = atob(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

        console.log(mimeString);

        // write the bytes of the string to an ArrayBuffer
        var ab = new ArrayBuffer(byteString.length);
        var ia = new Uint8Array(ab);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        // write the ArrayBuffer to a blob, and you're done
        var bb = new Blob([ab], { type: mimeString });

        //convertimos el blob a file

        var file = new File([bb], 'thefile.jpg', { type: 'image/jpg', lastModified: Date.now() });


        success(file);

    } catch (ex) { err(ex); }


}



//redimensiona la imagen a un tamanio proporcional, retorn&&o un base64 que sera convertido a blob

function redimensionar(files, success, err) {
    try {
        var file = files;
        var canvas = document.createElement('canvas');
        var img = document.createElement("img");
        var imgHeight = null;
        var imgWidth = null; //conservamos las dimensiones de la imagen para calcular un redimension adecuada
        var reader = new FileReader();
        const width = 450;
        const height = 600;
        reader.onload = function (e) {


            img.src = e.target.result; //obtiene un base64 


            img.onload = function () { //obtenemos las dimensiones naturales de la imagen

                imgHeight = this.height;
                imgWidth = this.width;

                if (imgWidth > imgHeight) { //si la anchura es mayor a la altura es una imagen l&&scape
                    imgWidth = height;
                    imgHeight = width;

                } else { //si la altura es mayor a la anchura es portrait;

                    imgWidth = width;
                    imgHeight = height;

                }

                canvas.width = imgWidth;
                canvas.height = imgHeight;

                var ctx = canvas.getContext("2d");
                ctx.drawImage(img, 0, 0, imgWidth, imgHeight);

                var dataurl = canvas.toDataURL("image/jpg", 0.5);

                base64ToBlob(dataurl, function (blob) {

                    success(blob);


                }, function (error) {

                    err(error);


                })


            };


        }
        reader.readAsDataURL(file);

    } catch (ex) { err("No pudimos procesar tu imagen, elige otra"); }

}


//para obtener el id del producto en base al plazo al monto y a la frecuencia.

function obtenerIDProducto(id_monto, id_plazo, id_frecuencia, success, err) {


    try {

        /****************monto 50***********************************/

        if (id_monto == 1 && id_frecuencia == 1 && id_plazo == 1) {
            /*success = 1;*/
            success(44);
        }
        if (id_monto == 1 && id_frecuencia == 1 && id_plazo == 2) {


            /*success = 2;*/

            success(45);
        }
        if (id_monto == 1 && id_frecuencia == 2 && id_plazo == 2) {

            /*success = 3;*/

            success(46);


        }
        if (id_monto == 1 && id_frecuencia == 1 && id_plazo == 3) {

            /*success = 4;*/
            success(47);

        }
        if (id_monto == 1 && id_frecuencia == 2 && id_plazo == 3) {
            /*success = 5;*/

            success(48);

        }
        if (id_monto == 1 && id_frecuencia == 3 && id_plazo == 3) {


            /*success = 6;*/

            success(49);

        }
        if (id_monto == 1 && id_frecuencia == 1 && id_plazo == 4) {

            /*success = 7;*/

            success(50);


        }
        if (id_monto == 1 && id_frecuencia == 2 && id_plazo == 4) {


            success(51);


        }
        if (id_monto == 1 && id_frecuencia == 3 && id_plazo == 4) {


            /* success = 9;*/

            success(52);


        }
        if (id_monto == 1 && id_frecuencia == 1 && id_plazo == 5) {

            /* success = 10;*/

            success(53);
        }
        if (id_monto == 1 && id_frecuencia == 2 && id_plazo == 5) {

            /*success = 11;*/

            success(54);
        }
        if (id_monto == 1 && id_frecuencia == 3 && id_plazo == 5) {

            /*success = 12;*/

            success(55);
        }

        /*100*/

        if (id_monto == 2 && id_frecuencia == 1 && id_plazo == 1) {

            /* success = 13;*/

            success(56);
        }
        if (id_monto == 2 && id_frecuencia == 1 && id_plazo == 2) {


            /*success = 14;*/

            success(57);
        }
        if (id_monto == 2 && id_frecuencia == 2 && id_plazo == 2) {

            /* success = 15;*/

            success(58);


        }
        if (id_monto == 2 && id_frecuencia == 1 && id_plazo == 3) {
            /* success = 16;*/

            success(59);
        }
        if (id_monto == 2 && id_frecuencia == 2 && id_plazo == 3) {


            /*success = 17;*/


            success(60);


        }
        if (id_monto == 2 && id_frecuencia == 3 && id_plazo == 3) {


            /*success = 18;*/

            success(61);

        }
        if (id_monto == 2 && id_frecuencia == 1 && id_plazo == 4) {


            /*success = 19;*/

            success(62);
        }
        if (id_monto == 2 && id_frecuencia == 2 && id_plazo == 4) {
            /*success = 20*/

            success(63);
        }
        if (id_monto == 2 && id_frecuencia == 3 && id_plazo == 4) {

            /*
            success = 21;*/

            success(64);
        }
        if (id_monto == 2 && id_frecuencia == 1 && id_plazo == 5) {

            /*success = 22;*/

            success(65);
        }
        if (id_monto == 2 && id_frecuencia == 2 && id_plazo == 5) {
            /*success = 23;*/

            success(66);
        }
        if (id_monto == 2 && id_frecuencia == 3 && id_plazo == 5) {
            /*success = 24;*/

            success(67);
        }
        if (id_monto == 3) {
            /*id_montos = 'OTROS';
            id_plazos = 'OTROS';
            success = 25;*/
        }

        /*150*/

        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 1) {

            success(26);

        }

        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 2) {

            success(27);

        }
        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 2) {

            success(28);

        }
        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 3) {

            success(29);

        }
        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 3) {

            success(30);

        }



        if (id_monto == 4 && id_frecuencia == 3 && id_plazo == 3) {

            success(31);

        }
        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 4) {

            success(32);

        }
        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 4) {

            success(33);

        }

        if (id_monto == 4 && id_frecuencia == 3 && id_plazo == 4) {

            success(34);

        }

        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 5) {

            success(35);

        }

        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 5) {

            success(36);

        }

        if (id_monto == 4 && id_frecuencia == 3 && id_plazo == 5) {

            success(37);

        }
        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 6) {

            success(38);

        }
        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 6) {

            success(39);

        }
        if (id_monto == 4 && id_frecuencia == 3 && id_plazo == 6) {

            success(40);

        }
        if (id_monto == 4 && id_frecuencia == 1 && id_plazo == 7) {

            success(41);

        }
        if (id_monto == 4 && id_frecuencia == 2 && id_plazo == 7) {

            success(42);

        }
        if (id_monto == 4 && id_frecuencia == 3 && id_plazo == 7) {

            success(43);

        }


    } catch (ex) { err(ex) }


}


//utilizado para guardar los datos del cliente de los primeros 2 pasos

const guardar_cliente = (solicitud, success, err) => { //solicitud.id_monto, solicitud_id_plazo, solicitud_id_frecuencia, solicitud.IdTipoIdentificacion, solicitud.Identificacion, solicitud.PrimerNombre, solicitud.SegundoNombre, solicitud.PrimerApellido, solicitud.SegundoApellido, solicitud.TelefonoCel, solicitud.Correo, solicitud.IdProducto, UsrModifica: WEB
    obtenerIDProducto(solicitud.id_monto, solicitud.id_plazo, solicitud.id_frecuencia,
        function (idproducto) {
            let s = appStore.get('solicitud');
            let solicitud_envio = {
                IdTipoIdentificacion: solicitud.IdTipoIdentificacion,
                Identificacion: solicitud.Identificacion,
                PrimerNombre: s['nombre'],
                SegundoNombre: s['segundoNombre'],
                PrimerApellido: s['primerApellido'],
                SegundoApellido: s['segundoApellido'],
                TelefonoCel: solicitud.TelefonoCel,
                Correo: solicitud.Correo,
                IdProducto: idproducto,
                idBanco: null,
                UsrModifica: 'WEB',
                Origen: appStore.get('origen'),
                Status: "SOK1"
            };

            $.ajax({
                url: `${apiUrl}/GuardarPersonaWeb`,
                type: 'POST',
                data: solicitud_envio,
                success: function (result) {
                    console.log("GuardarPersonaWeb");
                    console.log(result);
                    if (result.Status === "SOK1") {
                        setTimeout(function () { success(result) }, 10000);
                    } else {
                        swal(message.swalErrorTitle, message.savePersonError, "error");
                    }

                },
                error: function (xhr, desc, errs) {
                    swal("A ocurrido un error", "Lo sentimos, no pudimos verificar sus datos, intentelo más tarde ", "error");
                    console.log(errs);
                    console.log(xhr);
                    console.log(desc);
                    err("Lo sentimos, no pudimos verificar sus datos, intentelo más tarde ");
                }
            });

        }, function (error) {
            err("Lo sentimos, presentamos problemas en nuestro servidor, intentelo más tarde");
            console.log(error);
        });


}


//genera el pin digitado

function generar_pin(solicitud, callback, err) { //solicitud.IdTipoIdentificacion, solicitud.Identificacion
    $.ajax({
        url: `${apiUrl}/GeneraNuevoPIN`,
        type: 'POST',
        data: solicitud,
        success: function (result) {
            callback(result);
        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
            err("Lo sentimos, no pudimos generar el pin de verificacion, intentelo más tarde ");
        }
    });
}

//validar pin digitado por el usuario

function validar_pin(solicitud, callback, err) { //solicitud.IdTipoIdentificacion, solicitud.Identificacion
    console.log("validar pin ");
    console.log(solicitud);
    $.ajax({
        url: `${apiUrl}/ValidarPIN`,
        type: 'POST',
        data: solicitud,
        success: function (result) {
            console.log(result);
            callback(result);

        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
            err("Lo sentimos, no pudimos validar el pin de verificacion, intentelo más tarde ");
        }
    });
}




//envia las fotos de la solicitud (frontal, trasera, selfie, firma);


function enviar_cedulaFrontal(file, tipoIDCedula, Cedula, callback, err) { //TipoIDCedula----Nacional (1) o Dimex (2)
    try {
        console.log(File);
        console.log("enviando la cedula frontal");
        redimensionar(file, (blob) => {
            let formDataBlob = new FormData();
            formDataBlob.append('Frontal', blob); //id de la cedula frontal paso 2

            $.ajax({
                url: `${apiUrl}/GuardaFotoCedula?IdTipoIdentificacion=${tipoIDCedula}&Identificacion=${Cedula}`,
                type: 'POST',
                data: formDataBlob,
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    let msg = null;
                    if (Array.isArray(result)) {
                        msg = result[0].Mensaje;
                    }
                    console.log("success");
                    console.log(result);
                    if (msg == 'SOK02') {
                        callback(result);
                    } else {
                        err("Error al guardar la foto");
                    }
                },
                error: function (xhr, desc, errs) {
                    console.log(errs);
                    console.log(xhr);
                    console.log(desc);
                    err(errs);
                }
            });
        }, (e) => {
            throw e;
        });
    } catch (e) {
        alert(e);
    }

}


//envia las fotos de la solicitud (frontal, trasera, selfie, firma);


function enviar_cedulaDorso(file, tipoIDCedula, Cedula, callback, err) { //TipoIDCedula----Nacional (1) o Dimex (2
    redimensionar(file, (blob) => {
        console.log("enviando la cedula dorso");
        let formDataBlob = new FormData();
        formDataBlob.append('Dorso', blob); //id de la cedula dorso paso 3
        $.ajax({
            url: `${apiUrl}/GuardaFotoCedulaTrasera?IdTipoIdentificacion=${tipoIDCedula}&Identificacion=${Cedula}`,
            type: 'POST',
            data: formDataBlob,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                let msg = null;
                if (Array.isArray(result)) {
                    msg = result[0].Mensaje;
                }
                console.log("success");
                console.log(result);
                if (msg == 'SOK03') {
                    callback(result);
                } else {
                    err("Error al guardar la cedula dorso");
                }
            },
            error: function (xhr, desc, errs) {
                alert(errs);
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                err(errs);
            }
        });

    }, (e) => {
        throw e;
    });

}


//envia las fotos de la solicitud (frontal, trasera, selfie, firma);


function enviar_cedulaSelfie(file, tipoIDCedula, Cedula, callback, err) { //TipoIDCedula----Nacional (1) o Dimex (2)
    redimensionar(file, (blob) => {
        let formDataBlob = new FormData();
        formDataBlob.append('Selfie', blob); //id de la cedula dorso paso 3
        $.ajax({
            url: `${apiUrl}/GuardaFotoSelfie?IdTipoIdentificacion=${tipoIDCedula}&Identificacion=${Cedula}`,
            type: 'POST',
            data: formDataBlob,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                console.log(result);
                if (result === "SOK04") {
                    callback(result);
                } else {
                    swal(message.swalErrorTitle, "Error al guardar Selfie", "error");
                }
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                err(errs);
            }
        });

    }, (e) => {
        throw e;
    });
}

const creaContratoPagare = (tipoIdentificacion, identificacion) => {
    console.log("Crear contrato pagare");
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: `${apiUrl}/CreaContratoPagare`,
            data: {
                IdTipoIdentificacion: tipoIdentificacion,
                Identificacion: identificacion
            },
            dataType: 'json',
            success: (data) => {
                console.log("Crear contrato pagare response");
                console.log(data);
                return resolve(data);
            },
            error: (err) => {
                swal({
                    type: 'error',
                    title: 'Oops...',
                    text: "Error al crear contrato"
                });
                return reject(err);
            }
        });
    });
}

//firma
function enviar_cedulaFirma(base64File, tipoIDCedula, Cedula, callback, err) { //TipoIDCedula----Nacional (1) o Dimex (2)
    console.log("Enviar firma");
    console.log(base64File);
    console.log(tipoIDCedula);
    console.log(Cedula);

    base64ToBlob(base64File, function (blob) { //input.file(0)

        console.log("enviando la cedula firma");

        console.log(blob);

        var formDataBlob = new FormData();

        formDataBlob.append('Firma', blob);

        $.ajax({
            url: `${apiUrl}/GuardaFotoFirma?IdTipoIdentificacion=${tipoIDCedula}&Identificacion=${Cedula}`,
            type: 'POST',
            data: formDataBlob,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result === "SOK05") {
                    creaContratoPagare(tipoIDCedula, Cedula).then((result1) => {
                        callback(result);
                    }).catch(e => {
                        console.error("Error al guardar contrato pagare");
                    });
                } else {
                    swal(message.swalErrorTitle, "Error al enviar cedula firma", "error");
                }
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                err(errs);
            }
        });

    }, function (err) {
        err(err);
    });


}


//Guarda la referencia laboral

function guardar_referenciaLaboral(solicitud, callback, err) { //solicitud.IdTipoIdentificacion, solicitud.Identificacion, solicitud.Empresa, solicitud.Telefono, solicitud.SupervisorDirecto, solicitud.UsrModifica


    console.log("guardar referencia ");

    console.log(solicitud);


    $.ajax({
        url: `${apiUrl}/GuardaReferenciaLaboral`,
        type: 'POST',
        data: solicitud,

        success: function (result) {
            if (Array.isArray(result) && result[0].Mensaje === "SOK07") {
                callback(result);
            } else {
                swal({
                    type: 'error',
                    title: 'Oops...',
                    text: "Error al guardar referencia laborar"
                });
            }
        },

        error: function (xhr, desc, errs) {
            swal({
                icon: 'error',
                title: 'Oops...',
                text: "Error al guardar referencia laborar"
            });
            console.log(errs);
            console.log(xhr);
            console.log(desc);

        }
    });




}


//Comparar imagenes de cedula

function CompararImagenes(solicitud, success, err) { //solicitud.IdTipoIdentificacion, solicitud.Identificacion

    console.log("comparar imagenes ");

    console.log(solicitud);

    $.ajax({
        url: `${apiUrl}/CompararImagenes`,
        type: 'POST',
        data: solicitud,
        success: function (result) {
            console.log(result);
            success(result);

        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
            err("Lo sentimos, no pudimos comparar su rostro ");

        }
    });
}


//GuardaCuentaBancaria

function GuardaCuentaBancaria(solicitud, callback, err) { //solicitud.IdTipoIdentificacion, solicitud.Identificacion, solicitud.idBanco, solicitud.Cuenta, solicitud.CuentaSinpe, solicitud.CuentaIban, solicitud.UsrModifica, solicitud.Operaciones, solicitud.TipoMoneda, solicitud.TipoCuenta.
    console.log("bancaria pin ");
    console.log(solicitud);
    $.ajax({
        url: `${apiUrl}/GuardaCuentaBancaria`,
        type: 'POST',
        data: solicitud,
        success: function (result) {
            if (Array.isArray(result)) {

            }
            callback(result);
        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
            err("Lo sentimos, no pudimos guardar la cuenta bancaria ");
        }
    });

}
/*
Obtiene los datos del contrato necesarios para el paso 9
*/

const getContractData = (tipoCedula, cedula) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: `${apiUrl}/ConsultaDatosContratoPagare`,
            data: {
                IdTipoIdentificacion: tipoCedula,
                Identificacion: cedula
            },
            dataType: 'json',
            success: (data) => {
                return resolve(data);
            },
            error: (err) => {
                swal({
                    type: 'error',
                    title: 'Oops...',
                    text: "Error al guardar referencia laborar",
                    timer: 2000,
                    // showConfirmButton: false
                });
                return reject(err);
            }
        });
    });
};


function logPaso3(IdTipoIdentificacion, Identificacion) {
    console.log(solicitud_cliente);
    paso4Req.IdTipoIdentificacion = IdTipoIdentificacion;
    paso4Req.Identificacion = Identificacion;

    $.ajax({
        url: `${apiUrl}/Paso3`,
        type: 'POST',
        data: {
            IdTipoIdentificacion,
            Identificacion
        },
        success: function (result) {
            console.log(result);
        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
        }
    });
}

function logPaso4(data) {
    $.ajax({
        url: `${apiUrl}/Paso4`,
        type: 'POST',
        data/*: {
            IdTipoIdentificacion,
            Identificacion,
            Credito_Aprobado,
        }*/,
        success: function (result) {
        },
        error: function (xhr, desc, errs) {
            console.log(errs);
            console.log(xhr);
            console.log(desc);
        }
    });
}

const logPinVerificado = () => {
    return new Promise((resolve, reject) => {
        const d = appStore.get('solicitud');
        return $.ajax({
            url: `${apiUrl}/PinVerificado`,
            type: 'POST',
            data: {
                IdTipoIdentificacion: d.IdTipoIdentificacion,
                Identificacion: d.Identificacion
            },
            success: function (result) {
                console.log(result);
                return resolve(result);
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                return reject(errs);
            }
        });
    });
}

const logPinFallido = () => {
    return new Promise((resolve, reject) => {
        const d = appStore.get('solicitud');
        return $.ajax({
            url: `${apiUrl}/PinFallido`,
            type: 'POST',
            data: {
                IdTipoIdentificacion: d.IdTipoIdentificacion,
                Identificacion: d.Identificacion
            },
            success: function (result) {
                console.log(result);
                return resolve(result);
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                return reject(errs);
            }
        });
    });
}




function logPasoCedula(identificacion) {
    return new Promise((resolve, reject) => {
        const url = `${apiUrl}/PasoIdentificacion`
        return $.ajax({
            data: {
                identificacion,
                ...appStore.get("device-info")
            },
            url,
            type: 'POST',
            success: function (result) {
                return resolve(result);
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                return reject(errs);
            }
        });
    });
}

let logPaso0 = () => {
    return new Promise((resolve, reject) => {
        const url = `${apiUrl}/Paso0`
        return $.ajax({
            data: {
                IdProducto: appStore.get("idProducto")
            },
            url,
            type: 'POST',
            success: function (result) {
                return resolve(result);
            },
            error: function (xhr, desc, errs) {
                console.log(errs);
                console.log(xhr);
                console.log(desc);
                return reject(errs);
            }
        });
    });
}