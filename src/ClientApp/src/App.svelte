<script>
	import { Route } from 'tinro';
	import Home from './routes/Home.svelte';
	import Error from './routes/Error.svelte';
	import Profile from './routes/Profile.svelte';
	import Navbar from './components/Navbar.svelte';
	import About from './routes/About.svelte';
	import SignUp from './routes/SignUp.svelte';
	import SignIn from './routes/SignIn.svelte';
	import { beforeUpdate } from 'svelte';
	import { refreshTokenPair } from './helperFunctions';
	import CreateArticle from './routes/CreateArticle.svelte';
	import Page from "./components/Page.svelte";
	import PreTitle from "./components/PreTitle.svelte";
	import Articles from './routes/Articles.svelte';
	import CardsHolder from './components/CardsHolder.svelte';
	import Outline from './components/Outline.svelte';

	// Try to fetch refresh token from cookies.
	beforeUpdate(() => {
		refreshTokenPair();
	});

	let links = [
		{ text: 'Домой', link: '/' },
		{ text: 'Профиль', link: '/profile' },
		{ text: 'О нас', link: '/about' }
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

	<Route path="/createarticle">
		<CreateArticle />
	</Route>

	<Route path="/articles/:articleId" let:meta>
		<Articles articleId={meta.params.articleId} />
	</Route>

	<Route path="/articles">
		<Articles />
	</Route>

	<Route fallback>
		<Error />
	</Route>
</Route>

<Page styles="height: 50px; min-height: 50px; background-color: black;">
	<CardsHolder styles="min-height: 100px;">
		<a href="https://github.com/fpetrov/KnOwl"><Outline styles="position: relative; width: 40px;" src="\resources\icons\GitHub.svg" /></a>
		<a href="https://telegram.org/"><Outline styles="position: relative; width: 40px;" src="\resources\icons\Telegram.svg" /></a>
		<a href="https://vk.com/"><Outline styles="position: relative; width: 40px;" src="\resources\icons\Vk.svg" /></a>
	</CardsHolder>
	<PreTitle>&copy; {new Date().getFullYear()} - Fedor Petrov.</PreTitle>
</Page>