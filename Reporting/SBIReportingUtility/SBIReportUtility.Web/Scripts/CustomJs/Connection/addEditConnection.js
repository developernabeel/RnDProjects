function testConnection() {
    if (!$("#frmConnection").valid())
        return;

    var connectionInfo = {
        ConnectionName: $('#txtConnectionName').val(),
        ProjectId: $('#ddlProjects').val(),
        SID: $('#txtSID').val(),
        IpAddress: $('#txtIpAddress').val(),
        PortNumber: $('#txtPortNumber').val(),
        ConnectionUsername: $('#txtUserName').val(),
        ConnectionPassword: $('#txtPassword').val()
    };

    $.ajax({
        type: "POST",
        url: SBIReportUtility.Url.root + 'Connection/TestConnection',
        data: connectionInfo,
        success: function (json) {
            if (json.Success) {
                ShowMessage('alert-success', json.Message);
            }
            else {
                ShowMessage('alert-danger', json.Message);
            }
        },
        error: function (err, status) {
            ShowMessage('alert-danger', err.statusText);
        }
    });
}