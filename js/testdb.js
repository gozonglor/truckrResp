// document.getElementById("webSqlSubmitBtn").addEventListener("click", insertSql);
// document.getElementById("webStorageSubmitBtn").addEventListener("click", insertWeb);
// document.getElementById("refreshWeb").addEventListener("click", refreshWeb);



$(function() {                                       // <== Doc Ready
    $("#offline_donor_name1").change(function() {                  // When the email is changed
        $('#offline_donor_name2').val(this.value);                  // copy it over to the mail
    });
});


var db =  openDatabase('testlocal3', '1.0', 'My First Web Database', 2 * 1024 *1024);
	//run a transaction
	db.transaction(function(tx) { 
			//execute a query to create a new table called "user" with two fields (id and name)
			tx.executeSql("CREATE TABLE IF NOT EXISTS tooForms (id INTEGER PRIMARY KEY ASC, profileJson STRING UNIQUE)");
			//tx.executeSql("INSERT INTO people (id, name) VALUES (5, 'team 5');");
		},error_log);
		
		//execute other transaction to get users
		db.transaction(function(tx) { 
			//execute a query to create a new table called "user" with two fields (id and name)
			tx.executeSql("SELECT * FROM tooForms ", [], function(tx, results){
				var n_rows = results.rows.length;//number of results
				var rows = null;
				console.log("Number of too forms: "+n_rows);
				for (var i = 0; i < n_rows; i++){
					row  = results.rows.item(i);
					console.log("Too forms to sync: "+row.id+" - "+row.profileJson);
				}
			});
		},error_log);
		
		// function to show all errors that can occur during execution
		function error_log(error){
			console.log(error.message);
			alert(error.message);
		}


function insertWeb() {
	var name = document.getElementById("name").value;
	var profile =  {};
	profile.icecream = document.getElementById("icecream").value;
	profile.movie = document.getElementById("movie").value;
    localStorage.setItem("name", profile);
	alert("Finished storing "+name+"'s information in the HTML5's local storage object.");
}

//http://stackoverflow.com/questions/9144982/retrieve-all-data-from-localstorage-without-knowing-the-key-name
function refreshWeb(){
	var i = 0,
		oJson = {},
		sKey;
	for (; sKey = localStorage.key(i); i++) {
		oJson[sKey] = localStorage.getItem(sKey);
	}
	document.getElementById("dataWeb").innerHTML = document.getElementById("dataWeb").innerHTML+"<br>"+oJson;
}

//http://www.kirupa.com/html5/check_if_internet_connection_exists_in_javascript.htm
//https://subinsb.com/how-to-check-if-internet-connection-exists-in-jquery-javascript
function doesConnectionExist() {
	 var xhr = new XMLHttpRequest();
	 var file = "http://google.com/";
	 var r = Math.round(Math.random() * 10000);
	 xhr.open('HEAD', file + "?subins=" + r, false);
	 try {
	  xhr.send();
	  if (xhr.status >= 200 && xhr.status < 304) {
	   return true;
	  } else {
	   return false;
	  }
	 } catch (e) {
	  return false;
	 }

}


function testConnect(){

var pass = false;
  var response = $.ajax({
                type: "GET",
                url: "http://truckrtest.pcscrm.com/api/checkinternet/1",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,

                success: function(response) {
					pass = true;
                },

                error: function() {
					pass = false;
                },
            }).responseText;
 return pass;

}

function findConnection(){
	var response = testConnect();
	
	if (response == true) {
		window.location = "index.html";
		//TODO: Where I will have to build the capabilities on the app to recognize there is a TOO form in local storage and give user the option to tie it to a client.
		
	}
	else{
		alert("No internet connection!");
	}
}

function validateOffline(){
	if ((document.getElementById("offline_donor_name1").value == "") || (document.getElementById("offline_donation_description").value == "") || (typeof $('input[name=offline_naidOption]:checked').val() == 'undefined')){
		alert("Please fill out all fields.");
		return false;
	}
	else{
		return true;
	}
}

