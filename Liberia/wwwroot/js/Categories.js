$(document).ready(function () {

    //StartDataTable//
    $('#DataTable').DataTable();
    //EndDataTable//

    //$('.js-toggle-delete').click(function ()
    $('body').delegate('.js-toggle-delete', 'click', function () {

        let Category = $(this);
        let Categoryid = $(this).data('id');

        bootbox.confirm({
            title: 'Delete Category',
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
                        url: "/Categories/ToggleStatus",
                        data: {
                            id: Categoryid,
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        type: "POST",
                        success: function (ModifiedOn) {
                            var status = Category.parents('tr').find('.js-toggle-badge');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('text-bg-success text-bg-danger');

                            Category.parents('tr').addClass("animate__animated ");
                            Category.parents('tr').toggleClass("animate__shakeX");

                            Category.parents('tr').find('.js-modified-on').html(ModifiedOn);
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