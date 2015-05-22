//TO-DO: Global variable to change and switch production from on to off. 
//Look at this fine fine piece of code: http://jsfiddle.net/vovkss/CSKDK/
//TO DO: Find ways to simplify the compileRelationship_____() methods
//Global Variables-----------------------------------------------------
//---State information
//Client/donor data: Could be simplified by putting it into a client object => {}
var testing = true;
var chosenClientID = 0; //the ID of the chosen donor. Example: "8"
var chosenClientFName; //the chosen donor's first name. Example: "Harry"
var chosenClientLName; //the chosen donor's last name. Example: "Potter"
var donorOrgS = ""; //name of the org the donor is tied to. Is updated in the call to the transferform controller.
var donorEmail = ""; //donor's email, will be data sent to the email controller
var donorID = 0;

//Transfer form data
var lotNumber = 0; //LOT NUMBER DATA -- to be updated by the generateLotNum1 method
var base64SigImg = ""; //SIGNATURE DATA -- the donor's signature in a base 64 string
var customer_signature = false; //BOOLEAN DATA for validating the transfer form -- "is there a signature?"
var globalDate = ""; //DATE DATA -- the date on which the form was filled out
var formHtml = ""; //EMAIL BODY DATA -- Text sent in the body of the email

//---Authentication
var ekey = "";
var currentUser = ""; //Username, example: "glor"
var locationID = 0;
var wordage = "";


//----------------------------------------------------------------------


//Set up function called upon load of clients.html
//Initializes the global username and ekey in the js by using the query string generated from login page
//This is how the current user follows us through the different modules and actions on the app
function loadCurrentUser() {
    //example url: ...clients.html?user=glor&ABC123
    var url = document.URL; //get current page url
    var urlTokens = url.split('='); //split into local url and query string
    var query = urlTokens[1]; //get query string
    //currentUser = vars; //for testing

    if (typeof query !== "undefined") {
        query = query.split('&');
        var name = query[0];
        name = name.split('!');
        var username = name[1];
        locationID = name[0];
        currentUser = username;
        ekey = query[1];
        document.getElementById("hidden_username").innerHTML = username;
        document.getElementById('show_user_status').innerHTML = username + " " + locationID;
        //alert("currentUser: "+currentUser+" ekey: "+ekey); //for testing
    } else {
        ekey = null;
    }
}

//Generate a client id for a new client. 
//Parameter: a client object 
//Output: a string representing the id
//formula for generating a new id:
//[FIRST LETTER OF FIRST NAME]+[RANDOM LETTER IN FIRST NAME]+[RANDOM LETTER OF LAST NAME] 
//
//**comments: the purpose is to AVOID conflicts when multiple ppl using the app create a new client
//thus, we want to create a unique id
function generateID(newClient) {
    var firstName = newClient.firstName; //Grab the first name of the new donor
    var charsF = firstName.split(""); //Split the first name into characters, grab the array of characters
    var firstLetter = charsF[0]; //Grab the first character of donor's first name
    var bit1 = firstLetter.charCodeAt(0); //Generate the unicode/ansci integer of the first character

    //var randomFIndex = Math.floor((Math.random() * ((firstName.length-1))) + 0); //Generate a random number
    //var nextLetter = charsF[randomFIndex]; //Grab a random character from the donor's first name
    //var bit2 = nextLetter.charCodeAt(0); //Generate the uncode/ansci integer of the random character

    var lastName = newClient.lastName; //Grab the last name of the new donor
    var charsL = lastName.split(""); //Grab the character array of the last name 
    var randomLIndex = Math.floor((Math.random() * ((lastName.length - 1))) + 0); //Generate a random number
    var lastLetter = charsL[randomLIndex]; //Grab a random letter from the last name
    var bit3 = lastLetter.charCodeAt(0); //Unicode/ansci integer of the random letter from last name

    var bit4 = currentUser;
    bit4 = bit4.charCodeAt(0);
    bit4 = bit4.toString().charAt(0);

    bit1 = bit1.toString().charAt(0);

    var finalID = "" + bit1 + bit3 + bit4; //String the 'bits' together

    return finalID;
}

//Output: List of relationships associated with particular donor
//Currently returning a list with only 1 object: the standard default 'computer donor' relationship.
//
//**comments: Keeping this. May be useful in future if we want to add more to filtering and creating new clients.
//FUNCTION PUTS TOGETHER THE DONOR RELATIONSHIP. ORIGINALLY CHECKBOXES THAT WERE DISPLAYED ON HTML PAGE. HTML DEFAULTS TO ONE CHECKBOX: DONOR.
//ALSO THINK OF IT AS A SETTER FOR THE RELATIONSHIP ATTRIBUTE IN A CLIENTOBJ.
//USED IN THE NEW_CLIENT_MODULE
function compileRelationshipNew() {
    var checkboxes = document.forms['relationship_new_form'].elements; //Grab any existing checkboxes in the relationship_new_form.
    allRelationships = []; //An array/list to hold the relationship checkboxes that were checked 
    allRelationships[0] = checkboxes[0].name; //But again, since we technically only have one relationship... just grab the first and only element in the list
    return allRelationships; //Return the list, which holds the literal string "Donor" 
}

//builds the list of relationship (donor), looks at the relationship_filter_form instead	   
function compileRelationshipFilter() {
    var checkboxes = document.forms['relationship_filter_form'].elements;
    allRelationships = [];
    allRelationships[0] = checkboxes[0].name;
    return allRelationships;
}

//Output: Always return false if any required fields are not filled out. Also updates view to show error message.
//USED IN THE NEW_CLIENT_MODULE
function validateFormNew() {
    var elements = document.getElementById("new_client_form").elements; //Grab the form fields in the new_client_form
    var check = true;
    for (var i = 0; i < elements.length; i++) { //For each form field...
        if (!(elements[i].name === "optional")) {
            //alert("current field: "+elements[i].value);

            if (((elements[i].value === "") || (elements[i].value === null)) || (elements[i].value === 0)) {
                alert("Please fill out all fields");
                document.getElementById("error_message_create_new").innerHTML = "Please fill out all fields"; //Replace the error message in the document with this error message
                //reset the message after every click, run on every post back
                check = false;
                return check;
            } else {
                document.getElementById("error_message_create_new").innerHTML = ""; //Replace the error message in the document with this error message

                check = true;
            }
        }
    }
    return check;
}

