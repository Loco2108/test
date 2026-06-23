<script lang="ts">
	import { page } from '$app/state';
	import {
		BodyProfileEnum,
		ColorProfileEnum,
		FaceProfileEnum,
		HatProfileEnum,
	} from '$lib/api.js';
	import CustomizableAvatar, {
		type CustomizableAvatarSettings,
	} from '$lib/components/CustomizableAvatar.svelte';
	import { Play, User } from '@lucide/svelte';
	import QRCode from 'qrcode';

	let mode: 'participant' | 'presentator' = $state('presentator');
	let presenterCanvas: HTMLCanvasElement | undefined = $state();
	let modalCanvas: HTMLCanvasElement | undefined = $state();
	let fullscreenQRModal: HTMLDialogElement | undefined = $state();

	let mockUsers: { settings: CustomizableAvatarSettings; name: string }[] = [
		{
			settings: {
				color: ColorProfileEnum.Green,
				hat: HatProfileEnum.Hat04,
				face: FaceProfileEnum.Face04,
				body: BodyProfileEnum.Body04,
			},
			name: 'Mike Oxlong',
		},
		{
			settings: {
				color: ColorProfileEnum.Red,
				hat: HatProfileEnum.Hat02,
				face: FaceProfileEnum.Face01,
				body: BodyProfileEnum.Body03,
			},
			name: 'Fixi Hartmann',
		},
		{
			settings: {
				color: ColorProfileEnum.Yellow,
				hat: HatProfileEnum.Hat05,
				face: FaceProfileEnum.Face05,
				body: BodyProfileEnum.Body05,
			},
			name: 'Chris P. Bacon',
		},
		{
			settings: {
				color: ColorProfileEnum.Blue,
				hat: HatProfileEnum.Hat01,
				face: FaceProfileEnum.Face01,
				body: BodyProfileEnum.Body01,
			},
			name: 'xX_UltraGamer_HD_1080p_UHD_HDR_Ryzen10-6969x_GeForce-RTX-5090_Xx',
		},
	];

	$effect(() => {
		const url = page.url.href;
		if (mode === 'presentator') {
			if (presenterCanvas) {
				QRCode.toCanvas(presenterCanvas, url);
			}

			if (modalCanvas) {
				QRCode.toCanvas(modalCanvas, url);
			}
		}
	});

	let { params } = $props();

	function setFullscreen() {
		var elem = document.documentElement;

		var rfs = elem.requestFullscreen;

		if (typeof rfs !== undefined && rfs) {
			rfs.call(elem);
		}
	}
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
					<div>
						<div>Join on</div>
						<div class="text-lg font-bold">{page.url.origin}/live</div>
					</div>

					<div class="divider divider-vertical md:divider-horizontal"></div>

					<div>
						<div>Room Code:</div>
						<span class="text-6xl font-bold text-primary">{params.SessionId}</span>
					</div>

					<div class="divider divider-vertical md:divider-horizontal"></div>

					<button
						class="btn h-full p-2 btn-ghost"
						aria-label="Fullscreen QR-Code"
						onclick={() => fullscreenQRModal?.showModal()}>
						<canvas
							bind:this={presenterCanvas}
							class="min-h-30 min-w-30"
							style="image-rendering: pixelated;"></canvas>
					</button>
				</div>
			</div>
		</div>
	</div>

	<div class="mx-auto mt-8 w-fit">
		<button class="btn btn-lg btn-primary" onclick={setFullscreen}>
			<Play />
			Start Session
		</button>
	</div>
{/if}

<ul class="list mx-auto mt-8 w-fit max-w-full rounded-box bg-base-100 shadow-md md:min-w-96">
	<li class="p-4 pb-2 text-xs tracking-wide opacity-60">Participants</li>

	{#if !mockUsers.length}
		<li class="list-row flex items-center opacity-60">
			<div class="avatar rounded-full bg-base-300 p-4">
				<User />
			</div>
			<div class="truncate">No participants yet</div>
		</li>
	{/if}

	{#each mockUsers as user}
		<li class="list-row flex items-center">
			<div class="shrink-0">
				<CustomizableAvatar size={15} settings={user.settings} />
			</div>
			<div class="min-w-0 flex-1 truncate text-2xl font-bold">{user.name}</div>
		</li>
	{/each}
</ul>

<dialog class="modal modal-bottom md:modal-middle" bind:this={fullscreenQRModal}>
	<div class="modal-box flex flex-col items-center justify-center p-6">
		<div class="aspect-square w-full max-w-xs md:max-w-sm">
			<canvas
				bind:this={modalCanvas}
				class="block min-h-full min-w-full"
				style="image-rendering: pixelated;">
			</canvas>
		</div>

		<div class="modal-action w-full">
			<form method="dialog" class="w-full">
				<button class="btn btn-block btn-outline">Schließen</button>
			</form>
		</div>
	</div>

	<form method="dialog" class="modal-backdrop">
		<button>close</button>
	</form>
</dialog>
