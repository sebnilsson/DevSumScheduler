import { Vue, Component, Prop } from "vue-property-decorator";

import TableTimeslot from './TableTimeslot.vue'

@Component({
	components: {
		TableTimeslot
	}
})
export default class Table extends Vue {
	@Prop()
	day: any;
	@Prop()
	dayIndex: number;

	get columnWidth() {
		var part = Math.floor(100 / this.day.locations.length) || 100;
		return `${part}%`;
	}
}