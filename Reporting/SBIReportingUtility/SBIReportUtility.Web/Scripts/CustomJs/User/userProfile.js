$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    var pfid = $('#hdnPfid').val();
    oTable = $('#tbl-projects').DataTable({
        "ajax": SBIReportUtility.Url.root + "User/ProjectList?pfid=" + pfid,
        "columnDefs": [
            {
                "targets": [2],
                "render": function (data, type, row) {
                    return getIsAdminButtonHtml(row);
                }
            },
            {
                "targets": [3],
                "title": "Unassign Project",
                "render": function (data, type, row) {
                    return getActionButtonsHtml(row);
                }
            }
        ],
        "columns": [
            { data: "ProjectName", title: "Project Name" },
            { data: "Description", title: "Description" },
            { data: "IsProjectAdmin", title: "Is Admin" }
        ],
        "order": [[0, "asc"]],
        responsive: true
    });
}

function fnShowAssignProjectPopup() {
    var pfid = $('#hdnPfid').val();
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'User/ShowAssignProjectPopup?pfid=' + pfid + '&nocache=' + new Date().getTime(),
        success: function (status) {
            if (status != null) {
                $('#projectModal').html(status);
                $.validator.unobtrusive.parse($("#projectModal"));
                var userPfid = $('#hdnPfid').val();
                $('#hdnPopupPfid').val(userPfid)
                $('#projectModal').modal('show');
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnToggleIsAdmin(mappingId, isProjectAdmin) {
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'User/UpdateProjectAdminStatus?id=' + mappingId + '&isAdmin=' + isProjectAdmin,
        success: function (json) {
            if (json.success) 
                ShowMessage('alert-success', json.message);
            else
                ShowMessage('alert-danger', json.message);

            if (oTable) {
                oTable.ajax.reload();
            }
            else {
                getGrid();
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}

function fnUnassignProject(mappingId) {
    $('#unassign-modal').modal('show');
    $('#unassign-modal').find('#btn-confirm').unbind('click').bind('click', function () {
        $.ajax({
            type: "POST",
            url: SBIReportUtility.Url.root + 'User/UnassignProject?id=' + mappingId,
            success: function (json) {
                if (json.success) {
                    $('#unassign-modal').modal('hide');
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
            },
            error: function (err, status) {
                ShowMessage('alert-danger', err.statusText);
            }
        });
    });
}

function fnOnSuccess(json) {
    if (json.success) {
        $('#projectModal').modal('hide');
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

function getIsAdminButtonHtml(rowData) {
    var pfid = $('#hdnPfid').val();

    var title = rowData.IsProjectAdmin == 1 ? 'Revoke Admin right' : 'Grant Admin right';
    var isChecked = rowData.IsProjectAdmin == 1 ? 'checked="checked"' : '';

    return '<input id="chk-admin-' + rowData.Id + '" class="checkbox-custom" type="checkbox"' + ' onclick="fnToggleIsAdmin(' + rowData.MappingId + ', ' + rowData.IsProjectAdmin + ')" ' + isChecked + ' title="' + title + '">' +
            '<label for="chk-admin-' + rowData.Id + '" class="checkbox-custom-label"><span></span></label>';
}

function getActionButtonsHtml(rowData) {
    return "<button onclick='fnUnassignProject(" + rowData.MappingId + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Unassign Project'> <i class='fa fa-times'></i></button>";
}