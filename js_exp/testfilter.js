$(function(){
    $("#filter_button").click(function(){
	var NewPerson = { };

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
//var DTO =   { 'ClientObj' : NewPerson };
    $.ajax({
        type: "POST",
        url: "http://localhost:49237/api/filter",
		contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(NewPerson),
        success: function (json) {
            alert("it's working");
		    result = json;
		    console.dir(json);//displays the list of properties in our json object NewPerson
            //cannot call it from here because ajax does not return the value until after ajax call completes
		    //how to display the list of clients?
		    buildHtmlTable(json);

		},
        error: function(e) {
            console.log(e.message);
        }

    });
    });
    typeof json;
});



function buildHtmlTable(myList) {
     var columns = addAllColumnHeaders(myList);
 
     for (var i = 0 ; i <= myList.length ; i++) {
         var row$ = $('<tr/>');
         for (var colIndex = 0 ; colIndex < columns.length ; colIndex++) {
             var cellValue = myList[i][columns[colIndex]];
 
             if (cellValue == null) { cellValue = ""; }
			 var radioButtons = $('table#clientTable input[type="radio"]');

             row$.append($('<td/>').html(cellValue));

         }
         $("#clientTable").append(row$);
     }
 }
 
  // Adds a header row to the table and returns the set of columns.
 // Need to do union of keys from all records as some records may not contain
 // all records
 function addAllColumnHeaders(myList)
 {
     var columnSet = [];
     var headerTr$ = $('<tr/>');
 
     for (var i = 0 ; i <= myList.length ; i++) {
         var rowHash = myList[i];
         for (var key in rowHash) {
             if ($.inArray(key, columnSet) == -1){
                 columnSet.push(key);
                 headerTr$.append($('<th/>').html(key));
             }
         }
     }
     $("#clientTable").append(headerTr$);
 
     return columnSet;
 }