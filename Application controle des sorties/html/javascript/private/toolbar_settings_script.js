


var ToolBarSettings =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  var list_item=[];
	  var list_content=[];
	  var list_script=[];
	  
	  function reset()
	  {
		   try 
          {
            subDestroy();
          } catch (e) {}
          /* function in each javascript that delete from memory all listner added and variable
             * If the file is loaded for the fisrt time there is no destroy function (that why there is try{}
          */
          document.getElementById("content_element").innerHTML = GlobalDefinition.CONTENT_WAITING_PAGE;  
	  }
	  
	  
	  function loadContent(file_name_content, file_name_script)
	  {
		  reset();
		  $("#content_element").load(GlobalDefinition.AJAX_HTML_CONTENT + file_name_content, function() {
		  DataRequest.loadScript(document.body, "sub_script", GlobalDefinition.AJAX_JAVASCRIPT_TAG +  file_name_script);});
	  }
	  
	  
	  function clickOnItem(el)
	  {//click on item of the toolbar 
		  for(i=0; i<list_item.length;i++)
		  {
			  if(list_item[i] != el)
			  {
				 list_item[i].style.backgroundColor="transparent"; 
			  }else
			  {
				 list_item[i].style.backgroundColor=GlobalDefinition.BODY_COLOR;
				 loadContent(list_content[i], list_script[i]);
			  }			  
		  }   
	  }
	  
	  
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			//load images
			document.getElementById("upload_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.UPLOAD_IMAGE);
			document.getElementById("account_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.ACCOUNT_IMAGE);
			
			list_item.push(document.getElementById("upload_image").parentElement);
			list_item.push(document.getElementById("account_image").parentElement);
			
			list_content.push(GlobalDefinition.FILE_NAME_UPLOAD_FILES_HTML_CONTENT);
			list_content.push(GlobalDefinition.FILE_NAME_USER_ACCOUNT_HTML_CONTENT);
			
			list_script.push(GlobalDefinition.FILE_NAME_UPLOAD_FILES_SCRIPT);
			list_script.push(GlobalDefinition.FILE_NAME_USER_ACCOUNT_SCRIPT);
			
			list_item[0].addEventListener("click", function() { clickOnItem(list_item[0]); });
		    list_item[1].addEventListener("click", function() { clickOnItem(list_item[1]); });
			
			clickOnItem(list_item[0]);
		  },
		  
		  destroy : function()
		  {
		  },
        };
      })();

	  
	  ToolBarSettings.initialize();
	  
	  function Destroy()
	  {
		  ToolBarSettings.destroy();
		  delete this.ToolBarSettings;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    