<script>
	import { Route } from 'tinro';
	import Home from './routes/Home.svelte';
	import Error from './routes/Error.svelte';
	import Profile from './routes/Profile.svelte';
	import Navbar from './components/Navbar.svelte';
	import About from './routes/About.svelte';
	import SignUp from './routes/SignUp.svelte';
	import SignIn from './routes/SignIn.svelte';
	import { onMount } from 'svelte';
	import { refreshTokenPair } from './helperFunctions';

	// Try to fetch refresh token from cookies.
	onMount(() => {
		refreshTokenPair();
	});

	let links = [
		{ text: 'Home', link: '/' },
		{ text: 'Profile', link: '/profile' },
		{ text: 'About', link: '/about' }
	];
</script>

<Navbar links={links} />

<Route>
	<Route path="/">
		<Home />
	</Route>

	<Route path="/profile">
		<Profile />
	</Route>

	<Route path="/about">
		<About />
	</Route>

	<Route path="/signup">
		<SignUp />
	</Route>

	<Route path="/signin">
		<SignIn />
	</Route>

	<Route fallback>
		<Error />
	</Route>
</Route>