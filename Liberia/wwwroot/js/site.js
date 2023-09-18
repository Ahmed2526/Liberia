var updatedRow;

$(document).ready(function () {

    //Disable button and add loading animation.
    $('form').not('#SignOut').on('submit', function () {
        if ($('#kt_docs_tinymce_basic').length > 0) {
            $('#kt_docs_tinymce_basic').each(function () {
                var input = $(this);
                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);
            });
        }
        var isvalid = $(this).valid();
        if (isvalid) disableSubmitButton();
    });
    // End Disable button.

    //Toggle Password Show/Hide  //Not Working
    let togglePassword = document.querySelector("#togglePassword");
    let password = document.querySelector("#password");
    if ($('#togglePassword').length > 0) {
        togglePassword.addEventListener("click", function () {
            // toggle the type attribute
            const type = password.getAttribute("type") === "password" ? "text" : "password";
            password.setAttribute("type", type);
            // toggle the eye icon
            this.classList.toggle('fa-eye');
            this.classList.toggle('fa-eye-slash');
        });
    }
    //End Password Show/Hide

    //TinyMce Start
    if ($('#kt_docs_tinymce_basic').length > 0) {
        var options = { selector: "#kt_docs_tinymce_basic", height: "265" };
        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }
        tinymce.init(options);
    }
    //TincyMce End

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
                applySelect2();
            },
            error: function () {
                showErrorMessage();
            }
        });
        modal.modal('show');
    });

    $('.js-signout').on('click', function () {
        $('#SignOut').submit();
    });
});
//Disable button and add loading animation.
function disableSubmitButton() {
    $("body :submit").attr('disabled', 'true').attr("data-kt-indicator", "on");
}
function onModalBegin() {
    disableSubmitButton();
}

function onModalSuccess(item) {
    showSuccessMessage();
    $('#Modal').modal('hide');

    //Check if Add or Edit and handle it
    if (updatedRow === undefined) {
        $('tbody').prepend(item);
    }
    else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }

    //For Re render the newly added item
    KTMenu.init();
    //KTMenu.initGlobalHandlers();
}

function onModalfailure(obj) {
    showErrorMessage(obj.responseText);
}

function onModalFinish() {
    //bring back save button to normal behaviour.
    $("body :submit").removeAttr('disabled').removeAttr("data-kt-indicator");
}



function applySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        $('form').not('#SignOut').validate().element('#' + $(this).attr('id'));
    });
}

function showErrorMessage(data) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: data == undefined ? 'Something went wrong!' : data,
    })
}
function showSuccessMessage(data) {
    Swal.fire(
        'Done!',
        data == undefined ? 'Saved Successfully!' : data,
        'success'
    )
}


