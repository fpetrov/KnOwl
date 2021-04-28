<script>
    export let name = '';
</script>

<div class="inputHolder">
    <input name={name} type="file" id={name} multiple>
    <label for={name}>
        <div class="inputButton">
          <slot></slot>
        </div>
    </label>  
</div>

<style>
.inputHolder {
  width: 100%;
  position: relative;
  margin: 15px 0;
  text-align: center;
}
 
input {
  opacity: 0;
  visibility: hidden;
  position: absolute;
}
 
label {
  width: 100%;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: justify;
      -ms-flex-pack: justify;
          justify-content: space-between;
  -webkit-box-align: center;
      -ms-flex-align: center;
          align-items: center;
  -ms-flex-wrap: wrap;
      flex-wrap: wrap;
}

.inputButton {
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

.inputButton:hover {
    background-color: transparent;
    border-color: #000;
    color: #000;
    outline: none;
}
</style>