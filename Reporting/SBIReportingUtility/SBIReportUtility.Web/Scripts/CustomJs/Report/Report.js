$(document).ready(function () {
    getGrid();

    $('body').on('change', '#ddlProjects', function () {
        var projectId = $(this).val();
        if (projectId == '') {
            $('#ddlConnections').html('<option value="">Select</option>');
            return;
        }

        $.ajax({
            type: 'POST',
            url: SBIReportUtility.Url.root + 'Report/GetConnectionsByProject?projectId=' + projectId,
            success: function (json) {
                if (json.success) {
                    $('#ddlConnections').html('<option value="">Select</option>');
                    for (var i = 0; i < json.connectionList.length; i++) {
                        var con = json.connectionList[i];
                        $('#ddlConnections').append('<option value="' + con.id + '">' + con.text + '</option>');
                    }
                }
            },
            error: function (err, status) {
                ShowMessage('alert-danger', err.statusText);
            }
        });
    });
});

var oTable;
function getGrid() {
    oTable = $('#table-reportList').DataTable({
        "ajax": SBIReportUtility.Url.root + "Report/List",
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
        "<button onclick='fnDeleteReport(" + rowData.Id + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Delete Report' > <i class='fa fa-trash'></i></button> &nbsp;" +
        "<a href='" + SBIReportUtility.Url.root + "Report/DownloadReport?reportId=" + rowData.Id + "' target='_blank' class='btn btn-primary btn-circle' data-toggle='tooltip' data-placement='left' data-original-title='Download report' ><i class='fa fa-download'></i></a>";
}

function fnAddEditReport(reportId) {
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'Report/ShowAddEditPopup?reportId=' + reportId + '&nocache=' + new Date().getTime(),
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
            url: SBIReportUtility.Url.root + 'Report/ActivateDeactivateReport?reportId=' + reportId + '&nocache=' + new Date().getTime(),
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
            url: SBIReportUtility.Url.root + 'Report/DeleteReport?reportId=' + reportId + '&nocache=' + new Date().getTime(),
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