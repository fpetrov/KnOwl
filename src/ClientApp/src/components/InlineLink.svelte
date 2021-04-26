<script>
    export let href;
    export let styles;
</script>

<a style={styles} href={href}>
    <slot></slot>
</a>

<style>
a {
  position: relative;
  text-decoration: none;
  display: inline-block;
  padding: 0 1px;
  transition: color ease 0.3s;
  
}

a::before, a::after {
    content: '';
    position: absolute;
    background-color: var(--color-action);
    z-index: -1;
    height: 7%;
  }

a::before {
    width: 0%;
    left: 0;
    bottom: 0;
    transition: width ease 0.4s;
  }

a::after {
    width: 100%;
    left: 0;
    bottom: 0;
    transition: all ease 0.6s;
  }

a:hover::before {
      width: 100%;
    }

a:hover::after {
      left: 100%;
      width: 0%;
      transition: all ease 0.2s;
    }
</style>