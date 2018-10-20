
    var AuthorizedExit = (
      /* All the clicked element in order to allow student to exit the school are 
       * stored in an array "list_authorized_item" according to the following format
       * [row of the clicked cell, column of the clicked cell, date in, date out].
       * In order to protect that list from modification we define it as a local variable
       * only accecible from that function
       */

      function() 
	  { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
		const DATE_FORMAT = 'dd/mm/yyyy';
		const ALT_FIELD = '#datepicker';
		const CLOSE_TEXT = 'Fermer'
		const PREV_TEXT = 'Précédent';
		const NEXT_TEXT = 'Suivant';
		const CURRENT_TEXT = 'Aujourd\'hui';
		const MONTH_NAMES = ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'];
		const MONT_NAMES_SHORT = ['Janv.', 'Févr.', 'Mars', 'Avril', 'Mai', 'Juin', 'Juil.', 'Août', 'Sept.', 'Oct.', 'Nov.', 'Déc.'];
		const DAY_NAMES = ['Dimanche', 'Lundi', 'Mardi', 'Mercredi', 'Jeudi', 'Vendredi', 'Samedi'];
		const DAY_NAMES_SHORT = ['Dim.', 'Lun.', 'Mar.', 'Mer.', 'Jeu.', 'Ven.', 'Sam.'];
		const DAY_NAMES_MIN = ['D', 'L', 'M', 'M', 'J', 'V', 'S'];
		const WEEK_HEADER = 'Sem.';
		const ATTRIBUTE_NAME_DATE_IN = "date_in";
		const ATTRIBUTE_NAME_DATE_OUT = "date_out";
		
		var table_element;
		var timeSpan_text_format="";
		var date_text_format="";
		var timeSlot_separator='';

		const DELAT_TIME_MINUTES=30;//min
		const DELTAT_DISTANCE=7;//%
		const ACTIVE_CELLS_COLOR = "rgb(32, 197, 115)";
		const DEFAULT_CELL_COLOR = "rgb(255, 255, 255)";
		const READ_ONLY_CELL_COLOR ="rgb(81, 211, 170)";
		const START_TIME_ATTRIBUTE_NAME = "startTime";
		const END_TIME_ATTRIBUTE_NAME = "endTime";
		const DAY_OF_WEEK_ATTRIBUTE_NAME  = "dayOfWeek";
		const START_DATE_ATTRIBUTE_NAME ="startDate";
	    const END_DATE_ATTRIBUTE_NAME ="endDate";
		const META_TEXT_ATTRIBUTE_NAME ="metaText";
		const AUTHORIZATION_ID_ATTRIBUTE_NAME="authorizationId";
		const TAG_NAME_CELL="cell";
		const DEFAULT_AUTHORIZATION_ID=-1;
		
		var START_TIME_ON_TABLE=new TimeSpan(7,0);//07:00
		var table;
		var cellule_height;
		var cellule_width;
		var cellule_start_x;
		var cellule_start_y;
		var index_cell=0;
		var mouse_over_table=false;
		const class_name_main_font_theme = "main-font-theme";
		const class_name_main_cells = "main-cells-style";
		const class_name_active_cell = "active-cell-style";
		const class_name_split_cell = "split-cell-style";

		var list_school_days=[];
		var list_id_removed_authorization=[];
		var list_new_authorization=[];
		
		
		function setChange(response)
		{
			  try {
				  var data = JSON.parse(response);
				  
				  DataRequest.getSystemInstructions(data);//manage system instructions
				  
				  if(data[GlobalDefinition.REQUEST_OBJECT_TAG] == GlobalDefinition.REQUEST_GET_STUDY_DATA)
				  {
					  timeSpan_text_format =  data[GlobalDefinition.REQUEST_TIMESPAN_TEXT_FORMAT];
					  date_text_format = data[GlobalDefinition.REQUEST_DATE_TEXT_FORMAT];
					  timeSlot_separator = data[GlobalDefinition.REQUEST_TIMESPLOT_SEPARATOR];
					  
					  //create table
					  var list_study_hours = data[GlobalDefinition.REQUEST_STUDY_HOURS];//as to be ordered list
					  START_TIME_ON_TABLE = TimeSpan.TryParse(list_study_hours[0].split(timeSlot_separator)[0], timeSpan_text_format);
					  list_school_days = data[GlobalDefinition.REQUEST_SCHOOL_DAYS];
					  //create the table
					  createTable(list_school_days, list_study_hours);
					  
					  //add exsiting authorization on the table
					  var list_authorization = data[GlobalDefinition.REQUEST_AUTHORIZATIONS_LIST];
					  var list_read_only_obecjt=[];
					  for(var i=0; i<list_authorization.length;i++)
					  {
						  var TimeSlot = list_authorization[i];
						  
						  var start_text_date = TimeSlot[GlobalDefinition.REQUEST_START_DATE];
						  var end_text_date = TimeSlot[GlobalDefinition.REQUEST_END_DATE];
						  var day_of_week = list_school_days.lastIndexOf(TimeSlot[GlobalDefinition.REQUEST_DAY_OF_WEEK_TEXT]);
						  var start_time = TimeSpan.TryParse(TimeSlot[GlobalDefinition.REQUEST_START_TIME], timeSpan_text_format);
						  var end_time = TimeSpan.TryParse(TimeSlot[GlobalDefinition.REQUEST_END_TIME], timeSpan_text_format);
						  var authorization_id = TimeSlot[GlobalDefinition.REQUEST_AUTHORIZATION_ID];
						  var read_only = TimeSlot[GlobalDefinition.REQUEST_READ_ONLY] || false;
						  var meta_text = TimeSlot[GlobalDefinition.REQUEST_META_TEXT];
						  
						  if(read_only)
						  {
							  /* When a timeSlot obejct is set to readOnly then user cannot not make modifications on that object
							   * Then the user can not edit the whole time slot taken by the object. So user can not mode modification on that time slot
							   * The solution is to split the cell that represent the time slot : one part will allow modification and the other part
							   * is for the read only object
							   
							   time slot graphic epresentation
							   ---------------------------------------
							   | normal cell     | ready only cell	|
							   |				 |					|
							   |				 |					|
							   |				 |					|
							   ---------------------------------------
							   
							   * But all the cell on the table are drawn as individual object (and same thing for the timeSlot object in the list #list_authorization
							   * Thus, if we draw the read only timeSlot before any other object, that object will be drawn on the read only object (which will disapear)
							   * In other words, we have to draw all the cells that are not read only cell and then draw the read only cell onto the table
							  */
							  list_read_only_obecjt.push(TimeSlot);//use to know what object draw
						  }else
						  {
							 var cell_element = createTableCell(start_time, end_time , day_of_week, authorization_id, read_only, meta_text);//draw normal cellule
							 setActivateStateOnCell(cell_element,start_text_date,end_text_date);   
						  }
					  }
					  
					  //draw read only cellule
					  for(var i=0; i<list_read_only_obecjt.length;i++)
					  {
						  var TimeSlot = list_read_only_obecjt[i];
						  
						  var start_text_date = TimeSlot[GlobalDefinition.REQUEST_START_DATE];
						  var end_text_date = TimeSlot[GlobalDefinition.REQUEST_END_DATE];
						  var day_of_week = list_school_days.lastIndexOf(TimeSlot[GlobalDefinition.REQUEST_DAY_OF_WEEK_TEXT]);
						  var start_time = TimeSpan.TryParse(TimeSlot[GlobalDefinition.REQUEST_START_TIME], timeSpan_text_format);
						  var end_time = TimeSpan.TryParse(TimeSlot[GlobalDefinition.REQUEST_END_TIME], timeSpan_text_format);
						  var authorization_id = TimeSlot[GlobalDefinition.REQUEST_AUTHORIZATION_ID];
						  var read_only = TimeSlot[GlobalDefinition.REQUEST_READ_ONLY];
						  var meta_text = TimeSlot[GlobalDefinition.REQUEST_META_TEXT];
						  
						  var cell_element = createTableCell(start_time, end_time , day_of_week, authorization_id, read_only, meta_text);
						  setActivateStateOnCell(cell_element,start_text_date,end_text_date);
					  }
				  }
				  

				} catch (e) {}
			  
		}
		
		
		
		
		function initializeDatePickers() 
		{
		  /*
		   * Set date picker in french for the date in and date out
		   */
		   var data ={
			altField: ALT_FIELD,
			closeText: CLOSE_TEXT,
			prevText: PREV_TEXT,
			nextText: NEXT_TEXT,
			currentText: CURRENT_TEXT,
			monthNames: MONTH_NAMES,
			monthNamesShort: MONT_NAMES_SHORT,
			dayNames: DAY_NAMES,
			dayNamesShort: DAY_NAMES_SHORT,
			dayNamesMin: DAY_NAMES_MIN,
			weekHeader: WEEK_HEADER,
			dateFormat: DATE_FORMAT
		  };
		  
		   document.getElementById("datepicker_in").setAttribute("datepicker", data);
		   document.getElementById("datepicker_out").setAttribute("datepicker", data);
		}
		
		
		function verifyDates(text_date_in, text_date_out) 
		{
		  /*
		   * Check if the dates are correct
		   */
		  var date_in = DateTool.TryParse(text_date_in, date_text_format);
		  var date_out = DateTool.TryParse(text_date_out, date_text_format);

		  if (date_in == null || date_out == null) {
			alert('Le format des dates n\'est pas valide.\nVeuillez suivre le format suivant : "jj-mm-aaaa"');
		  } else if (date_in > date_out) {
			alert("Avec les dates spécifiées, la sortie débute après avoir pris fin");
		  } else {

			return true;
		  }

		  return false;
		}


		function appendCssClass(name, content)
		{
			var new_class = "."+name+"{"+content+"}";
			var main_style = document.getElementById("style_authorization_school_office");
			main_style.innerHTML = main_style.innerHTML + new_class;
		}
		
		function normalizeHours(txt_hours)
		{
			return txt_hours.replace(timeSlot_separator,"<br>");
		}
		
		function hasClass(elem, className) 
		{
			return (' ' + elem.className + ' ').indexOf(' ' + className+ ' ') > -1;
		}
		
		
		function createTable(list_columns, list_rows)
		{
			/* We don't use a standard HTML table because the cells of the table have to be adjustable
			   The purpose is to set each cells at the correct place according to their time value
			   (each cells represent a start and a end time)
			   
			   So we have to create a custom table (or a fake grid like HTML table) but with each cell as
			   individual element place on the grid
			*/
			
			/* #table_container
			   ------------------------------------------------------------------------------  <----- No y-overflow (#table_container)
			   | -------------------------------------------------------------------------- |
			   | |#header_table			       ------------------						  |	|
			   | |	   ------------------	   |----------------|	 ------------------	  |	|
			   | |	   |#header_element |	   ||	           ||	 |	              |	  |	|  		  <--- No y-overflow (#header_table)
			   | |	   ------------------	   |----------------|    ------------------   | |
			   | ------------------------------|----------------|--------------------------	|
			   | ------------------------------|----------------|--------------------------	|
			   | | #table					   |				|						  | |
			   | | ----------------------------|----------------|------------------------ | |
			   | | |#row_container			   |				|						| | |
			   | | | -----------------		   |				|						| |	|
			   | | |  | #row_element |		   |				|						| |	|
			   | | | -----------------		   |				|						| |	|
			   | | ----------------------------|----------------|------------------------ |	|
			   | | ----------------------------|----------------|------------------------ | |
			   | | |						   |				|						| | |  		  <--- y-overflow (#table)
			   | | | -----------------		   |				|						| |	|
			   | | |  |             |		   |				|						| |	|
			   | | | -----------------		   |				|						| |	|
			   | | ----------------------------|----------------|------------------------ |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |				|						  |	|
			   | |							   |#column_container|				          |	|
			   | |							   ------------------				          |	|
			   | --------------------------------------------------------------------------	|
			   ------------------------------------------------------------------------------

			   VIEW RESULT :
			   
			   
			   #table_container
               --------------------------------------------------------------------------------------
			   |                | list_columns[0]  | list_columns[1]  | list_columns[2]  | ...      |
			   |----------------------------------------------------------------------------------- |
			   |list_rows[0]    |                  |                  |                  |          |
			   |----------------------------------------------------------------------------------- |
			   |list_rows[1]    |                  |                  |                  |          |
			   |----------------------------------------------------------------------------------- |
			   |list_rows[2]    |                  |                  |                  |          |
			   |----------------------------------------------------------------------------------- |
			   |...             |                  |                  |                  |          |
			   --------------------------------------------------------------------------------------
			*/
		
			var table_container = document.getElementById("table_container");
			
			/*------------------------------------ create the global style and variable in that whole scope ------------------------------------*/
			var border_container_css = "border : 1px solid rgb(33, 92, 89);";

			//table header style 
			var header_height=10;//%
			var header_width= 100;//%
		    /* Since we work here with pourcentage the header element (#header_table) and the table element (#table) must have the same width in the container 
			   (#table_container). Or we must have do do some ratio to get the corect poucentage from one element to another.
			   Thus, to simplify the problem we decided to set all the element width inside the container (#table_container) to the same (100% of the container #table_container)
			*/
			//table header css class
			var class_content = "position:absolute;top:0%;left:0%;"+"width:"+header_width+"%;"+"height:"+header_height+"%;";
			appendCssClass("header_table",class_content);
			
			//Create the header element to apppend it as child of the container (#table_container)
			var header_table = document.createElement("DIV");
			header_table.classList.add("header_table");
			
			/*The header with is now fixed to 100% of the container (#table_container) but we can set each element inside the header (which representes the columns) to 
			  start at a certain point inside the header (#header_table)
			*/
			var start_header_element=10;//% start of the element inside the header (#header_table) == start of the columns
			var end_header_element=99;//% end of the element inside the header (#header_table) == end of the columns 
			//Set to 99% and not 100% because of some visual effects
			var header_element_height = 100;//% of the header (#header_table)
			var header_element_width = (end_header_element-start_header_element)/list_columns.length;
			//With the number of element we have to create we set the width of those elements to fit together inside the element (from teh start point to the end point)
			
			
			//css class for each element inside the header element (#header_table)
			var font_size ="calc(1.2vw + 1.2vh)";
			var class_content="position:absolute;text-align:center;background-color:transparent;height:"+header_element_height+"%;"+"width:"+header_element_width+"%;"+"top:0%;font-size:"+font_size+";";
			appendCssClass("main-header-element-style",class_content); 
			
			/*To fake a table with individual elements we have to draw lines
			  To simplified the problem we just create container at the same start (and end) position than the elements inside the header but create to be displayed 
			  in front of those element and to cover the whole container (#table_container). 
			  Then we just set the border on to fake the line (now "column container" refers to those elements)
			  (And we do the same for the rows)
			*/
			
			//css class of the column container
			var class_content = "pointer-events: none;position:absolute;background-color:transparent;"+border_container_css+"height:100%;"+"width:"+header_element_width+"%;"+"top:0%;";
			appendCssClass("main-column-container",class_content);
			
			
			//Set position of each elements in the header and of the columns container
			for(var i=0;i<list_columns.length;i++)
			{
				
				var left = (start_header_element+(i*header_element_width));//position of new element
				
				//------ column container
				
				//class of the new element (that contains the new position)
				var class_content = "left:"+left+"%;";
				if(i!=0)
				{
					class_content = class_content + "border-left:none;";
				}
				var name = "column-container-"+i;
				appendCssClass(name,class_content);
				//create the element
				var column_container = document.createElement("DIV");
				column_container.classList.add("main-column-container");
				column_container.classList.add(name);
				
				table_container.appendChild(column_container)//append element to the main container (#table_container)
				
				//------ element inside the header (#header_table)
				
				//class of the new element
				var class_content = "left:"+left+"%;";
				var name = "header-element-"+i;
				appendCssClass(name,class_content);
				
				//create header element
				var header_element = document.createElement("SPAN");
				header_element.classList.add(class_name_main_font_theme);
				header_element.classList.add("main-header-element-style");
				header_element.classList.add(name);
				header_element.innerHTML=list_columns[i];
				
				header_table.appendChild(header_element);//append the element inside the header (#header_table)
				
			}
			
			table_container.appendChild(header_table);//append the finished header (#header_table) to the container (#table_container)
			/*In fact we decided to use a separate container for the columns (#header_table) and the row (#table) to keep the columns name 
			  in front of the page even if the user scroll into the container (#table_container)
			*/


			/*------------------------------------ create the table element ------------------------------------*/
			
			//-------- create the element (#table) that will contains all the rows element and the rows container (to fake the lines of the table)
			
			//create the css class of the table element (#table)
			var class_content="position:absolute;background-color:transparent;overflow:hidden;"+"top:"+header_height+"%;"+"left:0%;"+"width:100%;"+"height:"+(100-header_height)+"%;";
			appendCssClass("table-style",class_content); 

			//create the table element (#table)
			table = document.createElement("DIV"); //table is a global variable inside the script
			table.setAttribute("id", "table");//set id for that element (used later)
			table.classList.add("table-style");
			
			table_container.appendChild(table);//append the table element (#table) to the container (#table_container)

			
			/*------------------------------------ create the body of the table element ------------------------------------*/
			
			/* As before we use a row element that represent the rname of the row and a row container that fake the lines of the table
			   Since all the row elements and the row conatiners will be in the same container (#table), which was not the case for the header elements (columns element)
			   and the column containers, we decided to create the row element inside the row container
			*/
			
			
			//------- global variable in that scop for the row elements and the row containers

			//-------- set variable to be used after (only to follow the code with simplified variable name)
			cellule_width = header_element_width;
			cellule_height = 10;
			//--------
			
			//--- row containers
			var start_row = 0;//% start of the row containers belong the y axis
			var row_width=end_header_element;//%
			/* The width of the table element (#table) is the same as the header element (#headerè_table) 
			   but all the row container must stop at the end of the last columns (in the header element #header_table) to correctly fake the lines of the table
			   So we have to set the row containers width as the end of the last columns in the header element (#header_table)
			   (only because the header element (#header_table) and the table element (#table) start at the same point inside the container (#table_container)
			*/
			var row_height=cellule_height;//% (can be what we want here)
			
			//--- row element
			var row_element_width=start_header_element;
			/*the start point of the header elements (column names) must be the width of the row element (row names) 
			 (#header_table)
			  ------------------------------------------
			  |           |(start of the header element) 
			  ------------------------------------------
			  (#table)
			  ------------------------------------------
			  ||row 1     |(end of the row element == start of the header element)
			  ||row 2     |
			 -------------------------------------------
		    */
			var row_element_height=100;//% of the row container
			
			//-------- set variable to be used after (only to follow the code with simplified variable name)
			cellule_start_x = row_element_width;
			cellule_start_y = start_row;
			//--------
			
			//css class of the row containers
			var class_content = "pointer-events: none;position:absolute;background-color:transparent;"+border_container_css+"left:0%;"+"height:"+cellule_height+"%;"+"width:"+row_width+"%;";
			appendCssClass("main-row-container",class_content);

			//css class of the row elements
			var font_size ="calc(0.5vw + 0.35vh)";
			var class_content="position:absolute;font-size:"+font_size+";"+"text-align:center;background-color:transparent;height:"+row_element_height+"%;"+"width:"+row_element_width+"%;"+"left=0%;";
			appendCssClass("main-row-element-style",class_content); 

			/*We also add cells elements to detect click on each cell of the table*/
			//css class of the cellule elements
			var class_content = "position:absolute;text-align:center;"+"background-color:"+DEFAULT_CELL_COLOR+";"+"height:"+cellule_height+"%;"+"width:"+cellule_width+"%;"+"font-size:"+font_size+";"+border_container_css;
			appendCssClass(class_name_main_cells,class_content);
			//css class of active cellule element
			var class_content = "background-color:"+ACTIVE_CELLS_COLOR+";";
			appendCssClass(class_name_active_cell,class_content);
			//create class for split cellule element
			var class_content = "width:"+ (1.0*cellule_width/2)+"%;";
			appendCssClass(class_name_split_cell,class_content);
			
			//--- create the body of the table element (#table)
			for(var i=0;i<list_rows.length;i++)
			{
				//--- row container
				
				/* The position of the row containers and the cells are defined by the time inside the row element */
				var split_text_hours = list_rows[i].split(timeSlot_separator);
				if(split_text_hours.length==2)
				{
					var start_time = TimeSpan.TryParse(split_text_hours[0], timeSpan_text_format);
					var end_time = TimeSpan.TryParse(split_text_hours[1], timeSpan_text_format);
					
					var [x_pos, y_pos, height] = getPositionCell(start_time, end_time , 0);
					
					//css class of the new element
					var class_content ="top:"+y_pos+"%;"+"height:"+height+"%";
					var name = "row-container-"+i;
					appendCssClass(name,class_content);	
					//create element
					var row_container = document.createElement("DIV");
					row_container.classList.add("main-row-container");
					row_container.classList.add(name);
					row_container.setAttribute("id",name);
					
					//--- row element
					var row_element = document.createElement("SPAN");
					row_element.classList.add(class_name_main_font_theme);
					row_element.classList.add("main-row-element-style");
					row_element.innerHTML= normalizeHours(list_rows[i]);
					
					row_container.appendChild(row_element);//append the row element to the row container

					table.appendChild(row_container);//append the row container to the table element (#table)
					
					//--- create the cells
					for(var j=0; j<list_columns.length;j++)
					{
						createTableCell(start_time, end_time , j);
						
					}
					
				}
						
				
			}
			
			/*Since we have to set the coordinates of each element of the table if the size of a container change all the elements will be offset
			  For example if the scrollbar appears in the table element (#table) all the cells will be offset of their original poisitons (which were correct)
			  After lot of try to get the size of the scrollbar to balance its presence or to put the scrollbar outsite the table element (#table)
			  we decided to remove the scrollbar from the view while allowing user to use the mouse wheel to navigate inside the table element (#table)
			*/

			table.addEventListener("mouseover", mouseOverTable);
			/*when the user will focus on the table element (#table) we will disable all the scrollbar inside the page 
			  because when an element does not have an visible scrollbar all the wheel movements are send the the first visible scrollbar 
              (which is definetly not the one inside the table element (#table)).
			  As result, we have to disable all the scrollbar in the page (temporeraly) to prevent any other element to get the wheel movements and mouve
			*/
			table.addEventListener("mouseout", mouseOutTable);
			//enable the scrollbar inside the page
			window.addEventListener("keydown",keyDownTable);
			addMouseWheelListner(table, MouseWheelHandler);
			//detect the wheel mouvements inside the table element (#table)
		}
		
	
		function getPositionCell(start_time, end_time, column_index)
		{
			/* Given the start of an slot time (#start_time) and the end of the slot time (end_time) and the column
			   return the (x,y) coordinated of the element on the table and the height of the element 
			   (width is fixed outise that scope)
			*/
			
			/*Since we make operations on the timeSpan the value will change (and in javascript pointer are pass as arguments and not a copy of the variable).
			  So we have to make a copy a the given timeSpan (a custum copy create inside the TimeSpan structure)
			*/
			var time1 = start_time.Copy();//deepcopy
			var time2 = end_time.Copy()

			var y_pos = (time1.toMinutes() - START_TIME_ON_TABLE.toMinutes())*DELTAT_DISTANCE/DELAT_TIME_MINUTES;
			var x_pos = cellule_start_x+(column_index*(cellule_width));
			var height = Math.abs((time2.Subtract(time1).toMinutes()))* DELTAT_DISTANCE / DELAT_TIME_MINUTES;
			
			index_cell = index_cell +1;
			
			return [x_pos, y_pos, height]
		}
		
		
		function createTableCell(start_time, end_time, column_index, authorization_id=DEFAULT_AUTHORIZATION_ID, read_only=false,meta_text="")
		{
			
			var [x_pos, y_pos, height] = getPositionCell(start_time, end_time , column_index);
			//css class of the cells
			var class_content ="left:"+x_pos+"%;"+"top:"+y_pos+"%;"+"height:"+height+"%;";
			if(read_only)//read only cellule
			{
				splitCellsOnTableUnderTimeSlot(start_time, end_time , column_index);
				/* First we get all the cellule on the table that are positionate under the current read only cell
				   Then we chaneg their style to be split in half 
				*/
				var cell_width = (1.0*cellule_width/2);
				class_content = class_content + "background-color:"+READ_ONLY_CELL_COLOR+";"+"left:"+ (x_pos +cell_width) +"%;"+"width:"+cell_width+"%;";
				//the read only cell is positionate on the second part of a normal cell (at the right of the first half of the split cellule)
			}

			//cells are create over the row containers so we have to set the x and y position
			var name = "cell-style-"+ index_cell;
			appendCssClass(name,class_content);
			
			//create cell
			var cell = document.createElement(TAG_NAME_CELL);
			cell.classList.add(class_name_main_font_theme);
			cell.classList.add(class_name_main_cells);
			cell.classList.add(name);
			
			cell.setAttribute(START_TIME_ATTRIBUTE_NAME, start_time.toText(timeSpan_text_format));
			cell.setAttribute(END_TIME_ATTRIBUTE_NAME, end_time.toText(timeSpan_text_format));
			cell.setAttribute(DAY_OF_WEEK_ATTRIBUTE_NAME, list_school_days[column_index]);
			cell.setAttribute(START_DATE_ATTRIBUTE_NAME, "");
			cell.setAttribute(END_DATE_ATTRIBUTE_NAME, "");
			cell.setAttribute(AUTHORIZATION_ID_ATTRIBUTE_NAME, authorization_id);
			cell.setAttribute(META_TEXT_ATTRIBUTE_NAME, meta_text);
			cell.setAttribute("title", start_time.toText(timeSpan_text_format) + " - " + end_time.toText(timeSpan_text_format));

			if(!read_only)//not read only cellule = can be edited
			{
				addClickAndDblClickOnElement(cell,displayMetaTextCellOnTable, changeActiviationStateCellOnTable);
			}else//cellule can not be edited
			{
				cell.addEventListener("click", displayMetaTextCellOnTable);
			}
			
			table.appendChild(cell);//append cells to the table element (#table)
			
			return cell;
		}
		
		function splitCellsOnTableUnderTimeSlot(start_time, end_time, column_index)
		{
			/*According to the given time slot (start_time and end_time) and the corresponding column on the table
			  the method find all the cell that are inside that timeslot and split them in half
			*/
		
			var integer_start_time = start_time.toInteger();
			var integer_end_time = end_time.toInteger();
			
			var child_list = table.childNodes;
			for (var i = 0; i < child_list.length; i++) 
			{
				var child = child_list[i];
				if(child.tagName.toUpperCase()==TAG_NAME_CELL.toUpperCase())
				{
					if(child.getAttribute(DAY_OF_WEEK_ATTRIBUTE_NAME) == list_school_days[column_index])//cell on the needed column
					{
						var time1 = TimeSpan.TryParse(child.getAttribute(START_TIME_ATTRIBUTE_NAME), timeSpan_text_format).toInteger();
						
						if(time1 >= integer_start_time && time1 < integer_end_time) // s_time < end_time and not s_time <= end_time
						{//cellule inside the time slot
							child.classList.add(class_name_split_cell);//split the cellule in half
						}
					}
				}
			}	
		}

			
		function addMouseWheelListner(elem, callBackMethod)
		{	
			if (elem.addEventListener)
			{
				// IE9, Chrome, Safari, Opera
				elem.addEventListener("mousewheel", callBackMethod, false);
				// Firefox
				elem.addEventListener("DOMMouseScroll", callBackMethod, false);
			}
			// IE 6/7/8
			else
			{
				elem.attachEvent(callBackMethod);
			}
			
		}
		
		function mouseOverTable()
		{
			/*when the user will focus on the table element (#table) we will disable all the scrollbar inside the page 
			  because when an element does not have an visible scrollbar all the wheel movements are send the the first visible scrollbar 
              (which is definetly not the one inside the table element (#table)).
			  As result, we have to disable all the scrollbar in the page (temporeraly) to prevent any other element to get the wheel movements and mouve
			*/
			var elem = this.parentElement;
			while(elem) 
			{
				elem.classList.add("stop-scrolling");
				elem = elem.parentElement;
			}
			
			mouse_over_table =true;
		}

		function mouseOutTable()
		{
			//enable the scrollbar inside the page
			elem = this.parentElement;
			while(elem) 
			{
				elem.classList.remove("stop-scrolling");
				elem = elem.parentElement;
			}
			
			mouse_over_table =false;
		}
		
		 
		function MouseWheelHandler(e)
		{
			// cross-browser wheel delta
			var e = window.event || e; // old IE support
			var delta = Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail)));

			scrollIntoTable(delta);
			
			return false;
		}
		
		
		function keyDownTable (e)
		{
			/*Scroll into the table element (#table) using the keyboard (up/down arrow)*/
			
			if(mouse_over_table)//user want to scroll inside the table element (#table)
			{
				e = e || window.event;

				var delta = 0;
				if (e.keyCode == '38') 
				{// up arrow
					delta = 1;
				}
				else if (e.keyCode == '40') 
				{// down arrow
					delta =-1;
				}
				
				scrollIntoTable(delta);
			}
		}
		
		
		function scrollIntoTable(delta)
		{
			var scrollTop = $(table).scrollTop();
			$(table).scrollTop(scrollTop-Math.round(delta * 20));
		}
		
		
		function addClickAndDblClickOnElement(elem, methodOnSingleClick, methodOnDoubleClick)
		{
			//code from https://stackoverflow.com/questions/6330431/jquery-bind-double-click-and-single-click-separately/7845282#7845282
			var DELAY = 200, clicks = 0, timer = null;
			
			elem.addEventListener("click", function(e)
				{
					clicks++;  //count clicks

					if(clicks === 1) {

						timer = setTimeout(function() {//perform single-click action 

							methodOnSingleClick(e);   
							clicks = 0;             //after action performed, reset counter

						}, DELAY);

					} else {//perform double-click action 

						clearTimeout(timer);    //prevent single-click action
						methodOnDoubleClick(e);
						clicks = 0;             //after action performed, reset counter
					}
				
				});
				
			elem.addEventListener("dblclick",function(e)
				{
					e.preventDefault();  //cancel system double-click event
				});
		}
		
		
		function displayMetaTextCellOnTable(e)
		{
			var elem = e.target;
			var authorization_id = elem.getAttribute(AUTHORIZATION_ID_ATTRIBUTE_NAME);
			var meta_text = elem.getAttribute(META_TEXT_ATTRIBUTE_NAME);
			
			if(hasClass(elem, class_name_active_cell))
			{
				if((authorization_id!=DEFAULT_AUTHORIZATION_ID))//already set authorization (cannot modify the meta text)
				{
					alert("Contenu de l'object :\n" + meta_text);
				}else//new authorization (can add meta text like the reason of the authorization)
				{
					var messageBoxResponse = prompt("Motif : ", "");
					if ( (messageBoxResponse != null) && (messageBoxResponse != "") ) 
					{
						elem.setAttribute(META_TEXT_ATTRIBUTE_NAME, messageBoxResponse);
					}
				}
			}
				
		}
		
		function changeActiviationStateCellOnTable(e)
		{
			var elem = e.target;
			
			if(hasClass(elem, class_name_active_cell))
			{
				var authorization_id = elem.getAttribute(AUTHORIZATION_ID_ATTRIBUTE_NAME);
				if(authorization_id!=DEFAULT_AUTHORIZATION_ID)
				{
					var current_authorization_id = parseInt(authorization_id) || -1;
					list_id_removed_authorization.push(current_authorization_id);//add id of the authorization object into a list
					elem.parentNode.removeChild(elem);
				}
				elem.classList.remove(class_name_active_cell);
				elem.setAttribute(START_DATE_ATTRIBUTE_NAME, "");
				elem.setAttribute(END_DATE_ATTRIBUTE_NAME, "");
				elem.innerHTML="";
			}else
			{
				var date_in  = DateTool.dateToText(document.getElementById('datepicker_in').valueAsDate,date_text_format);
				var date_out = DateTool.dateToText(document.getElementById('datepicker_out').valueAsDate,date_text_format);

				if (verifyDates(date_in, date_out)) //ckech if the dates are correct
				{
					setActivateStateOnCell(elem, date_in,date_out);
				}	  
				  
			}
		}
		
		
		function setActivateStateOnCell(elem, start_date, end_date)
		{
			elem.classList.add(class_name_active_cell);
			elem.setAttribute(START_DATE_ATTRIBUTE_NAME, start_date);
			elem.setAttribute(END_DATE_ATTRIBUTE_NAME, end_date);
			elem.innerHTML = "Du " + start_date + "<br>"+ "Au " + end_date;
		}
		
		
		
		
		function onClickSetActualDate () 
		{
		  /*When the user click on the corresponding button
		   * we automatically set the current date from the current day
		   * to the following saturday (of the current week)
		   */
		  var today = new Date(); //curent date

		  today.setDate(today.getDate() - today.getDay() );//date to sunday of the current week
		  document.getElementById('datepicker_in').valueAsDate = today;

		  today.setDate(today.getDate() + (7 - today.getDay())); //date to saturday of the current week
		  document.getElementById('datepicker_out').valueAsDate = today;
		}
		
		function getAllNewAuthorization()
		{
			list_new_authorization =[];//reset list
			
			var child_list = table.childNodes;
			for (var i = 0; i < child_list.length; i++) 
			{
				var child = child_list[i];
				if( (child.tagName.toUpperCase()==TAG_NAME_CELL.toUpperCase()) && (hasClass(child, class_name_active_cell)) )
				{
					var authorization_id = child.getAttribute(AUTHORIZATION_ID_ATTRIBUTE_NAME);
					if( (authorization_id!=null) && (authorization_id==DEFAULT_AUTHORIZATION_ID) )
					{
						var start_date = child.getAttribute(START_DATE_ATTRIBUTE_NAME);
						var end_date = child.getAttribute(END_DATE_ATTRIBUTE_NAME);
						var text_day_of_week = child.getAttribute(DAY_OF_WEEK_ATTRIBUTE_NAME);
						var start_time = child.getAttribute(START_TIME_ATTRIBUTE_NAME);
						var end_time = child.getAttribute(END_TIME_ATTRIBUTE_NAME);
						var read_only = false;
						var meta_text = child.getAttribute(META_TEXT_ATTRIBUTE_NAME);
						
						if( (start_date!="") && (end_date!="") && (text_day_of_week!="") && (start_time!="") && (end_time!="") )
						{
							var TimeSlot = new TextTimeSlot(start_date, end_date, text_day_of_week, start_time, end_time,read_only, meta_text);
							list_new_authorization.push(TimeSlot);
						}else
						{
							alert("ERREUR : les paramètres spécifiés ne sont pas valide");
						}
					}
				}
			}
		}


		function onSaveClick  () 
		{
		  getAllNewAuthorization();
		  var [type_of_table, index] = ToolBarSchoolOffice.getTypeAndIndex();
		  
		  var json_data_to_send = DataRequest.setAuthorizationForStudent(type_of_table,index,list_new_authorization, list_id_removed_authorization);
		  
		  AccountVerification.start(GlobalDefinition.REQUEST_SENSITIVE_DATA_AUTHORIZE_STUDENT,json_data_to_send);  
		  
		}
		
        return { //all those function are available outside the module
		
		initialize : function () 
		{
		  document.getElementById("save_button").addEventListener("click", onSaveClick);
		  document.getElementById("actual_date_button").addEventListener("click",onClickSetActualDate);
		  
		  onClickSetActualDate(); //set actual date on load
		  initializeDatePickers();
		  
		  var [type_of_table, index] = ToolBarSchoolOffice.getTypeAndIndex();
		  DataRequest.sendPostRequest(DataRequest.setStudyData(type_of_table,index), setChange);//get the study hour to put in the table
		},
		
		destroy : function ()
		{
			window.removeEventListener("keydown",keyDownTable);
		},
		
		getList : function()
		{
			getAllNewAuthorization();
			return list_new_authorization;
		},
		
		


        };
      })();


	  
	  AuthorizedExit.initialize();
	  
	  function subDestroy()
	  {
		  AuthorizedExit.destroy();
		  delete this.AuthorizedExit;
	  }

    


	

    



    




    