<script lang="ts">
	import RecipeForm from '$lib/components/recipes/RecipeForm.svelte';
	import type { PageData } from './$types';

	let { data }: { data: PageData } = $props();
</script>

<svelte:head>
	<title>Edit {data.recipe.title} - Recipes</title>
</svelte:head>

<RecipeForm mode="edit" recipe={data.recipe} />
