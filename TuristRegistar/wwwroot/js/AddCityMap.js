var map;
var map2;
var marker;
var marker2;

function centerToUsersLocation() {
    if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(
            function success(position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map2.setCenter(pos);
                map2.setZoom(12);
                marker2.setPosition(pos);
                document.getElementById("latitude-value-addcity").value = pos.lat;
                document.getElementById("longitude-value-addcity").value = pos.lng;
            }
        );
    }
}

function setLatLng() {
    var pos2 = {
        lat: +($('#latitude-value-addcity').val()),
        lng: +($('#longitude-value-addcity').val())
    };
    var pos = {
        lat: +($('#latitude-value').val()),
        lng: +($('#longitude-value').val())
    };

    map2.setCenter(pos2);
    map2.setZoom(12);
    marker2.setPosition(pos2);
    document.getElementById("latitude-value-addcity").value = $('#latitude-value-addcity').val();
    document.getElementById("longitude-value-addcity").value = $('#longitude-value-addcity').val();

    map.setCenter(pos);
    map.setZoom(12);
    marker.setPosition(pos);
    document.getElementById("latitude-value").value = $('#latitude-value').val();
    document.getElementById("longitude-value").value = $('#longitude-value').val();

}

function initMap() {
    if (+($('#latitude-value').val()) === 0 || +($('#longitude-value').val()) === 0) {
        centerToUsersLocation();
        map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: 43.823508, lng: 18.374364 },
            zoom: 7
        });
        marker = new google.maps.Marker({
            position: { lat: 43.823508, lng: 18.374364 },
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
    else {
        map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: +($('#latitude-value').val()), lng: +($('#longitude-value').val()) },
            zoom: 10
        });
        marker = new google.maps.Marker({
            position: { lat: +($('#latitude-value').val()), lng: +($('#longitude-value').val()) },
            map: map,
            draggable: true,
            title: "Lokacija"
        });


        google.maps.event.addListener(marker, 'dragend', function () {
            document.getElementById("latitude-value").value = marker.getPosition().lat();
            document.getElementById("longitude-value").value = marker.getPosition().lng();
        });
    }



    if (+($('#latitude-value-addcity').val()) === 0 || +($('#longitude-value-addcity').val()) === 0) {
        centerToUsersLocation();
        map2 = new google.maps.Map(document.getElementById('map-addcity'), {
            center: { lat: 43.823508, lng: 18.374364 },
            zoom: 7
        });
        marker2 = new google.maps.Marker({
            position: { lat: 43.823508, lng: 18.374364 },
            map: map2,
            draggable: true,
            title: "Lokacija"
        });

        document.getElementById("latitude-value-addcity").value = "43.823508";
        document.getElementById("longitude-value-addcity").value = "18.374364";



        google.maps.event.addListener(marker2, 'dragend', function () {
            document.getElementById("latitude-value-addcity").value = marker2.getPosition().lat();
            document.getElementById("longitude-value-addcity").value = marker2.getPosition().lng();
        });
    }
    else {
        map2 = new google.maps.Map(document.getElementById('map-addcity'), {
            center: { lat: +($('#latitude-value-addcity').val()), lng: +($('#longitude-value-addcity').val()) },
            zoom: 10
        });
        marker2 = new google.maps.Marker({
            position: { lat: +($('#latitude-value-addcity').val()), lng: +($('#longitude-value-addcity').val()) },
            map: map2,
            draggable: true,
            title: "Lokacija"
        });


        google.maps.event.addListener(marker2, 'dragend', function () {
            document.getElementById("latitude-value-addcity").value = marker2.getPosition().lat();
            document.getElementById("longitude-value-addcity").value = marker2.getPosition().lng();
        });
    }

}