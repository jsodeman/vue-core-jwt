// general
import Denied from "@/views/denied.vue";

// account
import Register from "@/views/account-register.vue";
import RegisterConfirm from "@/views/account-confirm.vue";
import RegisterComplete from "@/views/account-register-complete.vue";
import Login from "@/views/account-login.vue";
import PasswordReset from "@/views/account-pwreset.vue";
import PasswordResetComplete from "@/views/account-pwreset-complete.vue";

// sample pages
import Home from "@/views/home.vue";
import Restricted from "@/views/restricted.vue";
import Admin from "@/views/admin.vue";

// the access property defined which roles are allowed to access a route
// * = anyone/public, or the access property can just be left off
// multiple allowed roles per route can be provided in the array
export default [
	// root
	{
		path: "/",
		redirect: { name: "home" },
	},
	{
		path: "/home",
		component: Home,
		meta: { title: "Home - Public", access: ["*"] },
		name: "home",
	},
	{
		path: "/denied",
		component: Denied,
		meta: { title: "Access Denied", access: ["*"] },
		name: "denied",
	},
	{
		path: "/register",
		component: Register,
		meta: { title: "Register", access: ["*"] },
		name: "account-register",
	},
	{
		path: "/confirm/:id",
		component: RegisterConfirm,
		meta: { title: "Account Confirmation", access: ["*"] },
		name: "account-confirm",
	},
	{
		path: "/register-complete",
		component: RegisterComplete,
		meta: { title: "Registration Complete", access: ["*"] },
		name: "account-register-complete",
	},
	{
		path: "/login",
		component: Login,
		meta: { title: "Login", access: ["*"] },
		name: "account-login",
	},
	{
		path: "/reset",
		component: PasswordReset,
		meta: { title: "Password Reset", access: ["*"] },
		name: "pw-reset",
	},
	{
		path: "/reset-complete/:id",
		component: PasswordResetComplete,
		meta: { title: "Password Reset", access: ["*"] },
		name: "pw-reset-complete",
	},
	{
		path: "/admin",
		component: Admin,
		meta: { title: "Admin - Admin only", access: ["Admin"] },
		name: "admin",
	},
	{
		path: "/restricted",
		component: Restricted,
		meta: { title: "Restricted - Logged In users only", access: ["Admin", "Normal"] },
		name: "restricted",
	},
];
