$(document).ready(function () {
    $("#tblData").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Book/GetAll",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "isbn", "width": "20%" },
            { "data": "author", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "price", "width": "15%" },

        ]
    });
})