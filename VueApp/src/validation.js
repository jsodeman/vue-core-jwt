import { extend } from "vee-validate";
// eslint-disable-next-line camelcase
import { required, email, min_value, min } from "vee-validate/dist/rules";

extend("required", {
	...required,
	message: "Required",
});

extend("min_value", {
	// eslint-disable-next-line camelcase
	...min_value,
	message: "At least one",
});

extend("min", {
	...min,
	message: "Too short",
});

extend("url", {
	validate: (str) => {
		const pattern = new RegExp("^(https?:\\/\\/)?" + // protocol
			"((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|" + // domain name
			"((\\d{1,3}\\.){3}\\d{1,3}))" + // OR ip (v4) address
			"(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*" + // port and path
			"(\\?[;&a-z\\d%_.~+=-]*)?" + // query string
			"(\\#[-a-z\\d_]*)?$", "i"); // fragment locator
		return !!pattern.test(str);
	},
	message: "This is not a valid URL",
});

extend("email", {
	...email,
	message: "Invalid email address",
});