//User filters for a client, pressing 'filter client' button --> controller calls methods to build filtered client table
//Contains Post request to filter controller, passing in a client object and returning list of client objects
$(function() {
    $("#filter_button").click(function(e) {

        $("#client_table_module").hide();
        $("#client_filter_table").html("");
        document.getElementById("error_message_filter").innerHTML = "";
        document.getElementById("error_message_select").innerHTML = "";

        var NewPerson = {};
        NewPerson.contact_relationship = compileRelationshipFilter();
        NewPerson.contact_status = $("#ddlContactStatus_f").val();
        NewPerson.id = $("#id_f").val();
        NewPerson.firstName = $("#firstName_f").val();
        NewPerson.midName = $("#midName_f").val();
        NewPerson.lastName = $("#lastName_f").val();
        NewPerson.phone = $("#phone_f").val();
        NewPerson.org = $("#org_f").val();
        NewPerson.email = $("#email_f").val();
        NewPerson.address = $("#address_f").val();
        NewPerson.city = $("#city_f").val();
        NewPerson.zip = $("#zip_f").val();
        NewPerson.state = $("#state_f").val();
        NewPerson.notes = $("#notes_f").val();
        NewPerson.locationid = parseInt(locationID);

        var formFill = validateFormFilter();
        var success = false;

        if (formFill === true) {
            document.getElementById("error_message_filter").innerHTML = "";
            $("#filter_module").hide();
            $("#client_table_module").show();
            document.getElementById("title_bar").innerHTML = "Filtered Client Table";

            var response = $.ajax({
                type: "POST",
                url: "http://truckrtest.pcscrm.com/api/filter",
                async: false,
                headers: {
                    "Authorization": "Basic " + ekey,
                    "Content-Type": "application/json",
                    "Connection": "keep-alive"
                },
                data: JSON.stringify(NewPerson),
                //Authorization: Basic ekey,

                success: function(data) {
                    if (data.length == 0) {
                        //alert("No results found");
                        document.getElementById("error_message_select").innerHTML = "No results found.";
                        document.getElementById("message_select").innerHTML = data.length + " result(s) found.";
                    } else {
                        document.getElementById("error_message_select").innerHTML = "";
                        document.getElementById("message_select").innerHTML = data.length + " result(s) found.";
                        success = true;
                        handleData(data);
                    }
                },
                error: function() {
                    sucess = false;
                    //ekey = "";
                    //automaticLogout();
                    //document.getElementById("error_message_filter").innerHTML = "No connection to API/Invalid encryption key.";
                    alert("No connection to API/Invalid encryption key.");
                }
            }).responseText;
            e.preventDefault();
        } else {
            $("#client_table_module").hide();
            document.getElementById("error_message_filter").innerHTML = "Please specify a field to filter by.";
            alert("Please specify a field to filter by.");

        }

    });
});

//"logging out"
function automaticLogout() {
    ekey = "";
    currentUser = "";
    currentDate = "";
    customer_signature = false;
    testing = true;
    chosenClientID = 0; //the ID of the chosen donor. Example: "8"
    chosenClientFName; //the chosen donor's first name. Example: "Harry"
    chosenClientLName; //the chosen donor's last name. Example: "Potter"
    donorOrgS = ""; //name of the org the donor is tied to. Is updated in the call to the transferform controller.
    donorEmail = ""; //donor's email, will be data sent to the email controller

    //Transfer form data
    var lotNumber = 0; //LOT NUMBER DATA -- to be updated by the generateLotNum1 method
    var base64SigImg = ""; //SIGNATURE DATA -- the donor's signature in a base 64 string
    var globalDate = ""; //DATE DATA -- the date on which the form was filled out
    var formHtml = ""; //EMAIL BODY DATA -- Text sent in the body of the email
    window.location = "index.html";
}

//User is creating a new client --> We're sending the new client to the database.
//Controller that waits for when user creates a new client. 
//Contains Post request to newclient controller, passing in a client object, returning success (true) or failure (false).
//USED IN THE NEW_CLIENT_MODULE
$(function() {
    $("#create_new_client_button").click(function(e) {

        ////*/*/CALLING AUTHORIZE/*/*///

        //First, validate the form.
        var validation = validateFormNew();
        if (validation === false) { //If not all fields are filled in...
            return; //Break out of the entire function
            //Error message is executed in validateFormNew();
        }

        //If we got to this point, validation was true... so start compiling the NewPerson object.
        var NewPerson = {};

        //Compiling the NewPerson object by looking at the form fields...
        NewPerson.contact_relationship = compileRelationshipNew();
        NewPerson.contact_status = $("#ddlContactStatus_new").val(); //looks at value entered in form field with given id 
        NewPerson.id = $("#id_new").val();
        NewPerson.firstName = $("#firstName_new").val();
        NewPerson.lastName = $("#lastName_new").val();
        NewPerson.phone = $("#phone_new").val();
        NewPerson.org = $("#org_new").val();
        NewPerson.email = $("#email_new").val();
        NewPerson.address = $("#address_new").val();
        NewPerson.address2 = $("#address2_new").val();
        NewPerson.city = $("#city_new").val();
        NewPerson.zip = $("#zip_new").val();
        NewPerson.state = $("#state_new").val();
        NewPerson.notes = $("#notes_new").val();
        NewPerson.username = currentUser;
        NewPerson.key = ekey;
        NewPerson.id = generateID(NewPerson); //Generate an id for the person using the generateID function.
        NewPerson.locationid = parseInt(locationID);

        var response = $.ajax({ //use jquery to make a post to the api with the new client 
            type: "POST",
            url: "http://truckrtest.pcscrm.com/api/newclient",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            headers: {
                "Authorization": "Basic " + ekey,
                "Content-Type": "text/json",
                "Connection": "keep-alive"
            },
            data: JSON.stringify(NewPerson), //Passing in NewPerson as a JSON object -- the new client that was just created

            success: function(data) { //the connection was successfully made

                //alert("Data: "+data);
                if (data !== false) { //[TBD] if the list returned by the api is not empty (meaning we have successfully passed the newly created client over to the api side to be dealth with via database
                    //alert("New client successfully added.");
                    document.getElementById("error_message_create_new").innerHTML = ""; //Error message is cleared out
                    document.getElementById("message_create_new").innerHTML = "Donor '" + NewPerson.firstName + " " + NewPerson.lastName + "' has been created."; //Good message posted to let user know their client has been created
                    chosenClientFName = NewPerson.firstName; // Assigned the global variable to be the new person first name and last name
                    chosenClientLName = NewPerson.lastName;
                    donorOrgS = NewPerson.org;
                    chosenClientID = NewPerson.id;
                    var hiddenDonor = document.getElementById('hidden_client_id'); //grab the hidden donor field, which is a div
                    hiddenDonor.innerHTML = getDonor();

                    var showDonorStatus = document.getElementById('show_donor_status');
                    showDonorStatus.innerHTML = chosenClientFName + " " + chosenClientLName;
                    $("#new_client_module").hide();
                    generateLotNum1();
                    donorEmail = NewPerson.email;
                    $("#generate_module").show();
                    document.getElementById("title_bar").innerHTML = "Generate Form";

                } else {
                    // alert("empty list");			
                    document.getElementById("error_message_create_new").innerHTML = "Please fill out all fields";
                }
            },

            error: function() {
                // alert("no success--error");
                automaticLogout();
                alert("No connection to API/Invalid encryption key.");
            }
        }).responseText;
        e.preventDefault();
    });
});

