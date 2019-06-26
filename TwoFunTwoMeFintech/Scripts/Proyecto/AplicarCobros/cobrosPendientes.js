//variables de objetos de cobros
var ObjecCobroBCR = null;
var ObjecCobroBAC = null;
var ObjecCobroBN = null;

$(document).ready(function () {
    //LLamada a los metodos para llenar las tablass
    getCuentasCobros();
    //metodo del boton buscar
    $('#btnBuscar').on('click', function () {
        getCobrosPendientesFiltrado();
    });
    //metodo del boton de subir los archivos
    $('#btnSubirArchivos').on('click', function () {
        if (ObjecCobroBCR != null && ObjecCobroBAC != null && ObjecCobroBN != null) {
            guardarArchivos();
        }
        else {
            alert("No se han Seleccionado Todos los Archivos");
        }
    });
    //Metodos temporales para procesar las solicitudes
    $('#btnCorrerArchivos').on('click', function () {
        BuscarNombresArchivos();
    });
    $('#btnProcesarArchivos').on('click', function () {
        procesarArchivos();
    });
    //metodo para seleccionar la carpeta de los archivos para subir
    seleccionarCarpetaBCR();
    seleccionarCarpetaBAC();
    seleccionarCarpetaBN();
    seleccionarMultiple();
});
function getCuentasCobros() {
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AplicarCobros/GetCuentasCobros",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += '<tr>';
                    html += '<td>' + item.ReferenciaBancaria + '</td>';
                    html += '<td id="idBanco-' + item.ReferenciaBancaria+'">' + item.IdBanco + '</td>';
                    html += '<td>' + item.BancoTransaccion + '</td>';
                    html += '<td>' + item.FechaTransaccion + '</td>';
                    html += '<td>' + item.DescripcionTrans + '</td>';
                    html += '<td>' + item.MontoCredito + '</td>';
                    html += '<td>' + '<input style="width:100%;" id="txtNumeroCredito-' + item.ReferenciaBancaria +'" class="hide form-control">' + '</td>';
                    html += '<td id="filaAcciones">' + '<form id="form-editar-' + item.ReferenciaBancaria + '" class="form-inline"><button type="button" id="btnEditar-' + item.ReferenciaBancaria + '" class="btn btn-primary btn-primary-editar" onClick="editar(\'' + item.ReferenciaBancaria + '\')"><span class="glyphicon glyphicon-pencil"></span>  Editar</button></form>' + '<form id="form-acciones-' + item.ReferenciaBancaria + '" class="hide form-inline"><button style="margin-right:5px;" type="button" id="btnGuardar-' + item.ReferenciaBancaria + '" class="btn btn-primary btn-primary-asignar" onClick="guardar(\'' + item.ReferenciaBancaria + '\')"><span class="glyphicon glyphicon-ok"></span><span class="tooltiptexMenu">Guardar</span></button><button type="button" id="btnCancelar-' + item.ReferenciaBancaria + '" class="btn btn-primary btn-primary-eliminar" onClick="cancelar(\'' + item.ReferenciaBancaria +'\')"><span class="glyphicon glyphicon-remove"></span><span class="tooltiptexMenu">Cancelar</span></button></form>' + '</td>';
                    html += '</tr>';
                });
                $("#tableCuentaCobros").dataTable().fnClearTable();
                $("#tableCuentaCobros").dataTable().fnDestroy();
                $('#tblCuentaCobros').html(html);

                $("#tableCuentaCobros").dataTable({
                    "order": [[3, "desc"]]
                });
                $("select[name*='tblCuentaCobros_length']").change();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function editar(referencia) {
    $('#form-editar-' + referencia).addClass('hide');
    $('#form-acciones-' + referencia).removeClass('hide');
    $('#txtNumeroCredito-' + referencia).removeClass('hide');
    $('#txtNumeroCredito-' + referencia).val('');
}
function guardar(referencia) {

    var empObj = {
        Referencia: referencia,
        IdBanco: $('#idBanco-' + referencia).text(),
        IdCredito: $('#txtNumeroCredito-' + referencia).val()
    };
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AplicarCobros/GuardarCuenta",
            type: "POST",
            data: JSON.stringify(empObj),
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                $('#form-editar-' + referencia).removeClass('hide');
                $('#form-acciones-' + referencia).addClass('hide');
                $('#txtNumeroCredito-' + referencia).addClass('hide');
                getCuentasCobros();
                alertify.success(result);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function cancelar(referencia) {
    $('#form-editar-' + referencia).removeClass('hide');
    $('#form-acciones-' + referencia).addClass('hide');
    $('#txtNumeroCredito-' + referencia).addClass('hide');
    $('#txtNumeroCredito-' + referencia).val('');
}
function seleccionarCarpetaBCR() {
    $(".input-folder-cobros-BCR").before(
        function () {
            if (!$(this).prev().hasClass('seleccionar-carpeta-BCR')) {
                var element = $("<input id='folder-cobros-BCR' onchange='handleCobrosBCR(this);' type='file' class='seleccionar-carpeta' multiple style='visibility:hidden; height:0'>");
                element.change(function () {
                    $('#txtArchivoBCR').val((element.val()).split('\\').pop());
                });
                $(this).find("#btnExaminarBCR").click(function () {
                    element.click();
                });
                return element;
            }
        }
    );
}
function seleccionarCarpetaBAC() {
    $(".input-folder-cobros-BAC").before(
        function () {
            if (!$(this).prev().hasClass('seleccionar-carpeta-BAC')) {
                var element = $("<input id='folder-cobros-BAC' onchange='handleCobrosBAC(this);' type='file' class='seleccionar-carpeta' style='visibility:hidden; height:0'>");
                element.change(function () {
                    $('#txtArchivoBAC').val((element.val()).split('\\').pop());
                });
                $(this).find("#btnExaminarBAC").click(function () {
                    element.click();
                });
                return element;
            }
        }
    );
}
function seleccionarCarpetaBN() {
    $(".input-folder-cobros-BN").before(
        function () {
            if (!$(this).prev().hasClass('seleccionar-carpeta-BN')) {
                var element = $("<input id='folder-cobros-BN' onchange='handleCobrosBN(this);' type='file' class='seleccionar-carpeta' style='visibility:hidden; height:0'>");
                element.change(function () {
                    $('#txtArchivoBN').val((element.val()).split('\\').pop());
                });
                $(this).find("#btnExaminarBN").click(function () {
                    element.click();
                });
                return element;
            }
        }
    );
}
function seleccionarMultiple() {
    $(".input-folder-cobros").before(
        function () {
            if (!$(this).prev().hasClass('seleccionar-archivos-multiple')) {
                var element = $("<input id='archivos-cobros' onchange='handleCobrosTotal(this);' type='file' name='files[]' class='seleccionar-archivos-multiple' multiple style='visibility:hidden; height:0'>");

                $(this).find("#btnExaminarMultiple").click(function () {
                    element.click();
                });
                return element;
            }
        }
    );
}
function handleCobrosBCR(evt) {
    ObjecCobroBCR = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecCobroBCR = { Base64: base64String, Name: f.name, Type: f.type}
        };
    })(f);
    reader.readAsBinaryString(f);
}
function handleCobrosBAC(evt) {
    ObjecCobroBAC = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecCobroBAC = { Base64: base64String, Name: f.name, Type: f.type }
        };
    })(f);
    reader.readAsBinaryString(f);
}
function handleCobrosBN(evt) {
    ObjecCobroBN = null
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecCobroBN = { Base64: base64String, Name: f.name, Type: f.type }
        };
    })(f);
    reader.readAsBinaryString(f);
}
//metodo para selecionar y procesar los trs archivos de cobros
function handleCobrosTotal(evt) {
    ObjecCobroBCR = null;
    ObjecCobroBAC = null;
    ObjecCobroBN = null;

    //llenamos los text box con los nombres de los archivos
    for (var i = 0; i < evt.files.length; i++ ){
        switch (evt.files[i].name) {
            case 'ECBCR.xlsx':
                $('#txtArchivoBCR').val((evt.files[i].name));
                handleCobrosBCR(evt, i)
                break;
            case 'ECBCR.xlsx':
                $('#txtArchivoBAC').val((evt.files[i].name));
                break;
            case 'ECBCR.xlsx':
                $('#txtArchivoBN').val((evt.files[i].name));
                break;
            default:
                alert("El Archivo: " + evt.files[i].name +" NO puede ser procesado pues el nombre no pertenece a ninguno valido");
                break;
        }
    }
    //procesamos los archivos como un objeto
    var f = $(evt)[0].files[0];
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            var base64String = window.btoa(binaryData);
            ObjecCobroBCR = { Base64: base64String, Name: evt.files[0].name, Type: evt.files[0].type }
            ObjecCobroBAC = { Base64: base64String, Name: evt.files[1].name, Type: evt.files[0].type }
            ObjecCobroBN = { Base64: base64String, Name: evt.files[2].name, Type: evt.files[0].type }
        };
    })(f);
    reader.readAsBinaryString(f);
}   
//Metodo para guardar los files al servidor
function guardarArchivos(){
    var listaObjetos = [ObjecCobroBCR, ObjecCobroBAC, ObjecCobroBN];
    //guardarGestion('120', 'Modificación de cedula Frontal', '');
    $.ajax({
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        url: "/AplicarCobros/SubirArchivosCobros",
        type: "POST",
        data: JSON.stringify(listaObjetos),
        contentType: "application/json;charset=utf-8",
        dataType: "Json",
        success: function (result) {
            $('#txtArchivoBCR').val('');
            $('#txtArchivoBAC').val('');
            $('#txtArchivoBN').val('');
            alert(result);
        },
        error: function (errormessage) {
            alertify.error(errormessage.responseText);
        }
    });
}
//metodos para extraer nombres de los archivos
function BuscarNombresArchivos() {
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AplicarCobros/BuscarNombreArchivos",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                alert(result);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
function procesarArchivos() {
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax(
        {
            url: "/AplicarCobros/ProcesarDocumentos",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                alert(result);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
