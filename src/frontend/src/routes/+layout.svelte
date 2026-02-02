<script lang="ts">
	import '../styles/index.css';
	import 'flag-icons/css/flag-icons.min.css';
	import { onMount } from 'svelte';
	import { initTheme } from '$lib/state/theme.svelte';
	import * as m from '$lib/paraglide/messages';
	import { Toaster } from '$lib/components/ui/sonner';
	import * as Tooltip from '$lib/components/ui/tooltip';
	import { globalShortcuts } from '$lib/state/shortcuts.svelte';
	import { goto } from '$app/navigation';
	import { resolve } from '$app/paths';
	import { logout } from '$lib/auth';
	import { ShortcutsHelp } from '$lib/components/layout';
	import { toggleSidebar } from '$lib/state';

	let { children } = $props();

	onMount(() => {
		return initTheme();
	});

	async function handleSettings() {
		await goto(resolve('/settings'));
	}
</script>

<svelte:window
	use:globalShortcuts={{
		settings: handleSettings,
		logout: logout,
		toggleSidebar: toggleSidebar
	}}
/>

<ShortcutsHelp />

<svelte:head>
	<title>{m.app_name()}</title>
	<meta name="description" content={m.meta_description()} />
	<link rel="icon" type="image/x-icon" href="/favicon.ico" />
	<link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png" />
	<link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png" />
	<link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon.png" />
	<link rel="manifest" href="/site.webmanifest" />
</svelte:head>

<Tooltip.Provider>
	<Toaster />
	{@render children()}
</Tooltip.Provider>
