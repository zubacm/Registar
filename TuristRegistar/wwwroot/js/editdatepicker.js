var $DPicker1 = $('#datepicker1');
var $DPicker2 = $('#datepicker2');
$DPicker1.datepicker({
    uiLibrary: 'bootstrap4',
    format: 'yyyy-mm-dd',
    change: function (e) {
        RecreateDatePicker2();
    },
    minDate: new Date()

});

$DPicker2.datepicker({
    uiLibrary: 'bootstrap4',
    format: 'yyyy-mm-dd',
    minDate: new Date(new Date() + (24 * 60 * 60 * 1000)),
    change: function (e) {
        RecreateDatePicker1();
    }
});

function RecreateDatePicker1() {
    var checkout = $('#datepicker2').val();
    var maxDate = new Date(new Date(checkout) - (24 * 60 * 60 * 1000));
    var currentcheckin = $('#datepicker1').val();
    if (checkout && currentcheckin)
        $('#period-btn').prop('disabled', false);

    $DPicker1.destroy();
    $DPicker1.datepicker({
        uiLibrary: 'bootstrap4',
        format: 'yyyy-mm-dd',
        value: currentcheckin,
        minDate: new Date(),
        maxDate: maxDate,
        change: function (e) {
            RecreateDatePicker2();
        }
    });
}

function RecreateDatePicker2() {
    var checkin = $('#datepicker1').val();
    var mindate = new Date();
    if (checkin)
        mindate = new Date(new Date(checkin) + (24 * 60 * 60 * 1000));

    var currentcheckout = $('#datepicker2').val();
    if (checkin && currentcheckout)
        $('#period-btn').prop('disabled', false);

    $DPicker2.destroy();
    $DPicker2.datepicker({
        uiLibrary: 'bootstrap4',
        format: 'yyyy-mm-dd',
        value: currentcheckout,
        minDate: mindate,
        change: function (e) {
            RecreateDatePicker1();
        }
    });
}

function AddPeriod() {
    var checkin = $('#datepicker1').val();
    var checkout = $('#datepicker2').val();


    var objId = $('#objectid').val();

    var data = new FormData;
    data.append("UnavailablePeriodsString", checkin + ":" + checkout);
    data.append("Id", objId);

    $.ajax({
        type: "Post",
        url: "/Object/AddPeriod",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            var myhtml = '<li checkin="' + checkin + '" checkout="' + checkout + '" class="list-group-item row no-gutters  d-flex justify-content-between lh-condensed" >'
                + '<div class="list-group-item row "><i class="fas fa-sign-out-alt fa-2x"></i>&nbsp;&nbsp;<h4>' + checkin + '</h4></div>'
                + '<div class="list-group-item row"><i class="fas fa-sign-out-alt fa-2x fa-flip-horizontal"></i>&nbsp;&nbsp;<h4>' + checkout + '</h4></div>'
                + '<div><a onclick="removePeriod(this)" period-id="' + response+'" class="btn pointer"><i class="fa fa-times"></i></a></div>'
                + '</li>';
            $('#periods').append(myhtml);
        },
        error: function () {
            alert('Greška prilikom dodavanja podataka.');
        }
    });



    $('#period-btn').prop('disabled', true);
    $DPicker1.destroy();
    $DPicker2.destroy();

    $DPicker1.datepicker({
        uiLibrary: 'bootstrap4',
        format: 'yyyy-mm-dd',
        change: function (e) {
            RecreateDatePicker2();
        },
        minDate: new Date()

    });

    $DPicker2.datepicker({
        uiLibrary: 'bootstrap4',
        format: 'yyyy-mm-dd',
        minDate: new Date(new Date() + (24 * 60 * 60 * 1000)),
        change: function (e) {
            RecreateDatePicker1();
        }
    });
}

function removePeriod(ev) {
    var periodid = $(ev).attr('period-id');

    var data = new FormData;
    data.append("DeletePeriodId", periodid);

    $.ajax({

        type: "Post",
        url: "/Object/DeletePeriod",
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            $(ev).parent().parent().remove();
        },
        error: function () {
            alert('Greška prilikom brisanja podataka.');
        }
    });
}



