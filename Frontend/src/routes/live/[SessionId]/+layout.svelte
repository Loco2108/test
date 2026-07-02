<!--
Note: The states and their screens are purely vibe coded, so later this will look a whole lot different.
Normal participants will only have the "voting board" and the presenter only displays the result WHILE the participants are voting.
None of this will stay as it is right now
-->

<script module lang="ts">
	export interface SessionContext {
		startSession: () => void;
	}
</script>

<script lang="ts">
	import { setContext } from 'svelte';
	import { fly, fade } from 'svelte/transition';
	import { backOut } from 'svelte/easing';
	import { ChevronRight, Trophy, BarChart2 } from '@lucide/svelte';

	type Phase = 'lobby' | 'starting' | 'question' | 'results' | 'finished';

	let { children } = $props();

	let phase: Phase = $state('lobby');
	let animationStep = $state(3);
	let currentQuestion = $state(0);

	const mockQuestions = [
		{
			question: "What' your favorite programming language?",
			options: ['TypeScript', 'Rust', 'Python', 'Go'],
		},
		{
			question: 'Are you happy with this session so far?',
			options: ['Very happy', 'Somewhat happy', 'neutral', 'Not happy :('],
		},
		{
			question: 'Which Frontend Framework are you using?',
			options: ['Svelte', 'React', 'Vue', 'Angular'],
		},
	];

	const mockVotes = [
		[14, 8, 5, 3],
		[7, 12, 4, 2],
		[18, 9, 6, 1],
	];

	function startSession() {
		setFullscreen(true);
		phase = 'starting';
		animationStep = 3;

		const tick = () => {
			animationStep--;
			if (animationStep > 0) {
				setTimeout(tick, 1000);
			} else {
				setTimeout(() => {
					currentQuestion = 0;
					phase = 'question';
				}, 1200);
			}
		};
		setTimeout(tick, 1000);
	}

	function showResults() {
		phase = 'results';
	}

	function nextOrFinish() {
		if (currentQuestion < mockQuestions.length - 1) {
			currentQuestion++;
			phase = 'question';
		} else {
			phase = 'finished';
		}
	}

	function resetToLobby() {
		phase = 'lobby';
		currentQuestion = 0;
		setFullscreen(false);
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

{#if phase === 'lobby'}
	<div out:fade={{ duration: 300 }}>
		{@render children()}
	</div>
{:else if phase === 'starting'}
	<div
		class="fixed inset-0 z-50 flex flex-col items-center justify-center bg-primary"
		in:fade={{ duration: 400 }}
		out:fade={{ duration: 400 }}>
		{#if animationStep > 0}
			<div class="text-[15rem] font-bold">Stimmti</div>
			<span
				class="countdown font-mono text-[10rem] font-black text-primary-content select-none"
				aria-live="polite">
				<span style="--value:{animationStep};"></span>
			</span>
		{:else}
			<div
				class="text-center text-primary-content"
				in:fly={{ y: 50, duration: 500, easing: backOut }}>
				<div class="text-5xl font-black md:text-7xl">Let's go!</div>
				<div class="mt-3 text-xl opacity-75">Session started…</div>
			</div>
		{/if}
	</div>
{:else if phase === 'question'}
	{@const q = mockQuestions[currentQuestion]}
	<div
		class="fixed inset-0 z-50 flex flex-col bg-base-100 p-4 md:p-8"
		in:fly={{ x: 80, duration: 450, easing: backOut }}
		out:fly={{ x: -80, duration: 300 }}>
		<div class="mb-6 flex items-center gap-3">
			<span class="shrink-0 text-sm font-medium opacity-60">
				Question {currentQuestion + 1} / {mockQuestions.length}
			</span>
			<div class="h-2 flex-1 overflow-hidden rounded-full bg-base-300">
				<div
					class="h-full rounded-full bg-primary transition-all duration-700"
					style="width: {((currentQuestion + 1) / mockQuestions.length) * 100}%">
				</div>
			</div>
		</div>

		<div class="flex flex-1 flex-col items-center justify-center gap-8">
			<h1 class="max-w-2xl text-center text-3xl font-bold md:text-5xl">{q.question}</h1>
			<div class="grid w-full max-w-2xl grid-cols-1 gap-3 md:grid-cols-2">
				{#each q.options as option, i}
					<button
						class="btn h-auto min-h-16 py-3 text-wrap btn-outline btn-lg"
						in:fly={{ y: 30, duration: 350, delay: i * 80 }}>
						{option}
					</button>
				{/each}
			</div>
		</div>

		<div class="mt-4 flex justify-end">
			<button class="btn btn-primary" onclick={showResults}>
				<BarChart2 />
				Show Answers
			</button>
		</div>
	</div>
{:else if phase === 'results'}
	{@const q = mockQuestions[currentQuestion]}
	{@const votes = mockVotes[currentQuestion]}
	{@const total = votes.reduce((a, b) => a + b, 0)}
	{@const maxVotes = Math.max(...votes)}
	<div
		class="fixed inset-0 z-50 flex flex-col bg-base-100 p-4 md:p-8"
		in:fly={{ x: 80, duration: 450, easing: backOut }}
		out:fly={{ x: -80, duration: 300 }}>
		<div class="mb-8 flex items-start gap-3">
			<span class="badge shrink-0 badge-lg badge-neutral">
				{currentQuestion + 1}/{mockQuestions.length}
			</span>
			<h2 class="text-xl font-bold">{q.question}</h2>
		</div>

		<div class="mx-auto flex w-full max-w-2xl flex-1 flex-col justify-center gap-5">
			{#each q.options as option, i}
				{@const pct = total > 0 ? (votes[i] / total) * 100 : 0}
				{@const isWinner = votes[i] === maxVotes}
				<div
					class="flex items-center gap-3"
					in:fly={{ x: -40, duration: 400, delay: i * 100 }}>
					<div class="w-36 shrink-0 text-right text-sm font-medium">{option}</div>
					<div class="h-10 flex-1 overflow-hidden rounded-lg bg-base-300">
						<div
							class="flex h-full items-center justify-end px-3 text-sm font-bold transition-[width] duration-1000 {isWinner
								? 'bg-primary text-primary-content'
								: 'bg-neutral text-neutral-content'}"
							style="width: {pct}%; transition-delay: {i * 120}ms;">
							{#if pct > 12}
								{votes[i]} ({pct.toFixed(0)}%)
							{/if}
						</div>
					</div>
					{#if pct <= 12}
						<span class="w-16 text-xs opacity-60">{votes[i]} ({pct.toFixed(0)}%)</span>
					{/if}
				</div>
			{/each}
		</div>

		<div class="mt-4 text-center text-sm opacity-50">{total} total answers</div>

		<div class="mt-6 flex justify-end">
			{#if currentQuestion < mockQuestions.length - 1}
				<button class="btn btn-primary" onclick={nextOrFinish}>
					<ChevronRight />
					Next Question
				</button>
			{:else}
				<button class="btn btn-success" onclick={nextOrFinish}>
					<Trophy />
					End Session
				</button>
			{/if}
		</div>
	</div>
{:else if phase === 'finished'}
	<div
		class="fixed inset-0 z-50 flex flex-col items-center justify-center bg-base-100 p-8 text-center"
		in:fade={{ duration: 600 }}>
		<div in:fly={{ y: -40, duration: 600, delay: 300, easing: backOut }}>
			<Trophy class="mx-auto mb-4 text-warning" size={80} />
			<h1 class="text-5xl font-black">Session ended!</h1>
			<p class="mt-3 text-lg opacity-60">
				{mockQuestions.length} Questions · {mockVotes
					.flatMap((v) => v)
					.reduce((a, b) => a + b, 0)} Answers
			</p>
		</div>
		<button class="btn mt-12 btn-lg btn-primary" onclick={resetToLobby}> Back to Lobby </button>
	</div>
{/if}
