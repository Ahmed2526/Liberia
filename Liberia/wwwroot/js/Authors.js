$(document).ready(function () {

    //StartDataTable//
    $('#DataTable').DataTable();

    $('body').delegate('.js-toggle-delete', 'click', function () {

        let Author = $(this);
        let Authorid = $(this).data('id');

        bootbox.confirm({
            title: 'Delete Author',
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
                        url: "/Authors/ToggleStatus",
                        data: {
                            id: Authorid,
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        type: "POST",
                        success: function (ModifiedOn) {
                            var status = Author.parents('tr').find('.js-toggle-badge');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('text-bg-success text-bg-danger');

                            Author.parents('tr').addClass("animate__animated ");
                            Author.parents('tr').toggleClass("animate__shakeX");

                            Author.parents('tr').find('.js-modified-on').html(ModifiedOn);
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
