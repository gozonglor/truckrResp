
function magicEnter(){
//alert("asdiisahdj");

// if (document.getElementById("main") != null){
// alert("asdjkas");
			// document.getElementById('login_button').click(); 
			// alert("closed");
		
	// }
					
	// if (document.getElementById("filter_module").style.display != "none"){
	// alert("asdkjasdhakjsdhkasjdhakARHG! filter module is showing");
		// document.getElementById('filter_button').click(); 

	// }
	
	// else{
	
	if (document.getElementById("signature_module").style.display != "none"){
		// alert("sig module is shown");

		document.getElementById('submit_me').click(); 
	}
	if (document.getElementById("transfer_module").style.display != "none"){
		// alert("transfer module is shown");

		document.getElementById('accept_button').click(); 
	}
	if (document.getElementById("generate_module").style.display != "none"){
		// alert("genreate module is shown");

		document.getElementById('transfer_button').click(); 
	}
	// }
	// if (document.getElementById("new_client_module").style.display != "none"){
		// alert("new client module is shown");

		// document.getElementById('create_new_client_button').click();
	// }
	
					
	// if ($("filter_module").is(":visible")){
	// alert("filter module is shown");
		// $('filter_button').trigger('click'); 
	// }
	// if ($("signature_module").is(":visible")){
		// alert("sig module is shown");

		// $('submit_me').trigger('click'); 
	// }
	// if ($("transfer_module").is(":visible")){
		// alert("transfer module is shown");

		// $('submit_button').trigger('click'); 
	// }
	// if ($("generate_module").is(":visible")){
		// alert("genreate module is shown");

		// $('transfer_button').trigger('click'); 
	// }
	// if ($("new_client_module").is(":visible")){
		// alert("new client module is shown");

		// $('create_new_client_button').trigger('click'); 
	// }
	
	
}

document.onkeypress = function(e) {
// alert("you pressed a button and...");

    e = e || window.event;
    var charCode = e.which;
    if (charCode== "13") {
	// alert("hi");
		magicEnter();
    }
};