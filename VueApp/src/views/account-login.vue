<template>
	<div>
		<v-row>
			<v-col cols="12" md="6" offset-md="3" lg="4" offset-lg="4" class="my-12">
				<div class="px-12">
					<h3 class="text-center">Welcome Back</h3>
					<h5 class="text-center">Please login to your account</h5>

					<v-row>
						<v-col class="caption">Demo Accounts:</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="info" outlined @click="demo('normal@test.com','password')" class="all-case">Normal: normal@test.com / password</v-btn>
						</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="info" outlined @click="demo('admin@test.com','password')" class="all-case">Admin: admin@test.com / password</v-btn>
						</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="warning" outlined @click="demo('unconfirmed@test.com','password')" class="all-case">Unconfirmed: admin@test.com / password</v-btn>
						</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="success" outlined :to="{ name: 'account-confirm', params: { id: 'db06011dca3a4276aaba2fab9547286b'} }" class="all-case">Sample Good Account Confirmation Link</v-btn>
						</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="error" outlined :to="{ name: 'account-confirm', params: { id: 'db06011dca3a4276aaba2fab95472111'} }" class="all-case">Sample Bad Account Confirmation Link</v-btn>
						</v-col>
					</v-row>
					<v-row>
						<v-col>
							<v-btn color="success" outlined :to="{ name: 'pw-reset-complete', params: { id: '272065a0222948c2ad5b6cdefb11066e'} }" class="all-case">Sample PW Reset Link</v-btn>
						</v-col>
					</v-row>

					<ValidationObserver v-slot="{ invalid }">

						<v-row dense class="mt-4">
							<v-col>
								<ValidationProvider v-slot="{ errors }" name="Email" rules="required|email">
									<v-text-field
										label="Email"
										placeholder="Email"
										outlined
										v-model="form.email"
										:error-messages="errors"
										class="req v-input--is-label-active"
									></v-text-field>
								</ValidationProvider>
							</v-col>
						</v-row>
						<v-row dense>
							<v-col>
								<ValidationProvider v-slot="{ errors }" name="Password" rules="required">
									<v-text-field
										label="Password"
										placeholder="Password"
										outlined
										:type="passwordVisible ? 'text' : 'password'"
										v-model="form.password"
										:error-messages="errors"
										@keyup.enter="passwordEnter"
										class="req v-input--is-label-active"
										@click:append="passwordVisible = !passwordVisible"
										:append-icon="passwordVisible ? 'far fa-eye' : 'far fa-eye-slash'"
									></v-text-field>
								</ValidationProvider>
							</v-col>
						</v-row>
						<v-alert type="warning" :value="message !== null" tile class="my-6"><span class="dark-grey--text">{{message}}</span></v-alert>
						<v-row>
							<v-col>
								<v-btn color="primary" class="mr-4" :disabled="invalid || posting" @click="login" :loading="posting">Login</v-btn>

								<p class="mt-4"><router-link :to="{ name: 'pw-reset' }">Password Reset</router-link></p>
								<p><router-link :to="{ name: 'account-register' }">Register</router-link></p>
							</v-col>
						</v-row>

					</ValidationObserver>
				</div>
			</v-col>
		</v-row>
	</div>
</template>
<script>
import api from "@/api";
import { actionTypes } from "@/store-types";

export default {
	name: "account-login",
	mounted() {
	},
	data() {
		return {
			posting: false,
			form: {
				email: null,
				password: null,
			},
			message: null,
			passwordVisible: false,
		};
	},
	methods: {
		login() {
			this.posting = true;
			this.message = null;

			api.login(this.form)
				.then(
					({ data }) => {
						this.posting = false;
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
		passwordEnter() {
			if (this.errors.any()) {
				return;
			}
			this.login();
		},
		demo(user, pw) {
			this.form.email = user;
			this.form.password = pw;
		},
	},
};
</script>
<style lang="scss">
</style>
