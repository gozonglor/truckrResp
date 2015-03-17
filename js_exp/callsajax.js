function compileRelationship(){
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
		NewPerson.contact_relationship = compileRelationship();
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