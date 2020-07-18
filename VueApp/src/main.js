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
import { actionTypes, commitTypes } from "@/store-types";
import App from "@/App.vue";
import "@/styles/vueapp.scss";

Vue.config.productionTip = false;

// ******** router ********
Vue.use(VueRouter);

// ******** validation ********
// vee-validate common components
Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);

// ******** directives ********
// v-integer and v-decimal
Vue.use(NumericDirective);

// ******** other components ********
Vue.use(Toast, {
	timeout: 2000,
});

// load any starting data the app needs from the server like paths, keys, lists
store.dispatch(actionTypes.APP_LOAD)
	// checks to see if a JWT cookie exists and validates it
	.then(() => store.dispatch(actionTypes.CHECK_TOKEN))
	.then(() => {
		// configure the router navigation guards to check auth before following routes on each navigation event
		// roles are assigned to routes in routes.js
		router.beforeEach((to, from, next) => {
			// public routes, no access value set or it's set to *
			if (!Object.prototype.hasOwnProperty.call(to.meta, "access") || to.meta.access.includes("*")) {
				next();
				return;
			}

			// private route, users that aren't signed in go to login
			if (!store.state.signedIn) {
				// store the path they were trying to go so we can send them there after login
				store.commit(commitTypes.SET_REDIRECT, { name: to.name, params: to.params });
				next({ name: "account-login" });
				return;
			}

			// user doesn't have the correct role
			if (!to.meta.access.includes(store.state.user.role)) {
				next({ name: "denied" });
				return;
			}

			// correct roles, continue
			next();
		});

		// create the main Vue instance
		new Vue({
			router,
			store,
			vuetify,
			render: (h) => h(App),
		}).$mount("#app");

		// GLOBAL ERROR LOGGING AND HANDLING
		// this sends client-side errors to the server for logging in the database

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
