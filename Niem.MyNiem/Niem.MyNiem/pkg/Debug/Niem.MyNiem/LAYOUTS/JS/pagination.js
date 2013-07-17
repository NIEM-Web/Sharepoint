// JavaScript Document
$(document).ready(function () {
    var rows = $('#tblDiscussions > tbody > tr').length;
    var no_rec_per_page = 5;
    var no_pages = Math.ceil(rows / no_rec_per_page);
    var $pagenumbers = $('<div id="pages"></div>');
    for (i = 0; i < no_pages; i++) {
        if (i == 0)
            $('<span class="PageNumbers CurrentPage" style="cursor: hand;">' + (i + 1) + '</span>&nbsp;&nbsp;').appendTo($pagenumbers);
        else
            $('<span class="PageNumbers" style="cursor: hand;">' + (i + 1) + '</span>&nbsp;&nbsp;').appendTo($pagenumbers);
    }
    $pagenumbers.insertAfter('#tblDiscussions');
    $('.PageNumbers').hover(
function () {
    $(this).addClass('hover');
},
function () {
    $(this).removeClass('hover');
}
);
    $('#tblDiscussions > tbody > tr').hide();
    var tr = $('#tblDiscussions > tbody > tr');
    for (var i = 0; i <= no_rec_per_page - 1; i++) {
        $(tr[i]).show();
    }
    $('span').click(function (event) {
        $('#tblDiscussions > tbody > tr').hide();
        $('span.CurrentPage').removeClass('CurrentPage');
        $(this).addClass("CurrentPage");
        for (i = ($(this).text() - 1) * no_rec_per_page; i <= $(this).text() * no_rec_per_page - 1; i++) {
            $(tr[i]).show();
        }
    });
});