<script lang="ts">
	import './layout.css';
	import favicon from '$lib/assets/favicon.svg';
	import { onMount } from 'svelte';
	import { themeManager } from '$lib/Theme.svelte';
	import { loginUser } from '$lib/authStore.svelte';

	let { children, data } = $props();

	onMount(() => {
		themeManager.init();
	});

	$effect.pre(() => {
		if (!data.user) return;

		loginUser(data.user);
	});
</script>

<svelte:head><link rel="icon" href={favicon} /></svelte:head>
{@render children()}