//User is starting to create a new transfer form --> We're sending pre-information to build the final transfer form
//Contains Post request to transfer form controller, passing in a 'building form' object, returning the language on the transfer form
//USED IN THE GENERATE_MODULE, IMPORTANT: To do: ACTUAL TEXT OF THE TRANSFER FORM IS BEING STORED/HARD CODED IN THE TRANSFER FORM CONTROLLER
$(function() {
    $("#transfer_button").click(function(e) {
        ////*/*/CALLING AUTHORIZE/*/*///

        var BuildingForm = {};

        BuildingForm.naidChoice = $('input[name=naidOption]:checked').val(); //will either be a string stating 'yesNaid' or 'noNaid'
        BuildingForm.donationDescription = $("#donation_description").val();
        BuildingForm.org = donorOrgS;

        var result = validateTransferForm();
        var success = false;
        if (result === false) { //do nothing if the form is valid. note: error handling was covered in validateTransferForm() 
        } else {
            var response = $.ajax({
                type: "POST",
                url: "http://truckrtest.pcscrm.com/api/transferform",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                headers: {
                    "Authorization": "Basic " + ekey,
                    "Content-Type": "text/json",
                    "Connection": "keep-alive"
                },
                data: JSON.stringify(BuildingForm),
                //Authorization: Basic ekey,

                success: function(response) {
                    $("#generate_module").hide();
                    $("#transfer_module").show();
                    document.getElementById("title_bar").innerHTML = "Transfer of Ownership";
					

                    var formOwnership = donorOrgS;
                    response = response.split("%");
					


                    if (response[0] == "False") { //if there is no org tied to the donor
                        formOwnership = chosenClientFName.toUpperCase() + " " + chosenClientLName.toUpperCase() + " "; //form ownership information belongs to individual donor
                    } //else, the passed in response[0] already has the org name in it, no need to parse it or add an else statement.
					
					var cleanResponse = response[1].split("_");
					var firstParagraph = cleanResponse[0] + formOwnership + cleanResponse[1]; 
					
					wordage = formOwnership+"%"+firstParagraph+"%"+response[4]; //store global form wordage to be referenced when form is submitted...

					
                    //First Paragraph
					formHtml = ""; //formatted transfer of ownership form
                    document.getElementById("customTOOForm").innerHTML = "<b>"+formOwnership+"</b>" + firstParagraph; 
                    var data = formOwnership + response[1] + " what is this Data?!"; //Not sure what this this?
                    formHtml = data; 
					
					//Second Paragraph: Donation description
                    document.getElementById("customTOOForm").innerHTML = document.getElementById("customTOOForm").innerHTML+ "<br><br><div style='background:#EEEEEE'>"+response[2]+"</div>";

					//Third Paragraph: Data wipe choice, naid or no naid
					document.getElementById("customTOOForm").innerHTML = document.getElementById("customTOOForm").innerHTML+ "<br><br><div style='border: 2px solid; padding: 10px;'>"+response[3]+"</div>";
                    

                    var dateObj = new Date();
                    var month = dateObj.getUTCMonth() + 1; //months from 1-12
                    var day = dateObj.getUTCDate();
                    var year = dateObj.getUTCFullYear();
                    var newdate = "" + month + "/" + day + "/" + year;
                    globalDate = newdate;
                    document.getElementById("transfer_form_date1").innerHTML = "Date: " + globalDate + " " + " Lot Number: " + lotNumber;
                },

                error: function() {
                    sucess = false;
                    automaticLogout();
                    //document.getElementById("error_message_transfer").innerHTML = "No connection to API/Invalid encryption key.";
                    alert("No connection to API/Invalid encryption key.");
                }
            }).responseText;
            e.preventDefault();
        }
    });
});

function init() {
    document.getElementById("setScroll").addEventListener('click', function() {
        document.getElementById("").style.top = -120; //Scroll by 120px
    });
}

//User tries to login, clicks login buttom --> button listener authenticates user
//user object with encrypted pw and username is posted to confirm controller
//Used in the index.html page
$(function() {
    $("#login_button").click(function(e) {
        var validation = validateFormLogin();
        var NewPerson = {};

        NewPerson.username = $("#userID").val();
        pass = (CryptoJS.SHA1($("#password").val()));
        newpass = (pass.toString(CryptoJS.enc.Hex)).toUpperCase();

        NewPerson.pw = newpass;

        var success = false;

        if (validation === false) {
            document.getElementById("error_message_login").innerHTML = "Please fill out all fields.";
            alert("Please fill out all fields");

        } else {
            var response = $.ajax({
                type: "POST",
                url: "http://truckrtest.pcscrm.com/api/confirm",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify(NewPerson),

                success: function(response) {
                    if (response !== "401") { //originally, if response == true, checks if user is authorized. returns 401 if not authorized.
                        currentUser = NewPerson.username;
                        document.getElementById("error_message_login").innerHTML = 'Logging in...';
                        //setTimeout(allowLogin, 3000);
                        //allowLogin();
                        var guidlocation = response.split("_");

                        ekey = guidlocation[0];
                        locationID = guidlocation[1];
                        //alert("location id is "+locationID);

                        //allowLogin();
                        window.location = "clients.html" + "?user=" + locationID + "!" + currentUser + "&" + ekey;


                    } else {
                        document.getElementById("error_message_login").innerHTML = "Wrong password or username.";
                        alert("Wrong password or username.");

                    }
                },

                error: function() {
                    sucess = false;
                    document.getElementById("error_message_login").innerHTML = "No connection to API/Invalid encryption key.";
                    alert("No connection to API/Invalid encryption key.");

                },
                beforeSend: function(xhr) {
                    xhr.setRequestHeader('Authorization', 'Basic ' + ekey);
                }
            }).responseText;
        }
        e.preventDefault();
        setTimeout(resetError, 3000);
        //http://stackoverflow.com/questions/20890943/javascript-settimeout-not-working
    });
});

