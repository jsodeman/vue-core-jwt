module.exports = {
	devServer: {
		progress: false,
	},
	css: {
		sourceMap: true,
		loaderOptions: {
			sass: {
				implementation: require("sass"),
			},
		},
	},
	configureWebpack: {
		devtool: "eval-source-map",
	},
	transpileDependencies: [
		"vuetify",
	],
};
