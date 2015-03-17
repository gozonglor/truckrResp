function compileRelationshipFilter(){ //builds the list of relationship (donor), looks at the relationship_filter_form instead
	var checkboxes = document.forms['relationship_filter_form'].elements;
	allRelationships = new Array();
	allRelationships[0] = checkboxes[0].name;
	return allRelationships;
}
function validateFormFilter() {
 	var requiredFields = document.getElementById("filtering_form").elements;  
	var check = false;
	for (var i=0; i < requiredFields.length; i++){ 
		var val = requiredFields[i].value;
		if (val !== "") { 
			check = true;
			break;
		}
	}
	return check;
}
	
$(function () { 	
	 $("#filter_button").click(function (e) {
	 
	 	$("#client_table_module").hide();
		$("#client_filter_table").html("");
		document.getElementById("error_message_filter").innerHTML = "";
		document.getElementById("error_message_select").innerHTML = "";


        var NewPerson = {};
		NewPerson.contact_relationship = compileRelationshipFilter();
		NewPerson.contact_status = $("#ddlContactStatus_f").val();
        NewPerson.id = $("#id_f").val();
        NewPerson.firstName = $("#firstName_f").val();
        NewPerson.lastName = $("#lastName_f").val();
        NewPerson.phone = $("#phone_f").val();
        NewPerson.org = $("#org_f").val();
        NewPerson.email = $("#email_f").val();
        NewPerson.address = $("#address_f").val();
        NewPerson.city = $("#city_f").val();
        NewPerson.zip = $("#zip_f").val();
        NewPerson.state = $("#state_f").val();
        NewPerson.notes = $("#notes_f").val();
		
		var formFill = validateFormFilter();
		var success=false;
		
		if (formFill == true){
			document.getElementById("error_message_filter").innerHTML = "";
			$("#filter_module").hide();
			$("#client_table_module").show();
			
			var response =$.ajax({
				type: "POST",
				url: "http://localhost:49234/api/filter",
				contentType: "application/json; charset=utf-8",
				Type: "json",
				async: false,
				data: JSON.stringify(NewPerson),
				
				success: function (data) {
				success =true;
					handleData(data);
				},
            
				error: function(){
				sucess=false;
				document.getElementById("error_message_filter").innerHTML = "No API connected.";

				}
			}).responseText;
			e.preventDefault();
		}
		else{
			$("#client_table_module").hide();
			document.getElementById("error_message_filter").innerHTML = "Please specify a field to filter by.";
		}
    });
});

function handleData(data) { //A smaller function to consolidate the longer ones of building the html table
    buildHtmlTable(data);
}


$(function () { 	
	$('#selected_client_button').click(function(){ 
	   var table = document.getElementById('client_filter_table'); //grab the table of filtered clients 
	   for (var i = 1; i < table.rows.length; i++){ //loop through them and grab their information
			var entry = table.rows[i];
			var firstCol = entry.childNodes[0];
			var radioButton = firstCol.firstChild;
			var clientID = entry.childNodes[1];
			var clientFName = entry.childNodes[2]; 
			var clientLName = entry.childNodes[3];
			clientFName = clientFName.innerText;
			clientLName = clientLName.innerText;
			if (radioButton.checked){ //checks if the 'i' particular radio button is checked --i.e. the 3rd if you are on the fourth iteration of the loop
				document.getElementById("error_message_select").innerHTML = "You have selected "+clientFName+" "+clientLName;
				
				//GLOBALIZING
				chosenClientID = clientID.firstChild.data; //set the global variable chosenClientID to be the id of the selected child
				chosenClientFName = clientFName; //set global variable of first name
				chosenClientLName = clientLName; //set global variable of last name
				donorEmail = entry.childNodes[6];
				
				break; //break out of the for loop, since we have a selected donor already
			}
		}
		
		//if we have looped through the entire table of filtered clients and none of the radio buttons were checked
		//the chosenClientID must be null (has not been set to anything), and if it is null...
		if (chosenClientID == null){
				document.getElementById("error_message_select").innerHTML = "Please select a donor.";
		}
		
		else{ //otherwise... if we have selected a client
			var hiddenDonor = document.getElementById('hidden_client_id'); //grab the hidden donor field, which is a div
			// var hiddenDonorName = document.getElementById('hidden_donor_name'); //grab the hidden donor field, which is a div
			// hiddenDonorName.innerHTML = ""+chosenClientFName+" "+chosenClientLName+"."; //set the innerHTML to be the id of the donor

			hiddenDonor.innerHTML = getDonor();
			
			var storeClient =$.ajax({ //Makes the post call. 
				type: "POST",
				url: "http://localhost:49234/api/values",
				contentType: "application/json; charset=utf-8",
				Type: "json",
				async: false,
				data: JSON.stringify(chosenClientID),
					
				success: function () {
					document.getElementById("message_select").innerHTML = "Grabbing from the database...Donors are successfully filtered.";
					generateLotNum1();
					$("#client_table_module").hide(); //Hide current module
					$("#filter_module").hide(); //Hide current module
					$("#generate_module").show(); //Show the wanted module

				},
					 
				error: function(){
					document.getElementById("error_message_select").innerHTML = "No connection to API."; 
				}
			}).responseText;
		}
	});
});


function buildHtmlTable(myList) {
    var columns = addAllColumnHeaders(myList);//call function taking in list of clients, 
    for (var i = 0 ; i <= myList.length+1 ; i++) {//for each client
        var row$ = $('<tr/>');//create a row 
        for (var colIndex = 0 ; colIndex < columns.length ; colIndex++) {//for each column in the row	
			if (colIndex == 0){
				var cellValue = $('<input type="radio" name="radioBtn" id="radioBtn" value="' + myList[i][columns[colIndex+1]] + '">');
			}
			else{
				var cellValue = myList[i][columns[colIndex]]; //set the cell value to client in the client list (????, why is it being referred as a 2d array?)
			}
            
			if (cellValue == null) { cellValue = ""; } //if its empty, make it ""
            row$.append($('<td/>').html(cellValue));
		}
        $("#client_filter_table").append(row$); //to be renamed
    }
}

// Adds a header row to the table and returns the set of columns.
// Need to do union of keys from all records as some records may not contain
// all records
function addAllColumnHeaders(myList) {
    var columnSet = [];
    var headerTr$ = $('<tr/>');
	
    for (var i = 0 ; i <= myList.length+1; i++) {
		if (i==0){
			var rowHash = " ";
			columnSet.push(rowHash);
			headerTr$.append($('<th/>').html(rowHash));
		}
		else{
			var rowHash = myList[i];
			for (var key in rowHash) {
				if ($.inArray(key, columnSet) == -1) {
					columnSet.push(key);
					headerTr$.append($('<th/>').html(key));
				}
			}
		}	
	}
	
    $("#client_filter_table").append(headerTr$); //To be renamed.
    return columnSet;
}
