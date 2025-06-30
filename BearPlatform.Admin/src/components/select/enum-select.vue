<script lang="ts" setup>
import { computed } from 'vue';
import * as Enums from '@/api/apiEnums';
import { getEnumValue } from '@/utils/common';
defineOptions({
  name: 'EnumSelect',
  inheritAttrs: false
});
interface Props {
  groupBy: string;
}

const props = defineProps<Props>();

const data = computed(() => {
  return props.groupBy
    ? getEnumValue(Enums[props.groupBy as keyof typeof Enums]).map(item => ({
        label: Enums[props.groupBy as keyof typeof Enums][item],
        value: item
      }))
    : [];
});

const value = defineModel<string>('value', {
  required: true
});
</script>

<template>
  <ElSelect v-model="value" v-bind="$attrs" placeholder="请选择" clearable>
    <ElOption v-for="item in data" :key="item.value" :label="item.label" :value="item.value" />
  </ElSelect>
</template>
