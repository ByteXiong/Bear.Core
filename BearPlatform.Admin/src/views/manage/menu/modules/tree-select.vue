<script setup lang="tsx">
import type { MenuTypeEnum } from '@/api/apiEnums';
import type { RouteTreeSelectDTO } from '@/api/globals';
import { useRequest } from '~/packages/alova/src/client';
defineOptions({
  name: 'MenuTree',
  inheritAttrs: false
});

interface Props {
  types: MenuTypeEnum[];
  valKey: keyof RouteTreeSelectDTO;
}

const { types, valKey } = defineProps<Props>();

const { data: options } = useRequest(
  () =>
    Apis.Menu.post_treeselect({
      data: types,
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
</script>

<template>
  <ElSelect v-bind="$attrs">
    <ElOption v-for="(row, index) in options" :key="index" :value="row[valKey] || ''" :label="row.title || ''" />
  </ElSelect>
</template>

<style scoped></style>
