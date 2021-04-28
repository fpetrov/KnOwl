<script>
    export let styles;
</script>

<div style="{styles}">
    <slot></slot>
</div>

<style>
    div {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 0 15px;
    }
</style>