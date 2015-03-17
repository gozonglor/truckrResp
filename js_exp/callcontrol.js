  function AddToCart(id)
  {
    $.ajax({
      url: 'http://localhost:64816/api/filter/',
	  type: "POST",
      data: { id: id },
      success: function(){
        alert('Added');      
      }
    });
  }
  
  function sendData() {
    $.ajax({
        url: '/api/filter/',
        type: 'POST',
        data: { json: JSON.stringify({
            name:"Bob",
            ...
        })},
        dataType: 'json'
    });
}