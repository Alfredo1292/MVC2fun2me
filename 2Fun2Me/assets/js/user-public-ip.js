'use strict';
// FREE: no request linit for ipify.org
const getIpAddress = () => {
    return new Promise((resolve, reject) => {
        let request = new XMLHttpRequest();
        request.open('GET', "https://api.ipify.org?format=jsonp=", true);
        request.onload = function () {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                return resolve(request.responseText);
            } else {
                // We reached our target server, but it returned an error
                return reject(new Error('Error getting public ip address'));
            }
        }
        request.onerror = function () {
            // There was a connection error of some sort
            return reject(Error('get ip address connection failed'));
        }
        request.send();
    });
}