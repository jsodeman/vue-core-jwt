<template>
	<v-alert type="error" class="mb-6" v-if="hasErrors">
		{{ errorSummary }}
	</v-alert>
</template>

<script>
import _ from "lodash";

export default {
	name: "error-summary",
	components: {},
	props: {
		errors: {
			type: Object,
		},
	},
	data() {
		return {};
	},
	computed: {
		hasErrors() {
			return this.errorList.length > 0;
		},
		errorSummary() {
			return _.join(this.errorList, ", ");
		},
		errorList() {
			return Object.entries(this.errors).filter((i) => i[1].length > 0).map((i) => i[0]);
		},
	},
	watch: {},
	created() {
	},
	mounted() {
	},
	methods: {},
};
</script>
