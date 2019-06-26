//Load Data in Table when documents is ready
$(document).ready(function () {

	cargarMontosProductos();
	//loadData();
	//$('body').css("background-image", "none");
	//$('#example').dataTable();
	//ocultarBloqueo();
});
//Load Data function
function loadData() {

	var Id = $('#ddlMontoCredito').val();
	$('.Producto').html('');
	processing: true; // for show progress bar  
	serverSide: true; // for process server side  
	filter: true; // this is for disable filter (search box)  
	orderMulti: false; // for disable multiple column at once
	paging: false;
	$.ajax({
		url: "/Producto/ListarProductos/" + Id,
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.Id + '</td>';
				html += '<td>' + item.NombreProducto + '</td>';
				html += '<td>' + item.MontoProducto + '</td>';
				html += '<td>' + item.PlazoDias + '</td>';
				html += '<td>' + item.Frecuencia + '</td>';
				html += '<td>' + item.FrecuenciaDias + '</td>';
				html += '<td>' + item.CantidadCuotas + '</td>';
				html += '<td>' + item.Originacion + '</td>';
				html += '<td>' + item.Tasa + '</td>';
				html += '<td>' + item.Tasadiaria + '</td>';
				html += '<td>' + item.Cuota + '</td>';
				html += '<td>' + item.Originacion2 + '</td>';
				html += '<td>' + item.Monto + '</td>';
				html += '<td>' + item.DiasMora + '</td>';
				html += '<td>' + item.Activo + '</td>';
				html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getProducto(' + item.Id + ')"> Editar </a></td>';
				html += '</tr>';
			});

			$('.Producto').html(html);

			$("#ProductosTable").DataTable();
			$("select[name*='UserTable_length']").change();
			ocultarBloqueo();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
			ocultarBloqueo();
		}
	});

}
function cargarMontosProductos() {
	$.ajax({
		url: "/Producto/CargaMontosProductos",
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			$("#ddlMontoCredito").html("");
			$.each(result, function (key, item) {
				$("#ddlMontoCredito").append(new Option(item.Descripcion, item.Monto));
			});
		},
		error: function (errormessage) {
			ocultarBloqueo();
			alertify.error("llenarComboboxProducto\n" + errormessage.responseText);
		}
	});
}

function CrearProducto() {

	var res = validateNuevo();
	if (res == false) {
		return;
	}

	var empObj = {
		NombreProducto: $("#txtNuevoNombreProducto").val(),
		MontoProducto: $("#txtNuevoMontoProducto").val(),
		PlazoDias: $("#txtNuevoPlazoDias").val(),
		Frecuencia: $("#ddlNuevoFrecuencia option:selected").val(),
		FrecuenciaDias: $("#txtnuevaFrecuenciaDias").val(),
		CantidadCuotas: $("#txtNuevaCantidadCuotas").val(),
		Originacion: $("#txtNuevaOriginacion").val(),
		Tasa: $("#txtNuevaTasa").val(),
		Tasadiaria: $("#txtNuevaTasaDiaria").val(),
		Cuota: $("#txtnuevaCuota").val(),
		Originacion2: $("#txtnuevaOriginacion2").val(),
		Monto: $("#txtNuevoMontoCargoMora").val(),
		DiasMora: $("#txtnuevaDiasMora").val()
	};

	$.ajax({
		processing: true, // for show progress bar
		serverSide: true, // for process server side
		filter: true, // this is for disable filter (search box)
		orderMulti: false, // for disable multiple column at once
		url: "/Producto/CrearProducto",
		type: "POST",
		data: JSON.stringify(empObj),
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			if (result.Respuesta == "EPRN01") {
				alertify.error("El nombre del producto ya existe!!!!!!");
			}
			else if (result.Respuesta == "EPR01") {
				alertify.error("El producto ya existe!!!!!!");
			}
			else {
				alertify.alert("Se ha creado el registro para el teléfono: " + $('#NuevoTelefono').val() + ".", function () {
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
				});
			}
		},
		error: function (errormessage) {
			alertify.error(errormessage.responseText);
		}
	});
}

