var urlField = null;
var context = null;
var web = null;

$(document).ready(function ()
{
    urlField = $('input[title="Url"]');
    HideFields();
});
_spBodyOnLoadFunctionNames.push("HandlePageLoad");



function HandlePageLoad()
{
   
    var newformExpression = "/Lists/Reviews/NewForm.aspx";
    var editFormExpression = "/Lists/Reviews/EditForm.aspx";
    if (newformExpression == window.location.pathname)
    {
        ExecuteOrDelayUntilScriptLoaded(PopulateFields, 'sp.ui.dialog.js');
    }

}

function HideFields()
{
    $(urlField).closest("tr").hide();
    $(urlField).css('border', '0px');
    $(urlField).css('background-color', 'transparent');
    $(urlField).attr('readOnly', true);
}


function PopulateFields()
{

    var dialog = SP.UI.ModalDialog.get_childDialog();
    if (dialog != null)
    {
        var args = dialog.get_args();
        if ($(urlField) != null)
        {
            $(urlField).val(args.articleUrl);
        }

    }

} 
