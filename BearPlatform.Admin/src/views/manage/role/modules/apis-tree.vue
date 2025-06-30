<script setup lang="tsx">
import { computed, h, ref, watch } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { $t } from '@/locales';
import { MenuType, VersionEnum } from '@/api/apiEnums';
import { ApisTreeSelectDTO } from '@/api/globals';
defineOptions({
  name: 'ApisTree'
});


interface Props {
  version :VersionEnum |number
  modelValue: Array<string> | null | undefined;

}
const { modelValue , version } = defineProps<Props>();

const emit = defineEmits(['update:modelValue']);

watch(

  () => modelValue,
  newValue => {
    newValue?.forEach((id: string) =>
    treeRef.value?.setChecked(id, true, false)
  );
  },
  { deep: true }
);


/** 获取数据Tree */
const { data } = useRequest(
  () =>
    Apis.Apis.get_treeselect({
      params: { version:version as VersionEnum },
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
const treeRef=ref();


const checkChange=(data: ApisTreeSelectDTO)=> {
 const selectIds= treeRef.value.getCheckedNodes().filter((x: ApisTreeSelectDTO)=>x.children==null).map((x: ApisTreeSelectDTO)=>x.id)
  // setNode(node);
  emit('update:modelValue',selectIds);
}

const close = () => {
  emit('update:modelValue', []);
  treeRef.value.setCheckedKeys([]);
};
defineExpose({
  close
});
</script>

<template>

    <el-tree
        ref="treeRef"
        node-key="id"
        show-checkbox
        :data="data"
        :default-expand-all="true"
        @check="checkChange"
      >
        <template #default="{ data }">
          {{ data.label }}
        </template>
      </el-tree>
</template>

<style scoped></style>
