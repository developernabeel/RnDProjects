$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    var projectId = $('#hdnProjectId').val();
    oTable = $('#table-reportList').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/ProjectReportsList?projectId=" + projectId,
        "columnDefs": [{
            "targets": [6],
            "render": function (data, type, row) {
                return getActionButtonsHtml(row);
            }
        }],
        "columns": [
            { data: "Name", title: "Name" },
            { data: "Description", title: "Description" },
            { data: "ProcedureName", title: "Procedure" },
            { data: "ProjectName", title: "Project" },
            { data: "ConnectionName", title: "Connection" },
            { data: "IsActive", title: "Status" },
            { data: "Action", title: "Actions" }
        ],
        responsive: true
    });
}

function getActionButtonsHtml(rowData) {
    var isActive = rowData.IsActive == 'Active';
    var activeClass = rowData.IsActive == 'Active' ? 'btn-danger' : 'btn-success';
    var activeText = rowData.IsActive == 'Active' ? 'Deactivate Report' : 'Activate Report';
    var activeIcon = rowData.IsActive == 'Active' ? 'fa fa-times' : 'fa fa-check';
    return "<button onclick='fnActivateDeactivateReport(" + rowData.Id + ", " + isActive + ");' class='btn " + activeClass + " btn-circle' data-toggle='tooltip' data-original-title='" + activeText + "'> <i class='" + activeIcon + "'></i></button>&nbsp;" +
        "<button onclick='fnAddEditReport(" + rowData.Id + ");' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Edit Report' > <i class='fa fa-pencil'></i></button> &nbsp;" +
        "<button onclick='fnDeleteReport(" + rowData.Id + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Delete Report' > <i class='fa fa-trash'></i></button>";
}

function fnAddEditReport(reportId) {
    var projectId = $('#hdnProjectId').val();
    $.ajax({
        type: "GET",
        url: SBIReportUtility.Url.root + 'Project/ShowAddEditProjectReportPopup?projectId=' + projectId + '&reportId=' + reportId + '&nocache=' + new Date().getTime(),
        success: function (status) {
            if (status != null) {
                $('#reportModal').html(status);
                $.validator.unobtrusive.parse($("#reportModal"));
                $('#reportModal').modal('show');
                $('#inputReportName').focus();
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnOnReportSuccess(data) {
    ShowMessage(data.cssClass, data.result);
    $('#reportModal').modal('hide');
    if (oTable) {
        oTable.ajax.reload();
    }
    else {
        getGrid();
    }
}

function fnActivateDeactivateReport(reportId, isActive) {
    $('#reportConfirm').find('#hdnReportId').val(reportId);
    var txtMessage = isActive ? 'Are you sure you want to deactivate this report?' : 'Are you sure you want to activate this report?';
    $('#txt-modalmsg').text(txtMessage);
    $('#reportConfirm').modal('show');
    $('#reportConfirm').find('#btnConfirm').unbind('click').bind('click', function () {
        $.ajax({
            type: "POST",
            url: SBIReportUtility.Url.root + 'Project/ActivateDeactivateProjectReport?reportId=' + reportId + '&nocache=' + new Date().getTime(),
            success: function (data) {
                if (data != null) {
                    ShowMessage("alert-success", data.result);
                    $('#reportConfirm').modal('hide');
                    $('#reportConfirm').find('#hdnReportId').val('');
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
    return false;
}

function fnDeleteReport(reportId) {
    $('#reportConfirm').find('#hdnReportId').val(reportId);
    $('#msgText').html("Are you sure you want to delete report?");
    $('#reportConfirm').modal('show');
    $('#reportConfirm').find('#btnConfirm').unbind('click').bind('click', function () {
        $.ajax({
            type: "POST",
            url: SBIReportUtility.Url.root + 'Project/DeleteProjectReport?reportId=' + reportId + '&nocache=' + new Date().getTime(),
            success: function (data) {
                if (data != null) {
                    ShowMessage("alert-success", data.result);
                    $('#reportConfirm').modal('hide');
                    $('#reportConfirm').find('#hdnReportId').val('');
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
    return false;
}