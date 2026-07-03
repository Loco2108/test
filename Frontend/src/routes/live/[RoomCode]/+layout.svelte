<script module lang="ts">
	export interface SessionContext {
		startSession: () => void;
	}
</script>

<script lang="ts">
	import { setContext } from 'svelte';
	import { fly, fade } from 'svelte/transition';
	import { backOut } from 'svelte/easing';
	import { SessionState } from '$lib/wsClient/Backend.Models.Enums.js';
	import { Loader, LoaderCircle } from '@lucide/svelte';

	let { children, data } = $props();
	let hub = $derived(data.hub);

	let phase: SessionState = $derived(hub.state?.sessionState ?? SessionState.Lobby);

	function startSession() {
		setFullscreen(true);
		phase = SessionState.Loading;
	}

	function setFullscreen(active: boolean) {
		var elem = document.documentElement;

		if (!active && document.fullscreenElement) {
			document.exitFullscreen();
			return;
		}

		var rfs = elem.requestFullscreen;

		if (typeof rfs !== undefined && rfs) {
			rfs.call(elem);
		}
	}

	setContext<SessionContext>('session', { startSession });
</script>

{#if phase === SessionState.Lobby}
	<div out:fade={{ duration: 300 }}>
		{@render children()}
	</div>
{:else if phase === SessionState.Loading}
	<div
		class="fixed inset-0 z-50 flex flex-col items-center justify-center bg-primary"
		in:fade={{ duration: 400 }}
		out:fade={{ duration: 400 }}>
		<div class="text-6xl font-bold text-white md:text-[10rem]">Stimmti</div>
		<div class="mt-4 flex gap-2 text-xl font-bold text-white">
			<LoaderCircle class="animate-spin" />
			Loading
		</div>
	</div>
{:else if phase === SessionState.Question}
	Question...
{/if}
