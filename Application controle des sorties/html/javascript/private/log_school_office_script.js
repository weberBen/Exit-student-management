


var LogSchoolOffice =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  var log_element;
	  var log_txt="";
	  
	  function updateLogTxt(txt)
	  {
		  log_txt = txt;
		  document.getElementById("log_textarea").value = log_txt;
	  }
	  
	  
	  function setChange(response)
	  {
		  try {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_GET_LOG)
			  {
				  updateLogTxt(data[GlobalDefinition.REQUEST_RESULT]);
			  }else
			  {
				  updateLogTxt("ERREUR");
			  }
			  

			} catch (e) 
			{
				updateLogTxt("ERREUR");
			}
		  
	  }
	  
		function downloadLogOnClick()
		{
			var filename = ("registre_" + ToolBarSchoolOffice.getFormatedTextInfo().replace(/\s/g, '_')).toUpperCase(); //replace ' ' by '_'
			log_txt = "GÉNÉRÉ LE : " + DataRequest.dateToString(new Date()) +"\n\n\n" + log_txt;
			DataRequest.downloadFileFromTxt(filename,log_txt);
		}
		
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			  var [type_of_table, index] = ToolBarSchoolOffice.getTypeAndIndex();
			  DataRequest.sendPostRequest(DataRequest.setLogRequest(type_of_table,index) , setChange);
			  
			  log_element = document.getElementById("log_textarea");
			  updateLogTxt("CHARGEMENT");
			  
			  document.getElementById("download_log_button").addEventListener("click",downloadLogOnClick);

		  },
		  
		  destroy : function()
		  {
		  },
        };
      })();

	  
	  LogSchoolOffice.initialize();
	  
	  function subDestroy()
	  {
		  LogSchoolOffice.destroy();
		  delete this.LogSchoolOffice;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    