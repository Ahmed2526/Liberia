$(document).ready(function () {

    //Delete Action
$('body').delegate('.js-toggle-delete', 'click', function () {

    let Author = $(this);
    let Authorid = $(this).data('id');

    bootbox.confirm({
        title: 'Delete Copy',
        message: 'Are You Sure You Want To do This ?',
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-danger'
            },
            cancel: {
                label: 'No',
                className: 'btn-secondary'
            }
        },
        callback: function (result) {

            if (result) {
                $.ajax({
                    url: "/BooksCopies/ToggleStatus",
                    data: {
                        id: Authorid,
                        '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                    },
                    type: "POST",
                    success: function () {
                        var status = Author.parents('tr').find('.js-toggle-badge');
                        var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                        status.text(newStatus).toggleClass('badge-light-success badge-light-danger');

                        Author.parents('tr').addClass("animate__animated ");
                        Author.parents('tr').toggleClass("animate__shakeX");

                    },
                    error: function () {
                        showErrorMessage();
                    }
                });
            }
        }
    });
});

});