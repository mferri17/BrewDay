
/* ---------------------------------------- SUBMIT HELPER ---------------------------------------- */

/**
 * Handler generico di un submit, valida i principali input in base alla classe assegnata (required-input, number-int, number-float, datepicker).
 * Invocare come callback sull'evento che scaturisce il submit (non deve essere l'evento di submit stesso onde evitare chiamate ricorsive).
 * @param {Event} event l'evento che scatena il submit, essenziale per risalire al form che si sta submittando
 */
var defaultSubmitHandler = function (event) {
    event.preventDefault();
    var form = $(event.target).closest("form");

    if (defaultValidation(form))
        form.submit();
}


/**
 * Validazione standard di un form in base alle classi assegnate agli input (required-input, number-int, number-float, datepicker).
 * Invocare come una normale chiamata a funzione e controllarne il valore booleano di ritorno.
 * @param {any} form l'oggetto jQuery che rappresenta il form da validare
 * @return {bool} true se il form in input è valido, false altrimenti
 */
var defaultValidation = function (form) {
    var valid = true;
    form.find(".has-error").removeClass("has-error");

    // campi obbligatori
    form.find(".required-input").each(function (index, element) {
        if ($(element).val() == "") {
            $(element).focus().parent().addClass("has-error");
            valid = false;
        }
    });

    // numeri interi
    form.find(".number-int").each(function (index, element) {
        let value = $(element).val();
        if (value != "" && !value.isIntNumber()) {
            $(element).focus().parent().addClass("has-error");
            valid = false;
        }
    });

    // numeri decimali
    form.find(".number-float").each(function (index, element) {
        let value = $(element).val().replace(",", ".");
        if (value != "" && !value.isFloatNumber()) {
            $(element).focus().parent().addClass("has-error");
            valid = false;
        }
    });

    // date in formato dd/mm/yy
    form.find(".datepicker").each(function (index, element) {
        let value = $(element).val();
        if (value != "")
            try {
                parsedDate = $.datepicker.parseDate('dd/mm/yy', value);
            }
            catch (err) {
                $(element).focus().parent().addClass("has-error");
                valid = false;
            }
    });

    return valid;
}