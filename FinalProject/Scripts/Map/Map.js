var map;
function initializeMap() {
    var myLatlng = new google.maps.LatLng(0.0, 0.0);
    var mapOptions = {
        zoom:2,
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