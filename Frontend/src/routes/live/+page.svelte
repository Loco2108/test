<script>
	import { ChevronLeft, QrCode } from '@lucide/svelte';
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
					<form class="w-full">
						<fieldset class="fieldset">
							<legend class="fieldset-legend">Room Code</legend>
							<input type="text" class="input w-full" placeholder="A1B2C3" required />
						</fieldset>

						<button class="btn btn-block btn-primary">Join</button>
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
