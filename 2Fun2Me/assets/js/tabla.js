function tablaayudantes() {
    $.ajax({
        url: "controller/tabla.php",
        type: 'POST',
        dataType: 'json',
        cache: false
    }).done(function (data) {

        $('#Tabla').dataTable().fnDestroy();
        var tbody = document.getElementById("respuesta_tabla");

        while (tbody.firstChild) {
            tbody.removeChild(tbody.firstChild);
        }

        var x;
        for (x = 0; x < data.tabla.length; x++) {

            var tr = document.createElement('tr');
            var th = document.createElement('th');
            var th1 = document.createElement('th');
            var th2 = document.createElement('th');
            var th3 = document.createElement('th');
            var th4 = document.createElement('th');
            var th5 = document.createElement('th');
            var th6 = document.createElement('th');
            var th7 = document.createElement('th');
            var th8 = document.createElement('th');
            var th9 = document.createElement('th');
            var th10 = document.createElement('th');
            var th11 = document.createElement('th');

            tr.className = 'gradeA';
            th.className = 'dt-center';
            th1.className = 'dt-center';
            th2.className = 'dt-center';
            th3.className = 'dt-center';
            th4.className = 'dt-center';
            th5.className = 'dt-center';
            th6.className = 'dt-center';
            th7.className = 'dt-center';
            th8.className = 'dt-center';
            th9.className = 'dt-center';
            th10.className = 'dt-center';
            th11.className = 'dt-center';

            th.innerHTML = data.tabla[x][0];
            th1.innerHTML = data.tabla[x][1];
            th2.innerHTML = data.tabla[x][2];
            th3.innerHTML = data.tabla[x][3];
            th4.innerHTML = data.tabla[x][4];
            th5.innerHTML = data.tabla[x][5];
            th6.innerHTML = data.tabla[x][6];
            th7.innerHTML = data.tabla[x][7];
            th8.innerHTML = data.tabla[x][8];
            th9.innerHTML = data.tabla[x][9];
            th10.innerHTML = data.tabla[x][10];
            th11.innerHTML = data.tabla[x][11];

            tr.appendChild(th);
            tr.appendChild(th1);
            tr.appendChild(th2);
            tr.appendChild(th3);
            tr.appendChild(th4);
            tr.appendChild(th5);
            tr.appendChild(th6);
            tr.appendChild(th7);
            tr.appendChild(th8);
            tr.appendChild(th9);
            tr.appendChild(th10);
            tr.appendChild(th11);
            tbody.appendChild(tr);
        }

        $('#Tabla').dataTable({
            "language": {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                }
            },
            dom: 'Bfrtip',
            lengthMenu: [
                [10, 25, 50, -1],
                ['10 filas', '25 filas', '50 filas', 'Mostrar Todo']
            ],
            buttons: [
                'copyHtml5',
                'excelHtml5',
                'csvHtml5',
                'pdfHtml5',
                'pageLength'
            ]
        });

        $('.dt-buttons').addClass('btn-group');
        $('.dt-button').removeClass('dt-button');

        return false;
    });
}

tablaayudantes();