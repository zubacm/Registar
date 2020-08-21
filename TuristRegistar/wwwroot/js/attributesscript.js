var currency = getCookie('Currency');
if (!currency)
    currency = "BAM";

var payedOffers = [];
var payedOffersWithValues = [];
var occupancabasedprices = [];
var countableOffers = [];
var cntOffersAndValues = [];
var unavailableperiods = [];
var offers = [];

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length === 2) return parts.pop().split(";").shift();
}

function fillValues() {
    var mylist = document.getElementById('countable-attr-container').getElementsByTagName("li");
    for (var i = 0; i < mylist.length; i++) {
        var idvalue = $(mylist[i]).attr('id-value');
        var myvalue = $(mylist[i]).find('input')[0].value;
        cntOffersAndValues.push(idvalue + ":" + myvalue);
        $('#countable-offers').val(cntOffersAndValues);
    }


    var specialofferslist = document.getElementById('specialoffers-container').getElementsByTagName("li");
    for (i = 0; i < specialofferslist.length; i++) {
        var idvalueso = $(specialofferslist[i]).attr('id-value');
        var myvalueso = $(specialofferslist[i]).find('input')[0].value;
        payedOffersWithValues.push(idvalueso + ":" + myvalueso);
        $('#special-offersvwithval').val(payedOffersWithValues);
    }

    if ($('#occupancy-model').val() === "true") {
        var occ_b_prices = document.getElementById('prices').getElementsByTagName("input");
        for (i = 0; i < occ_b_prices.length; i += 2) {
            var visitors = $(occ_b_prices[i]).attr('value');
            var pricevalue = $(occ_b_prices[i + 1]).val();
            occupancabasedprices.push(visitors + ":" + pricevalue);
        }
        $('#occupancybased-prices').val(occupancabasedprices);
    }

    var periodslist = document.getElementById('periods').getElementsByTagName("li");
    for (i = 0; i < periodslist.length; i++) {
        var checkin = $(periodslist[i]).attr('checkin');
        var checkout = $(periodslist[i]).attr('checkout');
        unavailableperiods.push(checkin + ":" + checkout);
    }
    $('#unavailable-periods').val(unavailableperiods);
}



function GetAttributes() {
    var sel = document.getElementById("attribute-select");
    var text = sel.options[sel.selectedIndex].text;
    var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed"  value="'
        + sel.value + '"> <h5 class="my-0 col-md-12">'
        + text
        + '<a class="btn btn-error pointer" onclick="attributeRemove(this);"  style="float:right;padding:0;line-height:0;"><i class="fa fa-times"></i></a></h5></li>';
    $("#attributes-container").append(
        myhtml
    );
    offers.push(sel.value);
    $("#added-offers").val(offers);

    RefreshOffersSelectList();
}

function RefreshOffersSelectList() {
    $.ajax({
        type: 'GET',
        url: "/Object/GetObjectAttributes",
        contentType: "application/json",
        data: { excludedAttributesId: document.getElementById('added-offers').value },
        dataType: 'json',
        success: function (data) {
            var offersSelectList = $('#attribute-select');
            offersSelectList.empty();
            offersSelectList.append(
                '<option disabled selected>Izaberite...</option>'
            );

            $.each(data, function (index, model) {
                offersSelectList.append(
                    $('<option>', {
                        value: model.value
                    }).text(model.text)
                );
            });
        },
        error: function () {
            alert('Greška prilikom pribavljanja podataka.');
        }
    });
}

function attributeRemove(ev) {

    var removedvalue = $(ev).parent().parent().attr('value');
    const index = offers.indexOf(removedvalue);
    if (index > -1) {
        offers.splice(index, 1);
    }
    $("#added-offers").val(offers);
    $(ev).parent().parent().remove();
    RefreshOffersSelectList();
}


function GetCntAttributes() {
    var sel = document.getElementById("cnt-attr-select");
    var text = sel.options[sel.selectedIndex].text;
    var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
        + sel.value + '" value="">'
        + '<span class="my-0 col-md-12"><div class="row">'
        + '<h5 class="col-md-4">' + text + '</h6>'
        + '<input class="col-md-4 form-control form-control-sm myinput" placeholder="Unesite vrijednost" required type="number" min="1" onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 48 && event.charCode <= 57"/>'
        + '<div class="col-md-4"><a  class="btn btn-error pointer" onclick="removeCntOffer(this);"  style="float:right;padding:0;line-height:0;">'
        + '<i class="fa fa-times"></i></a></div></div></h5></li>';
    $("#countable-attr-container").append(
        myhtml
    );

    countableOffers.push(sel.value);
    $("#selectlist-cntoffers").val(countableOffers);
    RefreshCntOffersSelectList();
}

function removeCntOffer(ev) {
    var myelement = $(ev).parent().parent().parent().parent();
    console.log(removedidvalue);
    var removedidvalue = $(myelement).attr('id-value');
    const index = countableOffers.indexOf(removedidvalue);
    if (index > -1) {
        countableOffers.splice(index, 1);
    }
    $("#selectlist-cntoffers").val(countableOffers);
    $(myelement).remove();
    RefreshCntOffersSelectList();
}


function RefreshCntOffersSelectList() {
    $.ajax({
        type: 'GET',
        url: "/Object/GetObjectCntAttributes",
        contentType: "application/json",
        data: { excludedAttributesId: document.getElementById('selectlist-cntoffers').value },
        dataType: 'json',
        success: function (data) {
            var offersSelectList = $('#cnt-attr-select');
            offersSelectList.empty();
            offersSelectList.append(
                '<option disabled selected>Izaberite...</option>'
            );

            $.each(data, function (index, model) {
                offersSelectList.append(
                    $('<option>', {
                        value: model.value
                    }).text(model.text)
                );
            });
        },
        error: function () {
            alert('Greška prilikom pribavljanja podataka.');
        }
    });
}


