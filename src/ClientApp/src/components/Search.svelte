<script>
    export let inputText;
    export let value = '';
    export let styles;
</script>

<input type="text" style={styles} placeholder="{inputText}" name="{inputText}" bind:value={value} id='{inputText}' required />

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