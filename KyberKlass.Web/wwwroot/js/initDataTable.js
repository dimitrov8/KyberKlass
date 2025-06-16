
(function ($) {
    function setupDataTable({ resultContainerSelector, searchUrl, tableSelector }) {
        const config = {
            dom:
                "<'d-flex justify-content-between align-items-center'<'dt-top-left'><'dt-top-right'B>>" +
                't' +
                "<'d-flex justify-content-between align-items-center'<'dt-bottom-left'l><'dt-bottom-center'p><'dt-bottom-right'>>",
            buttons: [
                { extend: 'csv', exportOptions: { columns: ':not(:last-child)' } },
                { extend: 'excel', exportOptions: { columns: ':not(:last-child)' } },
                { extend: 'pdf', exportOptions: { columns: ':not(:last-child)' } },
                { extend: 'print', exportOptions: { columns: ':not(:last-child)' } },
            ],
            pageLength: 10,
            lengthMenu: [10, 25],
            responsive: true,
        };

        function initDataTable() {
            const $table = $(tableSelector);
            if ($.fn.DataTable.isDataTable($table)) {
                $table.DataTable().destroy();
            }
            $table.DataTable(config);
            $(`${tableSelector}_filter`).hide();
        }

        initDataTable();


            $(`#searchInput`).off('keyup').on('keyup', function () {
                const searchTerm = $(this).val();

                clearTimeout(window.delayTimer);
                window.delayTimer = setTimeout(() => {
                    if (searchTerm.length >= 3 || searchTerm.length === 0) {
                        $.ajax({
                            url: searchUrl,
                            type: 'GET',
                            data: { searchTerm: searchTerm },
                            headers: { 'X-Requested-With': 'XMLHttpRequest' },
                            success: function (data) {
                                $(resultContainerSelector).html(data);
                                initDataTable();
                            },
                            error: function () {
                                console.error('Error fetching search results');
                            },
                        });
                    }
                }, 300);
            });
        }

    window.setupDataTable = setupDataTable;
})(jQuery);
