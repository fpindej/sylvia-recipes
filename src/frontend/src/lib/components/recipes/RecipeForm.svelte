<script lang="ts">
	import { goto } from '$app/navigation';
	import { Input } from '$lib/components/ui/input';
	import { Button } from '$lib/components/ui/button';
	import { Label } from '$lib/components/ui/label';
	import { Textarea } from '$lib/components/ui/textarea';
	import * as Card from '$lib/components/ui/card';
	import { Badge } from '$lib/components/ui/badge';
	import {
		type Recipe,
		type CreateRecipeRequest,
		type UpdateRecipeRequest,
		workspaceNeededLabels,
		timeCategoryLabels,
		messinessLabels
	} from '$lib/types/recipe';
	import { createRecipe, updateRecipe, getTags, getEquipment } from '$lib/api/recipes';
	import { ArrowLeft, Plus, X, Loader2 } from 'lucide-svelte';
	import { toast } from '$lib/components/ui/sonner';
	import { onMount } from 'svelte';

	interface Props {
		recipe?: Recipe;
		mode: 'create' | 'edit';
	}

	let { recipe, mode }: Props = $props();

	// Store initial values from recipe prop
	const initialValues = {
		title: recipe?.title ?? '',
		description: recipe?.description ?? '',
		instructions: recipe?.instructions ?? '',
		prepTimeMinutes: recipe?.prepTimeMinutes ?? null,
		cookTimeMinutes: recipe?.cookTimeMinutes ?? null,
		servings: recipe?.servings ?? null,
		proteinGrams: recipe?.proteinGrams ?? null,
		isTried: recipe?.isTried ?? false,
		sourceUrl: recipe?.sourceUrl ?? '',
		imageUrl: recipe?.imageUrl ?? '',
		notes: recipe?.notes ?? '',
		workspaceNeeded: recipe?.workspaceNeeded ?? null,
		timeCategory: recipe?.timeCategory ?? null,
		messiness: recipe?.messiness ?? null,
		cuisineTags: (recipe?.tags ?? []).filter(t => t.tagType === 'Cuisine').map(t => t.name ?? ''),
		typeTags: (recipe?.tags ?? []).filter(t => t.tagType === 'Type').map(t => t.name ?? ''),
		customTags: (recipe?.tags ?? []).filter(t => t.tagType === 'Custom').map(t => t.name ?? ''),
		equipment: (recipe?.equipment ?? []).map(e => e.name ?? '')
	};

	// Form state
	let title = $state(initialValues.title);
	let description = $state(initialValues.description);
	let instructions = $state(initialValues.instructions);
	let prepTimeMinutes = $state<number | null>(initialValues.prepTimeMinutes);
	let cookTimeMinutes = $state<number | null>(initialValues.cookTimeMinutes);
	let servings = $state<number | null>(initialValues.servings);
	let proteinGrams = $state<number | null>(initialValues.proteinGrams);
	let isTried = $state(initialValues.isTried);
	let sourceUrl = $state(initialValues.sourceUrl);
	let imageUrl = $state(initialValues.imageUrl);
	let notes = $state(initialValues.notes);
	let workspaceNeeded = $state<string | null>(initialValues.workspaceNeeded ?? null);
	let timeCategory = $state<string | null>(initialValues.timeCategory ?? null);
	let messiness = $state<string | null>(initialValues.messiness ?? null);

	// Tags state
	let cuisineTags = $state<string[]>(initialValues.cuisineTags);
	let typeTags = $state<string[]>(initialValues.typeTags);
	let customTags = $state<string[]>(initialValues.customTags);
	let newCuisineTag = $state('');
	let newTypeTag = $state('');
	let newCustomTag = $state('');

	// Equipment state
	let equipment = $state<string[]>(initialValues.equipment);
	let newEquipment = $state('');

	// Loading state
	let isSubmitting = $state(false);

	// Autocomplete suggestions
	let allTags = $state<{ name: string; tagType: string }[]>([]);
	let allEquipment = $state<string[]>([]);

	onMount(async () => {
		try {
			const [tagsData, equipmentData] = await Promise.all([getTags(), getEquipment()]);
			allTags = tagsData.map(t => ({ name: t.name ?? '', tagType: t.tagType ?? '' }));
			allEquipment = equipmentData.map(e => e.name ?? '');
		} catch (e) {
			// Silent fail for autocomplete
		}
	});

	function addTag(type: 'Cuisine' | 'Type' | 'Custom', value: string) {
		const trimmed = value.trim();
		if (!trimmed) return;

		if (type === 'Cuisine' && !cuisineTags.includes(trimmed)) {
			cuisineTags = [...cuisineTags, trimmed];
			newCuisineTag = '';
		} else if (type === 'Type' && !typeTags.includes(trimmed)) {
			typeTags = [...typeTags, trimmed];
			newTypeTag = '';
		} else if (type === 'Custom' && !customTags.includes(trimmed)) {
			customTags = [...customTags, trimmed];
			newCustomTag = '';
		}
	}

	function removeTag(type: 'Cuisine' | 'Type' | 'Custom', tag: string) {
		if (type === 'Cuisine') {
			cuisineTags = cuisineTags.filter(t => t !== tag);
		} else if (type === 'Type') {
			typeTags = typeTags.filter(t => t !== tag);
		} else {
			customTags = customTags.filter(t => t !== tag);
		}
	}

	function addEquipmentItem(value: string) {
		const trimmed = value.trim();
		if (trimmed && !equipment.includes(trimmed)) {
			equipment = [...equipment, trimmed];
			newEquipment = '';
		}
	}

	function removeEquipmentItem(item: string) {
		equipment = equipment.filter(e => e !== item);
	}

	function handleKeyDown(event: KeyboardEvent, type: 'Cuisine' | 'Type' | 'Custom' | 'Equipment') {
		if (event.key === 'Enter') {
			event.preventDefault();
			if (type === 'Equipment') {
				addEquipmentItem(newEquipment);
			} else {
				const value = type === 'Cuisine' ? newCuisineTag : type === 'Type' ? newTypeTag : newCustomTag;
				addTag(type, value);
			}
		}
	}

	async function handleSubmit(e: SubmitEvent) {
		e.preventDefault();
		
		if (!title.trim() || !instructions.trim()) {
			toast.error('Title and instructions are required');
			return;
		}

		isSubmitting = true;

		const tags = [
			...cuisineTags.map(name => ({ name, tagType: 'Cuisine' })),
			...typeTags.map(name => ({ name, tagType: 'Type' })),
			...customTags.map(name => ({ name, tagType: 'Custom' }))
		];

		try {
			if (mode === 'create') {
				const request: CreateRecipeRequest = {
					title: title.trim(),
					instructions: instructions.trim(),
					description: description.trim() || null,
					prepTimeMinutes: prepTimeMinutes,
					cookTimeMinutes: cookTimeMinutes,
					servings: servings,
					proteinGrams: proteinGrams,
					isTried: isTried,
					sourceUrl: sourceUrl.trim() || null,
					imageUrl: imageUrl.trim() || null,
					notes: notes.trim() || null,
					workspaceNeeded: workspaceNeeded,
					timeCategory: timeCategory,
					messiness: messiness,
					tags: tags.length > 0 ? tags : null,
					equipmentNames: equipment.length > 0 ? equipment : null
				};

				const id = await createRecipe(request);
				toast.success('Recipe created!');
				goto(`/recipes/${id}`);
			} else if (recipe?.id) {
				const request: UpdateRecipeRequest = {
					title: title.trim(),
					instructions: instructions.trim(),
					description: description.trim() || null,
					prepTimeMinutes: prepTimeMinutes,
					cookTimeMinutes: cookTimeMinutes,
					servings: servings,
					proteinGrams: proteinGrams,
					isTried: isTried,
					sourceUrl: sourceUrl.trim() || null,
					imageUrl: imageUrl.trim() || null,
					notes: notes.trim() || null,
					workspaceNeeded: workspaceNeeded,
					timeCategory: timeCategory,
					messiness: messiness,
					tags: tags.length > 0 ? tags : null,
					equipmentNames: equipment.length > 0 ? equipment : null
				};

				await updateRecipe(recipe.id, request);
				toast.success('Recipe updated!');
				goto(`/recipes/${recipe.id}`);
			}
		} catch (e) {
			toast.error(mode === 'create' ? 'Failed to create recipe' : 'Failed to update recipe');
		} finally {
			isSubmitting = false;
		}
	}
