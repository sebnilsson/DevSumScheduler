import axios from 'axios';
var dayCache = {};
var speakerCache = {};
var ApiService = /** @class */ (function () {
    function ApiService() {
    }
    ApiService.getDays = function () {
        return axios
            .get('/api/days')
            .then(function (response) {
            return response.data;
        });
    };
    ApiService.getDay = function (index) {
        var url = "api/day/" + index;
        var cachedDay = dayCache[url];
        if (cachedDay) {
            return Promise.resolve(cachedDay);
        }
        return axios
            .get(url)
            .then(function (response) {
            var day = response.data;
            dayCache[url] = day;
            return day;
        });
    };
    ApiService.getSpeaker = function (slug) {
        var url = "api/speaker/" + slug;
        var cachedSpeaker = speakerCache[url];
        if (cachedSpeaker) {
            return Promise.resolve(cachedSpeaker);
        }
        return axios
            .get(url)
            .then(function (response) {
            var speaker = response.data;
            speakerCache[url] = speaker;
            return speaker;
        });
    };
    return ApiService;
}());
export default ApiService;
//# sourceMappingURL=ApiService.js.map