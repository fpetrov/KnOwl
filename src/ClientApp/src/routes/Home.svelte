<script>
	import Card from "../components/Card.svelte";
	import CardsHolder from "../components/CardsHolder.svelte";
	import Page from "../components/Page.svelte";
	import PreTitle from "../components/PreTitle.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
	import Button from "../components/Button.svelte";
	import { user } from '../storage';
	import { httpClient } from "../httpClient";
	import EmptyCard from "../components/EmptyCard.svelte";
	import Outline from "../components/Outline.svelte";
	import ExtendedTitle from "../components/ExtendedTitle.svelte";
	import { onMount } from "svelte";

	let features = [
		{ title: 'Дизайн', description: 'Наш сервис реализует современный и легкий дизайн.', style: "max-width: 35vw;" },
		{ title: 'Технологии', description: 'Мы используем проверенные временем фреймворки, чтобы добиться идеального баланса производительности и комфорта.', style: "max-width: 45vw;" },
		{ title: 'Кроссплатформенность', description: 'Кроссплатформенность - это важная составляющая любого продукта. Вы можете использовать KnOwl в вашем браузере или в VK и Telegram.', style: "" }
	];

	let newArticles;

	onMount(() => loadNewArticles());

	function loadNewArticles()
	{
		httpClient.url("/api/Storage/articles/take/5")
            .auth(`Bearer ${$user.Jwt}`)
            .get()
            .json(json => {
                newArticles = json;
            })
            .catch(error => {

        	});
	}
</script>

{#if $user.IsAuth}
	<Page>
		<CardsHolder>
			<EmptyCard>
				<MainTitle>Привет, <b>{$user.Name}</b>!</MainTitle>
				<Button href="/createarticle">Написать статью</Button>
				<Title><b>Новые</b> статьи:</Title>
				{#if newArticles != undefined}
					{#each newArticles as {id, title, content}}
						<Card styles='min-height: 290px; min-width: 250px; margin: 10px 5px;'>
							<Title>{title}</Title>
							<PreTitle>{content}</PreTitle>
							<Button styles="margin-bottom: 20px;" href="/articles/{id}">Читать</Button>
						</Card>
					{/each}
					{:else}
					<Title>Новых публикаций нет</Title>
				{/if}
			</EmptyCard>
		</CardsHolder>
	</Page>
	{:else}
		<Page>
			<MainTitle>Привет, это <b>KnOwl</b></MainTitle>
			<Title>Приложение, где ты можешь <b>сохранять</b> свои заметки.</Title>
			<PreTitle>А также управлять своими данными с простой и красивой панели.</PreTitle>
			<Button href="/signup">Начать</Button>
		</Page>

		<Page>
			<ExtendedTitle><Outline styles="margin-top: -0.58em; margin-left: -3em;" src="/resources/outlines/outline_1.svg" />Features</ExtendedTitle>
			<CardsHolder>
				{#each features as {title, description, style}}
					<Card styles='{style} min-height: 290px; min-width: 250px; margin: 10px 5px;'>
						<Title>{title}</Title>
						<PreTitle>{description}</PreTitle>
					</Card>
				{/each}
			</CardsHolder>
		</Page>
{/if}