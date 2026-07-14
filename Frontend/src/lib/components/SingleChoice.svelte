<script lang="ts">
	import type { ClassValue } from 'svelte/elements';
	import type { Option } from '$lib/components/MultipleChoice.svelte';

	type Props = {
		options?: Option[];
		value?: number;
		class?: ClassValue;
		style?: string;
	};

	let { options = [], value = $bindable(), class: classes, style }: Props = $props();

	let radioName = crypto.randomUUID();
</script>

<div class={['flex w-full flex-col items-center gap-2 md:max-w-120', classes]} {style}>
	{#each options as option, index}
		<label
			class={[
				'flex w-full cursor-pointer gap-2 rounded-box bg-base-100 p-2',
				option.checked && 'outline outline-primary',
			]}>
			<input
				type="radio"
				class="radio radio-primary"
				name={radioName}
				value={index}
				bind:group={value} />
			{option.label}
		</label>
	{/each}
</div>
