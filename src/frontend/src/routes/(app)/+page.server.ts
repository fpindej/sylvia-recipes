import type { PageServerLoad } from './$types';
import { getRecipes } from '$lib/api/recipes';
import type { RecipeFilters } from '$lib/types/recipe';

export const load: PageServerLoad = async ({ url, fetch }) => {
	const searchParams = url.searchParams;

	const filters: RecipeFilters = {
		searchTerm: searchParams.get('search') ?? undefined,
		isTried: searchParams.has('tried') ? searchParams.get('tried') === 'true' : undefined,
		timeCategory: searchParams.get('time') ?? undefined,
		workspaceNeeded: searchParams.get('workspace') ?? undefined,
		messiness: searchParams.get('messiness') ?? undefined,
		cuisines: searchParams.get('cuisines') ?? undefined,
		types: searchParams.get('types') ?? undefined,
		equipment: searchParams.get('equipment') ?? undefined,
		minProteinGrams: searchParams.has('minProtein') ? Number(searchParams.get('minProtein')) : undefined,
		pageNumber: searchParams.has('page') ? Number(searchParams.get('page')) : 1,
		pageSize: searchParams.has('pageSize') ? Number(searchParams.get('pageSize')) : 12
	};

	const recipesResponse = await getRecipes(filters, fetch, url.origin);

	return {
		recipes: recipesResponse.items,
		pagination: {
			pageNumber: recipesResponse.pageNumber,
			pageSize: recipesResponse.pageSize,
			totalCount: recipesResponse.totalCount,
			totalPages: recipesResponse.totalPages,
			hasPreviousPage: recipesResponse.hasPreviousPage,
			hasNextPage: recipesResponse.hasNextPage
		},
		filters
	};
};
