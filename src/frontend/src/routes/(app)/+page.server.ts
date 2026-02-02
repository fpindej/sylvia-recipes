import type { PageServerLoad } from './$types';
import { getRecipes } from '$lib/api/recipes';
import type { RecipeFilters, TimeCategory, WorkspaceNeeded, Messiness } from '$lib/types/recipe';

// Helper functions to validate URL params against valid enum values
function parseTimeCategory(value: string | null): TimeCategory | undefined {
	if (value === 'Quick' || value === 'Medium' || value === 'Long' || value === 'Overnight') {
		return value;
	}
	return undefined;
}

function parseWorkspaceNeeded(value: string | null): WorkspaceNeeded | undefined {
	if (value === 'Small' || value === 'Medium' || value === 'Large') {
		return value;
	}
	return undefined;
}

function parseMessiness(value: string | null): Messiness | undefined {
	if (value === 'Low' || value === 'Medium' || value === 'High') {
		return value;
	}
	return undefined;
}

export const load: PageServerLoad = async ({ url, fetch }) => {
	const searchParams = url.searchParams;

	const filters: RecipeFilters = {
		searchTerm: searchParams.get('search') ?? undefined,
		isTried: searchParams.has('tried') ? searchParams.get('tried') === 'true' : undefined,
		timeCategory: parseTimeCategory(searchParams.get('time')),
		workspaceNeeded: parseWorkspaceNeeded(searchParams.get('workspace')),
		messiness: parseMessiness(searchParams.get('messiness')),
		cuisines: searchParams.get('cuisines') ?? undefined,
		types: searchParams.get('types') ?? undefined,
		equipment: searchParams.get('equipment') ?? undefined,
		minProteinGrams: searchParams.has('minProtein')
			? Number(searchParams.get('minProtein'))
			: undefined,
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
