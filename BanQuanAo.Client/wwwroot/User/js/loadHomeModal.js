// Modal Minicart
$(document).ready(function () {

    $('.minicart-btn').click(function (e) {
        e.preventDefault();

        var $modal = $('#miniCartModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});