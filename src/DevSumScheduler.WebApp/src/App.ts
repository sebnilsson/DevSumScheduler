declare const $: any;

import { Vue, Component } from "vue-property-decorator";

import ApiService from './ApiService';
import TimeslotSelectionService from './TimeslotSelectionService';
import IDay from './IDay';
import ITimeslot from './ITimeslot';

import Navigation from './Navigation.vue'
import Table from './Table.vue'

@Component({
	components: {
		Navigation,
		Table
	}
})
export default class App extends Vue {
	days: string[] = [];
	dayIndex: number = 0;
	day: IDay = this.getDefaultDay();
	isDataLoading: boolean = false;
	hasDataLoadFailed: boolean = false;

	private dayCache: any = {};

	mounted() {
		this.loadAllData();
	}

	loadAllData() {
		ApiService
			.getDays()
			.then(data => {
				this.days.splice(0, this.days.length);

				for (let d of data) {
					this.days.push(d);
				}
			});
		
		this.loadDay(0);
	}

	dayChange(index: number) {
		this.dayIndex = index;

		this.loadDay(index);

		$('.popover-element').popover('hide');
	}

	loadDay(index: number) {
		this.isDataLoading = true;

		this.$set(this, 'day', this.getDefaultDay());

		ApiService
			.getDay(index)
			.then((day: IDay) => {
				day.timeslots
					.filter(t => t.isSelectable)
					.forEach(t => {
						t.id = `${t.startsAt}|${t.endsAt}`;

						t.sessions.forEach(s => this.$set(s, 'isSelected', s.isSelected));
					});
				
				TimeslotSelectionService.apply(this.dayIndex, day.timeslots);

				this.$set(this, 'day', day);

				this.$forceUpdate();

				this.isDataLoading = false;
			})
			.catch(() => {
				this.hasDataLoadFailed = true;
				this.isDataLoading = false;
			});
	}

	private getDefaultDay() {
		return {
			title: '',
			locations: [],
			timeslots: new Array<ITimeslot>()
		};
	}
}