<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import Card from "../components/Card.svelte";
	import { httpClient } from "../httpClient";
    import PreTitle from "../components/PreTitle.svelte";
    import { onMount } from "svelte";
    import { redirect } from "../helperFunctions";
    import { user } from '../storage';
    import InlineLink from "../components/InlineLink.svelte";
    import Button from "../components/Button.svelte";
    import { notyf } from "../notyf";
import CardsHolder from "../components/CardsHolder.svelte";
import EmptyCard from "../components/EmptyCard.svelte";

    export let articleId;

	let articleData = { Title: "", Content: "", Author: "", TimeToRead: { Minutes: 1, Seconds: 1 }, Created: "" };
    let additionalResources = { Mesh: "", YouTube: "" };

    let userArticles;

    onMount(() => {
        if (articleId != undefined)
        {
        httpClient.url("/api/Storage/articles/" + articleId)
            .get()
            .json(json => {
                articleData.Title = json.title;
                articleData.Content = json.content;
                articleData.Created = json.created;
                articleData.Author = json.author.name;
                articleData.TimeToRead = calculateTimeToRead(json.content);

                // Fill up additional resources.
                var encodedTitle = encodeURIComponent(json.title);
                additionalResources.Mesh = 'https://uchebnik.mos.ru/catalogue?search=' + encodedTitle + '&sort_column=relevance';
                additionalResources.YouTube = 'https://www.youtube.com/results?search_query=' + encodedTitle;
            })
            .catch(error => {
                redirect("/error");
        })
        }
        else if ($user.Jwt != undefined)
        {
            httpClient.url("/api/Storage/articles/private")
                .auth(`Bearer ${$user.Jwt}`)
                .get()
                .json(json => {
                    userArticles = json;
                })
                .catch(error => {
				    notyf.error("Невозможно загрузить статьи пользователя!");
        	});
        }
    });

    function calculateTimeToRead(content)
    {
        var wordsCount = content.split(' ').filter(word => word.length >= 3).length;
		var time = Math.ceil(wordsCount / 2);
		var minutes = time / 60;
		var seconds = time % 60;
		
		return { Minutes: minutes, Seconds: seconds };
	}
</script>

<Page>
    {#if articleId == undefined}
        <MainTitle>Ваши статьи:</MainTitle>
        {#if userArticles != undefined}
            <CardsHolder>
                <EmptyCard>
                    {#each userArticles as {id, title, content}}
			            <Card styles='min-height: 290px; min-width: 250px; margin: 10px 5px;'>
				            <Title>{title}</Title>
				            <PreTitle>{@html content}</PreTitle>
				            <Button styles="margin-bottom: 20px;" href="/articles/{id}">Читать</Button>
			            </Card>
		            {/each}
                </EmptyCard>
            </CardsHolder>

            {:else}
                <Title>Загрузка...</Title>
        {/if}

        {:else}
            {#if articleData != undefined}
                <EmptyCard styles='min-height: 290px; min-width: 250px; margin: 10px 5px;'>
                    <MainTitle>{articleData.Title}</MainTitle>
                    <PreTitle>Автор: {articleData.Author}</PreTitle>
                    <PreTitle>Опубликовано: {articleData.Created}</PreTitle>
                    {#if articleData.TimeToRead.Minutes < 1}
                        {#if articleData.TimeToRead.Seconds < 5}
                            <PreTitle>Время чтения: {articleData.TimeToRead.Seconds} секунды</PreTitle>

                            {:else}
                                <PreTitle>Время чтения: {articleData.TimeToRead.Seconds} секунд</PreTitle>
                        {/if}
                        {:else}
                            {#if articleData.TimeToRead.Minutes < 5}
                                <PreTitle>Время чтения: {articleData.TimeToRead.Minutes} минуты</PreTitle>

                                {:else}
                                <PreTitle>Время чтения: {articleData.TimeToRead.Minutes} минут</PreTitle>
                            {/if}
                    {/if}
                    <Title>{@html articleData.Content}</Title>

                    <Title>Проверка</Title>


                    <Title>Дополнительные материалы</Title>
                    <EmptyCard>
                        <InlineLink styles="font-size: 25px;" href={additionalResources.Mesh}>МЭШ</InlineLink>
                        <InlineLink styles="font-size: 25px;" href={additionalResources.YouTube}>YouTube</InlineLink>
                    </EmptyCard>
                </EmptyCard>

            {:else}
                <Title>Загрузка...</Title>
            {/if}
    {/if}
</Page>