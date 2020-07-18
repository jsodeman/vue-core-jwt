// this mixin provides an easy way for components to access the most used
// data from the store and Vuetify state
// you could apply this globally but 3rd party components will also get these
// values and you may end up with naming conflicts
export default {
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
};
