<script lang="ts">
	import { goto, invalidateAll } from '$app/navigation';
	import { resolve } from '$app/paths';
	import * as m from '$lib/paraglide/messages';
	import { Badge } from '$lib/components/ui/badge';
	import { Button } from '$lib/components/ui/button';
	import * as Card from '$lib/components/ui/card';
	import { timeCategoryLabels, workspaceNeededLabels, messinessLabels } from '$lib/types/recipe';
	import { markRecipeAsTried, markRecipeAsNotTried, deleteRecipe } from '$lib/api/recipes';
	import {
		ArrowLeft,
		Clock,
		ChefHat,
		Zap,
		Check,
		Edit,
		Trash2,
		ExternalLink,
		Users,
		Scale,
		Utensils
	} from 'lucide-svelte';
	import { toast } from '$lib/components/ui/sonner';

	let { data } = $props();

	let isLoading = $state(false);

	const recipe = $derived(data.recipe);

	const totalTime = $derived((recipe.prepTimeMinutes ?? 0) + (recipe.cookTimeMinutes ?? 0) || null);

	const cuisineTags = $derived((recipe.tags ?? []).filter((t) => t.tagType === 'Cuisine'));
	const typeTags = $derived((recipe.tags ?? []).filter((t) => t.tagType === 'Type'));
	const customTags = $derived((recipe.tags ?? []).filter((t) => t.tagType === 'Custom'));

	async function handleToggleTried() {
		if (!recipe.id) return;
		isLoading = true;
		try {
			if (recipe.isTried) {
				await markRecipeAsNotTried(recipe.id);
				toast.success('Recipe marked as not tried');
			} else {
				await markRecipeAsTried(recipe.id);
				toast.success('Recipe marked as tried!');
			}
			await invalidateAll();
		} catch {
			toast.error('Failed to update recipe status');
		} finally {
			isLoading = false;
		}
	}

	async function handleDelete() {
		if (!recipe.id) return;
		if (!confirm('Are you sure you want to delete this recipe?')) return;

		isLoading = true;
		try {
			await deleteRecipe(recipe.id);
			toast.success('Recipe deleted');
			goto(resolve('/'));
		} catch {
			toast.error('Failed to delete recipe');
		} finally {
			isLoading = false;
		}
	}

	function formatTime(minutes: number): string {
		if (minutes < 60) return `${minutes} min`;
		const hours = Math.floor(minutes / 60);
		const mins = minutes % 60;
		return mins > 0 ? `${hours}h ${mins}m` : `${hours}h`;
	}
</script>

<svelte:head>
	<title>{m.meta_titleTemplate({ title: recipe.title ?? 'Recipe' })}</title>
	<meta name="description" content={recipe.description ?? 'View recipe details'} />
</svelte:head>

