(function ($) {
    function setupDataTable({resultContainerSelector, searchUrl, tableSelector, onInitComplete}) {
        const config = {
            dom:
                "<'d-flex justify-content-between align-items-center dt-toolbar'<'dt-top-left'><'dt-top-right'B>>" +
                't' +
                "<'d-flex justify-content-between align-items-center'<'dt-bottom-left'l><'dt-bottom-center'p><'dt-bottom-right'>>",
            buttons: [
                {extend: 'csv', exportOptions: {columns: ':not(:last-child)'}},
                {extend: 'excel', exportOptions: {columns: ':not(:last-child)'}},
                {extend: 'pdf', exportOptions: {columns: ':not(:last-child)'}},
                {extend: 'print', exportOptions: {columns: ':not(:last-child)'}},
            ],
            pageLength: 10,
            lengthMenu: [10, 25],
            initComplete: function () {
                if (typeof onInitComplete === 'function') {
                    onInitComplete();
                }
            },
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

        function performSearch() {
            const searchTerm = $('#searchInput').val();
            const roleFilter = $('#roleFilter').val();

            clearTimeout(window.delayTimer);
            window.delayTimer = setTimeout(() => {
                $.ajax({
                    url: searchUrl,
                    type: 'GET',
                    data: {
                        searchTerm: searchTerm,
                        roleFilter: roleFilter,
                    },
                    headers: {'X-Requested-With': 'XMLHttpRequest'},
                    success: function (data) {
                        $(resultContainerSelector).html(data);
                        initDataTable();
                    },
                    error: function () {
                        console.error('Error fetching search results');
                    },
                });
            }, 300);
        }

        $('#searchInput').off('input keyup').on('input keyup', performSearch);
        $('#roleFilter').off('change').on('change', performSearch);
    }

    window.setupDataTable = setupDataTable;

    let resizeTimer;
    $(window).on('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            $('.custom-table').each(function () {
                const table = $(this).DataTable();
                if (table) {
                    $(this).css('width', '');
                    table.columns.adjust();
                }
            });
        }, 50);
    });
})(jQuery);
