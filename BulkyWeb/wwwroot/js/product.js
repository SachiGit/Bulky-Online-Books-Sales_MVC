var dataTable;

$(document).ready(function () {   
    //console.log("test")
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { data: 'title',"width" : "25%" },     //parameters checked with Json fomatter
            { data: 'isbn', "width":  "15%" },
            { data: 'author', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data)
                {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width" : "25%"
            }
        ]
    });
}

function Delete(url)
{
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,   //parameter of the Line #34 method  onClick URL
                type: 'DELETE',   //this is [HttpDelete]
                success: function (data) {     //function for get the 'data' back
                    dataTable.ajax.reload();   //reload the full data table quickly
                    toastr.success(data.message);   //if passed we are getting 'success' and a 'message' at ProductController Line #226
                }
            })
            //});
        }
    });
}
