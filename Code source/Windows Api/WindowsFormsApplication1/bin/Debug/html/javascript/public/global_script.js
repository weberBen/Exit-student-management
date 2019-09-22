class GlobalDefinition {
	
	
	constructor() 
	{
		this.length_rfid_id;
		this.length_short_secure_id;
		this.body_color;
		this.error_audio_file_name;
		this.school_name;
	}
	
    static get AJAX_JAVASCRIPT_TAG() { return "javascriptFile.ashx?javascriptFile="; }
    static get AJAX_IMAGE() { return "images.ashx?im="; }
	static get AJAX_HTML_CONTENT () {return "htmlContent.ashx?htmlContent=";}
	static get AJAX_DOWNLOAD_FILE () {return "downloadFile.ashx?downloadFile=";}
	static get AJAX_AUDIO_FILE () {return "play.aspx?s=";}
	
	static get NAME_DEFAULT_PHOTO () { return "default_student_photo.png";}
	static get NAME_DIVISION_PHOTO () { return "student_division_image.png";}
	static get DEFAULT_PHOTO() { return this.AJAX_IMAGE + this.NAME_DEFAULT_PHOTO; }
	static get SCHOOL_OFFICE_IMAGE() { return "school_office_icon.png"; }
	static get DOOR_IMAGE() { return "door_icon.png"; }
	static get SETTINGS_IMAGE() { return "settings_icon.png"; } 
	static get BANNED_IMAGE() { return "banned_icon.jpg"; }
	static get SEARCH_STUDENT_IMAGE() { return "search_student_icon.png"; }
	static get STOP_STUDENT_IMAGE() { return "stop_student_icon.png"; }
	static get AUTHORIZE_STUDENT_IMAGE() { return "authorize_student_icon.png"; }
	static get LOG_STUDENT_IMAGE() { return "log_student_icon.png"; }
	static get STOP_STUDENT_BUTTON_IMAGE() { return "stop_student_image.png"; }
	static get ERROR_IMAGE() { return "error_icon.png"; }
	static get NO_ERROR_IMAGE() { return "no_error_icon.png"; }
	static get ICON_IMAGE() { return "icon_png.png"; }
	static get UPLOAD_IMAGE() { return "upload_icon.png"; }
	static get ACCOUNT_IMAGE() { return "account_icon.png"; }
	static get DELETE_IMAGE() { return "delete_icon.png"; }
	static get CURRENT_DATE() { return "current_date"; }
	
	static get FILE_NAME_DOOR_SCRIPT() { return "door_script.js"; }
	static get FILE_NAME_AUTHORIZATION_SCRIPT() { return "authorization_script.js"; }
	static get FILE_NAME_CONNECTION_SCRIPT() { return "connection_script.js"; }
	static get FILE_NAME_WAITING_SCRIPT() { return "waiting_script.js"; }
	static get FILE_NAME_TOOLBAR_SCHOOL_OFFICE_SCRIPT() { return "toolbar_school_office_script.js"; }
	static get FILE_NAME_SEARCH_SCHOOL_OFFICE_SCRIPT() { return "search_school_office_script.js"; }
	static get FILE_NAME_STOP_SCHOOL_OFFICE_SCRIPT() { return "stop_school_office_script.js"; }
	static get FILE_NAME_AUTHORIZE_SCHOOL_OFFICE_SCRIPT() { return "authorize_school_office_script.js"; }
	static get FILE_NAME_LOG_SCHOOL_OFFICE_SCRIPT() { return "log_school_office_script.js"; }
	static get FILE_NAME_TOOLBAR_SETTINGS_SCRIPT() { return "toolbar_settings_script.js"; }
	static get FILE_NAME_UPLOAD_FILES_SCRIPT() { return "upload_files_script.js"; }
	static get FILE_NAME_USER_ACCOUNT_SCRIPT() { return "user_account_scritp.js"; }
	
	static get FILE_NAME_DOOR_HTML_CONTENT() { return "door_content.html"; }
	static get FILE_NAME_HEADER_DOOR_HTML_CONTENT() { return "header_door.html"; }
	static get FILE_NAME_AUTHORIZATION_HTML_CONTENT() { return "authorization_content.html"; }
	static get FILE_NAME_CONNECTION_HTML_CONTENT() { return "connection_content.html"; }
	static get FILE_NAME_WAITING_HTML_CONTENT() { return "waiting_content.html"; }
	static get FILE_NAME_HEARDER_TOOLBAR_SCHOOL_OFFICE_HTML_CONTENT() { return "toolbar_school_office_header.html"; }
	static get FILE_NAME_SEARCH_STUDENT_SCHOOL_OFFICE_HTML_CONTENT() { return "search_school_office_content.html"; }
	static get FILE_NAME_STOP_STUDENT_SCHOOL_OFFICE_HTML_CONTENT() { return "stop_school_office_content.html"; }
	static get FILE_NAME_AUTHORIZE_STUDENT_SCHOOL_OFFICE_HTML_CONTENT() { return "authorize_school_office_content.html"; }
	static get FILE_NAME_LOG_STUDENT_SCHOOL_OFFICE_HTML_CONTENT() { return "log_school_office_content.html"; }
	static get FILE_NAME_STUDENT_SCHOOL_OFFICE_STRUCTURE_HTML_CONTENT() { return "subcontent_school_office_structure_content.html"; }
	static get FILE_NAME_ACCOUNT_VERIFICATION_HTML_CONTENT() { return "account_verification_content.html"; }
	static get FILE_NAME_HEARDER_TOOLBAR_SETTINGS_HTML_CONTENT() { return "toolbar_settings_header.html"; }
	static get FILE_NAME_UPLOAD_FILES_HTML_CONTENT() { return "upload_files_content.html"; }
	static get FILE_NAME_USER_ACCOUNT_HTML_CONTENT() { return "user_account_content.html"; }
	
	static get FILE_NAME_LICENCE() { return "Informations_legales.zip"; }
	
	static get REQUEST_SYSTEM_INSTRUCTIONS() { return "system_instructions"; }
	static get REQUEST_RELOAD_CLIENT_PAGE() { return "reload_client_page"; }
	
	static get EXIT_UNAUTHORIZED_VALUE (){ return 0; }
	static get EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE (){ return 1; }
	static get EXIT_AUTHORIZED_UNDER_CONDITION_VALUE (){ return 2; }
	
	static get REQUEST_OBJECT_TAG() { return "request_object"; }
	static get REQUEST_MESSAGE_TO_DISPLAY () {return "message_to_display"; }
	static get REQUEST_TAG_RFID_DOOR() { return "rfid_from_door"; }
	static get REQUEST_TAG_RFID_VALUE() { return "rfid_value"; }
	static get REQUEST_TAG_ESTABLISH_CONNECTION() { return "establish_connection"; }
	static get REQUEST_REST_TIME_SINCE_BANNED () {return "rest_time_since_banned";}
	static get REQUEST_DISCONNECTION_FROM_SESSION () {return "session_disconnection";}
	static get REQUEST_RESULT_DISCONNECTION_FROM_SESSION () {return "result_disconnection_from_session";}
	static get REQUEST_CHECK_IF_SESSION_IS_OPENED () {return "is_session_opened";}
	static get REQUEST_RESULT_CHECK_IF_SESSION_IS_OPENED () {return "result_check_session_opened";}
	static get REQUEST_GET_SEARCH_RESULT () {return "get_search_result";}
	static get REQUEST_GET_ALL_STUDENT_DIVISION () {return "get_all_student_division";}
	static get REQUEST_GET_STUDENT_INFO () {return "get_current_student_info";}
	static get REQUEST_GET_STUDENT_INFO_FOR_SCHOOL_OFFICE () {return "get_student_for_school_office";}
	static get REQUEST_GET_LENGTH_RFID_ID () {return "get_length_rfid_id";}
	static get REQUEST_GET_LENGTH_SHORT_SECURE_ID () {return "get_length_short_secure_id";}
	
	static get REQUEST_LENGTH() {return "length";}
		
	static get REQUEST_SEARCH_WORD_STUDENT_LIST () {return "search_word_student";}
	static get REQUEST_STUDENT_ID_LIST () {return "student_id_list";}
	static get REQUEST_STUDENT_INFO_LIST () {return "student_info_list";}
	static get REQUEST_STUDENT_DIVISION_TABLE_ID_LIST () {return "student_division_table_id_list";}	
	static get REQUEST_STUDENT_DIVISION_LIST () {return "student_division_list";}	
	
	static get REQUEST_ACTIVE_STATE () {return "active_state";}
	static get REQUEST_INACTIVE_STATE () {return "inactive_state";}
	static get REQUEST_STUDENT_SCOPE () {return "student_scope";}
	static get REQUEST_DIVISION_SCOPE () {return "division_scope";}

	static get REQUEST_STUDENT_TABLE_ID () {return "student_table_id";}
	static get REQUEST_STUDENT_LAST_NAME () {return "student_last_name";}
	static get REQUEST_STUDENT_FISRT_NAME () {return "student_first_name";}
	static get REQUEST_STUDENT_DIVISION () {return "student_division";}
	static get REQUEST_STUDENT_PHOTO () {return "student_photo";}
	static get REQUEST_STUDENT_PHOTO_EXTENSION () {return "student_photo_extension";}
	static get REQUEST_STUDENT_EXIT_AUTHORIZATION () {return "student_exit_authorization";}
	static get REQUEST_STUDENT_AUTHORIZATION_LIST () {return "student_authorization_list";}
	static get REQUEST_STUDENT_LOG () {return "student_log";}

	static get REQUEST_AGENT_ID () {return "agent_id";}
	static get REQUEST_AGENT_PASSWORD () {return "agent_password";}
	static get REQUEST_AGENT_SHORT_SECURE_ID () {return "agent_short_secure_id";}
	
	static get REQUEST_RESULT_IDENTIFICATION () {return "result_identification_at_connection";}
	static get REQUEST_AGENT_LAST_NAME () {return "agent_last_name";}
	static get REQUEST_AGENT_FIRST_NAME () {return "agent_first_name";}
	static get REQUEST_AGENT_JOB () {return "agent_job";}

	static get CONTENT_WAITING_PAGE() 
	{ 
		return '<html><style type="text/css">.gwd-div-jqiw {text-align:center;font-size:250%;position: absolute;width: 40%;height: 10%;top: 45%;left: 30%;}</style><span class="main-font-theme gwd-div-jqiw" id="display_waiting_info_span">Veuillez patienter ...</span></html>';
	 
	}
	
	static get LENGTH_RFID_ID() {return this.length_rfid_id;}
	static set LENGTH_RFID_ID(value) {this.length_rfid_id = value;}
	
	static get LENGTH_SHORT_SECURE_ID() {return this.length_short_secure_id;}
	static set LENGTH_SHORT_SECURE_ID(value) {this.length_short_secure_id = value;}
	
	static get ERROR_SOUND() {return this.error_audio_file_name;}
	static set ERROR_SOUND(value) {this.error_audio_file_name = value;}
	
	static get SCHOOL_NAME() {return this.school_name;}
	static set SCHOOL_NAME(value) {this.school_name = value;}
	
	static get BODY_COLOR() {return this.body_color;}
	static set BODY_COLOR(value) {this.body_color = value;}

	static get REQUEST_SENSITIVE_DATA () {return "sensitive_data"; }
	static get REQUEST_TYPE_OF_SENSITIVE_DATA () {return "type_of_sensitive_data"; }
	static get REQUEST_SENSITIVE_DATA_STOP_STUDENT () {return "stop_student_as_sensitive_data"; }
	static get REQUEST_SENSITIVE_DATA_AUTHORIZE_STUDENT () {return "authorize_student_as_sensitive_data"; }
	static get REQUEST_SENSITIVE_DATA_VALUE () {return "value_sensitive_data"; }
	static get REQUEST_RESULT_AUTHENTIFICATION_FOR_SENSITIVE_DATA () {return "result_authentification_for_sensitive_data"; }
	static get REQUEST_TYPE_OF_TABLE () {return "type_of_table"; }
	static get REQUEST_INDEX_ROW () {return "row_index"; }
	static get REQUEST_TEXT_REASON () {return "text_reason"; }
	static get REQUEST_PROCESS_ERROR () {return "process_error"; }
	
	static get REQUEST_GET_LOG () {return "get_log_data"; }
	static get REQUEST_RESULT () {return "result"; }
	
	static get TYPE_OF_STUDENT_TABLE() { return 0; }
	static get TYPE_OF_AGENT_TABLE() { return 1; }
	static get TYPE_OF_DIVISION_TABLE() { return 2; }
	
	static get REQUEST_GET_STUDY_DATA() { return "study_data"; }
	static get REQUEST_STUDY_HOURS() { return "study_hours"; }
	static get REQUEST_SCHOOL_DAYS() { return "school_days"; }
	static get REQUEST_TIMESPAN_TEXT_FORMAT() { return "timeSpan_text_format"; }
	static get REQUEST_DATE_TEXT_FORMAT() { return "date_text_format"; }
	static get REQUEST_TIMESPLOT_SEPARATOR() { return "timeSlot_separator"; }
	static get REQUEST_START_DATE() { return "start_date";}
	static get REQUEST_END_DATE() { return "end_date";}
	static get REQUEST_DAY_OF_WEEK_TEXT() { return "day_of_week";}
	static get REQUEST_START_TIME() { return "start_time";}
	static get REQUEST_END_TIME() { return "end_time";}
	static get REQUEST_AUTHORIZATION_ID() {return "id";}
	static get REQUEST_READ_ONLY() {return "read_only";}
	static get REQUEST_META_TEXT() { return "meta_text";}
	static get REQUEST_AUTHORIZATIONS_LIST() {return "authorizations_list";}
	static get REQUEST_ID_REMOVED_AUTHORIZATION_LIST() {return "id_removed_authorization_list";}
	
	static get REQUEST_CHANGE_PASSWORD() {return "set_new_password";}
	static get REQUEST_NEW_PASSWORD () {return "new_password";}
	static get REQUEST_CHANGE_SHORT_ID() {return "set_new_short_id";}
	static get REQUEST_NEW_SHORT_ID () {return "new_short_id";}
	
	static get REQUEST_UPLOAD_FILE () {return "upload_file";}
	static get REQUEST_GET_VALID_EXTENSIONS_FILE () {return "get_extension_lists";}
	static get REQUEST_TYPE_OF_UPLOADED_FILE () {return "type_of_uploaded_file";}
	static get REQUEST_STUDENT_STATE_FILE () {return "STUDENT_STATE_FILE";}
	static get REQUEST_UNAUTHORIZED_EXIT_SOUND_FILE () {return "UNAUTHORIZED_EXIT_SOUND_FILE";}
	static get REQUEST_BINARY_STRING () {return "bytes_array";}
	static get REQUEST_FILE_EXTENSION () {return "extension";}
	static get REQUEST_FILE_NAME () {return "file_name";}
	static get REQUEST_GET_FILE_NAME_ERROR_SOUND () {return "file_name_error_sound";}
	
	static get REQUEST_GET_SCHOOL_NAME () {return "school_name";}
	
	static get REQUEST_GET_SAVED_REASONS_FOR_EXIT_BAN () {return "saved_reasons_ban";}
	
}

