module.exports = {
    root: true,

    env: {
		node: true,
		browser: true,
	},

    extends: [
		"plugin:vue/essential",
		"@vue/airbnb",
	],

    rules: {
      indent: [
        2,
        'tab'
      ],
      quotes: [
        2,
        'double'
      ],
      'linebreak-style': [
        0
      ],
      'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
      'spaced-comment': [
        0
      ],
      'max-len': [
        0
      ],
      'no-new': [
        0
      ],
      'no-tabs': [
        0
      ],
      'object-shorthand': [
        0
      ],
      'func-names': [
        0
      ],
      'no-param-reassign': [
        0
      ],
      'import/no-extraneous-dependencies': [
        0
      ],
      'function-paren-newline': [
        0
      ],
      'no-underscore-dangle': [
        0
      ],
      'object-curly-newline': [
        0
      ],
      'prefer-destructuring': [
        0
      ],
      'operator-linebreak': [
        0
      ],
      'implicit-arrow-linebreak': [
        0
      ],
      'no-bitwise': [
        0
      ],
      'no-mixed-operators': [
        0
      ],
      'no-loop-func': [
        0
      ],
      'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off'
    },

    parserOptions: {
		parser: 'babel-eslint',
	},

    globals: {
	},

};
