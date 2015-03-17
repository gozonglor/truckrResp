$(document).ready(function(){
	$("#filter_button").click(function(){
		$("#selected_clients").show();
	});
});

function generateID(newClient){
	var firstName = newClient.firstName;
	var chars = firstName.split("");
	var firstLetter = chars[0];
	var randomFIndex = Math.floor((Math.random() * ((firstName.length)-1)) + 1);
	var nextLetter = chars[randomFIndex];
	
	var bit1 = firstLetter.charCodeAt(0);
	var bit2 = nextLetter.charCodeAt(0);
	
	//var phone = newClient.phone;
	//var phoneBits = (""+phone).split("");//turn it into a string before splitting it
	//var bit3 = phoneBits.slice(6, 9);
	
	var lastName = newClient.lastName;
	var charsL = lastName.split("");
	var randomLIndex = Math.floor((Math.random() * ((lastName.length)-1)) + 1);
	var lastLetter = charsL[randomLIndex];
	
	var bit4 = lastLetter.charCodeAt(0);
	
	//var finalID = ""+bit1+bit2+bit3+bit4;
	var finalID = ""+bit1+bit2+bit4;
	return finalID;
}

function compileRelationshipNC(){
	var checkboxes = document.forms['relationship_form'].elements;
	allRelationships = new Array();
	allRelationships[0] = checkboxes[0].name;
	return allRelationships;
}

function validateFormNC() {
	var elements = document.getElementById("new_client_form").elements;	
	
	for (var i=0; i < elements.length; i++){
		//if (selectedRelationships.length != 0){
			//good you passed the first test
		//	if (element[i].type == 'checkbox'){
				//do not worry then
		//	}
		//	else{
				if (((elements[i].value == "") || (elements[i].value == null)) || (elements[i].value ==0)) {
					alert("Please fill out all fields.");
					return false;
			}
		}
	}

$(function () {  
    $("#create_new_client_button").click(function (e) {
		var NewPerson = {};    

		NewPerson.contact_relationship = compileRelationshipNC();
		NewPerson.contact_status = $("#ddlContactStatus").val();
        NewPerson.id = $("#id").val();
        NewPerson.firstName = $("#firstName").val();
        NewPerson.lastName = $("#lastName").val();
        NewPerson.phone = $("#phone").val();
        NewPerson.org = $("#org").val();
        NewPerson.email = $("#email").val();
        NewPerson.address = $("#address").val();
        NewPerson.city = $("#city").val();
        NewPerson.zip = $("#zip").val();
        NewPerson.state = $("#state").val();
        NewPerson.notes = $("#notes").val();
		 
		 
		NewPerson.id = generateID(NewPerson); 
		var validation = validateFormNC();
	
		var success=false;
        
		var response =$.ajax({
            type: "POST",
            url: "http://localhost:49234/api/client",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
			async: false,
            data: JSON.stringify(NewPerson),
			 
            success: function (response) {
				if  (validation == false){
					//Taken care of in the validation function declared earlier.
				}
				else{
					window.location="http://localhost:49234/generate.html";
				}
            },
			
            error: function(){
				sucess=false;
                alert("no success--error");
            }
        }).responseText;
	e.preventDefault();
    });
});

/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////

//validateForm -- a function that checks to make sure the user has filled out 
//user name and password fields using simple if __ or ____ logic.
//returns false if either password of userID filed is empty/null.
function validateFormLOG() {
    var x = document.forms["login_form"]["userID"].value;
	var y = document.forms["login_form"]["password"].value;

    if ((x==null || x=="") || (y==null || y=="")) {
        alert("Please fill out all fields.");
        return false;
    }
}

$(function () { 
    $("#login_button").click(function (e) {
		var validation = validateFormLOG();
		var NewPerson = {};    

        NewPerson.un = $("#userID").val();
        NewPerson.pw = $("#password").val();
        
		var success=false;
        
		var response =$.ajax({
            type: "POST",
            url: "http://localhost:49234/api/confirm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
			async: false,
            data: JSON.stringify(NewPerson),
			 
            success: function (response) {
				//success =true;
				//alert("successful controller call")
				if (response == true){
					allowLogin();
				}
				else{
				if  (validation == false){
				}
				else{
				alert("Wrong password or username")
				}
				}
            },
			
            error: function(){
				sucess=false;
                alert("no success--error");
            }
        }).responseText;
	e.preventDefault();
    });
});

function allowLogin(data) {
    window.location="http://localhost:49234/clients.html";
}


/////////////////////////////////////////////////////////////////



function compileRelationshipFilter(){
	var checkboxes = document.forms['relationship_form'].elements;
	allRelationships = new Array();
	allRelationships[0] = checkboxes[0].name;
	return allRelationships;
}

$(function(){
    $('#selectedClient_button').click(function(){
       //do something with the selected client
    });
  });

$(function () {
    $("#filter_button").click(function (e) {
        var NewPerson = {};
		NewPerson.contact_relationship = compileRelationshipFilter();
		NewPerson.contact_status = $("#ddlContactStatus").val();
        NewPerson.id = $("#id").val();
        NewPerson.firstName = $("#firstName").val();
        NewPerson.lastName = $("#lastName").val();
        NewPerson.phone = $("#phone").val();
        NewPerson.org = $("#org").val();
        NewPerson.email = $("#email").val();
        NewPerson.address = $("#address").val();
        NewPerson.city = $("#city").val();
        NewPerson.zip = $("#zip").val();
        NewPerson.state = $("#state").val();
        NewPerson.notes = $("#notes").val();
		
		var success=false;
        
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
                alert("no success--error");
            }
        }).responseText;
					e.preventDefault();
    });
});



function handleData(data) {
    buildHtmlTable(data);
	//checkIfSelected();
	//isChecked();
	//returnChosenOne(data);
}

function returnChosenOne(myList) {
	alert($('input[name="chosenClient"]:checked').val());
}

function isChecked(){
var table = document.getElementById('clientTable');
	for (var i = 0; i < table.rows.length; i++){
		alert(table.rows[0].innerHTML)
	}
}

function checkIfSelected(){
	var table = document.getElementById("selected_clients");
	for (var i = 0 ; i <= table.width+1 ; i++) {//for each client
	    for (var colIndex = 0 ; colIndex < columns.length ; colIndex++) {//for each column in the row
			if (colIndex == 0){
				if (document.getElementById("").checked == true){
					var chosen = table.rows[i].cells[1].value;
					alert(chosen);
				}
				}
	}
}
}

function buildHtmlTable(myList) {
    var columns = addAllColumnHeaders(myList);//call function taking in list of clients, 
		//var radioBtn = $('<input type="radio" name="radioBtn"/>');

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
        $("#clientTable").append(row$);
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
		    //radioBtn.append(
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
    $("#clientTable").append(headerTr$);

    return columnSet;
}

