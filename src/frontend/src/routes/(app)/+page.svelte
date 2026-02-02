<script lang="ts">
	import { goto } from '$app/navigation';
	import * as m from '$lib/paraglide/messages';
	import { RecipeCard } from '$lib/components/recipes';
	import { RecipeFilters } from '$lib/components/recipes';
	import { Button } from '$lib/components/ui/button';
	import { Plus, ChefHat } from 'lucide-svelte';
	import type { RecipeFilters as RecipeFiltersType } from '$lib/types/recipe';

	let { data } = $props();

	function handleFilterChange(filters: RecipeFiltersType) {
		const params = new URLSearchParams();

		if (filters.searchTerm) params.set('search', filters.searchTerm);
		if (filters.isTried !== undefined) params.set('tried', String(filters.isTried));
		if (filters.timeCategory !== undefined) params.set('time', String(filters.timeCategory));
		if (filters.workspaceNeeded !== undefined) params.set('workspace', String(filters.workspaceNeeded));
		if (filters.messiness !== undefined) params.set('messiness', String(filters.messiness));
		if (filters.cuisines) params.set('cuisines', filters.cuisines);
		if (filters.types) params.set('types', filters.types);
		if (filters.equipment) params.set('equipment', filters.equipment);
		if (filters.minProteinGrams !== undefined) params.set('minProtein', String(filters.minProteinGrams));
		if (filters.pageNumber && filters.pageNumber > 1) params.set('page', String(filters.pageNumber));

		const queryString = params.toString();
		goto(queryString ? `?${queryString}` : '/', { replaceState: true, noScroll: true });
	}

	function handleRecipeClick(recipeId: string) {
		goto(`/recipes/${recipeId}`);
	}

	function goToPage(pageNum: number) {
		handleFilterChange({ ...data.filters, pageNumber: pageNum });
	}
</script>

<svelte:head>
	<title>{m.meta_titleTemplate({ title: 'My Recipes' })}</title>
	<meta name="description" content="Browse and manage your recipe collection" />
</svelte:head>

<div class="container mx-auto space-y-6 p-6">
	<!-- Header -->
	<div class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
		<div>
			<h1 class="text-3xl font-bold tracking-tight">My Recipes</h1>
			<p class="text-muted-foreground">
				{data.pagination.totalCount} recipe{data.pagination.totalCount === 1 ? '' : 's'} in your collection
			</p>
		</div>
		<Button href="/recipes/new" class="gap-2">
			<Plus class="h-4 w-4" />
			Add Recipe
		</Button>
	</div>

	<!-- Filters -->
	<RecipeFilters filters={data.filters} onfilterchange={handleFilterChange} />

	<!-- Recipe grid -->
	{#if data.recipes.length > 0}
		<div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
			{#each data.recipes as recipe (recipe.id)}
				<RecipeCard {recipe} onclick={() => handleRecipeClick(recipe.id)} />
			{/each}
		</div>

		<!-- Pagination -->
		{#if data.pagination.totalPages > 1}
			<div class="flex items-center justify-center gap-2">
				<Button
					variant="outline"
					size="sm"
					disabled={!data.pagination.hasPreviousPage}
					onclick={() => goToPage(data.pagination.pageNumber - 1)}
				>
					Previous
				</Button>
				<span class="text-sm text-muted-foreground">
					Page {data.pagination.pageNumber} of {data.pagination.totalPages}
				</span>
				<Button
					variant="outline"
					size="sm"
					disabled={!data.pagination.hasNextPage}
					onclick={() => goToPage(data.pagination.pageNumber + 1)}
				>
					Next
				</Button>
			</div>
		{/if}
	{:else}
		<!-- Empty state -->
		<div class="flex flex-col items-center justify-center rounded-lg border border-dashed p-12 text-center">
			<ChefHat class="h-12 w-12 text-muted-foreground" />
			<h3 class="mt-4 text-lg font-semibold">No recipes found</h3>
			<p class="mt-2 text-sm text-muted-foreground">
				{#if data.filters.searchTerm || data.filters.isTried !== undefined || data.filters.timeCategory !== undefined}
					Try adjusting your filters to find what you're looking for.
				{:else}
					Get started by adding your first recipe.
				{/if}
			</p>
			{#if !data.filters.searchTerm && data.filters.isTried === undefined && data.filters.timeCategory === undefined}
				<Button href="/recipes/new" class="mt-4 gap-2">
					<Plus class="h-4 w-4" />
					Add your first recipe
				</Button>
			{/if}
		</div>
	{/if}
</div>
