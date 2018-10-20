
var StopStudentSchoolOffice =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  var reason_textarea;
	  
	  function setChange(response)
	  {
		  try {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  console.log("data="+response);
			  switch(data[GlobalDefinition.REQUEST_OBJECT_TAG])
			  {
				  case GlobalDefinition.REQUEST_GET_ALL_STUDENT_DIVISION:
				  
					student_division_list = data[GlobalDefinition.REQUEST_STUDENT_DIVISION_LIST];
					break;
				  case GlobalDefinition.REQUEST_GET_SAVED_REASONS_FOR_EXIT_BAN:
				  
					showList(data[GlobalDefinition.REQUEST_RESULT]);
					break;
					
				
			  }

			} catch (e) {}
		  
	  }
	  
	  function clickOnListItem(el)
	  {
		  reason_textarea.value = el.innerText;
	  }
	  
	  function mouseOverListItem(el)
	  {
		  el.classList.add("li-active");
	  }
	  
	  function mouseOutListItem(el)
	  {
		  el.classList.remove("li-active");
	  }
	  
	  
	  function showList(list_items)
	  {
		 var list_element =  document.getElementById("list_view_reason");
		 var even=true;
		 for(var i=0; i<list_items.length;i++)
		 {
			var item_element = document.createElement("LI");
			item_element.classList.add("secondary-font-theme");
			item_element.classList.add("li-main");
			item_element.style="cursor:pointer";
			if(even)
			{
				item_element.classList.add("li-even");
				even=false;
			}else
			{
				item_element.classList.add("li-odd");
				even=true;
			}
			
			item_element.innerText = list_items[i];
			item_element.addEventListener("click",function(event){clickOnListItem(event.target);});
			item_element.addEventListener("mouseover",function(event){mouseOverListItem(event.target);});
			item_element.addEventListener("mouseout",function(event){mouseOutListItem(event.target);});
			
			list_element.appendChild(item_element);
		 }
		 
		 reason_textarea.value="";
	  }
	  
	  
	  function onClickStopButton ()
	  {
		  var stop_student = true;
		  var [type_of_table, index] = ToolBarSchoolOffice.getTypeAndIndex();
		  var reason_value = reason_textarea.value;
		  
		  var json_data_to_send = DataRequest.setStopForStudent(type_of_table,index,reason_value);
		  
		  AccountVerification.start(GlobalDefinition.REQUEST_SENSITIVE_DATA_STOP_STUDENT,json_data_to_send); 
		  
	  }
	  
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			reason_textarea = document.getElementById("reason_textarea");
			document.getElementById("stop_image").setAttribute("src",GlobalDefinition.AJAX_IMAGE + GlobalDefinition.STOP_STUDENT_BUTTON_IMAGE);
			document.getElementById("stop_image").parentElement.addEventListener("click", onClickStopButton);
			
			DataRequest.sendPostRequest(DataRequest.setExitBanReasons(), setChange);//get list of saved reasons for an exit ban

		  },
		  
		  destroy : function()
		  {
		  },
        };
      })();

	  
	  StopStudentSchoolOffice.initialize();
	  
	  function subDestroy()
	  {
		  StopStudentSchoolOffice.destroy();
		  delete this.SearchStudentSchoolOffice;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    