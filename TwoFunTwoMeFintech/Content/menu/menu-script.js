$(document).ready(function () {
    $('.container').addClass('container-menu-abierto');
    $('.item').each(function (index) {
        var valorItem = $(this).find('img').attr("id");
        switch (valorItem.toLowerCase()) {
            case 'administrador':
                $(this).find('img').attr("src", "/Content/menu/iconos/administrar.png");
                break;
            case 'cobros':
                $(this).find('img').attr("src", "/Content/menu/iconos/cobros.png");
                break;
            case 'ventas':
                $(this).find('img').attr("src", "/Content/menu/iconos/ventas.png");
                break;
            case 'tesoreria':
                $(this).find('img').attr("src", "/Content/menu/iconos/tesoreria.png");
                break;
            case 'finanzas':
                $(this).find('img').attr("src", "/Content/menu/iconos/finanzas.png");
                break;
            default:
                $(this).find('img').attr("src", "/Content/menu/iconos/neutral.png");
                break;
        }
    });
    $('.subitem a').each(function (index) {
        var valor = $(this).text();
        switch (valor.toLowerCase()) {
            case 'usuarios':
                $(this).find('span').addClass('fas fa-user-edit');
                break;
            case 'rules':
                $(this).find('span').addClass('fas fa-ruler-combined');
                break;
            case 'sub menús':
                $(this).find('span').addClass('fas fa-indent');
                break;
            case 'menús principales':
                $(this).find('span').addClass('glyphicon glyphicon-menu-hamburger');
                break;
            case 'configuración buckets':
                $(this).find('span').addClass('fas fa-chart-bar');
                break;
            case 'crear buckets':
                $(this).find('span').addClass('glyphicon glyphicon-pencil');
                break;
            case 'cola de cobros':
                $(this).find('span').addClass('fas fa-search-dollar');
                break;
            case 'scorecard experto':
                $(this).find('span').addClass('fas fa-address-card');
                break;
            case 'forzar solicitud':
                $(this).find('span').addClass('fas fa-clipboard-list');
                break;
            case 'transferencias':
                $(this).find('span').addClass('fas fa-donate');
                break;
            case 'dash board':
                $(this).find('span').addClass('glyphicon glyphicon-dashboard');
                break;
            case 'reporte transferencias':
                $(this).find('span').addClass('glyphicon glyphicon-list-alt');
                break;
            case 'cambio sinpe':
                $(this).find('span').addClass('glyphicon glyphicon-retweet');
                break;
            case 'colocacion':
                $(this).find('span').addClass('glyphicon glyphicon-usd');
                break;
            case 'configurar buckets':
                $(this).find('span').addClass('glyphicon glyphicon-stats');
                break;
            case 'revisar documentos':
                $(this).find('span').addClass('glyphicon glyphicon-file');
                break;
            case 'pasos agentes':
                $(this).find('span').addClass('glyphicon glyphicon-user');
                break; 
            case 'verificar cuentas':
                $(this).find('span').addClass('glyphicon glyphicon-check');
                break; 
                
            default:
                $(this).find('span').addClass('glyphicon glyphicon-question-sign');
        }
    });
});
$(function () {
    var menu_ul = $('.menu > li > ul'),
        menu_a = $('.menu > li > a');

    menu_ul.hide();
    menu_a.click(function (e) {
        e.preventDefault();
        if (!$(this).hasClass('active') && !$(this).hasClass('header-brand')) {
            menu_a.removeClass('active');
            menu_ul.filter(':visible').slideUp('normal');
            $(this).addClass('active').next().stop(true, true).slideDown('normal');
            $('.flecha-menu span').removeClass('glyphicon glyphicon-menu-down');
            $('.flecha-menu span').addClass('glyphicon glyphicon-menu-right');
            $(this.lastElementChild).removeClass('glyphicon glyphicon-menu-right');
            $(this.lastElementChild).addClass('glyphicon glyphicon-menu-down');
        } else {
            if (!$(this).hasClass('header-brand')) {
                $(this).removeClass('active');
                $(this).next().stop(true, true).slideUp('normal');
                $(this.lastElementChild).removeClass('glyphicon glyphicon-menu-down');
                $(this.lastElementChild).addClass('glyphicon glyphicon-menu-right');
            }
        }
    });

});

$("#menu-principal-funcion").click(function () {
    //cerrar el menu
    if ($('#menu-principal-funcion a span').hasClass('glyphicon glyphicon-align-right')) {
        //contenedor del menu e icono del menu
        $('#menu').addClass('esconder-menu');
        $('#menu-principal-funcion a span').removeClass('glyphicon glyphicon-align-right');
        $('#menu-principal-funcion a span').addClass('glyphicon glyphicon-align-justify');
        //imagen del menu
        $('#imagen-menu').removeClass('imagen-menu-abierto');
        $('#imagen-menu').addClass('imagen-menu-cerrado');
        //mover el contenedor del contedigo general
        $('#body .container').removeClass('container-menu-abierto');
        $('#body .container').addClass('container-menu-cerrado');
        //Activar el tool tip text
        $('.subitem p').addClass('mostrar-tip-text');
        $('.item').children('p').addClass('mostrar-item-tip-text');
        $('.cerrar-sesion p').addClass('mostrar-tip-text');

    }
    else {
        //abrir el menu
        if ($('#menu-principal-funcion a span').hasClass('glyphicon glyphicon-align-justify')) {
            //contenedor del menumenu e icono del menu
            $('#menu').removeClass('esconder-menu');
            $('#menu-principal-funcion a span').removeClass('glyphicon glyphicon-align-justify');
            $('#menu-principal-funcion a span').addClass('glyphicon glyphicon-align-right');
            //imagen del menu
            $('#imagen-menu').removeClass('imagen-menu-cerrado');
            $('#imagen-menu').addClass('imagen-menu-abierto');
            //mover el contenedor del contedigo general
            $('#body .container').removeClass('container-menu-cerrado');
            $('#body .container').addClass('container-menu-abierto');
            //Desactivar el tool tip text
            $('.subitem p').removeClass('mostrar-tip-text');
            $('.item').children('p').removeClass('mostrar-item-tip-text');
            $('.cerrar-sesion p').removeClass('mostrar-tip-text');

        }
    }
});
$(function () {
    $('.item a')
        .mouseover(function () {
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/administrar.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/administrar-hover.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/cobros.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/cobros-hover.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/ventas.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/ventas-hover.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/finanzas.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/finanzas-hover.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/tesoreria.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/tesoreria-hover.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/neutral.png") {
                $(this).find('img').attr("src", "/Content/menuiconos//neutral-hover.png");
            }
        })
        .mouseout(function () {
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/administrar-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/administrar.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/cobros-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/cobros.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/ventas-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/ventas.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/tesoreria-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/tesoreria.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/finanzas-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/finanzas.png");
            }
            if ($(this).find('img').attr('src') == "/Content/menu/iconos/neutral-hover.png") {
                $(this).find('img').attr("src", "/Content/menu/iconos/neutral.png");
            }
        });
});