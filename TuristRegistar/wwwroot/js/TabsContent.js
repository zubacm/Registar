
$.get('@Url.Action("_ChangePassword", "Auth")', function (content) {
    $("#nav-password").html(content);
});
$.get('@Url.Action("_UpdateProfile", "Auth")', function (content) {
    $("#nav-profile").html(content);
});
$.get('@Url.Action("_UpdateUser", "Auth")', function (content) {
    $("#nav-user").html(content);
});