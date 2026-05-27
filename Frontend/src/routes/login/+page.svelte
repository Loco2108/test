<script lang="ts">
	import { goto } from '$app/navigation';
	import Login from '$lib/components/Login.svelte';
	import ThemeToggle from '$lib/components/ThemeToggle.svelte';
</script>

<ThemeToggle class="fixed top-4 right-4" />

<div class="flex min-h-screen w-full items-center justify-center">
	<Login onloginfinished={() => goto('/app', { invalidateAll: true })} />
</div>
