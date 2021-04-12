<script>
    export let styles;
</script>

<div style="{styles}">
    <slot></slot>
</div>

<style>
    div {
        width: 100%;
        display: flex;
	    flex-direction: row;
        justify-content: space-around;
        flex-wrap: wrap;
	    align-items: center;
    }
</style>