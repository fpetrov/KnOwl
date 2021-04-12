<script>
    export let href;
    export let styles;
</script>

<a style={styles} href={href}>
    <slot></slot>
</a>

<style>
    a {
        text-decoration: none;
        border-bottom: 1px solid var(--color-text-main);
        padding-bottom: 10px;
    }
</style>