var tabladata;



jQuery.ajax(
    {
        url: '@Url.Action("ListarEnvios", "Home")',
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            console.log(data);
        }

    })

tabladata = $("#tabla").DataTable(

    {

        responsive: true,
        ordering: false,
        "ajax": {
            url: '@Url.Action("ListarEnvios", "Home")',
            type: "GET",
            dataType: "json"
        },
        "columns": [
            {
                "data": null, render: function (data, type, row) {
                    // Combinar campos
                    return `${row.IdRemitente}</br>${row.CategoriaMail}</br> ${row.Asunto}</br>`
                }
            },
            {
                "data": "Fecha", render: function (value) {
                    if (value === null) return "";
                    var pattern = /Date\(([^)]+)\)/; //date format from server side
                    var results = pattern.exec(value);
                    var dt = new Date(parseFloat(results[1]));
                    if (dt.getFullYear() === 9999) return ""; //Control para MaxValue
                    return ('0' + dt.getDate()).slice(-2) + "-" + ('0' + (dt.getMonth() + 1)).slice(-2) + "-" + dt.getFullYear() + " " + ('0' + dt.getHours()).slice(-2) + ":" + ('0' + dt.getMinutes()).slice(-2) + ":" + ('0' + dt.getSeconds()).slice(-2);
                }
            },


            {
                "data": "Mensaje",
                "render": function (data, type, row, meta) {

                    return `<a role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
							<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-dots" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="#9e9e9e" fill="none" stroke-linecap="round" stroke-linejoin="round">
								<path stroke="none" d="M0 0h24v24H0z" fill="none"/>
								<circle cx="5" cy="12" r="1" />
								<circle cx="12" cy="12" r="1" />
								<circle cx="19" cy="12" r="1" />
							</svg>
						</a>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
						<a role"button" onClick="editModal(${row.Mensaje})" class="dropdown-item"><i class="fas fa-pen text-danger"></i> Editar</a>
						<a role"button" onClick="createModalItems(${row.Mensaje})" class="dropdown-item"><i class="fas fa-plus text-success"></i> Agregar Item</a>
						<div class="dropdown-divider"></div>
						<a role"button" onClick="changeStatus(${row.Mensaje})" class="dropdown-item"><i class="fas ${row.ErrorId == 1 ? 'fa-ban text-danger' : 'fa-check text-success'}"></i> ${row.STATUS == 1 ? 'Inhabilitar' : 'Habilitar'}</a>
					  </div>`;
                }
            },

            { "data": "Id", "name": "Adjuntos" },


            {
                "data": null, render: function (data, type, row, value) {

                    // Combinar campos
                    return `${row.Fecha}</br>${row.FechaEnvio}</br>`

                }


            }

        ],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.3/i18n/es_es.json"


        }


    });
