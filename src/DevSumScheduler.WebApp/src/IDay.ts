import ITimeslot from './ITimeslot';

export default interface IDay {
	title: string;
	locations: string[];
	timeslots: Array<ITimeslot>;
}