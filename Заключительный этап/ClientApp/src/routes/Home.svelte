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
	import Search from "../components/Search.svelte";
	import { redirect } from "../helperFunctions";
	import { notyf } from "../notyf";

	let features = [
		{ title: 'Дизайн', description: 'Наш сервис реализует современный и легкий дизайн.', style: "max-width: 35vw;" },
		{ title: 'Технологии', description: 'Мы используем проверенные временем фреймворки, чтобы добиться идеального баланса производительности и комфорта.', style: "max-width: 45vw;" },
		{ title: 'Кроссплатформенность', description: 'Кроссплатформенность - это важная составляющая любого продукта. Вы можете использовать KnOwl в вашем браузере или в VK и Telegram.', style: "" }
	];

	let newArticles;

	// Search article.
	let articleTitle;

	onMount(() => loadNewArticles());

	function searchArticle(title)
	{
		httpClient.url("/api/Storage/articles/search")
            .auth(`Bearer ${$user.Jwt}`)
            .post({ article: { title: title, content: "", type: 0, tags: null, images: null, created: "" } })
            .json(json => {
                redirect("/articles/" + json.id);
            })
            .catch(error => {
				notyf.error("Статья не существует!");
        	});
	}

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
				<CardsHolder styles='align-items: center; position: relative; height: 100px; max-width: 450px; margin: 10px 5px;'>
					<Outline styles="position: absolute; top: 25px; left: 13.5px; width: 25px;" src="\resources\icons\Search.svg" />
					<Search bind:value={articleTitle} styles="position: relative; padding: 20px 47.5px; font-size: 18px;" inputText="Название статьи" />
					<Button onClick={() => searchArticle(articleTitle)}>Поиск</Button>
				</CardsHolder>
				<Button styles="margin-top: 55px;" href="/createarticle">Написать статью</Button>
				<Title><b>Новые</b> статьи:</Title>
				{#if newArticles != undefined}
					{#each newArticles as {id, title, content}}
						<Card styles='min-height: 290px; min-width: 250px; margin: 10px 5px;'>
							<Title>{title}</Title>
							<PreTitle>{@html content}</PreTitle>
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
			<Title>Приложение, где ты можешь <b>сохранять</b> и <b>делиться</b> своими заметками.</Title>
			<PreTitle>А также управлять своими данными с минималистичной панели.</PreTitle>
			<Button href="/signup">Начать</Button>
		</Page>

		<Page>
			<ExtendedTitle><Outline styles="margin-top: -0.58em; margin-left: -1.80em;" src="/resources/outlines/outline_1.svg" />Особенности</ExtendedTitle>
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