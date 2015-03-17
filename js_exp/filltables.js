var myList = [
	{
		"organization": "Macalester College Bike Shop",
		"address": "1600 Grand Avenue",
		"zip": "55105",
		"city": "Saint Paul",
		"state": "MN",
		"firstName":"Hipster",
		"lastName":"Elephant Heart",
		"phone":"123-456-7890",
		"email":"helpp@hotmail.com",
		"notes":"I am hungry."
	},
	{
		"organization": "Survey Corps",
		"address": "Little Cottage behind Wall Maria",
		"zip": "666666",
		"city": "Post-Apocalyptic World",
		"state": "Earth",
		"firstName":"Eren",
		"lastName":"Jaeger",
		"phone":"0",
		"email":"titansl4y3r@nomoretitans.com",
		"notes":"I'm gonna be the very best."
	},
	{
		"organization": "Team Rocket",
		"address": "",
		"zip": "",
		"city": "",
		"state": "",
		"firstName":"Grunt",
		"lastName":"Felix",
		"phone":"0",
		"email":"pokemonfan@teamr.org",
		"notes":"Fufufufufufu...."
	}
]

// Builds the HTML Table out of myList.
// Builds the HTML Table out of myList json data from Ivy restful service.

//This function would just take in a list into the method.
//This method would be called in the section of the code that calls our API.
//If the client post is successful, creates a list and sends it to this method.

 function buildHtmlTable(myList) {
     var columns = addAllColumnHeaders(myList);
 
     for (var i = 0 ; i < myList.length ; i++) {
         var row$ = $('<tr/>');
         for (var colIndex = 0 ; colIndex < columns.length ; colIndex++) {
             var cellValue = myList[i][columns[colIndex]];
 
             if (cellValue == null) { cellValue = ""; }
			 
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
 
     for (var i = 0 ; i < myList.length ; i++) {
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