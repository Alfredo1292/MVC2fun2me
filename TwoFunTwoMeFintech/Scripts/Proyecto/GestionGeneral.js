//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
    $('body').css("background-image", "none");
	//$('#example').dataTable();
});

function loadData() {

	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$.ajax({
		url: "/GestionGeneral/Detalle",
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			var html = '';
			$.each(result.ListTipos, function (key, item) {

				$("#ddlTipoGestion").append(new Option(item.Descripcion, item.Id));
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	return false;
}

function Busqueda() {
	var empObj = {
		Identificacion: $('#txtBusquedaSolicitud').val()
	};
	$('#SolicitudesTablebody').html();
	$('#GestionesGeneralBody').html();
	$('#GestionesAprobadasBody').html();
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');
	processing: true; // for show progress bar  
	serverSide: true; // for process server side  
	filter: true; // this is for disable filter (search box)  
	orderMulti: false; // for disable multiple column at once
	paging: false;

	$.ajax({
		processing: true, // for show progress bar
		serverSide: true, // for process server side
		filter: true, // this is for disable filter (search box)
		orderMulti: false, // for disable multiple column at once
		url: "/GestionGeneral/Buscar",
		type: "POST",
		data: JSON.stringify(empObj),
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data.ListSolicitudes, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.Id + '</td>';
				html += '<td>' + item.Cliente + '</td>';
				html += '<td>' + item.NombreProducto + '</td>';
				html += '<td>' + item.Monto + '</td>';
				if (data.IdAgente == 1) {
					html += '<td width=470px;"><a class="btn btn-primary"  href="#" onclick="GuardarGestion(' + item.Id + ')">Guardar Gestión</a> | <a class="btn btn-primary"  href="#" onclick="VerAprobaciones(' + item.Id + ')">Ver Aprobaciones</a> | <a class="btn btn-primary"  href="#" onclick="VerGestiones(' + item.Id + ')">Ver Gestiones</a></td>';					
				}
				else {
					html += '<td width="300px;"><a class="btn btn-primary"  href="#" onclick="GuardarGestion(' + item.Id + ')">Guardar Gestión</a> | <a class="btn btn-primary"  href="#" onclick="VerAprobaciones(' + item.Id + ')">Ver Aprobaciones</a></td>';
				}
				
				html += '</tr>';
			});
			$('#SolicitudesTable').dataTable().fnClearTable();
			$('#SolicitudesTable').dataTable().fnDestroy();

			$('#SolicitudesTablebody').html(html);

			$("#SolicitudesTable").DataTable();
			$("select[name*='SolicitudesTable_length']").change();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});

}
	
function AprobarGestion(ID) {
	var res = validate();
	if (res == false) {
		return false;
	}
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');
	var empObj = {
		IdSolicitud: ID,
		Detalle : $('#txtDetalle').val()		
	};
	$.ajax({
		url: "/GestionGeneral/Aprobar",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {						
			$('#txtFechaGestion').html();
			//$('#txtBusquedaSolicitud').val("");
			//$('#ddlTipoGestion').html("");
			$('#GestionesGeneralBody').html();		
			$('#GestionesAprobadasBody').html();
			$('#myModalGestiones').modal('hide');
			$('#myModalGestionesAprobadas').modal('hide');
			$('#txtDetalle').val("");

			alertify.alert("Id: " + ID + " Se aprobó correctamente la gestión!", function () {
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
				//location.reload();
				Busqueda();
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

function GuardarGestion(ID) {
	var res = validate();
	if (res == false) {
		return false;
	}
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');

	var empObj = {
		IdSolicitud: ID,
		Detalle: $('#txtDetalle').val(),
		FechaGestion: $('#txtFechaGestion').val(),
		TipoGestion: $('#ddlTipoGestion option:selected').val()
	};
	$.ajax({
		url: "/GestionGeneral/guardar",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			$('#txtFechaGestion').html();
			//$('#txtBusquedaSolicitud').val(""); 
			
			$('#txtDetalle').val("");
			$('#GestionesGeneralBody').html();
			$('#GestionesAprobadasBody').html(); 
			alertify.alert("Id: " + ID + " Se guardó correctamente la gestión!", function () {
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
				//location.reload();
				//loadData;
				Busqueda();
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}


function AprobarGestion(ID) {
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');
	var empObj = {
		IdGestionGeneral: ID,
		Detalle: $('#txtDetalle').val()
	};
	$.ajax({
		url: "/GestionGeneral/Aprobar",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			$('#myModalGestiones').modal('hide');
			$('#myModalGestionesAprobadas').modal('hide');
			$('#txtFechaGestion').html();
			$('#ddlTipoGestion option:selected').val('0');
			$('#txtDetalle').val("");
			$('#GestionesGeneralBody').html();
			$('#GestionesAprobadasBody').html();
			alertify.alert("Id: " + ID + " Se aprobó correctamente la gestión!", function () {
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
				//location.reload();
				Busqueda();
			});
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

function VerGestiones(id) {
	var empObj = {
		IdSolicitud: id,
		Detalle: ""
	};	
	$('#GestionesGeneralBody').html();
	$('#GestionesAprobadasBody').html(); 
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');
	processing: true; // for show progress bar  
	serverSide: true; // for process server side  
	filter: true; // this is for disable filter (search box)  
	orderMulti: false; // for disable multiple column at once
	paging: false;

	$.ajax({
		processing: true, // for show progress bar
		serverSide: true, // for process server side
		filter: true, // this is for disable filter (search box)
		orderMulti: false, // for disable multiple column at once
		url: "/GestionGeneral/VerGestiones",
		type: "POST",
		data: JSON.stringify(empObj),
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.IdGestionGeneral + '</td>';
				html += '<td>' + item.IdSolicitud + '</td>';
				html += '<td>' + item.nombre + '</td>';
				html += '<td>' + item.Detalle + '</td>';
				html += '<td>' + item.DesTipoGestion + '</td>';
				html += '<td><a href="#" class="btn btn-primary" onclick="return AprobarGestion(' + item.IdGestionGeneral + ')"> Aprobar </a> | <a href="#" class="btn btn-primary" onclick="return DeleleGestion(' + item.IdGestionGeneral + ')"> Eliminar</a></td>';
				html += '</tr>';
			});
								
			$('#GestionesTable').dataTable().fnClearTable();
			$('#GestionesTable').dataTable().fnDestroy();
			$('#GestionesTable').append(html);

			$('#myModalGestiones').modal('show');
			
			$("#GestionesTable").DataTable();
			$("select[name*='GestionesTable_length']").change();
			Busqueda();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});

}

function VerAprobaciones(id) {
	var empObj = {
		IdSolicitud: id,
		Detalle: ""
	};
	
	$('#txtFechaGestion').css('border-color', 'lightgrey');
	$('#txtBusquedaSolicitud').css('border-color', 'lightgrey');
	$('#ddlTipoGestion').css('border-color', 'lightgrey');
	$('#txtDetalle').css('border-color', 'lightgrey');
	processing: true; // for show progress bar  
	serverSide: true; // for process server side  
	filter: true; // this is for disable filter (search box)  
	orderMulti: false; // for disable multiple column at once
	paging: false;

	$.ajax({
		processing: true, // for show progress bar
		serverSide: true, // for process server side
		filter: true, // this is for disable filter (search box)
		orderMulti: false, // for disable multiple column at once
		url: "/GestionGeneral/VerGestionesAprobadas",
		type: "POST",
		data: JSON.stringify(empObj),
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.IdGestionGeneral + '</td>';
				html += '<td>' + item.IdSolicitud + '</td>';
				html += '<td>' + item.nombre + '</td>';
				html += '<td>' + item.Detalle + '</td>';
				html += '<td><a href="#" class="btn btn-primary" onclick="return DeleleGestionAprobada(' + item.IdGestionGeneral + ')"> Eliminar</a></td>';
				html += '</tr>';
			});			
			$('#GestionesAprobadasTable').dataTable().fnClearTable();
			$('#GestionesAprobadasTable').dataTable().fnDestroy();

			$('#GestionesAprobadasBody').append(html);
			$('#myModalGestionesAprobadas').modal('show');

			$("#GestionesAprobadasTable").DataTable();
			$("select[name*='GestionesAprobadasTable_length']").change();
			Busqueda();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});

}
function validate() {
	var isValid = true;
	if ($('#txtFechaGestion').val().trim() == "") {
		$('#txtFechaGestion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtFechaGestion').css('border-color', 'lightgrey');
	}
	if ($('#txtDetalle').val().trim() == "") {
		$('#txtDetalle').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtDetalle').css('border-color', 'lightgrey');
	}
	if ($('#ddlTipoGestion option:selected').val() == 0) {
		$('#ddlTipoGestion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlTipoGestion').css('border-color', 'lightgrey');
	}
	return isValid;
}
//function for deleting employee's record
function DeleleGestion(ID) {


	alertify.confirm("¿Desea Continuar?",
		function () {
			$.ajax(
				{
					url: "/GestionGeneral/EliminarGestion",
					type: "POST",
					data: {
						ID
					},
					success: function (result) {
						alertify
							.alert(result.Mensaje);
						$('#GestionesGeneralBody').html();
						$('#GestionesAprobadasBody').html();
						$('#myModalGestiones').modal('hide');
						$('#myModalGestionesAprobadas').modal('hide');
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
//function for deleting employee's record
function DeleleGestionAprobada(ID) {


	alertify.confirm("¿Desea Continuar?",
		function () {
			$.ajax(
				{
					url: "/GestionGeneral/EliminarGestionAprobada",
					type: "POST",
					data: {
						ID
					},
					success: function (result) {
						alertify
							.alert(result.Mensaje);
						$('#GestionesGeneralBody').html();
						$('#GestionesAprobadasBody').html();
						$('#myModalGestiones').modal('hide');
						$('#myModalGestionesAprobadas').modal('hide');
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
