<script>
    export let title;
    export let items;
    export let value;
    export let id = "";
</script>

<div class="selection">
    <select id={id} bind:value={value}>
        <option value="" disabled="disabled" selected="selected">{title}</option>
        {#each items as item, i}
            <option value={i + 1}>{item}</option>
        {/each}
    </select>
</div>

<style>
    select {
        -webkit-appearance: none;
        border: none;
        outline: none;
        padding: 7px 40px 7px 12px;
        width: 100%;
        border: 1px solid #E8EAED;
        border-radius: 5px;
        background: white;
        cursor: pointer;
        font-family: inherit;
        font-size: 16px;
        transition: all 150ms ease;
    }

    select option {
        color: #223254;
    }

    select option[value=""][disabled] {
        display: none;
    }
</style>