function validateNuevo() {
	var isValid = true;
	if ($('#txtNuevoNombreProducto').val().trim() == "") {
		$('#txtNuevoNombreProducto').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevoNombreProducto').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevoMontoProducto').val().trim() == "") {
		$('#txtNuevoMontoProducto').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevoMontoProducto').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevoPlazoDias').val().trim() == "") {
		$('#txtNuevoPlazoDias').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevoPlazoDias').css('border-color', 'lightgrey');
	}
	if ($('#ddlNuevoFrecuencia option:selected').val() == "0") {
		$('#ddlNuevoFrecuencia').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlNuevoFrecuencia').css('border-color', 'lightgrey');
	}
	if ($('#txtnuevaFrecuenciaDias').val().trim() == "") {
		$('#txtnuevaFrecuenciaDias').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtnuevaFrecuenciaDias').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevaCantidadCuotas').val().trim() == "") {
		$('#txtNuevaCantidadCuotas').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevaCantidadCuotas').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevaOriginacion').val().trim() == "") {
		$('#txtNuevaOriginacion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevaOriginacion').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevaTasa').val().trim() == "") {
		$('#txtNuevaTasa').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevaTasa').css('border-color', 'lightgrey');
	}
	if ($('#txtnuevaCuota').val().trim() == "") {
		$('#txtnuevaCuota').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtnuevaCuota').css('border-color', 'lightgrey');
	}
	if ($('#txtNuevoMontoCargoMora').val().trim() == "") {
		$('#txtNuevoMontoCargoMora').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtNuevoMontoCargoMora').css('border-color', 'lightgrey');
	}
	if ($('#txtnuevaDiasMora').val().trim() == "") {
		$('#txtnuevaDiasMora').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtnuevaDiasMora').css('border-color', 'lightgrey');
	}
	
	return isValid;
}

