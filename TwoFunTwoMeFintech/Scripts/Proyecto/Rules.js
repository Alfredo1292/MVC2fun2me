//Load Data in Table when documents is ready
$(document).ready(function () {
    loadData();
    $('body').css("background-image", "none");
	//$('#example').dataTable();
});

//Load Data function
function loadData() {
	processing: true; // for show progress bar  
	serverSide: true; // for process server side  
	filter: true; // this is for disable filter (search box)  
	orderMulti: false; // for disable multiple column at once
	paging: false;
	$.ajax({
		url: "/Rules/Detalle",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.IdRule + '</td>';
				html += '<td>' + item.NombreModelo + '</td>';
				html += '<td>' + item.Descripcion + '</td>';
				html += '<td>' + item.Proceso + '</td>';
				html += '<td>' + item.Condicion + '</td>';
				html += '<td>' + item.Tipo + '</td>';
				html += '<td>' + item.ActivoString + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.IdRule + ')"> Editar </a></td>';
				html += '</tr>';
			});

			$('.tbody').html(html);

			$("#RulesTable").DataTable();
			$("select[name*='RulesTable_length']").change();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	//	$("#UserTable").DataTable();
}



//Function for getting the Data Based upon Employee ID
function getbyID(id) {
	$('#IdRule').css('border-color', 'lightgrey');
	$('#NombreModelo').css('border-color', 'lightgrey');
	$('#Descripcion').css('border-color', 'lightgrey');
	$('#Proceso').css('border-color', 'lightgrey');
	$('#Tipo').css('border-color', 'lightgrey');
	$('#ddlActivo').css('border-color', 'lightgrey');
	$.ajax({
		url: "/Rules/Edit/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			var html = '';
			$.each(result, function (key, item) {
				$('#IdRule').val(item.IdRule);
				$('#NombreModelo').val(item.NombreModelo);
				$('#Descripcion').val(item.Descripcion);
				$('#Proceso').val(item.Proceso);
				$('#Condicion').val(item.Condicion);
				$('#Tipo').val(item.Tipo);				
				document.getElementById('ddlActivo').value = item.Activo;				
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
		IdRule: $('#IdRule').val(),
		NombreModelo: $('#NombreModelo').val(),
		Descripcion: $('#Descripcion').val(),
		Proceso: $('#Proceso').val(),
		Condicion: $('#Condicion').val(),
		Tipo: $('#Tipo').val(),
		Activo: $('#ddlActivo option:selected').val(),
	};
	$.ajax({
		url: "/Rules/Update",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			$('#myModal').modal('hide');			
			$('#NombreModelo').val("");
			$('#Descripcion').val("");
			$('#Proceso').val("");
			$('#Condicion').val("");
			$('#Tipo').val("");
			$('#Activo').val("");
			alertify.alert("Id: " + $('#IdRule').val() + " Se actualizo", function () {
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
				location.reload();
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
					url: "/Rules/Eliminar",
					type: "POST",
					data: {
						ID
					},
					success: function (result) {
						alertify
							.alert(result.Mensaje);

						location.reload();
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
	$('#IdRule').val("");
	$('#NombreModelo').val("");
	$('#Descripcion').val("");
	$('#Proceso').val("");
	$('#Condicion').val("");
	$('#Tipo').val("");
	$('#Activo').val("");
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
	if ($('#IdRule').val().trim() == "") {
		$('#IdRule').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#IdRule').css('border-color', 'lightgrey');
	}
	if ($('#NombreModelo').val().trim() == "") {
		$('#NombreModelo').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#nombNombreModelore').css('border-color', 'lightgrey');
	}
	if ($('#Descripcion').val().trim() == "") {
		$('#Descripcion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#Descripcion').css('border-color', 'lightgrey');
	}
	if ($('#Proceso').val().trim() == "") {
		$('#Proceso').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#Proceso').css('border-color', 'lightgrey');
	}
	if ($('#Condicion').val().trim() == "") {
		$('#Condicion').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#Condicion').css('border-color', 'lightgrey');
	}
	if ($('#Tipo').val().trim() == "") {
		$('#Tipo').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#Tipo').css('border-color', 'lightgrey');
	}
	return isValid;
}