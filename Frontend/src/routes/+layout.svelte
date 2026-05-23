<script lang="ts">
	import './layout.css';
	import favicon from '$lib/assets/favicon.svg';
	import { onMount } from 'svelte';
	import { themeManager } from '$lib/Theme.svelte';
	import { goto } from '$app/navigation';

	onMount(() => {
		themeManager.init();

		goto('./app');
	});

	let { children } = $props();
</script>

<svelte:head><link rel="icon" href={favicon} /></svelte:head>
{@render children()}
