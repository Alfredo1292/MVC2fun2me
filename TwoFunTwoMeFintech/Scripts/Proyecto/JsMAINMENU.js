
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
		url: "/MAINMENU/Detalle",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.ID + '</td>';
				html += '<td>' + item.MAINMENU + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.ID + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.ID + ')">Eliminar</a></td>';
				html += '</tr>';
			});

            $("#MAINMENUTable").dataTable().fnClearTable();
            $("#MAINMENUTable").dataTable().fnDestroy();

            $('.tbody').html(html);

			//$.noConflict();
			$("#MAINMENUTable").dataTable();

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
	$('#MAINMENU').css('border-color', 'lightgrey');
	$.ajax({
		url: "/MAINMENU/Edit/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			$.each(result, function (key, item) {
				$('#ID').val(item.ID);
				$('#MAINMENU').val(item.MAINMENU);
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
		MAINMENU: $('#MAINMENU').val(),
	};
	$.ajax({
		url: "/MAINMENU/Update",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			//loadData();
			$('#myModal').modal('hide');
			$('#MAINMENU').val("");

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
				$('#ID').val("");
				location.reload();
			});
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
		MAINMENU: $('#txtMAINMENU').val(),
	};
	$.ajax({
		url: "/MAINMENU/Crear",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			$('#txtMAINMENU').val("");
			alertify.alert("Se Creó correctamente el menú! " , function () {
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
			//    url: "/MAINMENU/Eliminar",
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
					url: "/MAINMENU/Eliminar",
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
	$('#MAINMENU').val("");
	$('#btnUpdate').hide();
	$('#btnAdd').show();
	$('#ID').css('border-color', 'lightgrey');
	$('#MAINMENU').css('border-color', 'lightgrey');
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
	if ($('#MAINMENU').val().trim() == "") {
		$('#MAINMENU').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#MAINMENU').css('border-color', 'lightgrey');
	}
	return isValid;
}

//Valdidation using jquery
function validateCreate() {
	var isValid = true;
	if ($('#txtMAINMENU').val().trim() == "") {
		$('#txtMAINMENU').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#txtMAINMENU').css('border-color', 'lightgrey');
	}
	return isValid;
}