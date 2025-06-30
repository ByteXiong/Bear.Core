<script lang="ts" setup>
import { computed } from 'vue';
import * as Enums from '@/api/apiEnums';
defineOptions({
  name: 'AllEnumSelect',
  inheritAttrs: false
});
const value = defineModel<string>('value', {});

const data = computed(() => {
  const enumNames = Object.keys(Enums);
  return enumNames.map(item => ({ label: item, value: item }));
});

// interface Emits {
//   (e: 'onChange'): void;
// }
</script>

<template>
  <ElSelect v-model="value" v-bind="$attrs" :placeholder="$t('common.placeholder')" clearable>
    <ElOption v-for="item in data" :key="item.value" :label="item.label" :value="item.value" />
  </ElSelect>
</template>