var DataRequest =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside

	    function ResponseFromServer (response)
		{
			try 
			{
				  var data = JSON.parse(response);
				  
				  switch(data[GlobalDefinition.REQUEST_OBJECT_TAG])
				  {
					  case GlobalDefinition.REQUEST_GET_LENGTH_RFID_ID:
						GlobalDefinition.LENGTH_RFID_ID =  data[GlobalDefinition.REQUEST_LENGTH];
						break;
					case GlobalDefinition.REQUEST_GET_LENGTH_SHORT_SECURE_ID:
						GlobalDefinition.LENGTH_SHORT_SECURE_ID =  data[GlobalDefinition.REQUEST_LENGTH];
						break;
					case GlobalDefinition.REQUEST_GET_FILE_NAME_ERROR_SOUND:
						GlobalDefinition.ERROR_SOUND = data[GlobalDefinition.REQUEST_FILE_NAME];
						break;
					case GlobalDefinition.REQUEST_GET_SCHOOL_NAME:
						GlobalDefinition.SCHOOL_NAME = data[GlobalDefinition.REQUEST_RESULT];
						break;
				  }
			} catch (e) {}
		}
		
		function sendPostRequestToServer(data, listenerCallBack) 
		{

		  var x = new XMLHttpRequest();
		  x.onreadystatechange = function() 
		  {

			if (x.readyState === 4) 
			{
			  listenerCallBack(x.responseText);
			}
		  };
		  x.open("POST", document.URL);
		  x.send(data);
		  
		  data="";//data can include sensitive informations like password
		  delete this.data;
		}
		
		function  getBodyBackgroundColor()
		{
			try
			{
				var e= document.body
				var cs = document.defaultView.getComputedStyle(e,null);
				
				GlobalDefinition.BODY_COLOR = cs.getPropertyValue('background-color');
			}catch(e)
			{
				setTimeout(function() { getBodyBackgroundColor();},100);
			}
		}
		
		
		function initializeParms()
		{
			var data = {};
			
			data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_LENGTH_RFID_ID;
			sendPostRequestToServer(JSON.stringify(data),ResponseFromServer);
			
			data = {}
			data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_LENGTH_SHORT_SECURE_ID;
			sendPostRequestToServer(JSON.stringify(data),ResponseFromServer);
			
			data = {}
			data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_FILE_NAME_ERROR_SOUND;
			sendPostRequestToServer(JSON.stringify(data),ResponseFromServer);
			
			data = {}
			data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_SCHOOL_NAME;
			sendPostRequestToServer(JSON.stringify(data),ResponseFromServer);
			
			getBodyBackgroundColor();
		}
		
		
		function concactObjectArrays(object_array_1, object_array_2)
		{
			var res=object_array_1;

			for(var key in object_array_2)
			{
				res[key]=object_array_2[key];
			}
			
			return res;
		}
		
		
		initializeParms();
	  
        return { //all those function are available outside the module

          setRfidRequestForDoor: function(rfid_value) //get the current list (like read only)
            {
              var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_TAG_RFID_DOOR;
			  data[GlobalDefinition.REQUEST_TAG_RFID_VALUE]=rfid_value;
			  return JSON.stringify(data);
            },
			
		  setCertifiedConnection : function(user_id, user_password)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_TAG_ESTABLISH_CONNECTION;
			  data[GlobalDefinition.REQUEST_AGENT_ID]=user_id;
			  data[GlobalDefinition.REQUEST_AGENT_PASSWORD]=user_password;
			  
			  user_id="";
			  user_password="";
			  
			  delete this.user_id;
			  delete this.user_password;
			  
			  return JSON.stringify(data);
			  
	      },
		  
		  setTimeBannedRequest : function ()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_REST_TIME_SINCE_BANNED;
			  return JSON.stringify(data);
		  },
		  
		  setDisconnectionFromSession : function ()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_DISCONNECTION_FROM_SESSION;
			  return JSON.stringify(data);
		  },
		  
		  setCheckIfSessionOpened : function ()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_CHECK_IF_SESSION_IS_OPENED;
			  return JSON.stringify(data);
		  },
		  
		  setSearchStudent : function(search_word)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_SEARCH_RESULT;
			  data[GlobalDefinition.REQUEST_SEARCH_WORD_STUDENT_LIST] = search_word;
			  return JSON.stringify(data);
  
		  },
		  
		  setStudentDivisionList : function ()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_ALL_STUDENT_DIVISION;
			  return JSON.stringify(data);
			  
		  },
		  
		  setStopForStudent : function (type_of_table, index_row, reason)
		  {
			  var data={}
			  data[GlobalDefinition.REQUEST_TYPE_OF_TABLE] = type_of_table;
			  data[GlobalDefinition.REQUEST_INDEX_ROW] = index_row;
			  data[GlobalDefinition.REQUEST_TEXT_REASON] = reason;
			  return data;
		  },
		  
		  setAuthorizationForStudent : function(type_of_table, index_row, authorization_list, authorization_removed_list)
		  {
			  var data={}
			  data[GlobalDefinition.REQUEST_TYPE_OF_TABLE] = type_of_table;
			  data[GlobalDefinition.REQUEST_INDEX_ROW] = index_row;
			  data[GlobalDefinition.REQUEST_AUTHORIZATIONS_LIST] = authorization_list;
			  data[GlobalDefinition.REQUEST_ID_REMOVED_AUTHORIZATION_LIST] = authorization_removed_list;
			  
			  return data;
		  },
		  
		  setSensitiveData : function(authentification, sensitive_data_type, json_data_to_send)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_SENSITIVE_DATA;
			  data[GlobalDefinition.REQUEST_AGENT_SHORT_SECURE_ID] = authentification;
			  data[GlobalDefinition.REQUEST_TYPE_OF_SENSITIVE_DATA] = sensitive_data_type;
			  
			  authentification = "";
			  delete this.authentification;
			  
			  return JSON.stringify(concactObjectArrays(data,json_data_to_send));
		  },
		  
		  setLogRequest : function(type_of_table, row_index)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_LOG;
			  data[GlobalDefinition.REQUEST_TYPE_OF_TABLE]=type_of_table;
			  data[GlobalDefinition.REQUEST_INDEX_ROW]=row_index;
			  return JSON.stringify(data);
		  },
		  
		  setStudentInfoByIndex : function(student_table_index)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_STUDENT_INFO;
			  data[GlobalDefinition.REQUEST_STUDENT_TABLE_ID] = student_table_index;
			  return JSON.stringify(data);
		  },
		  
		  setStudentForSchoolOfficeByIndex: function(student_table_index)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.GET_STUDENT_INFO_FOR_SCHOOL_OFFICE;
			  data[GlobalDefinition.REQUEST_STUDENT_TABLE_ID] = student_table_index;
			  return JSON.stringify(data);
			  
		  },
		  
		  setStudyData : function(type_of_table,row_index)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_STUDY_DATA;
			  data[GlobalDefinition.REQUEST_TYPE_OF_TABLE]=type_of_table;
			  data[GlobalDefinition.REQUEST_INDEX_ROW]=row_index;
			  return JSON.stringify(data);
		  },
		  
		  
		  setNewAgentPassword : function(user_id, user_password, new_password)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_CHANGE_PASSWORD;
			  data[GlobalDefinition.REQUEST_AGENT_ID]=user_id;
			  data[GlobalDefinition.REQUEST_AGENT_PASSWORD]=user_password;
			  data[GlobalDefinition.REQUEST_NEW_PASSWORD] = new_password;
			  
			  user_password="";
			  new_password="";
			  delete this.user_password;
			  delete this.new_password;
			  
			  return JSON.stringify(data);
		  },
		  
		  setNewAgentShortId : function(user_id, user_password)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_CHANGE_SHORT_ID;
			  data[GlobalDefinition.REQUEST_AGENT_ID]=user_id;
			  data[GlobalDefinition.REQUEST_AGENT_PASSWORD]=user_password;

			  user_password="";
			  delete this.user_password;
			  
			  return JSON.stringify(data);
		  },
		  
		  setExtensionListForUploadedFile : function()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_VALID_EXTENSIONS_FILE;
			  return JSON.stringify(data);
			  
		  },
		  
		  setUploadFile: function(type_of_file, binary_string, file_extension)
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_UPLOAD_FILE;
			  data[GlobalDefinition.REQUEST_TYPE_OF_UPLOADED_FILE] = type_of_file;
			  data[GlobalDefinition.REQUEST_BINARY_STRING] = binary_string;
			  data[GlobalDefinition.REQUEST_FILE_EXTENSION] = file_extension;
			  	
			  return JSON.stringify(data);	  
		  },
		  
		  setExitBanReasons: function()
		  {
			  var data = {};
			  data[GlobalDefinition.REQUEST_OBJECT_TAG] = GlobalDefinition.REQUEST_GET_SAVED_REASONS_FOR_EXIT_BAN;
			  return JSON.stringify(data);
		  },
		  
		  getSystemInstructions : function (data)
		  {
			  try
			  {
				  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_SYSTEM_INSTRUCTIONS)
				  {
					  if(data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY]!=null && data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY]!="")
					  {
						  var message = data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY] +"\nActualiser la page ?";
						  if (confirm(message)) //ask if user want to reload the page
						  {
								location.reload();
						  }
						
					  }else if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]!=null && data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
					  {
						  var message = "Une erreur est survenue" +"\nActualiser la page ?";
						  if (confirm(message)) //ask if user want to reload the page
						  {
								location.reload();
						  }
					  }

					  if(data[GlobalDefinition.REQUEST_RELOAD_CLIENT_PAGE] != null && data[GlobalDefinition.REQUEST_RELOAD_CLIENT_PAGE]==true)
					  {
						  location.reload();
					  }
				  }
				return true; 
			  }catch(e)
			  {
				  return false;
			  }
				  
		  },
		  
		  
		 loadScript : function (el, script_id, script_source) 
		 {
		  // Check for existing script element and delete it if it exists
		  //element=element that contains the script or element to append the script define by its id "script_id" and its source (src) "script_source"
			  var js = document.getElementById(script_id);
			  if (js !== null) 
			  {
				
				el.removeChild(js);
			  }

			  // Create new script element and load a script into it
			  js = document.createElement("script");
			  js.src = script_source;
			  js.id = script_id;
			  el.appendChild(js);
		 },
		 
		  downloadFileFromServer : function (source, filename) 
		{
		  var a = document.createElement("a");
		  document.body.appendChild(a);
		  a.style = "display: none";
		  a.href = source;
		  a.download = filename;
		  a.click();
		  document.body.removeChild(a);
		},
		
		downloadFileFromTxt : function(filename, txt) 
		{
			  txt = txt.replace(/\n/g,"\r\n");
			  txt = txt.replace(/\t/g,"\r\t");
			  
			  var element = document.createElement('a');
			  element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(txt));
			  element.setAttribute('download', filename);

			  element.style.display = 'none';
			  document.body.appendChild(element);

			  element.click();

			  document.body.removeChild(element);
		},
		
		
		getFile : function (source)
		{
			var resourceContent;
			$(function(){
					$.ajax({
						url: source,
						async: false,   // asynchronous request? (synchronous requests are discouraged...)
						cache: false,   // with this, you can force the browser to not make cache of the retrieved data
						dataType: "text",  // jQuery will infer this, but you can set explicitly
						success: function( data, textStatus, jqXHR ) {
							resourceContent = data; // can be a global variable too...
							// process the content...
						}
					});
				});
				
			return resourceContent;
		},

		getFormatedSessionInfo : function(agent_last_name, agent_first_name, agent_job)
		{
			return agent_last_name.toUpperCase() +" "+ agent_first_name.toLowerCase() +" ("+agent_job.toUpperCase()+")";
		},
		
		getFomatedStudentInfo : function (student_last_name, student_first_name, student_division)
		{
			return student_last_name.toUpperCase() +" "+ student_first_name.toLowerCase() +" ("+student_division.toUpperCase()+")";
		},
		
		stringVoidOrEmpty : function (text_value)
		 {
			 var res;
			 if( (text_value==null) || (text_value.replace(/\s/g, '').length==0))
			 {
				 res = true;
			 }else
			 {
				 res= false;
			 }
			 
			 text_value=="";
			 return res;
			 
		 },
		 
		 dateToString : function(date)
		 {
			 var dd = date.getDate();
			 var mm = date.getMonth() + 1; //January is 0!
			 var yyyy = date.getFullYear();
			 var hour = date.getHours();
			 var min = date.getMinutes();
			 var sec = date.getSeconds();

			 if (dd < 10) 
			 {
			  dd = '0' + dd;
			 }
			 if (mm < 10) 
			 {
			  mm = '0' + mm;
			 }
			 if (hour < 10) 
			 {
			  hour = '0' + hour;
			 }
			 if (min < 10) 
			 {
			  min = '0' + min;
			 }
			 if (sec < 10) 
			 {
			  sec = '0' + sec;
			 }
			 
			 

			return dd + '/' + mm + '/' + yyyy+ " "+hour+":"+min+":"+sec;
			 
		 },
		 
		 
		sendPostRequest : function(data, listenerCallBack) 
		{
		  sendPostRequestToServer(data, listenerCallBack);
		  
		  data="";//data can include sensitive informations like password
		  delete this.data;
        },
		
		
		listNullOrEmpty : function (list)
		{
			  if(list == null || list.length==0)
			  {
				  return true;
			  }else
			  {
				  return false;
			  }
		},
		
		
		charIn : function(char_to_find, ref)
		{
			var i=0;
			while( (i<ref.length) && (ref[i]!=char_to_find) )
			{
				i=i+1;
			}
			
			if(i!=ref.length)
			{
				return true;
			}else
			{
				return false;
			}
		},
		
		removeAt : function(value, index)
		{
			var res="";
			for(var i=0;i<value.length;i++)
			{
				if(i!=index)
				{
					res = res + value[i];
				}
			}
			
			return res;
			
		},
		
		replaceAt : function(value, index, new_char)
		{
			var res="";
			for(var i=0;i<value.length;i++)
			{
				if(i==index)
				{
					res = res + new_char;
				}else
				{
					res = res + value[i];
				}
			}
			
			return res;
			
		},
		
		reverseText : function (value)
	    {
			var res="";
			for(var i=value.length-1;i>=0;i--)
			{
				res = res + value[i];
			}
			
			return res;
			
		},
		
		hasVerticalScrollBar : function(elem)
		{
			
			return ($(elem)[0].scrollHeight > $(elem).innerHeight());
		},
		
		
		
		
		
		};
      })();

	  
