<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import InputField from "../components/InputField.svelte";
    import Password from "../components/Password.svelte";
    import { notyf } from "../notyf";
    import { httpClient } from "../httpClient";
    import { user } from '../storage';
    import { saveUser, delayedRedirect, createFingerprint, generateString } from '../helperFunctions';
    import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
    import InlineLink from "../components/InlineLink.svelte";

    let userData = { name: "", password: "", fingerprint: "" };

    function authenticate()
    {
        userData.fingerprint = generateString(15);

        httpClient.url("/api/Authentication/authenticate")
            .post(userData)
            .json(json => {
                createFingerprint(userData.fingerprint);
                saveUser(json.jwt, json.refreshToken);
                notyf.success("Welcome back, " + $user.Name);
                delayedRedirect("/profile", 2000);
            })
            .catch(error => {
                notyf.error("User fields are incorrect!");
            });
    }
</script>

<Page>
	<MainTitle>Sign in</MainTitle>
	<Title>Please <b>fill up</b> these fields to continue:</Title>
    <EmptyCard styles="min-width: 270px; max-width: 450px;">
        <InputField inputText="Name" bind:value={userData.name} />
        <Password inputText="Password" bind:value={userData.password} />    
        <Button onClick={() => authenticate()}>Continue</Button>
        <InlineLink styles="margin-top: 10px;" href="/signup">Don't have an account? </InlineLink>
    </EmptyCard>
</Page>