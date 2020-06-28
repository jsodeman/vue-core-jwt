import axios from "axios";

const basePath = process.env.NODE_ENV === "development" ? "https://localhost:5001/api/" : "/api/";
axios.defaults.baseURL = basePath;

const requestInterceptor = (request) => {
	request.withCredentials = true;
	return request;
};

axios.interceptors.request.use((request) => requestInterceptor(request));

const jwtCheck = () => axios.get("jwtcheck");
const appSettings = () => axios.get("vuestoredata");
const login = (r) => axios.post("login", r);
const logout = () => axios.delete("login");
const log = (error) => axios.post("clientlog", { error: JSON.stringify(error) });
const passwordReset = (r) => axios.post("passwordreset", r);
const passwordResetComplete = (r) => axios.post("passwordreset/complete", r);
const register = (r) => axios.post("register", r);
const confirm = (r) => axios.post("register/confirm", r);
const user = (id) => axios.get(`users/${id}`);

export default {
	appSettings,
	basePath,
	confirm,
	jwtCheck,
	log,
	login,
	logout,
	passwordReset,
	passwordResetComplete,
	register,
	user,
};
