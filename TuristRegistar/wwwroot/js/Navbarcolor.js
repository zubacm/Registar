$(window).scroll(function () {
    var scroll = $(window).scrollTop();
    if (scroll < 300) {
        //rgba(15, 29, 95, 0.8)
        $('.fixed-top').css('background', 'rgba(15,29,95, 0.8)');
    } else {
        $('.fixed-top').css('background', 'rgba(15,29,95, 0.8)');
    }
});

$(document).ready(function () {

    $('.fixed-top').css('background', 'rgba(15,29,95, 0.8)');

});
