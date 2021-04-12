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
        width: 100% !important;
        border-radius: 15px;
        cursor: pointer;
        /* color: white; */
        padding: 0 15px;
        background-color: var(--color-panel-bg);
        /* box-shadow: 0px 0px 15px 0px rgba(0, 0, 0, 0.06); */
        transition: all 0.5s ease;
    }
</style>