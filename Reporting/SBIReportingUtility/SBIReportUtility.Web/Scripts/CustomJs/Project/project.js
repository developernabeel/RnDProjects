$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    oTable = $('#table-projectList').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/List",
        //data: data,
        "columnDefs": [
            {
            "targets": [0],
            "visible": false,
            "searchable":false
            },
            {
                "targets": [3],
                "render": function (data, type, row) {
                    return getActionButtonsHtml(row);
                }
            }
        ],
        "columns": [
            { data: "ProjectName" ,title:"Name"},
            { data: "Description", title: "Description" },
            { data: "IsActive", title: "Status" },
            { data: "Action", title: "Actions" }
            
        ],
        responsive: true
    });
}

function fnActivateDeactivateProject(projectId, isActive)
{
    $('#projectConfirm').find('#hdnProjectId').val(projectId);
    var txtMessage = isActive ? 'Are you sure you want to deactivate this project?' : 'Are you sure you want to activate this project?';
    $('#txt-modalmsg').text(txtMessage);
    $('#projectConfirm').modal('show');
    $('#projectConfirm').find('#btnConfirm').unbind('click').bind('click', function () {
        $.ajax({
            type: "POST",
            url: SBIReportUtility.Url.root + 'Project/ActivateDeactivateProject?projectId=' + projectId + '&nocache=' + new Date().getTime(),
            success: function (data) {
                if (data != null) {
                    ShowMessage("alert-success", data.result);
                    $('#projectConfirm').modal('hide');
                    $('#projectConfirm').find('#hdnProjectId').val('');
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

function fnAddEditProject(projectId)
{
    $.ajax({
        type: "GET",
        url: SBIReportUtility.Url.root + 'Project/ShowAddEditPopup?projectId=' + projectId + '&nocache=' + new Date().getTime(),
        success: function (status) {
            if (status != null) {
                $('#projectModal').html(status);
                $.validator.unobtrusive.parse($("#projectModal"));
                $('#projectModal').modal('show');
                $('#inputProjectName').focus();
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnOnProjectSuccess(data)
{
    ShowMessage(data.cssClass, data.result);
    $('#projectModal').modal('hide');
    if (oTable) {
        oTable.ajax.reload();
    }
    else {
        getGrid();
    }
}

function getActionButtonsHtml(rowData) {
    var isActive = rowData.IsActive == 'Active';
    var activeClass = rowData.IsActive == 'Active' ? 'btn-danger' : 'btn-success';
    var activeText = rowData.IsActive == 'Active' ? 'Deactivate project' : 'Activate project';
    var activeIcon = rowData.IsActive == 'Active' ? 'fa fa-times' : 'fa fa-check';
    return "<button onclick='fnActivateDeactivateProject(" + rowData.Id + ", " + isActive + ");' class='btn " + activeClass + " btn-circle' data-toggle='tooltip' data-original-title='" + activeText + "'> <i class='" + activeIcon + "'></i></button>&nbsp;" +
        "<button onclick='fnAddEditProject(" + rowData.Id + ");' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Edit project' > <i class='fa fa-pencil'></i></button>";
}