<div class="container mx-auto max-w-4xl space-y-6 p-6">
	<!-- Back button and actions -->
	<div class="flex items-center justify-between">
		<Button variant="ghost" onclick={() => goto(resolve('/'))} class="gap-2">
			<ArrowLeft class="h-4 w-4" />
			Back to recipes
		</Button>
		<div class="flex gap-2">
			<Button variant="outline" href={resolve(`/recipes/${recipe.id}/edit`)} class="gap-2">
				<Edit class="h-4 w-4" />
				Edit
			</Button>
			<Button variant="destructive" onclick={handleDelete} disabled={isLoading} class="gap-2">
				<Trash2 class="h-4 w-4" />
				Delete
			</Button>
		</div>
	</div>

	<!-- Main content -->
	<div class="space-y-6">
		<!-- Header with image -->
		{#if recipe.imageUrl}
			<div class="aspect-video w-full overflow-hidden rounded-lg">
				<img src={recipe.imageUrl} alt={recipe.title} class="h-full w-full object-cover" />
			</div>
		{/if}

		<!-- Title and status -->
		<div class="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
			<div class="space-y-2">
				<h1 class="text-3xl font-bold tracking-tight">{recipe.title}</h1>
				{#if recipe.description}
					<p class="text-lg text-muted-foreground">{recipe.description}</p>
				{/if}
			</div>
			<Button
				variant={recipe.isTried ? 'secondary' : 'default'}
				onclick={handleToggleTried}
				disabled={isLoading}
				class="shrink-0 gap-2 {recipe.isTried
					? 'bg-green-100 text-green-800 hover:bg-green-200 dark:bg-green-900 dark:text-green-100'
					: ''}"
			>
				{#if recipe.isTried}
					<Check class="h-4 w-4" />
					Tried
				{:else}
					<ChefHat class="h-4 w-4" />
					Mark as Tried
				{/if}
			</Button>
		</div>

		<!-- Quick info cards -->
		<div class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
			{#if totalTime}
				<Card.Root>
					<Card.Content class="flex items-center gap-3 p-4">
						<Clock class="h-5 w-5 text-muted-foreground" />
						<div>
							<p class="text-sm text-muted-foreground">Total Time</p>
							<p class="font-medium">{formatTime(totalTime)}</p>
						</div>
					</Card.Content>
				</Card.Root>
			{/if}

			{#if recipe.servings}
				<Card.Root>
					<Card.Content class="flex items-center gap-3 p-4">
						<Users class="h-5 w-5 text-muted-foreground" />
						<div>
							<p class="text-sm text-muted-foreground">Servings</p>
							<p class="font-medium">{recipe.servings}</p>
						</div>
					</Card.Content>
				</Card.Root>
			{/if}

			{#if recipe.proteinGrams}
				<Card.Root>
					<Card.Content class="flex items-center gap-3 p-4">
						<Scale class="h-5 w-5 text-muted-foreground" />
						<div>
							<p class="text-sm text-muted-foreground">Protein</p>
							<p class="font-medium">{recipe.proteinGrams}g</p>
						</div>
					</Card.Content>
				</Card.Root>
			{/if}

			{#if recipe.timeCategory !== undefined && recipe.timeCategory !== null}
				<Card.Root>
					<Card.Content class="flex items-center gap-3 p-4">
						<Zap class="h-5 w-5 text-muted-foreground" />
						<div>
							<p class="text-sm text-muted-foreground">Time Category</p>
							<p class="font-medium">{timeCategoryLabels[recipe.timeCategory]}</p>
						</div>
					</Card.Content>
				</Card.Root>
			{/if}
		</div>

		<!-- Time breakdown -->
		{#if recipe.prepTimeMinutes || recipe.cookTimeMinutes}
			<Card.Root>
				<Card.Header>
					<Card.Title class="text-lg">Time Breakdown</Card.Title>
				</Card.Header>
				<Card.Content class="flex gap-8">
					{#if recipe.prepTimeMinutes}
						<div>
							<p class="text-sm text-muted-foreground">Prep Time</p>
							<p class="text-xl font-medium">{formatTime(recipe.prepTimeMinutes)}</p>
						</div>
					{/if}
					{#if recipe.cookTimeMinutes}
						<div>
							<p class="text-sm text-muted-foreground">Cook Time</p>
							<p class="text-xl font-medium">{formatTime(recipe.cookTimeMinutes)}</p>
						</div>
					{/if}
				</Card.Content>
			</Card.Root>
		{/if}

		<!-- Tags -->
		{#if cuisineTags.length > 0 || typeTags.length > 0 || customTags.length > 0}
			<Card.Root>
				<Card.Header>
					<Card.Title class="text-lg">Tags</Card.Title>
				</Card.Header>
				<Card.Content class="space-y-3">
					{#if cuisineTags.length > 0}
						<div class="flex flex-wrap items-center gap-2">
							<span class="text-sm text-muted-foreground">Cuisine:</span>
							{#each cuisineTags as tag (tag.name)}
								<Badge variant="secondary">{tag.name}</Badge>
							{/each}
						</div>
					{/if}
					{#if typeTags.length > 0}
						<div class="flex flex-wrap items-center gap-2">
							<span class="text-sm text-muted-foreground">Type:</span>
							{#each typeTags as tag (tag.name)}
								<Badge>{tag.name}</Badge>
							{/each}
						</div>
					{/if}
					{#if customTags.length > 0}
						<div class="flex flex-wrap items-center gap-2">
							<span class="text-sm text-muted-foreground">Custom:</span>
							{#each customTags as tag (tag.name)}
								<Badge variant="outline">{tag.name}</Badge>
							{/each}
						</div>
					{/if}
				</Card.Content>
			</Card.Root>
		{/if}

		<!-- Equipment -->
		{#if recipe.equipment && recipe.equipment.length > 0}
			<Card.Root>
				<Card.Header>
					<Card.Title class="flex items-center gap-2 text-lg">
						<Utensils class="h-5 w-5" />
						Equipment Needed
					</Card.Title>
				</Card.Header>
				<Card.Content>
					<div class="flex flex-wrap gap-2">
						{#each recipe.equipment as item (item.name)}
							<Badge variant="outline">{item.name}</Badge>
						{/each}
					</div>
				</Card.Content>
			</Card.Root>
		{/if}

		<!-- Additional info -->
		{#if (recipe.workspaceNeeded !== undefined && recipe.workspaceNeeded !== null) || (recipe.messiness !== undefined && recipe.messiness !== null)}
			<Card.Root>
				<Card.Header>
					<Card.Title class="text-lg">Additional Info</Card.Title>
				</Card.Header>
				<Card.Content class="flex gap-8">
					{#if recipe.workspaceNeeded !== undefined && recipe.workspaceNeeded !== null}
						<div>
							<p class="text-sm text-muted-foreground">Workspace Needed</p>
							<p class="font-medium">{workspaceNeededLabels[recipe.workspaceNeeded]}</p>
						</div>
					{/if}
					{#if recipe.messiness !== undefined && recipe.messiness !== null}
						<div>
							<p class="text-sm text-muted-foreground">Messiness</p>
							<p class="font-medium">{messinessLabels[recipe.messiness]}</p>
						</div>
					{/if}
				</Card.Content>
			</Card.Root>
		{/if}

		<!-- Instructions -->
		<Card.Root>
			<Card.Header>
				<Card.Title class="text-lg">Instructions</Card.Title>
			</Card.Header>
			<Card.Content>
				<div class="prose prose-sm dark:prose-invert max-w-none whitespace-pre-wrap">
					{recipe.instructions}
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Notes -->
		{#if recipe.notes}
			<Card.Root>
				<Card.Header>
					<Card.Title class="text-lg">Personal Notes</Card.Title>
				</Card.Header>
				<Card.Content>
					<div class="prose prose-sm dark:prose-invert max-w-none whitespace-pre-wrap">
						{recipe.notes}
					</div>
				</Card.Content>
			</Card.Root>
		{/if}

		<!-- Source URL (external link, not internal navigation) -->
		{#if recipe.sourceUrl}
			<Card.Root>
				<Card.Content class="flex items-center gap-2 p-4">
					<ExternalLink class="h-4 w-4 text-muted-foreground" />
					{@const externalUrl = recipe.sourceUrl}
					<span
						role="link"
						tabindex="0"
						class="cursor-pointer text-sm text-primary hover:underline"
						onclick={() => window.open(externalUrl, '_blank', 'noopener,noreferrer')}
						onkeydown={(e) =>
							e.key === 'Enter' && window.open(externalUrl, '_blank', 'noopener,noreferrer')}
					>
						View original source
					</span>
				</Card.Content>
			</Card.Root>
		{/if}
	</div>
</div>
