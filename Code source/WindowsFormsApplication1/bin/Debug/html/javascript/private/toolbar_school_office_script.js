
var ToolBarSchoolOffice =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  var list_item=[];
	  var list_content=[];
	  var list_script=[];
	  var student_division_table_id_list=[];
	  var student_division_list=[];
	  var Student;
	  var Division;
	  const index_list_content_to_first_load = 0;
	  var structure_loaded=false;
	  const text_header_for_student = "";
	  const text_header_for_division = "Classe : ";
	  var current_txt_info="";
	  var index_current_page=-1;
	  
	  
	  StudentData = function(student_database_index=-1, student_last_name="", student_first_name="", student_division="", student_photo=GlobalDefinition.NAME_DEFAULT_PHOTO) 
		{
			this.index = student_database_index;
			this.last_name = student_last_name;
			this.first_name = student_first_name;
			this.division = student_division;
			this.photo = student_photo;	
		};
		
	  DivisionData =  function (division_database_index=-1)
		{
			this.index = division_database_index;
			this.division=getClasseTagByIndex(division_database_index);
			this.photo = GlobalDefinition.NAME_DIVISION_PHOTO;
		};


		function getClasseTagByIndex (division_database_index)
		{
		    var index = student_division_table_id_list.lastIndexOf(division_database_index);
			if(index != -1)
			{
				return student_division_list[index];
			}else
			{
				return "";
			}
		}
				
	  function reset()
	  {
		   try 
          {
            subDestroy();
          } catch (e) {}
          /* function in each javascript that delete from memory all listner added and variable
             * If the file is loaded for the fisrt time there is no destroy function (that why there is try{}
          */
          document.getElementById("school_office_structure_content").innerHTML = GlobalDefinition.CONTENT_WAITING_PAGE;  
	  }
	  
	  
	  function loadContent(file_name_content, file_name_script)
	  {
		  reset();
		  $("#school_office_structure_content").load(GlobalDefinition.AJAX_HTML_CONTENT + file_name_content, function() {
		  DataRequest.loadScript(document.body, "sub_script", GlobalDefinition.AJAX_JAVASCRIPT_TAG +  file_name_script);});
	  }

	  function clickOnItem(el)
	  {//click on item of the toolbar
	  
		  if(el!=list_item[index_list_content_to_first_load])
		  {
			 if( (Student.index == -1) && (Division.index == -1) )
			  {
				  alert("Veuillez selectionner un élève ou une classe avant de poursuivre");
				  return;
			  } 
		  }else
		  {
			 resetSelection(); 
		  }
		  
		  updateStructureHeader();
			  
			  
		  for(i=0; i<list_item.length;i++)
		  {
			  if(list_item[i] != el)
			  {
				 list_item[i].style.backgroundColor="transparent"; 
			  }else
			  {
				 index_current_page = i;
				 list_item[i].style.backgroundColor=GlobalDefinition.BODY_COLOR;
				 loadContent(list_content[i], list_script[i]);
			  }			  
		  }
		   
	  }
	  
	  
	  function setChange(response)
	  {
		  
		  try 
		  {
			  var data = JSON.parse(response);
			  
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_GET_ALL_STUDENT_DIVISION)
			  {
				  student_division_list = data[GlobalDefinition.REQUEST_STUDENT_DIVISION_LIST];
				  student_division_table_id_list = data[GlobalDefinition.REQUEST_STUDENT_DIVISION_TABLE_ID_LIST];
			  }

			} catch (e) {}
		  
	  }
	  
	  
	 function onStuctureContentLoaded()
	 {
		document.getElementById("student_image").setAttribute("src", GlobalDefinition.DEFAULT_PHOTO);
		clickOnItem(list_item[index_list_content_to_first_load]);//load first page (statement only here because other content had to be load first)
	 }
	 
	 function updateStructureHeader()
	  {
		  var txt;
		  var txt_on_image;
		  var photo;
		  
		 if(Student.index!=-1)
		 {
			 txt = text_header_for_student + DataRequest.getFomatedStudentInfo(Student.last_name, Student.first_name, Student.division);
			 txt_on_image ="";
			 photo = Student.photo;
			 
		 }else if(Division.index!=-1)
		 {
			 txt = text_header_for_division + Division.division;
			 txt_on_image = Division.division;
			 photo = Division.photo;
		 }else
		 {
			 txt = "";
			 txt_on_image = "";
			 photo = GlobalDefinition.NAME_DEFAULT_PHOTO;
		 }
		 
		 current_txt_info = txt;
		 changeHeader(txt,photo);
		 document.getElementById("span_on_image").innerText = txt_on_image;//display text on the image container

	  }
	  
	  function changeHeader(txt, photo_name)
	  {
		  document.getElementById("student_info").innerText = txt;
		  document.getElementById("student_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + photo_name);
		  
	  }
	  
	  function resetSelection ()
	  {
		  Student= new StudentData();
		  Division= new DivisionData();
	  }
	  
	 
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			  
			  document.getElementById("search_student_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.SEARCH_STUDENT_IMAGE);
			  document.getElementById("stop_studen_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.STOP_STUDENT_IMAGE);
			  document.getElementById("authorize_student_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.AUTHORIZE_STUDENT_IMAGE);
			  document.getElementById("log_student_image").setAttribute("src", GlobalDefinition.AJAX_IMAGE + GlobalDefinition.LOG_STUDENT_IMAGE);
			  
			  //load the main structure into the div
			  $("#content_element").load(GlobalDefinition.AJAX_HTML_CONTENT + GlobalDefinition.FILE_NAME_STUDENT_SCHOOL_OFFICE_STRUCTURE_HTML_CONTENT ,
											function() {onStuctureContentLoaded();});
			  //end
			  
			  list_item.push(document.getElementById("search_student_image").parentElement);
			  list_item.push(document.getElementById("stop_studen_image").parentElement);
			  list_item.push(document.getElementById("authorize_student_image").parentElement);
			  list_item.push(document.getElementById("log_student_image").parentElement);
			  
			  list_content.push(GlobalDefinition.FILE_NAME_SEARCH_STUDENT_SCHOOL_OFFICE_HTML_CONTENT);
			  list_content.push(GlobalDefinition.FILE_NAME_STOP_STUDENT_SCHOOL_OFFICE_HTML_CONTENT);
			  list_content.push(GlobalDefinition.FILE_NAME_AUTHORIZE_STUDENT_SCHOOL_OFFICE_HTML_CONTENT);
			  list_content.push(GlobalDefinition.FILE_NAME_LOG_STUDENT_SCHOOL_OFFICE_HTML_CONTENT);
			  
			  list_script.push(GlobalDefinition.FILE_NAME_SEARCH_SCHOOL_OFFICE_SCRIPT);
			  list_script.push(GlobalDefinition.FILE_NAME_STOP_SCHOOL_OFFICE_SCRIPT);
			  list_script.push(GlobalDefinition.FILE_NAME_AUTHORIZE_SCHOOL_OFFICE_SCRIPT);
			  list_script.push(GlobalDefinition.FILE_NAME_LOG_SCHOOL_OFFICE_SCRIPT);
			  
			  
			  list_item[0].addEventListener("click", function() { clickOnItem(list_item[0]); });
		      list_item[1].addEventListener("click", function() { clickOnItem(list_item[1]); });
			  list_item[2].addEventListener("click", function() { clickOnItem(list_item[2]); });
			  list_item[3].addEventListener("click", function() { clickOnItem(list_item[3]); });
			  
			  DataRequest.sendPostRequest(DataRequest.setStudentDivisionList(), setChange);
			  
			  resetSelection ();
			  
		  },
		  
		  setStudent : function (student_database_index, student_last_name, student_first_name, student_division, student_photo) 
		  {
			  Division= new DivisionData();//reset division bacause user can only choose one division OR one student
			  Student= new StudentData(student_database_index, student_last_name, student_first_name, student_division, student_photo);
			  //set new student
			  updateStructureHeader();
		  },
		  
		  getStudent : function ()
		  {
			  return Student;
		  },
		  
		  setDivision : function (division_database_index) 
		  {
			  Student= new StudentData();//reset student
			  Division = new DivisionData(division_database_index);
			  //set new division
			  updateStructureHeader();
		  },
		  
		  getDivision : function ()
		  {
			  return Division;
		  },
		  
		  getAllStudentDivision : function ()
		  {
			  return [student_division_table_id_list, student_division_list];
		  },
  
		  destroy : function()
		  {
			  
		  },
		  
		  getClasseTagByIndex : function (division_database_index)
		  {
				return getClasseTagByIndex(division_database_index);
		  },
		  
		  getTypeAndIndex : function()
		  {
			  var type_of_table=-1;
			  var index = Student.index;
			  
			  if(index !=-1)
			  {
				  type_of_table = GlobalDefinition.TYPE_OF_STUDENT_TABLE;
			  }else
			  {
				  index = Division.index;
				  if(index!=-1)
				  {
					  type_of_table = GlobalDefinition.TYPE_OF_DIVISION_TABLE;
				  }
			  }
			  
			  return [type_of_table, index]
		  },
		  
		  getFormatedTextInfo : function()
		  {
			  return current_txt_info;
		  },
		  
		  reloadCurrentPage : function()
		  {
			clickOnItem(list_item[index_current_page]);		  
		  },
		  
		  
        };
      })();

	  
	  ToolBarSchoolOffice.initialize();
	  
	  function destroy()
	  {
		  ToolBarSchoolOffice.destroy();
		  delete this.ToolBarSchoolOffice;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    