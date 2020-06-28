<template>
	<div>
		<v-row>
			<v-col cols="12" md="6" offset-md="3" lg="4" offset-lg="4" class="my-12">
				<div class="px-12">
					<h3 class="text-center">Password Reset</h3>

					<div v-if="!complete">
						<h5 class="text-center">Enter a new password for your account</h5>

						<ValidationObserver v-slot="{ invalid }">
							<v-row class="mt-6">
								<v-col>
									<ValidationProvider v-slot="{ errors }" name="Password" rules="required|min:8">
										<v-text-field
											:type="passwordVisible ? 'text' : 'password'"
											:append-icon="passwordVisible ? 'far fa-eye' : 'far fa-eye-slash'"
											@click:append="passwordVisible = !passwordVisible"
											label="New Password"
											class="req"
											outlined
											v-model="password"
											:error-messages="errors"
											hint="8 character minimum"
											persistent-hint
										></v-text-field>
									</ValidationProvider>
								</v-col>
							</v-row>
							<v-alert type="error" :value="message !== null" tile class="my-6">{{message}}</v-alert>
							<v-row>
								<v-col class="text-center">
									<v-btn color="primary" :disabled="invalid || posting" @click="submit" :loading="posting">Submit</v-btn>
								</v-col>
							</v-row>
						</ValidationObserver>
					</div>
					<div v-else>
						<v-alert type="success" tile class="my-6">Your password has been changed and you are now logged in.</v-alert>
					</div>
				</div>
			</v-col>
		</v-row>
	</div>
</template>
<script>
import api from "@/api";
import { actionTypes } from "@/store-types";

export default {
	mounted() {
	},
	data() {
		return {
			posting: false,
			password: null,
			message: null,
			complete: false,
			passwordVisible: false,
		};
	},
	methods: {
		submit() {
			this.posting = true;
			this.message = null;

			const request = {
				newPassword: this.password,
				emailToken: this.$route.params.id,
			};

			api.passwordResetComplete(request)
				.then(
					({ data }) => {
						this.posting = false;

						this.$store.dispatch(actionTypes.SAVE_LOGIN, data)
							.then(() => {
								this.complete = true;
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
