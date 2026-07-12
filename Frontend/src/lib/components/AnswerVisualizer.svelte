<script lang="ts">
	import type { AnswerDisplayDto, AnswerOptionDto } from '$lib/wsClient/Backend.Dto';
	import { QuestionTypeEnum } from '$lib/wsClient/Backend.Models.Enums';
	import { slide } from 'svelte/transition';
	import type { Option } from './MultipleChoice.svelte';
	import WordCloud from './WordCloud.svelte';
	import NumberScale from './NumberScale.svelte';
	import NumberScaleVisualizer from './NumberScaleVisualizer.svelte';

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

<div class="mb-16 self-start text-2xl font-bold opacity-50">
	Participants answered:
	<div class="ml-1 badge badge-outline badge-lg">{answers?.totalParticipantsAnswered ?? 0}</div>
	/
	<div class="ml-1 badge badge-outline badge-lg">{participantCount ?? 0}</div>
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
	<div class="flex max-h-full w-full flex-col gap-2 md:max-w-120">
		{#each answers?.freeTextResults ?? [] as answer (answer.id)}
			<div class="chat-end chat scrollbar-none shadow" in:slide>
				<div class="chat-bubble chat-bubble-primary whitespace-pre-wrap">
					{answer.text}
				</div>
			</div>
		{/each}
	</div>
{:else if questionType === QuestionTypeEnum.NumberScale}
	<NumberScaleVisualizer {answers} {scaleMinValue} {scaleMaxValue} />
{/if}
