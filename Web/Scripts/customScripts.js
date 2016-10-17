/// <reference path="jquery-1.10.2.js" />
$(document).ready(function() {

    $("#search").each(function() {
        $(this).autocomplete({ source: $(this).attr("data-autocomplete") });
    });


})