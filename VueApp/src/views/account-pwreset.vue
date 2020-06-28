<template>
	<div class="view-login">
		<v-row>
			<v-col cols="12" md="6" offset-md="3" class="my-12">
				<div class="center-panel px-12">
					<h1 class="text-center">Password Reset</h1>

					<ValidationObserver v-slot="{ invalid }">
					<div v-if="!submitComplete">
						<v-row>
							<v-col>
								<ValidationProvider v-slot="{ errors }" name="Email" rules="required|email">
									<v-text-field
										label="Email"
										class="req"
										outlined
										v-model="email"
										:error-messages="errors"
									></v-text-field>
								</ValidationProvider>
							</v-col>
						</v-row>

						<v-alert type="warning" :value="message !== null" tile class="my-6"><span class="dark-grey--text">{{message}}</span></v-alert>

						<v-row>
							<v-col class="text-center">
								<v-btn color="primary" :disabled="invalid || posting" @click="submit" :loading="posting">Submit</v-btn>
							</v-col>
						</v-row>
					</div>
					</ValidationObserver>
					<div v-if="submitComplete">
						<v-row>
							<v-col class="green--text text-center mb-8">
								<v-icon class="green--text mr-2">fas fa-check-circle</v-icon> <b>You have been sent an email with a password reset link.</b>
							</v-col>
						</v-row>
					</div>
				</div>
			</v-col>
		</v-row>
	</div>
</template>
<script>
import api from "@/api";

export default {
	mounted() {
	},
	data() {
		return {
			submitComplete: false,
			posting: false,
			email: null,
			message: null,
		};
	},
	methods: {
		submit() {
			this.posting = true;
			this.message = null;

			api.passwordReset({ email: this.email })
				.then(
					() => {
						this.submitComplete = true;
						this.posting = false;
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
