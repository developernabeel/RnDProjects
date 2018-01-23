$(document).ready(function () {
    getGrid();
});


var oTable;
function getGrid() {
    var projectId = $('#hdnProjectId').val();
    oTable = $('#tbl-connections').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/ProjectConnectionsList?projectId=" + projectId,
        "columnDefs": [
            {
                "targets": [4],
                "render": function (data, type, row) {
                    return getActionButtonsHtml(row);
                }
            }
        ],
        "columns": [
            { data: "ConnectionName", title: "Connection Name" },
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
            url: SBIReportUtility.Url.root + 'Project/DeleteProjectConnection?connectionId=' + connectionId + '&nocache=' + new Date().getTime(),
            success: function (json) {
                if (json.success) {
                    ShowMessage("alert-success", json.message);
                }
                else {
                    ShowMessage("alert-danger", json.message);
                }

                $('#delete-modal').modal('hide');
                if (oTable) {
                    oTable.ajax.reload();
                }
                else {
                    getGrid();
                }
            },
            error: function (err, status) {
                alert('error: Something is wrong. Please contact Administrator.')
            }
        });
    });
}

function getActionButtonsHtml(rowData) {
    var projectId = $('#hdnProjectId').val();
    return "<a href='" + SBIReportUtility.Url.root + "Project/AddEditProjectConnection?projectId=" + projectId + "&connectionId=" + rowData.Id + "' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Edit Connection' > <i class='fa fa-pencil'></i></a>&nbsp;" +
        "<button onclick='fnDeleteConnection(" + rowData.Id + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Delete Connection'> <i class='fa fa-trash-o'></i></button>";
}