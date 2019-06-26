function tablaayudantes() {
    $.ajax({
        url: "controller/contador.php",
        type: 'POST',
        dataType: 'json',
        cache: false
    }).done(function (data) {

        $('#table').dataTable().fnDestroy();
        var tbody = document.getElementById("respuesta_tabla");

        while (tbody.firstChild) {
            tbody.removeChild(tbody.firstChild);
        }

        var x;
        for (x = 0; x < data.tabla.length; x++) {

            var tr = document.createElement('tr');
            var th = document.createElement('th');
            var th1 = document.createElement('th');

            tr.className = 'gradeA';
            th.className = 'dt-center';

            th.innerHTML = data.tabla[x][0];
            th1.innerHTML = data.tabla[x][1];

            tr.appendChild(th);
            tr.appendChild(th1);
            tbody.appendChild(tr);
        }

        $('#table').dataTable({
            "order": [[1, "desc"]]
        });

        return false;
    });
}

tablaayudantes();