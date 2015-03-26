$(function () { 
    $("#login_button").click(function (e) {
		var validation = validateFormLogin();
		var NewPerson = {};    

        NewPerson.username = $("#userID").val();
		pass = (CryptoJS.SHA1($("#password").val()));
		newpass = (pass.toString(CryptoJS.enc.Hex)).toUpperCase();
		NewPerson.pw = newpass;  
		
		var success=false;
       
		if (validation === false){
				alert("Please fill out all fields");				
		}
		
		else{
			var response =$.ajax({
				type: "POST",
				url: "http://truckrtest.pcscrm.com/api/confirm",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				async: false,
				data: JSON.stringify(NewPerson),
				 
				success: function (response) {
					if (response !== ""){ //originally, if response == true
						currentUser = NewPerson.username;
						document.getElementById("error_message_login").innerHTML = 'Logging in...';
						//setTimeout(allowLogin, 3000);
						//allowLogin();
						ekey = response;	
						allowLogin();
						
					}
					else{
						document.getElementById("error_message_login").innerHTML = "Wrong password or username.";
						alert("Wrong password or username.");				

					}
				},
				
				error: function(){
					sucess=false;
					document.getElementById("error_message_login").innerHTML = "No connection to API/Invalid encryption key.";
					alert("No connection to API/Invalid encryption key.");				

				}
			}).responseText;
		}
		e.preventDefault();
		setTimeout(resetError, 3000);
		//http://stackoverflow.com/questions/20890943/javascript-settimeout-not-working
    });
});