export default interface ISession {
	endsAt: Date;
	isSelected: boolean,
	location: string;
	speakerSlug: string;
	speakerTitle: string;
	startsAt: Date;
	title: string;
}