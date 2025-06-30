<script setup lang="tsx">
import { computed, h, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { $t } from '@/locales';
import { RolePermissionParam } from '@/api/globals';
import MenuTree from './menu-tree.vue';
import ApisTree from './apis-tree.vue';
import {VersionEnum} from "@/api/apiEnums";
import {getEnumValue} from "@/utils/common";
import { TabsPaneContext } from 'element-plus';
defineOptions({
  name: 'PermissionRef'
});
const visible = ref<boolean>(false);

type FormType = RolePermissionParam;

interface Emits {
  (e: 'refresh'): void;
}
const emit = defineEmits<Emits>();



const title = computed(() => {
  return '角色权限';
});


/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
{
  return Apis.Role.post_setpermission({
      data: form,
      transform: () => {
        visible.value = false;
        resetFrom();
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh');
      }
    })
  },
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
    } as FormType
  }
);
/** 获取数据getbyroleid */
const { send: getData, data: treeIds } = useRequest(
  roleId =>
    Apis.Role.get_getpermission({
      params: { roleId: roleId },
      transform: res => {
        updateForm({...res.data});
        return res.data;
      }
    }),
  {
    force: true,
    immediate: false
  }
);


const menuTreeRef=ref();
const apisPcRef=ref();
const apisAppRef=ref();
const apisDefRef=ref();
// 打开
const openForm = async (id?: string|null) => {
  resetFrom();

  visible.value = true;
  await getData(id);
};

const closeForm = () => {
  menuTreeRef.value.close()
  apisPcRef.value.close()
  apisAppRef.value.close()
  apisDefRef.value.close()
  resetFrom();
};

defineExpose({
  openForm
});

const activeName = ref("menu");
const handleClick = (pane: TabsPaneContext, ev: Event) => {
//  console.log(pane.paneName,ev);



}


</script>

<template>
  <ElDialog v-model="visible" :title="title" preset="card" class="w-800px" @close="closeForm">

    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
      <el-tab-pane label="菜单-电脑端" name="menu">
        <MenuTree  ref="menuTreeRef" v-model="formData.menuIds" ></MenuTree>
      </el-tab-pane>

      <el-tab-pane label="Apis-电脑端" :name="1" >
        <ApisTree   ref="apisPcRef"  v-model="formData.pc "   :version="1"></ApisTree>
      </el-tab-pane>
      <el-tab-pane label="Apis-小程序" :name="2" >
        <ApisTree     ref="apisAppRef" v-model="formData.app "   :version="2"></ApisTree>
      </el-tab-pane>
      <el-tab-pane label="Apis-第三方" :name="0" >
        <ApisTree    ref="apisDefRef"  v-model="formData.def "   :version="0"></ApisTree>
      </el-tab-pane>




    </el-tabs>

    <template #footer>
      <ElSpace :size="16" class="float-right">
        <ElButton @click="closeForm">{{ $t('common.cancel') }}</ElButton>
        <ElButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</ElButton>
      </ElSpace>
    </template>
  </ElDialog>
</template>

<style scoped></style>
