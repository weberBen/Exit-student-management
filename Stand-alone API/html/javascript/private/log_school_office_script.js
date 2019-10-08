

var LogSchoolOffice =(

      function() { //all these functions that allow direct modification of the list are only availabe in the module "AuthorizedExit", not outside
	  
	  const TAG_ID ="Id";
	  const TAG_STATE = "State";
	  const TAG_EFFECTIVE = "Effective";
	  const TAG_SCOPE = "Scope";
	  const TAG_START_DATE = "Start_date";
	  const TAG_END_DATE = "End_date";
	  const TAG_START_TIME = "Start_time";
	  const TAG_END_TIME = "End_time";
	  const TAG_DAY_OF_WEEK = "Day_of_the_week";
	  const TAG_IN_CHARGE = "In_charge";
	  const TAG_REASONS = "Reason";
	  const TAG_RECORDS = "Records";
	  const TAG_RECORDS_MODIFICATION_DATE = "Modification_date";
	  const TAG_RECORDS_MODIFICATION = "Modification";
	  const TAG_RECORDS_IN_CHARGE = "In_charge";
	  const TAG_DIVISION = "Division";
	  const TAG_DATE ="Date";
	  const TAG_MODIFICATION_DATE ="Modification_date";
	  const TAG_VERIFICATION = "Verification";
	  
	  var active_state;
	  var inactive_state;
	  var student_scope;
	  var division_scope;
	  var current_date;
	  
	  var log_element;
	  var log_txt="";
	  
	  var built_tree;
	  
	  	var sort_by = function(field, reverse, primer)
		{
		   //https://stackoverflow.com/questions/979256/sorting-an-array-of-javascript-objects
		   var key = primer ? 
			   function(x) {return primer(x[field])} : 
			   function(x) {return x[field]};

		   reverse = !reverse ? 1 : -1;

		   return function (a, b) {
			   return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
			 } 
		}

	  function setChange(response)
	  {
		  try 
		  {
			  
			  var data = JSON.parse(response);
			  DataRequest.getSystemInstructions(data);//manage system instructions
			  
			  if(data[GlobalDefinition.REQUEST_OBJECT_TAG]==GlobalDefinition.REQUEST_GET_LOG)
			  {
				active_state = data[GlobalDefinition.REQUEST_ACTIVE_STATE];
				inactive_state = data[GlobalDefinition.REQUEST_INACTIVE_STATE];
				student_scope  = data[GlobalDefinition.REQUEST_STUDENT_SCOPE];
				division_scope = data[GlobalDefinition.REQUEST_DIVISION_SCOPE];
				current_date = data[GlobalDefinition.CURRENT_DATE];
				
				var json_tree = JSON.parse(data[GlobalDefinition.REQUEST_RESULT]);
				built_tree = createTree(json_tree["Authorization"], json_tree["Ban"], json_tree["Profil"], json_tree["Regular"]);
				
				createTreeView(built_tree, 0, document.getElementById("drop_down_tree"));
				initializeTreeView();
				
			  }
			  

			}catch(error){}
		  
	}
	
	function createNodeTree(name,class_list, next)
	{
		/* We can represent a tree like that :
			Section 1
				item 1
				item 2
				item 3
					sub item 1
			Section 2
				item 1
				
			One node of the tree is represented by a JSON object {Label : "", Class_list : [], Next : []}
				*Label is the name of the node (ex : "Section 1", "item 1", "sub item 2", etc)
				*Class_list is an array that save all the class we need to add to the element
				*Next is an array that save all the child node of that element
			If the node has no child, then Next is null
			
			For example :
			res[0] = {"Section 1", ["class_name_1"], 
						[ 
							{"item 1", [], null}, 
							{"item 2", [], null}, 
							{"item 3", [], 
								[
									{"sub item 1", [], null}
								]
							}
						]
					}
			res[1] = {"Section 2", [], 
						[ 
							{"item 1", [], null}
						]
					}
		*/
		
		return {Label : name , Class_list : class_list, Next : next};
	}

	
	function recordsToTree(list, index)
	{
		/*records[i] =
						{
							(int)Date,
							(string)Modification,
							(string)In_charge
                        }
		*/
		if(index>=list.length)
		{
			return [];
		}
		
		data = list[index];
		arg = [createNodeTree("Date : "+data[TAG_RECORDS_MODIFICATION_DATE],[],null)].concat([createNodeTree("Responsable : "+data[TAG_RECORDS_IN_CHARGE],[],null)]);

		return [createNodeTree(data[TAG_RECORDS_MODIFICATION], [], arg)].concat(recordsToTree(list, index+1));
	}
	
	function authorizationToTree(list, index)
	{
		/*Authorisation[i] = 
			{
                    (int)Date,
                    (string)Id,
                    (int)State,
                    (bool)Effective,
                    (int)Scope ,
					(string)Division,
                    (string)Start_date,
                    (string)End_date,
                    (string)Start_time,
                    (string)End_Time,
                    (string)Day_of_the_week,
                    (string)In_charge,
                    (string)Reason ,
                    (array)Records

            }
		*/
		if(index>=list.length)
		{
			return [];
		}
	
		var data = list[index];
		var name = data[TAG_START_DATE]+" / "+data[TAG_END_DATE] +" - "+list[index][TAG_DAY_OF_WEEK] + " de "+list[index][TAG_START_TIME]+" à "+list[index][TAG_END_TIME];
		var scope = "";
		var list_class = [];
		var list_next = [];
		
		list_next.push(createNodeTree("Identifiant : " + data[TAG_ID], [], null));
		
		if(data[TAG_STATE] == inactive_state)
		{
			list_next.push(createNodeTree("Etat : supprimé", [], null));
			list_class.push("inactive-state");
		}else if(data[TAG_EFFECTIVE]==true)
		{
			list_class.push("effective-state-authorization");
		}
		list_next.push(createNodeTree("Actif : " + data[TAG_EFFECTIVE], [], null));
		
		if(data[TAG_SCOPE]==student_scope)
		{
			scope = "élève";
		}else if(data[TAG_SCOPE]==division_scope)
		{
			scope = "classe "+data[TAG_DIVISION];
		}
		
		list_next.push(createNodeTree("Portée : " + scope, [], null));
		list_next.push(createNodeTree("Responsable : " + data[TAG_IN_CHARGE], [], null));
		list_next.push(createNodeTree("Motif : " + data[TAG_REASONS], [], null));
		var next = recordsToTree(data[TAG_RECORDS],0);
		next.sort(sort_by(TAG_DATE, true, parseInt));
		//sort the list following the member date
		list_next.push(createNodeTree("Historique des modifications", [], next));
		
		return [createNodeTree(name, list_class, list_next)].concat(authorizationToTree(list, index+1));
	}
	
	function banToTree(list, index)
	{
		/* Ban[i] =    
				{
                    (int)Date
                    (int)State,
					(bool)Effective,
                    (int)Scope,
					(string)Division,
                    (string)Start_date),
                    (string)End_date,
                    (string)Start_time,
                    (string)End_Tim,
                    (string)In_charge,
                    (string)Reason,
                }
		*/
		if(index>=list.length)
		{
			return [];
		}
	
		var data = list[index];
		var name = data[TAG_START_DATE]+"/"+data[TAG_END_DATE];
		var scope = "";
		var list_class = [];
		var list_next = [];
		
		if(data[TAG_STATE] == inactive_state)
		{
			list_next.push(createNodeTree("Etat : supprimé", [], null));
			list_class.push("inactive-state");
		}else if(data[TAG_EFFECTIVE]==true)
		{
			list_class.push("effective-state-ban");
		}
		list_next.push(createNodeTree("Actif : " + data[TAG_EFFECTIVE], [], null));
		
		if(data[TAG_SCOPE]==student_scope)
		{
			scope = "élève";
		}else if(data[TAG_SCOPE]==division_scope)
		{
			scope = "classe "+data[TAG_DIVISION];
		}
		
		
		list_next.push(createNodeTree("Portée : " + scope, [], null));
		list_next.push(createNodeTree("Responsable : " + data[TAG_IN_CHARGE], [], null));
		list_next.push(createNodeTree("Motif : " + data[TAG_REASONS], [], null));
		
		return [createNodeTree(name, list_class, list_next)].concat(banToTree(list, index+1));
	}
	
	
	function profilToTree(list, index)
	{
		/* Profil[i] =    
                {
                    (int)Date,
                    (string)Modification_date,
                    (string)In_charge,
                    (string)Reason,
                }
		*/
		if(index>=list.length)
		{
			return [];
		}
	
		var data = list[index];
		var name = data[TAG_REASONS];
		var list_class = [];
		var list_next = [];
		
		list_next.push(createNodeTree("Date : " + data[TAG_MODIFICATION_DATE], [], null));
		list_next.push(createNodeTree("Responsable : " + data[TAG_IN_CHARGE], [], null));
		
		return [createNodeTree(name, list_class, list_next)].concat(profilToTree(list, index+1));
	}
	
	
	function regularToTree(list, index)
	{
		/* Regular[i] =    
                {
					(int)Date,
					(string)Start_time ,
					(string)End_time ,
					(bool)Verification
					(string) Reason
				}
		*/
		if(index>=list.length)
		{
			return [];
		}
	
		var data = list[index];
		var name;
		var list_class = [];
		var list_next = [];
		
		name = data[TAG_START_TIME] +" - "+data[TAG_END_TIME];
		if(data[TAG_VERIFICATION] == true )
		{
			name = name + " (vérification requise)";
			list_class.push("effective-state-authorization-with-verification");
		}else
		{
			list_class.push("effective-state-authorization");
		}
		
		list_next.push(createNodeTree("Motif : " + data[TAG_REASONS], [], null));
		
		return [createNodeTree(name, list_class, list_next)].concat(regularToTree(list, index+1));
	}
	
	
	
	function createNodeTree(name,class_list, next)
	{
		/* We can represent a tree like that :
			Section 1
				item 1
				item 2
				item 3
					sub item 1
			Section 2
				item 1
				
			One node of the tree is represented by a JSON object {Label : "", Class_list : [], Next : []}
				*Label is the name of the node (ex : "Section 1", "item 1", "sub item 2", etc)
				*Class_list is an array that save all the class we need to add to the element
				*Next is an array that save all the child node of that element
			If the node has no child, then Next is null
			
			For example :
			res[0] = {"Section 1", ["class_name_1"], 
						[ 
							{"item 1", [], null}, 
							{"item 2", [], null}, 
							{"item 3", [], 
								[
									{"sub item 1", [], null}
								]
							}
						]
					}
			res[1] = {"Section 2", [], 
						[ 
							{"item 1", [], null}
						]
					}
		*/
		
		return {Label : name , Class_list : class_list, Next : next};
	}

	function createTree(authorization_list, ban_list, profil_list, regular_list)
	{
		/*sort all the list following their date member*/
		authorization_list.sort(sort_by(TAG_DATE, true, parseInt));
		ban_list.sort(sort_by(TAG_DATE, true, parseInt));
		profil_list.sort(sort_by(TAG_DATE, true, parseInt));
		regular_list.sort(sort_by(TAG_DATE, true, parseInt));
		
		/*create node from the list*/
		var tree1 = createNodeTree("Interdiction de sortie",[],banToTree(ban_list,0));
		var tree2 = createNodeTree("Autorisation de sortie",[],authorizationToTree(authorization_list,0));
		var tree3 = createNodeTree("Sortie journalière ("+current_date+")",[],regularToTree(regular_list,0));
		var tree4 = createNodeTree("Modification du profil",[],profilToTree(profil_list,0));
		
		return [tree1, tree2, tree3, tree4];//return the tree
	}
	
	function createSectionTreeView(label, list_class_name)
	{
		/*In a tree, a new section is represented by the following script :
			<li> <span class="caret"> Name_of_the_section</span>
				<ul class="nested">
					<- insert sub elements here
				</ul>
          </li>
		*/
		var section = document.createElement("LI");
		
		var elem = document.createElement("SPAN");
		elem.innerHTML = label;
		elem.classList.add("caret");
		addClassesToElement(elem, list_class_name);
		
		var sub_section = document.createElement("UL");
		sub_section.classList.add("nested");
		
		section.appendChild(elem);
		section.appendChild(sub_section);
		
		return section;
	}
	
	function createSubElementTreeView(elem_name, list_class_elem)
	{
		var elem = document.createElement("LI");
		elem.innerText = elem_name;
		addClassesToElement(elem, list_class_elem);
		
		return elem;
	}
	
	
	function createTreeView(tree, index,  previous_elem)
	{
		if(tree==null || index>=tree.length)
		{
			return ;
		}
		
		var data =  tree[index];
		var elem;
		if(data.Next == null || data.Next.length==0)
		{
			elem = createSubElementTreeView(data.Label, data.Class_list);
		}else
		{
			elem = createSectionTreeView(data.Label, data.Class_list);
		}
		previous_elem.appendChild(elem);
		
		
		createTreeView(data.Next, 0, elem.getElementsByTagName("UL")[0]);
		createTreeView(tree, index+1, previous_elem);
	}
	
	
	function initializeTreeView()
	{
		var toggler = document.getElementsByClassName("caret");
		var i;

		for (i = 0; i < toggler.length; i++) 
		{
		  toggler[i].addEventListener("click", function() {
			this.parentElement.querySelector(".nested").classList.toggle("active");
			this.classList.toggle("caret-down");
		  });
		}
	}
	
	function treeToText(tree, index)
	{
		if(tree==null || index>=tree.length)
		{
			return "";
		}
		
		var data =  tree[index];
		var txt;
		if(data.Next == null || data.Next.length==0)
		{
			txt = "\t" + data.Label +"\n";
		}else
		{
			txt = data.Label+"\n";
		}
		
		txt = txt + "\t" + treeToText(data.Next, 0);
		txt = txt + treeToText(tree, index+1);
		
		return txt;
	}
	
	
	function appendCssClass(name, content)
	{
		var new_class = "."+name+"{"+content+"}";
		var main_style = document.getElementById("style_authorization_school_office");
		main_style.innerHTML = main_style.innerHTML + new_class;
	}
		
		
     function downloadLogOnClick()
    {
	/*
		var filename = ("registre_" + ToolBarSchoolOffice.getFormatedTextInfo().replace(/\s/g, '_')).toUpperCase(); //replace ' ' by '_'
		log_txt = "GÉNÉRÉ LE : " + DataRequest.dateToString(new Date()) +"\n\n\n" + log_txt;
		DataRequest.downloadFileFromTxt(filename,log_txt);*/
    }
	
	function addClassesToElement(elem, list_class_name)
	{
		for(var i = 0; i<list_class_name.length;i++)
		{
			elem.classList.add(list_class_name[i]);
		}
	}
	
	  
        return { //all those function are available outside the module

		  initialize : function ()
		  {
			  var [type_of_table, index] = ToolBarSchoolOffice.getTypeAndIndex();
			  DataRequest.sendPostRequest(DataRequest.setLogRequest(type_of_table,index) , setChange);
			  
			  document.getElementById("download_log_button").addEventListener("click",downloadLogOnClick);

		  },
		  
		  destroy : function()
		  {
		  },
        };
      })();

	  
	  LogSchoolOffice.initialize();
	  
	  function subDestroy()
	  {
		  LogSchoolOffice.destroy();
		  delete this.LogSchoolOffice;
	  }
	  
	  
	  
	  
	  
	  
	  
	
    

    