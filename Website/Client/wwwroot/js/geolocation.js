function canUseGeolocation() {
    return !!navigator.geolocation;
}

async function getCurrentLocation() {
    return await new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition((position) => {
            resolve({
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            });
        }, (error) => {
            reject(error);
        },
        {
            enableHighAccuracy: true
        });
    });
}

function startWatchingPosition(dotNetHelper) {
    const watchId = navigator.geolocation.watchPosition(
        (position) => {
            const location = {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            };
            dotNetHelper.invokeMethodAsync('UpdateLocation', location);
        },
        (error) => {
            reject.error(error);
        },
        {
            enableHighAccuracy: true
        }
    );

    return watchId;
}

function stopWatchingPosition(watchId) {
    navigator.geolocation.clearWatch(watchId);
}

