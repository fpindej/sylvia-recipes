/**
 * Recipe-related types for the frontend.
 * Re-exports types from generated API and adds helper utilities.
 */

import type { components } from '$lib/api/v1';

// Re-export types from generated API
export type Recipe = components['schemas']['RecipeResponse'];
export type RecipeListResponse = components['schemas']['RecipeListResponse'];
export type CreateRecipeRequest = components['schemas']['CreateRecipeRequest'];
export type UpdateRecipeRequest = components['schemas']['UpdateRecipeRequest'];
export type Tag = components['schemas']['TagResponse'];
export type Equipment = components['schemas']['EquipmentResponse'];
export type TagRequest = components['schemas']['TagRequest'];

// Enum types (these match the backend enum values)
export type WorkspaceNeeded = components['schemas']['WorkspaceNeeded'];
export type TimeCategory = components['schemas']['TimeCategory'];
export type Messiness = components['schemas']['Messiness'];
export type TagType = components['schemas']['TagType']; // String enum: "Cuisine", "Type", "Custom"

// Enum values for use in code
// WorkspaceNeeded, TimeCategory, Messiness are numbers from backend
export const WorkspaceNeededValues = {
	Small: 0,
	Medium: 1,
	Large: 2
} as const;

export const TimeCategoryValues = {
	Quick: 0,
	Medium: 1,
	Long: 2,
	Overnight: 3
} as const;

export const MessinessValues = {
	Low: 0,
	Medium: 1,
	High: 2
} as const;

// TagType is a string enum from backend
export const TagTypeStrings = {
	Cuisine: 'Cuisine',
	Type: 'Type',
	Custom: 'Custom'
} as const;

export interface RecipeFilters {
	searchTerm?: string;
	isTried?: boolean;
	cuisines?: string;
	types?: string;
	equipment?: string;
	workspaceNeeded?: number;
	timeCategory?: number;
	messiness?: number;
	minProteinGrams?: number;
	pageNumber?: number;
	pageSize?: number;
}

// Helper functions for enum display
export const workspaceNeededLabels: Record<number, string> = {
	[WorkspaceNeededValues.Small]: 'Small',
	[WorkspaceNeededValues.Medium]: 'Medium',
	[WorkspaceNeededValues.Large]: 'Large'
};

export const timeCategoryLabels: Record<number, string> = {
	[TimeCategoryValues.Quick]: 'Quick (≤30 min)',
	[TimeCategoryValues.Medium]: 'Medium (1-3 hrs)',
	[TimeCategoryValues.Long]: 'Long (3+ hrs)',
	[TimeCategoryValues.Overnight]: 'Overnight'
};

export const messinessLabels: Record<number, string> = {
	[MessinessValues.Low]: 'Low',
	[MessinessValues.Medium]: 'Medium',
	[MessinessValues.High]: 'High'
};

export const tagTypeLabels: Record<string, string> = {
	Cuisine: 'Cuisine',
	Type: 'Type',
	Custom: 'Custom'
};
