function createDocument(naidOption){
	if (naidOption ==yesNaid){
	$("#answer").load("verbage.txt #yesNaid");
	}
	
	else{
	$("#answer").load("verbage.txt #noNaid");

	}
}
