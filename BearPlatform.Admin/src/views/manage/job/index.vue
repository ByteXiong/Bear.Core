<script setup lang="tsx">
import { ref, useTemplateRef } from 'vue';
import type { TableColumn, TableView } from '@/api/globals';
import DynamicList from '@/views/table/dynamic-list.vue';
import EditForm from '@/views/table/edit-form.vue';

const searchData = ref<TableColumn[]>([]);
const listRef = useTemplateRef('listRef');
const loadChange = (table: TableView) => {
  searchData.value = table?.columns?.filter(item => item.searchType !== null) || [];
};
const searchParams = ref<UI.SearchParams>({});

// 打开编辑/新增
const editFormRef = useTemplateRef('editFormRef');
const openForm = (id: string | null) => {
  editFormRef.value?.openForm(id);
};
// // 删除
// const handleDelete = (ids: string[]) => {
//   listRef.value?.handleDelete(ids);
// };
// 刷新
const refresh = () => {
  listRef.value?.reload();
};
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <DynamicList
      ref="listRef"
      tableof="JobDTO"
      router=""
      config-id=""
      page-url="/api/Job/GetPage"
      add-url="/api/Job/Add"
      del-url="/api/Job/Delete"
      :search-params="searchParams"
      @load-change="loadChange"
      @open-form="openForm"
    ></DynamicList>
    <!-- 新增/编辑 -->

    <EditForm
      ref="editFormRef"
      config-id=""
      router=""
      tableof="UpdateTenantParam"
      info-url="/api/Tenant/GetInfo"
      submit-url="/api/Tenant/Update"
      @refresh="refresh"
    ></EditForm>
  </div>
</template>
