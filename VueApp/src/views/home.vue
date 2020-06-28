<template>
	<div>
		<h1>Home</h1>

		<p>This page is public.</p>

		<v-alert type="info" v-if="signedIn">
			User: {{$store.state.user.name}} | Role: {{$store.state.user.role}}
		</v-alert>
		<v-alert type="info" v-else>
			Not signed in
		</v-alert>
	</div>
</template>

<script>
export default {
	name: "home",
	components: {},
	props: {
	},
	data() {
		return {};
	},
	computed: {},
	watch: {},
	created() {
	},
	mounted() {
	},
	methods: {},
};
</script>

<style lang="scss">

</style>
