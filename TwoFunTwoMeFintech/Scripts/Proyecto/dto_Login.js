$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    e.target // newly activated tab
    e.relatedTarget // previous active tab
    if (!e.target.innerHTML == 'Nuevo')
        $('#fisrtTab').css("display", "block");
    else $('#fisrtTab').css("display", "none");
    if (e.target.innerHTML == 'Usuarios')
        $('#fisrtTab').css("display", "block");
});


//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
    $('body').css("background-image", "none");
    //$('#example').dataTable();
});

//Load Data function
function loadData() {
	$('#UserTable').html();
    processing: true; // for show progress bar  
    serverSide: true; // for process server side  
    filter: true; // this is for disable filter (search box)  
    orderMulti: false; // for disable multiple column at once
    paging: false;
    $.ajax({
        url: "/User/Detalle",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = '';
            $.each(data, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.cod_agente + '</td>';
                html += '<td>' + item.nombre + '</td>';
                html += '<td>' + item.correo + '</td>';
                html += '<td>' + item.estado + '</td>';
                html += '<td>' + item.ROLES + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.cod_agente + ')"> Editar </a> | <a class="btn btn-primary btn-primary-asignar"  href="#" onclick="getbyIDAsignarMenu(' + item.cod_agente + ')">Asign Menú</a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.cod_agente + ')">Eliminar</a></td>';
                html += '</tr>';
            });

			$('#UserTable').dataTable().fnClearTable();
			$('#UserTable').dataTable().fnDestroy();

            $('.tbody').html(html);

            $("#UserTable").DataTable();
            $("select[name*='UserTable_length']").change();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //	$("#UserTable").DataTable();
}



