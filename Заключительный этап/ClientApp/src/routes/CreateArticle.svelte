<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
    import InputField from "../components/InputField.svelte";
    import { notyf } from "../notyf";
    import { httpClient } from "../httpClient";
    import Button from "../components/Button.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
    import { user } from "../storage";
    import { delayedRedirect } from "../helperFunctions";
    import Select from "../components/Select.svelte";
    import TextArea from "../components/TextArea.svelte";

    let articleRequest = { article: { title: "", content: "", type: 0, tags: null, images: null, created: "" } };

    let tags;

    function createArticle()
    {
        //articleRequest.article.tags = tags.split(/(\s+)/).filter(e => e.trim().length > 0);
        var currentDate = new Date();
	    var currentDay = currentDate.getDate();
	    var currentMonth = currentDate.getMonth() + 1;
	    var currentYear = currentDate.getFullYear();
	
	    let fullDate = currentDay + '.' + currentMonth + '.' + currentYear;

        articleRequest.article.created = fullDate;
        
        httpClient.url("/api/Storage/articles")
            .auth(`Bearer ${$user.Jwt}`)
            .post(articleRequest)
            .json(json => {
                notyf.success("Статья добавлена!");
                delayedRedirect("/", 2000);
            })
            .catch(error => {
                notyf.error("Невозможно добавить статью, попробуйте поменять ее название!");
            });
    }
</script>

<Page>
	<MainTitle>Опубликовать статью</MainTitle>
	<Title>Пожалуйста, <b>заполните</b> эти поля, чтобы продолжить:</Title>
    <EmptyCard styles="min-width: 270px; max-width: 450px;">
        <InputField inputText="Название" bind:value={articleRequest.article.title} />
        <TextArea inputText="Текст статьи. Должен содержать по крайней мере 30 слов." bind:value={articleRequest.article.content} />
        <InputField inputText="Тэги для поиска" bind:value={tags} />
        <Select title="Тип статьи" id="ArticleType" bind:value={articleRequest.article.type} items={['Личная', 'Публичная']}/>
        <Button onClick={() => createArticle()}>Отправить</Button>
    </EmptyCard>
</Page>