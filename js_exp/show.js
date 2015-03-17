	/*********************************************************************************/
	/* showing the hidden filter table                                               */
	/*********************************************************************************/

$(document).ready(function(){
	$("#filter_button").click(function(){
		$("#selected_clients").show();
	});
});




//	/*********************************************************************************/
//	/* logic for the filter table                                                    */
//	/*********************************************************************************/
//    var table = $("#clientsDataTable");
//    $.each(data, function(idx, elem){
//        table.append("<tr><td>"+elem.username+"</td><td>"+elem.name+"</td>   <td>"+elem.lastname+"</td></tr>");
//    });
