<script lang="ts">
	let { params, children } = $props();
</script>

Room {params.SessionId}

{@render children()}
