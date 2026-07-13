<script lang="ts">
	import type { AnswerDisplayDto } from '$lib/wsClient/Backend.Dto';
	import { QuestionTypeEnum } from '$lib/wsClient/Backend.Models.Enums';
	import { slide } from 'svelte/transition';
	import type { Option } from './MultipleChoice.svelte';
	import WordCloud from './WordCloud.svelte';
	import NumberScaleVisualizer from './NumberScaleVisualizer.svelte';
	import { Quote } from '@lucide/svelte';
	import { flip } from 'svelte/animate';

	type Props = {
		answers?: AnswerDisplayDto;
		questionType?: QuestionTypeEnum;
		participantCount?: number;
		choiceOptions?: Option[];
		scaleMinValue?: number;
		scaleMaxValue?: number;
	};

	let {
		answers,
		questionType,
		participantCount,
		choiceOptions = [],
		scaleMinValue,
		scaleMaxValue,
	}: Props = $props();
</script>

<div class="mb-12 flex items-center gap-2 self-start text-sm text-base-content/50">
	<span>Participants:</span>
	<div class="badge badge-outline badge-sm text-base-content/70">
		{answers?.totalParticipantsAnswered ?? 0} / {participantCount ?? 0}
	</div>
</div>

{#if questionType === QuestionTypeEnum.SingleChoice || questionType === QuestionTypeEnum.MultipleChoice}
	{@const totalAnswers = answers?.choiceResults.reduce((acc, curr) => acc + curr.count, 0) ?? 0}

	<div class="flex w-full flex-col gap-4">
		{#each choiceOptions as item}
			{@const answerItem = answers?.choiceResults.find(
				(x) => x.answerOption.id === item.answerOption.id
			)}
			{@const percentage =
				totalAnswers === 0 ? 0 : ((answerItem?.count ?? 0) / totalAnswers) * 100}

			<div>
				<div class="mb-2 flex items-center gap-2">
					<span class="text-lg font-bold">
						{item.answerOption.description}
					</span>

					<div class="ml-1 badge badge-outline">{answerItem?.count ?? 0}</div>
				</div>

				<div
					class="relative min-h-16 w-full overflow-hidden rounded-box bg-base-100 shadow-md">
					<div
						class={[
							'absolute left-0 h-full w-full transition-all',
							percentage < 33.33 && 'bg-error',
							percentage >= 33.33 && percentage < 66.66 && 'bg-warning',
							percentage >= 66.66 && 'bg-success',
						]}
						style="transform: translateX(calc(-100% + {percentage}%));">
					</div>
				</div>
			</div>
		{/each}
	</div>
{:else if questionType === QuestionTypeEnum.WordCloud}
	<WordCloud {answers} />
{:else if questionType === QuestionTypeEnum.FreeText}
	<div class="grid w-full grid-cols-1 gap-4 sm:grid-cols-2 md:max-w-5xl">
		{#each answers?.freeTextResults ?? [] as answer (answer.id)}
			<div
				class="card bg-base-100 text-balance wrap-anywhere text-base-content shadow"
				in:slide
				animate:flip={{ duration: 400 }}>
				<div class="card-body whitespace-pre-wrap">
					<Quote class="fill-base-content text-base-content/0" />
					<p class="text-lg leading-relaxed italic">
						{answer.text}
					</p>
				</div>
			</div>
		{/each}
	</div>
{:else if questionType === QuestionTypeEnum.NumberScale}
	<NumberScaleVisualizer {answers} {scaleMinValue} {scaleMaxValue} />
{/if}
