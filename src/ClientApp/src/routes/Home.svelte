<script>
	import Card from "../components/Card.svelte";
	import CardsHolder from "../components/CardsHolder.svelte";
	import Page from "../components/Page.svelte";
	import PreTitle from "../components/PreTitle.svelte";
	import Title from "../components/Title.svelte";
	import MainTitle from "../components/MainTitle.svelte";
	import Button from "../components/Button.svelte";
	import { user, highLevelApi } from '../storage';
	import { httpClient } from "../httpClient";
	import { notyf } from "../notyf";
	import EmptyCard from "../components/EmptyCard.svelte";
	import InputField from "../components/InputField.svelte";
	import Outline from "../components/Outline.svelte";
	import ExtendedTitle from "../components/ExtendedTitle.svelte";

	let features = [
		{ title: 'Design', description: 'Our service implementing modern and user-friendly design.', style: "max-width: 35vw;" },
		{ title: 'Technologies', description: 'We are using future-proof frameworks to achieve the perfect balance of performance and convenience.', style: "max-width: 45vw;" },
		{ title: 'Cross-Platform', description: 'Cross-platform is an important constituent of any product. You can use KnOwl in your browser and in Telegram or Vk', style: "" }
	];

	let item = {
		deliverData: "",
		fetchedData: "",
		fetchedCellData: ""
	};

	function deliverItem()
	{
		httpClient.url($highLevelApi + "/get_item_from_storage_json")
			.query({ id: item.deliverData })
			.options({ mode: "no-cors" })
			.get()
			.text(t => notyf.success(t))
			.catch(error => notyf.error("There is no item with this id!"));
	}

	function searchItem()
	{
		httpClient.url($highLevelApi + "/get_data_from_uuid_json")
			.query({ id: item.fetchedData })
			.options({ mode: "no-cors" })
			.get()
			.json(j => notyf.success("Found item with this id. Name: " + j.name + "Width: " + j.size_width + "Height: " + j.size_height + "Depth: " + j.size_depth))
			.catch(error => notyf.error("There is no item with this id!"));
	}

	function searchCell()
	{
		httpClient.url($highLevelApi + "/get_cell_json")
			.query({ id: item.fetchedCellData })
			.options({ mode: "no-cors" })
			.get()
			.json(j => notyf.success("Found item with this id. Name: " + j.name + "Width: " + j.size_width + "Height: " + j.size_height + "Depth: " + j.size_depth))
			.catch(error => notyf.error("There is no item with this id!"));
	}
</script>

{#if $user.IsAuth}
	<Page>
			<CardsHolder>
				<EmptyCard>
					<MainTitle>Welcome to management panel, <b>{$user.Name}</b>!</MainTitle>
					<Title><b>Current</b> storage scheme:</Title>
					<img src="{$highLevelApi}/storage_image" alt="Storage Scheme" />
				</EmptyCard>
				<CardsHolder styles="justify-content: center;">
				<EmptyCard>
					<Title>Deliver item</Title>
					<InputField inputText="Id or name of item" bind:value={item.deliverData} />
					<Button onClick={() => deliverItem()}>Deliver</Button>
				</EmptyCard>
				<EmptyCard>
					<Title>Search item</Title>
					<InputField inputText="Id" bind:value={item.fetchedData} />
					<Button onClick={() => searchItem()}>Search</Button>
				</EmptyCard>
				<EmptyCard>
					<Title>Search cell</Title>
					<InputField inputText="Id or name of cell" bind:value={item.fetchedCellData} />
					<Button onClick={() => searchCell()}>Search</Button>
				</EmptyCard>
				</CardsHolder>
				<EmptyCard>
					<MainTitle>PDF</MainTitle>
					<CardsHolder styles="justify-content: center;">
						<EmptyCard>
							<Title>Get <b>PDF</b> of the main storage</Title>
							<Button href="{$highLevelApi}/pdf_main">Open</Button>
						</EmptyCard>
						<EmptyCard>
							<Title>Get <b>PDF</b> of the remote storage</Title>
							<Button href="{$highLevelApi}/pdf_remote">Open</Button>
						</EmptyCard>
					</CardsHolder>
				</EmptyCard>
			</CardsHolder>
	</Page>
	{:else}
		<Page>
			<MainTitle>Hi, this is <b>KnOwl</b></MainTitle>
			<Title>A place where you can <b>save</b> your notes.</Title>
			<PreTitle>Easily manage your data from simple and fast panel.</PreTitle>
			<Button href="/signup">Let's go!</Button>
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

		<Page>
			<Title>Contacts</Title>
			<PreTitle>&copy; {new Date().getFullYear()} - ReSharper.</PreTitle>
		</Page>
{/if}