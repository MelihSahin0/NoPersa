async function canUseGeolocation() {
    if (!navigator.geolocation) {
        return false; // Geolocation is not supported by the browser
    }

    try {
        const permission = await navigator.permissions.query({ name: 'geolocation' });

        if (permission.state === 'granted') {
            return true;
        }

        if (permission.state === 'prompt') {
            return new Promise((resolve) => {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        resolve(true);
                    },
                    (error) => {
                        resolve(false);
                    }
                );
            });
        }

        return false;
    } catch (error) {
        console.error('Error checking geolocation permission:', error);
        return false;
    }
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

