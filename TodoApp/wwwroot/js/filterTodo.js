$(document).ready(function () {
    // Get query parameters
    const params = new URLSearchParams(window.location.search);

   // Set dropdowns based on query values
    const status = params.get("statusInput");
    const priority = params.get("priorityInput");

    if (status) {
        $('#status').val(status);
    }

    if (priority) {
        $('#priority').val(priority);
    }

    // Auto-submit form on change
    $('#status, #priority').on('change', function () {
        $('#filterForm').submit();
    });
});
