$(document).ready(function () {
    getGrid();   
});

var oTable;
function getGrid() {
    oTable = $('#tbl-connections').DataTable({
        "ajax": SBIReportUtility.Url.root + "Connection/List",
        "columnDefs": [
            {
                "targets": [5],
                "render": function (data, type, row) {
                    return getActionButtonsHtml(row);
                },
                "searchable": false
            }
        ],
        "columns": [
            { data: "ConnectionName", title: "Connection Name" },
            { data: "ProjectName", title: "Project Name" },
            { data: "SID", title: "SID" },
            { data: "IpAddress", title: "IP Address" },
            { data: "PortNumber", title: "Port Number" },
            { data: "Action", title: "Actions" }
        ],
        responsive: true
    });
}

function fnDeleteConnection(connectionId) {
    $('#delete-modal').modal('show');
    $('#delete-modal').find('#btn-delete').unbind('click').bind('click', function () {
        $.ajax({
            type: "POST",
            url: SBIReportUtility.Url.root + 'Connection/DeleteConnection?connectionId=' + connectionId + '&nocache=' + new Date().getTime(),
            success: function (json) {
                if (json.success) {
                        ShowMessage("alert-success", json.message);
                        $('#delete-modal').modal('hide');
                        if (oTable) {
                            oTable.ajax.reload();
                    }
                    else {
                        getGrid();
                    }
                }
            },
            error: function (err, status) {
                alert('error: Something is wrong. Please contact Administrator.')
            }
        });
    });
}

function getActionButtonsHtml(rowData) {
    return "<a href='" + SBIReportUtility.Url.root + "Connection/AddEditConnection?connectionId=" + rowData.Id + "' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Edit Connection' > <i class='fa fa-pencil'></i></a>&nbsp;" +
        "<button onclick='fnDeleteConnection(" + rowData.Id + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Delete Connection'> <i class='fa fa-trash-o'></i></button>";
}