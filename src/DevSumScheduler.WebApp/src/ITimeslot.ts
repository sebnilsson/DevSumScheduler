import ISession from './ISession';

export default interface ITimeslot {
	id: string;
	endsAt: string;
	isSelectable: boolean;
	sessions: Array<ISession>;
	startsAt: string;
}