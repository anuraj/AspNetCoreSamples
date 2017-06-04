var urlToImage = require('url-to-image');

module.exports = function (callback, url, imageName) {
    urlToImage(url, imageName).then(function () {
        callback(/* error */ null, imageName);
    }).catch(function (err) {
        callback(err, imageName);
    });
};