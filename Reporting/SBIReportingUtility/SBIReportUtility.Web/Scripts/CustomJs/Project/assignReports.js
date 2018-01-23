$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    var projectId = $('#hdnProjectId').val();
    var pfid = $('#hdnPfid').val();
    oTable = $('#tbl-reports').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/AssignReportsList?projectId=" + projectId + "&pfid=" + pfid,
        "columnDefs": [
            {
                "targets": [4],
                "render": function (data, type, row) {
                    return row.IsAssigned ? 'Yes' : 'No';
                }
            },
            {
                "targets": [5],
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
            { data: "IsAssigned", title: "Is Assigned" },
            { data: "Action", title: "Action" }
        ],
        responsive: true
    });
}

function fnAssignUnassignReport(reportId, pfid) {
    var projectId = $('#hdnProjectId').val();
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'Project/AssignUnassignReport',
        data: {
            Pfid: pfid,
            ProjectId: projectId,
            ReportId: reportId
        },
        success: function (json) {
            if (json.success) {
                ShowMessage('alert-success', json.message);

                if (oTable) {
                    oTable.ajax.reload();
                }
                else {
                    getGrid();
                }
            }
            else
                ShowMessage('alert-danger', json.message);
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function getActionButtonsHtml(rowData) {
    var pfid = $('#hdnPfid').val();

    var title = rowData.IsAssigned ? 'Unassign this report' : 'Assign this report';
    var isChecked = rowData.IsAssigned ? 'checked="checked"' : '';

    return '<input id="chk-assign-' + rowData.Id + '" class="checkbox-custom" type="checkbox"' + ' onclick=fnAssignUnassignReport(' + rowData.Id + ',' + pfid + ') ' + isChecked + ' title="' + title + '">' +
            '<label for="chk-assign-' + rowData.Id + '" class="checkbox-custom-label" >' + '<span></span>' + '</label>';
}