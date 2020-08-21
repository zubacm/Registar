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


function GetAttributes() {
    var sel = document.getElementById("attribute-select");
    var text = sel.options[sel.selectedIndex].text;

 
    var atrid = sel.options[sel.selectedIndex].value;
    var objId = $('#objectid').val();

    var data = new FormData;
    data.append("AddAttributeId", atrid);
    data.append("Id", objId);

    $.ajax({
        type: "Post",
        url: "/Object/AddObjectHasAttribute",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed"  value="'
                + sel.value + '"> <h5 class="my-0 col-md-12">'
                + text
                + '<a atr-id="' + atrid + '" class="btn btn-error pointer" onclick="attributeRemove(this);"  style="float:right;padding:0;line-height:0;"><i class="fa fa-times"></i></a></h5></li>';

            $("#attributes-container").append(
                myhtml
            );
            offers.push(sel.value);
            $("#added-offers").val(offers);

            RefreshOffersSelectList();

        },
        error: function () {
            alert('Greška prilikom dodavanja podataka.');
        }
    }); 


    
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


    var atrid = $(ev).attr('atr-id');
    var objId = $('#objectid').val();

    var data = new FormData;
    data.append("DeleteAttributeId", atrid);
    data.append("Id", objId);

    $.ajax({
        type: "Post",
        url: "/Object/DeleteObjectHasAttribute",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            $(ev).parent().parent().remove();
            RefreshOffersSelectList();
        },
        error: function () {
            alert('Greška prilikom brisanja podataka.');
        }
    }); 
}


function GetCntAttributes() {
    $('#add-cnt-offer').empty();

    var sel = document.getElementById("cnt-attr-select");
    var text = sel.options[sel.selectedIndex].text;
    var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
        + sel.value + '" text-value="' + text + '">'
        + '<span class="my-0 col-md-12"><div class="row">'
        + '<h5 class="col-md-4">' + text + '</h5>'
        + '<input class="col-md-4 form-control form-control-sm myinput" placeholder="Unesite vrijednost" required type="number" min="1" onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 48 && event.charCode <= 57"/>'
        + '<div class="col-md-4"><a  class="btn btn-primary pointer" onclick="addCntOffer(this);"  style="color:white;float:right;line-height:0;">'
        + 'Dodaj&nbsp<i class="fa fa-plus"></i></a></div></div></h5></li>';
    $("#add-cnt-offer").append(
        myhtml
    );
}
function addCntOffer(ev) {
    var element = $(ev).parent().parent().parent().parent();
    var idvalue = $(element).attr('id-value');
    var textvalue = $(element).attr('text-value');
    var myvalue = $(element).find('input')[0].value;
    if (!myvalue)
        alert("Molimo Vas da unesete vrijednost.");
    else {
        var objId = $('#objectid').val();

        var data = new FormData;
        data.append("AddCntAttributeId", idvalue);
        data.append("AddCntAttributeValue", myvalue);
        data.append("Id", objId);

        $.ajax({
            type: "Post",
            url: "/Object/AddCntAttribute",
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
                    + idvalue + '" value="">'
                    + '<span class="my-0 col-md-12"><div class="row">'
                    + '<h5 class="col-md-4">' + textvalue + '</h6>'
                    + '<input class="col-md-4 col-md-4 form-control form-control-sm" value ="'+myvalue+'" disabled />'
                            + '<div class="col-md-4"><a  class="btn btn-error pointer" onclick="removeCntOffer(this);"  style="float:right;padding:0;line-height:0;">'
                            + '<i class="fa fa-times"></i></a></div></div></h5></li>';

                $("#countable-attr-container").append(
                    myhtml
                );
                $(element).remove();
                countableOffers.push(idvalue);
                $("#selectlist-cntoffers").val(countableOffers);
                RefreshCntOffersSelectList();

            },
            error: function () {
                alert('Greška prilikom dodavanja podataka.');
            }
        }); 

        
    }
}

