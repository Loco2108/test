<script lang="ts">
	import type { Poll } from '$lib/api';
	import { apiClient } from '$lib/apiClient';
	import { PollConnection } from '$lib/signalr.svelte';
	import { _fetchQuestions } from './+page';

	let { data } = $props();

	const pollService = new PollConnection('http://localhost:5202/pollhub', onUpdateReceived);

	$effect(() => {
		pollService.start('some');
	});

	let questionString = $state('');
	let questions: Poll[] = $state(data.initialQuestions);
	let newQuestionsAvailable = $state(false);

	function submitQuestion() {
		if (!questionString) return;

		const currentConnectionId = pollService.connection.connectionId;

		apiClient.api
			.v1TestCreate(
				{ question: questionString },
				{ headers: { 'X-Connection-Id': currentConnectionId } }
			)
			.then((res) => {
				questionString = '';
				if (res.status === 200) {
					console.log('Successfully uploaded question');
					questions.push(res.data);
				}
			})
			.catch(() => {
				console.log('Error Uploading data');
			});
	}

	function onUpdateReceived(res: Poll) {
		newQuestionsAvailable = true;

		questions.push(res);
	}
</script>

<div class="p-4">
	<div class="flex gap-2">
		<div class="join">
			<div>
				<label class="input join-item">
					<input type="text" required bind:value={questionString} />
				</label>
				<div class="validator-hint hidden">Enter question</div>
			</div>
			<button class="btn join-item btn-neutral" onclick={submitQuestion}>Submit</button>
		</div>

		<button
			class="btn btn-primary"
			onclick={async () => {
				questions = await _fetchQuestions();
				newQuestionsAvailable = false;
			}}>Get Questions</button
		>

		{#if newQuestionsAvailable}
			<div class="flex items-center gap-1">
				<div class="inline-grid *:[grid-area:1/1]">
					<div class="status animate-ping status-success"></div>
					<div class="status status-success"></div>
				</div>
				New Questions Available
			</div>
		{/if}
	</div>

	<ul class="list rounded-box bg-base-100 shadow-md">
		<li class="p-4 pb-2 text-xs tracking-wide opacity-60">Questions</li>
		{#each questions as question}
			<li class="list-row">
				{question.question}
			</li>
		{/each}
	</ul>
</div>
