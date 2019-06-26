'use strict';
/*Seleccion del tipo de cuenta en el paso 8*/
function tipoCuentaChange() {
    setTimeout(() => {
        $("#cuenta_cliente").focus();
    }, 50);
}

const bancoSelectChange = (val) => {
    const easingSpeed = 600;
    console.log(val);
    val = parseInt(val);
    switch (val) {
        case 10://Otro
            {
                $("#tipo-cuenta-row").hide(easingSpeed);
                $("#cuenta-row").hide(easingSpeed);
                $("#cuenta-simpe-row").show(easingSpeed);
                break;
            }
        default: {
            if (val === 4) {
                $("#tipo-cuenta-row").hide(easingSpeed);
            } else {
                $("#tipo-cuenta-row").show(easingSpeed);
            }
            $("#cuenta-simpe-row").hide(easingSpeed);
            $('#cuenta-row').show(easingSpeed);
            mostarCampoCuenta();
        }
    }
}

/*monto, plazo, frecuencia para la solicitud**/

var monto_save = 0;
var frecuencia_save = 0;
var plazo_save = 0;


/******************************************/
$(document).on('input', '#rangePlazo', function () {
    var value = $(this).val();
    showDatePlazo(value);
});

function showDatePlazo(value) {
    if (value == 20) {
        $('#valuePlazo').html('1 semana');
    } else if (value == 40) {
        $('#valuePlazo').html('2 semana');
    } else if (value == 60) {
        $('#valuePlazo').html('1 mes');
    } else if (value == 80) {
        $('#valuePlazo').html('2 mes');
    } else if (value == 100) {
        $('#valuePlazo').html('3 mes');
    }

}
var rangeSlider = function () {
    var slider = $('.range-slider'),
        range = $('.range-slider__range'),
        value = $('.range-slider__value');

    slider.each(function () {

        value.each(function () {
            var value = $(this).prev().attr('value');
            $(this).html('' + value);
        });

        range.on('input', function () {
            $(this).next(value).html('₡' + this.value);
        });
    });

    slider.on('slide', () => {
        console.log('slide');
    });
};
rangeSlider();

const data = {
    "cuotas": [{
        "money": 50,
        "plazo": 7,
        "frecuencia": "semanal",
        "monto": "62.000"
    },
    {
        "money": 50,
        "plazo": 14,
        "frecuencia": "semanal",
        "monto": "31.000"
    },
    {
        "money": 50,
        "plazo": 15,
        "frecuencia": "quincenal",
        "monto": "63.000"
    },
    {
        "money": 50,
        "plazo": 28,
        "frecuencia": "semanal",
        "monto": "16.000"
    },

    {
        "money": 50,
        "plazo": 30,
        "frecuencia": "quincenal",
        "monto": "32.000"
    },
    {
        "money": 50,
        "plazo": 30,
        "frecuencia": "mensual",
        "monto": "65.000"
    },
    {
        "money": 50,
        "plazo": 63,
        "frecuencia": "semanal",
        "monto": "8.000"
    },
    {
        "money": 50,
        "plazo": 60,
        "frecuencia": "quincenal",
        "monto": "17.000"
    },
    {
        "money": 50,
        "plazo": 60,
        "frecuencia": "mensual",
        "monto": "34.000"
    },


    {
        "money": 50,
        "plazo": 91,
        "frecuencia": "semanal",
        "monto": "6.000"
    },

    {
        "money": 50,
        "plazo": 90,
        "frecuencia": "quincenal",
        "monto": "12.000"
    },
    {
        "money": 50,
        "plazo": 90,
        "frecuencia": "mensual",
        "monto": "24.000"
    },

    {
        "money": 100,
        "plazo": 7,
        "frecuencia": "semanal",
        "monto": "118.000"
    },
    {
        "money": 100,
        "plazo": 14,
        "frecuencia": "semanal",
        "monto": "60.000"
    },
    {
        "money": 100,
        "plazo": 15,
        "frecuencia": "quincenal",
        "monto": "120.000"
    },

    {
        "money": 100,
        "plazo": 28,
        "frecuencia": "semanal",
        "monto": "31.000"
    },
    {
        "money": 100,
        "plazo": 30,
        "frecuencia": "quincenal",
        "monto": "62.000"
    },
    {
        "money": 100,
        "plazo": 30,
        "frecuencia": "mensual",
        "monto": "125.000"
    },
    {
        "money": 100,
        "plazo": 63,
        "frecuencia": "semanal",
        "monto": "15.000"
    },
    {
        "money": 100,
        "plazo": 60,
        "frecuencia": "quincenal",
        "monto": "32.000"
    },
    {
        "money": 100,
        "plazo": 60,
        "frecuencia": "mensual",
        "monto": "66.000"
    },
    {
        "money": 100,
        "plazo": 91,
        "frecuencia": "semanal",
        "monto": "11.000"
    },
    {
        "money": 100,
        "plazo": 90,
        "frecuencia": "quincenal",
        "monto": "23.000"
    },
    {
        "money": 100,
        "plazo": 90,
        "frecuencia": "mensual",
        "monto": "46.000"
    },
    {
        "money": 150,
        "plazo": 7,
        "frecuencia": "semanal",
        "monto": "179.000"
    },

    {
        "money": 150,
        "plazo": 14,
        "frecuencia": "semanal",
        "monto": "91.000"
    },
    {
        "money": 150,
        "plazo": 15,
        "frecuencia": "quincenal",
        "monto": "183.000"
    },
    {
        "money": 150,
        "plazo": 28,
        "frecuencia": "semanal",
        "monto": "46.000"
    },
    {
        "money": 150,
        "plazo": 30,
        "frecuencia": "quincenal",
        "monto": "94.000"
    },

    {
        "money": 150,
        "plazo": 30,
        "frecuencia": "mensual",
        "monto": "190.000"
    },
    {
        "money": 150,
        "plazo": 63,
        "frecuencia": "semanal",
        "monto": "22.000"
    },
    {
        "money": 150,
        "plazo": 60,
        "frecuencia": "quincenal",
        "monto": "49.000"
    },
    {
        "money": 150,
        "plazo": 60,
        "frecuencia": "mensual",
        "monto": "99.000"
    },
    {
        "money": 150,
        "plazo": 91,
        "frecuencia": "semanal",
        "monto": "16.000"
    },

    {
        "money": 150,
        "plazo": 90,
        "frecuencia": "quincenal",
        "monto": "34.000"
    },
    {
        "money": 150,
        "plazo": 90,
        "frecuencia": "mensual",
        "monto": "69.000"
    },

    {
        "money": 150,
        "plazo": 119,
        "frecuencia": "semanal",
        "monto": "13.000"
    },

    {
        "money": 150,
        "plazo": 120,
        "frecuencia": "quincenal",
        "monto": "27.000"
    },
    {
        "money": 150,
        "plazo": 120,
        "frecuencia": "mensual",
        "monto": "54.000"
    },

    {
        "money": 150,
        "plazo": 154,
        "frecuencia": "semanal",
        "monto": "10.000"
    },
    {
        "money": 150,
        "plazo": 150,
        "frecuencia": "quincenal",
        "monto": "22.000"
    },

    {
        "money": 150,
        "plazo": 150,
        "frecuencia": "mensual",
        "monto": "45.000"
    }
    ]

}

