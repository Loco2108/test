<script lang="ts">
	import { authStore } from '$lib/authStore.svelte';
	import SessionCard from '$lib/components/SessionCard.svelte';
	import {
		ArrowRight,
		BadgePlus,
		BadgeQuestionMark,
		CirclePlus,
		MessagesSquare,
		Zap,
	} from '@lucide/svelte';
	import type { ComponentProps } from 'svelte';

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

	let demoSessions: ComponentProps<typeof SessionCard>[] = [
		{
			title: 'Svelte 5 vs. React',
			color: 'primary',
			subtitle: 'Frontend Preferences',
			participantCount: 42,
			runAt: new Date(2026, 4, 28),
		},
		{
			title: 'C# & ASP.NET Architecture',
			color: 'success',
			subtitle: 'Live Q&A',
			participantCount: 15,
			runAt: new Date(),
		},
		{
			title: 'Docker Deployment Quiz',
			color: 'error',
			subtitle: 'DevOps Module',
			participantCount: 8,
			runAt: new Date(2026, 3, 10),
		},
		{
			title: 'Milestone 1 (M1) Check-in',
			color: 'accent',
			subtitle: 'Planning Phase',
			participantCount: 3,
			runAt: new Date(2026, 4, 16),
		},
		{
			title: 'MySQL Performance Tuning',
			color: 'info',
			subtitle: 'Database Workshop',
			participantCount: 27,
			runAt: new Date(2026, 5, 2),
		},
		{
			title: 'NodeJS Seminar Feedback',
			color: 'warning',
			subtitle: 'General Feedback',
			participantCount: 19,
			runAt: new Date(2026, 4, 25),
		},
		{
			title: 'Mentimeter Alternatives',
			color: 'secondary',
			subtitle: 'Feature Voting',
			participantCount: 34,
			runAt: new Date(2026, 6, 12),
		},
		{
			title: 'Sprint Retrospective',
			color: 'accent',
			subtitle: 'Team Feedback',
			participantCount: 4,
			runAt: new Date(2026, 5, 20),
		},
	];
</script>

<div
	class="mt-4 flex w-full flex-col items-center justify-between gap-2 md:mt-16 md:flex-row md:px-16">
	<div>
		<h2 class="text-4xl font-bold">
			Welcome Back, {authStore.user?.username}
		</h2>
		<p class="text-lg text-secondary">Gather Insights like never before!</p>
	</div>

	<button class="btn w-fit btn-lg btn-primary">
		<BadgePlus />
		CREATE NEW SURVEY
	</button>
</div>

<h3 class="mt-16 mb-2 text-2xl font-bold">Quick Start</h3>

<div class="flex gap-4 overflow-auto">
	{#each templates as template}
		{@const Icon = template.icon}

		<div class="card min-w-70 bg-base-100 shadow-sm card-md md:min-w-96">
			<div class="card-body">
				<div class="w-fit rounded-sm bg-accent p-2 text-accent-content">
					<Icon />
				</div>
				<h2 class="card-title">{template.label}</h2>
				<p>
					{template.description}
				</p>
				<div class="card-actions">
					<button class="btn btn-block btn-outline btn-accent">
						<CirclePlus />
						Create
					</button>
				</div>
			</div>
		</div>
	{/each}
</div>

<div class="mt-16 mb-2 flex gap-2">
	<h3 class="text-2xl font-bold">Recent Sessions</h3>
	<a class="btn btn-outline btn-sm btn-secondary" href="/app/library?view=sessions">
		<ArrowRight size={16} />
		View All
	</a>
</div>

<div class="flex gap-4 overflow-auto">
	{#each demoSessions as session}
		<SessionCard {...session} />
	{/each}
</div>
