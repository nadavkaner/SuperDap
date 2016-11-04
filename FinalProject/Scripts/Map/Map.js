var map;
function initializeMap() {
    var myLatlng = new google.maps.LatLng(31.704044, 34.873349);
    var mapOptions = {
        zoom: 10,
        center: myLatlng
    }
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
}

function addMarkerToMap(store) {
    var marker = new google.maps.Marker({
        position: { lat: store.Coordinates.Lat, lng: store.Coordinates.Long },
        map: map,
        title: store.Name
    });
}