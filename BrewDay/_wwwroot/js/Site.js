﻿
/* ------------------------------- CODICE COMUNE A TUTTE LE PAGINE ------------------------------- */

$(document).ready(function () {

    // Sostituisce le virgole con i punti (sul keypress) per tutti gli elementi che hanno classe "number-float"
    $(document).on("keypress", ".number-float", replacePointWithComma);
    

    // Aggiunge datepicker di jQuery UI a tutti gli input che hanno classe "datepicker"
    $(document).on("focus", ".datepicker:not([readonly])", function () {
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

});


