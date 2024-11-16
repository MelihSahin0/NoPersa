var carIcon = new L.Icon({
    iconUrl: 'images/marker-icon-car.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [30, 30],
    iconAnchor: [10, 20],
    popupAnchor: [1, -20],
    shadowSize: [30, 30]
});

var blueIcon = new L.Icon({
    iconUrl: 'images/marker-icon-blue.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
});

var redIcon = new L.Icon({
    iconUrl: 'images/marker-icon-red.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
});

var greenIcon = new L.Icon({
    iconUrl: 'images/marker-icon-green.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
});

function updateMapHeight() {
    const mapContainer = document.getElementById('map');

    if (mapContainer) {
        mapContainer.style.height = window.innerHeight * 0.7 + 'px';
    }
}

window.addEventListener('resize', updateMapHeight);

function calculateMaxBounds(points) {
    let minLat = Infinity;
    let maxLat = -Infinity;
    let minLng = Infinity;
    let maxLng = -Infinity;

    points.forEach(point => {
        const lat = point[0];
        const lng = point[1];
        if (lat < minLat) minLat = lat;
        if (lat > maxLat) maxLat = lat;
        if (lng < minLng) minLng = lng;
        if (lng > maxLng) maxLng = lng;
    });

    return L.latLngBounds(
        [minLat - 0.001, minLng - 0.001],
        [maxLat + 0.001, maxLng + 0.001] 
    );
}

var leafletMap;
function isInitialized() {
    return !!leafletMap;
}

function clearMap() {

    clearMarkers();
    clearMyMarker();

    if (routeLayer) {
        leafletMap.removeLayer(routeLayer);
        routeLayer = null;
    }

    leafletMap.eachLayer(layer => {
        leafletMap.removeLayer(layer);
    });

    leafletMap = null;
}

function initialize(elementId, lat, lng, zoom, baseUrl, pointsJson) {
    const points = JSON.parse(pointsJson);

    updateMapHeight();

    leafletMap = L.map(elementId).setView([lat, lng], zoom);

    const bounds = calculateMaxBounds(points);
    leafletMap.setMaxBounds(bounds);
    leafletMap.fitBounds(bounds);

    L.tileLayer(`https://${baseUrl}/DeliveryManagement/{z}/{x}/{y}`, {
        maxZoom: 17,
        minZoom: 14,
        maxBoundsViscosity: 1.0,
        useCache: true,
        crossOrigin: true,
        pouchdb: new PouchDB('mapTilesCache')
    }).addTo(leafletMap);
}

var markers = [];
function addMarker(lat, lng, popupText, imageId) {
    let marker;
    if (imageId == 0) {
        marker = L.marker([lat, lng], { icon: greenIcon }).addTo(leafletMap)
            .bindPopup(popupText);
    }
    else if (imageId == 1) {
        marker = L.marker([lat, lng], { icon: blueIcon }).addTo(leafletMap)
            .bindPopup(popupText);
    }
    else {
        marker = L.marker([lat, lng], { icon: redIcon }).addTo(leafletMap)
            .bindPopup(popupText);
    }
    markers.push(marker);
}

function clearMarkers() {
    markers.forEach(marker => leafletMap.removeLayer(marker));
    markers = [];
}

var myMarker;
function addMyMarker(lat, lng, popupText) {
    myMarker = L.marker([lat, lng], { icon: carIcon }).addTo(leafletMap)
        .bindPopup(popupText);
}

function clearMyMarker() {
    if (!!myMarker) {
        leafletMap.removeLayer(myMarker);
        myMarker = null;
    }
}

var routeLayer;
function drawRoute(coordinatesJson) {
    const coordinates = JSON.parse(coordinatesJson);

    if (routeLayer) {
        leafletMap.removeLayer(routeLayer);
    }

    routeLayer = L.polyline(coordinates, { color: 'blue' }).addTo(leafletMap);
}