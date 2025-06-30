<script lang="ts" setup>
import { computed, ref, watch } from 'vue';
import { OrderTypeEnum } from '@/api/apiEnums';
import type { TableColumn } from '@/api/globals';
import { getEnumValue } from '@/utils/common';
import { $t } from '@/locales';
defineOptions({
  name: 'SortCascader',
  inheritAttrs: false
});

interface Props {
  columns: TableColumn[] | null;
  modelValue: Record<string, OrderTypeEnum>;
}
const props = defineProps<Props>();
interface Emits {
  (e: 'update:modelValue', value: Record<string, OrderTypeEnum>): void;
}

const emits = defineEmits<Emits>();

const value = ref<Array<Array<string>> | []>([]);

const options = computed(() => {
  return props.columns
    ?.filter(x => x.prop)
    .map(item => {
      return {
        value: item.prop || '',
        label: `${$t(item.label || '')}( ${item.prop || ''})`,
        children: getEnumValue(OrderTypeEnum).map(e => {
          return {
            value: e,
            label: $t(`enum.${OrderTypeEnum[e]}`)
          };
        })
      };
    });
});

watch(
  () => props.modelValue,
  () => {
    value.value = Object.keys(props.modelValue || {}).map((item: string) => [
      item,
      props.modelValue[item] as unknown as string
    ]);
  },
  { immediate: true }
);
const handleChange = (tags: any) => {
  // 分组
  value.value = tags.reduce((prev: Array<Array<string>>, curr: Array<string>) => {
    const index = prev.findIndex((item: any) => item[0] === curr[0]);
    if (index !== -1) {
      prev[index] = curr;
    } else {
      prev.push(curr);
    }
    return prev;
  }, []);

  emits(
    'update:modelValue',
    value.value.reduce((acc, item) => ({ ...acc, [item[0]]: item[1] as unknown as OrderTypeEnum }), {})
  );
};
</script>

<template>
  <ElCascader v-model="value" :options="options" v-bind="$attrs" :props="{ multiple: true }" @change="handleChange" />
</template>
