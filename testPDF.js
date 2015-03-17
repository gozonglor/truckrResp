$(function () { 	
	 $("#make_pdf").click(function (e) {
		//take screenshot of the canvas
		alert("Yeah bruh");
		//demoFromHTML();
		
		//instantiate a variable to hold our base 64 string
		var img = "";
		
		//http://stackoverflow.com/questions/14595541/capture-div-into-image-using-html2canvas
		//generate the base 64 string using html2canvas
		html2canvas($('#actual_data'), {
			onrendered: function(canvas) {
				img = canvas.toDataURL()
				//window.open(img);
				//alert(img);
				//document.getElementById("holder").innerHTML = img;
						//demoFromHTML(img);
						//FUCK it the pdf base 64 keeps getting corrupted. ok now lets see if we can just send an IMAGE >:(
						other(img);

			}
		});
		
		//put it on a pdf
		demoFromHTML(img);
		alert("HBASD");
		
	});
});

function other(pdfBase64){

		//take the base 64 string....
		//pdfBase64 = pdfBase64.split("$");
		//pdfBase64 = pdfBase64[1];
		var TransferForm = {"dateFilled":"1/1/11","pdf64":"","donorSig":"1opu21","lotNum":12345,"naidChoice":"yesNaid","donorID":1,"username":"Gozong","donationDescription":"2 dell laptops, 4 dell desktops","org":"Somewhere","formatted":""};


		TransferForm.formatted = "Thank you for calling PCs for People to safely and legally dispose of your used hardware. Attached is a screen shot of the transfer of ownership form.";
		
		pdfBase64 = pdfBase64.split(',');
		pdfBase64 = pdfBase64[1];
		TransferForm.pdf64 = pdfBase64;

		//doc.text(20,20, TransferForm.formatted);
		
		
		var success=false;
	

			var response =$.ajax({
				type: "POST",
				url: "http://localhost:49234/api/submit",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				async: false,
				data: JSON.stringify(TransferForm),
				//Authorization: Basic ekey,
				 
				success: function (response) {
				
					if (response == true){				
						alert("Transfer Form successfully added to database. Email sent.");
						//And send email somehow simultaneously. I suppose you can try calling your sendemail class in this controller. 
					}
					else{
						alert("AJAX request returned an error. Wrong format detected. Error: "+response);
					}
				},
				
				error: function(){
					sucess=false;
					document.getElementById("error_message_transfer").innerHTML = "No connection to API/Invalid encryption key.";
				}
			}).responseText;
			e.preventDefault();
		}


function demoFromHTML(base64){
         var doc = new jsPDF('p', 'in', [8,12]); //say 3 5 instead of 'letter'

           var source = $('#actual_data')[0];//originally:$('#transfer_module').first()
         var specialElementHandlers = {
             '#bypassme': function(element, renderer) {
                 return true;
             }
         };

         doc.fromHTML(
            $('#actual_data').get(0), // [Refer Exact code tutorial][2]HTML string or DOM elem ref.
             0.5,    // x coord, originally 0.5
             1,    // y coord, originally 0.5
             {
                 'width': 6.5, // max width of content on PDF
                 'elementHandlers': specialElementHandlers
             });
        
		doc.addImage(base64, 'PNG', 0, 0);

		var holder = doc.output('dataurlstring');
		
		alert(holder);
		
		document.getElementById('holder').innerHTML= holder.trim();
		 
		//document.getElementById('holder').innerText = "Holder$"+holder;
		//window.open(holder);
		
		//other(document.getElementById('holder').innerText);

}



