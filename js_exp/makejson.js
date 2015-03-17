//var o = {"organization":"",
//		"address":"",
//		"zip":"",
//		"city":"",
//		"state":"",
//		"firstName":"",
//		"lastName":"",
//		"phone":"",
//		"email":"",
//		"id":"",
//		"notes":""}

$.fn.serializeObject = function()
{
    var o = {};
    var a = this.serializeArray();
    $.each(a, function() {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$(function() {
    $('form').submit(function() {
        $('#result').text(JSON.stringify($('form').serializeObject()));
        return false;
    });
});

$.ajax({
    type: "POST",
    contentType: "application/json; charset=utf-8",
    url: "http://localhost:49237/api/filter",
    data: {request:$.toJSON(o)},
    dataType: "json",
    success: function(response){
        alert(response);
    }
})