</script>

<div class="container mx-auto max-w-3xl space-y-6 p-6">
	<!-- Header -->
	<div class="flex items-center gap-4">
		<Button variant="ghost" onclick={() => history.back()} class="gap-2">
			<ArrowLeft class="h-4 w-4" />
			Back
		</Button>
		<h1 class="text-2xl font-bold">
			{mode === 'create' ? 'Add New Recipe' : 'Edit Recipe'}
		</h1>
	</div>

	<form onsubmit={handleSubmit} class="space-y-6">
		<!-- Basic Info -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Basic Information</Card.Title>
			</Card.Header>
			<Card.Content class="space-y-4">
				<div class="space-y-2">
					<Label for="title">Title *</Label>
					<Input id="title" bind:value={title} placeholder="Recipe title" required />
				</div>

				<div class="space-y-2">
					<Label for="description">Description</Label>
					<Textarea id="description" bind:value={description} placeholder="Brief description of the recipe" rows={2} />
				</div>

				<div class="space-y-2">
					<Label for="instructions">Instructions *</Label>
					<Textarea id="instructions" bind:value={instructions} placeholder="Step-by-step cooking instructions" rows={8} required />
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Time & Servings -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Time & Servings</Card.Title>
			</Card.Header>
			<Card.Content class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
				<div class="space-y-2">
					<Label for="prepTime">Prep Time (min)</Label>
					<Input id="prepTime" type="number" min="0" bind:value={prepTimeMinutes} placeholder="30" />
				</div>
				<div class="space-y-2">
					<Label for="cookTime">Cook Time (min)</Label>
					<Input id="cookTime" type="number" min="0" bind:value={cookTimeMinutes} placeholder="45" />
				</div>
				<div class="space-y-2">
					<Label for="servings">Servings</Label>
					<Input id="servings" type="number" min="1" bind:value={servings} placeholder="4" />
				</div>
				<div class="space-y-2">
					<Label for="protein">Protein (g)</Label>
					<Input id="protein" type="number" min="0" step="0.1" bind:value={proteinGrams} placeholder="25" />
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Categories -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Categories</Card.Title>
			</Card.Header>
			<Card.Content class="grid gap-4 sm:grid-cols-3">
				<div class="space-y-2">
					<Label for="timeCategory">Time Category</Label>
					<select
						id="timeCategory"
						class="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50"
						value={timeCategory ?? ''}
						onchange={(e) => timeCategory = e.currentTarget.value || null}
					>
						<option value="">Select...</option>
						{#each Object.entries(timeCategoryLabels) as [value, label]}
							<option value={value}>{label}</option>
						{/each}
					</select>
				</div>
				<div class="space-y-2">
					<Label for="workspaceNeeded">Workspace Needed</Label>
					<select
						id="workspaceNeeded"
						class="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50"
						value={workspaceNeeded ?? ''}
						onchange={(e) => workspaceNeeded = e.currentTarget.value || null}
					>
						<option value="">Select...</option>
						{#each Object.entries(workspaceNeededLabels) as [value, label]}
							<option value={value}>{label}</option>
						{/each}
					</select>
				</div>
				<div class="space-y-2">
					<Label for="messiness">Messiness</Label>
					<select
						id="messiness"
						class="flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50"
						value={messiness ?? ''}
						onchange={(e) => messiness = e.currentTarget.value || null}
					>
						<option value="">Select...</option>
						{#each Object.entries(messinessLabels) as [value, label]}
							<option value={value}>{label}</option>
						{/each}
					</select>
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Tags -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Tags</Card.Title>
			</Card.Header>
			<Card.Content class="space-y-4">
				<!-- Cuisine Tags -->
				<div class="space-y-2">
					<Label for="cuisineTag">Cuisine Tags</Label>
					<div class="flex gap-2">
						<Input
							id="cuisineTag"
							bind:value={newCuisineTag}
							placeholder="Add cuisine (e.g., Italian, Mexican)"
							onkeydown={(e: KeyboardEvent) => handleKeyDown(e, 'Cuisine')}
							list="cuisine-suggestions"
						/>
						<Button type="button" variant="outline" onclick={() => addTag('Cuisine', newCuisineTag)}>
							<Plus class="h-4 w-4" />
						</Button>
					</div>
					<datalist id="cuisine-suggestions">
						{#each allTags.filter(t => t.tagType === 'Cuisine') as tag}
							<option value={tag.name}></option>
						{/each}
					</datalist>
					{#if cuisineTags.length > 0}
						<div class="flex flex-wrap gap-2">
							{#each cuisineTags as tag}
								<Badge variant="secondary" class="gap-1">
									{tag}
									<button type="button" class="hover:text-destructive" onclick={() => removeTag('Cuisine', tag)}>
										<X class="h-3 w-3" />
									</button>
								</Badge>
							{/each}
						</div>
					{/if}
				</div>

				<!-- Type Tags -->
				<div class="space-y-2">
					<Label for="typeTag">Type Tags</Label>
					<div class="flex gap-2">
						<Input
							id="typeTag"
							bind:value={newTypeTag}
							placeholder="Add type (e.g., Dinner, Dessert)"
							onkeydown={(e: KeyboardEvent) => handleKeyDown(e, 'Type')}
							list="type-suggestions"
						/>
						<Button type="button" variant="outline" onclick={() => addTag('Type', newTypeTag)}>
							<Plus class="h-4 w-4" />
						</Button>
					</div>
					<datalist id="type-suggestions">
						{#each allTags.filter(t => t.tagType === 'Type') as tag}
							<option value={tag.name}></option>
						{/each}
					</datalist>
					{#if typeTags.length > 0}
						<div class="flex flex-wrap gap-2">
							{#each typeTags as tag}
								<Badge class="gap-1">
									{tag}
									<button type="button" class="hover:text-destructive" onclick={() => removeTag('Type', tag)}>
										<X class="h-3 w-3" />
									</button>
								</Badge>
							{/each}
						</div>
					{/if}
				</div>

				<!-- Custom Tags -->
				<div class="space-y-2">
					<Label for="customTag">Custom Tags</Label>
					<div class="flex gap-2">
						<Input
							id="customTag"
							bind:value={newCustomTag}
							placeholder="Add custom tag"
							onkeydown={(e: KeyboardEvent) => handleKeyDown(e, 'Custom')}
							list="custom-suggestions"
						/>
						<Button type="button" variant="outline" onclick={() => addTag('Custom', newCustomTag)}>
							<Plus class="h-4 w-4" />
						</Button>
					</div>
					<datalist id="custom-suggestions">
						{#each allTags.filter(t => t.tagType === 'Custom') as tag}
							<option value={tag.name}></option>
						{/each}
					</datalist>
					{#if customTags.length > 0}
						<div class="flex flex-wrap gap-2">
							{#each customTags as tag}
								<Badge variant="outline" class="gap-1">
									{tag}
									<button type="button" class="hover:text-destructive" onclick={() => removeTag('Custom', tag)}>
										<X class="h-3 w-3" />
									</button>
								</Badge>
							{/each}
						</div>
					{/if}
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Equipment -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Equipment</Card.Title>
			</Card.Header>
			<Card.Content class="space-y-2">
				<div class="flex gap-2">
					<Input
						bind:value={newEquipment}
						placeholder="Add equipment (e.g., Stand Mixer, Dutch Oven)"
						onkeydown={(e: KeyboardEvent) => handleKeyDown(e, 'Equipment')}
						list="equipment-suggestions"
					/>
					<Button type="button" variant="outline" onclick={() => addEquipmentItem(newEquipment)}>
						<Plus class="h-4 w-4" />
					</Button>
				</div>
				<datalist id="equipment-suggestions">
					{#each allEquipment as item}
						<option value={item}></option>
					{/each}
				</datalist>
				{#if equipment.length > 0}
					<div class="flex flex-wrap gap-2">
						{#each equipment as item}
							<Badge variant="outline" class="gap-1">
								{item}
								<button type="button" class="hover:text-destructive" onclick={() => removeEquipmentItem(item)}>
									<X class="h-3 w-3" />
								</button>
							</Badge>
						{/each}
					</div>
				{/if}
			</Card.Content>
		</Card.Root>

		<!-- URLs & Notes -->
		<Card.Root>
			<Card.Header>
				<Card.Title>Additional Information</Card.Title>
			</Card.Header>
			<Card.Content class="space-y-4">
				<div class="space-y-2">
					<Label for="sourceUrl">Source URL</Label>
					<Input id="sourceUrl" type="url" bind:value={sourceUrl} placeholder="https://example.com/recipe" />
				</div>
				<div class="space-y-2">
					<Label for="imageUrl">Image URL</Label>
					<Input id="imageUrl" type="url" bind:value={imageUrl} placeholder="https://example.com/image.jpg" />
				</div>
				<div class="space-y-2">
					<Label for="notes">Personal Notes</Label>
					<Textarea id="notes" bind:value={notes} placeholder="Any personal notes or modifications" rows={3} />
				</div>
				<div class="flex items-center gap-2">
					<input
						type="checkbox"
						id="isTried"
						bind:checked={isTried}
						class="h-4 w-4 rounded border-gray-300 text-primary focus:ring-primary"
					/>
					<Label for="isTried" class="font-normal">I have tried this recipe</Label>
				</div>
			</Card.Content>
		</Card.Root>

		<!-- Submit -->
		<div class="flex justify-end gap-4">
			<Button type="button" variant="outline" onclick={() => history.back()}>Cancel</Button>
			<Button type="submit" disabled={isSubmitting}>
				{#if isSubmitting}
					<Loader2 class="mr-2 h-4 w-4 animate-spin" />
				{/if}
				{mode === 'create' ? 'Create Recipe' : 'Save Changes'}
			</Button>
		</div>
	</form>
</div>
