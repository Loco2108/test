<script lang="ts">
	import { goto } from '$app/navigation';
	import { HeartCrack, LoaderCircle, X } from '@lucide/svelte';
	import type { ClassValue } from 'svelte/elements';
	import { addToast } from './Toast/Toast.svelte';

	type Props = {
		class?: ClassValue;
		style?: string;
		ref?: HTMLDialogElement;
	};

	let { class: classes, style, ref = $bindable() }: Props = $props();

	let formRef: HTMLFormElement | null = $state(null);
	let isLoading = $state(false);

	function reset() {
		formRef?.reset();
	}

	async function createSurvey() {
		console.error("Survey creation hasn't been implemented yet");

		isLoading = true;
		await new Promise((f) => setTimeout(f, 1000));
		isLoading = false;
		addToast({
			type: 'error',
			label: 'Survey Creation has not been implemented yet',
			icon: HeartCrack,
		});
	}
</script>

<dialog class={['modal', classes]} {style} bind:this={ref} onclose={reset}>
	<div class="modal-box">
		<form method="dialog">
			<button class="btn absolute top-2 right-2 btn-ghost btn-sm" disabled={isLoading}>
				<X />
			</button>
		</form>
		<h3 class="text-lg font-bold">New Survey</h3>
		<div class="p-4">
			<form
				class=""
				bind:this={formRef}
				onsubmit={async (e) => {
					e.preventDefault();
					await createSurvey();
					goto(`/app/surveys/${crypto.randomUUID()}`);
				}}
				onreset={() => {
					isLoading = false;
				}}>
				<fieldset class="fieldset">
					<legend class="fieldset-legend">Survey Title</legend>
					<input type="text" class="input w-full" placeholder="My Survey" required />
				</fieldset>

				<fieldset class="fieldset">
					<legend class="fieldset-legend">Survey Description</legend>
					<input type="text" class="input w-full" placeholder="My Description" />
				</fieldset>

				<div class="mt-4 flex flex-col gap-2">
					<button
						class="btn btn-secondary"
						type="reset"
						onclick={() => ref?.close()}
						disabled={isLoading}>
						Cancel
					</button>
					<button class="btn btn-primary" type="submit" disabled={isLoading}>
						{#if isLoading}
							<LoaderCircle class="animate-spin" />
						{/if}
						Create Survey
					</button>
				</div>
			</form>
		</div>
	</div>
</dialog>
