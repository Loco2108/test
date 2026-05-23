<script lang="ts">
	import { themeManager } from '$lib/Theme.svelte';
	import { Sun, Moon, Menu } from '@lucide/svelte';
</script>

<div class="navbar mb-4 bg-base-100 shadow-sm">
	<div class="navbar-start">
		<div class="dropdown">
			<div tabindex="0" role="button" class="btn btn-circle btn-ghost">
				<Menu size={24} />
			</div>
			<ul
				tabindex="-1"
				class="dropdown-content menu z-1 mt-3 w-52 menu-sm rounded-box bg-base-100 p-2 shadow"
			>
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
			<label class="toggle text-base-content">
				<input
					type="checkbox"
					class="theme-controller"
					checked={themeManager.theme === 'dark'}
					onchange={() => themeManager.toggle()}
				/>

				<Sun size={16} />

				<Moon size={16} />
			</label>

			<div class="dropdown dropdown-end">
				<div tabindex="0" role="button" class="btn avatar btn-circle btn-ghost">
					<div class="w-10 rounded-full bg-black">
						<img
							alt="Profile Picture"
							src="https://imgs.search.brave.com/OsCbxE16JFtHL8gaUlTNAf2CEe5D4R7xYeMudwhHvZg/rs:fit:860:0:0:0/g:ce/aHR0cHM6Ly93d3cu/YW1vbmd1c2F2YXRh/cmNyZWF0b3IuY29t/L2Fzc2V0cy9pbWcv/Y2hhcmFjdGVycy82/MDAvcmVkLnBuZw"
						/>
					</div>
				</div>
				<ul
					tabindex="-1"
					class="dropdown-content menu z-1 mt-3 w-52 menu-sm rounded-box bg-base-100 p-2 shadow"
				>
					<li>
						<a class="justify-between"> Profile </a>
					</li>
					<li><a>Settings</a></li>
					<li><a>Logout</a></li>
				</ul>
			</div>
		</div>
	</div>
</div>
