//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();

   
    buscarGrid();
});

function buscarGrid() {
    $(".search").keyup(function () {
        var searchTerm = $(".search").val();
        var listItem = $('.results tbody').children('tr');
        var searchSplit = searchTerm.replace(/ /g, "'):containsi('")

        $.extend($.expr[':'], {
            'containsi': function (elem, i, match, array) {
                return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
            }
        });

        $(".results tbody tr").not(":containsi('" + searchSplit + "')").each(function (e) {
            $(this).attr('visible', 'false');
        });

        $(".results tbody tr:containsi('" + searchSplit + "')").each(function (e) {
            $(this).attr('visible', 'true');
        });

        var jobCount = $('.results tbody tr[visible="true"]').length;
        $('.counter').text(jobCount + ' item');

        if (jobCount == '0') { $('.no-result').show(); }
        else { $('.no-result').hide(); }
    });
}

//Load Data function
function loadData() {
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/ScoreCardExperto/Detalle",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + item.NombreModelo + '</td>';
                html += '<td>' + item.RangoInicial + '</td>';
                html += '<td>' + item.RangoFinal + '</td>';
                html += '<td>' + item.Puntaje + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="return Editar(' + item.Id + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.Id + ')">Eliminar</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

//Function for getting the Data Based upon Employee ID
function Editar(id) {
    $('#id').css('border-color', 'lightgrey');
    $('#nombreModelo').css('border-color', 'lightgrey');
    $('#rangoInicial').css('border-color', 'lightgrey');
    $('#rangoFinal').css('border-color', 'lightgrey');
    $('#Puntaje').css('border-color', 'lightgrey');
    $.ajax({
        url: "/ScoreCardExperto/Editar/" + id,
        type: "POST",
        success: function (result) {
            var html = '';
            $('#id').val(result.Id);
            $('#nombreModelo').val(result.NombreModelo);
            $('#rangoInicial').val(result.RangoInicial);
            $('#rangoFinal').val(result.RangoFinal);
            $('#Puntaje').val(result.Puntaje);
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();


        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//function for updating employee's record
function Update() {
    var empObj = {
        Id: $('#id').val(),
        NombreModelo: $('#nombreModelo').val(),
        RangoInicial: $('#rangoInicial').val(),
        RangoFinal: $('#rangoFinal').val(),
        Puntaje: $('#Puntaje').val(),
    };
    $.ajax({
        url: "/ScoreCardExperto/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');

            $('#nombreModelo').val("");
            $('#rangoInicial').val("");
            $('#rangoFinal').val("");
            $('#Puntaje').val("");

            alertify.alert("Id: " + $('#id').val() + " Se actualizo", function () {
                var searchTerm = $(".search").val();
                var listItem = $('.results tbody').children('tr');
                var searchSplit = searchTerm.replace(/ /g, "'):containsi('")

                $.extend($.expr[':'], {
                    'containsi': function (elem, i, match, array) {
                        return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                    }
                });

                $(".results tbody tr").not(":containsi('" + searchSplit + "')").each(function (e) {
                    $(this).attr('visible', 'false');
                });

                $(".results tbody tr:containsi('" + searchSplit + "')").each(function (e) {
                    $(this).attr('visible', 'true');
                });

                var jobCount = $('.results tbody tr[visible="true"]').length;
                $('.counter').text(jobCount + ' item');

                if (jobCount == '0') { $('.no-result').show(); }
                else { $('.no-result').hide(); }
            });
            $('#id').val("");
        },
        error: function (errormessage) {
            alertify.error('UPPS, lo ciento, favor contacte con el Administrador');
            //alert(errormessage.responseText);
        }
    });
}
//function for deleting employee's record
function Delele(ID) {


    alertify.confirm("¿Desea Continuar?",
        function () {

            //$.ajax({
            //    url: "/User/Eliminar",
            //    type: "POST",
            //    data: {
            //        ID
            //    },
            //    contentType: "application/json;charset=UTF-8",
            //    dataType: "json",
            //    success: function (result) {
            //        loadData();
            //    },
            //    error: function (errormessage) {
            //        return false;
            //    }
            //});
            $.ajax(
                {
                    url: "/ScoreCardExperto/Eliminar",
                    type: "POST",
                    data: {
                        ID
                    },
                    success: function (result) {
                        alertify.alert(result.Mensaje);

                        loadData();
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = p3;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).message;
                        alertify.error('UPPS, lo ciento, favor contacte con el Administrador');
                    }
                });
        });




}
//Function for clearing the textboxes
function clearTextBox() {
    $('#cod_agente').val("");
    $('#estado').val("");
    $('#nombre').val("");
    $('#correo').val("");
    $('#ROLID').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#cod_agente').css('border-color', 'lightgrey');
    $('#nombre').css('border-color', 'lightgrey');
    $('#correo').css('border-color', 'lightgrey');
    $('#ROLID').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#cod_agente').val().trim() == "") {
        $('#cod_agente').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cod_agente').css('border-color', 'lightgrey');
    }
    if ($('#nombre').val().trim() == "") {
        $('#nombre').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#nombre').css('border-color', 'lightgrey');
    }
    if ($('#correo').val().trim() == "") {
        $('#correo').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#correo').css('border-color', 'lightgrey');
    }
    if ($('#ROLID').val().trim() == "") {
        $('#ROLID').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ROLID').css('border-color', 'lightgrey');
    }
    return isValid;
}

