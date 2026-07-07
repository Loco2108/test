<script module lang="ts">
	export interface SessionContext {
		startSession: () => void;
	}
</script>

<script lang="ts">
	import { onMount, setContext } from 'svelte';
	import { fade } from 'svelte/transition';
	import {
		ParticipantRole,
		QuestionTypeEnum,
		SessionState,
	} from '$lib/wsClient/Backend.Models.Enums.js';
	import { LoaderCircle, Trophy, User } from '@lucide/svelte';
	import { addToast } from '$lib/components/Toast/Toast.svelte';
	import NumberScale from '$lib/components/NumberScale.svelte';
	import ThemeToggle from '$lib/components/ThemeToggle.svelte';
	import SingleChoice from '$lib/components/SingleChoice.svelte';
	import { type Option } from '$lib/components/MultipleChoice.svelte';
	import MultipleChoice from '$lib/components/MultipleChoice.svelte';

	let { children, data } = $props();
	let hub = $derived(data.hub);

	let phase: SessionState = $derived(hub.state?.sessionState ?? SessionState.Lobby);

	let choiceOptions: Option[] = $state([]);
	$effect(() => {
		const answers = hub.state?.currentQuestion?.answerOptions ?? [];

		choiceOptions = answers.map((x) => ({
			label: x.description,
			checked: false,
		}));
	});

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

{#if phase !== SessionState.Lobby}
	<div class="navbar border-b border-base-300 bg-base-100 px-4">
		<div class="navbar-start gap-2">
			<span class="text-lg font-bold">Stimmti</span>
		</div>
		<div class="navbar-center font-bold opacity-80">
			{hub.state?.sessionName}
		</div>
		<div class="navbar-end">
			<ThemeToggle />
		</div>
	</div>
{/if}

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
		<h1 class="mx-auto mt-8 w-fit text-center text-5xl font-bold text-balance">
			{hub.state?.currentQuestion?.name}
		</h1>

		{#if hub.state?.currentQuestion?.description}
			<p class="mx-auto mt-4 w-fit text-center text-3xl opacity-80">
				{hub.state.currentQuestion.description}
			</p>
		{/if}

		<div class="divider mb-16"></div>

		{#if hub.state?.role === ParticipantRole.Participant}
			{#if hub.state.currentQuestion?.questionType === QuestionTypeEnum.MultipleChoice}
				<MultipleChoice class="mx-auto" bind:options={choiceOptions} />
			{:else if hub.state.currentQuestion?.questionType === QuestionTypeEnum.NumberScale}
				<div class="card w-full bg-base-100 md:mx-auto md:max-w-120">
					<div class="card-body w-full p-12 md:p-6">
						<NumberScale
							minValue={hub.state.currentQuestion.minValue}
							maxValue={hub.state.currentQuestion.maxValue} />
					</div>
				</div>
			{:else if hub.state.currentQuestion?.questionType === QuestionTypeEnum.SingleChoice}
				<SingleChoice class="mx-auto" options={choiceOptions} />
			{:else if hub.state.currentQuestion?.questionType === QuestionTypeEnum.WordCloud}
				<fieldset class="mx-auto fieldset w-full md:max-w-120">
					<legend class="fieldset-legend text-lg">Describe in one word</legend>
					<input type="text" class="input w-full" placeholder="Exciting" />
				</fieldset>
			{:else if hub.state.currentQuestion?.questionType === QuestionTypeEnum.FreeText}
				<fieldset class="mx-auto fieldset w-full md:max-w-120">
					<legend class="fieldset-legend text-lg">Enter your thoughts</legend>
					<textarea class="textarea w-full"></textarea>
				</fieldset>
			{/if}
			<div class="mt-16 flex justify-center">
				<button class="btn btn-primary btn-xl">Submit</button>
			</div>
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
			class="absolute top-0 left-0 flex h-screen w-full items-center justify-center"
			in:fade={{ duration: 400 }}
			out:fade={{ duration: 400 }}>
			<div class="card min-w-96 bg-base-100 card-md">
				<div class="card-body items-center gap-2">
					<Trophy class="size-15 text-warning" />

					<span class="text-4xl font-bold">{hub.state.sessionName}</span>

					<span class="text-2xl opacity-80">
						{hub.state.participants.length} Participants
					</span>

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
