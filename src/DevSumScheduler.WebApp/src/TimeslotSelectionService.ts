import ITimeslot from './ITimeslot';

const localStorageKeyPrefix : string = 'TimeslotSelctions';

export default abstract class TimeslotSelectionService {
	static apply(dayIndex: number, timeslots: Array<ITimeslot>) {
		const selections = this.getSelections(dayIndex);

		for (const key in selections) {
			if (selections.hasOwnProperty(key)) {
				const timeslot = timeslots.find(x => x.id === key);
				
				if (timeslot) {
					const value = selections[key];

					const session = timeslot.sessions[value];

					if (session) {
						session.isSelected = true;
					}
				}
			}
		}
	}

	static remove(dayIndex: number, sessionId: string) {
		const selections = this.getSelections(dayIndex);

		selections[sessionId] = undefined;

		this.saveSelections(dayIndex, selections);
	}

	static update(dayIndex: number, sessionId: string, index: number) {
		const selections = this.getSelections(dayIndex);

		selections[sessionId] = index;

		this.saveSelections(dayIndex, selections);
	}
	
	private static getSelections(dayIndex: number) : any {
		const localStorageKey = this.getLocalStorageKey(dayIndex);
		const stored : any = localStorage.getItem(localStorageKey);
		const selections = JSON.parse(stored);

		return selections || {};
	}

	private static saveSelections(dayIndex: number, selections: any) {
		const localStorageKey = this.getLocalStorageKey(dayIndex);
		const selectionJson = JSON.stringify(selections);

		localStorage.setItem(localStorageKey, selectionJson);
	}

	private static getLocalStorageKey(dayIndex: number) {
		return `${localStorageKeyPrefix}-${dayIndex}`
	}
}