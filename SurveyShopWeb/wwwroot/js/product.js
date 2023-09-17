var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall'},
        "columns": [
            { data: 'id' },
            { data: 'name' },
            { data: 'barCode' },
            { data: 'price' },
            { data: 'category.name' },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>&nbspEdit</a>
                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger">
                            <i class="bi bi-trash-fill"></i>&nbspDelete</a>
                    `
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}