let appUtils = {};
appUtils.adsProvider = location.href.includes('/index2') ? 'fb' : 'ads-n';
appUtils.urlPath = appUtils.adsProvider === 'fb' ? 'index2/' : '';
appUtils.loadJS = (url, appendOf ) => {
    //url is URL of external file, implementationCode is the code
    //to be called from the file, location is the location to 
    //insert the <script> element
    return new Promise((resolve, reject) => {
        let scriptTag = document.createElement('script');
        appendOf = appendOf || document.body;
        scriptTag.src = url;
        scriptTag.onload = () => {
            return resolve();
        };

        scriptTag.onerror =() => {
            return reject();
        };
        appendOf.appendChild(scriptTag);
    });
};

appUtils.capitalizeFirstLetter = function (s) {
    return s.replace(/^[a-z]{1}/, function (v) {
        return v.toUpperCase();
    });
    //return string.charAt(0).toUpperCase() + string.slice(1);
};

appUtils.validateNumberOnType = function (evt) {
    let keynum = evt.which;
    if ((keynum > 47 && keynum < 58) || /* (keynum > 95 && keynum < 106) ||*/[8, 0, 9, 39, 37, 46].indexOf(keynum) > -1) {
        return true;
    } else {
        evt.preventDefault();
        evt.stopPropagation();
        evt.stopImmediatePropagation();
        return false;
    }
}

appUtils.parseTemplate = (data, template) => {
    for (let i in data) {
        template = template.replace(new RegExp(`\\$\\{${i}}`, 'g'), data[i]);
    }
    return template;
}

appUtils.validateCrPhoneNumber = (str) => {
    return str.length === 8 && ["2", "8", "7", "6"].includes(str.charAt("0"));
}