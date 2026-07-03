<script>
	import { goto } from '$app/navigation';
	import { addToast } from '$lib/components/Toast/Toast.svelte';
	import { SessionConnection } from '$lib/signalr.svelte';
	import { ChevronLeft, QrCode } from '@lucide/svelte';
	import { onMount } from 'svelte';

	let hub = new SessionConnection();
	let roomCode = $state('');

	onMount(() => {
		hub.init();
	});

	async function joinSession() {
		if (!roomCode.trim()) addToast({ label: 'No session code provided', type: 'error' });

		let res = await hub.joinSession(roomCode);

		if (res) {
			goto(`live/${roomCode}`);
		} else {
			addToast({ label: 'No active session with the given code was found', type: 'error' });
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
					<form class="w-full" onsubmit={joinSession}>
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
