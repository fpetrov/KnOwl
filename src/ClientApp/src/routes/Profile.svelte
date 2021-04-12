<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
	import { user, lowLevelApi, highLevelApi } from '../storage';
	import { signOut, redirect } from '../helperFunctions';
	import Button from "../components/Button.svelte";
	import CardsHolder from "../components/CardsHolder.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
	import InputField from "../components/InputField.svelte";
	import InputFile from "../components/InputFile.svelte";
</script>

<Page>
	<MainTitle>Profile</MainTitle>

	{#if $user.IsAuth}
		<EmptyCard styles="max-width: 450px;">
			<Title>{$user.Name}</Title>
			<Title>Your role: {$user.Role}</Title>
			<Button onClick={() => { signOut(); redirect("/"); } }>Sign out</Button>

			<form action="api/storage/image" method="post" enctype="multipart/form-data">
				<InputFile name="files">
					Select files
				</InputFile>
				<label>
					<input type="submit" name="button" id="button" value="Submit" />
				</label>
			</form>

			{#if $user.Role == "Admin"}
				<MainTitle>Admin Panel</MainTitle>
				<CardsHolder styles="justify-content: center;">
					<EmptyCard>
						<Title>Change <b>address</b> of low level service</Title>
						<InputField inputText="Address of low level API" bind:value={$lowLevelApi} />
					</EmptyCard>
					<EmptyCard>
						<Title>Change <b>address</b> of high level service</Title>
						<InputField inputText="Address of high level API" bind:value={$highLevelApi} />
					</EmptyCard>
				</CardsHolder>
			{/if}
    	</EmptyCard>
		{:else}
			<Title>Please, sign into your account.</Title>
	{/if}
</Page>