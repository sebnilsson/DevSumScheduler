var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
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
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Vue, Component, Prop } from "vue-property-decorator";
import TimeslotSelectionService from './TimeslotSelectionService';
var TableSession = /** @class */ (function (_super) {
    __extends(TableSession, _super);
    function TableSession() {
        var _this = _super.call(this) || this;
        console.log('TableSession.ctor');
        return _this;
    }
    /*created() {
        console.log('TableSession.created');
    }*/
    TableSession.prototype.mounted = function () {
        if (!this.session.speakerTitle) {
            return;
        }
        console.log('TableSession.mounted');
        this.popoverElement = this.$refs.popoverElement;
        var popoverContent = this.$refs.popoverContent;
        $(this.popoverElement)
            .popover({
            content: popoverContent,
            html: true,
            placement: 'bottom',
            template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body bg-dark"></div></div>'
        })
            .on('show.bs.popover', function () {
            $('.popover-element').popover('hide');
        });
    };
    TableSession.prototype.beforeDestroy = function () {
        console.log('TableSession.beforeDestroy');
        if (this.popoverElement) {
            $(this.popoverElement).popover('hide').popover('dispose');
        }
    };
    TableSession.prototype.selectSession = function () {
        this.timeslot.sessions.filter(function (x) { return x.isSelected; }).forEach(function (x) { return x.isSelected = false; });
        this.session.isSelected = true;
        TimeslotSelectionService.update(this.dayIndex, this.timeslot.id, this.sessionIndex);
    };
    TableSession.prototype.removeSession = function () {
        this.session.isSelected = false;
        TimeslotSelectionService.remove(this.dayIndex, this.timeslot.id);
    };
    TableSession.prototype.showSpeaker = function () {
        if (!this.timeslot.isSelectable || !this.session.speakerSlug) {
            return;
        }
        var url = "/api/speaker/" + this.session.speakerSlug;
        $.fancybox.open({
            src: url,
            type: 'iframe',
            scrolling: 'no'
        });
    };
    __decorate([
        Prop(),
        __metadata("design:type", Number)
    ], TableSession.prototype, "dayIndex", void 0);
    __decorate([
        Prop(),
        __metadata("design:type", Object)
    ], TableSession.prototype, "session", void 0);
    __decorate([
        Prop(),
        __metadata("design:type", Number)
    ], TableSession.prototype, "sessionIndex", void 0);
    __decorate([
        Prop(),
        __metadata("design:type", Object)
    ], TableSession.prototype, "timeslot", void 0);
    TableSession = __decorate([
        Component,
        __metadata("design:paramtypes", [])
    ], TableSession);
    return TableSession;
}(Vue));
export default TableSession;
//# sourceMappingURL=TableSession.js.map