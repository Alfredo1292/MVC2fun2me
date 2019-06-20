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
		url: "/Roles/Detalle",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (data) {
			var html = '';
			$.each(data, function (key, item) {
				html += '<tr>';
				html += '<td>' + item.ID + '</td>';
                html += '<td>' + item.ROLESNOMBRE + '</td>';
                html += '<td><a href="#" class="btn btn-primary btn-primary-editar" onclick="return getbyID(' + item.ID + ')"> Editar </a> | <a class="btn btn-primary btn-primary-eliminar"  href="#" onclick="Delele(' + item.ID + ')">Eliminar</a></td>';
				html += '</tr>';
			});			
			$('.tbody').html(html);
			
			$('#RolesTable').DataTable();
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});

	//	$("#UserTable").DataTable();
}

//Function for getting the Data Based upon ROLES ID
function getbyID(id) {
	$('#ID').css('border-color', 'lightgrey');
	$('#ROL').css('border-color', 'lightgrey');
	$.ajax({
		url: "/Roles/Edit/" + id,
		typr: "GET",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			var html = '';
			$.each(result, function (key, item) {
				$('#ID').val(item.ID);
                $('#ROL').val(item.ROLESNOMBRE);		
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

//function for updating Roles record
function Update() {
	var res = validate();
	if (res == false) {
		return false;
	}
	var empObj = {
		ID: $('#ID').val(),
        ROLESNOMBRE: $('#ROL').val()
	};
	$.ajax({
		url: "/Roles/Update",
		data: JSON.stringify(empObj),
		type: "POST",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {			
			$('#myModal').modal('hide');
			$('#ID').val("");
			$('#ROl').val("");
			//loadData();

			alertify.alert("Id: " + $('#id').val() + " Se actualizo", function () {
				var searchTerm = $(".search").val();
				var listItem = $('.results tbody').children('tr');
				//var searchSplit = searchTerm.replace(/ /g, "'):containsi('")

				$.extend($.expr[':'], {
					'containsi': function (elem, i, match, array) {
						return (elem.textContent || elem.innerText || '').toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
					}
				});

				//$(".results tbody tr").not(":containsi('" + searchSplit + "')").each(function (e) {
				//	$(this).attr('visible', 'false');
				//});

				//$(".results tbody tr:containsi('" + searchSplit + "')").each(function (e) {
				//	$(this).attr('visible', 'true');
				//});

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
//function for deleting Roles record
function Delele(ID) {


	alertify.confirm("¿Desea Continuar?",
		function () {
			$.ajax(
				{
					url: "/Roles/Eliminar",
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
						alertify.error('UPPS, lo ciento, favor contacte con el Administrador');
					}
				});
		});
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
	if ($('#ROL').val().trim() == "") {
		$('#ROL').css('border-color', 'Red');
		isValid = false;
	}
	else {
		$('#ROL').css('border-color', 'lightgrey');
	}
	return isValid;
}
//Function for clearing the textboxes
function clearTextBox() {
	$('#ID').val("");
	$('#ROL').val("");
	$('#btnUpdate').hide();
	$('#btnAdd').show();
	$('#ID').css('border-color', 'lightgrey');
	$('#ROL').css('border-color', 'lightgrey');
}