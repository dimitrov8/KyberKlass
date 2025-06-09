function setupDataTable({ searchInputSelector, resultContainerSelector, searchUrl, tableSelector }) {
  const config = {
    dom:
      "<'d-flex justify-content-between mb-2'<'dt-top-left'f><'dt-top-right'B>>" +
      't' +
      "<'d-flex justify-content-between align-items-center mt-2'<'dt-bottom-left'l><'dt-bottom-center'p>>",
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

      $(`${tableSelector}_filter label`).contents().filter(function () {
          return this.nodeType === 3;
      }).remove();

      $(`${tableSelector}_filter input`).attr('placeholder', 'Search...');
  }

  initDataTable();

  let delayTimer;
  $(document).on('keyup', searchInputSelector, function () {
    clearTimeout(delayTimer);
    delayTimer = setTimeout(() => {
      const searchTerm = $(this).val();
      if (searchTerm.length >= 3 || searchTerm.length === 0) {
        $.ajax({
          url: searchUrl,
          type: 'GET',
          data: { search: searchTerm },
          success: function (data) {
            $(resultContainerSelector).html(data);
            initDataTable();
          },
          error: function (xhr, status, error) {
            console.error('Error fetching search results');
          },
        });
      }
    }, 300);
  });
}
