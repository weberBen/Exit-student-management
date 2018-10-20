
var SearchStudentSchoolOffice =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  var timer;
	  const timeout_launch_search=500;//ms
	  var txtType;
	  var default_data_text_to_rotate = ["..."];
	  var search_box_element;

	  
	  function setChange(response)
	  {
		  txtType.stop();
		  
		  try {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  switch(data[GlobalDefinition.REQUEST_OBJECT_TAG])
			  {
				  case GlobalDefinition.REQUEST_GET_ALL_STUDENT_DIVISION:
				  
					student_division_list = data[GlobalDefinition.REQUEST_STUDENT_DIVISION_LIST];
					break;
				case GlobalDefinition.REQUEST_GET_SEARCH_RESULT:
					setNewListToListWidget(document.getElementById("list_box"), "student_datbase_id",
										   data[GlobalDefinition.REQUEST_STUDENT_ID_LIST],data[GlobalDefinition.REQUEST_STUDENT_INFO_LIST],
										   clickOnStudentItem);
					break;
					
				case GlobalDefinition.REQUEST_GET_STUDENT_INFO:
				
					var student_id = data[GlobalDefinition.REQUEST_STUDENT_TABLE_ID];
					var last_name = data[GlobalDefinition.REQUEST_STUDENT_LAST_NAME];
					var first_name = data[GlobalDefinition.REQUEST_STUDENT_FISRT_NAME];
					var division = data[GlobalDefinition.REQUEST_STUDENT_DIVISION];
					var photo = data[GlobalDefinition.REQUEST_STUDENT_PHOTO];
					
					ToolBarSchoolOffice.setStudent(student_id, last_name,first_name,division,photo);
					
					break;
				
			  }
			  

			} catch (e) {}
		  
	  }
	  
	  function setNewListToListWidget(list_element,attribute_to_add, list_id,list_info,onClickMethod)
	  {
		  //remove the old item from the listBox
		  while (list_element.firstChild) 
		  {
			list_element.removeChild(list_element.firstChild);
		  }

		  //add new item
		  for(i=0; i<list_info.length;i++)
		  {
			  appenItemToListWidget(list_element, attribute_to_add, list_id[i], list_info[i],onClickMethod);
		  }
		  
		  if(list_id.length==1)//the search result contains only one result
		  {
			  clickOnStudentItem(list_id[0]);//simulate the click on that student
		  }else if(list_id.length==0)//no result
		  {
			  var entry = document.createElement('li');
			  entry.classList.add("main-font-theme");
			  entry.classList.add("ul-li");
			  entry.appendChild(document.createTextNode("Aucun résultat"));
			  list_element.appendChild(entry);
		  }
	  }
	  
	  
	  
	  function appenItemToListWidget(element_list_array, attribute_to_add, value_attribute, txt_to_add, onClickMethod)
	  {

		var entry = document.createElement('li');
		entry.classList.add("main-font-theme");
		entry.classList.add("ul-li");
		entry.addEventListener("mouseover", function () { entry.classList.add("ul-hover");});
		entry.addEventListener("mouseout", function () { entry.classList.remove("ul-hover");});
		entry.addEventListener("click", function () { onClickMethod(entry.getAttribute(attribute_to_add)); });
		entry.setAttribute(attribute_to_add, value_attribute);
		entry.appendChild(document.createTextNode(txt_to_add));
		element_list_array.appendChild(entry);
		  
	  }
	  
	  function clickOnStudentItem(student_table_id)
	  {
		  if(student_table_id!=null)
		  {
			DataRequest.sendPostRequest(DataRequest.setStudentInfoByIndex(student_table_id),setChange);
		  }
	  }
	  
	  function clickOnDivisionItem(text_division_table_id)
	  {
		  var temp_division_id = parseInt(text_division_table_id) || -1;
		  ToolBarSchoolOffice.setDivision(temp_division_id);
	  }
	  
	  function addListenerAllChange(el, listnerCallBack)
	  {
		  
		el.each(function() {
		var elem = $(this);

		// Save current value of element
		elem.data('oldVal', elem.val());

		// Look for changes in the value
		elem.bind("propertychange change click keyup input paste", function(event){
		  // If value has changed...
		  if (elem.data('oldVal') != elem.val()) {
		   // Updated stored value
		   elem.data('oldVal', elem.val());

		   // Value change
		   listnerCallBack();
		 }
		});
		});
	  }
	  
	  
	  function onChangeSearchBox()
	  {
		  //when user stop typing in the input after timeout ms we send the request to the server
		  if(timer)
		  {
			  clearTimeout(timer);
			  timer=0;
		  }
		  
		  timer = setTimeout(function() { timeOut();}, timeout_launch_search);
	  }
	  
	  function timeOut()
	  {
		  var search_word = document.getElementById("search_box").value;
		  if(search_word.replace(/\s/g, '').length!=0)
		  {
			  txtType.start();
			  DataRequest.sendPostRequest(DataRequest.setSearchStudent(search_word),setChange);
		  }
		  
	  }
	 
	  
	  function loadDivisionIntoListBox(list_division_table_id, list_division)
	  {
		  var [list_division_table_id, list_division] = ToolBarSchoolOffice.getAllStudentDivision();
		  
		  if( DataRequest.listNullOrEmpty(list_division_table_id) ||  DataRequest.listNullOrEmpty(list_division) )
		  {//request to the server is not complete yet
			  setTimeout(function() { loadDivisionIntoListBox(list_division_table_id, list_division);},100);
			  return;
		  }else
		  {//request complete
			setNewListToListWidget(document.getElementById("list_box_division"), "division_datbase_id",
										   list_division_table_id,list_division,
										   clickOnDivisionItem);
		  }
	  }
	  
	  function inputKeyDown(e) 
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
		  
		  if(keynum>=48 && keynum<=57)
		  {
			  /* since each char is just added to the existing text into the textbox
               * when user select text and next want to add digit char, then the seleted text will not be removed
               * So to avoid that issue we check if text is selected. If so we replace that selection by a blank
              */
				
			var selection = window.getSelection();
			if( (selection!=null) && (selection.length!=0))
			{
				search_box_element.value = search_box_element.value.replace(selection,"");//remove selected text
			}
			
			search_box_element.value = search_box_element.value + String.fromCharCode(keynum);//add the char to the existing text (at the cursor position)
			
			e.preventDefault();//remove the key press event
		  }
      }

	  
	  function onClickSearchBox()
	  {
		  search_box_element.select();//select all text
		  
	  }  
	  
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			var el = document.getElementById("waiting_for_search_result_array");
			var textToRotate = default_data_text_to_rotate;
			var periode = 2000;
			txtType = new TxtTypeAnnimation(el, textToRotate,periode );
			
			 addListenerAllChange($("#search_box"),onChangeSearchBox);
			 search_box_element = document.getElementById("search_box");
			 search_box_element.addEventListener("keydown", inputKeyDown);
			 search_box_element.addEventListener("click",onClickSearchBox);
			 search_box_element.addEventListener("blur",function(){ this.focus();});
			 //dispaly text when hover  
			 var message = "Veuillez entrer au choix le nom OU le prénom OU la classe OU l'identifiant RFID de l'élève recherché.\n"
						 + "Si besoin remplacer les caractères ne faisant pas parti de l'alphabet par un espace et les caractères accentués par ceux non accentués";
			 search_box_element.setAttribute("alt",message);
			 search_box_element.setAttribute("title",message);

			 loadDivisionIntoListBox();//load all the division into a listbox
			 
			 search_box_element.focus();
  
		  },
		  
		  destroy : function()
		  {
			  txtType.stop();
			  delete this.txtType;
		  },
        };
      })();

	  
	  SearchStudentSchoolOffice.initialize();
	  
	  function subDestroy()
	  {
		  SearchStudentSchoolOffice.destroy();
		  delete this.SearchStudentSchoolOffice;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    