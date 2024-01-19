var dataTable;

$(document).ready(function () {   
    console.log("test")
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { data: 'title',"width" : "20%" },     //parameters checked with Json fomatter
            { data: 'isbn', "width":  "15%" },
            { data: 'author', "width": "20%" },
            { data: 'listPrice', "width": "15%" },
            { data: 'category.name', "width": "15%" }
        ]
    });
}


