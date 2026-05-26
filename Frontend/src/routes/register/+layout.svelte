<script lang="ts">
	import ThemeToggle from '$lib/components/ThemeToggle.svelte';

	let { children } = $props();
</script>

<ThemeToggle class="fixed top-4 right-4" />

{@render children()}
