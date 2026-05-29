import { redirect } from '@sveltejs/kit';
import type { PageLoad } from './$types';

const validViews = ['surveys', 'sessions', 'templates'] as const;
type View = (typeof validViews)[number];

export const load: PageLoad = ({ url }) => {
	const view = url.searchParams.get('view')?.toLowerCase();

	if (!view || !(validViews as readonly string[]).includes(view)) {
		throw redirect(302, `${url.pathname}?view=${validViews[0]}`);
	}

	return {
		currentView: view as View,
	};
};
