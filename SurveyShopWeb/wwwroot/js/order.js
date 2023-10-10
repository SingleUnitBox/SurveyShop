var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall'},
        "columns": [
            { data: 'id' },
            { data: 'name' },
            { data: 'phoneNumber' },
            { data: 'applicationUser.email' },
            { data: 'orderStatus' },
            { data: 'orderTotal' },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <a href="/admin/order/details?orderId=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i></a>
                    `
                }
            }
        ]
    });
}