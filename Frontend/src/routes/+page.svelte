<script>
	import { goto } from '$app/navigation';
	import { authStore } from '$lib/authStore.svelte';
</script>

<div class="p-4">
	<h1 class="mb-4 text-4xl font-bold">Landing Page</h1>

	<button class="btn" onclick={() => goto('/register')}>Go to Register</button>
	<button class="btn" onclick={() => goto('/login')}>Go to Login</button>

	{#if authStore.isLoggedIn}
		<p class="mt-4 font-bold">You are already logged in!</p>
		<button class="btn btn-primary" onclick={() => goto('/app')}>Go to Dashboard</button>
	{/if}
</div>
