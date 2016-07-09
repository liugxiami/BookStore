/// <reference path="jquery-2.2.3.min.js" />
$(function () {
    var tb1 = $("#tb1");
    var tb2 = $("#tb2");
    var tb3 = $("#tb3");

    function doCalculate(o) {
        var sendData = { x: tb1.val(), y: tb2.val(), opt: o };
        $.post('/Tool/Calculate', sendData).success(function (responseData) {
            tb3.val(responseData);
        }).error(function () {
            alert("opps!");
        });
    }

    $("#btnAdd").click(function () {
        doCalculate("add");
    });
    $("#btnSub").click(function () {
        doCalculate("sub");
    });
    $("#btnMul").click(function () {
        doCalculate("mul");
    });
    $("#btnDiv").click(function () {
        doCalculate("div");
    });
})