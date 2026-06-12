<script lang="ts">
	import Pagination from '$lib/components/Pagination.svelte';
	import { ChartNoAxesCombined, Scroll, User } from '@lucide/svelte';

	let mockSurveys = [
		{
			name: 'C# & ASP.NET Architecture',
			sessions: 25,
		},
		{
			name: 'NodeJS Seminar Feedback',
			sessions: 20,
		},
		{
			name: 'Docker Deployment Quiz',
			sessions: 15,
		},
		{
			name: 'Svelte 5 vs. React',
			sessions: 10,
		},
		{
			name: 'Sprint Retrospective',
			sessions: 8,
		},
	];

	let mockSessions = [
		{
			name: 'C# & ASP.NET Architecture',
			participants: 24,
			date: new Date('2024-06-01'),
		},
		{
			name: 'NodeJS Seminar Feedback',
			participants: 20,
			date: new Date('2024-06-05'),
		},
		{
			name: 'Svelte 5 vs. React',
			participants: 10,
			date: new Date('2024-06-15'),
		},
		{
			name: 'Sprint Retrospective',
			participants: 8,
			date: new Date('2024-06-20'),
		},
		{
			name: 'Docker Deployment Quiz',
			participants: 5,
			date: new Date('2024-06-10'),
		},
	];
</script>

<div class="mb-4 flex w-full justify-center">
	<div class="stats w-full stats-vertical bg-base-100 shadow md:w-fit md:stats-horizontal">
		<div class="stat text-primary">
			<div class="stat-figure text-secondary">
				<Scroll size={28} />
			</div>
			<div class="stat-title">Surveys</div>
			<div class="stat-value">8</div>
			<div class="stat-desc">↗︎ 2 in the last 30 days</div>
		</div>

		<div class="stat text-accent">
			<div class="stat-figure text-secondary">
				<ChartNoAxesCombined size={28} />
			</div>
			<div class="stat-title">Sessions</div>
			<div class="stat-value">116</div>
			<div class="stat-desc">↗︎ 10 in the last 30 days</div>
		</div>

		<div class="stat text-warning">
			<div class="stat-figure text-secondary">
				<User size={28} />
			</div>
			<div class="stat-title">Participants</div>
			<div class="stat-value">628</div>
			<div class="stat-desc">↗︎ 42 in the last 30 days</div>
		</div>
	</div>
</div>

<div class="tabs-lift tabs">
	<input type="radio" name="my_tabs_3" class="tab" aria-label="Most used Surveys" checked />
	<div class="tab-content border-base-300 bg-base-100 p-6">
		<div class="mb-2 overflow-x-auto">
			<table class="table">
				<thead>
					<tr>
						<th></th>
						<th>Survey Name</th>
						<th># Sessions</th>
					</tr>
				</thead>
				<tbody>
					{#each mockSurveys as survey, index}
						<tr>
							<th>{index + 1}</th>
							<td>{survey.name}</td>
							<td>{survey.sessions}</td>
						</tr>
					{/each}
				</tbody>
			</table>
		</div>

		<div class="divider"></div>

		<Pagination numPages={5} currentPage={1} />
	</div>

	<input type="radio" name="my_tabs_3" class="tab" aria-label="Participants in Sessions" />
	<div class="tab-content border-base-300 bg-base-100 p-6">
		<div class="mb-2 overflow-x-auto">
			<table class="table">
				<thead>
					<tr>
						<th></th>
						<th>Session Name</th>
						<th># Participants</th>
						<th>Date</th>
					</tr>
				</thead>
				<tbody>
					{#each mockSessions as session, index}
						<tr>
							<th>{index + 1}</th>
							<td>{session.name}</td>
							<td>{session.participants}</td>
							<td
								>{session.date.toLocaleDateString('en-US', {
									year: 'numeric',
									month: 'long',
									day: '2-digit',
								})}</td>
						</tr>
					{/each}
				</tbody>
			</table>
		</div>

		<div class="divider"></div>

		<Pagination numPages={5} currentPage={1} />
	</div>
</div>
