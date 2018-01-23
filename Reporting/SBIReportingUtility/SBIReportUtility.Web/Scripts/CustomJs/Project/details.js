$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    var projectId = $('#hdnProjectId').val();
    oTable = $('#tbl-reports').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/ReportsList?projectId=" + projectId,
        //data: data,
        "columnDefs": [
            {
                "targets": [4],
                "render": function (data, type, row) {
                    return getActionButtonsHtml(row);
                }
            }
        ],
        "columns": [
            { data: "Name", title: "Report Name" },
            { data: "Description", title: "Description" },
            { data: "ProcedureName", title: "Procedure Name" },
            { data: "ConnectionName", title: "Connection Name" },
            { data: "Action", title: "Actions" }
        ],
        responsive: true
    });
}

function getActionButtonsHtml(rowData) {
    return "<a href='" + SBIReportUtility.Url.root + "Report/DownloadReport?reportId=" + rowData.Id + "' target='_blank' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Download Report'><i class='fa fa-download'></i></a>";
}