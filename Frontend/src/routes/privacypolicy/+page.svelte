<script lang="ts">
	import { ChevronLeft } from '@lucide/svelte';
</script>

<button class="btn mt-4 mb-4 ml-4 btn-lg" aria-label="Navigate Back" onclick={() => history.back()}>
	<ChevronLeft />
</button>

<div class="card mx-auto flex w-full flex-col gap-4 bg-base-100 p-4 shadow md:w-200">
	<h2 class="text-xl font-bold">Privacy Policy</h2>
</div>
