<script setup lang="tsx">
import { computed, h, ref, watch } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { $t } from '@/locales';
import { MenuType } from '@/api/apiEnums';
import { RouteTreeSelectDTO } from '@/api/globals';
defineOptions({
  name: 'MenuTree'
});


interface Props {
  modelValue: Array<string>|null;
}
const { modelValue } = defineProps<Props>();

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
    Apis.Menu.get_treeselect({
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




const checkChange=(data: RouteTreeSelectDTO)=> {
  const node = treeRef.value.getNode(data.id);
  setNode(node);
  emit('update:modelValue', treeRef.value.getCheckedKeys());
}

const setNode=(node: any)=>{
  if (node.checked) {
    //如果当前是选中checkbox,则递归设置父节点和父父节点++选中
    setParentNode(node);
  } else {
    //当前是取消选中,将所有子节点都取消选中
    setChildenNode(node);
  }
}

const setParentNode=(node: any)=> {
  if (node.parent) {
    for (const key in node) {
      if (key === "parent") {
        node[key].checked = true;
        setParentNode(node[key]);
      }
    }
  }
}
const setChildenNode=(node: any)=>  {
  let len = node.childNodes.length;
  for (let i = 0; i < len; i++) {
    node.childNodes[i].checked = false;
    setChildenNode(node.childNodes[i]);
  }
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
        :props="{ label: 'title' }"
        @check="checkChange"
        check-strictly
      >
        <template #default="{ data }">
          {{ data.title }}
        </template>
      </el-tree>
</template>

<style scoped></style>
