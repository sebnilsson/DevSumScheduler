var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Vue, Component } from "vue-property-decorator";
import ApiService from './ApiService';
import TimeslotSelectionService from './TimeslotSelectionService';
import Navigation from './Navigation.vue';
import Table from './Table.vue';
var App = /** @class */ (function (_super) {
    __extends(App, _super);
    function App() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.days = [];
        _this.dayIndex = 0;
        _this.day = _this.getDefaultDay();
        _this.isDataLoading = false;
        _this.hasDataLoadFailed = false;
        _this.dayCache = {};
        return _this;
    }
    App.prototype.mounted = function () {
        this.loadAllData();
    };
    App.prototype.loadAllData = function () {
        var _this = this;
        ApiService
            .getDays()
            .then(function (data) {
            _this.days.splice(0, _this.days.length);
            for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
                var d = data_1[_i];
                _this.days.push(d);
            }
        });
        this.loadDay(0);
    };
    App.prototype.dayChange = function (index) {
        this.dayIndex = index;
        this.loadDay(index);
        $('.popover-element').popover('hide');
    };
    App.prototype.loadDay = function (index) {
        var _this = this;
        this.isDataLoading = true;
        this.$set(this, 'day', this.getDefaultDay());
        ApiService
            .getDay(index)
            .then(function (day) {
            day.timeslots
                .filter(function (t) { return t.isSelectable; })
                .forEach(function (t) {
                t.id = t.startsAt + "|" + t.endsAt;
                t.sessions.forEach(function (s) { return _this.$set(s, 'isSelected', s.isSelected); });
            });
            TimeslotSelectionService.apply(_this.dayIndex, day.timeslots);
            _this.$set(_this, 'day', day);
            _this.$forceUpdate();
            _this.isDataLoading = false;
        })
            .catch(function () {
            _this.hasDataLoadFailed = true;
            _this.isDataLoading = false;
        });
    };
    App.prototype.getDefaultDay = function () {
        return {
            title: '',
            locations: [],
            timeslots: new Array()
        };
    };
    App = __decorate([
        Component({
            components: {
                Navigation: Navigation,
                Table: Table
            }
        })
    ], App);
    return App;
}(Vue));
export default App;
//# sourceMappingURL=App.js.map