function getProducto(id) {
	$('#txtEditNombreProducto').css('border-color', 'lightgrey');
	$('#txtEditMontoProducto').css('border-color', 'lightgrey');
	$('#txtEditPlazoDias').css('border-color', 'lightgrey');
	$('#ddlEditFrecuencia').css('border-color', 'lightgrey');
	$('#txtEditFrecuenciaDias').css('border-color', 'lightgrey');
	$('#txtEditOriginacion').css('border-color', 'lightgrey');
	$('#txtEditTasa').css('border-color', 'lightgrey');
	$('#txtEditTasaDiaria').css('border-color', 'lightgrey');
	$('#txtnuevaCuota').css('border-color', 'lightgrey');
	$('#txtEditOriginacion2').css('border-color', 'lightgrey');
	$('#txtEditMontoCargoMora').css('border-color', 'lightgrey');
	$('#txtEditDiasMora').css('border-color', 'lightgrey');
	$.ajax({
		url: "/Producto/ObtieneProducto/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			var html = '';
			$('#txtEditIdProducto').val(result.Id);
			$('#txtEditNombreProducto').val(result.NombreProducto);
			$('#txtEditMontoProducto').val(result.MontoProducto);
			$('#txtEditPlazoDias').val(result.PlazoDias);
			$('#ddlEditFrecuencia').val(result.Frecuencia);
			$('#txtEditFrecuenciaDias').val(result.FrecuenciaDias);
			$('#txtEditCantidadCuotas').val(result.CantidadCuotas);
			$('#txtEditOriginacion').val(result.Originacion);
			$('#txtEditTasa').val(result.Tasa);
			$('#txtEditTasaDiaria').val(result.Tasadiaria);
			$('#txtEditCuota').val(result.Cuota);
			$('#txtEditOriginacion2').val(result.Originacion2);
			$('#txtEditMontoCargoMora').val(result.Monto);
			$('#txtEditDiasMora').val(result.DiasMora);
			$('#ChkEditActivo').prop('checked', result.Activo);
			$('#myModalEdit').modal('show');
			$('#btnEditUpdate').show();
			$('#btnAdd').hide();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	return false;
}

function ActualizarProducto() {
	var res = validateEdit();
	if (res == false) {
		return;
	}
	var empObj = {
		Id: $('#txtEditIdProducto').val(),
		NombreProducto: $("#txtEditNombreProducto").val(),
		MontoProducto: $("#txtEditMontoProducto").val(),
		PlazoDias: $("#txtEditPlazoDias").val(),
		Frecuencia: $("#ddlEditFrecuencia option:selected").val(),
		FrecuenciaDias: $("#txtEditFrecuenciaDias").val(),
		CantidadCuotas: $("#txtEditCantidadCuotas").val(),
		Originacion: $("#txtEditOriginacion").val(),
		Tasa: $("#txtEditTasa").val(),
		Tasadiaria: $("#txtEditTasaDiaria").val(),
		Cuota: $("#txtEditCuota").val(),
		Originacion2: $("#txtEditOriginacion2").val(),
		Monto: $("#txtEditMontoCargoMora").val(),
		DiasMora: $("#txtEditDiasMora").val(),
		Activo: $("#ChkEditActivo option:selected").val()
	};
	$.ajax({
		processing: true, // for show progress bar
		serverSide: true, // for process server side
		filter: true, // this is for disable filter (search box)
		orderMulti: false, // for disable multiple column at once
		url: "/Producto/ActualizarPruducto",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			//loadData();
			    $('#myModalEdit').modal('hide');
			    $("#txtEditNombreProducto").val('');
			    $("#txtEditMontoProducto").val('');
			    $("#txtEditPlazoDias").val('');
			    $("#ddlEditFrecuencia option:selected").val('');
			    $("#txtEditFrecuenciaDias").val('');
			    $("#txtEditCantidadCuotas").val('');
		      	$("#txtEditOriginacion").val('');
			    $("#txtEditTasa").val('');
			    $("#txtEditTasaDiaria").val('');
			    $("#txtEditCuota").val('');
			    $("#txtEditOriginacion2").val('');
			    $("#txtEditMontoCargoMora").val('');
			    $("#txEditDiasMora").val('');
			$("#ChkEditActivo option:selected").val('');
			if (result.Respuesta == "OK") {
				alertify.alert("Id: " +  $('#txtEditIdProducto').val() + " Se actualizo", function () {
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
					$('#txtEditIdProducto').val("");
				});
				ocultarBloqueo();
				load();
			}
			else {
				alertify.error("Error al Actualizar el Producto!!!!!");
			}
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}


function validateEdit() {
	var isValid = true;
	if ($('#txtEditNombreProducto').val().trim() == "") {
		$('#txtEditNombreProducto').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditNombreProducto').css('border-color', 'lightgrey');
	}
	if ($('#txtEditMontoProducto').val().trim() == "") {
		$('#txtEditMontoProducto').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditMontoProducto').css('border-color', 'lightgrey');
	}
	if ($('#txtEditPlazoDias').val().trim() == "") {
		$('#txtEditPlazoDias').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditPlazoDias').css('border-color', 'lightgrey');
	}
	if ($('#ddlEditFrecuencia option:selected').val() == "0") {
		$('#ddlEditFrecuencia').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlNuevoFrecuencia').css('border-color', 'lightgrey');
	}
	if ($('#txtEditFrecuenciaDias').val().trim() == "") {
		$('#txtEditFrecuenciaDias').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditFrecuenciaDias').css('border-color', 'lightgrey');
	}
	if ($('#txtEditCantidadCuotas').val().trim() == "") {
		$('#txtEditCantidadCuotas').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditCantidadCuotas').css('border-color', 'lightgrey');
	}
	if ($('#txtEditOriginacion').val().trim() == "") {
		$('#txtEditOriginacion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditOriginacion').css('border-color', 'lightgrey');
	}
	if ($('#txtEditTasa').val().trim() == "") {
		$('#txtEditTasa').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditTasa').css('border-color', 'lightgrey');
	}
	if ($('#txtEditCuota').val().trim() == "") {
		$('#txtEditCuota').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditCuota').css('border-color', 'lightgrey');
	}
	if ($('#txtEditMontoCargoMora').val().trim() == "") {
		$('#txtEditMontoCargoMora').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditMontoCargoMora').css('border-color', 'lightgrey');
	}
	if ($('#txtEditDiasMora').val().trim() == "") {
		$('#txtEditDiasMora').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtEditDiasMora').css('border-color', 'lightgrey');
	}
	if ($('#ChkEditActivo option:selected').val() == "0") {
		$('#ChkEditActivo').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ChkEditActivo').css('border-color', 'lightgrey');
	}
	return isValid;
}