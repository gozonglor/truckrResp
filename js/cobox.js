<!--broken-->
<!DOCTYPE HTML>
<html>
    <head>
        <script src="cordova.js"></script>
        
		<script type="text/javascript">
		//initialize DB
		var db = openDatabase('mydb', '1.0', 'Test DB', 2 * 1024 * 1024);
		var msg; 
		
		//executing setup queries--create DEMO table and insert two test values
		db.transaction(function (tx) {
			tx.executeSql('CREATE TABLE IF NOT EXISTS DEMO (id unique, log)');
			tx.executeSql('INSERT INTO DEMO (id, log) VALUES (1, "foobar")');
			tx.executeSql('INSERT INTO DEMO (id, log) VALUES (2, "logmsg")');
			msg = '<p>Log message created and row inserted.</p>';
			document.querySelector('#status').innerHTML =  msg;
		});

		//executing new query--
		db.transaction(function (tx) {
			tx.executeSql('SELECT * FROM DEMO', [], function (tx, results) {
				var len = results.rows.length, i;
				msg = "<p>Found rows: " + len + "</p>";
				document.querySelector('#status').innerHTML +=  msg;
				for (i = 0; i < len; i++){
					 msg = "<p><b>" + results.rows.item(i).log +"</b></p>";
					 document.querySelector('#status').innerHTML +=  msg;
				}
			}, null);
		});
        </script>
    
	</head>
    
	<body>
        <div id="status" name="status">Status Message</div>
    </body>

</html> 