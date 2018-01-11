$(window).on("resize", function () {
    var height = $(window).innerHeight() * 0.80 - $("#footer").outerHeight() - $("#navigation_bar").outerHeight() - $("#header").outerHeight();
    $(".album-section").height(height);
});

$(window).trigger('resize');

$(".album-section").niceScroll({
    cursorwidth: 5,
    cursoropacitymin: 0.4,
    cursorcolor: '#9E9E9E',
    cursorborder: 'none',
});