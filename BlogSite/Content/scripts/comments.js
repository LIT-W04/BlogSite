$(function() {
    $("#commenter-name, #content").on('keyup', function () {
        $("#submit-button").prop('disabled', !$("#commenter-name").val() || !$("#content").val());
    });
});