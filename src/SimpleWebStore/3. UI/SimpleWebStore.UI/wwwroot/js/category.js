﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Category/GetAll"
            },
            "columns": [
                { "data": "name", "width": "30%" },
                { "data": "displayOrder", "width": "50%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="d-flex justify-content-between">
                                <a href="/Admin/Category/Edit?id=${data}" asp-action="Edit" asp-route-id=@category.Id
                                    class="btn btn-outline-primary my-custom">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                            Edit
                                </a>
                                <a onClick=Delete('/Admin/Category/Delete/${data}')
                                    class="btn btn-outline-danger my-custom" >
                                        <i class="fa-solid fa-trash"></i>
                                            Delete
                                </a>
                            </div>
                            `
                    },
                    "width": "20%"
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