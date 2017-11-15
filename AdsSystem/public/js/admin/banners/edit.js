function typeUpdate() {
    $('.type').hide();
    $('.type-' + $('select[name="Type"]').val()).show();
}

$(document).ready(function () {
    typeUpdate();
    $('[name="Type"]').change(typeUpdate);
});