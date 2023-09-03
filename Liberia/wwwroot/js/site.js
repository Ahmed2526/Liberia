var updatedRow;

$(document).ready(function () {
    

    //Generic modal//
    $('body').delegate('.js-modal', 'click', function () {
        var modal = $('#Modal');
        var title = $(this).data('title');
        var btn = $(this);
        $('#ModalLabel').text(title);

        //For Catching The Edit Operation
        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }

        //ajax request to c# controller to get the form
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
function onModalBegin() {
    //Disable button and add loading animation.
    $("body :submit").attr('disabled', 'true').attr("data-kt-indicator","on");
}

function onModalSuccess(item) {
    showSuccessMessage();
    $('#Modal').modal('hide');

    //Check if Add or Edit and handle it
    if (updatedRow === undefined) {
        $('tbody').append(item);
    }
    else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }

    //For Re render the newly added item
    KTMenu.init();
    KTMenu.initGlobalHandlers();
}

function onModalfailure() {
    showErrorMessage();
}

function onModalFinish() {
    //bring back save button to normal behaviour.
    $("body :submit").removeAttr('disabled').removeAttr("data-kt-indicator");
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

