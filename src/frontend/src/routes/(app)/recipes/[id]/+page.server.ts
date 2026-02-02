import type { PageServerLoad } from './$types';
import { getRecipe } from '$lib/api/recipes';
import { error } from '@sveltejs/kit';

export const load: PageServerLoad = async ({ params, fetch, url }) => {
	try {
		const recipe = await getRecipe(params.id, fetch, url.origin);
		return { recipe };
	} catch {
		throw error(404, 'Recipe not found');
	}
};
