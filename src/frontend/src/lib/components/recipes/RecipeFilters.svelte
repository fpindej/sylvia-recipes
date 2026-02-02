<script lang="ts">
	import { Input } from '$lib/components/ui/input';
	import { Button } from '$lib/components/ui/button';
	import { Badge } from '$lib/components/ui/badge';
	import {
		type RecipeFilters,
		type TimeCategory,
		type WorkspaceNeeded,
		type Messiness,
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

	// Local state for debouncing search input - syncs with filters.searchTerm
	let localSearchTerm = $derived.by(() => filters.searchTerm ?? '');
	let showFilters = $state(false);
	let debounceTimer: ReturnType<typeof setTimeout>;

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
		const newValue =
			filters.isTried === undefined ? true : filters.isTried === true ? false : undefined;
		onfilterchange({ ...filters, isTried: newValue, pageNumber: 1 });
	}

	function setTimeCategory(value: TimeCategory | undefined) {
		onfilterchange({ ...filters, timeCategory: value ?? undefined, pageNumber: 1 });
	}

	function setWorkspaceNeeded(value: WorkspaceNeeded | undefined) {
		onfilterchange({ ...filters, workspaceNeeded: value ?? undefined, pageNumber: 1 });
	}

	function setMessiness(value: Messiness | undefined) {
		onfilterchange({ ...filters, messiness: value ?? undefined, pageNumber: 1 });
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
			<Search class="absolute top-1/2 left-3 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
			<Input
				type="text"
				placeholder="Search recipes..."
				value={localSearchTerm}
				oninput={handleSearchInput}
				class="pr-10 pl-10"
			/>
			{#if localSearchTerm}
				<button
					type="button"
					onclick={clearSearch}
					class="absolute top-1/2 right-3 -translate-y-1/2 text-muted-foreground hover:text-foreground"
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
		<div class="space-y-4 rounded-lg border bg-card p-4">
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
						{#each Object.entries(timeCategoryLabels) as [value, label] (value)}
							<Button
								variant={filters.timeCategory === value ? 'default' : 'outline'}
								size="sm"
								onclick={() => setTimeCategory(value as TimeCategory)}
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
						{#each Object.entries(workspaceNeededLabels) as [value, label] (value)}
							<Button
								variant={filters.workspaceNeeded === value ? 'default' : 'outline'}
								size="sm"
								onclick={() => setWorkspaceNeeded(value as WorkspaceNeeded)}
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
						{#each Object.entries(messinessLabels) as [value, label] (value)}
							<Button
								variant={filters.messiness === value ? 'default' : 'outline'}
								size="sm"
								onclick={() => setMessiness(value as Messiness)}
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
