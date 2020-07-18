<template>
	<div>
		<v-row v-if="!emailSent">
			<v-col cols="12" md="6" offset-md="3" lg="4" offset-lg="4" class="my-12">

				<ValidationObserver v-slot="{ invalid }">
					<form autocomplete="off">
						<div class="px-12">
							<h3 class="mb-6">Register</h3>

							<v-row dense>
								<v-col>
									<ValidationProvider v-slot="{ errors }" name="Name" rules="required">
										<v-text-field dense v-model="account.name" label="Name" class="req" outlined :error-messages="errors" />
									</ValidationProvider>
								</v-col>
							</v-row>
							<v-row dense>
								<v-col>
									<ValidationProvider v-slot="{ errors }" name="Email" rules="required|email">
										<v-text-field dense v-model="account.email" label="Email" class="req" outlined :error-messages="errors" />
									</ValidationProvider>
								</v-col>
							</v-row>
							<v-row dense>
								<v-col>
									<ValidationProvider v-slot="{ errors }" name="Password" rules="required|min:8">
										<v-text-field dense
											:type="passwordVisible ? 'text' : 'password'"
											:append-icon="passwordVisible ? 'far fa-eye' : 'far fa-eye-slash'"
											@click:append="passwordVisible = !passwordVisible"
											v-model="account.password"
											hint="8 character minimum"
											persistent-hint
											label="Password" class="req" outlined :error-messages="errors" />
									</ValidationProvider>
								</v-col>
							</v-row>

							<v-alert type="error" :value="message !== null" tile class="my-6">{{message}}</v-alert>

							<v-row dense>
								<v-col>
									<v-btn color="primary" :disabled="invalid || posting" @click="register" :loading="posting">Register</v-btn>
								</v-col>
							</v-row>
						</div>
					</form>
				</ValidationObserver>
			</v-col>
		</v-row>
		<v-row v-else>
			<v-col cols="12" md="6" offset-md="3" lg="4" offset-lg="4" class="my-12">
					<div class="px-12">
						<h3 class="mb-6">Register</h3>

						<p>Please check your email for a link to confirm you account and complete your registration.</p>
					</div>
			</v-col>
		</v-row>
	</div>
</template>
<script>
import api from "@/api";
import { actionTypes } from "@/store-types";

export default {
	components: {
	},
	mounted() {
	},
	data() {
		return {
			posting: false,
			account: {
				name: null,
				email: null,
				password: null,
			},
			message: null,
			passwordVisible: false,
			emailSent: false,
		};
	},
	computed: {
	},
	methods: {
		register() {
			this.posting = true;
			this.message = null;

			api.register(this.account)
				.then(
					({ data }) => {
						this.posting = false;
						// if email validation is required
						if (this.$store.state.config.validateEmail) {
							this.emailSent = true;
							return;
						}

						// no email val required, log use in and send to the next page
						this.$store.dispatch(actionTypes.SAVE_LOGIN, data)
							.then((redirect) => {
								this.$router.push(redirect);
							});
					},
					(r) => {
						this.message = r.response.data;
						this.posting = false;
					},
				);
		},
	},
};
</script>
<style lang="scss">
</style>
