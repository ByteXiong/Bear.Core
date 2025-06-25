<script lang="ts" setup>
import { OrderTypeEnum } from '@/api/apiEnums';
import { TableColumn } from '@/api/globals';
import { $t } from '@/locales';
import { getEnumValue } from '@/utils/common';
import { CascaderValue } from 'element-plus';
import { computed, nextTick, ref, watch } from 'vue';
defineOptions({
  name: 'SortCascader',
  inheritAttrs: false,
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


const value =ref<Array<Array<string>>| []>([]);

const  options = computed(()=>{
  return props.columns?.filter(x=>x.prop).map((item, index) => {
    return {
      value: item.prop||"",
      label: `${$t(item.label||"")}( ${item.prop||""})` ,
      children: getEnumValue(OrderTypeEnum).map((item) => {
        return {
          value: item,
          label:   $t( "enum." + OrderTypeEnum[item]) ,
        };
      }) ,
    };
  });
})

watch(
  () => props.modelValue,
  () => {
      value.value =Object.keys(props.modelValue||{}).map((item: string) => [item, props.modelValue[item] as unknown as string]);
  },
  { immediate: true }
)
const handleChange = (tags:any) => {
  //分组
   value.value = tags.reduce(function (
    prev: Array<Array<string>>,
    curr: Array<string>,
    i: number,
    arr: Array<Array<string>>
  ) {
     let index = prev.findIndex((item: any) =>item[0] == curr[0])
     if( index != -1){
        prev[index]=curr
     }else{
        prev.push(curr);
     }
    return prev;
  }, []);

emits('update:modelValue', value.value.reduce((acc, item) => ({ ...acc, [item[0]]: item[1] as unknown as OrderTypeEnum }), {}));
}
</script>

<template>
  <el-cascader v-model="value" :options="options"  v-bind="$attrs" :props="{multiple: true }"  @change="handleChange" />
</template>
