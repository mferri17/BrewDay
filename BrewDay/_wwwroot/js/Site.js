
/* ------------------------------- CODICE COMUNE A TUTTE LE PAGINE ------------------------------- */

$(document).ready(function () {

    // Sostituisce le virgole con i punti (sul keypress) per tutti gli elementi che hanno classe "number-float"
    $(document).on("keypress", ".number-float", replacePointWithComma);

    // Inizializza tutte le tabelle che hanno classe datatable
    $(".data-table").DataTable({
        language: { url: "/_wwwroot/vendor/datatables/Italian.json" },
        stateSave: true,
        responsive: true,
        colReorder: true,
        columnDefs: [
            { "targets": "no-sort", "orderable": false },
            { "targets": "no-search", "searchable": false },
        ]
    });

    // Inizializza tooltip per i suggerimenti utente
    $('[data-toggle="tooltip"]').tooltip();


    // Inizializza bootstrap confirmation per le conferme eliminazioni
    $('[data-toggle=confirmation]').confirmation({
        //btnOkClass: 'btn btn-sm btn-success',
        //btnCancelClass: 'btn btn-sm btn-danger'
        //popout: true,
        btnOkLabel: "Certo!",
        btnCancelLabel: "No",
        title: "Sei sicuro?"
    });

    // Aggiunge datepicker di jQuery UI a tutti gli input che hanno classe "datepicker"
    $(document).on("focus", ".datepicker:not([readonly])", function (event) {
        if ($(this).hasClass("hasDatepicker") === false) {
            $(this).datepicker({
                showOtherMonths: true,
                selectOtherMonths: true,
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd/mm/yy"
            });
        }
    });
    // Risolve bug Firefox per jQuery UI Datepicker all'interno di un Bootstrap Modal
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    // Riempie titolo e contenuto del modale di aiuto utente
    $(document).on("click", ".help-icon", function (event) {
        var header = $(this).attr("data-header");
        var body = $(this).attr("data-body");
        var modal = $(this).attr("data-target");
        $(modal).find(".modal-header").html(header);
        $(modal).find(".modal-body").html(body);
    });
    
});


