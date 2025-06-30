<script setup lang="tsx">
import { computed, ref, useTemplateRef } from 'vue';
import { useRoute } from 'vue-router';
import type { Sort, TableColumnCtx } from 'element-plus';
import { usePagination, useRequest } from 'alova/client';
import { alovaInstance } from '@/api';
import type { TableColumn, TableView } from '@/api/globals';
import type { OrderTypeEnum } from '@/api/apiEnums';
import { ConditionalType } from '@/api/sqlSugar';
import downloadExcel from '@/utils/downloadExcel';
import customRender from '@/utils/customRender';
import { $t } from '@/locales';
import CustomHeader from './customHeader.vue';
import type { Arg } from '~/packages/alova/src';
interface Props {
  tableof: string;
  router: string;
  configId: string;
  pageUrl: string;
  addUrl?: string;
  delUrl: string;
  param?: Arg;
}
// eslint-disable-next-line @typescript-eslint/no-unused-vars
const { tableof, router, configId, pageUrl, addUrl, delUrl, param } = defineProps<Props>();

interface Emits {
  (e: 'loadChange', tableView: TableView): void;
  (e: 'openForm', formId: string | null): void;
}
const tableRef = useTemplateRef('tableRef');
const emit = defineEmits<Emits>();
const tableView = ref<TableView>();
const columns = ref<Array<UI.TableColumnCheck>>([]);
const dataTableConfig = ref<UI.dataTableConfig>({
  sortList: {}
});
const loadChange = (table: TableView) => {
  tableView.value = table;
  columns.value =
    table?.columns
      ?.filter(item => item.isShow)
      .map(item => {
        return { ...item, checked: true, attrs: item.attrs } as unknown as UI.TableColumnCheck;
      }) || [];

  if (dataTableConfig.value) dataTableConfig.value.sortList = table.sorts || {};
  // searchData.value = res.data?.tableColumns?.filter(item => item.searchType !== null) || [];
  emit('loadChange', table);
};

const columnData = computed<Array<Partial<TableColumnCtx<TableColumn>> & TableColumn & { checked?: boolean }>>(() => {
  const arr = columns.value
    ?.filter(item => item.checked)
    .map(item => {
      const column = customRender(item.attrs);
      return {
        // label: $t(column.label),
        ...item,
        ...column,
        // renderHeader:(header) => {
        //   return <div>
        //       {header.column.label}      <icon-ic-round-search class="text-icon" />
        //     </div>;

        // },
        className: !item.isExcel ? 'onExcel' : '',
        prop: item.prop,
        label: $t(item.label),
        sortable: 'custom'
      } as Partial<TableColumnCtx<TableColumn>> & TableColumn & { checked?: boolean };
    });
  return arr;
});

// const { hasAuth } = useAuth();
const route = useRoute();
const keyWord = ref('');
const searchParams = ref<UI.SearchParams>({});
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
    alovaInstance.Get(pageUrl, {
      // Apis.TableColumn.get_page_configid_tableof({
      // pathParams: {
      //   configId: configId.value || '',
      //   tableof: tableof.value || ''
      // },
      params: {
        PageIndex: upPageIndex,
        pageSize: upPageSize,
        sortList: dataTableConfig.value.sortList,
        search: searchParams.value,
        ...param
      }
    }),
  {
    watchingStates: [keyWord, dataTableConfig, searchParams],
    force: false,
    immediate: false,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 15, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: (res: any) => res.data?.pagerInfo?.totalRowCount,
    data: (res: any) => res.data?.data as any
  }
);

// 删除
const { send: handleDelete } = useRequest(
  ids =>
    // Apis.TableColumn.delete_delete_configid_tableof({
    alovaInstance.Delete(delUrl, ids, {
      transform: (res: any) => {
        window.$message?.success('删除成功！');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

const sortChange = ({ prop, order }: Sort) => {
  dataTableConfig.value.sortList = {};
  dataTableConfig.value.sortList = { [prop]: order?.replace('ending', '') as unknown as OrderTypeEnum };
};
const checkedRowKeys = ref<string[]>([]);
// 打开编辑/新增

const openForm = (id: string | null) => {
  emit('openForm', id || '');
};

const handleClose = (prop: string) => {
  // eslint-disable-next-line @typescript-eslint/no-dynamic-delete
  delete searchParams.value[prop];
};
// ====================开始处理动态生成=====================
// 共享函数
defineExpose({
  openForm,
  reload,
  handleDelete,
  tableof,
  configId
});
</script>

<template>
  <ElCard class="sm:flex-1-hidden card-wrapper" body-class="ht50">
    <template #header>
      <div class="flex items-center justify-between">
        <p>{{ $t(route.meta.i18nKey || route.meta.title || '') }}</p>
        <TableHeaderOperation
          v-model:columns="columns"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @refresh="reload"
        >
          <template #prefix>
            <slot name="prefix"></slot>

            <TableViewSetting
              :config-id="configId"
              :tableof="tableof"
              :router="router"
              @load-change="loadChange"
            ></TableViewSetting>
          </template>
          <!-- v-if="hasAuth(dbConfig.addUrl)" -->
          <ElButton ghost type="primary" @click="openForm(null)">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </ElButton>
          <!--       -->
          <ElPopconfirm :title="$t('common.confirmDelete')" @confirm="handleDelete(checkedRowKeys)">
            <template #reference>
              <ElButton type="danger" plain :disabled="checkedRowKeys.length == 0">
                <template #icon>
                  <icon-ic-round-delete class="text-icon" />
                </template>
                {{ $t('common.batchDelete') }}
              </ElButton>
            </template>
          </ElPopconfirm>
          <ElButton ghost @click="downloadExcel(tableRef?.$el, $t(route.meta.i18nKey || route.meta.title || ''))">
            <template #icon>
              <icon-charm:download class="text-icon" />
            </template>
            {{ $t('导出') }}
          </ElButton>
        </TableHeaderOperation>
      </div>
    </template>
    <div class="mb-1 flex flex-wrap">
      <ElTag v-for="(item, index) in Object.keys(searchParams)" :key="index" closable @close="handleClose(item)">
        {{ columns.find(column => column.prop == item)?.label }}:
        {{ $t('sqlSugar.' + ConditionalType[searchParams[item].searchType || 0]) }}
        {{ searchParams[item].value }}
      </ElTag>
    </div>

    <div :class="total ? 'h-[calc(100%-50px)]' : 'h-full'">
      <ElTable
        v-bind="$attrs"
        id="table_excel"
        ref="tableRef"
        v-loading="loading"
        height="100%"
        border
        class="sm:h-full"
        sortable
        :data="data"
        row-key="id"
        @sort-change="sortChange"
      >
        <ElTableColumn v-for="(column, index) in columnData" :key="index" v-bind="column">
          <template v-if="column.searchType !== null" #header>
            <CustomHeader v-model="searchParams" :column="column"></CustomHeader>
          </template>
        </ElTableColumn>
      </ElTable>
    </div>

    <div v-if="total" class="mt-20px flex justify-end">
      <div>
        <ElPagination
          layout="total,prev,pager,next,sizes"
          :current-page="page"
          :total="total"
          :page-size="pageSize"
          :page-sizes="[15, 50, 200, 500, total || 1000].sort((a, b) => a - b)"
          :page-count="page"
          @current-change="
            val => {
              page = val;
            }
          "
          @size-change="
            val => {
              pageSize = val;
              page = 1;
            }
          "
        />

        <ElPagination layout="total,prev,pager,next,sizes" />
      </div>
    </div>
  </ElCard>
</template>

<style scoped></style>
