/*
AJAX: It stands for asynchronous javascript and XMLDocument
A way to trasnfer data between a web page and a web serve without
having to do a full page refresh, using an HTTP GET or an HTTP POST.
No need for a full page refresh to send data back and forth between
the page and the server. More seamless.

JQUERY: Will make a request, allow JS to say, "hey send me some information"
and then you register and function that says when the data comes back, execute this function.
So then it allows the user of the application to continue working. once the data
comes back successfully, then things on the webpage can change because that function
will fire off and you can update the new web page with the new data from the server.
^that is asynchronous part

XML part: Slightly misleading. Instead of sending XML back and forth between web page and the server,
JSON is sent around. JSON is in javascript syntax and you can work with it more 
easily as a Javascript developer.

JSON: Javascript Object Notation


*/

$(function(){

	$('#filter_button').click(function(){
	
	
	  $.ajax({
       url: 'js/testdata.json',
       dataType: 'json',
       success: function(data) {
          var items = [];

          $.each(data, function(key, val) {

            items.push('<li id="' + key + '">' + val + '</li>');    

          });

          $('<ul/>', {
             'class': 'interest-list',
             html: items.join('')
          }).appendTo('#selected_clients');

       },
      statusCode: {
         404: function() {
           alert('There was a problem with the server.  Try again soon!');
         }
       }
    });
  });
	
	



});