<script lang="ts">
	import {
		ChartPie,
		CircleQuestionMark,
		Cog,
		FileBadge,
		House,
		LibraryBig,
		LogOut,
		User,
	} from '@lucide/svelte';
	import ThemeToggle from './ThemeToggle.svelte';
	import { authStore, logoutUser } from '$lib/authStore.svelte';
	import { apiClient } from '$lib/apiClient';
	import { goto } from '$app/navigation';
	import { page } from '$app/state';

	let menuTabs = [
		{ label: 'Home', icon: House, href: '/app' },
		{ label: 'Library', icon: LibraryBig, href: '/app/library' },
		{ label: 'Overview', icon: ChartPie, href: '/app/overview' },
	];

	async function logout() {
		await apiClient.api.v1UserLogoutCreate().then((result) => {
			if (result.status === 200) {
				logoutUser();
				goto('/');
			}
		});
	}
</script>

{#snippet profileDropdown()}
	<div class="dropdown dropdown-end">
		<button class="btn bg-base-300 py-6 btn-ghost">
			<div class="avatar">
				<div class="w-10 rounded-full bg-base-100">
					{#if authStore.user?.profilePictureUrl}
						<img alt="Account" src={authStore.user.profilePictureUrl} />
					{:else}
						<User class="m-auto h-full" />
					{/if}
				</div>
			</div>

			<span class="hidden md:inline">
				{authStore.user?.username}
			</span>
		</button>

		<ul
			tabindex="-1"
			class="dropdown-content menu z-1 mt-3 w-52 menu-md rounded-box bg-base-100 p-2 shadow">
			<li>
				<a href="/app/settings">
					<User size={16} />
					Profile
				</a>
			</li>
			<li>
				<a href="/app/settings">
					<Cog size={16} />
					Settings</a>
			</li>
			<li>
				<details>
					<summary><FileBadge size={16} /> Legal</summary>
					<ul>
						<li><a>Privacy Policy</a></li>
						<li><a>Legal Notice</a></li>
					</ul>
				</details>
			</li>
			<li>
				<button onclick={logout}>
					<LogOut size={16} />
					Logout
				</button>
			</li>
		</ul>
	</div>
{/snippet}

<div class="navbar bg-base-100 shadow-sm">
	<div class="mx-auto navbar-start flex-col md:flex-row">
		<a href="/app" class="btn text-2xl font-extrabold btn-ghost">Stimmti</a>

		<div class="flex gap-2">
			<div role="tablist" class="tabs-box tabs flex-nowrap">
				{#each menuTabs as tab}
					{@const Icon = tab.icon}
					{@const isActive = page.url.pathname.endsWith(tab.href)}

					<a
						role="tab"
						href={tab.href}
						class={[
							'tab flex flex-nowrap items-center gap-1 transition-all',
							isActive && 'tab-active',
						]}>
						<Icon size={16} />
						{tab.label}
					</a>
				{/each}
			</div>
			<div class="block md:hidden">
				{@render profileDropdown()}
			</div>
		</div>
	</div>

	<div class="navbar-end hidden md:flex">
		<div class="flex items-center gap-4">
			<ThemeToggle />

			<div class="divider mx-0! divider-horizontal"></div>

			<a
				href="/app/help"
				class="btn btn-circle btn-ghost"
				aria-label="Help"
				title="Open Help Page">
				<CircleQuestionMark />
			</a>

			{@render profileDropdown()}
		</div>
	</div>
</div>
