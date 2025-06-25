<script lang="ts" setup>
import { useRequest } from '@sa/alova/client';
defineOptions({
  name: 'AllDictSelect',
  inheritAttrs: false
});
const value = defineModel<string>('value', {});
const { loading, data } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.Dict.get_select({
      transform: res => {
        return res.data?.map(item => {
          return {
            label: `${item.description || item.label || ''}(${item.type})`,
            value: item.value
          };
        });
      }
    }),
  {
    force: true,
    immediate: true
  }
);
</script>

<template>
  <ElSelect v-model="value" v-bind="$attrs" :placeholder="$t('common.placeholder')" :loading="loading">
    <ElOption v-for="item in data" :key="item.value" :label="item.label" :value="item.value" />
  </ElSelect>
</template>
