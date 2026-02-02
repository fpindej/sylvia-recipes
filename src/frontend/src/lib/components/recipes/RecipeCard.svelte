<script lang="ts">
	import { Badge } from '$lib/components/ui/badge';
	import * as Card from '$lib/components/ui/card';
	import {
		type Recipe,
		timeCategoryLabels,
		workspaceNeededLabels
	} from '$lib/types/recipe';
	import { Clock, ChefHat, Zap, Check } from 'lucide-svelte';

	interface Props {
		recipe: Recipe;
		onclick?: () => void;
	}

	let { recipe, onclick }: Props = $props();

	const totalTime = $derived(
		(recipe.prepTimeMinutes ?? 0) + (recipe.cookTimeMinutes ?? 0) || null
	);

	// TagType is a string enum from backend: "Cuisine", "Type", "Custom"
	const cuisineTags = $derived((recipe.tags ?? []).filter((t) => t.tagType === 'Cuisine'));
	const typeTags = $derived((recipe.tags ?? []).filter((t) => t.tagType === 'Type'));
</script>

<Card.Root
	class="cursor-pointer transition-shadow hover:shadow-lg {recipe.isTried
		? 'border-green-200 bg-green-50/30 dark:border-green-900 dark:bg-green-950/20'
		: ''}"
	{onclick}
>
	{#if recipe.imageUrl}
		<div class="aspect-video w-full overflow-hidden rounded-t-lg">
			<img src={recipe.imageUrl} alt={recipe.title} class="h-full w-full object-cover" />
		</div>
	{/if}

	<Card.Header class="pb-2">
		<div class="flex items-start justify-between gap-2">
			<Card.Title class="line-clamp-2 text-lg">{recipe.title}</Card.Title>
			{#if recipe.isTried}
				<Badge variant="secondary" class="shrink-0 bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100">
					<Check class="mr-1 h-3 w-3" />
					Tried
				</Badge>
			{/if}
		</div>
		{#if recipe.description}
			<Card.Description class="line-clamp-2">{recipe.description}</Card.Description>
		{/if}
	</Card.Header>

	<Card.Content class="space-y-3 pb-4">
		<!-- Time and stats -->
		<div class="flex flex-wrap items-center gap-3 text-sm text-muted-foreground">
			{#if totalTime}
				<span class="flex items-center gap-1">
					<Clock class="h-4 w-4" />
					{totalTime} min
				</span>
			{/if}
			{#if recipe.proteinGrams}
				<span class="flex items-center gap-1">
					<Zap class="h-4 w-4" />
					{recipe.proteinGrams}g protein
				</span>
			{/if}
			{#if recipe.servings}
				<span class="flex items-center gap-1">
					<ChefHat class="h-4 w-4" />
					{recipe.servings} servings
				</span>
			{/if}
		</div>

		<!-- Category badges -->
		<div class="flex flex-wrap gap-1.5">
			{#if recipe.timeCategory !== null && recipe.timeCategory !== undefined}
				<Badge variant="outline" class="text-xs">
					{timeCategoryLabels[recipe.timeCategory]}
				</Badge>
			{/if}
			{#if recipe.workspaceNeeded !== null && recipe.workspaceNeeded !== undefined}
				<Badge variant="outline" class="text-xs">
					{workspaceNeededLabels[recipe.workspaceNeeded]} space
				</Badge>
			{/if}
		</div>

		<!-- Tags -->
		{#if cuisineTags.length > 0 || typeTags.length > 0}
			<div class="flex flex-wrap gap-1.5">
				{#each cuisineTags.slice(0, 3) as tag}
					<Badge variant="secondary" class="text-xs">{tag.name}</Badge>
				{/each}
				{#each typeTags.slice(0, 2) as tag}
					<Badge class="text-xs">{tag.name}</Badge>
				{/each}
				{#if cuisineTags.length + typeTags.length > 5}
					<Badge variant="outline" class="text-xs">+{cuisineTags.length + typeTags.length - 5}</Badge>
				{/if}
			</div>
		{/if}
	</Card.Content>
</Card.Root>
