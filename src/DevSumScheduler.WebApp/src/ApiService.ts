import axios from 'axios';

import IDay from './IDay';

const dayCache: any = {};
const speakerCache: any = {};

export default abstract class ApiService {
	static getDays() : Promise<Array<string>> {
		return axios
			.get('/api/days')
			.then(response => {
				return response.data;
			});
	}

	static getDay(index: number) : Promise<IDay> {
		const url = `api/day/${index}`;

		const cachedDay = dayCache[url];
		if (cachedDay) {
			return Promise.resolve(cachedDay);
		}

		return axios
			.get(url)
			.then(response => {
				const day = response.data;

				dayCache[url] = day;

				return day;
			});
	}

	static getSpeaker(slug: string) {
		const url = `api/speaker/${slug}`;

		const cachedSpeaker = speakerCache[url];
		if (cachedSpeaker) {
			return Promise.resolve(cachedSpeaker);
		}

		return axios
			.get(url)
			.then(response => {
				const speaker = response.data;

				speakerCache[url] = speaker;

				return speaker;
			});
	}
}