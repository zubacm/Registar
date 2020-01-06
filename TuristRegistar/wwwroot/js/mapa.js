    var map;
    var marker;

    function centerToUsersLocation() {
        if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(
            function success(position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map.setCenter(pos);
                map.setZoom(12);
                marker.setPosition(pos);
                document.getElementById("latitude-value").value = pos.lat;
                document.getElementById("longitude-value").value = pos.lng;
            }
        );
    }
}

    function initMap() {
        centerToUsersLocation();
    map = new google.maps.Map(document.getElementById('map'), {
        center: {lat: 43.823508, lng: 18.374364 },
    zoom: 7
});
        marker = new google.maps.Marker({
        position: {lat: 43.823508, lng: 18.374364 },
    map: map,
    draggable: true,
    title: "Lokacija"
});

document.getElementById("latitude-value").value = "43.823508";
document.getElementById("longitude-value").value = "18.374364";

        google.maps.event.addListener(marker, 'dragend', function () {
        document.getElementById("latitude-value").value = marker.getPosition().lat();
    document.getElementById("longitude-value").value = marker.getPosition().lng();
});
}



