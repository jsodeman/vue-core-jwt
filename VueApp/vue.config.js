module.exports = {
	devServer: {
		progress: false,
	},
	css: {
		sourceMap: true,
	},
	configureWebpack: {
		devtool: "eval-source-map",
	},
	transpileDependencies: [
		"vuetify",
	],
};
