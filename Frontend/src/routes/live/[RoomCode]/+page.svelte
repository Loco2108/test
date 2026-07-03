<script lang="ts">
	import { page } from '$app/state';
	import AvatarCustomizer from '$lib/components/AvatarCustomizer.svelte';
	import CustomizableAvatar, {
		type CustomizableAvatarSettings,
	} from '$lib/components/CustomizableAvatar.svelte';
	import ThemeToggle from '$lib/components/ThemeToggle.svelte';
	import { getContext } from 'svelte';
	import { Play, Save } from '@lucide/svelte';
	import type { SessionContext } from './+layout.svelte';
	import QRCode from 'qrcode';
	import type { ClassValue } from 'svelte/elements';
	import UserAvatar from '$lib/components/UserAvatar.svelte';
	import { ParticipantRole } from '$lib/wsClient/Backend.Models.Enums.js';
	import { addToast } from '$lib/components/Toast/Toast.svelte';

	let { data, params } = $props();
	let hub = $derived(data.hub);

	let mode: 'participant' | 'presentator' = $state('presentator');
	let presenterCanvas: HTMLCanvasElement | undefined = $state();
	let modalCanvas: HTMLCanvasElement | undefined = $state();
	let fullscreenQRModal: HTMLDialogElement | undefined = $state();

	let avatarSettings: CustomizableAvatarSettings | undefined = $derived(
		hub.state?.userInformation?.profilePicture
	);
	let participantName: string | undefined = $derived(hub.state?.userInformation?.name);

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

	async function updateParticipant() {
		let res = await hub.updateParticipantData(participantName, avatarSettings);

		if (res) {
			addToast({ label: 'Successfully updated profile data', type: 'success' });
			return;
		}

		addToast({ label: 'Error updating profile data', type: 'error' });
	}

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
				<form class="w-full" onsubmit={updateParticipant}>
					<fieldset class="mt-4 fieldset">
						<legend class="fieldset-legend">Username</legend>
						<input
							type="text"
							class="input"
							bind:value={participantName}
							maxlength="64" />
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

{#if hub.state?.role == ParticipantRole.Presenter}
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
						<span class="text-6xl font-bold text-primary">{params.RoomCode}</span>
					</div>

					<div class="divider divider-vertical md:divider-horizontal"></div>

					<button
						class="btn h-fit btn-ghost p-2"
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

	<div class="card card-md">
		<div class="mx-auto card-body flex w-fit flex-row items-center">
			<div class="shrink-0">
				<UserAvatar size={15} profilePictureUrl={hub.state?.presenter.profilePictureUrl} />
			</div>
			<div class="min-w-0 flex-1 truncate text-2xl font-bold">
				{hub.state?.presenter.userName}
				<div class="badge badge-info">Presentator</div>
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

{#if hub.state?.role == ParticipantRole.Participant}
	<div class="card mx-auto my-4 w-fit bg-base-100 shadow card-md">
		<div class="card-body">
			<h2 class="card-title">Session</h2>

			<span class="text-xl font-bold text-primary">
				{params.RoomCode}
			</span>
		</div>
	</div>

	{@render customizationCard('w-fit mx-auto')}
{/if}

<ul class="list mx-auto mt-8 w-fit max-w-full rounded-box bg-base-100 shadow-md md:min-w-96">
	<li class="p-4 pb-2 text-xs tracking-wide opacity-60">
		<span>Participants</span>
		<kbd class="kbd kbd-sm">{hub.state?.participants.length}</kbd>
	</li>

	{#each hub.state?.participants as participant}
		{@const profilePicture = participant.profilePicture}
		<li class="list-row flex items-center">
			<div class="shrink-0">
				<CustomizableAvatar size={15} settings={profilePicture} />
			</div>
			<div class="min-w-0 flex-1 truncate text-2xl font-bold">
				{participant.name}
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
