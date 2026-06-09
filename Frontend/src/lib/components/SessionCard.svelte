<script lang="ts">
	import { goto } from '$app/navigation';
	import { ChartNoAxesCombined, User } from '@lucide/svelte';
	import type { ClassValue } from 'svelte/elements';

	type Props = {
		id?: string;
		color?: 'primary' | 'secondary' | 'accent' | 'info' | 'success' | 'warning' | 'error';
		title?: string;
		subtitle?: string;
		participantCount?: number;
		runAt?: Date;
		class?: ClassValue;
		style?: string;
	};

	let {
		id,
		color = 'primary',
		title,
		subtitle,
		participantCount,
		runAt,
		class: classes,
		style,
	}: Props = $props();
</script>

<div
	class={['card min-w-70 overflow-hidden bg-base-100 shadow-sm card-md md:min-w-96', classes]}
	{style}>
	<div
		class={[
			'min-h-2 w-full rounded-b-sm',
			color === 'primary' && 'bg-primary',
			color === 'secondary' && 'bg-secondary',
			color === 'accent' && 'bg-accent',
			color === 'info' && 'bg-info',
			color === 'success' && 'bg-success',
			color === 'warning' && 'bg-warning',
			color === 'error' && 'bg-error',
		]}>
	</div>

	<div class="card-body">
		<div class="card-title flex-col items-start gap-0">
			{title}
			{#if subtitle}
				<div class="text-sm text-secondary">{subtitle}</div>
			{/if}
		</div>

		<div class="avatar-group -space-x-6">
			{#each Array.from({ length: Math.random() * 20 + 1 }) as _, i}
				<div class="avatar">
					<div class="w-10 bg-info">
						<User class="m-auto h-full" />
					</div>
				</div>
			{/each}
		</div>

		<div class="divider m-0"></div>

		{#if participantCount !== undefined || runAt}
			<div class="flex justify-between gap-2">
				{#if participantCount !== undefined}
					<div class="flex flex-col items-start">
						Participants
						<kbd class="kbd w-fit">{participantCount}</kbd>
					</div>
				{/if}

				{#if runAt}
					<div class="flex flex-col items-end">
						Run at
						<kbd class="kbd w-fit">{runAt.toLocaleDateString()}</kbd>
					</div>
				{/if}
			</div>
		{/if}

		<button
			class={[
				'btn mt-auto btn-block',
				color === 'primary' && 'btn-primary',
				color === 'secondary' && 'btn-secondary',
				color === 'accent' && 'btn-accent',
				color === 'info' && 'btn-info',
				color === 'success' && 'btn-success',
				color === 'warning' && 'btn-warning',
				color === 'error' && 'btn-error',
			]}
			onclick={() => goto(`/app/sessions/${id}`)}>
			<ChartNoAxesCombined />
			Open
		</button>
	</div>
</div>
