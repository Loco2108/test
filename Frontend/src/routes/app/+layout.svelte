<script lang="ts">
	import NavigationBar from '$lib/components/NavigationBar.svelte';

	let { children } = $props();
</script>

<NavigationBar />

<div class="p-4">
	{@render children()}
</div>
