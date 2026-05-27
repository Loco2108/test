<script>
	import { authStore } from '$lib/authStore.svelte';
</script>

{#if authStore.user && authStore.user.username}
	<h2 class="mt-8 w-full text-center text-6xl font-bold">Hello, {authStore.user?.username}</h2>
{/if}
