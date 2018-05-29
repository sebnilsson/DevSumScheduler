declare const $: any;

import { Vue, Component, Prop } from "vue-property-decorator";

import TimeslotSelectionService from './TimeslotSelectionService';

import ISession from './ISession';
import ITimeslot from './ITimeslot';

@Component
export default class TableSession extends Vue {
	@Prop()
	dayIndex: number;
	@Prop()
	session: ISession;
	@Prop()
	sessionIndex: number;
	@Prop()
	timeslot: ITimeslot;
	
	private popoverElement : any;
	
	mounted() {
		if (!this.session.speakerTitle) {
			return;
		}
		
		this.popoverElement = this.$refs.popoverElement;
		const popoverContent: any = this.$refs.popoverContent;
		
		$(this.popoverElement)
			.popover({
				content: popoverContent,
				html: true,
				placement: 'bottom',
				template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body bg-dark"></div></div>'
			})
			.on('show.bs.popover', () => {
				$('.popover-element').popover('hide');
			});
	}

	beforeDestroy() {
		if (this.popoverElement) {
			$(this.popoverElement).popover('hide').popover('dispose');
		}
	}
	
	selectSession() {
		$(this.popoverElement).popover('hide');

		this.timeslot.sessions.filter(x => x.isSelected).forEach(x => x.isSelected = false);

		this.session.isSelected = true;

		TimeslotSelectionService.update(this.dayIndex, this.timeslot.id, this.sessionIndex);
	}

	removeSession() {
		$(this.popoverElement).popover('hide');

		this.session.isSelected = false;

		TimeslotSelectionService.remove(this.dayIndex, this.timeslot.id);
	}

	showSpeaker() {
		$(this.popoverElement).popover('hide');

		if (!this.timeslot.isSelectable || !this.session.speakerSlug) {
			return;
		}
		
		const url = `/api/speaker/${this.session.speakerSlug}`;

		$.fancybox.open({
			src: url,
			type: 'iframe',
			scrolling: 'no'
		});
	}
}