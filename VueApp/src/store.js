import Vuex from "vuex";
import Vue from "vue";
import { actionTypes, commitTypes } from "./store-types";
import api from "./api";

Vue.use(Vuex);

export default new Vuex.Store({
	state: {
		user: {
			id: null,
			email: null,
			role: null,
			name: null,
			customInfo: null,
			token: null,
			firstLogin: false,
		},
		signedIn: false,
		config: {
			someServiceApiKey: null,
			validateEmail: false,
		},
	},
	mutations: {
		[commitTypes.SET_USER](state, user) {
			state.user = user;
			state.signedIn = user.token !== null;
		},
		[commitTypes.SET_SETTINGS](state, config) {
			state.config = config;
		},
	},
	actions: {
		// initial data load from the server: paths, config info, lists, keys, etc
		[actionTypes.APP_LOAD]({ commit }) {
			return new Promise((resolve, reject) => {
				api.appSettings()
					.then(({ data }) => {
						commit(commitTypes.SET_SETTINGS, data);
						resolve();
					}).catch((e) => {
						reject(e);
					});
			});
		},
		[actionTypes.SAVE_LOGIN]({ commit }, authInfo) {
			return new Promise((resolve) => {
				// optional redirect for first time users
				// otherwise set to whatever landing page you like based on their role
				let redirect = { name: "home" };
				if (authInfo.firstLogin) {
					redirect = { name: "account-register-complete" };
				}

				commit(commitTypes.SET_USER, authInfo);

				resolve(redirect);
			});
		},
		[actionTypes.CLEAR_LOGIN]({ commit }) {
			const user = {
				id: null,
				email: null,
				role: null,
				name: null,
				customInfo: null,
				token: null,
				firstLogin: false,
			};
			commit(commitTypes.SET_USER, user);

			return api.logout();
		},
		// call the API to see if the JWT cookie exists and is good
		// gets a fresh cookie back and the latest user info
		[actionTypes.CHECK_TOKEN]({ dispatch }) {
			return api.jwtCheck().then((r) => {
				if (r.status === 200) {
					// store the fresh user info
					return dispatch(actionTypes.SAVE_LOGIN, r.data);
				}

				// check failed, clear the user, the nav guard will send them to login
				return dispatch(actionTypes.CLEAR_LOGIN);
			}).catch(() => dispatch(actionTypes.CLEAR_LOGIN));
		},
	},
});
