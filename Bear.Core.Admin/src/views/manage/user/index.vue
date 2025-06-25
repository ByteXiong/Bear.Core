<script setup lang="tsx">
import { ref } from 'vue';
import DynamicList from '@/views/table/dynamic-list.vue';
import EditForm from '@/views/table/edit-form.vue';
import type { TableColumn, TableView } from '@/api/globals';

const searchData = ref<TableColumn[]>([]);
const listRef = ref();
const loadChange = (table: TableView) => {
  searchData.value = table?.columns?.filter(item => item.searchType !== null) || [];
};
const searchParams = ref<UI.SearchParams>({});

// 打开编辑/新增
const editFormRef = ref();
const openForm = (id: string | null) => {
  editFormRef.value?.openForm(id);
};

const refresh = () => {
  listRef.value?.getData();
};
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch :search-data="searchData" @search-change="e => (searchParams = e)"></TableHeaderSearch>
    <DynamicList
      ref="listRef"
      tableof="UserDTO"
      router=""
      config-id=""
      page-url="/api/User/GetPage"
      add-url="/api/User/Add"
      del-url="/api/User/Delete"
      :search-params="searchParams"
      @load-change="loadChange"
      @open-form="openForm"
    ></DynamicList>
  </div>
  <!-- 新增/编辑 -->
</template>