//Function for getting the Data Based upon Employee ID
function getbyID(id) {
    $('#cod_agente').css('border-color', 'lightgrey');
    $('#nombre').css('border-color', 'lightgrey');
    $('#correo').css('border-color', 'lightgrey');
    $('#estado').css('border-color', 'lightgrey');
    $('#ddlRolId').css('border-color', 'lightgrey');
    $('#idEditarAgente strong').text("Editar Agente: " + id);
    $.ajax({
        url: "/User/Edit/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = '';           
			$("#ddlRolId").html('');
            $.each(result.ListRoles, function (key, item) {
                $("#ddlRolId").append(new Option(item.ROLES, item.ID));
            });
            $("#ddlAgencia").html('');
            $.each(result.ListaAgencias, function (key, item) {
                $("#ddlAgencia").append(new Option(item.Nombre, item.IdT_AgenciasExternas));
            });
            //agregar item al dropdown de tipo asignacion
			$("#ddAsignacion").html('');
			$("#ddAsignacion").append(new Option('--Seleccione--', 0));
            $("#ddAsignacion").append(new Option('Ninguno', 1));
            $("#ddAsignacion").append(new Option('Ventas', 2));
            $("#ddAsignacion").append(new Option('Cobros', 3));

			$.each(result.listadoDto_login, function (key, item) {
				$('#cod_agente').val(item.cod_agente);
				$('#nombre').val(item.nombre);
				$('#correo').val(item.correo);
				$('#estado').val(item.estado);
                $('#ddlRolId').val(item.ROLID);
                $('#ddlAgencia').val(item.IdAgencia);
                $('#ddAsignacion').val(item.TipoCola);
				$('#myModal').modal('show');
				$('#btnUpdate').show();
				$('#btnAdd').hide();
			});

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//function for updating employee's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        cod_agente: $('#cod_agente').val(),
        nombre: $('#nombre').val(),
        correo: $('#correo').val(),
        estado: $('#estado').val(),
        ROLID: $('#ddlRolId option:selected').val(),
        TipoCola: $('#ddAsignacion').val(),
        IdAgencia: $('#ddlAgencia').val()
    };
    $.ajax({
        url: "/User/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //loadData();
            $('#myModal').modal('hide');
            $('#cod_agente').val("");
            $('#estado').val("");
            $('#nombre').val("");
            $('#correo').val("");
			$('#ROLID').val("");

			alertify.alert("Id: " + $('#cod_agente').val() + " Se actualizo", function () {
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
				$('#IdRule').val("");
				loadData();
			});
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for create employee's record
function Crear() {
	var res =  validateCreate();
	if (res == false) {
		return false;
	}
	var empObj = {
		cod_agente: $('#txtcod_agente').val(),
		nombre: $('#txtnombre').val(),
		correo: $('#txtCorreo').val(),
        ROLID: $('#ddlRoles option:selected').val(),
        TipoCola: $('#ddlAsignacion option:selected').val(),
		pass: $('#txtpass').val(),
		ConfirmPassword: $('#txtConfirmPassword').val(),
        IdAgencia: $('#ddlAgenciasExternas').val()
	};
	$.ajax({
		url: "/User/Crear",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
		
			$('#myModal').modal('hide');
			$('#txtcod_agente').val('');
			$('#txtnombre').val('');
			$('#txtCorreo').val('');
            $('#ddlRoles').val();
            $('#ddlAgenciasExternas').val();
			$('#txtpass').val('');
			$('#txtConfirmPassword').val('');
			alertify.alert("Se ha creado el usuario" + $('#txtnombre').val(), function () {
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
				$('#txtnombre').val("");
				loadData();
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}
//function for deleting employee's record
function Delele(ID) {


    alertify.confirm("¿Desea Continuar?",
        function () {
            $.ajax(
                {
                    url: "/User/Eliminar",
                    type: "POST",
                    data: {
                        ID
                    },
                    success: function (result) {
                        alertify
                            .alert(result.Mensaje);
						$('#UserTable').html();
						loadData();
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = p3;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).message;
                        alert(err);
                    }
                });
        },
        function () {
            alertify.error('Cancel');
        });




}
//Function for clearing the textboxes
function clearTextBox() {
    $('#cod_agente').val("");
    $('#estado').val("");
    $('#nombre').val("");
    $('#correo').val("");
	$('#ddlRolId').html("");
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
	if (!$('#ddlRolId').val()) {
		$('#ddlRolId').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlRolId').css('border-color', 'lightgrey');
	}
    return isValid;
}

function validateCreate() {
	var isValid = true;
	if ($('#txtcod_agente').val().trim() == "") {
		$('#txtcod_agente').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtcod_agente').css('border-color', 'lightgrey');
	}
	if ($('#txtnombre').val().trim() == "") {
		$('#txtnombre').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtnombre').css('border-color', 'lightgrey');
	}
	if ($('#txtCorreo').val().trim() == "") {
		$('#txtCorreo').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtCorreo').css('border-color', 'lightgrey');
	}
	if (!$('#ddlRoles').val()) {
		$('#ddlRoles').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlRoles').css('border-color', 'lightgrey');
	}
	if ($('#txtpass').val().trim() == "") {
		$('#txtpass').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtpass').css('border-color', 'lightgrey');
	}
	if ($('#txtConfirmPassword').val().trim() == "") {
		$('#txtConfirmPassword').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtConfirmPassword').css('border-color', 'lightgrey');
	}
	return isValid;
}

function getbyIDAsignarMenu(id) {
	//$('#myModalAsigUsu').modal('show');
	$.ajax({
		url: "/AgenteMenu/Detalle/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
            var role = "";
			$.each(result.Listdto_Login, function (key, item) {
				$('#lblCOD_AGENTEtex').text(item.cod_agente);
				$('#lblnombre').text(item.nombre);
				$('#myModalAsigUsu').modal('show');
				$('#btnActualiza').show();
				$('#btnAdd').hide();
			});
            $.each(result.ListAgenteMenuAsig, function (key, item) {
                //vamos a validar el IdRole
                switch (item.IdRole) {
                    case 1:
                        role = 'Administrador';
                        break;
                    case 2:
                        role = 'Ventas';
                        break;
                    case 3:
                        role = 'Cobros';
                        break;
                    case 4:
                        role = 'Tesoreria';
                        break;
                    default:
                        role = 'NULL';
                        break;
                }
                $('#listAsignado').append('');
                $('#listAsignado').append('<a href="#" class="list-group-item" data-value="' + item.ID + '">' + item.SUBMENU + '<input type="checkbox" class="pull-right"><span class="tooltiptexMenu">Role: ' + role + '</span></a>');
			});
            $.each(result.ListAgenteMenuDesAsig, function (key, item) {
                switch (item.IdRole) {
                    case 1:
                        role = 'Administrador';
                        break;
                    case 2:
                        role = 'Ventas';
                        break;
                    case 3:
                        role = 'Cobros';
                        break;
                    case 4:
                        role = 'Tesoreria';
                        break;
                    default:
                        role = 'NULL';
                        break;
                }
                $('#listAsignado').append('');
                $('#listDesAsignado').append('<a href="#" class="list-group-item" data-value="' + item.ID + '">' + item.SUBMENU + '<input type="checkbox" class="pull-right"><span class="tooltiptexMenu">Role: ' + role + '</span></a>');
				
			});

			
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	return false;
}
//Actualizar Menú
function Actualizar() {

	var Class_listaAsignado = [];
		//Get the items
	$('#listAsignado a').each(function (index, value) {		
		   let AgenteMenu = {};
		    AgenteMenu.IDSUBMENU = $(value).data('value');
		    AgenteMenu.COD_AGENTE = $('#lblCOD_AGENTEtex').text();
		    AgenteMenu.SUBMENU = $(value).text();
		    AgenteMenu.ACCION = 'INSERTAR';
	     	Class_listaAsignado.push(AgenteMenu);
	});// Select first tab

	//Get the items
	$('#listDesAsignado a').each(function (index, value) {
		let AgenteMenu = {};
		AgenteMenu.IDSUBMENU = $(value).data('value');
		AgenteMenu.COD_AGENTE = $('#lblCOD_AGENTEtex').text();
		AgenteMenu.SUBMENU = $(value).text();
		AgenteMenu.ACCION = 'ELIMINAR';
		Class_listaAsignado.push(AgenteMenu);
	});// Select first tab

	$.ajax({
		url: "/AgenteMenu/Asignar",
		data: JSON.stringify({ 'listAsignado': Class_listaAsignado }), 
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			//loadData();
			$('#myModal').modal('hide');
			$('#lblnombre').val("");

			alertify.alert("Id: " + $('#lblCOD_AGENTEtex').text() + " Se actualizo", function () {
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
				$('#lblCOD_AGENTEtex').text("")
				//location.reload();
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

$('.add').click(function () {
	$('.all').prop("checked", false);
	var items = $("#listDesAsignado input:checked:not('.all')");
	var n = items.length;
	if (n > 0) {
		items.each(function (idx, item) {
			var choice = $(item);
			choice.prop("checked", false);
			choice.parent().appendTo("#listAsignado");
			$('#myModalAsigUsu').modal('show');
		});
	
	}
	else {
		alert("Choose an item from list 1");
	}
});

$('.remove').click(function () {
	$('.all').prop("checked", false);
	var items = $("#listAsignado input:checked:not('.all')");
	items.each(function (idx, item) {
		var choice = $(item);
		choice.prop("checked", false);
		choice.parent().appendTo("#listDesAsignado");
	});
});

/* toggle all checkboxes in group */
$('.all').click(function (e) {
	e.stopPropagation();
	var $this = $(this);
	if ($this.is(":checked")) {
		$this.parents('.list-group').find("[type=checkbox]").prop("checked", true);
	}
	else {
		$this.parents('.list-group').find("[type=checkbox]").prop("checked", false);
		$this.prop("checked", false);
	}
});

$('[type=checkbox]').click(function (e) {
	e.stopPropagation();
	$('#myModalAsigUsu').modal('show');
});

/* toggle checkbox when list group item is clicked */
$('.list-group a').click(function (e) {

	e.stopPropagation();

	var $this = $(this).find("[type=checkbox]");
	if ($this.is(":checked")) {
		$this.prop("checked", false);
	}
	else {
		$this.prop("checked", true);
	}

	if ($this.hasClass("all")) {
		$this.trigger('click');
	}
});

function clearMODAL2() {
	$('#lblCOD_AGENTEtex').html("");
	$('#lblnombre').html("");
	$('#listDesAsignado').html("");
	$('#listAsignado').html("");
	$('#btnActualiza').hide();
	$('#btnAdd').show();
	//location.reload();
}
