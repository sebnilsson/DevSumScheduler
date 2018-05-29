var localStorageKeyPrefix = 'TimeslotSelctions';
var TimeslotSelectionService = /** @class */ (function () {
    function TimeslotSelectionService() {
    }
    TimeslotSelectionService.apply = function (dayIndex, timeslots) {
        var selections = this.getSelections(dayIndex);
        var _loop_1 = function (key) {
            if (selections.hasOwnProperty(key)) {
                var timeslot = timeslots.find(function (x) { return x.id === key; });
                if (timeslot) {
                    var value = selections[key];
                    var session = timeslot.sessions[value];
                    if (session) {
                        session.isSelected = true;
                    }
                }
            }
        };
        for (var key in selections) {
            _loop_1(key);
        }
    };
    TimeslotSelectionService.remove = function (dayIndex, sessionId) {
        var selections = this.getSelections(dayIndex);
        selections[sessionId] = undefined;
        this.saveSelections(dayIndex, selections);
    };
    TimeslotSelectionService.update = function (dayIndex, sessionId, index) {
        var selections = this.getSelections(dayIndex);
        selections[sessionId] = index;
        this.saveSelections(dayIndex, selections);
    };
    TimeslotSelectionService.getSelections = function (dayIndex) {
        var localStorageKey = this.getLocalStorageKey(dayIndex);
        var stored = localStorage.getItem(localStorageKey);
        var selections = JSON.parse(stored);
        return selections || {};
    };
    TimeslotSelectionService.saveSelections = function (dayIndex, selections) {
        var localStorageKey = this.getLocalStorageKey(dayIndex);
        var selectionJson = JSON.stringify(selections);
        localStorage.setItem(localStorageKey, selectionJson);
    };
    TimeslotSelectionService.getLocalStorageKey = function (dayIndex) {
        return localStorageKeyPrefix + "-" + dayIndex;
    };
    return TimeslotSelectionService;
}());
export default TimeslotSelectionService;
//# sourceMappingURL=TimeslotSelectionService.js.map