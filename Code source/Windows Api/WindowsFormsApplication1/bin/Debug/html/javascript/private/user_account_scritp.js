


var UserAccount =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
		var display_short_id_element;
		const TIME_DIPSLAY_SHORT_ID = 10000;//ms
		var timer = 0;
	  
	  function resetTimer()
	  {
		  if(timer)
			 {
				clearTimeout(timer);
				timer = 0;
			 }
	  }
	  
	  function setChange(response)
	  {
		  txtType.stop();
		  resetTimer();
		  
		  try {
			  var data = JSON.parse(response);
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
			  {
				  alert(data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY]);
			  }
				  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_CHANGE_PASSWORD)
			  {
				  
				  alert("Les modification ont été enregistré");
				  location.reload();//agent has been disconnected from the server side
				  
			  }else if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_CHANGE_SHORT_ID)
			  {
				  display_short_id_element.value = data[GlobalDefinition.REQUEST_NEW_SHORT_ID];
				  timer = setTimeout(function(){ display_short_id_element.value = ""; },TIME_DIPSLAY_SHORT_ID);
			  }

			} catch (e) {}
		  
	  }
	  
	  function checkConnectionInfos()
	  {
		  return document.getElementById("id_input").value=="" || document.getElementById("password_input").value=="";
	  }
	  
	  function resetParms()
	  {
		  document.getElementById("id_input").value="";
		  document.getElementById("password_input").value="";
		  
		  document.getElementById("id_input").focus();
	  }
	  
	  
	  function newUserPassword()
	  {
		  if(document.getElementById("new_password_input").value != document.getElementById("confirmation_new_password_input").value)
		  {
			  alert("Les mots de passes ne correspondent pas");
			  return;
		  }else if(document.getElementById("new_password_input").value=="" || document.getElementById("confirmation_new_password_input").value=="")
		  {
			  alert("Veuillez saisir un nouveau mot de passe");
			  return;
		  }else if(checkConnectionInfos())
		  {
			  alert("Veuillez saisir vos informations de connexion");
			  return;
		  }
		  
		  txtType.start();
		  DataRequest.sendPostRequest(DataRequest.setNewAgentPassword(document.getElementById("id_input").value, document.getElementById("password_input").value, document.getElementById("new_password_input").value),setChange);
				
		  document.getElementById("new_password_input").value="";
		  document.getElementById("confirmation_new_password_input").value="";
		  resetParms();
		  document.getElementById("new_password_input").focus();
	  }
	  

	  
	  function newUserIdTag()
	  {
		  if(checkConnectionInfos())
		  {
			  alert("Veuillez saisir vos informations de connexion");
			  return;
		  }
		  
		  txtType.start();
		  DataRequest.sendPostRequest(DataRequest.setNewAgentShortId(document.getElementById("id_input").value, document.getElementById("password_input").value),setChange);
				
		  resetParms();
	  }
	  
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			var el = document.getElementById("waiting_for_result_array");
			var textToRotate = ["Veuillez patentier", "..."];
			var periode = 2000;
			txtType = new TxtTypeAnnimation(el, textToRotate,periode );
			
			display_short_id_element = document.getElementById("short_id_input");
			
			document.getElementById("submit_pwd_button").addEventListener("click", newUserPassword);
			document.getElementById("get_code_button").addEventListener("click", newUserIdTag);
			
			document.getElementById("id_input").focus();
		  },
		  
		  destroy : function()
		  {
			  resetTimer();
			 
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
	  
	  
	  
	  
	  
	  
	  
	
    

    