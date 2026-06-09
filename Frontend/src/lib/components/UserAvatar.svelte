<script lang="ts">
	import { authStore } from '$lib/authStore.svelte';
	import { User } from '@lucide/svelte';
	import type { ClassValue } from 'svelte/elements';

	type Props = {
		size?: number;
		class?: ClassValue;
		style?: string;
	};

	let { size = 10, class: classes, style }: Props = $props();
</script>

<div class={['avatar', classes]} {style}>
	<div class="rounded-full bg-base-100" style="width: {size / 4}rem;">
		{#if authStore.user?.profilePictureUrl}
			<img alt="Account" src={authStore.user.profilePictureUrl} />
		{:else}
			<User size={size * 2} class="m-auto h-full" />
		{/if}
	</div>
</div>