function removeCntOffer(ev) {
    var myelement = $(ev).parent().parent().parent().parent();
    var removedidvalue = $(myelement).attr('id-value');
    const index = countableOffers.indexOf(removedidvalue);
    if (index > -1) {
        countableOffers.splice(index, 1);
    }

    var objId = $('#objectid').val();

    var data = new FormData;
    data.append("DeleteCntAttributeId", removedidvalue);
    data.append("Id", objId);

    $.ajax({
        type: "Post",
        url: "/Object/DeleteCntAttribute",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {           
            $("#selectlist-cntoffers").val(countableOffers);
            $(myelement).remove();
            RefreshCntOffersSelectList();
        },
        error: function () {
            alert('Greška prilikom brisanja podataka.');
        }
    }); 
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
    $('#add-special-offer').empty();


    var sel = document.getElementById("specialoffer-select");
    var text = sel.options[sel.selectedIndex].text;
    var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
        + sel.value + '" text-value="' + text + '">'
        + '<span class="my-0 col-md-12"><div class="row">'
        + '<h5 class="col-md-4">' + text + '</h6>'
        + '<input class="col-md-4 form-control form-control-sm myinput" placeholder="Unesite cijenu" required type="number" step="0.000001" />'
        + '<div class="form-control-sm input-group-append col-md-1"><span class="input-group-text text-muted">' + currency + '</span></div>'
        + '<div class="col-md-3"><a  class="btn btn-primary pointer" onclick="addSpecialOffer(this);"  style="color:white;float:right;line-height:0;">'
        + 'Dodaj&nbsp<i class="fa fa-plus"></i></a></div></div></h5></li>';

    $("#add-special-offer").append(
        myhtml
    );
}
function addSpecialOffer(ev) {
    var element = $(ev).parent().parent().parent().parent();
    var idvalue = $(element).attr('id-value');
    var textvalue = $(element).attr('text-value');
    var myvalue = $(element).find('input')[0].value;
    if (!myvalue)
        alert("Molimo Vas da unesete vrijednost.");
    else {
        var objId = $('#objectid').val();

        var data = new FormData;
        data.append("AddSpecialOfferId", idvalue);
        data.append("AddSpecialOfferValue", myvalue);
        data.append("Id", objId);

        $.ajax({
            type: "Post",
            url: "/Object/AddSpecialOffer",
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                var myhtml = '<li class="list-group-item d-flex justify-content-between lh-condensed" id-value="'
                    + idvalue + '" value="">'
                    + '<span class="my-0 col-md-12"><div class="row">'
                    + '<h5 class="col-md-4">' + textvalue + '</h6>'
                    + '<input class="col-md-4 col-md-4 form-control form-control-sm" value ="' + myvalue + '" disabled />'
                    + '<div class="col-md-4"><a  class="btn btn-error pointer" onclick="removeSpecialOffer(this);"  style="float:right;padding:0;line-height:0;">'
                    + '<i class="fa fa-times"></i></a></div></div></h5></li>';

                $("#specialoffers-container").append(
                    myhtml
                );
                $(element).remove();
                payedOffers.push(idvalue);
                $("#selectlist-specialoffers").val(payedOffers);
                RefreshSpecialOffersSelectList();

            },
            error: function () {
                alert('Greška prilikom dodavanja podataka.');
            }
        });


    }

}


function removeSpecialOffer(ev) {
    var myelement = $(ev).parent().parent().parent().parent();
    var removedidvalue = $(myelement).attr('id-value');
    const index = payedOffers.indexOf(removedidvalue);
    if (index > -1) {
        payedOffers.splice(index, 1);
    }
    var objId = $('#objectid').val();

    var data = new FormData;
    data.append("DeleteSpecialOfferId", removedidvalue);
    data.append("Id", objId);

    $.ajax({
        type: "Post",
        url: "/Object/DeleteSpecialOffer",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            $("#selectlist-specialoffers").val(payedOffers);
            $(myelement).remove();
            RefreshSpecialOffersSelectList();
        },
        error: function () {
            alert('Greška prilikom brisanja podataka.');
        }
    }); 
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

function fillPrices() {
    var occ_b_prices = document.getElementById('prices').getElementsByTagName("input");
    for (i = 0; i < occ_b_prices.length; i += 2) {
        var visitors = $(occ_b_prices[i]).attr('value');
        var pricevalue = $(occ_b_prices[i + 1]).val();
        occupancabasedprices.push(visitors + ":" + pricevalue);
    }
    $('#occupancybased-prices').val(occupancabasedprices);
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