document.querySelector("#mont").textContent = data.cuotas[4].monto;

function calcularCuota() {
    var values = getInputs();
    var monto;
    var cuotas = data.cuotas;
    for (let index = 0; index < cuotas.length; index++) {
        if (values.money === cuotas[index].money && values.plazo === cuotas[index].plazo && values.frecuencia === cuotas[index].frecuencia) {
            monto = cuotas[index].monto;

        }
    }


    if (monto == undefined) {



        //******************************si selecciona una combinacion invalida se devuelve a la frecuencia semanal

        var frecuencia_value = parseInt($('#ex24').data('slider').getValue()); //slider frecuencia obtenemos su valor


        if (frecuencia_value == 1) { //semanal


            var semanal = $('#frecu .slider-tick-container .slider-tick');

            $(semanal).css("background", "gray"); //color de deshabilitado


            console.log(semanal[0]);
        }


        if (frecuencia_value == 2) { //quincenal

            var quincenal = $('#frecu .slider-tick-container .slider-tick');


            $(quincenal).css("background", "gray"); //color de deshabilitado


            console.log(quincenal[1]);
        }

        if (frecuencia_value == 3) { //mensual

            var mensual = $('#frecu .slider-tick-container .slider-tick');

            $(mensual).css("background", "gray"); //color de deshabilitado


            console.log(mensual[2]);

        }

        //var slider_value = frecuencia_value>1 ? (frecuencia_value-1) : 1; //le restamos 1 a la frecuencia si no es 1

        $("#ex24").slider({

        }).slider('setValue', 1);

        calcularCuota();


        $("#mont").html("Forma pago no permitida para el plazo");

        setTimeout(function () {
            $('#frecu .slider-tick-container .slider-tick').css("background", "linear-gradient(to bottom, #f9f9f9 0%, #f5f5f5 100%)");
            calcularCuota();
        }, 2000);


    } else { //si el monto es 100 o 50, se muestra hasta 3 meses

        $("#mont").html("₡" + monto);
    }

    logEvent({
        Evento: 'calcular-cuota',
        traceId: appStore.get('uId'),
        Paso: '0',
        Descripcion: 'evento del slider',
        Data: {
            idProducto: appStore.get('idProducto'),
            monto,
            cuotas,
            frecuencia: frecuencia_value
        }
    }).then(() => {
    }).catch(e => {
        throw e;
    })

}
var getInputs = () => {


    var money = $(".monto_credit").val();


    if (money == 1) {

        monto_save = 1; //monto para la solicitud, el id
        money = 50;

        var slider3_visible = $("#plazo2").is(":visible");


        if (!slider3_visible) { //refresca para evitar errores de css en el range input

            $(".tres_meses").show(); // mostramos la grafica de 3 meses
            $(".cinco_meses").hide(); //escondemos la de 5

            $('#ex21').slider('refresh');

        }

    } else if (money == 2) {

        monto_save = 2; //monto para la solicitud, el id
        money = 100;

        var slider3_visible = $("#plazo2").is(":visible");


        if (!slider3_visible) { //refresca para evitar errores de css en el range input

            $(".tres_meses").show(); // mostramos la grafica de 3 meses
            $(".cinco_meses").hide(); //escondemos la de 5

            $('#ex21').slider('refresh');

        }


    } else if (money == 3) {

        monto_save = 4; //monto para la solicitud, el id
        money = 150;

        var slider5_visible = $("#plazo3").is(":visible");


        if (!slider5_visible) { //refresca para evitar errores de css en el range input

            $(".tres_meses").hide(); // escodnemos la grafica de 3 meses
            $(".cinco_meses").show(); //mostramos la de 5

            $('#ex23').slider('refresh');

        }

    }

    var frecuencia = getFrecuencia(); //obtiene la frecuencia en base al id = 1 "retorna semanal", 2 = "retorna quincenal", 3 = "retorna mensual"
    var plazo = getPlazoDias(frecuencia); //en base a la frecuencia (semanal, quincenal, mensual) 
    //si semanal y 1 retorna 7(dias) si 2 retorna 14(dias), si 3 retorna 28(dias), si 4 retorna 63(dias), si 5 retorna 91(dias)  
    //si quincenal y 1 retorna 7(dias) si 2 retorna 15(dias), si 3 retorna 30(dias), si 4 retorna 60(dias), si 5 retorna 90(dias)
    //si mensual y 1 retorna 7(dias) si 2 retorna 15(dias), si 3 retorna 30(dias), si 4 retorna 60(dias), si 5 retorna 90(doas)
    return {
        "money": money,
        "frecuencia": frecuencia,
        "plazo": plazo
    }
}
var getFrecuencia = () => {
    var frecuencia = $(".frecuencia").val();


    frecuencia_save = frecuencia; //obtenemos la frecuencia para la solicitud, el id


    if (frecuencia == 1) {
        return "semanal"
    } else if (frecuencia == 2) {
        return "quincenal"
    } else if (frecuencia == 3) {
        return "mensual"
    }
}

