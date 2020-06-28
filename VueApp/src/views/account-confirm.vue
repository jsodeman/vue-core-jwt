<template>
	<div>
		<v-progress-linear :active="checking" indeterminate />

		<div v-if="message">
			<h1>Account Confirmation</h1>

			<v-alert type="warning" tile class="my-6"><span class="dark-grey--text">{{message}}</span></v-alert>
		</div>

	</div>
</template>

<script>
import api from "@/api";
import { actionTypes } from "@/store-types";

export default {
	name: "account-confirmation",
	components: {},
	props: {
	},
	data() {
		return {
			checking: true,
			message: null,
		};
	},
	computed: {},
	watch: {},
	created() {
		api.confirm({ emailToken: this.$route.params.id })
			.then(
				({ data }) => {
					this.checking = false;
					this.$store.dispatch(actionTypes.SAVE_LOGIN, data)
						.then((redirect) => {
							this.$router.push(redirect);
						});
				},
				(r) => {
					this.message = r.response.data;
					this.checking = false;
				},
			);
	},
	mounted() {
	},
	methods: {},
};
</script>

<style
	lang="scss">

</style>
