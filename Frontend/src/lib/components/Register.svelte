<script lang="ts">
	import { Mail } from '@lucide/svelte';

	const steps = [
		{ label: 'E-Mail', validInput: false },
		{ label: 'Username', validInput: false },
		{ label: 'Password', validInput: false },
	];

	let state = $state(0);
	let isAtFirstStep = $derived(state === 0);
	let isAtLastStep = $derived(state === steps.length - 1);

	function next() {
		if (state < steps.length - 1) {
			state++;
		}
	}

	function prev() {
		if (state > 0) {
			state--;
		}
	}
</script>

<div class="card w-96 bg-base-100 shadow-sm card-lg">
	<div class="card-body">
		<h2 class="card-title">Register</h2>

		<ul class="steps">
			{#each steps as step, index}
				<li class={['step', state >= index && 'step-primary']}>
					{index === state ? step.label : ''}
				</li>
			{/each}
		</ul>

		{#if state === 0}
			<label class="validator input">
				<Mail />
				<input type="email" placeholder="user@mail.com" required />
			</label>
		{:else if state === 1}{:else if state === 2}{/if}

		<div class="card-actions justify-end">
			{#if !isAtFirstStep}
				<button class="btn" onclick={prev}>Previous</button>
			{/if}

			{#if !isAtLastStep}
				<button class="btn btn-primary" onclick={next} disabled={!steps[state].validInput}
					>Next</button
				>
			{/if}

			{#if isAtLastStep}
				<button class="btn btn-primary" onclick={next}>Register</button>
			{/if}
		</div>
	</div>
</div>
