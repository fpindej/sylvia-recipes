import { createApiClient } from './client';
import type { components } from './v1';
import type { RecipeFilters } from '$lib/types/recipe';

// Re-export types from generated API for convenience
export type Recipe = components['schemas']['RecipeResponse'];
export type RecipeListResponse = components['schemas']['RecipeListResponse'];
export type CreateRecipeRequest = components['schemas']['CreateRecipeRequest'];
export type UpdateRecipeRequest = components['schemas']['UpdateRecipeRequest'];
export type Tag = components['schemas']['TagResponse'];
export type Equipment = components['schemas']['EquipmentResponse'];

// Re-export RecipeFilters for backward compatibility
export type { RecipeFilters } from '$lib/types/recipe';

/**
 * Fetches a paginated and filtered list of recipes.
 */
export async function getRecipes(
	filters: RecipeFilters = {},
	customFetch?: typeof fetch,
	baseUrl?: string
): Promise<RecipeListResponse> {
	const client = createApiClient(customFetch, baseUrl);

	const { data, error, response } = await client.GET('/api/v1/recipes', {
		params: {
			query: {
				SearchTerm: filters.searchTerm,
				IsTried: filters.isTried,
				Cuisines: filters.cuisines,
				Types: filters.types,
				Equipment: filters.equipment,
				WorkspaceNeeded: filters.workspaceNeeded,
				TimeCategory: filters.timeCategory,
				Messiness: filters.messiness,
				MinProteinGrams: filters.minProteinGrams,
				PageNumber: filters.pageNumber,
				PageSize: filters.pageSize
			}
		}
	});

	if (!response.ok || error) {
		throw new Error(`Failed to fetch recipes: ${response.statusText}`);
	}

	return (
		data ?? {
			items: [],
			totalCount: 0,
			pageNumber: 1,
			pageSize: 12,
			totalPages: 0,
			hasPreviousPage: false,
			hasNextPage: false
		}
	);
}

/**
 * Fetches a single recipe by ID.
 */
export async function getRecipe(
	id: string,
	customFetch?: typeof fetch,
	origin?: string
): Promise<Recipe> {
	const client = createApiClient(customFetch, origin);

	const { data, error, response } = await client.GET('/api/v1/recipes/{id}', {
		params: { path: { id } }
	});

	if (!response.ok || error) {
		throw new Error(`Failed to fetch recipe: ${response.statusText}`);
	}

	return data!;
}

/**
 * Creates a new recipe.
 */
export async function createRecipe(
	request: CreateRecipeRequest,
	customFetch?: typeof fetch
): Promise<string> {
	const client = createApiClient(customFetch);

	const { data, error, response } = await client.POST('/api/v1/recipes', {
		body: request
	});

	if (!response.ok || error) {
		throw new Error(`Failed to create recipe: ${response.statusText}`);
	}

	return data!;
}

/**
 * Updates an existing recipe.
 */
export async function updateRecipe(
	id: string,
	request: UpdateRecipeRequest,
	customFetch?: typeof fetch
): Promise<void> {
	const client = createApiClient(customFetch);

	const { error, response } = await client.PUT('/api/v1/recipes/{id}', {
		params: { path: { id } },
		body: request
	});

	if (!response.ok || error) {
		throw new Error(`Failed to update recipe: ${response.statusText}`);
	}
}

/**
 * Deletes a recipe (soft delete).
 */
export async function deleteRecipe(id: string, customFetch?: typeof fetch): Promise<void> {
	const client = createApiClient(customFetch);

	const { error, response } = await client.DELETE('/api/v1/recipes/{id}', {
		params: { path: { id } }
	});

	if (!response.ok || error) {
		throw new Error(`Failed to delete recipe: ${response.statusText}`);
	}
}

/**
 * Marks a recipe as tried.
 */
export async function markRecipeAsTried(id: string, customFetch?: typeof fetch): Promise<void> {
	const client = createApiClient(customFetch);

	const { error, response } = await client.POST('/api/v1/recipes/{id}/tried', {
		params: { path: { id } }
	});

	if (!response.ok || error) {
		throw new Error(`Failed to mark recipe as tried: ${response.statusText}`);
	}
}

/**
 * Marks a recipe as not tried.
 */
export async function markRecipeAsNotTried(id: string, customFetch?: typeof fetch): Promise<void> {
	const client = createApiClient(customFetch);

	const { error, response } = await client.DELETE('/api/v1/recipes/{id}/tried', {
		params: { path: { id } }
	});

	if (!response.ok || error) {
		throw new Error(`Failed to mark recipe as not tried: ${response.statusText}`);
	}
}

/**
 * Fetches all tags for autocomplete.
 */
export async function getTags(customFetch?: typeof fetch): Promise<Tag[]> {
	const client = createApiClient(customFetch);

	const { data, error, response } = await client.GET('/api/v1/recipes/tags');

	if (!response.ok || error) {
		throw new Error(`Failed to fetch tags: ${response.statusText}`);
	}

	return data ?? [];
}

/**
 * Fetches all equipment for autocomplete.
 */
export async function getEquipment(customFetch?: typeof fetch): Promise<Equipment[]> {
	const client = createApiClient(customFetch);

	const { data, error, response } = await client.GET('/api/v1/recipes/equipment');

	if (!response.ok || error) {
		throw new Error(`Failed to fetch equipment: ${response.statusText}`);
	}

	return data ?? [];
}
