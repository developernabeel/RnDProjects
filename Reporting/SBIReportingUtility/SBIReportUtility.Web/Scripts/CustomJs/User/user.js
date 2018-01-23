$(document).ready(function () {
    getGrid();
});

var oTable;
function getGrid() {
    oTable = $('#tbl-users').DataTable({
        "ajax": SBIReportUtility.Url.root + "User/List",
        "columnDefs": [
            {
                "targets": [4],
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
            { data: "Action", title: "User Profile" }
        ],
        responsive: true
    });
}

function fnShowAddPopup() {
    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'User/ShowAddPopup?nocache=' + new Date().getTime(),
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
        url: SBIReportUtility.Url.root + 'User/GetUserByPfid?pfid=' + pfid,
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
    return "<a href='" + SBIReportUtility.Url.root + "User/UserProfile?pfid=" + rowData.Pfid + "' class='btn btn-primary btn-circle' data-toggle='tooltip' data-original-title='View Profile' ><i class='fa fa-eye'></i></a> &nbsp;";
        //"<button onclick='fnDeleteUser(" + rowData.Id + ");' class='btn btn-danger btn-circle' data-toggle='tooltip' data-original-title='Delete'> <i class='fa fa-trash-o'></i></button>";
}