//User is selecting a client, pressing 'select client' button 
//--> We are updating our global client/donor data variables to match the selected client's data
//USED IN THE FILTERED_CLIENTS_MODULE*or something close to it
$(function() {
    $('#selected_client_button').click(function() {
        var checked = false;
        ////*/*/CALLING AUTHORIZE/*/*///
        ////*/*/CALLING AUTHORIZE/*/*///
        var table = document.getElementById('client_filter_table'); //grab the table of filtered clients 
        for (var i = 1; i < table.rows.length; i++) { //loop through them and grab their information
			var entry = table.rows[i];
            var firstCol = entry.childNodes[0];
            var radioButton = firstCol.firstChild;
            var clientID = entry.childNodes[1];
			            alert("here is the id of the donor: "+clientID);
			donorID = clientID;
            var clientFName = entry.childNodes[2];
            var clientLName = entry.childNodes[3];
            clientFName = clientFName.innerText;
            clientLName = clientLName.innerText;
            if (radioButton.checked) { //checks if the 'i' particular radio button is checked --i.e. the 3rd if you are on the fourth iteration of the loop
                checked = true;
                document.getElementById("error_message_select").innerHTML = "You have selected " + clientFName + " " + clientLName;

                //GLOBALIZING				
                //chosenClientID = clientID.firstChild.data; //set the global variable chosenClientID to be the id of the selected child
                chosenClientFName = clientFName; //set global variable of first name
                chosenClientLName = clientLName; //set global variable of last name
                donorEmail = entry.childNodes[6].innerText;
                donorOrgS = entry.childNodes[5].innerText;

                //alert("Globalizing line 265: "+donorEmail+" .... "+donorOrgS);
                //show the generate module
                $("#generate_module").show();

                break; //break out of the for loop, since we have a selected donor already
            }
        }

        //if we have looped through the entire table of filtered clients and none of the radio buttons were checked
        //the chosenClientID must be null (has not been set to anything), and if it is null...
        if (checked === false) {
            document.getElementById("error_message_select").innerHTML = "Please select a donor.";
            alert("Please select a donor");
        } else { //otherwise... if we have selected a client
            var hiddenDonor = document.getElementById('hidden_client_id'); //grab the hidden donor field, which is a div
            // var hiddenDonorName = document.getElementById('hidden_donor_name'); //grab the hidden donor field, which is a div
            // hiddenDonorName.innerHTML = ""+chosenClientFName+" "+chosenClientLName+"."; //set the innerHTML to be the id of the donor

            var showDonorStatus = document.getElementById('show_donor_status');
            showDonorStatus.innerHTML = donorOrgS +"<br>"+ clientFName + " " + clientLName;
            hiddenDonor.innerHTML = getDonor();
            document.getElementById("message_select").innerHTML = "Grabbing from the database...Donors are successfully filtered.";
            generateLotNum1();
            $("#client_table_module").hide(); //Hide current module
            $("#filter_module").hide(); //Hide current module
            $("#generate_module").show(); //Show the wanted module
        }

    });
});


//this function will clear out the filter fields
function clearFilterForm(){
	//document.getElementById("filtering_form").
	$('#filtering_form').trigger("reset");
}


//this function will clear out the create new client fields
function clearNewClientForm(){
	//document.getElementById("filtering_form").
	$('#new_client_form').trigger("reset");
}

//this function will clear out the creating forms
function clearCreatingForm(){
	//document.getElementById("filtering_form").
	$('#generate_too_form').trigger("reset");
}

//this function will clear out the too form
function clearTooForm(){
	$('#clearButton').trigger('click');
	$('#name').val('');
	$('#actual_too_form').trigger("reset");
	document.getElementById("typed_name").innerHTML = "";
	document.getElementById("typed_name").innerText = ""; 
	//document.getElementById("canvas_pad") = "";
	document.getElementById("sigImage").innerHTML = "";

}

//this function clears out every field in the db
function clearAllForms(){
	clearCreatingForm();
	clearFilterForm();
	clearNewClientForm();
	clearTooForm();
}
//Submitting the donor's signature
//--> Record it as a base 64 image to store and display again on the final pdf transfer form.
//Does validation and redisplays the signature on the next page 
$(function() {
    $("#submit_me").click(function(e) {
        var valid = validateSignature();
		sig_coord = document.getElementById("output").defaultValue;
        if ((valid === true) && (sig_coord != "")){
		
            document.getElementById("sig_message").innerHTML = "";
            document.getElementById("sig_error_message").innerHTML = "";
            //sig_coord = document.getElementById("output").defaultValue;

            if (sig_coord !== "") {
                $('.signed').show();
                customer_signature = true;
                var imgBaseSig = document.getElementById("canvas_pad").toDataURL();
                //alert("Original "+imgBaseSig);
                //imgBaseSig = imgBaseSig.split(",");
                document.getElementById("test_sig").innerHTML = imgBaseSig;
                //base64SigImg = imgBaseSig[1]; //store globally...

                base64SigImg = imgBaseSig;

                document.getElementById("sigImage").innerHTML = "<img src='" + base64SigImg + "'>";

                //document.getElementById("test_sig").innerHTML = document.getElementById("test_sig").innerHTML+"<br><br>"+base64SigImg;
                var api = $('.signed').signaturePad({
                    displayOnly: true
                }); //--yposs
                api.regenerate(sig_coord); //--
                //alert("base 64 image: "+base64SigImg);
                document.getElementById("typed_name").innerText = document.getElementById("name").value;
            } 
			// else {
                // customer_signature = true;
                // document.getElementById("typed_name").innerText = document.getElementById("name").value;
                // base64SigImg = document.getElementById("typed_name").innerText;
				// $('.signed').hide();
            // }

            $('#signature_module').hide();
            $('#transfer_module').show();
        } else { document.getElementById("sig_error_message").innerText = "Please sign.";
				alert("Please sign.");
}
    });
});

