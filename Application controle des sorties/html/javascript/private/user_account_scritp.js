


var UserAccount =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  function setChange(response)
	  {
		  txtType.stop();
		  try {
			  var data = JSON.parse(response);
			  DataRequest.getSystemInstructions(data);//manage system instructions

			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_CHANGE_PASSWORD)
			  {
				  if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
				  {
					  alert(data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY]);
				  }else
				  {
					  alert("Les modification ont été enregistré");
					  location.reload();//agent has been disconnected from the server side
				  }  
			  }

			} catch (e) {}
		  
	  }
	  
	  
	  function onSubmitButtonClick()
	  {
		  if(document.getElementById("new_password_input").value != document.getElementById("confirmation_new_password_input").value)
		  {
			  alert("Les mots de passes ne correspondent pas");
			  return;
		  }else if(document.getElementById("new_password_input").value=="" || document.getElementById("confirmation_new_password_input").value=="")
		  {
			  alert("Veuillez saisir un nouveau mot de passe");
			  return;
		  }else if(document.getElementById("id_input").value=="" || document.getElementById("password_input").value=="")
		  {
			  alert("Veuillez saisir vos informations de connexion");
			  return;
		  }
		  
		  txtType.start();
		  DataRequest.sendPostRequest(DataRequest.setNewAgentPassword(document.getElementById("id_input").value, document.getElementById("password_input").value, document.getElementById("new_password_input").value),setChange);
				
		  document.getElementById("new_password_input").value="";
		  document.getElementById("confirmation_new_password_input").value="";
		  document.getElementById("id_input").value="";
		  document.getElementById("password_input").value="";
		  
		  document.getElementById("new_password_input").focus();
	  }
	  
	  
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			var el = document.getElementById("waiting_for_result_array");
			var textToRotate = ["Veuillez patentier", "..."];
			var periode = 2000;
			txtType = new TxtTypeAnnimation(el, textToRotate,periode );
			
			
			document.getElementById("submit_button").addEventListener("click", onSubmitButtonClick);
			
			document.getElementById("new_password_input").focus();
		  },
		  
		  destroy : function()
		  {
			  txtType.stop();
			  delete this.txtType;
		  },
        };
      })();

	  
	  UserAccount.initialize();
	  
	  function subDestroy()
	  {
		  UserAccount.destroy();
		  delete this.UserAccount;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    