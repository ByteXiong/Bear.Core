import type { App } from 'vue';
import { createI18n } from 'vue-i18n';
import { alovaInstance } from '@/api';
import { localStg } from '@/utils/storage';
import messages from './locale';

const i18n = createI18n({
  locale: localStg.get('lang') || 'zh-CN',
  fallbackLocale: 'en',
  messages,
  legacy: false
});
// 混合后端国际化
const loadI18n = () => {
  alovaInstance.Get('/api/I18n/GetByLocale', { params: { locale: i18n.global.locale.value } }).then(({ data }: any) => {
    i18n.global.mergeLocaleMessage(i18n.global.locale.value, data);
  });
  // debugger;
  // Apis.I18n.get_getbylocale({
  //   params: {
  //     locale: i18n.global.locale.value
  //   },
  //   transform: ({ data }) => {
  //     debugger;
  //     i18n.global.mergeLocaleMessage(i18n.global.locale.value, data);
  //   }
  // });
};

/**
 * Setup plugin i18n
 *
 * @param app
 */
export function setupI18n(app: App) {
  app.use(i18n);
  loadI18n();
}

export const $t = i18n.global.t as App.I18n.$T;

export function setLocale(locale: App.I18n.LangType) {
  i18n.global.locale.value = locale;
  loadI18n();
}
