import Vuex from "vuex";
import Vue from "vue";
import { actionTypes, commitTypes } from "./store-types";
import api from "./api";

Vue.use(Vuex);

const defaultUser = () => ({
	id: null,
	email: null,
	role: null,
	name: null,
	customInfo: null,
	token: null,
	firstLogin: false,
});

export default new Vuex.Store({
	state: {
		user: defaultUser(),
		signedIn: false,
		config: {
			someServiceApiKey: null,
			validateEmail: false,
		},
		loginRedirect: null,
	},
	mutations: {
		[commitTypes.SET_USER](state, user) {
			Object.assign(state.user, user);
			state.signedIn = user.token !== null;
		},
		[commitTypes.SET_SETTINGS](state, config) {
			state.config = config;
		},
		[commitTypes.SET_REDIRECT](state, val) {
			state.loginRedirect = val;
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
		[actionTypes.SAVE_LOGIN]({ commit, state }, authInfo) {
			return new Promise((resolve) => {
				// if the user was trying to get to a path send them along
				// otherwise set to whatever landing page you like based on their role
				// optional redirect for first time users
				// TODO: customize for your needs

				let redirect = state.loginRedirect ? state.loginRedirect : { name: "home", params: {} };
				commit(commitTypes.SET_REDIRECT, null);

				if (authInfo.firstLogin) {
					redirect = { name: "account-register-complete", params: {} };
				}

				commit(commitTypes.SET_USER, authInfo);

				resolve(redirect);
			});
		},
		[actionTypes.CLEAR_LOGIN]({ commit }) {
			commit(commitTypes.SET_USER, defaultUser());

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
