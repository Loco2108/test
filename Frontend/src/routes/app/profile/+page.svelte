<script lang="ts">
	import {
		type IdentityError,
		type ProblemDetails,
		type ValidationProblemDetails,
	} from '$lib/api';
	import { apiClient } from '$lib/apiClient';
	import { authStore } from '$lib/authStore.svelte';
	import { addToast } from '$lib/components/Toast/Toast.svelte';
	import UserAvatar from '$lib/components/UserAvatar.svelte';
	import { Eye, Key, LoaderCircle, Pen, X } from '@lucide/svelte';
	import axios from 'axios';

	let usernameValue = $state(authStore.user?.username);
	let emailValue = $state(authStore.user?.email);
	let edited = $derived(
		usernameValue !== authStore.user?.username || emailValue !== authStore.user?.email
	);

	let profilePicModal: HTMLDialogElement | undefined = $state();
	let files: FileList | undefined = $state();
	let uploadingImage = $state(false);

	let changePasswordModal: HTMLDialogElement | undefined = $state();
	let changePasswordForm: HTMLFormElement | undefined = $state();
	let passwordVisible = $state(false);
	let oldPassword = $state('');
	let newPassword = $state('');
	let retypeNewPassword = $state('');
	let passwordsMatch = $derived(
		retypeNewPassword.trim() !== '' && newPassword === retypeNewPassword
	);
	let isSubmitting = $state(false);

	function updateInfo() {
		let usernameChanged = usernameValue !== authStore.user?.username;
		let emailChanged = emailValue !== authStore.user?.email;

		if (usernameValue && usernameChanged) {
			apiClient.api
				.v1UserUsernamePartialUpdate({ username: usernameValue })
				.then((res) => {
					addToast({ label: 'Username updated', type: 'success' });

					if (authStore.user) {
						authStore.user.username = res.data;
					}

					usernameValue = res.data;
				})
				.catch((e) => {
					if (axios.isAxiosError(e)) {
						if (e.response?.status == 409) {
							const errors = e.response.data as IdentityError[];

							errors.forEach((el) =>
								addToast({ label: el.description ?? '', type: 'error' })
							);
						}

						if (e.response?.status == 400) {
							const errors = e.response.data as ValidationProblemDetails;

							Object.keys(errors.errors ?? {}).forEach((el) => {
								addToast({
									label: errors.errors?.[el][0] ?? 'Unknown error occured',
									type: 'error',
								});
							});
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

	function uploadProfilePicture() {
		if (!files || !files.length) return;

		uploadingImage = true;

		apiClient.api
			.v1UserProfilePictureCreate({ file: files[0] })
			.then((res) => {
				addToast({ label: 'Profile picture updated successfully', type: 'success' });

				if (authStore.user) {
					authStore.user.profilePictureUrl = res.data;
				}
			})
			.catch((e) => {
				if (axios.isAxiosError(e)) {
					if (e.response?.status === 400 || e.response?.status === 500) {
						const error = e.response.data as ProblemDetails;

						addToast({
							label: error.detail ?? 'An unknown error occurred',
							type: 'error',
						});
						return;
					}

					addToast({ label: 'An unknown error occurred', type: 'error' });
				}
			})
			.finally(() => (uploadingImage = false));
	}

	function handlePasswordSubmit(event: SubmitEvent) {
		event.preventDefault();

		isSubmitting = true;

		apiClient.api
			.v1UserPasswordPartialUpdate({ oldPassword, newPassword })
			.then(() => {
				changePasswordModal?.close();
				changePasswordForm?.reset();

				addToast({ type: 'success', label: 'Password updated successfully', icon: Key });
			})
			.catch((e) => {
				if (axios.isAxiosError(e)) {
					if (e.response?.status === 400) {
						const error = e.response.data as ValidationProblemDetails;

						let errorMessage = 'An unknown error occurred';

						if (error?.errors && Object.keys(error.errors).length > 0) {
							const firstKey = Object.keys(error.errors)[0];
							errorMessage = error.errors[firstKey][0];
						}

						addToast({
							type: 'error',
							label: errorMessage,
						});
					}
				}
			})
			.finally(() => (isSubmitting = false));
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
						aria-label="Edit Profile Picture"
						onclick={() => profilePicModal?.showModal()}>
						<Pen />
					</button>
				</div>

				<button
					class="btn mt-8 btn-outline"
					onclick={() => changePasswordModal?.showModal()}>
					<Key />
					Update Password
				</button>
			</div>

			<form>
				<fieldset class="fieldset w-xs rounded-box border border-base-300 bg-base-200 p-4">
					<legend class="fieldset-legend">User Info</legend>

					<label class="label" for="usernameinput">Username</label>
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

<dialog class="modal" bind:this={profilePicModal}>
	<div class="modal-box">
		<form method="dialog">
			<button class="btn absolute top-2 right-2 btn-ghost btn-sm"><X /></button>
		</form>
		<h3 class="text-lg font-bold">Profile Picture</h3>

		<div class="flex flex-col items-center gap-4 p-4 md:flex-row">
			<UserAvatar class="rounded-full bg-base-300 p-4" size={30} />

			<div>
				<fieldset class="fieldset">
					<legend class="fieldset-legend">Upload image</legend>
					<input
						type="file"
						class="file-input"
						accept="image/jpg, image/jpeg, image/png, image/webp"
						bind:files />
				</fieldset>
				<button
					class="btn mt-2 btn-block btn-primary md:btn-sm"
					disabled={!files || !files.length || uploadingImage}
					onclick={uploadProfilePicture}>
					{#if uploadingImage}
						<LoaderCircle class="animate-spin" />
					{/if}
					Upload
				</button>
			</div>
		</div>
	</div>
</dialog>

<dialog class="modal" bind:this={changePasswordModal}>
	<div class="modal-box">
		<form method="dialog">
			<button class="btn absolute top-2 right-2 btn-ghost btn-sm"><X /></button>
		</form>
		<h3 class="text-lg font-bold">Change Password</h3>

		<form class="p-4" onsubmit={handlePasswordSubmit} bind:this={changePasswordForm}>
			<div class="w-full">
				<label class="input join-item mb-1 w-full">
					<Key class="opacity-50" />
					<input
						type="password"
						placeholder="Old Password"
						bind:value={oldPassword}
						required />
				</label>
			</div>

			<div class="join w-full">
				<div class="w-full">
					<label class="validator input join-item mb-1 w-full">
						<Key class="opacity-50" />
						<input
							type={passwordVisible ? 'text' : 'password'}
							placeholder="New Password"
							minlength="8"
							pattern={'^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$'}
							bind:value={newPassword}
							title="Must be more than 8 characters, including number, lowercase letter, uppercase letter"
							required />
					</label>
					<p class="validator-hint mb-2 hidden">
						Must be more than 8 characters, including
						<br />At least one number <br />At least one lowercase letter <br />At least
						one uppercase letter
					</p>
				</div>

				<button
					type="button"
					class="btn join-item btn-neutral"
					aria-label="Show Password"
					onclick={() => (passwordVisible = !passwordVisible)}>
					<Eye />
				</button>
			</div>

			<label class={['input w-full', passwordsMatch && 'input-success']}>
				<Key class="opacity-50" />
				<input
					type="password"
					class="w-full"
					placeholder="Retype New Password"
					bind:value={retypeNewPassword}
					required />
			</label>

			<button
				class="btn mt-4 btn-block btn-primary"
				type="submit"
				disabled={isSubmitting || !passwordsMatch}>
				{#if isSubmitting}
					<LoaderCircle class="animate-spin" />
				{/if}
				Change Password
			</button>
		</form>
	</div>
</dialog>
