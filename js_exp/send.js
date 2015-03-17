var body_message = $('#message').value;
var email = 'glor@macalester.edu'; //somehow grab the email of the current client.
var subject = 'js email';

$(function() {
    $("#accept_button").click(function (e) {
		alert("woival");
		
	var mailto_link = 'mailto:' + email + '?subject=' + subject + '&body=' + body_message;

    win = window.open(mailto_link, 'emailWindow');
    if (win && win.open && !win.closed) win.close();
	
	});
});


$(function () {
    $("#filter_button").click(function (e) {
        var NewPerson = {};
        var test = "please work";
        
		
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
            dataType: "json",
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