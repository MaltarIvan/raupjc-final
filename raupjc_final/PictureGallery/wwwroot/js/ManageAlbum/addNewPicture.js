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
        $('#imgPrev').attr('src', src = "/Content/default-picture.svg");

    }
}