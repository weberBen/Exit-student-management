


var UploadFiles =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
    var input_files_object=[];
	var file_to_send=[];
	var error=false;
	var index_send_files=0;
	var submit_button;
	var file_uploaded=false;
	
	var register=null;

	
	function setChange(response)
	{
	  try {
		  var data = JSON.parse(response);
		  
		  DataRequest.getSystemInstructions(data);//manage system instructions
		  
		  if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_GET_VALID_EXTENSIONS_FILE)
		  {//set extension for files
			  for(var i=0; i<input_files_object.length;i++)
			  {
				  
				  var uploaded_object = input_files_object[i];
				  if(uploaded_object.TypeOfFile == GlobalDefinition.REQUEST_STUDENT_STATE_FILE)
				  {
					  uploaded_object.ValidExtensions=data[GlobalDefinition.REQUEST_STUDENT_STATE_FILE];
					  
				  }else if(uploaded_object.TypeOfFile == GlobalDefinition.REQUEST_UNAUTHORIZED_EXIT_SOUND_FILE)
				  {
					  uploaded_object.ValidExtensions=data[GlobalDefinition.REQUEST_UNAUTHORIZED_EXIT_SOUND_FILE];
				  }
				  
				  input_files_object[i] = uploaded_object//update list
				  
			  }
		  }
		  else if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_UPLOAD_FILE)
		  {
			  register.innerHTML = register.innerHTML + "<br/>- " + "Fichier "+(index_send_files+1)+"/"+file_to_send.length+ " recu par le serveur";
			  
			  if(data[GlobalDefinition.REQUEST_PROCESS_ERROR]==true)
			  {
				  register.innerHTML = register.innerHTML + "<br/>- " + "Fichier "+(index_send_files+1)+"/"+file_to_send.length+ " n'a pas peu être enregistré !";
				  register.innerHTML = register.innerHTML + "<br/>    MOTIF : " + data[GlobalDefinition.REQUEST_MESSAGE_TO_DISPLAY];
				  error=true;
			  }else
			  {
				register.innerHTML = register.innerHTML + "<br/>- " + "Fichier "+(index_send_files+1)+"/"+file_to_send.length+ " a été enregistré avec succès";  
			  }
			  
			  index_send_files = index_send_files+1;
			  if(index_send_files<file_to_send.length)
			  {
				  setTimeout(function() {sendUplodedFile(index_send_files);}, 100); //needed for the browser to repaint the page (and ths show the new text inside the register element
			  }else
			  {
				  if(!error)
				  {
					  register.innerHTML = register.innerHTML + "<br/><br/> " + "Tout les fichiers ont été enregistré avec succès !";
					  setTimeout(function() {alert("Tous les fichiers ont été enregistré");},100);
				  }else
				  {
					  register.innerHTML = register.innerHTML + "<br/><br/> " + "ERREUR :  certains fichier n'ont pas pu être sauvegardé";
					  setTimeout(function() {alert("ERREUR : certains fichier n'ont pas pu être enregistré");},100);
				  }
				  
				  resetAllUploadedFile();
				  file_uploaded=false;
			  }
			  
			  
			  register.scrollTo(0,register.scrollHeight);//scroll to the button of the text area
			  
		  }
		  

		} catch (e) {}
	  
	}
	
	function resetAllUploadedFile()
	{
		for(var i=0; i<input_files_object.length;i++)
		{
			var object = input_files_object[i];
			object.Reset();
			input_files_object[i] = object;
		}
	}
	
	function sendUplodedFile(index)
	{
		var object = file_to_send[index_send_files];
		var data = DataRequest.setUploadFile(object.TypeOfFile,object.BinaryArray, object.FileExtension); 
		DataRequest.sendPostRequest(data, setChange);//send file (as binary array)to the server
		
		var before_text ="";
		if(register.innerHTML!="")
		{
			before_text="<br/><br/>- ";
		}else
		{
			before_text="- ";
		}
		
		register.innerHTML = register.innerHTML + before_text + "Envoie en cours du fichier "+(index_send_files+1)+"/"+file_to_send.length;
		register.scrollTo(0,register.scrollHeight);//scroll to the button of the text area
	}
	
	
	class UploadedFileClass
	{
		//create object to deal with uploaded file informations
		constructor(elem=null, type_of_file="",valid_extension=[], binary_array=[], file_extension="",file_name="")
		{
			this._Element=elem;
			this._TypeOfFile = type_of_file;
			this._ValidExtensions = valid_extension;
			this._BinaryArray = binary_array;
			this._FileName=file_name;
			this._FileExtension=file_extension
			
		}
		
		get Element () {return this._Element;}
		set Element(value) 
		{
			var that = this;
			this._Element=value;
			this._Element.addEventListener("change",function(){uploadFile(that);});
		}
		
		get TypeOfFile () {return this._TypeOfFile;}
		set TypeOfFile(value) {this._TypeOfFile=value;}
		
		get ValidExtensions () {return this._ValidExtensions;}
		set ValidExtensions(value) {this._ValidExtensions=value;}
		
		get BinaryArray () {return this._BinaryArray;}
		set BinaryArray(value) {this._BinaryArray=value;}	
		
		get FileName () {return this._FileName;}
		set FileName(value) {this._FileName=value;}
		
		get FileExtension () {return this._FileExtension;}
		set FileExtension(value) {this._FileExtension=value;}
		
		Reset()
		{
			this._Element.value="";
			this._BinaryArray=[];
			this._FileName="";
			this._FileExtension="";
		}
	}
	
	
	function uploadFile(uploaded_object)
	{
		file_uploaded = true;
		
		var elem =  uploaded_object.Element;
		if(validateInputFileFormat(elem, uploaded_object.ValidExtensions))
		{
			var reader = new FileReader();
			  reader.addEventListener("loadend", 
				  function() 
				  {
					  var arrayBuffer = this.result;
					  var array = new Uint8Array(arrayBuffer);
					  var fileByteArray =[];
					   for (var i = 0; i < array.length; i++) 
					   {
						   fileByteArray.push(array[i]);
					   }
					   
					   uploaded_object.BinaryArray = fileByteArray;
					   
					   file_uploaded = false;
				  }); 
			reader.readAsArrayBuffer(elem.files[0]);
			
			//set parms
			var file_name = elem.files[0].name;
			uploaded_object.FileName = file_name;
			var split_file_name = file_name.split('.');
			uploaded_object.FileExtension = "." + split_file_name[split_file_name.length-1];
		}
		
	}
	  
	function validateInputFileFormat(input_elem, valid_extentions) 
	{
		if (input_elem.type == "file") 
		{
			var sFileName = input_elem.value;
			if (sFileName.length > 0) 
			{
				var blnValid = false;
				for (var j = 0; j < valid_extentions.length; j++) 
				{
					var sCurExtension = valid_extentions[j].toLowerCase();
					var extension = sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase();
					if (extension == sCurExtension) 
					{
						blnValid = true;
						break;
					}
				}
				 
				if (!blnValid) 
				{
					alert("Erreur : le fichier \"" + sFileName + "\" n'est pas valide seul les extensions suivantes sont autorisées " + valid_extentions.join(", "));
					input_elem.value = "";
					return false;
				}
			}
		}
		return true;
	}
	
	function removeUploadedFile(elem)
	{
		var i=0;
		do
		{
			var object = input_files_object[i];
			i=i+1;
		}while( (i<input_files_object.length) && (object.Element.parentElement != elem.parentElement) );
		i = i-1;//one loop of more than we need
		if(i!=input_files_object.length)
		{
			console.log("oki");
			object.Reset();
			input_files_object[i]=object;
		}
	}
	  

	
	function onSubmitButtonClick()
	{
		if(file_uploaded)
		{
			alert("Veuillez attendre la fin du chargement des fichiers avant de valider les modifications");
			return;
		}
		
		//reset parameters
		file_to_send = [];
		register.innerHTML = "";
		file_uploaded=true;
		error=false;
		
		for(var i=0; i<input_files_object.length;i++)
		{
			var object = input_files_object[i];
			 if( (object.BinaryArray!=null) && (object.BinaryArray.length!=0) )
			 {
				 file_to_send.push(object);
			 }
		}
		
		//send the first file (after the first file has been send, we will send the next, ect
		if(file_to_send.length!=0)
		{
		   index_send_files = 0;
		   sendUplodedFile(index_send_files);
		}else
		{
			alert("Aucun fichier à envoyer !");
		}
		
		
	}
	
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			 
			  var image_src = GlobalDefinition.AJAX_IMAGE + GlobalDefinition.DELETE_IMAGE;
			  
			  
			  //all input which deal with file uploading
			  var uploaded_object = new UploadedFileClass();
			  var container = document.getElementById("student_state_file");
			  uploaded_object.Element = container.getElementsByTagName("input")[0];
			  uploaded_object.TypeOfFile = GlobalDefinition.REQUEST_STUDENT_STATE_FILE;

			  input_files_object.push(uploaded_object);//add object into a list
			  
			  //set image into the container
			  var img = container.getElementsByTagName("img")[0];
			  img.setAttribute("src", image_src);
			  img.addEventListener("click", function(){removeUploadedFile(this);});
			  
			  
			  var uploaded_object = new UploadedFileClass();
			  var container = document.getElementById("unauthorized_exit_sound_file");
			  uploaded_object.Element = container.getElementsByTagName("input")[0];
			  uploaded_object.TypeOfFile = GlobalDefinition.REQUEST_UNAUTHORIZED_EXIT_SOUND_FILE;

			  input_files_object.push(uploaded_object);//add object into a list
			  
			  //set image into the container
			  var img = container.getElementsByTagName("img")[0];
			  img.setAttribute("src", image_src);
			  img.addEventListener("click", function(){removeUploadedFile(this);});
			  
			  submit_button = document.getElementById("submit_button");
			  submit_button.addEventListener("click", onSubmitButtonClick);
			  
			  register = document.getElementById("log_state_text_array");
			  
			  DataRequest.sendPostRequest(DataRequest.setExtensionListForUploadedFile(), setChange);//get list of all allowed format file

		  },
		  
		  destroy : function()
		  {
			  
		  },
		  
		  getList:function()
		  {
			  return input_files_object;
		  },
        };
      })();
	  
	  
	  UploadFiles.initialize();
	  
	  function subDestroy()
	  {
		  UploadFiles.destroy();
		  delete this.UploadFiles;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    