$(document).ready(function () {
    $('.js-modal').on('click', function () {
        var modal = $('#Modal');
        var title = $(this).data('title');
        var btn = $(this);
        $('#ModalLabel').text(title);

        $.ajax({
            type: 'GET',
            url: btn.data('url'),
            success: function (form) {
                $('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            }
        });
        modal.modal('show');

    });
});
function onModalSuccess() {
    $('#Modal').modal('hide');
    showSuccessMessage();
}
function showErrorMessage() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Something went wrong!',
    })
}
function showSuccessMessage() {
    Swal.fire(
        'Done!',
        'Saved Successfully!',
        'success'
    )
}

