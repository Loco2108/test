<script lang="ts">
	import { goto } from '$app/navigation';
	import { scrollIntoViewOnMount } from '$lib/actions/scrollaction.js';
	import type { GetFolderResponseDto, GetSurveyResponseDto } from '$lib/api';
	import NewSurveyDialog from '$lib/components/NewSurveyDialog.svelte';
	import Pagination from '$lib/components/Pagination.svelte';
	import {
		Archive,
		BadgePlus,
		ChartNoAxesCombined,
		ChevronRight,
		Cog,
		Folder,
		Form,
		Play,
		Plus,
		Scroll,
		X,
	} from '@lucide/svelte';

	let { data } = $props();

	let menuTabs = [
		{ label: 'surveys', icon: Form },
		{ label: 'Sessions', icon: ChartNoAxesCombined },
		{ label: 'Templates', icon: Plus },
		{ label: 'Archive', icon: Archive },
	];

	let activeViewId = $derived(data.currentView);

	let surveys = $derived<GetSurveyResponseDto[]>(data.surveys);
	let folder = $derived<GetFolderResponseDto[]>(data.folder);
	let editingItem = $state<string | null>(null);
	let editingSurvey = $derived(
		surveys.find((s) => s.surveyId === editingItem) ??
			folder.flatMap((f) => f.surveys ?? []).find((s) => s.surveyId === editingItem) ??
			null,
	);
	let newSurveyDialogRef: HTMLDialogElement | undefined = $state();

</script>

<div class="flex w-full flex-col gap-4 md:flex-row">
	<ul class="menu h-fit w-full bg-base-100 shadow-sm md:sticky md:top-4 md:flex-1">
		<li>
			<h2 class="menu-title">Library</h2>
			<ul>
				{#each menuTabs as tab}
					<li class={activeViewId === tab.label.toLowerCase() ? 'menu-active' : ''}>
						<a href="?view={tab.label.toLowerCase()}">
							<tab.icon size={16} />
							{tab.label}
						</a>
					</li>
				{/each}
			</ul>
			<button class="btn mt-2 btn-primary" onclick={() => newSurveyDialogRef?.showModal()}>
				<BadgePlus />
				New Survey
			</button>
		</li>
	</ul>

	<div class="card flex-5 bg-base-100 shadow-sm card-md">
		<div class="card-body">
			<h2 class="card-title">
				{activeViewId.charAt(0).toUpperCase() + activeViewId.slice(1)}
			</h2>

			{#if activeViewId === 'surveys'}
				<ul class="menu w-full rounded-box">
					{#each folder as f (f.folderId)}
						<li>
							<details>
								<summary>
									<Folder size={16} />
									{f.name}
								</summary>
								<ul>
									{#each f.surveys ?? [] as survey (survey.surveyId)}
										<li>
											<button
												onclick={() => (editingItem = survey.surveyId)}
												class={[editingItem === survey.surveyId && 'bg-base-300']}>
												<Scroll size={16} />
												{survey.title}
												<ChevronRight size={16} />
											</button>
										</li>
									{/each}
								</ul>
							</details>
						</li>
					{/each}
					{#each surveys as survey (survey.surveyId)}
						<li>
							<button
								onclick={() => (editingItem = survey.surveyId)}
								class={[editingItem === survey.surveyId && 'bg-base-300']}>
								<Scroll size={16} />
								{survey.title}
								<ChevronRight size={16} />
							</button>
						</li>
					{/each}
				</ul>

				<div class="divider"></div>

				<Pagination numPages={5} currentPage={1} />
			{:else if activeViewId === 'sessions'}
				<p>Here will be the Sessions</p>
			{:else if activeViewId === 'templates'}
				<p>Here will be the Templates</p>
			{:else if activeViewId === 'archive'}
				<p>Here will be the Archive</p>
			{/if}
		</div>
	</div>

	{#if editingSurvey}
		<div
			class="card h-fit flex-3 bg-base-100 shadow-sm card-md"
			use:scrollIntoViewOnMount={editingSurvey.surveyId}>
			<div class="card-body">
				<div class="flex justify-between">
					<h2 class="card-title justify-between">
						<Scroll />
						{editingSurvey.title}
					</h2>
					<button
						class="btn btn-ghost btn-sm btn-neutral"
						onclick={() => (editingItem = null)}
						aria-label="Close">
						<X size={16} />
					</button>
				</div>

				<div class="p-4">
					Here will be quick settings that will be saved immediatly

					<fieldset
						class="fieldset w-full rounded-box border border-base-300 bg-base-100 p-4">
						<legend class="fieldset-legend">Some Quick Options</legend>
						<label class="label">
							<input type="checkbox" checked class="toggle" />
							Some
						</label>

						<label class="label">
							<input type="checkbox" class="toggle" />
							Other
						</label>
					</fieldset>
				</div>

				<div class="card-actions flex-col">
					<button
						class="btn btn-block btn-outline btn-sm btn-secondary"
						onclick={() => goto(`/app/surveys/${editingSurvey!.surveyId}`)}
						><Cog size={20} /> Full Settings</button>
					<button class="btn btn-block btn-primary"><Play /> Start Session </button>
				</div>
			</div>
		</div>
	{/if}
</div>

<NewSurveyDialog bind:ref={newSurveyDialogRef} />
