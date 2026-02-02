import { createApiClient } from './client';
import type {
	Recipe,
	RecipeListResponse,
	CreateRecipeRequest,
	UpdateRecipeRequest,
	RecipeFilters,
	Tag,
	Equipment
} from '$lib/types/recipe';

const API_BASE = '/api/v1/recipes';

/**
 * Fetches a paginated and filtered list of recipes.
 */
export async function getRecipes(
	filters: RecipeFilters = {},
	customFetch?: typeof fetch
): Promise<RecipeListResponse> {
	const client = createApiClient(customFetch);
	const params = new URLSearchParams();

	if (filters.searchTerm) params.set('searchTerm', filters.searchTerm);
	if (filters.isTried !== undefined) params.set('isTried', String(filters.isTried));
	if (filters.cuisines) params.set('cuisines', filters.cuisines);
	if (filters.types) params.set('types', filters.types);
	if (filters.equipment) params.set('equipment', filters.equipment);
	if (filters.workspaceNeeded !== undefined)
		params.set('workspaceNeeded', String(filters.workspaceNeeded));
	if (filters.timeCategory !== undefined) params.set('timeCategory', String(filters.timeCategory));
	if (filters.messiness !== undefined) params.set('messiness', String(filters.messiness));
	if (filters.minProteinGrams !== undefined)
		params.set('minProteinGrams', String(filters.minProteinGrams));
	if (filters.pageNumber) params.set('pageNumber', String(filters.pageNumber));
	if (filters.pageSize) params.set('pageSize', String(filters.pageSize));

	const url = `${API_BASE}?${params.toString()}`;
	const response = await (customFetch || fetch)(url, {
		method: 'GET',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to fetch recipes: ${response.statusText}`);
	}

	return response.json();
}

/**
 * Fetches a single recipe by ID.
 */
export async function getRecipe(id: string, customFetch?: typeof fetch): Promise<Recipe> {
	const response = await (customFetch || fetch)(`${API_BASE}/${id}`, {
		method: 'GET',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to fetch recipe: ${response.statusText}`);
	}

	return response.json();
}

/**
 * Creates a new recipe.
 */
export async function createRecipe(
	data: CreateRecipeRequest,
	customFetch?: typeof fetch
): Promise<string> {
	const response = await (customFetch || fetch)(API_BASE, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		credentials: 'include',
		body: JSON.stringify(data)
	});

	if (!response.ok) {
		const error = await response.text();
		throw new Error(`Failed to create recipe: ${error}`);
	}

	return response.json();
}

/**
 * Updates an existing recipe.
 */
export async function updateRecipe(
	id: string,
	data: UpdateRecipeRequest,
	customFetch?: typeof fetch
): Promise<void> {
	const response = await (customFetch || fetch)(`${API_BASE}/${id}`, {
		method: 'PUT',
		headers: { 'Content-Type': 'application/json' },
		credentials: 'include',
		body: JSON.stringify(data)
	});

	if (!response.ok) {
		const error = await response.text();
		throw new Error(`Failed to update recipe: ${error}`);
	}
}

/**
 * Deletes a recipe (soft delete).
 */
export async function deleteRecipe(id: string, customFetch?: typeof fetch): Promise<void> {
	const response = await (customFetch || fetch)(`${API_BASE}/${id}`, {
		method: 'DELETE',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to delete recipe: ${response.statusText}`);
	}
}

/**
 * Marks a recipe as tried.
 */
export async function markRecipeAsTried(id: string, customFetch?: typeof fetch): Promise<void> {
	const response = await (customFetch || fetch)(`${API_BASE}/${id}/tried`, {
		method: 'POST',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to mark recipe as tried: ${response.statusText}`);
	}
}

/**
 * Marks a recipe as not tried.
 */
export async function markRecipeAsNotTried(id: string, customFetch?: typeof fetch): Promise<void> {
	const response = await (customFetch || fetch)(`${API_BASE}/${id}/tried`, {
		method: 'DELETE',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to mark recipe as not tried: ${response.statusText}`);
	}
}

/**
 * Fetches all tags for autocomplete.
 */
export async function getTags(searchTerm?: string, customFetch?: typeof fetch): Promise<Tag[]> {
	const params = searchTerm ? `?searchTerm=${encodeURIComponent(searchTerm)}` : '';
	const response = await (customFetch || fetch)(`${API_BASE}/tags${params}`, {
		method: 'GET',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to fetch tags: ${response.statusText}`);
	}

	return response.json();
}

/**
 * Fetches all equipment for autocomplete.
 */
export async function getEquipment(
	searchTerm?: string,
	customFetch?: typeof fetch
): Promise<Equipment[]> {
	const params = searchTerm ? `?searchTerm=${encodeURIComponent(searchTerm)}` : '';
	const response = await (customFetch || fetch)(`${API_BASE}/equipment${params}`, {
		method: 'GET',
		credentials: 'include'
	});

	if (!response.ok) {
		throw new Error(`Failed to fetch equipment: ${response.statusText}`);
	}

	return response.json();
}
