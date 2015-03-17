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

function compileRelationship2(){
	var checkboxes = document.forms['relationship_form'].elements;
	allRelationships = new Array();
	allRelationships[0] = checkboxes[0].name;
	return allRelationships;
}

function compileRelationship(){
	var checkboxes = document.forms['relationship_form'].elements;
	allRelationships = new Array();
	var k=0;
	for (var i=0; i < checkboxes.length; i++){
		if (checkboxes[i].checked){
			k++;
			allRelationships[k] = checkboxes[i];
		}
	}
	return allRelationships;
}

function validateChecks(){
	var check = false;
	selectedRelationships = compileRelationship();
	if (selectedRelationships.length > 0){
		check = true;
	}
	
	if (check == false){
		alert("Contact relationship needs to be specified");
	}
	return check;
}

function validateForm() {
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

		NewPerson.contact_relationship = compileRelationship2();
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
		var validation = validateForm();
		//var validation2 = validateChecks();
		//No longer needing check validation since all clients are set to computer donors on default.
        
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
    //typeof json;
    //buildHtmlTable(json);
	//return success
});