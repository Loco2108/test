<script lang="ts">
	import { type IdentityError } from '$lib/api';
	import { apiClient } from '$lib/apiClient';
	import { authStore } from '$lib/authStore.svelte';
	import { addToast } from '$lib/components/Toast/Toast.svelte';
	import UserAvatar from '$lib/components/UserAvatar.svelte';
	import { Key, Pen } from '@lucide/svelte';
	import axios from 'axios';

	let usernameValue = $state(authStore.user?.username);
	let emailValue = $state(authStore.user?.email);
	let edited = $derived(
		usernameValue !== authStore.user?.username || emailValue !== authStore.user?.email
	);

	function updateInfo() {
		let usernameChanged = usernameValue !== authStore.user?.username;
		let emailChanged = emailValue !== authStore.user?.email;

		if (usernameValue && usernameChanged) {
			apiClient.api
				.v1UserUsernamePartialUpdate(usernameValue)
				.then((res) => {
					addToast({ label: 'Username updated', type: 'success' });

					if (authStore.user) {
						authStore.user.username = res.data;
					}

					usernameValue = res.data;
				})
				.catch((e) => {
					if (axios.isAxiosError<IdentityError[]>(e)) {
						if (e.response?.status == 409) {
							const errors = e.response.data;

							errors.forEach((el) =>
								addToast({ label: el.description ?? '', type: 'error' })
							);
						}
					}
				});
		}

		if (emailValue && emailChanged) {
			apiClient.api
				.v1UserEmailPartialUpdate(emailValue)
				.then((res) => {
					addToast({ label: 'E-Mail updated', type: 'success' });

					if (authStore.user) {
						authStore.user.email = res.data;
					}

					emailValue = res.data;
				})
				.catch((e) => {
					if (axios.isAxiosError<IdentityError[]>(e)) {
						if (e.response?.status == 409) {
							const errors = e.response.data;

							errors.forEach((el) =>
								addToast({ label: el.description ?? '', type: 'error' })
							);
						}
					}
				});
		}
	}
</script>

<div class="card w-full bg-base-100 shadow-sm card-md md:mx-auto md:w-fit">
	<div class="card-body">
		<h2 class="card-title text-2xl">Profile Settings</h2>

		<div class="divider my-0"></div>

		<div class="flex flex-col items-center gap-4 md:flex-row md:gap-16">
			<div>
				<div class="relative">
					<UserAvatar class="rounded-full bg-base-300 p-4" size={30} />
					<button
						class="btn absolute right-0 bottom-0 rounded-full"
						aria-label="Edit Profile Picture">
						<Pen />
					</button>
				</div>

				<button class="btn mt-8 btn-neutral">
					<Key />
					Update Password
				</button>
			</div>

			<form>
				<fieldset class="fieldset w-xs rounded-box border border-base-300 bg-base-200 p-4">
					<legend class="fieldset-legend">User Info</legend>

					<label class="label" for="usernameinput">Title</label>
					<input
						id="usernameinput"
						type="text"
						class="input"
						bind:value={usernameValue}
						oninput={() => (edited = true)} />

					<label class="label" for="emailinput">Email</label>
					<input
						id="emailinput"
						type="text"
						class="input"
						placeholder="my-awesome-page"
						bind:value={emailValue}
						oninput={() => (edited = true)} />

					<button
						class="btn mt-4 btn-neutral"
						type="submit"
						disabled={!edited}
						onclick={updateInfo}>
						Save Changes
					</button>
				</fieldset>
			</form>
		</div>
	</div>
</div>
