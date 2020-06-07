map = new google.maps.Map(document.getElementById('map'), {
    center: { lat: +($('#latitude-value').val()), lng: +($('#longitude-value').val()) },
    zoom: 15
});
marker = new google.maps.Marker({
    position: { lat: +($('#latitude-value').val()), lng: +($('#longitude-value').val()) },
    map: map,
    draggable: false,
    title: "Lokacija"
});

//map = new google.maps.Map(document.getElementById('map'), {
//    center: { lat: 43.823508, lng: 18.374364 },
//    zoom: 7
//});
//marker = new google.maps.Marker({
//    position: { lat: 43.823508, lng: 18.374364 },
//    map: map,
//    draggable: true,
//    title: "Lokacija"
//});