//Generates the lot number and populates the lot number fields in the generate module.
function generateLotNum1() {
    $("#generate_module").show();
    document.getElementById("title_bar").innerHTML = "Generating Form";
    var donor = document.getElementById("hidden_client_id").innerHTML;
    var currentDate = new Date();
    var minutes = currentDate.getMinutes();
    var seconds = currentDate.getSeconds();
    var lotNum = "" + donor + minutes + seconds;
    var lotNumField = document.getElementById("lot_number");
    //lotNumField.readOnly = false;
    //lotNumField.value = lotNum;
    //lotNumField.readOnly = true;
    lotNumField.innerHTML = lotNum;
    lotNum = parseFloat(lotNum);
    lotNumber = lotNum;
}

//Simple validation for the form to filter clients.
//returns true or false.
function validateFormFilter() {
    var requiredFields = document.getElementById("filtering_form").elements;
    var check = false;
    for (var i = 0; i < requiredFields.length; i++) {
        var val = requiredFields[i].value;
        if (val !== "") {
            check = true;
            break;
        }
    }
    return check;
}

//Validation for the signature piece: there must be a written or typed signature
function validateSignature() {
    var valid = false;
    if (document.getElementById("name").value === "") {
        document.getElementById("sig_error_message").innerText = "Please print donor name.";
		alert("Please print donor name");
        return valid;
    } 
	
	if (document.getElementById("name").value !== ""){
        valid = true;
        return valid;
    }
}

//validateForm -- a function that checks to make sure the user has filled out 
//user name and password fields using simple if __ or ____ logic.
//returns false if either password of userID filed is empty/null.
function validateFormLogin() {
    var x = document.forms["login_form"]["userID"].value;
    var y = document.forms["login_form"]["password"].value;

    if ((x === null || x === "") || (y === null || y === "")) {
        return false;
    } else {
        return true;
    }
}

//used on the generate module.
//naid and donation description must be filled in.
function validateTransferForm() {
    var result = true;
    if ((typeof $('input[name=naidOption]:checked').val() == 'undefined')) {
        document.getElementById("error_message_generate").innerHTML = "Please fill in all fields.";
        result = false;
    }
    if (!$.trim($("#donation_description").val())) {
        document.getElementById("error_message_generate").innerHTML = "Please fill in all fields.";
        result = false;
    }
    return result;
}

//Validate: did the donor sign/write their name? did the user write their name on the agreement?
//Used in final screen of the module. 
function checkTOOForm() {
    var validate = false;
    document.getElementById("message_transfer").innerHTML = "";
    if (((document.getElementById("client_title").value === "")) || (document.getElementById("p4p_name").value === "")) {
        // document.getElementById("error_message_transfer").innerHTML = "Missing customer signature and/or title.";
        document.getElementById("error_message_transfer").innerHTML = "Missing fields.";
		alert("Please fill out the missing fields!");

    } else {
        if (customer_signature === false) {
            document.getElementById("error_message_transfer").innerHTML = "Missing fields.";
			alert("Please fill out the missing fields!");
        } else {
            validate = true;
        }
    }
    return validate;
}

//Helper function: a simple getter function used to grab the global chosenClientID
function getDonor() {
    return chosenClientID;
}

//Helper function: small helper function to reset the errors on the login page (index.html)
function resetError() {
    document.getElementById("error_message_login").innerHTML = "<br>";
}

//Listens if the user has selected to sign their signature.
//Shows the signature module upon click.
$(document).ready(function() {
    $("#select_signature").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        $("#transfer_module").hide();
        $("#signature_module").show();
        document.getElementById("title_bar").innerHTML = "Client Signature";

    });
});


//Exit button directions
$(document).ready(function() {
    $('#exit_filter_table_button').click(function() {
        $('#client_table_module').hide(); //hide the current module ORGIINALLY COMMMENTED OUT 
    });
});

//Exit button directions
$(document).ready(function() {
    $('#exit_client_sig_button').click(function() {
        $('#signature_module').hide(); //hide the current module ORGIINALLY COMMMENTED OUT 
        $('#transfer_module').show();
        document.getElementById("title_bar").innerHTML = "Transfer of Ownership";

    });
});

//Helper function: switch window location along to url+query
function allowLogin(data) {
    // window.location.href = "Posts?Category=" + sel;
    //ekey = btoa(ekey);
    //
    window.location = "clients.html" + "?user=" + currentUser + "&" + ekey;
    //document.getElementById("testing").innerText="helllooo";
    //buildQuery();
    //alert("whoo");
}

$(document).ready(function() {
    $("#logout_button").click(function() {

        ekey = "";
        currentUser = "";
        currentDate = "";
        customer_signature = false;
        testing = true;
        chosenClientID = 0; //the ID of the chosen donor. Example: "8"
        chosenClientFName; //the chosen donor's first name. Example: "Harry"
        chosenClientLName; //the chosen donor's last name. Example: "Potter"
        donorOrgS = ""; //name of the org the donor is tied to. Is updated in the call to the transferform controller.
        donorEmail = ""; //donor's email, will be data sent to the email controller

        //Transfer form data
        var lotNumber = 0; //LOT NUMBER DATA -- to be updated by the generateLotNum1 method
        var base64SigImg = ""; //SIGNATURE DATA -- the donor's signature in a base 64 string
        var globalDate = ""; //DATE DATA -- the date on which the form was filled out
        var formHtml = ""; //EMAIL BODY DATA -- Text sent in the body of the email
        window.location = "index.html";

    });
});

