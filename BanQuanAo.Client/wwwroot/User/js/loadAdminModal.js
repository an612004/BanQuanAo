// Modal Crete Size
$(document).ready(function () {

    $('.btn-create-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#createSizeModal');
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

// Modal Edit Size
$(document).ready(function () {

    $('.btn-edit-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#editSizeModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Sizes/Edit/' + id,
            success: function (size) {
                $('#editSizeModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Size
$(document).ready(function () {

    $('.btn-delete-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteSizeModal');
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

// Modal Crete Color
$(document).ready(function () {

    $('.btn-create-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#createColorModal');
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

// Modal Edit Color
$(document).ready(function () {

    $('.btn-edit-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#editColorModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Color
$(document).ready(function () {

    $('.btn-delete-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteColorModal');
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

// Modal Edit Order
$(document).ready(function () {

    $('.btn-edit-order').click(function (e) {
        e.preventDefault();

        var $modal = $('#editOrderModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Orders/Edit/' + id,
            success: function (color) {
                $('#editOrderModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details Order
$(document).ready(function () {

    $('.btn-details-order').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsOrderModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'OrderDetails/Details/' + id,
            success: function (order) {
                $('#detailsOrderModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});



// Modal Crete Discount
$(document).ready(function () {

    $('.btn-create-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#createDiscountModal');
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

// Modal Edit Discount
$(document).ready(function () {

    $('.btn-edit-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#editDiscountModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details Discount
$(document).ready(function () {

    $('.btn-details-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsDiscountModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Discount/Details/' + id,
            success: function (order) {
                $('#detailsDiscountModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

/* Modal Delete Discount */
$(document).ready(function () {

    $('.btn-delete-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteDiscountModal');
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


// Modal Edit UserMem
$(document).ready(function () {

    $('.btn-edit-usermem').click(function (e) {
        e.preventDefault();

        var $modal = $('#editUserMemModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details UserMem
$(document).ready(function () {

    $('.btn-details-usermem').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsUserMemModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'UserMem/Details/' + id,
            success: function (order) {
                $('#detailsUserMemModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});


// Modal Create User
$(document).ready(function () {

    $('.btn-create-user').click(function (e) {
        e.preventDefault();

        var $modal = $('#createUserModal');
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

// Modal Edit User
$(document).ready(function () {

    $('.btn-edit-user').click(function (e) {
        e.preventDefault();

        var $modal = $('#editUserModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details User
$(document).ready(function () {

    $('.btn-details-user').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsUserModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'User/Details/' + id,
            success: function (order) {
                $('#detailsUserModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Product
$(document).ready(function () {

    $('.btn-delete-product').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteProductModal');
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


// Modal Details Product
$(document).ready(function () {

    $('.btn-details-product').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsProductModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'User/Details/' + id,
            success: function (order) {
                $('#detailsProductModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Edit Size
$(document).ready(function () {

    $('.btn-gift-dob').click(function (e) {
        e.preventDefault();

        var $modal = $('#GiftDoBModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        $.ajax({
            type: 'GET',
            url: 'Admin/GiftDoB'
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});