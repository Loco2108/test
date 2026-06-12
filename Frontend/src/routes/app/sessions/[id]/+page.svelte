<script lang="ts">
	let { params } = $props();

	let sessionId = $derived(params.id);
</script>

This shows data about {sessionId}
