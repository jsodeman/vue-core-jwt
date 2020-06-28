import Vue from "vue";
import Vuetify from "vuetify/lib";

Vue.use(Vuetify);

export default new Vuetify({
	theme: {
		options: {
			// customProperties: true,
		},
		themes: {
			light: {
				primary: "#00A7CB",
				secondary: "#162850",
				accent: "#f8d277",
				error: "#FF5252",
				info: "#2196F3",
				success: "#4CAF50",
				warning: "#FFC107",
				owlGrey: "#a5a5a5",
				lightGrey: "#f2f2f2",
				darkGrey: "#4f4f4f",
			},
		},
	},
	icons: {
		iconfont: "fa",
		values: {
			// sort: "far fa-sort-up",
		},
	},
});
