/**
 * Recipe-related types for the frontend.
 * These match the backend API response schemas.
 */

export enum WorkspaceNeeded {
	Small = 0,
	Medium = 1,
	Large = 2
}

export enum TimeCategory {
	Quick = 0,
	Medium = 1,
	Long = 2,
	Overnight = 3
}

export enum Messiness {
	Low = 0,
	Medium = 1,
	High = 2
}

export enum TagType {
	Cuisine = 0,
	Type = 1,
	Custom = 2
}

export interface Tag {
	id: string;
	name: string;
	tagType: TagType;
}

export interface Equipment {
	id: string;
	name: string;
}

export interface Recipe {
	id: string;
	title: string;
	instructions: string;
	description?: string | null;
	prepTimeMinutes?: number | null;
	cookTimeMinutes?: number | null;
	servings?: number | null;
	proteinGrams?: number | null;
	isTried: boolean;
	sourceUrl?: string | null;
	imageUrl?: string | null;
	notes?: string | null;
	workspaceNeeded?: WorkspaceNeeded | null;
	timeCategory?: TimeCategory | null;
	messiness?: Messiness | null;
	tags: Tag[];
	equipment: Equipment[];
	createdAt: string;
	updatedAt?: string | null;
}

export interface RecipeListResponse {
	items: Recipe[];
	totalCount: number;
	pageNumber: number;
	pageSize: number;
	totalPages: number;
	hasPreviousPage: boolean;
	hasNextPage: boolean;
}

export interface CreateRecipeRequest {
	title: string;
	instructions: string;
	description?: string | null;
	prepTimeMinutes?: number | null;
	cookTimeMinutes?: number | null;
	servings?: number | null;
	proteinGrams?: number | null;
	isTried?: boolean;
	sourceUrl?: string | null;
	imageUrl?: string | null;
	notes?: string | null;
	workspaceNeeded?: WorkspaceNeeded | null;
	timeCategory?: TimeCategory | null;
	messiness?: Messiness | null;
	tags?: { name: string; tagType: TagType }[] | null;
	equipmentNames?: string[] | null;
}

export interface UpdateRecipeRequest {
	title?: string | null;
	instructions?: string | null;
	description?: string | null;
	prepTimeMinutes?: number | null;
	cookTimeMinutes?: number | null;
	servings?: number | null;
	proteinGrams?: number | null;
	isTried?: boolean | null;
	sourceUrl?: string | null;
	imageUrl?: string | null;
	notes?: string | null;
	workspaceNeeded?: WorkspaceNeeded | null;
	timeCategory?: TimeCategory | null;
	messiness?: Messiness | null;
	tags?: { name: string; tagType: TagType }[] | null;
	equipmentNames?: string[] | null;
}

export interface RecipeFilters {
	searchTerm?: string;
	isTried?: boolean;
	cuisines?: string;
	types?: string;
	equipment?: string;
	workspaceNeeded?: WorkspaceNeeded;
	timeCategory?: TimeCategory;
	messiness?: Messiness;
	minProteinGrams?: number;
	pageNumber?: number;
	pageSize?: number;
}

// Helper functions for enum display
export const workspaceNeededLabels: Record<WorkspaceNeeded, string> = {
	[WorkspaceNeeded.Small]: 'Small',
	[WorkspaceNeeded.Medium]: 'Medium',
	[WorkspaceNeeded.Large]: 'Large'
};

export const timeCategoryLabels: Record<TimeCategory, string> = {
	[TimeCategory.Quick]: 'Quick (≤30 min)',
	[TimeCategory.Medium]: 'Medium (1-3 hrs)',
	[TimeCategory.Long]: 'Long (3+ hrs)',
	[TimeCategory.Overnight]: 'Overnight'
};

export const messinessLabels: Record<Messiness, string> = {
	[Messiness.Low]: 'Low',
	[Messiness.Medium]: 'Medium',
	[Messiness.High]: 'High'
};

export const tagTypeLabels: Record<TagType, string> = {
	[TagType.Cuisine]: 'Cuisine',
	[TagType.Type]: 'Type',
	[TagType.Custom]: 'Custom'
};
