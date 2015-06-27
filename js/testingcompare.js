//initialize the db
var db = openDatabase('CBDB', '1.0','mySpecialDatabaseThatWontWork',10*1024*1024);

//why are you dropping the table before it exists?sas
db.transaction(function (tx){
	tx.executeSql('DROP TABLE IF EXISTS cb'); //which table? any/all tables in the cb???
	alert("dropped table"); //
	createDB(); //initialize the database
	queryDB();
},
function (tx, error) {
	// error
	alert('0.Something went wrong: '+ error.message);
});

function createDB(){
db.transaction(function (tx) {       
    tx.executeSql('CREATE TABLE IF NOT EXISTS cb (id unique, text)');
    tx.executeSql('INSERT INTO cb (id, text) VALUES (1, "myTest")');
    tx.executeSql('INSERT INTO cb (id, text) VALUES (2, "another")');
    tx.executeSql('INSERT INTO cb (id, text) VALUES (3, "andYetAnother")') //creating a table for the fake data
    tx.executeSql('INSERT INTO cb (id, text) VALUES (4, "ohAndAgain")');
    alert("DB success");  
    },
    function (tx, error) {
        // error
        alert('1.Something went wrong: '+ error.message);
    });
} 

function queryDB(){
db.transaction(function (tx) {
    tx.executeSql('SELECT * FROM cb',[], function (tx, results) {
        var rows = results.rows;
        alert(rows.length);
        for (var index = 0; index < rows.length; index++) {
            var x = rows.item(index);
            alert(x.text);
        } 
    },
    function (tx, error) {
    // error
    alert('2.Something went wrong: '+ error.message);
    });
});
}