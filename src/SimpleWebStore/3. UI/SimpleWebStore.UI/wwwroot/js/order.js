var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("pending")) {
        loadDataTable("pending");
    }
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    if (url.includes("completed")) {
        loadDataTable("completed");
    }
    if (url.includes("all")) {
        loadDataTable("all");
    }       
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Order/GetAll?status=" + status
            },
            "columns": [
                { "data": "id", "width": "17%"},
                { "data": "name", "width": "15%" },
                { "data": "phoneNumber", "width": "15%" },
                { "data": "appUser.email", "width": "15%" },
                { "data": "orderStatus", "width": "10%" },
                { "data": "orderTotal", "width": "8%" },
                {
                    "data": "id", 
                    "render": function (data) {
                        return `
                            <div class="d-flex justify-content-between">
                                <a href="/Admin/Order/Details?orderId=${data}" 
                                    class="btn btn-outline-primary w-100">
                                        <i class="fa-solid fa-circle-info"></i>
                                            Details
                                </a>
                            </div>
                            `
                    },
                    "width": "10%"
                },
            ]
        }
    );
}