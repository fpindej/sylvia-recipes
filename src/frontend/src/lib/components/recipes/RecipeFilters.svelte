<script lang="ts">
	import { Input } from '$lib/components/ui/input';
	import { Button } from '$lib/components/ui/button';
	import { Badge } from '$lib/components/ui/badge';
	import {
		type RecipeFilters,
		WorkspaceNeededValues,
		TimeCategoryValues,
		MessinessValues,
		timeCategoryLabels,
		workspaceNeededLabels,
		messinessLabels
	} from '$lib/types/recipe';
	import { Search, X, Filter, ChevronDown } from 'lucide-svelte';

	interface Props {
		filters: RecipeFilters;
		onfilterchange: (filters: RecipeFilters) => void;
	}

	let { filters, onfilterchange }: Props = $props();

	// Local state for debouncing search input
	let localSearchTerm = $state('');
	let showFilters = $state(false);
	let debounceTimer: ReturnType<typeof setTimeout>;

	// Sync local state when filters prop changes (e.g., from URL navigation)
	$effect(() => {
		localSearchTerm = filters.searchTerm ?? '';
	});

	function handleSearchInput(event: Event) {
		const value = (event.target as HTMLInputElement).value;
		localSearchTerm = value;

		clearTimeout(debounceTimer);
		debounceTimer = setTimeout(() => {
			onfilterchange({ ...filters, searchTerm: value || undefined, pageNumber: 1 });
		}, 300);
	}

	function clearSearch() {
		localSearchTerm = '';
		onfilterchange({ ...filters, searchTerm: undefined, pageNumber: 1 });
	}

	function toggleTriedFilter() {
		const newValue = filters.isTried === undefined ? true : filters.isTried === true ? false : undefined;
		onfilterchange({ ...filters, isTried: newValue, pageNumber: 1 });
	}

	function setTimeCategory(value: number | undefined) {
		onfilterchange({ ...filters, timeCategory: value, pageNumber: 1 });
	}

	function setWorkspaceNeeded(value: number | undefined) {
		onfilterchange({ ...filters, workspaceNeeded: value, pageNumber: 1 });
	}

	function setMessiness(value: number | undefined) {
		onfilterchange({ ...filters, messiness: value, pageNumber: 1 });
	}

	function clearAllFilters() {
		localSearchTerm = '';
		onfilterchange({ pageNumber: 1, pageSize: filters.pageSize });
	}

	const hasActiveFilters = $derived(
		filters.searchTerm ||
			filters.isTried !== undefined ||
			filters.timeCategory !== undefined ||
			filters.workspaceNeeded !== undefined ||
			filters.messiness !== undefined ||
			filters.cuisines ||
			filters.types ||
			filters.equipment ||
			filters.minProteinGrams !== undefined
	);
</script>

<div class="space-y-4">
	<!-- Search bar -->
	<div class="flex gap-2">
		<div class="relative flex-1">
			<Search class="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
			<Input
				type="text"
				placeholder="Search recipes..."
				value={localSearchTerm}
				oninput={handleSearchInput}
				class="pl-10 pr-10"
			/>
			{#if localSearchTerm}
				<button
					type="button"
					onclick={clearSearch}
					class="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
				>
					<X class="h-4 w-4" />
				</button>
			{/if}
		</div>
		<Button
			variant={showFilters ? 'secondary' : 'outline'}
			onclick={() => (showFilters = !showFilters)}
			class="gap-2"
		>
			<Filter class="h-4 w-4" />
			Filters
			{#if hasActiveFilters}
				<Badge variant="secondary" class="ml-1 h-5 w-5 rounded-full p-0 text-xs">!</Badge>
			{/if}
			<ChevronDown class="h-4 w-4 transition-transform {showFilters ? 'rotate-180' : ''}" />
		</Button>
	</div>

	<!-- Filter panel -->
	{#if showFilters}
		<div class="rounded-lg border bg-card p-4 space-y-4">
			<div class="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
				<!-- Tried filter -->
				<div class="space-y-2">
					<label class="text-sm font-medium">Status</label>
					<div class="flex gap-2">
						<Button
							variant={filters.isTried === true ? 'default' : 'outline'}
							size="sm"
							onclick={() => toggleTriedFilter()}
							class="flex-1"
						>
							{filters.isTried === undefined ? 'All' : filters.isTried ? 'Tried' : 'Not tried'}
						</Button>
					</div>
				</div>

				<!-- Time category -->
				<div class="space-y-2">
					<label class="text-sm font-medium">Time</label>
					<div class="flex flex-wrap gap-1">
						<Button
							variant={filters.timeCategory === undefined ? 'secondary' : 'ghost'}
							size="sm"
							onclick={() => setTimeCategory(undefined)}
						>
							Any
						</Button>
						{#each Object.entries(timeCategoryLabels) as [value, label]}
							<Button
								variant={filters.timeCategory === Number(value) ? 'default' : 'outline'}
								size="sm"
								onclick={() => setTimeCategory(Number(value))}
							>
								{label.split(' ')[0]}
							</Button>
						{/each}
					</div>
				</div>

				<!-- Workspace -->
				<div class="space-y-2">
					<label class="text-sm font-medium">Workspace</label>
					<div class="flex flex-wrap gap-1">
						<Button
							variant={filters.workspaceNeeded === undefined ? 'secondary' : 'ghost'}
							size="sm"
							onclick={() => setWorkspaceNeeded(undefined)}
						>
							Any
						</Button>
						{#each Object.entries(workspaceNeededLabels) as [value, label]}
							<Button
								variant={filters.workspaceNeeded === Number(value) ? 'default' : 'outline'}
								size="sm"
								onclick={() => setWorkspaceNeeded(Number(value))}
							>
								{label}
							</Button>
						{/each}
					</div>
				</div>

				<!-- Messiness -->
				<div class="space-y-2">
					<label class="text-sm font-medium">Messiness</label>
					<div class="flex flex-wrap gap-1">
						<Button
							variant={filters.messiness === undefined ? 'secondary' : 'ghost'}
							size="sm"
							onclick={() => setMessiness(undefined)}
						>
							Any
						</Button>
						{#each Object.entries(messinessLabels) as [value, label]}
							<Button
								variant={filters.messiness === Number(value) ? 'default' : 'outline'}
								size="sm"
								onclick={() => setMessiness(Number(value))}
							>
								{label}
							</Button>
						{/each}
					</div>
				</div>
			</div>

			{#if hasActiveFilters}
				<div class="flex justify-end">
					<Button variant="ghost" size="sm" onclick={clearAllFilters}>
						<X class="mr-1 h-4 w-4" />
						Clear all filters
					</Button>
				</div>
			{/if}
		</div>
	{/if}
</div>
