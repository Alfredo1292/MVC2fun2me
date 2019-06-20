function CargaCuentaBancaria() {
    $('.transfer').html();
    var empObj = {
        Identificacion: $('#txtCedula').val(),
		IdSolicitud: $("#txtSolicitud").val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/CuentaBancaria/ConsultaCuentasBancarias",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.DescPredet + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td>' + item.DescCuenta + '</td>';
                html += '<td>' + item.DescMoneda + '</td>';
                html += '<td>' + item.Cuenta + '</td>';
                html += '<td id="cuenta-simpe">' + item.CuentaSinpe + '</td>';
                if (item.Verificacion == 1) {
                    html += '<td width=150px>' + '<p>' + item.VerificacionStatus +'<span id="verificar-mail-icon" class="far fa-check-circle"></span></p>' + '</td>';
                }
                else {
                    html += '<td width=150px>' + '<p> ' + item.VerificacionStatus + ' <span id="verificar-mail-icon" class="verificacionFallida far fa-times-circle"></span></p>' + '</td>';
                }
                html += '<td><a class="btn-tesoreria btn btn-primary btn-primary-asignar" onclick="mostrarImagenSinpe(' + $('#txtCedula').val() + ')">Imagen</a></td>';
                html += '<td width=175px ><form class="form-inline"><div role="group"><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.Id + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.Id + ',' + $('#txtCedula').val() + ')">Eliminar</a></div></form></td>';
                html += '</tr>';
            });

            $("#tblBCO").dataTable().fnClearTable();
            $("#tblBCO").dataTable().fnDestroy();
            //$('.transfer').html(html);
            $("#tblBCO").dataTable({
                "ordering": false,
                "paging": false,
                "searching": false
            });
            $('#bodyTableBCO').html(html);
            $("select[name*='bodyTableBCO_length']").change();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID
