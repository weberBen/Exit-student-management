
var AccountVerification =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
		  var sensitive_data_type="";
		  var json_data_to_send="";
		  var temp_container;
		  var content_loaded="";
		  var process_request=false;
		  var txtType;
		  var result_process_image_element;
		  var result_process_txt_element;
		  
		  
		  function loadContentIntoVar()
		  {
			  $.get(GlobalDefinition.AJAX_HTML_CONTENT + GlobalDefinition.FILE_NAME_ACCOUNT_VERIFICATION_HTML_CONTENT, 
						function( data ) 
						{
							content_loaded = data;
							initialize ();
						});
		  }
	  
		function showErrorToUser()
		{
			result_process_image_element.style.display="block";
			result_process_txt_element.style.display="block";
			
			result_process_image_element.setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.ERROR_IMAGE);
			result_process_txt_element.innerText = "ERREUR";
		}
		
		
		function showNoErrorToUser()
		{
			result_process_image_element.style.display="block";
			result_process_txt_element.style.display="block";
			
			result_process_image_element.setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.NO_ERROR_IMAGE);
			result_process_txt_element.innerText = "ENREGISTRÉ";
		}
		
		
	  function setChange(response)
	  {
		  try {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_SENSITIVE_DATA)
			  {
				  if(data[GlobalDefinition.REQUEST_RESULT_AUTHENTIFICATION_FOR_SENSITIVE_DATA]==true)
				  {
					
					  if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
					  {
						  showErrorToUser();
					  }else
					  {
						  showNoErrorToUser();
						  ToolBarSchoolOffice.reloadCurrentPage();
						  setTimeout(function() {removeFromDocument();},1000);
					  }
					
				  }else
				  {
					  showErrorToUser();
					  alert("Erreur : il est possible que le code entré soit incorrect ou que vous ne disposez pas des droits nécessaires pour effectuer cette opération ");
				  }
				  
				  
				  var message = data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY];
				  if(message !="")
				  {
					  alert(message);
				  }
				  
			  }
			  

			} catch (e) {}
		  
		  unFreezeContent();
	  }
	  

	  
	  function inputValueChange(elem, value)
	  {
		  if( (value == null) || value.replace(/\s/g, '').length == 0)
		  {
			  return;
		  }
		  
		  if(value.length == GlobalDefinition.LENGTH_SHORT_SECURE_ID)
		  {
			  freezeContent();
			  elem.value = "";
			  
			  var data = DataRequest.setSensitiveData(value,sensitive_data_type, json_data_to_send);
			  DataRequest.sendPostRequest(data, setChange);
		  }
	  }
	  
	  
	  function freezeContent()
	  {
		  txtType.start();
		  process_request=true;
		  $("#cancel_button_account_verification").prop('disabled', true);
		  $("#short_id_input_account_verification").prop('disabled', true);
	  }
	  
	  function unFreezeContent()
	  {
		  txtType.stop();
		  process_request=false;
		  $("#cancel_button_account_verification").prop('disabled', false);
		  $("#short_id_input_account_verification").prop('disabled', false);
		  document.getElementById("short_id_input_account_verification").focus();
	  }
	  
	  function removeFromDocument()
	  {
		  if(process_request==false)
		  {
			  result_process_image_element.style.display="none";
			  result_process_txt_element.style.display="none";
			  temp_container.style.display = "none";
			  result_process_image_element.setAttribute("src", "");
			  document.getElementById("short_id_input_account_verification").value="";
			  txtType.stop();
			  document.addEventListener('keydown',documentKeyPress);
			  document.getElementById("short_id_input_account_verification").removeEventListener("input", function (evt) {inputValueChange(this, this.value);});
			  document.getElementById("cancel_button_account_verification").removeEventListener("click", onCancelButtonClick);
			  document.removeEventListener('keydown',documentKeyPress);
		  }
		  
	  }
	  
	  function onCancelButtonClick()
	  {
		  removeFromDocument();
	  }
	  
	  function documentKeyPress(e)
	  {
		  var keynum;
		  if(window.event) 
		  { // IE                    
			   keynum = e.keyCode;
		  } else if(e.which)
		  { // Netscape/Firefox/Opera                   
			   keynum = e.which;
		  }
		  
		  if(keynum === 27)//escape pressed
		  {
			  removeFromDocument();
		  }
		  
		  return true;
	  }
	  

	  
	  function initialize ()
	  {
		  //load content into a div of body
		  
		  if(GlobalDefinition.BODY_COLOR==null)
		  {
			  //because this script is launched before the document has been created (so at that step no body color because not body element)
			  setTimeout(function() {initialize();},100);
			  return;
		  }
		  
		  
		  temp_container = document.createElement("DIV");
		  temp_container.innerHTML = content_loaded;
		  temp_container.style= "position: absolute;height: 100%;width: 100%;left: 0%;top: 0%;";
		  temp_container.style.backgroundColor = GlobalDefinition.BODY_COLOR;
		  temp_container.style.opacity = "0.95"; 
		  temp_container.style.display = "none";
		  document.body.appendChild(temp_container); 
		  
		  result_process_image_element = document.getElementById("result_porcess_account_verification_image");
	      result_process_txt_element = document.getElementById("message_process_account_verification_span");
		  
		  var el = document.getElementById("waiting_short_id_verification_span_account_verification");
		  var textToRotate = ["Vérification","..."];
		  var periode = 2000;
	      txtType = new TxtTypeAnnimation(el, textToRotate, periode );

	  }
	  
	  
	  
        return { //all those function are available outside the module

		  initializeParms : function ()
	      {
			  loadContentIntoVar();
		  },
		
		
		  start : function (type_of_sensitive_data, json_data)
		  {
			  
			  sensitive_data_type = type_of_sensitive_data;
			  json_data_to_send = json_data;
			  
			  result_process_image_element.style.display="none";
			  result_process_txt_element.style.display="none";
			  temp_container.style.display = "block";
			  document.addEventListener('keydown',documentKeyPress);
			  document.getElementById("short_id_input_account_verification").addEventListener("input", function (evt) {inputValueChange(this, this.value);});
			  document.getElementById("cancel_button_account_verification").addEventListener("click", onCancelButtonClick);
		  
			  document.getElementById("short_id_input_account_verification").focus();
		  },
		  
		  stop : function ()
		  {
			  removeFromDocument();
			  
		  },

		  destroy : function()
		  {
		  },
		  
		  sendCriticalData : function(type_of_sensitive_data , data)
		  {
			  sensitive_data_type = type_of_sensitive_data;
			  data_to_send = data;
		  },
		  
		  test : function()
		  {
			return process_request;  
		  },
        };
      })();


	  AccountVerification.initializeParms();
	  
	  
	  
	  
	  
	  
	
    

    