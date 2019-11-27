$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY HH:mm:ss');
    $.extend($.fn.dataTable.defaults, {
        "lengthMenu": [5, 10, 20, 50],
        "pageLength": 5,
        "language": {
            "url": "/lib/datatables/language/Vietnamese.json"
        }
    });
    $('#dtChange').DataTable({
        order:[0,'desc']
    });
    $('#dtCustomerCount').DataTable({
        order: [2, 'desc'],
        "columns": [
            null,
            null,
            { data: "2", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') }
        ]
    });
    $('#dtProductCount').DataTable({
        "columns": [
            null,
            { data: "1", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') },
            null,
            { data: "3", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') }
        ]
    });
    $('#dtProduct').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": [3, 4, 5] }
        ],
        "columns": [
            null,
            { data: "1", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') },
            null,
            null,
            null,
            null
        ]
    });
    $('#dtOrder').DataTable({
        order: [1, 'desc'],
        "columnDefs": [
            { "orderable": false, "targets": 5 }
        ],
        "columns": [
            null,
            null,
            null,
            { data: "3", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') },
            null,
            null
        ]
    });
    $('#dtProductSell').DataTable({
        "order": [1, 'asc'],
        "columnDefs": [
            { "orderable": false, "targets": 0 },
        ],
        "columns": [
            null,
            null,
            { data: "2", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') },
            null
        ]
    });
    $('#dtKH').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 3 }
        ]
    });
    $('#dtLSPvaNSX').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 1 }
        ]
    });
    $('#dtPrice').DataTable({
        order: [1, 'desc'],
        searching: false,
        paging: false,
        scrollY: 200,
        "info": false,
        "columns": [
            { data: "0", render: $.fn.dataTable.render.number(' ', '.', 0, ' ', ' VND') },
            null,
            null
        ]
    });
    $('#dtUser').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 2 }
        ]
    });
});