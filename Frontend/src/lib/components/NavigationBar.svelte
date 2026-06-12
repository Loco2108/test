<script lang="ts">
	import { LogOut, Menu, User } from '@lucide/svelte';
	import ThemeToggle from './ThemeToggle.svelte';
	import { authStore, logoutUser } from '$lib/authStore.svelte';
	import { apiClient } from '$lib/apiClient';
	import { goto } from '$app/navigation';

	async function logout() {
		await apiClient.api.v1UserLogoutCreate().then((result) => {
			if (result.status === 200) {
				logoutUser();
				goto('/');
			}
		});
	}
</script>

<div class="navbar mb-4 bg-base-100 shadow-sm">
	<div class="navbar-start">
		<div class="dropdown">
			<div tabindex="0" role="button" class="btn btn-circle btn-ghost">
				<Menu size={24} />
			</div>
			<ul
				tabindex="-1"
				class="dropdown-content menu z-1 mt-3 w-52 menu-sm rounded-box bg-base-100 p-2 shadow">
				<li><a>Homepage</a></li>
				<li><a>Portfolio</a></li>
				<li><a>About</a></li>
			</ul>
		</div>
	</div>
	<div class="navbar-center">
		<a class="btn text-xl btn-ghost">Stimmti</a>
	</div>
	<div class="navbar-end">
		<div class="flex items-center gap-2">
			<ThemeToggle />

			{authStore.user?.username}

			<div class="dropdown dropdown-end">
				<div tabindex="0" role="button" class="btn avatar btn-circle btn-ghost">
					<div class="w-10 rounded-full bg-base-300">
						{#if authStore.user?.profilePictureUrl}
							<img alt="Account" src={authStore.user.profilePictureUrl} />
						{:else}
							<User class="m-auto h-full" />
						{/if}
					</div>
				</div>
				<ul
					tabindex="-1"
					class="dropdown-content menu z-1 mt-3 w-52 menu-sm rounded-box bg-base-100 p-2 shadow">
					<li>
						<a class="justify-between"> Profile </a>
					</li>
					<li><a>Settings</a></li>
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
</div>
