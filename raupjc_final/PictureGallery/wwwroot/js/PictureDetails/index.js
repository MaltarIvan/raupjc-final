$("#like-button-div").on("mouseenter", function () {
    $("#users-liked-div").show();
    $("#users-disliked-div").hide();
});

$("#dislike-button-div").on("mouseenter", function () {
    $("#users-disliked-div").show();
    $("#users-liked-div").hide();
});

$("#hide-button-like").click(function() {
    $("#users-liked-div").hide();
});

$("#hide-button-dislike").click(function () {
    $("#users-disliked-div").hide();
});