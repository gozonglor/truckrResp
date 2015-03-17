
//validateForm -- a function that checks to make sure the user has filled out 
//user name and password fields using simple if __ or ____ logic.
//returns false if either password of userID filed is empty/null.
function validateForm() {
    var x = document.forms["login_form"]["userID"].value;
	var y = document.forms["login_form"]["password"].value;

    if ((x==null || x=="") || (y==null || y=="")) {
        alert("Please fill out all fields.");
        return false;
    }
}

$(function () { 
    $("#login_button").click(function (e) {
		var validation = validateForm();
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
    //typeof json;
    //buildHtmlTable(json);
	//return success
});

function allowLogin(data) {
	//var data2;
	//for (var i = 0; i <= data.length)
    window.location="http://localhost:49234/clients.html";

}


