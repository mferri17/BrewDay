
/* ----------------------------------------- STRING UTIL ----------------------------------------- */

/**
 * Data una stringa dal server nel formato 2017-04-14T00:00:00, restituisce una stringa della data nel formato locale (senza orario).
 * Invocare come una normale chiamata a funzione.
 * @param {string} data stringa rappresentante una data nel formato 2017-04-14T00:00:00
 * @return {string} stringa corrispondente alla data in input nel formato locale (senza orario) / stringa vuota se fallisce
 */
var parseServerDateToString = function (data) {
    try {
        let st = data.substring(0, 10)
        let datetime = new Date(st);
        var result = datetime.toLocaleDateString();
    }
    finally {
        return result ? result : "";
    }
}

// Invocare direttamente da una stringa, ritorna un boolean
String.prototype.isFloatNumber = function () {
    return /^[0-9.]+$/.test(this);
}

// Invocare direttamente da una stringa, ritorna un boolean
String.prototype.isIntNumber = function () {
    return /^[0-9]+$/.test(this);
}

/**
 * Sostituisce i punti inseriti con delle virgole. Invocare come callback di un evento keypress.
 * @param {Event} e l'evento keypress, passato in automatico quando si usa la forma abbreviata di chiamata a callback.
 */
var replacePointWithComma = function (e) {
    // '46' is the keyCode for '.'
    if (e.keyCode == '46' || e.charCode == '46') {
        if (document.selection) { // IE
            // Determines the selected text. If no text selected,
            // the location of the cursor in the text is returned
            var range = document.selection.createRange();
            // Place the comma on the location of the selection,
            // and remove the data in the selection
            range.text = ',';
        }
        else if (this.selectionStart || this.selectionStart == '0') { // Chrome + FF
            // Determines the start and end of the selection.
            // If no text selected, they are the same and
            // the location of the cursor in the text is returned
            // Don't make it a jQuery obj, because selectionStart 
            // and selectionEnd isn't known.
            var start = this.selectionStart;
            var end = this.selectionEnd;
            // Place the comma on the location of the selection,
            // and remove the data in the selection
            $(this).val($(this).val().substring(0, start) + ',' + $(this).val().substring(end, $(this).val().length));
            // Set the cursor back at the correct location in 
            // the text
            this.selectionStart = start + 1;
            this.selectionEnd = start + 1;
        } else {
            // if no selection could be determined, 
            // place the comma at the end.
            $(this).val($(this).val() + ',');
        }
        return false;
    }
}