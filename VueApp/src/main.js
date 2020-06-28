import Vue from "vue";
import VueRouter from "vue-router";
import NumericDirective from "@/vue-numeric-directive";
import Toast from "vue-toastification";
import "@/validation";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import vuetify from "@/plugins/vuetify";
import api from "@/api";
import router from "@/router/router";
import store from "@/store";
import { actionTypes } from "@/store-types";
import App from "@/App.vue";
import "@/styles/vueapp.scss";

Vue.config.productionTip = false;

// router
Vue.use(VueRouter);

// validation
Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);

// directives
Vue.use(NumericDirective);

// components
Vue.use(Toast, {
	timeout: 2000,
});

Vue.mixin({
	computed: {
		currentUser() {
			return this.$store.state.user;
		},
		signedIn() {
			return this.$store.state.signedIn;
		},
		mobile() {
			return this.$vuetify.breakpoint.mobile;
		},
	},
});

// load the initial app state from the API
// check an existing token if there is already one
store.dispatch(actionTypes.APP_LOAD)
	.then(() => store.dispatch(actionTypes.CHECK_TOKEN))
	.then(() => {
		// use navigation guards to check auth before following route
		router.beforeEach((to, from, next) => {
			// public routes
			if (!Object.prototype.hasOwnProperty.call(to.meta, "access") || to.meta.access.includes("*")) {
				next();
				return;
			}

			// users that aren't signed in
			if (!store.state.signedIn) {
				next({ name: "account-login" });
				return;
			}

			// incorrect roles
			if (!to.meta.access.includes(store.state.user.role)) {
				next({ name: "denied" });
				return;
			}

			// correct roles
			next();
		});

		new Vue({
			router,
			store,
			vuetify,
			render: (h) => h(App),
		}).$mount("#app");

		// GLOBAL ERROR LOGGING AND HANDLING

		// don't send errors to the API in development
		if (process.env.NODE_ENV === "development") {
			console.log("Error recording disabled for development");
			return;
		}

		// Vue errors
		Vue.config.errorHandler = (err, vm, info) => {
			const e = {
				error: JSON.parse(JSON.stringify(err, Object.getOwnPropertyNames(err))),
				vm: vm ? {
					name: vm.name,
					route: vm.$router.currentRoute.fullPath,
					tag: vm.$vnode.tag,
				} : {},
				info: info,
			};

			const logInfo = { source: "Vue", user: store.state.user, data: e };

			try {
				api.log(logInfo);
			} catch (x) {
				console.log(logInfo);
			}

			return false;
		};

		// global JS error logging
		window.addEventListener("error", (err) => {
			const e = {
				error: JSON.parse(JSON.stringify(err, Object.getOwnPropertyNames(err))),
			};

			const logInfo = { source: "Global Errors", user: store.state.user, data: e };

			try {
				api.log(logInfo);
			} catch (x) {
				console.log(logInfo);
			}
		});

		// unhandled Promise rejection error logging
		window.addEventListener("unhandledrejection", (err) => {
			const e = {
				error: JSON.parse(JSON.stringify(err, Object.getOwnPropertyNames(err))),
			};

			const logInfo = { source: "Unhandled Rejections", user: store.state.user, data: e };

			try {
				api.log(logInfo);
			} catch (x) {
				console.log(logInfo);
			}
		});
	});
