
var DoorScript = (function() {
	const HtmlColorNoExit = '#ff004b';
	const HtmlColorExit = '#26DCB5';
	const HtmlColorError = '#ffad33';
	const HtmlColorNetworkError ='#df7bdd';
	
	const text_exit_unauthorized = "SORTIE INTERDITE"; 
	const text_exit_authorized_under_condition = "SORTIE AUTORISÉE APRÈS VÉRIFICATION";
	const text_exit_authorized = "SORTIE AUTORISÉE";
	const text_no_matche ="AUCUNE CORRESPONDANCE TROUVÉE";
	const text_error="UNE ERREUR EST SURVENUE SUR LE SERVER !";
	const wait_message = "Attente de réponse du serveur ...";
	
	const MAX_TIME_STUDENT_ON_SCREEN= 30000;//120000;//ms
	const SLOW_CONNECTION_TIMEOUT = 500;//ms
	const TIMEOUT=5000;//ms
	const REFRESH_TIME = 60000; //60ms
	
	const MAX_CHAR_SHOWN_TEXT_REASON = 50
	
	var rfid_input_value; 
	var clock_timer;
	var audio_player;
	var image_handler;
	var tab_rfid_char=[];
	
	var element_focus_out;
	var element_display_id;
	
	var RfidHandler = (function() {
	
		var rfid_value="";
		var work_in_process=false;
		var request_timer=0;
		var slow_connection_timer = 0;
		var start=Date.now();
		var refresh_view = true;
		
		
		function stopTimer() 
		{
			if(request_timer)
			 {
				clearTimeout(request_timer);
				request_timer = 0;
			 }
			 
			 if(slow_connection_timer)
			 {
				clearTimeout(slow_connection_timer);
				slow_connection_timer = 0;
			 }
		}
		
		function resetParms()
		{
			rfid_value="";
			work_in_process =false;
			stopTimer();
		}
		
		function requestTimeoutHandler()
		{
			setGraphicView(HtmlColorNetworkError,"Connexion au serveur interrompue..."); 
			/*if the server takes too much time to send a response, we reset the parms (espacially the rifd value)
			So if after that time server send the response, the result will not be display on the screen because 
			the rfid send will not match with the one seek (because it has been reset)
			*/
			resetParms();
		}
		
		function slowConnectionTimeoutHandler()
		{
			setGraphicView("#f0c1ef",wait_message); 
			//display message to user when the response take time to be send by the server
		}
		
		
		function setChange(response)
		{
			
		  try 
		  {
			  var data = JSON.parse(response);
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
			  { 
				  if(data[GlobalDefinition.REQUEST_ERROR_CODE]==GlobalDefinition.REQUEST_ERROR_CODE_EXTERNAL_DATABASE_OFFLINE)
				  {
					DoorScript.destroy();
					MainMenu.displayOfflineServicePage();
				  }else
				  {
					setGraphicView(HtmlColorError,text_error); 
				  }
				  
			  }else if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_TAG_RFID_DOOR)
			  {
				  if(rfid_value != data[GlobalDefinition.REQUEST_TAG_RFID_VALUE])
				  {//if the network connection is slow, infomartion from a previous request can be send after we send a new request
					  resetView();
					  throw null;
				  }

				  stopTimer();//stop timer

				  var last_name = data[GlobalDefinition.REQUEST_STUDENT_LAST_NAME];
				  var first_name = data[GlobalDefinition.REQUEST_STUDENT_FISRT_NAME];
				  var division = data[GlobalDefinition.REQUEST_STUDENT_DIVISION];
				  var halfBoardDay = data[GlobalDefinition.REQUEST_STUDENT_HALF_BOARD_DAY];
				  var exit_regime = data[GlobalDefinition.REQUEST_STUDENT_LABEL_EXIT_REGIME];
				  var file_photo = data[GlobalDefinition.REQUEST_STUDENT_PHOTO];
				  var extension_photo =  data[GlobalDefinition.REQUEST_STUDENT_PHOTO_EXTENSION];
				  var exit_authorization = data[GlobalDefinition.REQUEST_STUDENT_EXIT_AUTHORIZATION];
				  var exit_reason = data[GlobalDefinition.REQUEST_TEXT_REASON];
				  if(exit_reason.length>MAX_CHAR_SHOWN_TEXT_REASON)
				  {
					  exit_reason =  exit_reason.substring(0,MAX_CHAR_SHOWN_TEXT_REASON) + "...";
				  }
				  
				  if (last_name == "" || first_name == "" || division == "") 
				  {
					resetRfidInput();
					setGraphicView(HtmlColorNoExit,text_no_matche, play_song=true); 
					
				  }else
				  {
					
					  var background_color;
					  var text_to_display;
					  var play_song;
					  
					  switch(exit_authorization)
					  {
						  case GlobalDefinition.EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE:
							background_color = HtmlColorExit;
							text_to_display = text_exit_authorized;
							play_song=false;
						  break;
						  case GlobalDefinition.EXIT_AUTHORIZED_UNDER_CONDITION_VALUE:
							background_color = HtmlColorError;
							text_to_display = text_exit_authorized_under_condition;
							play_song=false;
						  break;
						  case GlobalDefinition.EXIT_UNAUTHORIZED_VALUE:
							background_color = HtmlColorNoExit;
							text_to_display = text_exit_unauthorized;
							play_song=true;
						  break;
						  default :
							background_color = HtmlColorError;
							text_to_display = text_error;
							play_song=false;
						  break  
						  
					  }
					  
					  setGraphicView(background_color,text_to_display +"<br/>"+exit_reason, play_song, last_name, first_name, division, halfBoardDay, exit_regime, file_photo, extension_photo);   
				  }
				  
				  work_in_process = false;
			  }
			  
		  }catch(e){}
		}
		
		return {
			
			destroy : function ()
			{
				resetParms();
			},
			
			InProcess : function()
			{
				return work_in_process;
			},
			
			getRfid : function()
			{
				return rfid_value;
			},
			
			setRfid : function(value)
			{
				if(work_in_process)
				{
					return;
				}
				
				resetView();
				
				work_in_process = true;
				refresh_view = false;
				rfid_value = value;
				start = Date.now();
				
				var data = DataRequest.setRfidRequestForDoor(rfid_value);
				DataRequest.sendPostRequest(data, setChange);
				  
				request_timer = setTimeout(requestTimeoutHandler, TIMEOUT);
				slow_connection_timer = setTimeout(slowConnectionTimeoutHandler, SLOW_CONNECTION_TIMEOUT);
				
			},
			
			viewCanBeReset : function()
			{
				if( (Date.now() - start)>=MAX_TIME_STUDENT_ON_SCREEN )
				{
					if(refresh_view)
					{
						return false;
					}else
					{
						refresh_view = true;
						return true;
					}
				}
			},
			
			resetParms :  function()
			{
				resetParms();
			}
			
		
		};

})();

    
	
	function setGraphicView(background_color, header_text, play_song=false, last_name="",first_name="", division="", halBoardDay="", exit_regime="", file_photo="", extension_photo="")
	{
		document.getElementById('display_exit_result').style.backgroundColor = background_color;
		document.getElementById('display_exit_result').innerHTML = header_text;
		jQuery("#display_exit_result").fitText(1.1, { minFontSize: '10px', maxFontSize: '35px' });

		document.getElementById('nom_eleve').innerHTML = last_name;
		document.getElementById('prenom_eleve').innerHTML = first_name;
		document.getElementById('classe_eleve').innerHTML = division;
		
		lunch_regime = "";
		if(halBoardDay==GlobalDefinition.TRUE_VALUE)
		{
			lunch_regime = "DP";
		}else if(halBoardDay==GlobalDefinition.FALSE_VALUE)
		{
			lunch_regime = "Ext";
		}else
		{
			lunch_regime = "";
		}
		document.getElementById('lunch_regime').innerHTML = lunch_regime;
		
		
		if(exit_regime==null)
		{
			exit_regime = "";
		}
		document.getElementById('exit_regime').innerHTML = exit_regime;
		
		if(file_photo!="")
		{
			document.getElementById('photo_eleve').src = "data:image/"+extension_photo+";base64," + file_photo;
		}else
		{
			document.getElementById('photo_eleve').src = GlobalDefinition.DEFAULT_PHOTO;
		}
		
		if(play_song==true)
		{	
			audio_player.play();
		}
	}
	
	function resetView()
	{
		setGraphicView("#ffffff", "");
		resetRfidInput();
	}
	
	function updateTime() {
      var res = "";
      var d, m, y, h, min;


      var today = new Date();

      d = checkTime(today.getDate());
      m = checkTime(today.getMonth() + 1);
      y = today.getFullYear();
      h = checkTime(today.getHours());
      min = checkTime(today.getMinutes());

      res = getTextDay(today.getDay()) + " " + d + "/" + m + "/" + y + "<br>" + h + ":" + min;

      document.getElementById('heure_display').innerHTML = res;
	  
	  if(RfidHandler.viewCanBeReset())
	  {
		 resetView();
	  }
	  
	  clock_timer = setTimeout(updateTime, REFRESH_TIME);
      
    }


    function checkTime(i) {
      if (i < 10) {
        i = "0" + i
      }; // add zero in front of numbers < 10
      return i;
    }

    function getTextDay(day_of_the_week) {
      var text_day;
      switch (day_of_the_week) {
        case 0:
          text_day = "Dimanche";
          break;
        case 1:
          text_day = "Lundi";
          break;
        case 2:
          text_day = "Mardi";
          break;
        case 3:
          text_day = "Mercredi";
          break;
        case 4:
          text_day = "Jeudi";
          break;
        case 5:
          text_day = "Vendredi";
          break;
        case 6:
          text_day = "Samedi";
          break;
        default:
          text_day = "";
          break;

      }

      return text_day;
    }
	
	function keyDownRfid(e) 
	{
		
	 /* using keydown and not keycode because we can get control combinaison with keydown 
	  * Since the RFID reader is detect as an HI (humain interface) like keyboard, when the reader send data to computer
      * all the char are send are considered as numerci key. So the corresponding keyCode will be between D0 and D9
      * But because of the HI reader we have to press cap key in order to get input char as numerci key
      * else all we get is non numeric key (For example in AZERTY keyboard wihtout numerci pad, the key for '0' is also the key for '&' 
      * when the cap key is not pressed, so when the keycode to '0' will be detected the result will be '&' and not '0')
      * 
      * So we need to get combinaison and not just the key pressed
     */

	  var keynum;
	  if(window.event) 
	  { // IE                    
           keynum = e.keyCode;
      } else if(e.which)
	  { // Netscape/Firefox/Opera                   
           keynum = e.which;
      }
	  
	  RfidNewInput(keynum);

	  e.preventDefault();//stop key down because few rfid tag can trigger event on the page
	  return false;
    }
	
	function resetRfidInput()
	{
		tab_rfid_char = [];
		element_display_id.innerHTML = tabCharToString(tab_rfid_char);
	}
	
	function tabCharToString(tab, length=tab.length)
	{
		var string ="";
		for(var i=0; i<length;i++)
		{
			string = string + tab_rfid_char[i]; 
		}
		
		return string;
	}
	
	function RfidNewInput(keynum) 
	{
	  if(keynum==13)//return or enter key (send by the quite few rfdi reader at the end of the string send)
	  {
	    resetRfidInput();
		return;
	  }
	  var character = String.fromCharCode(keynum);
	  
	  
	  if(!RfidHandler.InProcess())
	  {
		  tab_rfid_char.push(character);
		  element_display_id.innerHTML = tabCharToString(tab_rfid_char);
		  //rfid_input_value = rfid_input_value + character
		  if(tab_rfid_char.length == GlobalDefinition.LENGTH_RFID_ID)
		  {
			  RfidHandler.setRfid( tabCharToString(tab_rfid_char,GlobalDefinition.LENGTH_RFID_ID) );
			  resetRfidInput();
		  }
	  }else
	  {
		 resetRfidInput();
	  }
	  
	}
	
	function addEvent(obj, evt, fn) 
	{
		if (obj.addEventListener) {
			obj.addEventListener(evt, fn, false);
		}
		else if (obj.attachEvent) {
			obj.attachEvent("on" + evt, fn);
		}
	}

	function mouseOutHandler(e)
	{
		e = e ? e : window.event;
		var from = e.relatedTarget || e.toElement;
		if (!from || from.nodeName == "HTML") 
		{//mouse out of the windows
			element_focus_out.style.visibility = "visible";//show element
		}
	}
	
	function focusIn(e)
	{
		/*Because all RFID tag are send to the computer as keyboard input,
		we need to be sure that user set the focus on out window. Then, if the user leave the windows,
		he will need to click on the button to reactivate the process
		*/
		element_focus_out.style.visibility = "hidden";//hide element (the button)
		resetRfidInput();
	}
	
	function setChange(response)
		{
			
		  try 
		  {
			  var data = JSON.parse(response);
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_CHECK_SERVICE_STATE)
			  {
					if(data[GlobalDefinition.REQUEST_SERVICE_STATE]!= GlobalDefinition.ONLINE_SERVICE)
					{
						DoorScript.destroy();
						MainMenu.displayOfflineServicePage();
					}
			  }
			  
		  }catch(e){}
		}
		  
		  

    return {
        
		initialize : function ()
		{	
			var data = DataRequest.setCheckServiceRequest();
			DataRequest.sendPostRequest(data, setChange);
			
			rfid_input_value ="";
			tab_rfid_char=[];
			element_focus_out = document.getElementById("button_focus_out");
			element_display_id =  document.getElementById("span_student_id");
	
			updateTime();
			//if previous listener was added, them each input char will be registered multiple time
			document.addEventListener('keydown',keyDownRfid);
			document.addEventListener('mouseout', mouseOutHandler);
			
			element_focus_out.addEventListener("click", focusIn);
			
			document.getElementById("photo_eleve").setAttribute("src",GlobalDefinition.DEFAULT_PHOTO);
			
			
			audio_player = new Audio(GlobalDefinition.AJAX_AUDIO_FILE + GlobalDefinition.ERROR_SOUND)
			audio_player.preload;
			
			image_handler = new Image();//preload default image (because if the connection to the server is closed, we still can load the default image)
			image_handler.src = GlobalDefinition.DEFAULT_PHOTO;
		},
		destroy : function ()
		{
			if(clock_timer)
			{
				clearTimeout(clock_timer);
				clock_timer = 0;
			}
			document.removeEventListener('keydown',keyDownRfid);
			document.removeEventListener('mouseout',mouseOutHandler);
			
			RfidHandler.resetParms();
			
		},
    };

})();


	//run function onload
	DoorScript.initialize();
	//end run function onload
	function destroy()
	{
		DoorScript.destroy();
		delete this.DoorScript; 
	}	

    