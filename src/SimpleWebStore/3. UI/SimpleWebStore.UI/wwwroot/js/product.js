var dataTable;
var notyf = new Notyf(
    {
        position: {
            x: 'right',
            y: 'bottom',
        },
        duration: 3000,
        dismissible: true,
        ripple: true
    });

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Product/GetAll"
            },
            "columns": [
                { "data": "title", "width": "15%"},
                { "data": "isbn", "width": "15%" },
                { "data": "price", "width": "15%" },
                { "data": "author", "width": "15%" },
                { "data": "category.name", "width": "15%" },
                {
                    "data": "id", 
                    "render": function (data) {
                        return `
                            <div class="d-flex justify-content-between">
                                <a href="/Admin/Product/Upsert?id=${data}" asp-action="Upsert" asp-route-id=@product.Id
                                    class="btn btn-outline-primary my-custom">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                            Edit
                                </a>
                                <a onClick=Delete('/Admin/Product/Delete/${data}')
                                    class="btn btn-outline-danger my-custom" >
                                        <i class="fa-solid fa-trash"></i>
                                            Delete
                                </a>
                            </div>
                            `
                    },
                    "width": "15%"
                },
            ]
        }
    );
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        notyf.success(data.message);
                    }
                    else {
                        notyf.error(data.message);
                    }
                }
            })
        }
    })
}