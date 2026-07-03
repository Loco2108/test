<script lang="ts">
	import { goto } from '$app/navigation';
	import type { ProblemDetails } from '$lib/api';
	import { apiClient } from '$lib/apiClient';
	import { addToast } from '$lib/components/Toast/Toast.svelte';
	import { SessionConnection } from '$lib/signalr.svelte';
	import { ChevronLeft, GitMergeConflict, QrCode } from '@lucide/svelte';
	import axios from 'axios';
	import { onMount } from 'svelte';

	let hub = new SessionConnection();
	let roomCode = $state('');
	$effect(() => {
		roomCode = roomCode.toUpperCase();
	});

	onMount(() => {
		hub.init();
	});

	async function checkSession() {
		if (!roomCode.trim()) addToast({ label: 'No session code provided', type: 'error' });

		try {
			await apiClient.api.v1SessionCheckSessionList({ roomCode });

			goto(`/live/${roomCode.trim()}`);
		} catch (e) {
			if (axios.isAxiosError<ProblemDetails>(e)) {
				let data = e.response?.data;

				if (data?.title && data?.detail) {
					addToast({
						label: `${data.title}: ${data.detail}`,
						type: 'error',
						icon: GitMergeConflict,
					});
				}
			} else {
				addToast({ label: 'Unknown error occured', type: 'error' });
			}
		}
	}
</script>

<button
	class="btn mt-4 ml-4 btn-ghost md:absolute"
	aria-label="Navigate Back"
	onclick={() => history.back()}>
	<ChevronLeft />
</button>

<div class="flex h-screen w-full items-center justify-center">
	<div class="flex w-fit min-w-96 flex-col items-center justify-center gap-4">
		<div class="card w-full bg-base-100 shadow-sm card-md">
			<div class="card-body">
				<h2 class="mx-auto card-title">Join Session</h2>

				<p class="text-center text-balance text-secondary">
					Enter your room code or scan the QR code to participate
				</p>

				<div class="mt-4 card-actions w-full justify-end">
					<form class="w-full" onsubmit={checkSession}>
						<fieldset class="fieldset">
							<legend class="fieldset-legend">Room Code</legend>

							<label class="otp otp-lg">
								<span></span>
								<span></span>
								<span></span>
								<span></span>
								<span></span>
								<span></span>
								<span></span>
								<input type="text" maxlength="7" required bind:value={roomCode} />
							</label>
						</fieldset>

						<button class="btn mt-4 btn-block btn-primary" type="submit">Join</button>
					</form>

					<div class="divider w-full">OR</div>

					<button class="btn btn-block btn-outline">
						<QrCode />
						Scan QR-Code
					</button>
				</div>
			</div>
		</div>
	</div>
</div>
