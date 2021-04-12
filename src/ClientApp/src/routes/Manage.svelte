<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
	import { user } from '../storage';
	import { signOut, redirect } from '../helperFunctions';
	import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
</script>

<Page>
	<MainTitle>Manage</MainTitle>

	{#if $user.IsAuth}
		<EmptyCard styles="max-width: 450px;">
			<Title>{$user.Name}</Title>
			<Title>Your role: {$user.Role}</Title>
			<Button onClick={() => { signOut(); redirect("/"); } }>Sign out</Button>
    	</EmptyCard>
		{:else}
			<Title>Please, sign into your account.</Title>
	{/if}
</Page>