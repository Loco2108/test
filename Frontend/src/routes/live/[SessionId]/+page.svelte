<script lang="ts">
	import { page } from '$app/state';
	import QRCode from 'qrcode';
	import { onMount } from 'svelte';

	let mode: 'participant' | 'presentator' = $state('presentator');
	let canvas: HTMLCanvasElement | undefined = $state();

	$effect(() => {
		if (mode === 'presentator' && canvas) {
			const url = page.url.href;
			QRCode.toCanvas(canvas, url);
		}
	});

	let { params } = $props();

	onMount(() => {
		const url = page.url.href;
		if (mode === 'presentator') {
			QRCode.toCanvas(canvas, url);
		}
	});
</script>

<input
	type="checkbox"
	onchange={(e) => (e.currentTarget.checked ? (mode = 'presentator') : (mode = 'participant'))}
	checked={mode === 'presentator'}
	class="toggle" />

{#if mode === 'presentator'}
	<div class="flex w-full justify-center">
		<div class="card bg-base-100 card-md">
			<div class="card-body">
				<div class="flex flex-col items-center gap-2 md:flex-row">
					<span class="text-6xl font-bold">{params.SessionId}</span>

					<div class="divider divider-vertical md:divider-horizontal"></div>

					<canvas
						bind:this={canvas}
						class="min-h-70 min-w-70"
						style="image-rendering: pixelated;"></canvas>
				</div>
			</div>
		</div>
	</div>
{/if}
