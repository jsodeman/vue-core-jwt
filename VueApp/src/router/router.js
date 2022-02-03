import VueRouter from "vue-router";
import routes from "./routes";

const router = new VueRouter({
	linkActiveClass: "active",
	mode: "history",
	routes,
	base: process.env.BASE_URL,
});

export default router;
