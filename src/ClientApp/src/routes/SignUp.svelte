<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import InputField from "../components/InputField.svelte";
    import Password from "../components/Password.svelte";
    import { notyf } from "../notyf";
    import { httpClient } from "../httpClient";
    import { saveUser, delayedRedirect, createFingerprint, generateString } from '../helperFunctions';
    import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
    import InlineLink from "../components/InlineLink.svelte";

    let userData = { name: "", password: "", fingerprint: "" };

    function authenticate()
    {
        userData.fingerprint = generateString(15);

        // Create new account.
        httpClient.url("/api/Authentication/")
            .post(userData)
            .res(response => {
                notyf.success("Your account has been successfully created!");
                createFingerprint(userData.fingerprint);

                // Fetch jwt and refresh token.
                httpClient.url("/api/Authentication/authenticate")
                    .post(userData)
                    .json(json => {
                        notyf.success("Welcome to KnOwl!");
                        saveUser(json.jwt, json.refreshToken);
                        delayedRedirect("/profile", 2000);
                    });
            })
            .catch(error => {
                notyf.error("User with this name already exists!")
            });
    }
</script>

<Page>
	<MainTitle>Sign up</MainTitle>
	<Title>Please <b>fill up</b> these fields to continue:</Title>
    <EmptyCard styles="min-width: 270px; max-width: 450px;">
        <InputField inputText="Name" bind:value={userData.name} />
        <Password inputText="Password" bind:value={userData.password} />    
        <Button onClick={() => authenticate()}>Continue</Button>
        <InlineLink styles="margin-top: 10px;" href="/signin">Already have an account?</InlineLink>
    </EmptyCard>
</Page>