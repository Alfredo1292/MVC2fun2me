$(document).ready(function () {
    $('#tblAgenteVentas').DataTable();
    //llamar al metodo del acordeon de la ventana modal
    acordeonModal();
    //llamar al metodo para mover los items
    moverItems();
});

//funciones visuales
function acordeonModal() {
    $('#accordion').find('.accordion-toggle').click(function () {
        //Expand or collapse this panel
        $(this).next().slideToggle('slow', 'swing');
        //Hide the other panels
        $(".accordion-content").not($(this).next()).slideUp('slow', 'swing');
    });
}
function moverItems() {
    $(function () {
        $("#horarios-disponibles-lunes , #horarios-asignados-lunes").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-martes , #horarios-asignados-martes").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-miercoles , #horarios-asignados-miercoles").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-jueves , #horarios-asignados-jueves").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-viernes , #horarios-asignados-viernes").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-sabado , #horarios-asignados-sabado").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
        $("#horarios-disponibles-domingo , #horarios-asignados-domingo").sortable({
            connectWith: ".connected",
            cursor: "pointer"
        }).disableSelection();
    });
}

//funciones para administrar los horarios 
function llenarVentanaModal(idAgente) {
    $('#idAgente strong').text("Agente: " + idAgente);  
    getHorasDisponibles(idAgente);
    getHorasAsignadas(idAgente);
    $('#modalAsignarHorarios').modal('show');   
}
function guardarHorariosAgente() {
    var idAgente = $('#idAgente').text().replace("Agente: ", "");
    guardarHorariosAsignados(idAgente)
    guardarHorariosDisponibles(idAgente)
}
function getHorasDisponibles(idAgente) {
    var horario;
    var list;
    var empObj = {
        cod_agente: idAgente
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AsignacionVentas/GetHorariosDisponibles",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                //llenamos los horarios disponibles de cada dia
                //lunes
                horario = (result[0].lunes.toString()).split(',');
                list = $("#horarios-disponibles-lunes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //martes
                horario = (result[0].martes.toString()).split(',');
                list = $("#horarios-disponibles-martes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //miercoles
                horario = (result[0].miercoles.toString()).split(',');
                list = $("#horarios-disponibles-miercoles");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //jueves
                horario = (result[0].jueves.toString()).split(',');
                list = $("#horarios-disponibles-jueves");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //viernes
                horario = (result[0].viernes.toString()).split(',');
                list = $("#horarios-disponibles-viernes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //sabado
                horario = (result[0].sabado.toString()).split(',');
                list = $("#horarios-disponibles-sabado");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //domingo
                horario = (result[0].domingo.toString()).split(',');
                list = $("#horarios-disponibles-domingo");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                alert(err);
            }
        });
}
function getHorasAsignadas(idAgente) {
    var horario;
    var list;
    var empObj = {
        cod_agente: idAgente
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AsignacionVentas/GetHorariosasignados",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                //llenamos los horarios disponibles de cada dia
                //lunes
                horario = (result[0].lunes.toString()).split(',');
                list = $("#horarios-asignados-lunes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }                  
                });
                //martes
                horario = (result[0].martes.toString()).split(',');
                list = $("#horarios-asignados-martes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //miercoles
                horario = (result[0].miercoles.toString()).split(',');
                list = $("#horarios-asignados-miercoles");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //jueves
                horario = (result[0].jueves.toString()).split(',');
                list = $("#horarios-asignados-jueves");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //viernes
                horario = (result[0].viernes.toString()).split(',');
                list = $("#horarios-asignados-viernes");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //sabado
                horario = (result[0].sabado.toString()).split(',');
                list = $("#horarios-asignados-sabado");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
                //domingo
                horario = (result[0].domingo.toString()).split(',');
                list = $("#horarios-asignados-domingo");
                list.html('');
                $.each(horario, function (key, item) {
                    if (item != "") {
                        list.append('<li Value=' + item + '>' + item + '</li>');
                    }
                });
            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                alert(err);
            }
        });
}
function guardarHorariosAsignados(idAgente) {
    var horarioAsignadoLunes, horarioAsignadoMartes, horarioAsignadoMiercoles, horarioAsignadoJueves, horarioAsignadoViernes, horarioAsignadoSabado, horarioAsignadoDomingo;
    ordenarLista('horarios-disponibles-lunes');
    //extraemos todos los horarios
    horarioAsignadoLunes = extraerHorarios('#horarios-asignados-lunes li');  
    horarioAsignadoMartes = extraerHorarios('#horarios-asignados-martes li');
    horarioAsignadoMiercoles = extraerHorarios('#horarios-asignados-miercoles li');
    horarioAsignadoJueves = extraerHorarios('#horarios-asignados-jueves li');   
    horarioAsignadoViernes = extraerHorarios('#horarios-asignados-viernes li');   
    horarioAsignadoSabado = extraerHorarios('#horarios-asignados-sabado li');   
    horarioAsignadoDomingo = extraerHorarios('#horarios-asignados-domingo li');
    
    var empObj = {
        cod_agente: idAgente,
        lunes: horarioAsignadoLunes,
        martes: horarioAsignadoMartes,
        miercoles: horarioAsignadoMiercoles,
        jueves: horarioAsignadoJueves,
        viernes: horarioAsignadoViernes,
        sabado: horarioAsignadoSabado,
        domingo: horarioAsignadoDomingo    
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AsignacionVentas/GuardarHorariosAsignados",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                return result;
            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                alert(err);
            }
        });
}
function guardarHorariosDisponibles(idAgente) {
     var horarioDisponibleLunes, horarioDisponibleMartes, horarioDisponibleMiercoles, horarioDisponibleJueves, horarioDisponibleViernes, horarioDisponibleSabado, horarioDisponibleDomingo;

    //extraemos todos los horarios
    horarioDisponibleLunes = extraerHorarios('#horarios-disponibles-lunes li');  
    horarioDisponibleMartes = extraerHorarios('#horarios-disponibles-martes li');  
    horarioDisponibleMiercoles = extraerHorarios('#horarios-disponibles-miercoles li');   
    horarioDisponibleJueves = extraerHorarios('#horarios-disponibles-jueves li');  
    horarioDisponibleViernes = extraerHorarios('#horarios-disponibles-viernes li');  
    horarioDisponibleSabado = extraerHorarios('#horarios-disponibles-sabado li');   
    horarioDisponibleDomingo = extraerHorarios('#horarios-disponibles-domingo li');

    var empObj = {
        cod_agente: idAgente,
        lunes: horarioDisponibleLunes,
        martes: horarioDisponibleMartes,
        miercoles: horarioDisponibleMiercoles,
        jueves: horarioDisponibleJueves,
        viernes: horarioDisponibleViernes,
        sabado: horarioDisponibleSabado,
        domingo: horarioDisponibleDomingo
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AsignacionVentas/GuardarHorariosDisponibles",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                return result;
            },
            error: function (xhr, status, p3, p4) {
                var err = p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                alert(err);
            }
        });
}
function extraerHorarios(lista) {
    var elementos = $(lista);
    var horario = "";
    elementos.each(function (index) {
        if (index === elementos.length - 1) {
            horario += $(this).text();

        } else {
            horario += $(this).text() + ",";
        }
    });
    return horario;
}
//Otras Funciones
function ordenarLista2(lista) {
    var elementos = '#' + lista+' li';
    var li = $('#horarios-asignados-lunes li');
    li.sort(function (a, b) {
        if (parseInt($(a).text()) > parseInt($(b).text()))
            return 1;
        else return -1;
    });
    $(li).empty().html(li);
}


