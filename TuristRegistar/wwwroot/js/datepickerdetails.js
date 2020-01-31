var $DPicker1 = $('#datepicker1');
var $DPicker2 = $('#datepicker2');
$DPicker1.datepicker({
    uiLibrary: 'bootstrap4',
    format: 'yyyy-mm-dd',
    disableDates: [new Date(2017, 10, 11), '11/12/2017'],
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