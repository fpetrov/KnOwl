<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import InputField from "../components/InputField.svelte";
    import Password from "../components/Password.svelte";
    import { notyf } from "../notyf";
    import { httpClient } from "../httpClient";
    import { user } from '../storage';
    import { saveUser, delayedRedirect } from '../helperFunctions';
    import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
    import InlineLink from "../components/InlineLink.svelte";

    let userData = { name: "", password: "", fingerprint: "" };

    function authenticate()
    {
        userData.fingerprint = navigator.userAgent;

        httpClient.url("/api/Authentication/authenticate")
            .post(userData)
            .json(json => {
                saveUser(json.jwt, json.refreshToken);
                notyf.success("Добро пожаловать, " + $user.Name);
                delayedRedirect("/profile", 2000);
            })
            .catch(error => {
                notyf.error("Введенные данные неверны!");
            });
    }
</script>

<Page>
	<MainTitle>Войти</MainTitle>
	<Title>Пожалуйста, <b>заполните</b> эти поля, чтобы продолжить:</Title>
    <EmptyCard styles="min-width: 270px; max-width: 450px;">
        <InputField inputText="Имя" bind:value={userData.name} />
        <Password inputText="Пароль" bind:value={userData.password} />    
        <Button onClick={() => authenticate()}>Продолжить</Button>
        <InlineLink styles="margin-top: 10px;" href="/signup">Нет аккаунта?</InlineLink>
    </EmptyCard>
</Page>