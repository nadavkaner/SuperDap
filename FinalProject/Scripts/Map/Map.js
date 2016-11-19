var map;
function initializeMap() {
    var myLatlng = new google.maps.LatLng(0.0, 0.0);
    var mapOptions = {
        zoom:2,
        center: myLatlng
    }
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
}

function addMarkerToMap(company) {
    var marker = new google.maps.Marker({
        position: { lat: company.Coordinates.Latitude, lng: company.Coordinates.Longitude },
        map: map,
        title: company.Name
    });
}