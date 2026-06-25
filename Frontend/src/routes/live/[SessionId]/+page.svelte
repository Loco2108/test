<script lang="ts">
	import { page } from '$app/state';
	import {
		BodyProfileEnum,
		ColorProfileEnum,
		FaceProfileEnum,
		HatProfileEnum,
	} from '$lib/api.js';
	import AvatarCustomizer from '$lib/components/AvatarCustomizer.svelte';
	import CustomizableAvatar, {
		type CustomizableAvatarSettings,
	} from '$lib/components/CustomizableAvatar.svelte';
	import ThemeToggle from '$lib/components/ThemeToggle.svelte';
	import { getContext } from 'svelte';
	import { Play, Save, User } from '@lucide/svelte';
	import type { SessionContext } from './+layout.svelte';
	import QRCode from 'qrcode';
	import type { ClassValue } from 'svelte/elements';

	function randomEnumValue<T extends Record<string, string>>(enumObj: T): T[keyof T] {
		const values = Object.values(enumObj) as T[keyof T][];
		return values[Math.floor(Math.random() * values.length)];
	}

	let mode: 'participant' | 'presentator' = $state('presentator');
	let presenterCanvas: HTMLCanvasElement | undefined = $state();
	let modalCanvas: HTMLCanvasElement | undefined = $state();
	let fullscreenQRModal: HTMLDialogElement | undefined = $state();

	let mockUsers: { settings: CustomizableAvatarSettings; name: string; isAdmin?: boolean }[] = [
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
			isAdmin: true,
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

	let avatarSettings: CustomizableAvatarSettings = $state({
		color: randomEnumValue(ColorProfileEnum),
		hat: randomEnumValue(HatProfileEnum),
		face: randomEnumValue(FaceProfileEnum),
		body: randomEnumValue(BodyProfileEnum),
	});

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
	const { startSession } = getContext<SessionContext>('session');
</script>

{#snippet customizationCard(classes?: ClassValue)}
	<div class={['card bg-base-100 card-md', classes]}>
		<div class="card-body flex flex-col items-center">
			<div class="flex w-full justify-between">
				<h2 class="card-title">Avatar Settings</h2>
				<ThemeToggle />
			</div>

			<AvatarCustomizer bind:settings={avatarSettings} />

			<div class="mt-auto card-actions w-full">
				<form class="w-full">
					<fieldset class="mt-4 fieldset">
						<legend class="fieldset-legend">Username</legend>
						<input type="text" class="input" value="Mike Oxlong" />
					</fieldset>

					<button class="btn mt-2 btn-block btn-outline btn-neutral">
						<Save />
						Save
					</button>
				</form>
			</div>
		</div>
	</div>
{/snippet}

<!-- I AM JUST A DEBUG TOGGLE! REMOVE ME WHEN IMPLEMENTIG THE REAL THING -->
<input
	type="checkbox"
	onchange={(e) => (e.currentTarget.checked ? (mode = 'presentator') : (mode = 'participant'))}
	checked={mode === 'presentator'}
	class="toggle" />

{#if mode === 'presentator'}
	<div class="flex w-full justify-center">
		<div class="card bg-base-100 shadow card-md">
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

	<div
		class="mt-4 flex w-full flex-col items-stretch justify-center gap-2 md:flex-row md:items-stretch">
		{@render customizationCard('')}

		<div class="card bg-base-100 card-md md:min-w-96">
			<div class="card-body">
				<h2 class="card-title">Settings</h2>

				<label class="flex cursor-pointer gap-2">
					<input type="checkbox" class="toggle" />
					<span class="label-text">Setting 1</span>
				</label>
				<label class="flex cursor-pointer gap-2">
					<input type="checkbox" class="toggle" />
					<span class="label-text">Setting 2</span>
				</label>
				<label class="flex cursor-pointer gap-2">
					<input type="checkbox" class="toggle" checked />
					<span class="label-text">Setting 3</span>
				</label>
				<fieldset class="fieldset">
					<legend class="fieldset-legend">Setting 5</legend>
					<input type="text" class="input w-full" />
				</fieldset>
				<fieldset class="fieldset">
					<legend class="fieldset-legend">Setting 5</legend>
					<input type="text" class="input w-full" />
				</fieldset>
				<button class="btn btn-block btn-outline btn-neutral">
					<Save />
					Save settings
				</button>
			</div>
		</div>
	</div>

	<div class="mx-auto mt-8 w-fit">
		<button
			class="btn btn-lg btn-primary"
			onclick={() => {
				startSession();
			}}>
			<Play />
			Start Session
		</button>
	</div>
{/if}

{#if mode === 'participant'}
	{@render customizationCard('w-fit mx-auto')}
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
			<div class="min-w-0 flex-1 truncate text-2xl font-bold">
				{user.name}
				{#if user.isAdmin}
					<div class="badge badge-info">Presentator</div>
				{/if}
			</div>
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
