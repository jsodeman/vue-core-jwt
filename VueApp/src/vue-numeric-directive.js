/* eslint-disable */
//+ Jonas Raoni Soares Silva
//@ http://raoni.org

export default class NumericDirective {
	constructor(input, binding) {
		Object.assign(this, { input, binding });
		input.addEventListener("keydown", this);
		input.addEventListener("change", this);
	}

	static install(Vue) {
		Vue.directive("decimal", this.directive);
		Vue.directive("integer", this.directive);
	}

  static directive = {
  	bind(el, binding) {
  		el = el.querySelector("input");
  		if (el) {
  			return new NumericDirective(el, binding);
  		}
  	},
  }

  handleEvent(event) {
  	this[event.type](event);
  }

  keydown(event) {
  	const { target, key, keyCode, ctrlKey } = event;
  	if (!(
  		(key >= "0" && key <= "9") ||
    (
    	((key === "." && this.binding.name === "decimal") || (key === "-" && !this.binding.modifiers.unsigned)) &&
      !~target.value.indexOf(key)
    ) ||
    [
    	"Delete", "Backspace", "Tab", "Esc", "Escape", "Enter",
    	"Home", "End", "PageUp", "PageDown", "Del", "Delete",
    	"Left", "ArrowLeft", "Right", "ArrowRight", "Insert",
    	"Up", "ArrowUp", "Down", "ArrowDown",
    ].includes(key) ||
    // ctrl+a, c, x, v
    (ctrlKey && [65, 67, 86, 88].includes(keyCode))
  	)) {
  		event.preventDefault();
  	}
  }

  change({ target }) {
  	const isDecimal = this.binding.name === "decimal";
  	let value = target.value;
  	if (!value) {
  		return;
  	}
  	const isNegative = /^\s*-/.test(value) && !this.binding.modifiers.unsigned;
  	value = value.replace(isDecimal ? /[^\d,.]/g : /\D/g, "");
  	if (isDecimal) {
  		const pieces = value.split(/[,.]/);
  		const decimal = pieces.pop().replace(/0+$/, "");
  		if (pieces.length) {
  			value = `${pieces.join("") || (decimal ? "0" : "")}${decimal ? `.${decimal}` : ""}`;
  		}
  	}
  	value = value.replace(/^(?:0(?!\b))+/, "");
  	if (value && isNegative) {
  		value = `-${value}`;
  	}
  	if (target.value !== value) {
  		target.value = value;
  		const event = document.createEvent("UIEvent");
  		event.initEvent("input", true, false, window, 0);
  		target.dispatchEvent(event);
  	}
  }
}
