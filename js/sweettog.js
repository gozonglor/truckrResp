//http://stackoverflow.com/questions/9778888/uncaught-typeerror-cannot-set-property-onclick-of-null
window.onload = function(){ 

	// document.getElementById("offline_button").onclick = function(){
		// swal("Yet another test.", "Something went wrong!", "error");
	// };
	


};

	function findConnection(){
	var response = testConnect();
	
	if (response == true) {
		window.location = "index.html";
		//TODO: Where I will have to build the capabilities on the app to recognize there is a TOO form in local storage and give user the option to tie it to a client.
		
	}
	else{
		// alert("No internet connection!");
		swal("No internet connection.", "Try again later when there's internet access.", "error");
	}
}

//http://stackoverflow.com/questions/26838965/sweet-alert-display-html-code-in-text
function signTest(){
swal({html:true,
title: "testing sig",
text: "<link rel='stylesheet' type='text/css' media='all' href='css/jquery.signaturepad.css'/><form action='' class='sigPad' id='sigPad' onsubmit='return false'><canvas class='pad' width='345px' height='500px' id='canvas_pad' style='background:#eeeeee;' align='overlay'></canvas><input type='hidden' name='output' class='output' id='output'></form><script src='js/jquery.signaturepad.js'></script><script>$(document).ready(function() {$('.sigPad').signaturePad();});</script>"
});

}
