<script>
    export let onClick;
    export let href;
</script>

<a href={href} on:click={onClick}>
    <slot></slot>
</a>

<style>
    a {
        background-color: #000;
        color: #fff;
        border: 2px solid #000;
        border-radius: var(--border-radius);
        outline: none;
        text-decoration: none;
        cursor: pointer;
        user-select: none;
        text-align: center;
        padding: 15px 5px;
        min-width: 170px;
        margin-top: 20px;
        font-size: 16px;
        transition: all var(--duration) ease-in-out;
    }

    a:hover {
        background-color: transparent;
        border-color: #000;
        color: #000;
        outline: none;
    }
</style>