<script>
	import { authStore } from '$lib/authStore.svelte';
	import { BadgePlus, BadgeQuestionMark, CirclePlus, MessagesSquare, Zap } from '@lucide/svelte';

	let templates = [
		{
			label: 'Quick Poll',
			description: 'A simple single-question poll to gauge immediate reactions.',
			icon: Zap,
		},
		{
			label: 'Feedback Form',
			description: 'Collect detailed Feedback after a meeting or an event.',
			icon: MessagesSquare,
		},
		{
			label: 'AnotherTemplate',
			description: "We are not sure what to put here, but we'll figure it out!",
			icon: BadgeQuestionMark,
		},
	];
</script>

<div class="mt-4 flex w-full flex-col justify-between gap-2 md:mt-16 md:flex-row md:px-32">
	<div>
		<h2 class="text-4xl font-bold">
			Welcome Back, {authStore.user?.username}
		</h2>
		<p class="text-lg text-secondary">Gather Insights like never before!</p>
	</div>

	<button class="btn w-fit btn-xl btn-primary">
		<BadgePlus />
		CREATE NEW SURVEY
	</button>
</div>

<h3 class="mt-16 text-2xl font-bold">Quick Start</h3>

<div class="flex w-full gap-4">
	{#each templates as template}
		{@const Icon = template.icon}

		<div class="card w-96 bg-base-100 shadow-sm card-md">
			<div class="card-body">
				<div class="w-fit rounded-sm bg-accent p-2 text-accent-content">
					<Icon />
				</div>
				<h2 class="card-title">{template.label}</h2>
				<p>
					{template.description}
				</p>
				<div class="card-actions justify-end">
					<button class="btn btn-block btn-outline btn-accent">
						<CirclePlus />
						Create
					</button>
				</div>
			</div>
		</div>
	{/each}
</div>
