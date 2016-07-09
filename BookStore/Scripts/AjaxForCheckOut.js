$(function () {
    var unitCost = $("#bookPrice");
    var orderUnits = $("#quantity");
    var total = $("#amount");
    var sendData = { price: unitCost.val(), quatity: orderUnits.val() }
    $.get('/CheckOut/AddToCart', sendData).success(function (responseData) {
        total.val(responseData);
    }).error(function () {
        alert("Error!")
    })
})//end of function