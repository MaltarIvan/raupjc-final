$(window).on("resize", function () {
    var height = $(window).innerHeight() * 0.80 - $("#footer").outerHeight() - $("#navigation_bar").outerHeight() - $("#header").outerHeight();
    $(".cont").height(height);
});

$(window).trigger('resize');

$(".cont").niceScroll({
    cursorwidth: 5,
    cursoropacitymin: 0.4,
    cursorcolor: '#9E9E9E',
    cursorborder: 'none',
});
function ShowPreview(input) {
    var fileName = $("#file-upload").val();
    if (!fileName.trim()) {
        fileName = "No file chosen.";
    }
    $("#file-name").html(fileName);
    if (input.files && input.files[0]) {
        var ImageDir = new FileReader();
        ImageDir.onload = function (e) {
            $('#imgPrev').attr('src', e.target.result);
        };
        ImageDir.readAsDataURL(input.files[0]);
    } else {
        $('#imgPrev').attr('src', src = "/Content/default-profile-picture.png");

    }
}