/////==================crytoJS============================//////////
var CryptoJS = CryptoJS || function(e, m) {
    var p = {},
        j = p.lib = {},
        l = function() {},
        f = j.Base = {
            extend: function(a) {
                l.prototype = this;
                var c = new l;
                a && c.mixIn(a);
                c.hasOwnProperty("init") || (c.init = function() {
                    c.$super.init.apply(this, arguments)
                });
                c.init.prototype = c;
                c.$super = this;
                return c
            },
            create: function() {
                var a = this.extend();
                a.init.apply(a, arguments);
                return a
            },
            init: function() {},
            mixIn: function(a) {
                for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]);
                a.hasOwnProperty("toString") && (this.toString = a.toString)
            },
            clone: function() {
                return this.init.prototype.extend(this)
            }
        },
        n = j.WordArray = f.extend({
            init: function(a, c) {
                a = this.words = a || [];
                this.sigBytes = c != m ? c : 4 * a.length
            },
            toString: function(a) {
                return (a || h).stringify(this)
            },
            concat: function(a) {
                var c = this.words,
                    q = a.words,
                    d = this.sigBytes;
                a = a.sigBytes;
                this.clamp();
                if (d % 4)
                    for (var b = 0; b < a; b++) c[d + b >>> 2] |= (q[b >>> 2] >>> 24 - 8 * (b % 4) & 255) << 24 - 8 * ((d + b) % 4);
                else if (65535 < q.length)
                    for (b = 0; b < a; b += 4) c[d + b >>> 2] = q[b >>> 2];
                else c.push.apply(c, q);
                this.sigBytes += a;
                return this
            },
            clamp: function() {
                var a = this.words,
                    c = this.sigBytes;
                a[c >>> 2] &= 4294967295 <<
                    32 - 8 * (c % 4);
                a.length = e.ceil(c / 4)
            },
            clone: function() {
                var a = f.clone.call(this);
                a.words = this.words.slice(0);
                return a
            },
            random: function(a) {
                for (var c = [], b = 0; b < a; b += 4) c.push(4294967296 * e.random() | 0);
                return new n.init(c, a)
            }
        }),
        b = p.enc = {},
        h = b.Hex = {
            stringify: function(a) {
                var c = a.words;
                a = a.sigBytes;
                for (var b = [], d = 0; d < a; d++) {
                    var f = c[d >>> 2] >>> 24 - 8 * (d % 4) & 255;
                    b.push((f >>> 4).toString(16));
                    b.push((f & 15).toString(16))
                }
                return b.join("")
            },
            parse: function(a) {
                for (var c = a.length, b = [], d = 0; d < c; d += 2) b[d >>> 3] |= parseInt(a.substr(d,
                    2), 16) << 24 - 4 * (d % 8);
                return new n.init(b, c / 2)
            }
        },
        g = b.Latin1 = {
            stringify: function(a) {
                var c = a.words;
                a = a.sigBytes;
                for (var b = [], d = 0; d < a; d++) b.push(String.fromCharCode(c[d >>> 2] >>> 24 - 8 * (d % 4) & 255));
                return b.join("")
            },
            parse: function(a) {
                for (var c = a.length, b = [], d = 0; d < c; d++) b[d >>> 2] |= (a.charCodeAt(d) & 255) << 24 - 8 * (d % 4);
                return new n.init(b, c)
            }
        },
        r = b.Utf8 = {
            stringify: function(a) {
                try {
                    return decodeURIComponent(escape(g.stringify(a)))
                } catch (c) {
                    throw Error("Malformed UTF-8 data");
                }
            },
            parse: function(a) {
                return g.parse(unescape(encodeURIComponent(a)))
            }
        },
        k = j.BufferedBlockAlgorithm = f.extend({
            reset: function() {
                this._data = new n.init;
                this._nDataBytes = 0
            },
            _append: function(a) {
                "string" == typeof a && (a = r.parse(a));
                this._data.concat(a);
                this._nDataBytes += a.sigBytes
            },
            _process: function(a) {
                var c = this._data,
                    b = c.words,
                    d = c.sigBytes,
                    f = this.blockSize,
                    h = d / (4 * f),
                    h = a ? e.ceil(h) : e.max((h | 0) - this._minBufferSize, 0);
                a = h * f;
                d = e.min(4 * a, d);
                if (a) {
                    for (var g = 0; g < a; g += f) this._doProcessBlock(b, g);
                    g = b.splice(0, a);
                    c.sigBytes -= d
                }
                return new n.init(g, d)
            },
            clone: function() {
                var a = f.clone.call(this);
                a._data = this._data.clone();
                return a
            },
            _minBufferSize: 0
        });
    j.Hasher = k.extend({
        cfg: f.extend(),
        init: function(a) {
            this.cfg = this.cfg.extend(a);
            this.reset()
        },
        reset: function() {
            k.reset.call(this);
            this._doReset()
        },
        update: function(a) {
            this._append(a);
            this._process();
            return this
        },
        finalize: function(a) {
            a && this._append(a);
            return this._doFinalize()
        },
        blockSize: 16,
        _createHelper: function(a) {
            return function(c, b) {
                return (new a.init(b)).finalize(c)
            }
        },
        _createHmacHelper: function(a) {
            return function(b, f) {
                return (new s.HMAC.init(a,
                    f)).finalize(b)
            }
        }
    });
    var s = p.algo = {};
    return p
}(Math);
(function() {
    var e = CryptoJS,
        m = e.lib,
        p = m.WordArray,
        j = m.Hasher,
        l = [],
        m = e.algo.SHA1 = j.extend({
            _doReset: function() {
                this._hash = new p.init([1732584193, 4023233417, 2562383102, 271733878, 3285377520])
            },
            _doProcessBlock: function(f, n) {
                for (var b = this._hash.words, h = b[0], g = b[1], e = b[2], k = b[3], j = b[4], a = 0; 80 > a; a++) {
                    if (16 > a) l[a] = f[n + a] | 0;
                    else {
                        var c = l[a - 3] ^ l[a - 8] ^ l[a - 14] ^ l[a - 16];
                        l[a] = c << 1 | c >>> 31
                    }
                    c = (h << 5 | h >>> 27) + j + l[a];
                    c = 20 > a ? c + ((g & e | ~g & k) + 1518500249) : 40 > a ? c + ((g ^ e ^ k) + 1859775393) : 60 > a ? c + ((g & e | g & k | e & k) - 1894007588) : c + ((g ^ e ^
                        k) - 899497514);
                    j = k;
                    k = e;
                    e = g << 30 | g >>> 2;
                    g = h;
                    h = c
                }
                b[0] = b[0] + h | 0;
                b[1] = b[1] + g | 0;
                b[2] = b[2] + e | 0;
                b[3] = b[3] + k | 0;
                b[4] = b[4] + j | 0
            },
            _doFinalize: function() {
                var f = this._data,
                    e = f.words,
                    b = 8 * this._nDataBytes,
                    h = 8 * f.sigBytes;
                e[h >>> 5] |= 128 << 24 - h % 32;
                e[(h + 64 >>> 9 << 4) + 14] = Math.floor(b / 4294967296);
                e[(h + 64 >>> 9 << 4) + 15] = b;
                f.sigBytes = 4 * e.length;
                this._process();
                return this._hash
            },
            clone: function() {
                var e = j.clone.call(this);
                e._hash = this._hash.clone();
                return e
            }
        });
    e.SHA1 = j._createHelper(m);
    e.HmacSHA1 = j._createHmacHelper(m)
})();
////////..............................................////////

