<template>
	<v-app>
		<v-navigation-drawer
			v-model="drawer"
			app
			v-if="mobile"
			class="mobile-nav"
		>
			<v-list dense nav>
				<v-list-item :to="{ name: 'home'}">
					<v-list-item-content>
						Home
					</v-list-item-content>
				</v-list-item>
				<v-list-item :to="{ name: 'restricted'}">
					<v-list-item-content>
						Non-Public
					</v-list-item-content>
				</v-list-item>
				<v-list-item :to="{ name: 'admin'}">
					<v-list-item-content>
						Admin
					</v-list-item-content>
				</v-list-item>
				<v-list-item :to="{ name: 'account-register'}" v-if="!signedIn">
					<v-list-item-content>
						Register
					</v-list-item-content>
				</v-list-item>
				<v-list-item v-if="!signedIn" :to="{ name: 'account-login'}">
					<v-list-item-content>Login</v-list-item-content>
					<v-list-item-action><v-icon title="Logout">fas fa-sign-in-alt</v-icon></v-list-item-action>
				</v-list-item>
				<v-list-item v-else @click="logout">
					<v-list-item-content>Logout</v-list-item-content>
					<v-list-item-action><v-icon title="Logout">fas fa-sign-out-alt</v-icon></v-list-item-action>
				</v-list-item>
			</v-list>
		</v-navigation-drawer>

		<v-app-bar
			color="lightGrey"
			height="128"
			app
			flat
			absolute
		>
			<v-app-bar-nav-icon v-if="mobile" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
			<div class="nav-logo">
				<img src="/images/logo.png" alt="Vue Logo">
			</div>

			<v-spacer></v-spacer>

			<template v-if="!mobile">
				<v-btn text :to="{ name: 'home'}">Home</v-btn>
				<v-btn text :to="{ name: 'restricted'}">Non-Public</v-btn>
				<v-btn text :to="{ name: 'admin'}">Admin-Only</v-btn>
				<v-btn v-if="!signedIn" text :to="{ name: 'account-register'}">Register</v-btn>
				<v-btn v-if="!signedIn" text :to="{ name: 'account-login'}">Login <v-icon title="Login" class="ml-1">fas fa-sign-in-alt</v-icon></v-btn>
				<v-btn v-else icon @click="logout"><v-icon title="Logout">fas fa-sign-out-alt</v-icon></v-btn>
			</template>
		</v-app-bar>
		<v-main>
			<v-container>
				<router-view></router-view>
			</v-container>
		</v-main>
	</v-app>
</template>
<script>
import commonMixin from "@/common-mixin";
import { actionTypes } from "./store-types";

export default {
	name: "app",
	mixins: [commonMixin],
	data() {
		return {
			drawer: false,
		};
	},
	mounted() {
	},
	watch: {
		$route() {
			// update the page title when the route changes
			document.title = this.$route.meta.title;
		},
	},
	computed: {
	},
	methods: {
		logout() {
			this.$store.dispatch(actionTypes.CLEAR_LOGIN)
				.then(() => {
					this.$router.push({ name: "account-login" });
				});
		},
	},
};
</script>
<style lang="scss">
	@import "@/styles/variables";

	.nav-logo {
		img {
			max-height: 128px;
			max-width: 128px;
		}
	}

	header {
		.v-btn__content {
			font-size: 15px;
			font-weight: 400;
			text-transform: none;
			letter-spacing: normal;
		}

		.v-btn--active {
			.v-btn__content {
				color: $primary;
				font-weight: 700;
			}
		}

		.theme--light.v-btn:hover::before {
			opacity: 0;
		}

		.theme--light.v-btn--active:hover::before, .theme--light.v-btn--active::before {
			opacity: 0;
		}
	}
</style>