function GetSpecialOffersAttributes() {


    var sel = document.getElementById("specialoffer-select");
    var text = sel.options[sel.selectedIndex].text;
    var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
        + sel.value + '" value="">'
        + '<span class="my-0 col-md-12"><div class="row">'
        + '<h5 class="col-md-4">' + text + '</h6>'
        + '<input class="col-md-4 form-control form-control-sm myinput" placeholder="Unesite cijenu" required type="number" step="0.000001" />'
        + '<div class="form-control-sm input-group-append col-md-1"><span class="input-group-text text-muted">' + currency + '</span></div>'
        + '<div class="col-md-3"><a class="btn btn-error pointer" onclick="removeSpecialOffer(this);"  style="float:right;padding:0;line-height:0;">'
        + '<i class="fa fa-times"></i></a></div></div></h5></li>';
    $("#specialoffers-container").append(
        myhtml
    );

    payedOffers.push(sel.value);
    $("#selectlist-specialoffers").val(payedOffers);
    RefreshSpecialOffersSelectList();
}

function removeSpecialOffer(ev) {
    var myelement = $(ev).parent().parent().parent().parent();
    var removedidvalue = $(myelement).attr('id-value');
    const index = payedOffers.indexOf(removedidvalue);
    if (index > -1) {
        payedOffers.splice(index, 1);
    }
    $("#selectlist-specialoffers").val(payedOffers);
    $(myelement).remove();
    RefreshSpecialOffersSelectList();
}

function RefreshSpecialOffersSelectList() {
    $.ajax({
        type: 'GET',
        url: "/Object/GetObjectAttributes",
        contentType: "application/json",
        data: { excludedAttributesId: document.getElementById('selectlist-specialoffers').value },
        dataType: 'json',
        success: function (data) {
            var offersSelectList = $('#specialoffer-select');
            offersSelectList.empty();
            offersSelectList.append(
                '<option disabled selected>Izaberite...</option>'
            );

            $.each(data, function (index, model) {
                offersSelectList.append(
                    $('<option>', {
                        value: model.value
                    }).text(model.text)
                );
            });
        },
        error: function () {
            alert('Greška prilikom pribavljanja podataka.');
        }
    });
}


$("#country-select").change(function () {
    GetCities();
});
function GetCities() {
    $.ajax({
        type: 'GET',
        url: "/Object/GetCitiesInCountry",
        contentType: "application/json",
        data: { countryId: document.getElementById('country-select').value },
        dataType: 'json',
        success: function (data) {
            var citySelectList = $('#city-select');
            citySelectList.empty();
            citySelectList.append(
                '<option disabled selected>Izaberite...</option>'
            );

            $.each(data, function (index, model) {
                citySelectList.append(
                    $('<option>', {
                        value: model.value
                    }).text(model.text)
                );
            });
        },
        error: function () {
            alert('Greška prilikom pribavljanja podataka.');
        }
    });
}


function changeToStandardModel() {
    $('#occupancy-model').val(false);
}
function changeToOccupancyBModel() {
    $('#occupancy-model').val(true);
    $('#prices').empty();
}
function emptyPrices() {
    $('#prices').empty();
}

function formPrices() {
    $('#prices').empty();
    occupancabasedprices = [];

    var minocc = $('#min-occ-ob').val();
    var maxocc = $('#max-occ-ob').val();

    if ((!maxocc) || (!minocc) || ((+maxocc) < 1) || ((+minocc) < 1) || ((+minocc) > (+maxocc))) {
        alert("Unesite ispravne podatke za broj posjetilaca");
    }
    else {
        $('#prices').append(
            '<label><span class="hidden-xs">Broj posjetilaca</span></label>'
        );
        for (var i = 0; i < (maxocc - minocc + 1); i++) {
            var visitors = (+minocc) + i;
            var myhtml = '<div class="form-group">'
                + '<div class="input-group row">'
                + '<input class="form-control" value="' + visitors + '" disabled>'
                + '<input type="number" step="0.000001" placeholder="Unesite cijenu" name="" class="form-control myinput" required="">'
                + '<div class="input-group-append"><span class="input-group-text text-muted">' + currency + '</span></div></div></div>';
            $('#prices').append(myhtml);
        }
    }
}



$(document).ready(function () {
    ReloadSelectLists();
});

function ReloadSelectLists() {
    var mylist = document.getElementById('countable-attr-container').getElementsByTagName("li");
    for (var i = 0; i < mylist.length; i++) {
        var idvalue = $(mylist[i]).attr('id-value');
        countableOffers.push(idvalue);
    }
    $("#selectlist-cntoffers").val(countableOffers);
    RefreshCntOffersSelectList();

    var specialofferslist = document.getElementById('specialoffers-container').getElementsByTagName("li");
    for (i = 0; i < specialofferslist.length; i++) {
        var idvalueso = $(specialofferslist[i]).attr('id-value');
        payedOffers.push(idvalueso);
    }
    $("#selectlist-specialoffers").val(payedOffers);
    RefreshSpecialOffersSelectList();

    var offerslist = document.getElementById('attributes-container').getElementsByTagName('li');
    for (i = 0; i < offerslist.length; i++) {
        var idvalueo = $(offerslist[i]).val();
        offers.push(idvalueo);
    }

    $("#added-offers").val(offers);
    RefreshOffersSelectList();
}


