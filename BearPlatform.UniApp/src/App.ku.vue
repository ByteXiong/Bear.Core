<!-- src/App.ku.vue | App.ku.vue -->

<script setup lang="ts">
import { useTheme } from './composables/useTheme';
import { getCache } from './utils/cache';

const { theme, themeVars } = useTheme({
//   buttonPrimaryBgColor: '#07c160',
//   buttonPrimaryColor: '#07c160',
});
theme.value = getCache('theme');
</script>

<template>
  <!-- <div>Hello AppKuVue</div> -->
  <!-- 需要确保已注册 WdConfigProvider 组件 -->
  <wd-config-provider :theme="theme" :theme-vars="themeVars">
    <KuRootView />
  </wd-config-provider>
</template>