//based on the code by GEOFF GRAHAM LAST UPDATED ON DECEMBER 14, 2017 (from CSS-TRICK) to animate a text


	TxtTypeAnnimation = function (element_to_use, text_to_rotate, period_rotation=2000)
	{
		
		this.textToRotate = text_to_rotate;
		this.el = element_to_use;
		this.numberLoop = 0;
		this.periodRotation = period_rotation;
		this.txt = '';
		this.isDeleting = false;
		this.timer =0;
		
		this.tick = function ()
		{
			var i = this.numberLoop % this.textToRotate.length;
			var fullTxt = this.textToRotate[i];

			if (this.isDeleting) 
			{
				this.txt = fullTxt.substring(0, this.txt.length - 1);
			} else 
			{
				this.txt = fullTxt.substring(0, this.txt.length + 1);
			}

			try
			{
				this.el.innerHTML = '<span class="wrap">'+this.txt+'</span>';
			}catch(e)
			{
				return;
			}

			var delta = 200 - Math.random() * 100;
			var that=this;

			if (this.isDeleting) { delta /= 2; }

			if (!this.isDeleting && this.txt === fullTxt) 
			{
				delta = this.periodRotation;
				this.isDeleting = true;
			} else if (this.isDeleting && this.txt === '') 
			{
				this.isDeleting = false;
				this.numberLoop++;
				delta = 500;
			}

			this.timer = setTimeout(function() { that.tick();}, delta);
			
		};
		
		this.stop = function ()
			{
				 if(this.timer)
				 {
					clearTimeout(this.timer);
					this.timer = 0;
				 }
				 
				 
				this.numberLoop=0;
				this.isDeleting = false;
				this.el.innerHTML="";
				 
			 };
		 
		 this.start = function()
			 {
				 this.stop();
				 this.tick();
			 };
		 
		 this.changeTextToRotate = function (new_text_to_rotate)
			 {
				this.numberLoop=0;
				this.isDeleting = false;
				
				this.textToRotate=new_text_to_rotate;
				this.tick();
			 };
			
	}
		
		
		
		
    class TimeSpan 
	{
		constructor(/*days=0, hours=0,minutes=0,secondes=0,milliseconde=0*/) 
		{
			var days=0;
			var hours=0;
			var minutes=0;
			var secondes=0;
			var millisecondes=0;
			
			switch(arguments.length)
			{
				case 2:
					hours = arguments[0];
					minutes = arguments[1];
					break;
				case 3:
					hours = arguments[0];
					minutes = arguments[1];
					secondes = arguments[2];
					break;
				case 4:
					hours = arguments[0];
					minutes = arguments[1];
					secondes = arguments[2];
					millisecondes = arguments[3];
					break;
				case 5:
					days = arguments[0];
					hours = arguments[1];
					minutes = arguments[2];
					secondes = arguments[3];
					millisecondes = arguments[4];
					break;
			}
			
			
			var SECONDE_TO_MILLISECONDE = 1000;
			var MINUTE_TO_SECONDE = 60;
			var HOUR_TO_MINUTE=60;
			var DAY_TO_HOUR=24;
			
			var to_multiply =[1 , SECONDE_TO_MILLISECONDE , MINUTE_TO_SECONDE , HOUR_TO_MINUTE , DAY_TO_HOUR];
			for(var i=1;i<to_multiply.length;i++)
			{
				to_multiply[i] = to_multiply[i]*to_multiply[i-1];
			}
			
			this._Factor = to_multiply;
			this._sign = 1;
			
			this._Days= days;
			this._Hours = hours;
			this._Minutes = minutes;
			this._Secondes = secondes;
			this._Millisecondes = millisecondes;
			
			this.FromIntegerToTime = function(value)
			{
				//set days
				var factor = this._Factor[this._Factor.length-1]
				var q = Math.floor(value/factor);
				var r = value%factor;
				
				this._Days = q;
				value = r;
				//set hours
				var factor = this._Factor[this._Factor.length-2]
				var q = Math.floor(value/factor);
				var r = value%factor;
				
				this._Hours = q;
				value = r;
				//set minutes
				var factor = this._Factor[this._Factor.length-3]
				var q = Math.floor(value/factor);
				var r = value%factor;
				
				this._Minutes = q;
				value = r;
				//set secondes
				var factor = this._Factor[this._Factor.length-4]
				var q = Math.floor(value/factor);
				var r = value%factor;
				
				this._Secondes = q;
				value = r;
				//set millisecondes
				var factor = this._Factor[this._Factor.length-5]
				var q = Math.floor(value/factor);
				
				this._Millisecondes = q;
			}
			this.normalizeTimeElement = function(value)
			{
				if(value<10)
				{
					return "0"+value;
				}else
				{
					return ""+value;
				}
			}
			
			this.FromIntegerToTime(this.toInteger());//set time to the correct format
		}
		
		get Days () {return this._Days;}
		get Hours () {return this._Hours;}
		get Minutes () {return this._Minutes;}
		get Secondes () {return this._Secondes;}
		get Millisecondes () {return this._Millisecondes;}
		static get _DefaultTextFormat () {return "d.HH:mm:ss:ffff";}
		static get _TextSeparator () { return ".:;, ";}
		
		
		Add (time)
		{
			this.FromIntegerToTime(this.toInteger() + time.toInteger());
			return this;
		}
		
		Subtract(time)
		{
			var res = this.toInteger() - time.toInteger();
			if(res>=0)
			{
				this._sign=1;
			}else
			{
				this._sign=-1;
			}
			this.FromIntegerToTime(Math.abs(res));
			return this;
		}
		
		Copy()
		{	
			var time = new TimeSpan();
			time._Days = this._Days;
			time._Hours = this._Hours;
			time._Minutes = this._Minutes;
			time._Millisecondes = this._Millisecondes;
			
			return time;
		}
		
		toText(format=null)
		{
			if(format==null)
			{
				if(this._Days!=0)
				{
					format=TimeSpan._DefaultTextFormat;
				}else
				{
					format=TimeSpan._DefaultTextFormat.replace("d.","");
				}
			}
			
			var res="";
			format = format.toLowerCase().replace(/\s/g, '');
			
			var days = "" + this._Days;
			var hours = this.normalizeTimeElement(this._Hours);
			var minutes = this.normalizeTimeElement(this._Minutes);
			var secondes = this.normalizeTimeElement(this._Secondes);
			var millisecondes= this.normalizeTimeElement(this._Millisecondes);
			
			var index_days=0;
			var index_hours=0;
			var index_minutes=0;
			var index_secondes=0;
			var index_millisecondes=0;
			
			for(var i=0;i<format.length;i++)
			{
				if(format[i]=='d')
				{
					if(index_days<days.length)
					{
						
						res = res + days;
						index_days = days.length;
					}
				}else if(format[i]=='h')
				{
					if(index_hours<hours.length)
					{
						res = res + hours;
						index_hours = hours.length;
					}
				}else if(format[i]=='m')
				{
					if(index_minutes<minutes.length)
					{
						res = res + minutes;
						index_minutes = minutes.length;
					}
				}else if(format[i]=='s')
				{
					if(index_secondes<secondes.length)
					{
						res = res + secondes;
						index_secondes = secondes.length;
					}
				}else if(format[i]=='f')
				{
					if(index_millisecondes<millisecondes.length)
					{
						res = res + millisecondes[index_millisecondes];
						index_millisecondes = index_millisecondes+1;
					}else
					{
						res = res +"0";
					}
				}else if(DataRequest.charIn(format[i],TimeSpan._TextSeparator))
				{
					res = res + format[i]; 
				}
				
			}
			
			return res;
		}
		
		toMinutes()
		{
			return this.toInteger()/this._Factor[2];
		}
		
		static FromMinutes(value)
		{
			var time = new TimeSpan();
			time.FromIntegerToTime(value*time._Factor[2]);
			
			return time;	
		}

		static TryParse (value, format=TimeSpan._DefaultTextFormat)
		{
			try
			{
				var sign =1;
				var days=0;
				var hours=0;
				var minutes=0;
				var secondes=0;
				var millisecondes=0;
				
				
				format = format.toLowerCase().replace(/\s/g, '');
				
				if(value.split('.').length==1)
				{
					format = format.replace("d.","");
				}

				if(value[0]=='-')
				{
					sign = -1;
					value = DataRequest.removeAt(value,0);	
				};
			
			
				var current_text_int="";
				var index=-1;
				
				for(var i=0; i<format.length;i++)
				{
					switch(format[i])
					{
						case 'd':
							index=0;
						break;
						case 'h':
							index=1;
						break;
						case 'm':
							index=2;
						break;
						case 's':
							index=3;
						break;
						case 'f':
							index=4;
						break;
					}

					
					if( (DataRequest.charIn(format[i],TimeSpan._TextSeparator)) || (i==format.length-1) )
					{
							while( (value[0]!=format[i]) )
							{
								if(value.length==0)
								{
									break;
								}
								
								current_text_int = current_text_int + value[0];
								value = DataRequest.removeAt(value,0);//equivalent of "count = count +1"
							}
							
							value = DataRequest.removeAt(value,0);//remove the separator
							
							switch(index)
							{
								case 0:
									days= parseInt(current_text_int);
								break;
								case 1:
									hours= parseInt(current_text_int);
								break;
								case 2:
									minutes= parseInt(current_text_int);
								break;
								case 3:
									secondes= parseInt(current_text_int);
								break;
								case 4:
									millisecondes= parseInt(current_text_int);
								break;
							}
							
						current_text_int="";
						index=-1;
					}
				}
			
			
			var time = new TimeSpan(days, hours, minutes, secondes, millisecondes)
			time._sign = sign;
			
			return time;
			
			}catch(e)
			{
				return new TimeSpan();
			}
		}

		
		toInteger()
		{

			return this._sign*(this._Millisecondes*this._Factor[0] + this._Secondes*this._Factor[1] + this._Minutes*this._Factor[2] + this._Hours*this._Factor[3] +this._Days*this._Factor[4]);
			
		}
	}
	
	
	
