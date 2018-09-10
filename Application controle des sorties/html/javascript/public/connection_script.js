
	var connection_data = (function() {
	
	var txtType ;
	const default_data_text_to_rotate =[ "Bienvenue", "Se connecter ici"];
	const COLOR_ORANGE ="#ff944d";
	const COLOR_RED="#ff3300";

	
		function submitArrayMouseOver()
		{
			document.getElementById("display_connection_info_span").style.backgroundColor= "#215C59";
		}
		
		function submitArrayMouseOut()
		{
			document.getElementById("display_connection_info_span").style.backgroundColor= "transparent";
		}
		
		
		function submitArrayClick()
		{
			if(!document.getElementById("accept_terme_of_use_checkbox").checked)
			{
				document.getElementById("display_use_of_terme_span").style.color="red";
				alert("Veuillez accepter les conditions d'utilisation");
				return;
				
			}else if ( DataRequest.stringVoidOrEmpty(document.getElementById("input_id_connexion").value) 
				    || DataRequest.stringVoidOrEmpty(document.getElementById("input_password_connexion").value))
			{
				alert("Veuillez remplir tous les champs de connexion avant de soumettre votre demande");
				return;
			}
			
			
			txtType.changeTextToRotate([ "Verification..."]);
			document.getElementById("display_connection_info_span").style.color=COLOR_ORANGE;
		
			var data = DataRequest.setCertifiedConnection(document.getElementById("input_id_connexion").value,document.getElementById("input_password_connexion").value);
			DataRequest.sendPostRequest(data,setChange);
			
			//reset fields
			document.getElementById("input_id_connexion").value="";
			document.getElementById("input_password_connexion").value ="";
			
			
			
		}
		

		function setChange(response) {
			try {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_TAG_ESTABLISH_CONNECTION)
			  {
				  
				  if(data[GlobalDefinition.REQUEST_RESULT_IDENTIFICATION]==false)
				  {
					  txtType.changeTextToRotate([ "ERREUR...","Veuillez réessayer"]);
					  document.getElementById("display_connection_info_span").style.color=COLOR_RED;

				  }else
				  {
					  var last_name = data[GlobalDefinition.REQUEST_AGENT_LAST_NAME];
					  var first_name = data[GlobalDefinition.REQUEST_AGENT_FIRST_NAME];
					  var job = data[GlobalDefinition.REQUEST_AGENT_JOB];
					  
					  
					  document.getElementById("display_session_info_span").innerText = DataRequest.getFormatedSessionInfo(last_name, first_name, job);
					  
					  MainMenu.clickOnSchoolOfficeIcon();
				  }
				  
				  var message = data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY];
				  if(message !="")
				  {
					  alert(message);
				  }
				  
			  }

			} catch (e) {}
		  
		}
		
		function downloadLicence()
		{

			DataRequest.downloadFileFromServer(GlobalDefinition.AJAX_DOWNLOAD_FILE + GlobalDefinition.FILE_NAME_LICENCE,GlobalDefinition.FILE_NAME_LICENCE);
		}
		

	
    return {
        
		initialize : function ()
		{

			var el = document.getElementById("display_connection_info_span");
			var textToRotate = default_data_text_to_rotate;
			var periode = 2000;
			txtType = new TxtTypeAnnimation(el, textToRotate, periode );
		    txtType.start();
			
			document.getElementById("div_before_display_info_span").addEventListener("mouseover",submitArrayMouseOver);
			document.getElementById("div_before_display_info_span").addEventListener("mouseout",submitArrayMouseOut);
			document.getElementById("div_before_display_info_span").addEventListener("click",submitArrayClick);

			
			document.getElementById("link_to_licence").addEventListener("click",downloadLicence);
			document.getElementById("icon_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.ICON_IMAGE);
			
			document.getElementById("title_span").innerText = GlobalDefinition.SCHOOL_NAME;
		},
		destroy : function ()
		{

			txtType.stop();
			delete this.txtType;
			
		},
		
		
    };

})();


	connection_data.initialize();
	
	function destroy()
	{
			connection_data.destroy();
			delete this.connection_data;
	}
		