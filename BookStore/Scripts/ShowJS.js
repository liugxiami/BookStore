$(function () {
    var item_to_delete;
    $('.deleteItem').click(function (e) {
        e.preventDefault;
        item_to_delete = $(this).data('id');
    });
    $("#btnContinueDelete").click(function () {
        $.get('/Book/DeleteBook', { id: item_to_delete }, function () {  //ajax sent 'bookId' to '/Book/DeleteBook'
            $("#deleteConfirmationModel").modal('hide');
            window.location = '/Book/Show';  //refresh website
        });
    });

    var books = $(".unitsinstock");
    books.each(function () {
        units = parseInt($(this).text());
        if (units > 80) {
            $(this).parent().addClass("sucess");
        } else if (units > 40) {
            $(this).parent().addClass("info");
        } else if (units > 20) {
            $(this).parent().addClass("warning");
        } else {
            $(this).parent().addClass("danger")
        }
    });

});//not working