class DateTool 
{
	static get DATE_SEPARATORS () { return "/-,.:;*\\ ";}
	static get DEFAULT_DATE_FORMAT () {return "dd-MM-yyyy"};
	
	static normalizeTimeElement(value)
	{
		if(value<10)
		{
			return "0"+value;
		}else
		{
			return ""+value;
		}
	}
	
	static TryParse(value, format)
	{
			try
			{
				const DATE_CHARS ="dmy";
				
				var days =1;
				var months=1;
				var years=1;

				format = format.replace(/\s/g, '');
				
				var current_text_int="";
				var index=-1;
				
				for(var i=0; i<format.length;i++)
				{
					switch(format[i])
					{
						case 'd':
						case 'D'://case 'D' ||'d' :
							index=0;
						break;
						case 'M': //'m' si for minutes only
							index=1;
						break;
						case 'Y'://case 'Y' ||'y' 
						case 'y':
							index=2;
						break;
					}
					
					
					if( (DataRequest.charIn(format[i],DateTool.DATE_SEPARATORS)) || (i==format.length-1) )
					{
							while( (value[0]!=format[i]) )
							{
								if(value.length==0)
								{
									break;
								}

								current_text_int = current_text_int + value[0];
								value = DataRequest.removeAt(value,0);//equivalent of "count = count +1"
							}
							value = DataRequest.removeAt(value,0);//remove the separator
							
							switch(index)
							{
								case 0:
									days= parseInt(current_text_int);
								break;
								case 1:
									months= parseInt(current_text_int);
								break;
								case 2:
									years= parseInt(current_text_int);
								break;
							}
							
						current_text_int="";
						index=-1;
					}
				}
				
				months = months -1;
				return new Date(years, months, days, 0, 0, 0, 0);

			}catch(e)
			{
				return null;
			}
	}
	
	
	static dateToText(value, format=null)
	{
		try
		{
			if(format==null)
			{
				format=DateTool.DEFAULT_DATE_FORMAT;
			}
			
			var res="";
			format = format.replace(/\s/g, '');

			var days = DateTool.normalizeTimeElement(value.getDate());
			var months = DateTool.normalizeTimeElement(value.getMonth() + 1); //January is 0!
			var years = DateTool.normalizeTimeElement(value.getFullYear());
			
			var index_days=0;
			var index_months=0;
			var index_years=0;
			
			for(var i=0;i<format.length;i++)
			{
				if(format[i]=='d' || format[i]=='D')
				{
					if(index_days<days.length)
					{
						
						res = res + days;
						index_days = days.length;
					}
				}else if(format[i]=='M')//'m' is ony for minutes
				{
					if(index_months<months.length)
					{
						res = res + months;
						index_months = months.length;
					}
				}else if(format[i]=='y' || format[i]=='Y')
				{
					if(index_years<years.length)
					{
						res = res + years;
						index_years = years.length;
					}
				}else if(DataRequest.charIn(format[i],DateTool.DATE_SEPARATORS))
				{
					res = res + format[i]; 
				}
			}
			
			return res;
			
		}catch(e)
		{
			return "";
		}
	}
	
}


