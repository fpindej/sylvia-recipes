import type { PageServerLoad } from './$types';

export const load: PageServerLoad = async () => {
	// No data needed for the create page
	// Tags and equipment are fetched client-side for autocomplete
	return {};
};