//si 5 meses o 3 meses o menos
var getPlazoDias = (frecuencia) => {

    var money = $(".monto_credit").val();

    var plazo = $(".plazo").val(); //plazo de 3 meses o menos

    if (money == 3) { // se esta mostrando el plazo de 5 meses o menos

        plazo = $(".plazo5meses").val(); //agarramos el dato de plazo del input range de 5 meses

    }

    plazo_save = plazo; //obtenemos el plazo para la solicitud, el id


    if (frecuencia == "semanal") {

        return getDiasSemanal(plazo);
    } else if (frecuencia == "quincenal") {

        return getDiasQuincenal(plazo);
    } else if (frecuencia == "mensual") {

        return getDiasQuincenal(plazo);
    }

}
var getDiasSemanal = (plazo) => {
    if (plazo == 1) {
        return 7
    } else if (plazo == 2) {
        return 14
    } else if (plazo == 3) {
        return 28
    } else if (plazo == 4) {
        return 63
    } else if (plazo == 5) {
        return 91
    } else if (plazo == 6) {
        return 119
    } else if (plazo == 7) {
        return 154
    }
}
var getDiasQuincenal = (plazo) => {
    if (plazo == 1) {
        return 7
    } else if (plazo == 2) {
        return 15
    } else if (plazo == 3) {
        return 30
    } else if (plazo == 4) {
        return 60
    } else if (plazo == 5) {
        return 90
    } else if (plazo == 6) {
        return 120
    } else if (plazo == 7) {
        return 150
    }
}
var getDiasMensual = (plazo) => {
    if (plazo == 1) {
        return 7
    } else if (plazo == 2) {
        return 15
    } else if (plazo == 3) {
        return 30
    } else if (plazo == 4) {
        return 60
    } else if (plazo == 5) {
        return 90
    } else if (plazo == 6) {
        return 120
    } else if (plazo == 7) {
        return 150
    }
}
// runData(data);