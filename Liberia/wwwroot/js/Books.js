$(document).ready(function () {

    //Start ServerSide DataTable
    var datatable = $('#BooksTable').DataTable({
        serverSide: true,
        processing: true,
        //stateSave: true,
        ajax: {
            url: '/Books/GetBooks',
            type: 'POST'
        },
        'drawCallback': function () {
            KTMenu.createInstances();
        },
        order: [[1, 'asc']],
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: [
            {
                "data": "id", "name": "Id"
            }
            ,
            {
                "name": "Title",
                "render": function (data, type, row) {
                    return `<div class="d-flex align-items-center">
                                 <a href="/Books/Details/${row.id}">
                                   <div class="symbol symbol-50px me-3">
                                      <img src="${row.thumbNail}" alt="">
                                   </div>
                                </a>
                                 <div class="d-flex justify-content-start flex-column">
                                <a href="/Books/Details/${row.id}" class="text-gray-800 fw-bold text-hover-primary mb-1 fs-6">${row.title}</a>
                                <span class="text-gray-400 fw-semibold d-block fs-7">by ${row.author}</span>
                                </div>
                    </div>`;
                }
            },
            {
                "data": "publisher", "name": "Publisher"
            },
            {
                "data": "hall", "name": "Hall"
            },
            {
                "name": "CreatedOn",
                "render": function (data, type, row) {
                    return ` <div class="">${new Date(row.publishingDate).toISOString().split('T')[0]}</div>`;
                }
            },
            {
                "name": "Categories",
                "orderable": false,
                "render": function (data, type, row) {
                    return `<span class="badge badge-light-info me-auto">
                        ${row.categories}
                    </span>`;
                }
            },
            {
                "name": "IsActive",
                "render": function (data, type, row) {
                    return `<span class="badge badge-light-${(row.isActive ? 'success' : 'danger')} js-toggle-badge">
                            ${(row.isActive ? 'Available' : 'Deleted')}
                    </span>`;
                }
            },
            {
                "name": "IsAvailableForRental",
                "render": function (data, type, row) {
                    return `<span class="badge badge-light-${(row.isAvailableForRental ? 'success' : 'warning')}">
                            ${(row.isAvailableForRental ? 'Available' : 'NotAvailable')}
                    </span>`;
                }
            },
            {
                "orderable": false,
                "render": function (data, type, row) {
                    return ` <button type="button" class="btn btn-icon btn-color-muted btn-active-light-primary" data-kt-menu-trigger="hover" data-kt-menu-placement="bottom-end">
                                                        <!--begin::Svg Icon | path: icons/duotune/general/gen024.svg-->
                                                        <span class="svg-icon svg-icon-2">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0z" />
                                                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8zm8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7z" />
                                                        </svg>
                                                        </span>
                                                        <!--end::Svg Icon-->
                                                        </button>
                                                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-200px py-3" data-kt-menu="true" style="">
                                                        <!--begin::Menu item-->
                                                        <div class="menu-item px-3">
                                                        <a href="/Books/Edit/${row.id}" class="menu-link px-3">
                                                        Edit
                                                        </a>
                                                        </div>
                                                        <div class="menu-item px-3">
                                                        <a href="/Books/Details/${row.id}" class="menu-link flex-stack px-3">
                                                        Details
                                                        </a>
                                                        </div>
                                                        <!--end::Menu item-->
                                                        <!--begin::Menu item-->
                                                        <div class="menu-item px-3">
                                                        <a href="javascript:;" data-id="${row.id}" class="menu-link flex-stack px-3 js-toggle-delete">
                                                        Delete
                                                        </a>
                                                        </div>
                                                         
                                                        <!--end::Menu item-->
                                                        </div>`;
                }

            }
        ]

    });
    //End ServerSide DataTable

    //Start Search Button
    $('#search').on('keyup click', function () {
        datatable.search($('#search').val()).draw();
    });
    //End Search Button

    //Delete Action For Books
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
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');

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
