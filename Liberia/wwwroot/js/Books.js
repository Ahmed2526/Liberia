$(document).ready(function () {
    //StartDataTable//
    $('#DataTable').DataTable();
    $('body').delegate('.js-toggle-delete', 'click', function () {

        let Book = $(this);
        let Bookid = $(this).data('id');

        bootbox.confirm({
            title: 'Delete Book',
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
                        url: "/Books/ToggleStatus",
                        data: {
                            id: Bookid,
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        type: "POST",
                        success: function (ModifiedOn) {
                            var status = Book.parents('tr').find('.js-toggle-badge');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('text-bg-success text-bg-danger');

                            Book.parents('tr').addClass("animate__animated ");
                            Book.parents('tr').toggleClass("animate__shakeX");

                            Book.parents('tr').find('.js-modified-on').html(ModifiedOn);
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
