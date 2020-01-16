//za upload files dugme
$(document).on("click", ".browse", function () {
    var file = $(this).parents().find(".file");
    file.trigger("click");
});
$('input[type="file"]').change(function (e) {
    var fileName = e.target.files[0].name;
    $("#file").val(fileName);

    var reader = new FileReader();
    reader.onload = function (e) {
        // get loaded data and render thumbnail.
        document.getElementById("preview").src = e.target.result;
    };
    // read the image file as a data URL.
    reader.readAsDataURL(this.files[0]);
});


$(document).ready(function () {

    $("#imageBrowes").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }

    });

});


var ReadImage = function (file) {

    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {

            var height = this.height;
            var width = this.width;
            var type = file.type;
            var size = ~~(file.size / 1024) + "KB";

            $("#targetImg").attr('src', _file.target.result);
            $("#description").text("" + size + ", " + height + "X " + width + ", " + type + "");
            $("#imgPreview").show();

        };

    };

};

var ClearPreview = function () {
    $("#imageBrowes").val('');
    $("#file").val('');
    $("#description").text('');
    $("#imgPreview").hide();

};

var Uploadimage = function () {

    var file = $("#imageBrowes").get(0).files;

    var data = new FormData;
    data.append("ImageFile", file[0]);
    data.append("ProductName", "SamsungA8");

    $.ajax({

        type: "Post",
        url: "/Object/ImageUploadTemp",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            ClearPreview();
            var myhtml = '<div class="mb-3 pics animation all" onmouseover="showTrash(this)" onmouseout="hideTrash(this)">'
                + '<img class="img-fluid" src="/Temp/' + response + '" alt="Forografija se ne može učitati">'
                + '<i class="fa fa-trash fa-7x text-dark trach-icon" onclick="deleteImg(this)"></i></div>';
            $('#gallery').append(myhtml);
            //$("#uploadedImage").append('<img src="/Temp/' + response + '" class="img-responsive thumbnail" style="object-fit: contain"/>');


        }
    });
};


function showTrash(e) {
    $(e).find('img').css("opacity", "0.4");
    $(e).find('i').css("display", "block");
}
function hideTrash(e) {
    $(e).find('img').css("opacity", "1");
    $(e).find('i').css("display", "none");
}
function deleteImg(e) {
    var container = $(e).parent();
    var filepath = $(e).parent().find('img').attr('src');
    var data = new FormData;
    data.append("DeleteImagePath", filepath);
    $.ajax({
        type: 'POST',
        url: "/Object/DeleteImageTemp",
        //contentType: "application/json",
        data: data,
        contentType: false,
        processData: false,
        success: function (data) {
            container.remove();
        },
        error: function () {
            alert('Greška prilikom brisanja fotografije.');
        }
    });
}
