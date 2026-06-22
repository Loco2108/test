<script>
	import { apiClient } from '$lib/apiClient';
	import { authStore, logoutUser } from '$lib/authStore.svelte';
	import UserAvatar from '$lib/components/UserAvatar.svelte';
	import { BadgePlus, ChevronLeft, LayoutDashboard, LogOut, QrCode } from '@lucide/svelte';

	async function logout() {
		await apiClient.api.v1UserLogoutCreate().then((result) => {
			if (result.status === 200) {
				logoutUser();
			}
		});
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

		{#if authStore.isLoggedIn}
			<div class="divider m-0"></div>

			<div class="card w-full bg-base-100 shadow-sm card-md">
				<div class="card-body flex flex-row items-center justify-center gap-4">
					Logged in as

					<div class="dropdown dropdown-end">
						<button class="btn w-fit bg-base-300 py-6 btn-ghost">
							<UserAvatar />

							{authStore.user?.username}
						</button>

						<ul
							tabindex="-1"
							class="dropdown-content menu z-10 mt-3 w-52 menu-md rounded-box bg-base-100 p-2 shadow">
							<li>
								<a href="/app">
									<LayoutDashboard size={16} />
									Dashboard
								</a>
							</li>
							<li>
								<button onclick={logout}>
									<LogOut size={16} />
									Logout
								</button>
							</li>
						</ul>
					</div>
				</div>
			</div>

			<div class="card w-full bg-base-100 shadow-sm card-md">
				<div class="card-body">
					<h2 class="mx-auto card-title">Create Session</h2>

					<div class="mt-4 card-actions w-full justify-end">
						<form class="w-full" onsubmit={() => console.log('ASlkjdhajdhaldahdls')}>
							<select class="select w-full">
								<option value="">-</option>
								<option value="xy">C# Feedback Round</option>
								<option value="yz">React vs. Svelte</option>
								<option value="za">Node.js task complexity</option>
							</select>

							<button class="btn mt-2 btn-block btn-secondary" type="submit">
								<BadgePlus />
								Create
							</button>
						</form>
					</div>
				</div>
			</div>
		{/if}
	</div>
</div>
