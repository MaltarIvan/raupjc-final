
$(window).on("resize", function () {
    var height = $(window).innerHeight() * 0.85 - $("#footer").outerHeight() - $("#navigation_bar").outerHeight() - $("#header").outerHeight();
    $(".cont").height(height);
});

$(window).trigger('resize');

$(".cont").niceScroll();