<script module lang="ts">
	export interface SessionContext {
		startSession: () => void;
	}
</script>

<script lang="ts">
	import { onMount, setContext } from 'svelte';
	import { fade } from 'svelte/transition';
	import { ParticipantRole, SessionState } from '$lib/wsClient/Backend.Models.Enums.js';
	import { LoaderCircle, Trophy, User } from '@lucide/svelte';
	import { addToast } from '$lib/components/Toast/Toast.svelte';

	let { children, data } = $props();
	let hub = $derived(data.hub);

	let phase: SessionState = $derived(hub.state?.sessionState ?? SessionState.Lobby);

	onMount(() => {
		if (phase === SessionState.Lobby) {
			setFullscreen(false);
			return;
		}

		setFullscreen(true);
	});

	async function startSession() {
		if (hub.state?.participants.length == 0) {
			addToast({
				label: 'There are no participants in this session',
				icon: User,
				type: 'error',
			});
			return;
		}

		let res = await hub.startSession();

		if (!res) {
			addToast({ label: 'Error starting session', type: 'error' });
			return;
		}

		setFullscreen(true);
	}

	async function nextQuestion() {
		let res = await hub.nextQuestion();

		if (!res) {
			addToast({
				label: 'Serious error occured. Try reloading the page',
				type: 'error',
			});
		}
	}

	async function closeSession() {
		let res = await hub.closeSession();

		setFullscreen(false);

		if (!res) {
			addToast({
				label: 'Error closing session. Try reloading the page',
				type: 'error',
			});
			return;
		}
	}

	function setFullscreen(active: boolean) {
		var elem = document.documentElement;

		if (!active) {
			if (document.fullscreenElement) {
				document
					.exitFullscreen()
					.catch((err) => console.error('Error closing fullscreen:', err));
			}

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
	<div in:fade={{ duration: 400 }} out:fade={{ duration: 400 }}>
		<h1 class="mx-auto mt-8 w-fit text-5xl font-bold text-balance">
			{hub.state?.currentQuestion?.name}
		</h1>

		{#if hub.state?.currentQuestion?.description}
			<p class="mx-auto mt-2 w-fit text-3xl opacity-80">
				{hub.state.currentQuestion.description}
			</p>
		{/if}

		{#if hub.state?.role === ParticipantRole.Presenter}
			<button class="btn absolute right-4 bottom-4 btn-primary btn-xl" onclick={nextQuestion}>
				Continue
			</button>
		{/if}
	</div>
{:else if phase === SessionState.Finished}
	{#if hub.state?.role === ParticipantRole.Presenter}
		<div
			class="flex h-screen w-full items-center justify-center"
			in:fade={{ duration: 400 }}
			out:fade={{ duration: 400 }}>
			<div class="card min-w-96 bg-base-100 card-md">
				<div class="card-body items-center gap-2">
					<Trophy class="size-15 text-warning" />

					<span class="text-4xl font-bold">{hub.state.sessionName}</span>

					<span class="text-2xl opacity-80"
						>{hub.state.participants.length} Participants</span>

					<div class="mt-4 card-actions w-full">
						<button class="btn btn-block btn-primary" onclick={closeSession}>
							Close Session
						</button>
					</div>
				</div>
			</div>
		</div>
	{:else}
		<div class="flex h-screen w-full flex-col items-center justify-center gap-2">
			<span class="text-4xl font-bold">Thanks for participating!</span>
			<span class="text-2xl opacity-80">You can close this tab now</span>
		</div>
	{/if}
{/if}
