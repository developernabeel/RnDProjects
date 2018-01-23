$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    var projectId = $('#hdnProjectId').val();
    oTable = $('#tbl-users').DataTable({
        "ajax": SBIReportUtility.Url.root + "Project/ProjectUsersList?projectId=" + projectId,
        "columnDefs": [
            {
                "targets": [4],
                "render": function (data, type, row) {
                    return row.IsAdmin ? 'Yes' : 'No';
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
            { data: "Pfid", title: "PFID" },
            { data: "Name", title: "Name" },
            { data: "Email", title: "Email Id" },
            { data: "Designation", title: "Designation" },
            { data: "IsAdmin", title: "Is Admin" },
            { data: "Action", title: "Action" }
        ],
        responsive: true
    });
}

function fnShowAddPopup() {
    var projectId = $('#hdnProjectId').val();
    $.ajax({
        type: "GET",
        url: SBIReportUtility.Url.root + 'Project/ShowAddProjectUserPopup?projectId=' + projectId + '&nocache=' + new Date().getTime(),
        success: function (status) {
            if (status != null) {
                $('#userModal').html(status);
                $.validator.unobtrusive.parse($("#userModal"));
                $('#userModal').modal('show');
                $('#userModal').find('input.form-control').val('');
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnSearchUser() {
    if (!$('#frmUser').valid())
        return;

    var pfid = $('#txtPfid').val();
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'Project/SearchUserByPfid?pfid=' + pfid,
        success: function (json) {
            if (json.success) {
                $('#txtName').val(json.user.Name);
                $('#txtEmail').val(json.user.EmailId);
                $('#txtDesignation').val(json.user.Designation);
            }
            else
                ShowMessage('alert-danger', json.message);
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnOnSuccess(json) {
    if (json.success) {
        $('#userModal').modal('hide');
        ShowMessage('alert-success', json.message);
    }
    else
        ShowMessage('alert-danger', json.message);

    if (oTable) {
        oTable.ajax.reload();
    }
    else {
        getGrid();
    }
}

function getActionButtonsHtml(rowData) {
    if (rowData.IsAdmin) 
        return "";

    var projectId = $('#hdnProjectId').val();
    return "<a href='" + SBIReportUtility.Url.root + "Project/AssignReports?projectId=" + projectId + "&pfid=" + rowData.Pfid
        + "' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='Assign Reports'> <i class='fa fa-file'></i></a>";
}