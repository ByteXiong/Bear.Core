<script setup lang="tsx">
import { ref, useTemplateRef } from 'vue';
import DynamicList from '@/views/table/dynamic-list.vue';
import EditForm from '@/views/table/edit-form.vue';
import type { TableColumn, TableView } from '@/api/globals';
import { useRequest } from "@sa/alova/client";
import { el } from 'element-plus/es/locale';
import { getEnumValue } from '@/utils/common';
import { VersionEnum } from '@/api/apiEnums';
import { Arg } from '~/packages/alova/src';


const { loading, send: getRefresh } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  ( version) =>
    Apis.Apis.get_refresh({
      params: {
        version
      },
      transform: (res) => {
         window.$message?.success('操作成功！');
         console.log( listRef.value);

         listRef.value?.reload();
      },
    }),
  {
    force: true,
    immediate: false,
  },
);

const searchData = ref<TableColumn[]>([]);
const listRef = useTemplateRef("listRef");
const loadChange = (table: TableView) => {
  searchData.value = table?.columns?.filter(item => item.searchType !== null) || [];
};
const searchParams = ref<UI.SearchParams>({});
const param = ref<Arg>({});
// 打开编辑/新增
const editFormRef = ref();
const openForm = (id: string | null) => {
  editFormRef.value?.openForm(id);
};

const refresh = () => {
  listRef.value?.reload();
};
const activeName = ref(VersionEnum.pc);
const tabschange = () => {
   param.value.version = activeName.value;
   listRef.value?.reload();
}
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">

    <TableHeaderSearch :search-data="searchData" @search-change="e => (searchParams = e)">
    </TableHeaderSearch>
    <el-tabs v-model="activeName" class="demo-tabs" @change="tabschange">
      <el-tab-pane v-for="item in getEnumValue(VersionEnum)" :label="VersionEnum[item]" :name="item" >


      </el-tab-pane>

    </el-tabs>
      <DynamicList
      ref="listRef"
      tableof="ApisDTO"
      router=""
      config-id=""
      page-url="/api/Apis/GetPage"
      del-url="/api/Apis/Delete"
      :search-params="searchParams"
      :param="param"
      @load-change="loadChange"
      @open-form="openForm"
    >
         <template #prefix>
             <el-button type="primary" @click="getRefresh( activeName)">刷新Apis-{{ VersionEnum[activeName] }}</el-button>
         </template>
  </DynamicList>

<!--   add-url="/api/Apis/Add" -->

  </div>
  <!-- 新增/编辑 -->
</template>
