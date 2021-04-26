<script>
    export let links;
</script>

<nav>
    {#each links as {text, link}}
	    <a href={link}>{text}</a>
    {/each}
</nav>

<style>
nav {
  width: 100%;
  height: 80px;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  position: fixed;
  top: 0;
  background-color: var(--color-navbar);
  backdrop-filter: saturate(180%) blur(20px);
  z-index: 1;
}

nav a {
  font-weight: 500;
  text-transform: uppercase;
  text-decoration: none;
  font-weight: 600;
  margin: 0 15px;
  font-size: 17px;
  letter-spacing: 1px;
  position: relative;
  display: inline-block;
  cursor: pointer;
}

nav a:before {
  content: '';
  position: absolute;
  width: 100%;
  height: 2px;
  background: var(--color-text-main);
  bottom: 0;
  animation: out 0.2s cubic-bezier(1, 0, 0.58, 0.97) 1 both;
}

nav a:hover:before {
  animation: in 0.2s cubic-bezier(1, 0, 0.58, 0.97) 1 both;
}

@keyframes in {
  0% {
    width: 0;
    left: 0;
    right: auto;
  }
  100% {
    left: 0;
    right: auto;
    width: 100%;
  }
}
@keyframes out {
  0% {
    width: 100%;
    left: auto;
    right: 0;
  }
  100% {
    width: 0;
    left: auto;
    right: 0;
  }
}
@keyframes show {
  0% {
    opacity: 0;
    transform: translateY(-10px);
  }
  100% {
    opacity: 1;
    transform: translateY(0);
  }
}
nav a:nth-child(1) {
  animation: show 0.2s 1.1s ease 1 both;
}

nav a:nth-child(2) {
  animation: show 0.2s 1.2s ease 1 both;
}

nav a:nth-child(3) {
  animation: show 0.2s 1.3s ease 1 both;
}

nav a:nth-child(4) {
  animation: show 0.2s 1.3s ease 1 both;
}
</style>