////////////////////////////////FOR TESTING PURPOSES/////////////////////////////////////////

$(document).ready(function() {
    $("#show_new_client_button").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        document.getElementById("error_message_create_new").innerHTML = "";
        $("#transfer_module").hide();
        $("#filter_module").hide();
        $("#new_client_module").show();
        document.getElementById("title_bar").innerHTML = "New Client";

    });
});

$(document).ready(function() {
    $("#back_filter").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        document.getElementById("error_message_filter").innerHTML = "";
        // $("#navigation_module").hide();
        $("#filter_module").show();

    });
});



$(document).ready(function() {
    $("#show_filter_button").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        document.getElementById("error_message_filter").innerHTML = "";
        // $("#navigation_module").hide();
        $("#transfer_module").hide();
        $("#new_client_module").hide();
        $("#filter_module").show();
        document.getElementById("title_bar").innerHTML = "Find Client";
    });
});

$(document).ready(function() {
    $("#show_generate_button").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        $("#transfer_module").hide();

        $("#generate_module").show();
        document.getElementById("title_bar").innerHTML = "Generate Form";

    });
});

$(document).ready(function() {
    $("#show_present_button").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        $("#transfer_module").show();
        document.getElementById("title_bar").innerHTML = "Transfer of Ownership";

    });
});

$(document).ready(function() {
    $("#show_table_button").click(function() {
        if ($('[name="module"]').is(':visible')) {
            $('[name="module"]').hide();
        }
        $("#transfer_module").hide();

        $("#client_table_module").show();
        document.getElementById("title_bar").innerHTML = "Filter Table";

    });
});

function handleData(data) { //A smaller function to consolidate the longer ones of building the html table
    buildHtmlTable(data);
}