function getbyID(id) {
    $('#ChkEditCuentaPredet').css('border-color', 'lightgrey');
    $('#ddlEditBanco').css('border-color', 'lightgrey');
    $('#ddlEditTipoCuenta').css('border-color', 'lightgrey');
    $('#ddlEditMoneda').css('border-color', 'lightgrey');
    $('#txtEditCuentaBancaria').css('border-color', 'lightgrey');
    $('#txtEditCuentaSinpe').css('border-color', 'lightgrey');
    $.ajax({
        url: "/CuentaBancaria/EditaCuentasBancarias/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $("#ddlEditBanco").html("");
            $("#ddlEditBanco").append(new Option("--------Seleccione--------", "-1"));

            $("#ddlEditTipoCuenta").html("");
            $("#ddlEditTipoCuenta").append(new Option("--------Seleccione--------", "-1"));
            $("#ddlEditMoneda").html("");
            $("#ddlEditMoneda").append(new Option("--------Seleccione--------", "-1"));

            $.each(result.ListBancos, function (key, item) {
                $("#ddlEditBanco").append(new Option(item.Descripcion, item.Id));
            });
            $.each(result.ListTipoCuentas, function (key, item) {
                $("#ddlEditTipoCuenta").append(new Option(item.Descripcion, item.Id));
            });
            $.each(result.ListTipoMoneda, function (key, item) {
                $("#ddlEditMoneda").append(new Option(item.Descripcion, item.Id));
            });
            $('#ChkEditCuentaPredet').prop('checked', result.Predeterminado);
            $('#IdCuentaBanc').val(result.Id);
            $('#ddlEditBanco').val(result.IdBanco);
            $('#ddlEditTipoCuenta').val(result.IdTipoCuenta);
            $('#ddlEditMoneda').val(result.IdTipoMoneda);
            $('#txtEditCuentaBancaria').val(result.Cuenta);
            $('#txtEditCuentaSinpe').val(result.CuentaSinpe);

            $('#ModalEditCuentaBanc').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        Id: $('#IdCuentaBanc').val(),
        Identificacion: $('#txtCedula').val(),
        Predeterminado: $('#ChkEditCuentaPredet:checked').length,
        IdBanco: $('#ddlEditBanco option:selected').val(),
        IdTipoMoneda: $('#ddlEditMoneda option:selected').val(),
        IdTipoCuenta: $('#ddlEditTipoCuenta option:selected').val(),
        Cuenta: $('#txtEditCuentaBancaria').val(),
        CuentaSinpe: $('#txtEditCuentaSinpe').val(),
    };
    //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
    guardarGestion('118', 'Informacion Bancaria', '');
    //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
    $.ajax({
        url: "/CuentaBancaria/ActualizaCuentasBancarias",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#ModalEditCuentaBanc').modal('hide');
            $('#IdCuentaBanc').val("");
            $('#txtEditCuentaBancaria').val("");
            $('#txtEditCuentaSinpe').val("");
            $("#ChkEditCuentaPredet").prop("checked", false);
            $('#ddlEditBanco').html();
            $('#ddlEditMoneda').html();
            $('#ddlEditTipoCuenta').html();

            alertify.alert("Id: " + $('#IdCuentaBanc').val() + " Se actualizo", function () {
                var searchTerm = $(".search").val();
                var listItem = $('.results tbody').children('tr');

                $.extend($.expr[':'], {
                    'containsi': function (elem, i, match, array) {
                        return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                    }
                });
                var jobCount = $('.results tbody tr[visible="true"]').length;
                $('.counter').text(jobCount + ' item');

                if (jobCount == '0') { $('.no-result').show(); }
                else { $('.no-result').hide(); }
                $('#IdCuentaBanc').val("");
                clearTextBoxEdit();
                CargaCuentaBancaria();
            });
            alertify.success("Proceso Realizado con Exito !!!");
            //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
            //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
            //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//function for create employee's record
function Crear() {
    var res = validateNuevo();
    if (res == false) {
        return false;
    }
    var empObj = {
        Identificacion: $('#txtCedula').val(),
        Predeterminado: $('#ChkNuevoCuentaPredet:checked').length,
        IdBanco: $('#ddlNuevoBanco option:selected').val(),
        IdTipoMoneda: $('#ddlNuevoMoneda option:selected').val(),
        IdTipoCuenta: $('#ddlNuevoTipoCuenta option:selected').val(),
        Cuenta: $('#txtNuevaCuentaBancaria').val(),
        CuentaSinpe: $('#txtNuevaCuentaSinpe').val(),
        //INICIO FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
        IdSolicitud: $("#txtSolicitud").val(),
        //FIN FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
    };
    //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
    guardarGestion('118', 'Informacion Bancaria', '');
    //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
    $.ajax({
        url: "/CuentaBancaria/CreaCuentasBancarias",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myModal').modal('hide');
            $('#txtNuevaCuentaBancaria').val("");
            $('#txtNuevaCuentaSinpe').val("");
            $("#ChkNuevoCuentaPredet").prop("checked", false);
            $('#ddlNuevoBanco').html();
            $('#ddlNuevoMoneda').html();
            $('#ddlNuevoTipoCuenta').html();
            if (result.Respuesta != "" || result.Respuesta != null) {
                alertify.alert(result.Respuesta, function () {

                });
            } else {

                alertify.alert("La Cuenta Bancaria se ha Creado con Exito.", function () {
                    var searchTerm = $(".search").val();
                    var listItem = $('.results tbody').children('tr');

                    $.extend($.expr[':'], {
                        'containsi': function (elem, i, match, array) {
                            return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                        }
                    });
                    var jobCount = $('.results tbody tr[visible="true"]').length;
                    $('.counter').text(jobCount + ' item');

                    if (jobCount == '0') { $('.no-result').show(); }
                    else { $('.no-result').hide(); }
                    getPasosAgenteCuentaBancaria();
                    CargaCuentaBancaria();
                });
            }
            alertify.success("Proceso Realizado con Exito !!!");
            //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
            //if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
            //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//function for deleting employee's record
function Delele(ID, Identificacion) {


    alertify.confirm("¿Desea Continuar?",
        function () {
            //INICIO FCAMACHO  GUARDAR GESTION 13/03/2019
            guardarGestion('118', 'Informacion Bancaria', '');
            //FIN FCAMACHO  GUARDAR GESTION 13/03/2019
            $.ajax(
                {
                    url: "/CuentaBancaria/EiminaCuentasBancarias",
                    type: "POST",
                    data: {
                        ID,
                        Identificacion
                    },
                    success: function (result) {
                        alertify
                            .alert(result.Respuesta);
                        $('#UserTable').html();
                        CargaCuentaBancaria();
                        alertify.success("Proceso Realizado con Exito !!!");
                        //INICIO FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                        // if ($("#ColaIniciada").val() == '0') { iniciarCola(); }
                        //FIN FCAMACHO 15/03/2019 PROCESO DE BUSQUEDA MANUAL PARA STATUS DE USUARIO
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = p3;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Respuesta;
                        alert(err);
                    }
                });
        },
        function () {
            alertify.error('Cancel');
        });




}

function LlenarCombobox() {
    var Id = 0;
    $('#ChkNuevoCuentaPredet').css('border-color', 'lightgrey');
    $('#ddlNuevoBanco').css('border-color', 'lightgrey');
    $('#ddlNuevoTipoCuenta').css('border-color', 'lightgrey');
    $('#ddlNuevoMoneda').css('border-color', 'lightgrey');
    $('#txtNuevaCuentaBancaria').css('border-color', 'lightgrey');
    $('#txtNuevaCuentaSinpe').css('border-color', 'lightgrey');
    $.ajax({
        url: "/CuentaBancaria/CargaCombobox/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';

            $("#ddlNuevoBanco").html("");
            $("#ddlNuevoBanco").append(new Option("--------Seleccione--------", "-1"));
            $.each(result.ListBancos, function (key, item) {
                $("#ddlNuevoBanco").append(new Option(item.Descripcion, item.Id));
            });
            $("#ddlNuevoTipoCuenta").html("");
            $("#ddlNuevoTipoCuenta").append(new Option("--------Seleccione--------", "-1"));
            $.each(result.ListTipoCuentas, function (key, item) {
                $("#ddlNuevoTipoCuenta").append(new Option(item.Descripcion, item.Id));
            });
            $("#ddlNuevoMoneda").html("");
            $("#ddlNuevoMoneda").append(new Option("--------Seleccione--------", "-1"));
            $.each(result.ListTipoMoneda, function (key, item) {
                $("#ddlNuevoMoneda").append(new Option(item.Descripcion, item.Id));
            });
            $('#ChkNuevoCuentaPredet').prop('checked', true);
            $('#ddlNuevoBanco').val('');
            $('#ddlNuevoTipoCuenta').val('');
            $('#ddlNuevoMoneda').val('');
            $('#txtNuevaCuentaBancaria').val('');
            $('#txtNuevaCuentaSinpe').val('');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//Function for clearing the textboxes
function clearTextBoxEdit() {
    $('#IdCuentaBanc').val("");
    $("#ChkEditCuentaPredet").prop("checked", false);
    $('#ddlEditBanco').html("");
    $('#ddlEditTipoCuenta').html("");
    $('#ddlEditMoneda').html("");
    $('#txtEditCuentaBancaria').val("");
    $('#txtEditCuentaSinpe').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#IdCuentaBanc').css('border-color', 'lightgrey');
    $('#txtEditCuentaBancaria').css('border-color', 'lightgrey');
    $('#txtEditCuentaSinpe').css('border-color', 'lightgrey');
    $('#ddlEditBanco').css('border-color', 'lightgrey');
    $('#ddlEditTipoCuenta').css('border-color', 'lightgrey');
    $('#ddlEditMoneda').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function MostrarCampoCuenta() {
    var banco_seleccionado = $("#ddlEditBanco").val();

    //PLUGIN INPUT MASK DE JQUERY

    if (banco_seleccionado == 1) {

        $("#txtEditCuentaBancaria").attr("placeholder", "999-99-999-999999-9");
        $('#txtEditCuentaBancaria').attr("maxlength", 15);
        $("#txtEditCuentaBancaria").attr("data-mask", "___-__-___-______-_");
    }

    if (banco_seleccionado == 2) {  //BCR normal

        $("#txtEditCuentaBancaria").attr("placeholder", "999-99999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 11);
        $("#txtEditCuentaBancaria").attr("data-mask", "___-________");
    }

    if (banco_seleccionado == 3) { //Popular

        $("#txtEditCuentaBancaria").inputmask("9999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 7);
        $("#txtEditCuentaBancaria").attr("data-mask", "_______");
    }


    if (banco_seleccionado == 4) { //BAC

        $("#txtEditCuentaBancaria").inputmask("999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 9);
        $("#txtEditCuentaBancaria").attr("data-mask", "_________");
    }

    if (banco_seleccionado == 5) { //Promerica

        $("#txtEditCuentaBancaria").inputmask("99999999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 14);
        $("#txtEditCuentaBancaria").attr("data-mask", "______________");
    }


    if (banco_seleccionado == 6) { //Davivienda

        $("#txtEditCuentaBancaria").inputmask("99999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 11);
        $("#txtEditCuentaBancaria").attr("data-mask", "___________");
    }


    if (banco_seleccionado == 7) { //Scotiabank

        $("#txtEditCuentaBancaria").inputmask("99999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 11);
        $("#txtEditCuentaBancaria").attr("data-mask", "___________");
    }


    if (banco_seleccionado == 8) { //LAFISE

        $("#txtEditCuentaBancaria").inputmask("999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 9);
        $("#txtEditCuentaBancaria").attr("data-mask", "_________");
    }

    if (banco_seleccionado == 13) { //COOPEALIANZA

        $("#txtEditCuentaBancaria").inputmask("99999999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 14);
        $("#txtEditCuentaBancaria").attr("data-mask", "______________");
    }


    if (banco_seleccionado == 17) { //COOPESERVIDORES SINPE

        $("#txtEditCuentaSinpe").inputmask("99999999999999999");
        $('#txtEditCuentaSinpe').attr("maxlength", 17);
        $("#txtEditCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 20) { //COOPEAMISTAD SINPE

        $("#txtEditCuentaSinpe").inputmask("99999999999999999");
        $('#txtEditCuentaSinpe').attr("maxlength", 17);
        $("#txtEditCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 21) { //COOPEANDE SINPE

        $("#txtEditCuentaSinpe").inputmask("99999999999999999");
        $('#txtEditCuentaSinpe').attr("maxlength", 17);
        $("#txtEditCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 18) { //MUCAP

        $("#txtEditCuentaBancaria").inputmask("999999999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 12);
        $("#txtEditCuentaBancaria").attr("data-mask", "____________");
    }


    if (banco_seleccionado == 19) { //Mutual Alajuela

        $("#txtEditCuentaBancaria").inputmask("9999999");
        $('#txtEditCuentaBancaria').attr("maxlength", 7);
        $("#txtEditCuentaBancaria").attr("data-mask", "_______");
    }


    if (banco_seleccionado == 10) { //OTROS 17 DIGITOS


        $("#txtEditCuentaSinpe").inputmask("99999999999999999");
        $('#txtEditCuentaSinpe').attr("maxlength", 17);
        $("#txtEditCuentaSinpe").attr("data-mask", "_________________");
    }


    $("#txtEditCuentaBancaria").val(''); //reseteamos el valor de la cuenta de banco
    $("#txtEditCuentaSinpe").val('');
}

function MostrarCampoCuentaNuevo() {
    var banco_seleccionado = $("#ddlNuevoBanco").val();

    //PLUGIN INPUT MASK DE JQUERY

    if (banco_seleccionado == 1) {

        $("#txtNuevaCuentaBancaria").attr("placeholder", "999-99-999-999999-9");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 15);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "___-__-___-______-_");
    }

    if (banco_seleccionado == 2) {  //BCR normal

        $("#txtNuevaCuentaBancaria").attr("placeholder", "999-99999999");
        $("#txtNuevaCuentaBancaria").attr("data-mask", "___-_______-_");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 11);

    }

    if (banco_seleccionado == 3) { //Popular

        $("#txtNuevaCuentaBancaria").inputmask("9999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 7);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "_______");
    }


    if (banco_seleccionado == 4) { //BAC

        $("#txtNuevaCuentaBancaria").inputmask("999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 9);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "_________");
    }

    if (banco_seleccionado == 5) { //Promerica

        $("#txtNuevaCuentaBancaria").inputmask("99999999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 14);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "______________");
    }


    if (banco_seleccionado == 6) { //Davivienda

        $("#txtNuevaCuentaBancaria").inputmask("99999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 11);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "___________");
    }


    if (banco_seleccionado == 7) { //Scotiabank

        $("#txtNuevaCuentaBancaria").inputmask("99999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 11);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "___________");
    }


    if (banco_seleccionado == 8) { //LAFISE

        $("#txtNuevaCuentaBancaria").inputmask("999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 9);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "_________");
    }

    if (banco_seleccionado == 13) { //COOPEALIANZA

        $("#txtNuevaCuentaBancaria").inputmask("99999999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 14);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "______________");
    }


    if (banco_seleccionado == 17) { //COOPESERVIDORES SINPE

        $("#txtNuevaCuentaSinpe").inputmask("99999999999999999");
        $('#txtNuevaCuentaSinpe').attr("maxlength", 17);
        $("#txtNuevaCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 20) { //COOPEAMISTAD SINPE

        $("#txtNuevaCuentaSinpe").inputmask("99999999999999999");
        $('#txtNuevaCuentaSinpe').attr("maxlength", 17);
        $("#txtNuevaCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 21) { //COOPEANDE SINPE

        $("#txtNuevaCuentaSinpe").inputmask("99999999999999999");
        $('#txtNuevaCuentaSinpe').attr("maxlength", 17);
        $("#txtNuevaCuentaSinpe").attr("data-mask", "_________________");
    }


    if (banco_seleccionado == 18) { //MUCAP

        $("#txtNuevaCuentaBancaria").inputmask("999999999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 12);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "____________");
    }


    if (banco_seleccionado == 19) { //Mutual Alajuela

        $("#txtNuevaCuentaBancaria").inputmask("9999999");
        $('#txtNuevaCuentaBancaria').attr("maxlength", 7);
        $("#txtNuevaCuentaBancaria").attr("data-mask", "_______");
    }


    if (banco_seleccionado == 10) { //OTROS 17 DIGITOS


        $("#txtNuevaCuentaSinpe").inputmask("99999999999999999");
        $('#txtNuevaCuentaSinpe').attr("maxlength", 17);
        $("#txtNuevaCuentaSinpe").attr("data-mask", "_________________");
    }


    $("#txtNuevaCuentaBancaria").val(''); //reseteamos el valor de la cuenta de banco
    $("#txtNuevaCuentaSinpe").val('');
}

function validaNumericos(event) {
    if (event.charCode >= 48 && event.charCode <= 57) {

        return true;
    }
    else {
        alert('El campo permite solo digitos númericos');
        return false;
    }

}
//Alfredo José Vargas Seinfarth// Inicio 19/02/2019
function bancoSelectChangeEdit(val) {
    const easingSpeed = 600;
    val = parseInt(val);
    switch (val) {
        case 10://Otro
            {
                $("#lblEditCuentaBancaria").hide();
                $('#lblCuentaSinpe').show(easingSpeed);
                $("#txtEditCuentaBancaria").hide();
                $('#txtEditCuentaSinpe').show(easingSpeed);
                MostrarCampoCuenta();
                break;
            }
        case 21://Otro
            {
                $("#lblEditCuentaBancaria").hide();
                $('#lblCuentaSinpe').show(easingSpeed);
                $("#txtEditCuentaBancaria").hide();
                $('#txtEditCuentaSinpe').show(easingSpeed);
                MostrarCampoCuenta();
                break;
            }
        case 17://Otro
            {
                $("#lblEditCuentaBancaria").hide();
                $('#lblCuentaSinpe').show(easingSpeed);
                $("#txtEditCuentaBancaria").hide();
                $('#txtEditCuentaSinpe').show(easingSpeed);
                MostrarCampoCuenta();
                break;
            }
        case 20://Otro
            {
                $("#lblEditCuentaBancaria").hide();
                $('#lblCuentaSinpe').show(easingSpeed);
                $("#txtEditCuentaBancaria").hide();
                $('#txtEditCuentaSinpe').show(easingSpeed);
                MostrarCampoCuenta();
                break;
            }
        default: {
            $("#lblCuentaSinpe").hide(easingSpeed);
            $('#lblEditCuentaBancaria').show(easingSpeed);
            $("#txtEditCuentaSinpe").hide(easingSpeed);
            $('#txtEditCuentaBancaria').show(easingSpeed);
            MostrarCampoCuenta();
        }
    }

}
function bancoSelectChangeNuevo(val) {
    const easingSpeed = 600;
    val = parseInt(val);
    switch (val) {
        case 10://Otro
            {
                $("#lblnuevaCuentaBancaria").hide();
                $('#lblnuevaCuentaSinpe').show(easingSpeed);
                $("#txtNuevaCuentaBancaria").hide();
                $('#txtNuevaCuentaSinpe').show(easingSpeed);

                MostrarCampoCuentaNuevo();
                break;
            }
        case 21://Otro
            {
                $("#lblnuevaCuentaBancaria").hide();
                $('#lblnuevaCuentaSinpe').show(easingSpeed);
                $("#txtNuevaCuentaBancaria").hide();
                $('#txtNuevaCuentaSinpe').show(easingSpeed);

                MostrarCampoCuentaNuevo();
                break;
            }
        case 17://Otro
            {
                $("#lblnuevaCuentaBancaria").hide();
                $('#lblnuevaCuentaSinpe').show(easingSpeed);
                $("#txtNuevaCuentaBancaria").hide();
                $('#txtNuevaCuentaSinpe').show(easingSpeed);

                MostrarCampoCuentaNuevo();
                break;
            }
        case 20://Otro
            {
                $("#lblnuevaCuentaBancaria").hide();
                $('#lblnuevaCuentaSinpe').show(easingSpeed);
                $("#txtNuevaCuentaBancaria").hide();
                $('#txtNuevaCuentaSinpe').show(easingSpeed);

                MostrarCampoCuentaNuevo();
                break;
            }
        default: {
            $("#lblnuevaCuentaSinpe").hide(easingSpeed);
            $('#lblnuevaCuentaBancaria').show(easingSpeed);
            $("#txtNuevaCuentaSinpe").hide(easingSpeed);
            $('#txtNuevaCuentaBancaria').show(easingSpeed);
            MostrarCampoCuentaNuevo();
        }
    }

}

//Valdidation using jquery
function validate() {
    var isValid = true;
    if (!$('#ddlEditBanco').val()) {
        $('#ddlEditBanco').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlEditBanco').css('border-color', 'lightgrey');
    }
    if (!$('#ddlEditTipoCuenta').val()) {
        $('#ddlEditTipoCuenta').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlEditTipoCuenta').css('border-color', 'lightgrey');
    }
    if (!$('#ddlEditMoneda').val()) {
        $('#ddlEditMoneda').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlEditMoneda').css('border-color', 'lightgrey');
    }
    if ($('#ddlEditBanco option:selected').val() != 10 && $('#ddlEditBanco option:selected').val() != 21 && $('#ddlEditBanco option:selected').val() != 17 && $('#ddlEditBanco option:selected').val() != 20) {
        if ($('#txtEditCuentaBancaria').val().trim() == "") {
            $('#txtEditCuentaBancaria').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#txtEditCuentaBancaria').css('border-color', 'lightgrey');
        }
    } else {
        if ($('#txtEditCuentaSinpe').val().trim() == "") {
            $('#txtEditCuentaSinpe').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#txtEditCuentaSinpe').css('border-color', 'lightgrey');
        }
    }
    return isValid;
}

//Valdidation using jquery
function validateNuevo() {
    var isValid = true;
    if (!$('#ddlNuevoBanco').val()) {
        $('#ddlNuevoBanco').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoBanco').css('border-color', 'lightgrey');
    }
    if (!$('#ddlNuevoTipoCuenta').val()) {
        $('#ddlNuevoTipoCuenta').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoTipoCuenta').css('border-color', 'lightgrey');
    }
    if (!$('#ddlNuevoMoneda').val()) {
        $('#ddlNuevoMoneda').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlNuevoMoneda').css('border-color', 'lightgrey');
    }
    if ($('#ddlNuevoBanco option:selected').val() != 10 && $('#ddlNuevoBanco option:selected').val() != 21 && $('#ddlNuevoBanco option:selected').val() != 17 && $('#ddlNuevoBanco option:selected').val() != 20) {
        if ($('#txtNuevaCuentaBancaria').val().trim() == "") {
            $('#txtNuevaCuentaBancaria').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#txtNuevaCuentaBancaria').css('border-color', 'lightgrey');
        }
    }
    else {
        if ($('#txtNuevaCuentaSinpe').val().trim() == "") {
            $('#txtNuevaCuentaSinpe').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#txtNuevaCuentaSinpe').css('border-color', 'lightgrey');
        }
    }
    return isValid;
}
//Alfredo José Vargas Seinfarth// Final 19/02/2019

//metodo para cargar los pasos de la solicitud
function getPasosAgenteCuentaBancaria() {
    var empObj = {
        IdSolicitud: $("#txtSolicitud").val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/Colocacion/getPAsosAgente",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var colaIniciada = $("#ColaIniciada").val();
                if (result.length == 1 && result[0].Paso == '5000' && colaIniciada == '1') {
                    //cerramos la ventana de pasos
                    cerrarPanelPAsos();
                    buscarCola();
                }
                else {
                    var html = '';
                    //validamos el Idtipo para mostrar cuales tabs tienen pasos pendientes
                    validarPasosAgenteTabsNav(result);
                    $.each(result, function (key, item) {

                        if (item.BitCompleto == 1) {
                            html += '<tr class="fila-marcada">';
                            html += '<td class="paso-marcado"><a data-toggle="tab" href="#contentTab' + item.IdTipo + '" class="tabTipo-' + item.IdTipo + '" onclick="seleccionarTab(' + item.IdTipo + ')" >' + item.Descripcion + '</a></td>';
                            html += '<td><a id=paso-' + item.Paso + ' class="btn btn-marcado" onclick="cambiarBitCompleto(' + item.Paso + ')"><span class="glyphicon glyphicon-ok"></span></a></td>';
                            html += '</tr>';
                        }
                        else {
                            html += '<tr class="fila-pendiente">';
                            html += '<td class="paso-pendiente"><a data-toggle="tab" href="#contentTab' + item.IdTipo + '" class="tabTipo-' + item.IdTipo + '"onclick="seleccionarTab(' + item.IdTipo + ')" >' + item.Descripcion + '</a></td>';
                            html += '<td><a id=paso-' + item.Paso + ' class="btn btn-pendiente" onclick="cambiarBitCompleto(' + item.Paso + ')"><span class="glyphicon glyphicon-remove"></span></a></td>';
                            html += '</tr>';
                        }

                    });
                    $('#tblPasosXAgentes').html(html);
                    CargaCuentaBancaria();
                    conteoPendientes();
                    oculatarPasos();
                    seleccionarTabPendienteCuentaBancaria();
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function reiniciarTabs() {
    //Primero quitamos la clase active a todos los tabs.
    $('#ul-tabs-ventas li').removeClass('active');
    $('#ul-tabs-ventas li').removeClass('tab-activo');
    //$('.line').removeClass('linea-activada');
    $('.tab-colocacion .tab-pane').removeClass('active');
    //Agregamos la clase active al tab por default
    $('#ul-tabs-ventas:first-child li').first().addClass('in active');
    $('#contentTab1').addClass('in active');
}
function validarPasosAgenteTabsNav(result) {
    //removemos la clase de pendientes para luego pintar las pendientes
    $('#ul-tabs-ventas li').removeClass('tab-pendientes');
    $.each(result, function (key, item) {
        switch (item.IdTipo) {
            case 1:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab1').addClass('tab-pendientes');
                }
                break;
            case 2:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab2').addClass('tab-pendientes');
                }
                break;
            case 3:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab3').addClass('tab-pendientes');
                }
                break;
            case 4:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab4').addClass('tab-pendientes');
                }
                break;
            case 5:
                if (!item.BitCompleto) { //si bit completo es false
                    $('#tab5').addClass('tab-pendientes');
                }
                break;
            default:
                break;
        }
    });
}
function seleccionarTab(idTipo) {
    $('#ul-tabs-ventas li').removeClass('active');
    $('#tab' + idTipo).addClass('in active');
    //cerrarPanelPAsos();
}
function cerrarPanelPAsos() {
    $('#panel-acordion').find('.panel-acordion-content').slideToggle('fast', 'swing');
    $('#panel-acordion').toggleClass('ocultar');
    $('#panel-acordion').toggleClass('mostrar');
    $('#btn-cerrar-pasos').toggleClass('hide');
}
function seleccionarTabPendienteCuentaBancaria() {
    var clase = $("#tblPasosXAgentes:first td a").attr('class');
    var idTab = clase.replace('tabTipo-', '');
    //quitamos la clase active de los tabs y luego agregamos la clase active al tab correcto
    $('#ul-tabs-ventas li').removeClass('active');
    $('#tab' + idTab).addClass('in active');
    //removemos la clase active en los content tabs y luego agregamos la clase active en el content tab correcto
    $('.tab-colocacion.tab-pane.fade').removeClass('active');
    $('#contentTab' + idTab).addClass('in active');
    //agregar active a la info de ma cuentas bancarias
    $('#tabCuentaBanc').addClass('in active');
    //pintar tabs dependiendo del progreso o el tab activo
    var indiceTab = $('#tab' + idTab).index();
    $('.line').removeClass('linea-activada');
    $('.line:lt(' + indiceTab + ')').addClass('linea-activada');

    $('#ul-tabs-ventas li').removeClass('tab-activo');
    $('#ul-tabs-ventas li:lt(' + indiceTab + ')').addClass('tab-activo');
}
//Validamos si esta info antes de marcar un paso como completado en el dashboard de pasos
function verificarPasoModuloBancario(paso) {
    switch (paso) {
        case 9://Foto Cedula Frontal
            if ($('#UrlFotoCedula').attr('src') == '' || $('#UrlFotoCedula').attr('src') == null) {
                alertify.error("No Existe Foto de Cedula Frontal. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            else {
                return true;
            }
            break;
        case 10://Foto Cedula Trasera
            if ($('#UrlFotoCedulaTrasera').attr('src') == '' || $('#UrlFotoCedulaTrasera').attr('src') == null) {
                alertify.error("No Existe Foto de Cedula Trasera. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            else {
                return true;
            }
            break;
        case 11://Foto selfie
            if ($('#UrlFotoSelfie').attr('src') == '' || $('#UrlFotoSelfie').attr('src') == null) {
                alertify.error("No Existe Foto de Selfie. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            else {
                return true;
            }
            break;
        case 14://Foto selfie
            if ($('#UrlFotoFirma').attr('src') == '' || $('#UrlFotoFirma').attr('src') == null) {
                alertify.error("No Existe Foto de la Firma. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            else {
                return true;
            }
            break;
        case 15://Solicita Referencias
            if (
                (
                    ($('#ddlTipoReferencia').val() != "-1" || $('#ddlTipoReferencia').val() != "0") &&
                    ($('#txtNombreRefFamiliar').val() != "") &&
                    ($('#txtTelefonoRefFamiliar').val() != "" || $('#txtTelefonoRefFamiliar').val() != "0")
                ) ||
                (
                    ($('#txtNombreRefEmpresa').val() != "") &&
                    ($('#txtTelefonoRefEmpresa').val() != "" || $('#txtTelefonoRefFamiliar').val() != "0") &&
                    ($('#txtNombreRefSupervisor').val() != "")
                ) ||
                (
                    ($('#txtNombreRefPersonal').val() != "") &&
                    ($('#txtTelefonoRefPersonal').val() != "" || $('#txtTelefonoRefPersonal').val() != "0")
                )
            ) {
                return true;
            }
            else {
                alertify.error("No Existe Ninguna Referencia Agregada. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            break;
        case 12://Solicita Cuenta Bancaria
            if ($('#cuenta-simpe').text() == '' || $('#cuenta-simpe').text() == null) {
                alertify.error("No Existe Ninguna Cuenta Bancaria. No Puede Marcar Este Paso como Completo.");
                return false;
            }
            else {
                return true;
            }
            break;
        default:
            alertify.error("No Existe Ninguna Validacion Para el Paso: " + paso);
            return true;
            break;
    }
}
function mostrarImagenSinpe(Identificacion) {
    var empObj = {
        Identificacion: Identificacion
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/CuentaBancaria/GetImagenSinpe",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result.Mensaje != "Completo") {
                    alertify.error(result.Mensaje);
                } else {
                    $("#imagen-sinpe img").attr("src", result.UrlImagenSinpe);
                    $('#ImagenSinpe').modal('show');
                }
            },
            error: function (errormessage) {
                alertify.error(result.Mensaje);
            }
        });
}