import { Vue, Component, Prop } from "vue-property-decorator";

import ITimeslot from './ITimeslot';

import TableSession from './TableSession.vue'

@Component({
	components: {
		TableSession
	}
})
export default class TableTimeslot extends Vue {
	@Prop()
	dayIndex: number;
	@Prop()
	timeslot: ITimeslot;
}