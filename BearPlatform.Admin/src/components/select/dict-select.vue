<script lang="ts" setup>
import { useRequest } from '@sa/alova/client';
defineOptions({
  name: 'DetailDetailSelect',
  inheritAttrs: false
});
interface Props {
  dictId: string;
}

const props = defineProps<Props>();
const { loading, data } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DetailDetail.get_select({
      params: {
        dictId: props.dictId
      },
      transform: res => {
        return res.data?.map(item => {
          return {
            label: item.label || '',
            value: item.value
          };
        });
      }
    }),
  {
    force: true,
    immediate: true,
    async middleware(_, next) {
      if (props.dictId) {
        await next();
      }
    }
  }
);
const value = defineModel<string>('value', {
  required: true
});
</script>

<template>
  <ElSelect v-model="value" v-bind="$attrs" :placeholder="$t('common.placeholder')" clearable :loading="loading">
    <ElOption v-for="item in data" :key="item.value" :label="item.label" :value="item.value" />
  </ElSelect>
</template>
