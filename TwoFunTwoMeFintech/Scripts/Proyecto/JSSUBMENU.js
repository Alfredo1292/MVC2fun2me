
//Load Data in Table when documents is ready
$(document).ready(function () {
<<<<<<< HEAD
    loadData();
    $('body').css("background-image", "none");
=======
	loadData();
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
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
		url: "/SUBMENU/Detalle",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.ID + '</td>';
				html += '<td>' + item.SUBMENU + '</td>';
				html += '<td>' + item.CONTROLLER + '</td>';
				html += '<td>' + item.descMAINMENUID + '</td>';
				html += '<td>' + item.descROLEID + '</td>';
				html += '<td>' + item.ACTION + '</td>';
<<<<<<< HEAD
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.ID + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.ID + ')">Eliminar</a></td>';
=======
				html += '<td><a href="#" class="btn btn-primary" onclick="return getbyID(' + item.ID + ')"> Editar </a> | <a class="btn btn-primary"  href="#" onclick="Delele(' + item.ID + ')">Eliminar</a></td>';
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
				html += '</tr>';
			});

			$('.tbody').html(html);
<<<<<<< HEAD
=======
			$.noConflict();
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
			$('#SUBMENUTable').DataTable();

		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
	//	$("#UserTable").DataTable();
}



//Function for getting the Data Based upon Employee ID
function getbyID(id) {
	$('#ID').css('border-color', 'lightgrey');
	$('#SUBMENU').css('border-color', 'lightgrey');
	$('#CONTROLLER').css('border-color', 'lightgrey');
	$('#ddlMAINMENU').css('border-color', 'lightgrey');
	$('#ddlROLEID').css('border-color', 'lightgrey');
	$('#ACTION').css('border-color', 'lightgrey');
	$.ajax({
		url: "/SUBMENU/Edit/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			$.each(result.listadoMainMenu, function (key, item) {

				$("#ddlMAINMENU").append(new Option(item.MAINMENU, item.ID));
			});
			$.each(result.ListRoles, function (key, item) {

				$("#ddlROLEID").append(new Option(item.ROLES, item.ID));
			});
			$.each(result.listadoSubMenu, function (key, item) {
				$('#ID').val(item.ID);
				$('#SUBMENU').val(item.SUBMENU);
				$('#CONTROLLER').val(item.CONTROLLER);
				$('#ACTION').val(item.ACTION);
				$('#ddlMAINMENU option:selected').val(item.MAINMENUID),
				$('#ddlROLEID option:selected').val(item.ROLEID),
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
		ID: $('#ID').val(),
		SUBMENU: $('#SUBMENU').val(),
		CONTROLLER: $('#CONTROLLER').val(),	
		MAINMENUID: $('#ddlMAINMENU option:selected').val(),
		ROLEID: $('#ddlROLEID option:selected').val(),
		ACTION: $('#ACTION').val(),
	};
	$.ajax({
		url: "/SUBMENU/Update",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			//loadData();
			$('#myModal').modal('hide');
			
			$('#SUBMENU').val("");
			$('#CONTROLLER').val("");
			$('#ddlMAINMENU').html("");
			$('#ddlROLEID').html("");
			$('#ACTION').val("");

			alertify.alert("Id: " + $('#ID').val() + " Se actualizo", function () {
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
			$('#ID').val("");
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}
//Funcion crear
function Crear() {
	var res = validateCreate();
	if (res == false) {
		return false;
	}
	var empObj = {
		SUBMENU: $('#TextSubMenu').val(),
		CONTROLLER: $('#TextCONTROLLER').val(),
		MAINMENUID: $('#ddlMAINMENUlist option:selected').val(),
		ROLEID: $('#ddlROLEIDlist option:selected').val(),
		ACTION: $('#TextAction').val(),
	};
	$.ajax({
		url: "/SUBMENU/Crear",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			//loadData();
			$('#myModal').modal('hide');

			$('#SUBMENU').val("");
			$('#CONTROLLER').val("");
			$('#ddlMAINMENU').html("");
			$('#ddlROLEID').html("");
			$('#ACTION').val("");

			alertify.alert("Se Creó correctamente el submenú!", function () {
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
				$('#ID').val("");
				location.reload();
			});
			$('#ID').val("");
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
			//    url: "/SUBMENU/Eliminar",
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
					url: "/SUBMENU/Eliminar",
					type: "POST",
					data: {
						ID
					},
					success: function (result) {
						alertify.alert(result.Mensaje);
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
	$('#ID').val("");
	$('#SUBMENU').val("");
	$('#CONTROLLER').val("");
	$('#ddlMAINMENU').html("");
	$('#ddlROLEID').html("");
	$('#ACTION').val("");
	$('#btnUpdate').hide();
	$('#btnAdd').show();
	$('#ID').css('border-color', 'lightgrey');
	$('#SUBMENU').css('border-color', 'lightgrey');
	$('#CONTROLLER').css('border-color', 'lightgrey');
	$('#ddlMAINMENU').css('border-color', 'lightgrey');
	$('#ddlROLEID').css('border-color', 'lightgrey');
	$('#ACTION').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validate() {
	var isValid = true;
	if ($('#ID').val().trim() == "") {
		$('#ID').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ID').css('border-color', 'lightgrey');
	}
	if ($('#SUBMENU').val().trim() == "") {
		$('#SUBMENU').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#SUBMENU').css('border-color', 'lightgrey');
	}
	if ($('#CONTROLLER').val().trim() == "") {
		$('#CONTROLLER').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#CONTROLLER').css('border-color', 'lightgrey');
	}
	if ($('#ACTION').val().trim() == "") {
		$('#ACTION').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ACTION').css('border-color', 'lightgrey');
	}
	return isValid;
}

//Valdidation using jquery
function validateCreate() {
	var isValid = true;
	if ($('#TextSubMenu').val().trim() == "") {
		$('#TextSubMenu').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#TextSubMenu').css('border-color', 'lightgrey');
	}
	if ($('#TextCONTROLLER').val().trim() == "") {
		$('#TextCONTROLLER').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#TextCONTROLLER').css('border-color', 'lightgrey');
	}
	if ($('#ddlMAINMENUlist option:selected').val() == 0) {
		$('#ddlMAINMENUlist').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlMAINMENUlist').css('border-color', 'lightgrey');
	}
	if ($('#ddlROLEIDlist option:selected').val() == 0) {
		$('#ddlROLEIDlist').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ddlROLEIDlist').css('border-color', 'lightgrey');
	}
	if ($('#TextAction').val().trim() == "") {
		$('#TextAction').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#TextAction').css('border-color', 'lightgrey');
	}
	return isValid;
}