class TextTimeSlot
{
	constructor(start_date, end_date, text_day_of_week, start_time, end_time,read_only=false, meta_text="")
	{
		
		this[GlobalDefinition.REQUEST_START_DATE]=start_date;
		this[GlobalDefinition.REQUEST_END_DATE]=end_date;
		this[GlobalDefinition.REQUEST_DAY_OF_WEEK_TEXT]=text_day_of_week;
		this[GlobalDefinition.REQUEST_START_TIME]=start_time;
		this[GlobalDefinition.REQUEST_END_TIME]=end_time;
		this[GlobalDefinition.REQUEST_READ_ONLY]=read_only;
		this[GlobalDefinition.REQUEST_META_TEXT]=meta_text;
	}
	
	set StartDate(value) {this[GlobalDefinition.REQUEST_START_DATE] = value;}
	set EndDate(value) {this[GlobalDefinition.REQUEST_END_DATE] = value;}
	set DayOfWeek(value) {this[GlobalDefinition.REQUEST_DAY_OF_WEEK_TEXT] = value;}
	set StartTime(value) {this[GlobalDefinition.REQUEST_START_TIME] = value;}
	set EndTime(value) {this[GlobalDefinition.REQUEST_END_TIME] = value;}
	set ReadOnly(value) {this[GlobalDefinition.REQUEST_READ_ONLY] = value;}
}