<script>
	import Page from "../components/Page.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
	import { user } from '../storage';
	import { signOut, redirect } from '../helperFunctions';
	import Button from "../components/Button.svelte";
	import CardsHolder from "../components/CardsHolder.svelte";
    import EmptyCard from "../components/EmptyCard.svelte";
	import { notyf } from "../notyf";
	import { httpClient } from "../httpClient";
	import InputField from "../components/InputField.svelte";
	import InputFile from "../components/InputFile.svelte";

	let userId;
	let userDataById;

	let moderatingArticles;

	function getUser()
	{
		httpClient.url("/api/Authentication/" + userId)
            .auth(`Bearer ${$user.Jwt}`)
            .get()
            .json(json => {
                userDataById = json;
            })
            .catch(error => {
                notyf.error("Невозможно загрузить пользователя!");
        });
	}
</script>

<Page>
	<MainTitle>Профиль</MainTitle>

	{#if $user.IsAuth}
		<EmptyCard styles="max-width: 450px;">
			<Title>{$user.Name}</Title>
			<Title>Ваша роль: {$user.Role}</Title>
			<Button href="/articles">Мои статьи</Button>
			<Button onClick={() => { signOut(); redirect("/"); } }>Выйти</Button>

			<form action="api/storage/images" method="post" enctype="multipart/form-data">
				<InputFile name="files">
					Выбрать файлы
				</InputFile>
				<label>
					<input type="submit" name="button" id="button" value="Submit" />
				</label>
			</form>

			{#if $user.Role == "Admin"}
				<MainTitle>Панель управления</MainTitle>
				<CardsHolder styles="justify-content: center;">
					<EmptyCard>
						<Title>Получить <b>пользователя</b> по Id</Title>
						<InputField inputText="Id" bind:value={userId} />
						<div class="container">
							<div class="table">
								<div class="table-header">
									<div class="header__item"><a id="id" class="filter__link">Id</a></div>
									<div class="header__item"><a id="name" class="filter__link filter__link--number">Имя</a></div>
									<div class="header__item"><a id="role" class="filter__link filter__link--number">Роль</a></div>
								</div>
								<div class="table-content">
									<div class="table-row">
										{#if userDataById != undefined}	
											<div class="table-data">{userDataById.id}</div>
											<div class="table-data">{userDataById.name}</div>
											<div class="table-data">{userDataById.role}</div>
										{/if}
									</div>
								</div>	
							</div>
						</div>
						<Button onClick={() => getUser() }>Загрузить данные пользователя</Button>
					</EmptyCard>
				</CardsHolder>
			{/if}
			{#if $user.Role == "Moderator"}
				<MainTitle>Панель управления</MainTitle>

			{/if}
    	</EmptyCard>
		{:else}
			<Title>Пожалуйста, войдите в свой аккаунт.</Title>
	{/if}
</Page>

<style>
	.container {
  max-width: 1000px;
  margin: 15px;
  margin-right: auto;
  margin-left: auto;
  display: flex;
  justify-content: center;
  align-items: center;
}

.table {
  width: 100%;
  border: 1px solid #EEEEEE;
}

.table-header {
  display: flex;
  width: 100%;
  background: #000;
  padding: 18px 0;
}

.table-row {
  display: flex;
  width: 100%;
  padding: 18px 0;
}
.table-row:nth-of-type(odd) {
  background: #EEEEEE;
}

.table-data, .header__item {
  flex: 1 1 20%;
  text-align: center;
}

.header__item {
  text-transform: uppercase;
}

.filter__link {
  color: white;
  text-decoration: none;
  position: relative;
  display: inline-block;
  padding-left: 24px;
  padding-right: 24px;
}
.filter__link::after {
  content: "";
  position: absolute;
  right: -18px;
  color: white;
  font-size: 12px;
  top: 50%;
  transform: translateY(-50%);
}
</style>