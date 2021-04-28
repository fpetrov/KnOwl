<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import InputField from "../components/InputField.svelte";
    import Password from "../components/Password.svelte";
    import { notyf } from "../notyf";
    import { httpClient } from "../httpClient";
    import { saveUser, delayedRedirect } from '../helperFunctions';
    import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
    import InlineLink from "../components/InlineLink.svelte";

    let userData = { name: "", password: "", fingerprint: "" };

    function authenticate()
    {
        userData.fingerprint = navigator.userAgent;

        // Create new account.
        httpClient.url("/api/Authentication/")
            .post(userData)
            .res(response => {
                notyf.success("Ваш аккаунт успешно создан!");

                // Fetch jwt and refresh token.
                httpClient.url("/api/Authentication/authenticate")
                    .post(userData)
                    .json(json => {
                        notyf.success("Добро пожаловать в KnOwl!");
                        saveUser(json.jwt, json.refreshToken);
                        delayedRedirect("/profile", 2000);
                    });
            })
            .catch(error => {
                notyf.error("Пользователь с таким именем уже существует!")
            });
    }
</script>

<Page>
	<MainTitle>Создать аккаунт</MainTitle>
	<Title>Пожалуйста, <b>заполните</b> эти поля, чтобы продолжить:</Title>
    <EmptyCard styles="min-width: 270px; max-width: 450px;">
        <InputField inputText="Имя" bind:value={userData.name} />
        <Password inputText="Пароль" bind:value={userData.password} />    
        <Button onClick={() => authenticate()}>Продолжить</Button>
        <InlineLink styles="margin-top: 10px;" href="/signin">Уже есть аккаунт?</InlineLink>
    </EmptyCard>
</Page>