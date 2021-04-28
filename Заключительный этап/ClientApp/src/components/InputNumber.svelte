<script>
    export let inputText;
    export let value = '';
    export let min = 0;
    export let max = 1;
</script>

<input type="number" placeholder="{inputText}" min={min} max={max} name="{inputText}" bind:value={value} id='{inputText}' required />

<style>
    input {
        width: 100%;
        padding: 15px 20px;
        margin: 8px 0;
        border: none;
        outline: none;
        user-select: none;
        background: var(--color-input-field);
        border-radius: var(--border-radius);
        color: var(--color-text-main);
        display: inline-block;
        box-sizing: border-box;
    }
</style>