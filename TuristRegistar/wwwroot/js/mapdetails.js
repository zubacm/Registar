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