function buildHtmlTable(myList) {
    var columns = addAllColumnHeaders(myList); //call function taking in list of clients, 
    for (var i = 0; i < myList.length; i++) { //for each client, used to be myList.length+1
        var row$ = $('<tr/>'); //create a row 
        //var row$ = $('<tr/>'); //create a row 
        for (var colIndex = 0; colIndex < columns.length; colIndex++) { //for each column in the row	

            if (i >= 0) {
                if (colIndex == 0) {
                    var cellValue = $('<input type="radio" name="radioBtn" id="radioBtn" value="' + myList[i][columns[colIndex + 1]] + '">');
                } else {
					if (cellValue = myList[i][columns[colIndex]] != ""){ //if it is empty
                    var cellValue = myList[i][columns[colIndex]]; //set the cell value to client in the client list (????, why is it being referred as a 2d array?)
					}
                }

                if (cellValue == null) {
                    cellValue = "";
                } //if its empty, make it ""
                row$.append($('<td/>').html(cellValue));
            }
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

    for (var i = 0; i <= myList.length+1; i++) { //used to be myList.length+1
        if (i == 0) {
            var rowHash = " ";
            columnSet.push(rowHash);
            headerTr$.append($('<th/>').html(rowHash));
        } else {
            var rowHash = myList[i-1];
            for (var key in rowHash) {
                if ($.inArray(key, columnSet) == -1) {
					if ((key == "firstName") || (key == "lastName") || (key == "email") || (key == "org") || (key == "phone") || (key == "contactid")){
                    columnSet.push(key);
                    headerTr$.append($('<th/>').html(key));
					}
                }
            }
        }
   }

    $("#client_filter_table").append(headerTr$); //To be renamed.
    return columnSet;
}

//==================            HTML TO CANVAS              =================//
$(function() {
    $("#accept_button").click(function(e) {
        //instantiate a variable to hold our base 64 string
        var img = "";
        //http://stackoverflow.com/questions/14595541/capture-div-into-image-using-html2canvas
        //generate the base 64 string using html2canvas
        html2canvas($('#actual_form'), {
            onrendered: function(canvas) {
                img = canvas.toDataURL();
                //window.open(img);
                //alert(img);
                //document.getElementById("holder").innerHTML = img;
                //demoFromHTML(img);
                //FUCK it the pdf base 64 keeps getting corrupted. ok now lets see if we can just send an IMAGE >:(
                other(img);
            }
        });
        //put it on a pdf
        //demoFromHTML(img);	
    });
});

function other(pdfBase64) {

    var result = checkTOOForm();
    if (result == false) { //orig false
        //Missing a field--a field is not filled out
    } else {
        var TransferForm = {};

        TransferForm.naidChoice = $('input[name=naidOption]:checked').val(); //will either be a string stating 'yesNaid' or 'noNaid'
        TransferForm.donationDescription = $("#donation_description").val();
        TransferForm.lotNum = lotNumber;
        TransferForm.donorID = parseInt(donorID);
        TransferForm.username = currentUser;
		var cleanSig = base64SigImg.split(',');
        TransferForm.donorSig = cleanSig[1];
        TransferForm.org = donorOrgS;
        TransferForm.dateFilled = globalDate;


        TransferForm.formatted = "Thank you for calling PCs for People to safely and legally dispose of your used hardware. Attached is a screen shot of the transfer of ownership form.";

        // pdfBase64 = pdfBase64.split(',');
        // pdfBase64 = pdfBase64[1];
        // TransferForm.pdf64 = "";

        TransferForm.donorEmail = donorEmail;
        TransferForm.wordage = wordage;
        TransferForm.firstName = chosenClientFName;


        var success = false;
		alert("Created the Transfer form... Making a post request now");

        var response = $.ajax({
            type: "POST",
            url: "http://truckrtest.pcscrm.com/api/submit",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            headers: {
                "Authorization": "Basic " + ekey,
                "Content-Type": "text/json",
                "Connection": "keep-alive" //Note: This might have been the key to solving the issue.
            },
			
			
            data: JSON.stringify(TransferForm),
            //Authorization: Basic ekey,

            success: function(response) {

                if (response == true) {
                    document.getElementById("error_message_transfer").innerHTML = "";
                    document.getElementById("message_transfer").innerHTML = "Transfer Form successfully added to database. Email sent to " + "<b>" + donorEmail + "</b>";
					alert("Transaction completed!");
					//document.getElementById("transfer_module").style.display = "none";
					//document.getElementById("transfer_module").style.visibility = "none";
					//document.getElementById("transfer_module").style.visibility = "none";
					
					$("#transfer_module").hide();
					$("#final_module").show();
					clearAllForms();
                } else {
                    document.getElementById("error_message_transfer").innerHTML = "Successful post. Bad API response. Error: " + response + ".  Could not insert final Transfer Form into db.";
                    alert("AJAX request returned an error. Error: " + response + ".  Could not insert Transfer Form into db.");
                }
            },

            error: function() {
                sucess = false;
                document.getElementById("error_message_transfer").innerHTML = "Error with post request. No connection to API/Invalid encryption key.";
            }
        }).responseText;
        //response.preventDefault();
				alert("Fell through the post request!");

    }
}


// function demoFromHTML(base64){
// var doc = new jsPDF('p', 'in', [8,11]); //say 3 5 instead of 'letter'

// var source = $('#transfer_module')[0];//originally:$('#transfer_module').first()
// var specialElementHandlers = {
// '#bypassme': function(element, renderer) {
// return true; 
// }
// };

// doc.fromHTML(
// $('#transfer_module').get(0), // [Refer Exact code tutorial][2]HTML string or DOM elem ref.
// 0.5,    // x coord, originally 0.5
// 1,    // y coord, originally 0.5
// {
// 'width': 6.5, // max width of content on PDF
// 'elementHandlers': specialElementHandlers
// });

// doc.addImage(base64, 'PNG', 0, 0);

// var holder = doc.output('dataurlstring');

// document.getElementById('holder').innerHTML= holder.trim();
// }

function autoPopZip() {

	var val = "";
	 
	var zip = document.getElementById("zip_new").value;


		val =zip;

    var response = $.ajax({
        type: "GET",
        url: "http://truckrtest.pcscrm.com/api/location/"+val,
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        async: false,
        // data: JSON.stringify(data),

        success: function(response) {
			if (response.cityList.length > 0){ //if it's returning a list of cities that match the zip code
				//hide the single cities input
				document.getElementById("city_new").style.visibility = "hidden";
				document.getElementById("city_new").style.display = "none";
				var listCities = response.cityList;
				populateCitiesDLL(listCities); //populate the drop down list
				document.getElementById("citiesDropDown").style.visibility = "visible"; //finally show the drop down list
				document.getElementById("state_new").value = response.stateLocation;
				document.getElementById("state_new").textContent = response.stateLocation;
			}
			else{
				document.getElementById("city_new").value = response.city;
				document.getElementById("city_new").textContent = response.city;
				document.getElementById("state_new").value = response.stateLocation;
				document.getElementById("state_new").textContent = response.stateLocation;

			}
        },

        error: function() {
            alert("Error.");
        }
    }).responseText;
}


function populateCitiesDLL(listCities){
	var ddlCityMenu = document.getElementById("citiesDropDown");
	for (var i=0; i<listCities.length; i++){
		var city = listCities[i];
		var element = document.createElement("option");
		element.textContent = city;
		element.value = city;
		ddlCityMenu.appendChild(element);
	}
}


			
function autoPopCity() {
  	 
	var city = document.getElementById("cityT").value;

	var cityObject = {};
	cityObject.name = city;
	
    var response = $.ajax({
        type: "GET",
        url: "http://truckrtest.pcscrm.com/api/location/",
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        async: false,
        data: JSON.stringify(city),
		

        success: function(response) {
			if (response.errorMsg == ""){

				if (response.zipList.length > 0){ //if it's returning a list of cities that match the zip code
					//hide the single cities input
					document.getElementById("zip_new").style.visibility = "hidden";
					document.getElementById("zip_new").style.display = "none";
					var listZips = response.zipList;
					populateZipcodesDLL(listZips); //populate the drop down list
					document.getElementById("zipcodesDropDown").style.visibility = "visible"; //finally show the drop down list
					document.getElementById("state_new").value = response.stateLocation;
					document.getElementById("state_new").textContent = response.stateLocation;
				}
				else{
					document.getElementById("state_new").value = response.stateLocation;
					document.getElementById("state_new").textContent = response.stateLocation;
					document.getElementById("zip_new").value = response.zip;
					document.getElementById("zip_new").textContent = response.zip;

				}
			}
			else{
				alert("Sorry, there is no existing data to reference and match with your input... but you can carry on like normal.");
			}
        },

        error: function() {
            alert("Error.");
        }
    }).responseText;
}

//to help auto pop the existing input field with a choice chosen from the drop down list
//source/help: http://stackoverflow.com/questions/22309036/change-the-value-of-input-fields-according-to-what-is-selected-from-the-dropdow
// var select = document.getElementById('zipcodesDropDown')[0];
// select.addEventListener('change', function () {
    // var texts = document.getElementsById('zip_new');
     // texts[i].value = select.value;
// });

//notes are same as above
//  
	
function populateZipcodesDLL(listZips){
	var ddlZipMenu = document.getElementById("zipcodesDropDown");
	for (var i=0; i<listZips.length; i++){
		var city = listZips[i];
		var element = document.createElement("option");
		element.textContent = city;
		element.value = city;
		ddlZipMenu.appendChild(element);
	}
}