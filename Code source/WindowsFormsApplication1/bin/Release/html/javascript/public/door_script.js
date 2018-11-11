
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
	
	var rfid_input_value; 
	var clock_timer;
	var audio_player;
	var image_handler;
	var old_rfid_id;
	var current_rfid_id;
	
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
				  setGraphicView(HtmlColorError,text_error); 
				  
			  }else if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_TAG_RFID_DOOR)
			  {
				  if(rfid_value != data[GlobalDefinition.REQUEST_TAG_RFID_VALUE])
				  {//if the network connection is slow, infomartion from a previous request can be send after we send a new request
					  resetView();
					  throw null;
				  }

				  stopTimer();//stop timer
				  
				  
				  
				  var last_name;
				  var first_name;
				  var division;
				  var photo_name;
				  var exit_authorization;

				  last_name = data[GlobalDefinition.REQUEST_STUDENT_LAST_NAME];
				  first_name = data[GlobalDefinition.REQUEST_STUDENT_FISRT_NAME];
				  division = data[GlobalDefinition.REQUEST_STUDENT_DIVISION];
				  photo_name = data[GlobalDefinition.REQUEST_STUDENT_PHOTO];
				  exit_authorization = data[GlobalDefinition.REQUEST_STUDENT_EXIT_AUTHORIZATION];

				  if (last_name == "" || first_name == "" || division == "") 
				  {	  
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

					  setGraphicView(background_color,text_to_display, play_song, last_name, first_name, division, GlobalDefinition.AJAX_IMAGE + photo_name);   
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
				
				var data =DataRequest.setRfidRequestForDoor(rfid_value);
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
			}
			
		
		};

})();

    
	
	function setGraphicView(background_color, header_text, play_song=false, last_name="",first_name="", division="", photo_name=GlobalDefinition.DEFAULT_PHOTO)
	{
		
		document.getElementById('display_exit_result').style.backgroundColor = background_color;
		document.getElementById('display_exit_result').innerHTML = header_text;


		document.getElementById('nom_eleve').innerHTML = last_name;
		document.getElementById('prenom_eleve').innerHTML = first_name;
		document.getElementById('classe_eleve').innerHTML = division;
		document.getElementById('photo_eleve').src = photo_name;
		
		if(play_song==true)
		{	
			audio_player.play();
		}
	}
	
	function resetView()
	{
		setGraphicView("#ffffff", "");
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
	  
	  RfidNewInput(String.fromCharCode(keynum));

	  e.preventDefault();//stop key down because few rfid tag can trigger event on the page
	  return false;
    }
	
	
	function RfidNewInput (character) 
	{
	  if(!RfidHandler.InProcess())
	  {
		  rfid_input_value = rfid_input_value + character
		  if(rfid_input_value.length == GlobalDefinition.LENGTH_RFID_ID)
		  {  
			  RfidHandler.setRfid(rfid_input_value);
			  rfid_input_value ="";
		  }
	  }else
	  {
		 rfid_input_value ="";//instruction important
	  }
	  
	}
	  
	
    return {
        
		initialize : function ()
		{
			rfid_input_value ="";
			
			updateTime();
			//if previous listener was added, them each input char will be registered multiple time
			document.addEventListener('keydown',keyDownRfid);
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
			
		},
    };

})();


	//run function onload
	DoorScript.initialize();
	//end run function onload
	function destroy()
	{
		RfidHandler.destroy();
		delete this.RfidHandler; 
		DoorScript.destroy();
		delete this.DoorScript; 
	}	

    