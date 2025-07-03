<script setup lang="tsx">
import { IconTypeEnum, type MenuTypeEnum } from '@/api/apiEnums';
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
    <ElOption v-for="(row, index) in options" :key="index" :value="row[valKey] || ''" :label="index">
      <div class="flex items-center">
        <SvgIcon
          :icon="row.iconType === IconTypeEnum.iconify ? row.icon || '' : undefined"
          :local-icon="row.iconType === IconTypeEnum.local ? row.icon || '' : undefined"
          class="text-icon"
        />

        <span>{{ row.title }}</span>
      </div>
    </ElOption>

    <template #label="{ label }">
      <div v-if="label" class="flex items-center">
        <SvgIcon
          :icon="options[label]?.iconType === IconTypeEnum.iconify ? options[label]?.icon || '' : undefined"
          :local-icon="options[label]?.iconType === IconTypeEnum.local ? options[label]?.icon || '' : undefined"
          class="text-icon"
        />
        <span>{{ options[label]?.title }}</span>
      </div>
    </template>
  </ElSelect>
</template>

<style scoped></style>