function acceptOffline(){
	if (validateOffline() == true){
	
//scrape all the data from the screen.
	
	//GOZONG LOR % is transferring ownership of electronic equipment to PCs for People. PCs for People will redistribute the refurbished computers to low income families and people with disabilities. This document fully transfers legal ownership from GOZONG LOR  to PCs for People, and along with that ownership all liability associated with disposal and use. PCs for People has a zero landfill policy and any non-functional parts will be locally recycled. No goods or services were exchanged in consideration of this contribution and PCs for People understands that no warranty is provided and all equipment is accepted as is.%By signing your name below you agree that you are a duly appointed and authorized representative of your company

	var sig_coord = document.getElementById("output").defaultValue;
	var imgBaseSig = "";
	if (((document.getElementById("offline_signers_name").value) == "") || ((document.getElementById("offline_signers_title").value) == "") || (sig_coord == "")) {
		alert("Please fill out all fields.");
		//document.getElementById("client_sign_wrapper").style.backgroundColor = "#ffff66";
		return;
	} else if (((document.getElementById("offline_signers_name").value) != "") && ((document.getElementById("offline_signers_title").value) != "") && (sig_coord != "")) {
		//$('.signed').show();
		//customer_signature = true;
		imgBaseSig = document.getElementById("canvas_pad").toDataURL();  //grab the data from canvas pad
		var newSig = imgBaseSig.split(",");
		imgBaseSig = newSig[1];
		alert("imgBaseSig. grab the data from canvas pad: "+imgBaseSig);

		//base64SigImg = imgBaseSig;
		
		//document.getElementById("client_sign_wrapper").style.backgroundColor = "#FFFFFF";

		//$('#signature_module').hide();
		//$('#transfer_module').show();
	} else {
		document.getElementById("sig_error_message").innerText = "Please sign.";
		alert("Please sign.");
	}
	
	
	
	
	//------------------
	
	
	var tooForm = {};

	var naidChoice = $('input[name=offline_naidOption]:checked').val();//yes or no naid
	var donationDescription = $("#donation_description").val();
	
	var fName = $("#offline_donor_name1").val()
	var firstParagraph = document.getElementById("firstPiece").innerText;
	var secondParagraph = document.getElementById("secondPiece").innerText;
	firstParagraph = firstParagraph+" "+fName+" "+secondParagraph;
	
	
	var dateObj = new Date();
	var month = dateObj.getUTCMonth() + 1; //months from 1-12
    var day = dateObj.getUTCDate();
    var year = dateObj.getUTCFullYear();
	var newdate = "" + month + "/" + day + "/" + year;
	var dateFilled = newdate;
	var formatted = "Thank you for calling PCs for People to safely and legally dispose of your used hardware. Attached is a screen shot of the transfer of ownership form.";
	var wordage = fName+"%"+firstParagraph+"%"+"By signing your name below you agree that you are a duly appointed and authorized representative of your company";
	var donationDescription = $("#offline_donation_description").val();
	var title = $("#offline_signers_title").val()
	var firstName = $("#offline_signers_name").val()
	var org = $("#offline_donor_name1").val()

	
	alert("Donation descriptiOn: "+donationDescription);
	
	tooForm.naidChoice = naidChoice;
	tooForm.donationDescription = donationDescription;
	tooForm.lotNum = 0;
	tooForm.donorID = 0;
	tooForm.username = "";
	tooForm.donorSig = imgBaseSig;
	tooForm.org = org;
	tooForm.dateFilled = dateFilled;
	tooForm.formatted = formatted;
	tooForm.wordage = wordage;
	tooForm.firstName = firstName;
	tooForm.title = title;
	tooForm.donorEmail = "";
	
	//localStorage.setItem("transfer form", tooForm);
	
		db.transaction(function(tx) { 
			//execute a query to create a new table called "user" with two fields (id and name)
			tx.executeSql("CREATE TABLE IF NOT EXISTS people (name STRING UNIQUE, profileJson TEXT(100))");
			//tx.executeSql("INSERT INTO people (name, profileJson) VALUES ("+name+", "+ "ham" + ");");
			tx.executeSql('INSERT INTO tooForms(profileJson) VALUES (?)',[JSON.stringify(tooForm)]);
			//tx.executeSql("INSERT INTO people (name, profileJson) VALUES ('Anna', 'team 5');");
		},error_log);

                        
alert("Stored in local storage. Remember to sync it when you have internet connection!");
}
}