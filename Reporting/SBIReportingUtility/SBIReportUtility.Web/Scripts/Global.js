// Global application namespace.
var SBIReportUtility = SBIReportUtility || {};

var GlobalData = {};
GlobalData.Timeout;
function ShowMessage(CssClass, text)
{
    ShowMessageWithTimer(CssClass, text, 6000);
}

function ShowMessageWithTimer(CssClass, text, timeInMilliSeconds) {
    $('#statusContainer').removeClass("alert-info alert-success alert-warning alert-danger");
    $('#statusContainer').addClass(CssClass);
    $("._statusMsgContent").html(text);

    $('#statusContainer').show();
    clearTimeout(GlobalData.Timeout);
    GlobalData.Timeout = setTimeout(function () {
        $('#statusContainer').hide('slow');
        $('#statusContainer').removeClass(CssClass);
        $("._statusMsgContent").html('');
    }, timeInMilliSeconds);
}

// Notifications Module.
SBIReportUtility.Notifications = (function () {
    // Default timeout is 3 seconds.
    var notificationTimeout = 3000;

    // Default fadeOut duration is 1 second.
    var fadeOutDuration = 1000;

    // Public methods.
    return {
        fadeOutNotification: function () {
            setTimeout(function () {
                $('.dismiss-notification').fadeOut(1000);
            }, notificationTimeout);
        }
    };
})();

// Ajax Loader Module.
SBIReportUtility.AjaxLoader = (function () {
    // Loader is enabled by default.
    var enableLoader = true;
    
    // Public methods.
    return {
        initLoader: function () {
            var p = document.querySelector('.path'),
            offset = 0;

            var offsetMe = function () {
                if (offset < -100) offset = 0;
                p.style.strokeDashoffset = offset;
                offset--;
                requestAnimationFrame(offsetMe);
            }
            offsetMe();
        },
        show: function () {
            if (enableLoader)
                $('#ajax-loader').show();
        },
        hide: function () {
            $('#ajax-loader').hide();
        },
        setEnable: function (value) {
            enableLoader = value;
        }
    };
})();

$(function () {
    SBIReportUtility.Notifications.fadeOutNotification();
    SBIReportUtility.AjaxLoader.initLoader();

    $(document)
    .ajaxStart(function () {
        SBIReportUtility.AjaxLoader.show();
    })
    .ajaxStop(function () {
        SBIReportUtility.AjaxLoader.hide();
    });

    $('body').tooltip({ selector: '[data-toggle="tooltip"]' });
});