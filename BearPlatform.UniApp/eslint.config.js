/** @type {import('eslint').Linter.Config} */

import process from 'node:process';
import antfu from '@antfu/eslint-config';

/**
 * @see https://github.com/antfu/eslint-config
 */

export default antfu(
  {
    formatters: true,
    vue: true,
    jsx: true,
    env: {
      node: true,
    },
    ignores: [
      './dist/*',
      './.vscode/*',
      './.idea/*',
      '**/androidPrivacy.json',
      'README.md',
    ],
  },
  {
    rules: {
      'style/indent': ['error', 2, { SwitchCase: 2 }],
      'style/quotes': ['error', 'single'],
      'style/semi': ['error', 'always'],
      'style/semi-style': ['error', 'last'],
      'style/max-len': ['error', {
        code: 160,
        tabWidth: 2,
        ignoreUrls: true,
        ignoreComments: true,
        ignoreStrings: true,
        ignoreTemplateLiterals: true,
        ignoreRegExpLiterals: true,
      }],
      'style/brace-style': ['error', '1tbs', { allowSingleLine: true }],
      'vue/script-indent': ['error', 2, { baseIndent: 0 }],
      'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
      'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
      'no-useless-catch': 'off',
    },
  },
);
