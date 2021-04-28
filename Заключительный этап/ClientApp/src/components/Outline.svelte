<script>
    export let src = "";
    export let styles = "";
    export let loading = "lazy";
</script>

<img src={src} loading={loading} style={styles} alt={src}>

<style>
    img {
		position: absolute;
    	left: 0%;
    	top: 0%;
    	right: auto;
    	bottom: auto;
        pointer-events: none;
	}
</style>