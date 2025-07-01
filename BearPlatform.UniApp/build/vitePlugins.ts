import type { PluginOption } from 'vite';
import uni from '@dcloudio/vite-plugin-uni';
import Components from '@uni-helper/vite-plugin-uni-components';
import { WotResolver } from '@uni-helper/vite-plugin-uni-components/resolvers';
import UniKuRoot from '@uni-ku/root';
import { visualizer } from 'rollup-plugin-visualizer';
import Unocss from 'unocss/vite';
import autoImport from 'unplugin-auto-import/vite';
import viteRestart from 'vite-plugin-restart';
import { isH5 } from './platform';

interface VitePluginConfig {
  isProd: boolean
}

export function createVitePlugins({ isProd }: VitePluginConfig): PluginOption[] {
  return [
    Components({
      resolvers: [WotResolver()],
    }),
    UniKuRoot(),
    // @ts-expect-error TODO uni() 会报错：uni is not a function,暂时使用此方式解决
    uni?.default(),
    Unocss({
      hmrTopLevelAwait: false,
    }),
    autoImport({
      include: [
        /\.[tj]sx?$/, // .ts, .tsx, .js, .jsx
        /\.vue$/,
        /\.vue\?vue/, // .vue
      ],
      imports: [
        'vue',
        'uni-app',
        'pinia',
        {
          'uni-mini-router': ['useRouter', 'useRoute'],
        },
        {
          'alova/client': ['useRequest'],
        },
      ],
      dts: 'typings/auto-imports.d.ts',
      eslintrc: {
        enabled: true,
      },
    }),
    viteRestart({
      restart: ['vite.config.ts', 'src/pages.json'],
    }),
    isH5 && isProd
    && visualizer({
      filename: './node_modules/.cache/visualizer/stats.html',
      open: true,
      gzipSize: true,
      brotliSize: true,
    }),
  ];
}
