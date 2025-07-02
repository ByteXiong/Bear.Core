// / <reference types="vite/client" />

declare module '*.vue' {
  import type { DefineComponent } from 'vue';

  const component: DefineComponent<NonNullable<unknown>, NonNullable<unknown>, any>;
  export default component;
}

interface ImportMetaEnv {
  readonly VITE_ENV: string
  readonly VITE_APP_TITLE: string
  readonly VITE_BASE_URL: string
  readonly VITE_UPLOAD_URL: string
  readonly VITE_APP_CACHE_PREFIX: string
  readonly VITE_PORT: number
  readonly VITE_USE_MOCK: string | boolean
  readonly VITE_PROXY_PREFIX: string
  readonly VITE_UPLOAD_PROXY_PREFIX: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
