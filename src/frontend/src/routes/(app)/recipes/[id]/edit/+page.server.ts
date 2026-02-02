import { error } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';
import { getRecipe } from '$lib/api/recipes';

export const load: PageServerLoad = async ({ params, fetch, url }) => {
	const recipe = await getRecipe(params.id, fetch, url.origin);

	if (!recipe) {
		error(404, 'Recipe not found');
	}

	return {
		recipe
	};
};
