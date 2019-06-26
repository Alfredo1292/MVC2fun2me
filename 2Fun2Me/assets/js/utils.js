let utils = {};
utils.uId = function () {
    // Math.random should be unique because of its seeding algorithm.
    // Convert it to base 36 (numbers + letters), and grab the first 9 characters
    // after the decimal.
    return '_' + Math.random().toString(36).substr(2, 9);
};
utils.loadJS = ({ url, appendOf, callback }) => {
    //url is URL of external file, implementationCode is the code
    //to be called from the file, location is the location to 
    //insert the <script> element

    let scriptTag = document.createElement('script');
    appendOf = appendOf || document.body;
    scriptTag.src = url;

    //scriptTag.onload = callback('onLoad');
    scriptTag.onreadystatechange = callback('onReadyStateChange');

    appendOf.appendChild(scriptTag);
};

utils.isIos = () => {
    return /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
}
utils.capitalizeFirstLetter = function (s) {
    return s.replace(/^[a-z]{1}/, function (v) {
        return v.toUpperCase();
    });
    //return string.charAt(0).toUpperCase() + string.slice(1);
};

utils.getUrlVar = function (variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
    return "";
}

utils.validateNumberOnType = function (evt) {
    let keynum;
    if (window.event) {// IE
        keynum = evt.keyCode;
    } else {
        keynum = evt.which;
    }
    if ((keynum > 47 && keynum < 58) || (keynum > 95 && keynum < 106) || [8, 0, 9, 39, 37, 46].indexOf(keynum) > -1) {
        return true;
    } else {
        evt.preventDefault();
        evt.stopPropagation();
        return false;
    }
}

utils.parseTemplate = (data, template) => {
    for (let i in data) {
        template = template.replace(new RegExp(`\\$\\{${i}}`, 'g'), data[i]);
    }
    return template;
}


utils.customStorage = function (keyName) {
    let obj = {};
    this.getAll = () => {
        if (keyName in localStorage) {
            obj = JSON.parse(localStorage[keyName]);
        }
        if (Object.keys(obj).length > 0) {
            return obj;
        } else {
            return;
        }
    }

    this.merge = (key, jsonOb) => {
        let current = this.get(key) || {};
        console.log('current is');
        console.log(current);
        for (let i in jsonOb) {
            current[i] = jsonOb[i];
        }
        this.set(key, current);
        return this.get(key);
    }

    this.set = (key, value) => {
        if (keyName in localStorage) {
            obj = JSON.parse(localStorage[keyName]);
        }

        obj[key] = value;
        localStorage[keyName] = JSON.stringify(obj);
        console.info("key %s set to ", key, value);
    }

    this.get = (value) => {
        if (keyName in localStorage) {
            obj = JSON.parse(localStorage[keyName]);
            return obj[value];
        } else {
            return;
        }
    }

    this.rmAll = () => {
        obj = {};
        delete (localStorage[keyName]);
    }

    this.rm = function (args) {
        if (keyName in localStorage) {
            obj = JSON.parse(localStorage[keyName]);
            if (Array.isArray(args)) {
                for (const i = 0, max = args.length; i < max; i++) {
                    delete (obj[args[i]]);
                }
            } else {
                delete (obj[args]);
            }
            localStorage[keyName] = JSON.stringify(obj);
        }
    }
}