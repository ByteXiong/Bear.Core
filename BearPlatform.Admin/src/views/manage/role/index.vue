<script setup lang="tsx">
import { createApp, ref } from 'vue';
import type { TableColumnCtx } from 'element-plus';
import { ElButton, ElPopconfirm } from 'element-plus';
import type { RoleDTO, TableColumn, TableView } from '@/api/globals';
import createComponent from '@/utils/createComponent';
import DynamicList from '@/views/table/dynamic-list.vue';
import { $t } from '@/locales';
import EditForm from '@/views/table/edit-form.vue';
import Permission from './modules/permission.vue';
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
// 删除
const handleDelete = (ids: string[]) => {
  listRef.value?.handleDelete(ids);
};

const reload = () => {
  listRef.value?.reload();
};

type DateType = RoleDTO;
const endColumn = ref<UI.TableColumnCheck[] | (Partial<TableColumnCtx<DateType>> & { checked?: boolean })[]>([
  {
    prop: 'use',
    label: '操作',
    align: 'center',
    checked: true,
    width: 200,
    formatter: row => (
      <div class="flex-center pr-10px">
        <ElButton type="primary" plain size="small" onClick={() => openTreeForm(row.id)}>
          权限
        </ElButton>

        <ElButton type="primary" plain size="small" onClick={() => openForm(row.id)}>
          编辑
        </ElButton>
        <ElPopconfirm title={$t('common.confirmDelete')} onConfirm={() => handleDelete([row.id])}>
          {{
            reference: () => (
              <ElButton type="danger" plain size="small">
                {$t('common.delete')}
              </ElButton>
            )
          }}
        </ElPopconfirm>
      </div>
    )
  }
]);

// 打开编辑/新增
const permissionRef = ref();
const openTreeForm = async (id: string | null) => {
  //   const container = document.createElement('div')
  //     document.body.appendChild(container)

  //  const app = createApp({
  //       // 组件选项
  //       template: `<Permission ref="permissionRef" />`,
  //       components: { Permission }
  //     })

  //  const root = app.mount(container)
  //  const instance = (root.$refs.permissionRef as typeof Permission)
  //  console.log(app,container,instance);
  // const {instance,on,unmount } =await  createComponent(Permission)

  //       instance.openForm(id);
  permissionRef.value?.openForm(id);
};
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch :search-data="searchData" @search-change="e => (searchParams = e)"></TableHeaderSearch>
    <DynamicList
      ref="listRef"
      tableof="RoleDTO"
      router=""
      config-id=""
      page-url="/api/Role/GetPage"
      add-url="/api/Role/Add"
      del-url="/api/Role/Delete"
      :search-params="searchParams"
      :end-column="endColumn"
      @load-change="loadChange"
      @open-form="openForm"
    ></DynamicList>
    <!-- 新增/编辑 -->

    <EditForm
      ref="editFormRef"
      config-id=""
      router=""
      tableof="UpdateRoleParam"
      info-url="/api/Role/GetInfo"
      submit-url="/api/Role/Update"
      @reload="reload"
    ></EditForm>
    <Permission ref="permissionRef"></Permission>
  </div>
</template>
