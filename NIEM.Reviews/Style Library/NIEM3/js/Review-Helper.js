    _spBodyOnLoadFunctionNames.push("HandlePageLoad");   

    function HandlePageLoad()   
    {
        var newformExpression = "/Lists/Reviews/NewForm.aspx";
        var editFormExpression = "/Lists/Reviews/EditForm.aspx";
        if (newformExpression == window.location.pathname)
        {
            ExecuteOrDelayUntilScriptLoaded(PopulateFields, 'sp.ui.dialog.js');
        }

        if (editFormExpression == window.location.pathname)
        {
            ExecuteOrDelayUntilScriptLoaded(HideFields, 'sp.ui.dialog.js');
        }
        
    }

    function HideFields()
    {
        var urlField = document.getElementById('ctl00_ctl37_g_7dbff10e_05f2_4051_9918_1121e4a6c7dd_ctl00_ctl05_ctl01_ctl00_ctl00_ctl04_ctl00_ctl00_TextField');
        urlField.readOnly = true;
    }

    function PopulateFields()
    {
        
      var dialog = SP.UI.ModalDialog.get_childDialog();
      if(dialog != null)
      {
      	var args = dialog.get_args();
      	var urlField = document.getElementById('ctl00_ctl37_g_80dc30e1_0c2e_46d1_b606_b12ccc3bcdd8_ctl00_ctl05_ctl01_ctl00_ctl00_ctl04_ctl00_ctl00_TextField');
		
		if(urlField != null)
		{
		    urlField.value = args.articleUrl;
		    urlField.readOnly = true;
      	}
       
    
      }
      
      //alert(args.ListId);
    } 
