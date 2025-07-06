<script setup lang="tsx">
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import type { TableColumnCtx } from 'element-plus';
import { ElButton, ElInput, ElOption, ElPopconfirm, ElSelect, ElSwitch } from 'element-plus';
import { useForm, useRequest } from '@sa/alova/client';
import { VueDraggable } from 'vue-draggable-plus';
import { Hide, View } from '@element-plus/icons-vue';
import { ConditionalType } from '@/api/sqlSugar';
import type { TableColumn, UpdateTableViewParam } from '@/api/globals';
import { getEnumValue } from '@/utils/common';
import createComponent from '@/utils/createComponent';
import I18nDrawer from '@/components/i18n/i18n-drawer.vue';
import { $t } from '@/locales';
import MonacoCode from '../modules/dynamic-component.vue';
import type { ComponentApi } from '../types';

defineOptions({ name: 'TableView' });

const route = useRoute();
const configId = ref<string>((route.query.configId as string) || '');
const tableof = ref<string>((route.query.tableof as string) || '');
const routerUrl = ref<string>((route.query.router as string) || '');

const formRef = ref();
let getTableHeader: () => void;

// 初始化表单
const {
  form: formData,
  send: submit,
  loading: submitLoading,
  updateForm
} = useForm(
  from =>
    Apis.TableView.post_setedit({
      data: from,
      transform: res => {
        window.$message?.success('保存成功！');
        formData.value.id = res.data;
        getTableHeader();
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: false,
    initialForm: {
      configId: configId.value,
      tableof: tableof.value,
      router: routerUrl.value
    } as UpdateTableViewParam,
    async middleware(_, next) {
      await next();
    }
  }
);

// 初始化表头数据获取
const { loading, send: fetchTableHeader } = useRequest(
  () =>
    Apis.TableView.get_getedit({
      params: {
        ConfigId: configId.value || '',
        Tableof: tableof.value || '',
        Router: routerUrl.value || ''
      },
      transform: res => {
        updateForm(res.data as UpdateTableViewParam);
      }
    }),
  {
    force: true,
    immediate: true
  }
);

// 赋值给全局函数
getTableHeader = fetchTableHeader;

// #region 组件函数
const openHeadAttrs = async (row: TableColumn) => {
  const { on, unmount } = await createComponent(MonacoCode, {
    modelValue: JSON.stringify(row.headAttrsString || {}) as unknown as ComponentApi,
    visible: true
  });

  on('Change', (code: ComponentApi) => {
    row.headAttrsString = JSON.stringify(code);
  });
  on('update:visible', () => {
    unmount();
  });
};

const openI180n = async () => {
  const { on, unmount } = await createComponent(I18nDrawer, { visible: true });
  on('update:visible', res => {
    if (!res) {
      unmount();
    }
  });
};

// 添加新列
function handleAdd() {
  formData.value?.columns?.push({
    isCustom: true
  } as TableColumn);
}

// 设置行样式
const rowClassName = ({ row }: { row: TableColumn; rowIndex: number }) => {
  if (row.isEditDel) return 'info-row';
  return '';
};
// #endregion

// #region 表格列配置
const columns = ref<Array<Partial<TableColumnCtx<TableColumn>> & { headAttrs?: ComponentApi; checked?: boolean }>>([
  {
    prop: 'sort',
    label: $t('table.dragsort'),
    align: 'center',
    checked: true,
    width: 80,
    formatter: row => {
      return (
        <div class="handle">
          <icon-mdi-drag class="mr-8px h-full cursor-move text-icon" />
          {row.sort}
        </div>
      );
    }
  },
  {
    prop: 'prop',
    label: $t('table.prop'),
    align: 'center',
    checked: true,
    width: 150
  },
  {
    prop: 'label',
    label: $t('table.label'),
    align: 'center',
    checked: true,
    width: 200,
    formatter: row => {
      return (
        <div class="flex items-center justify-between">
          <ElInput
            class="flex-1"
            v-model={row.label}
            onChange={() => {
              row.isEditDel = false;
            }}
          ></ElInput>
          <el-text type="primary" onClick={() => openI180n()} class="ml-8px">
            翻译
          </el-text>
        </div>
      );
    }
  },
  {
    prop: 'isShow',
    label: $t('table.table'),
    align: 'center',
    width: 80,
    checked: true,
    formatter: row => {
      return (
        <ElSwitch
          v-model={row.isShow}
          active-action-icon={View}
          inactive-action-icon={Hide}
          active-value={true}
          inactive-value={false}
          onChange={() => {
            row.isExcel = row.isShow;
          }}
        />
      );
    }
  },
  {
    prop: 'isExcel',
    label: $t('table.excel'),
    align: 'center',
    width: 80,
    checked: true,
    formatter: row => {
      return (
        <ElSwitch
          disabled={!row.isShow}
          v-model={row.isExcel}
          active-action-icon={View}
          inactive-action-icon={Hide}
          active-value={true}
          inactive-value={false}
        />
      );
    }
  },

  {
    prop: 'columnType',
    label: $t('table.columnType'),
    align: 'center',
    checked: true,
    formatter: row => {
      return (
        <div class="flex flex-center">
          {row.headAttrs?.component}
          <ElButton
            type="primary"
            onClick={() => openHeadAttrs(row)}
            plain={row.headAttrs === '{}'}
            loading={loading.value}
          >
            高级
          </ElButton>

          {/* {renderColumnType(row)} */}
        </div>
      );
    }
  },
  {
    prop: 'searchType',
    label: $t('table.searchType'),
    align: 'center',
    checked: true,
    formatter: row => {
      return (
        <ElSelect
          v-model={row.searchType}
          placeholder="请选择"
          clearable
          onChange={() => {
            row.isEditDel = false;
          }}
        >
          {getEnumValue(ConditionalType).map(item => (
            <ElOption label={$t(`sqlSugar.${ConditionalType[item]}`)} value={item} />
          ))}
        </ElSelect>
      );
    }
  },

  {
    prop: 'use',
    label: $t('table.use'),
    align: 'center',
    width: 180,
    checked: true,
    formatter: row => (
      <div class="flex-center gap-8px">
        {!row.isEditDel ? (
          <ElPopconfirm title={$t('common.confirmDelete')} onConfirm={() => (row.isEditDel = true)}>
            {{
              reference: () => (
                <ElButton type="danger" plain size="small">
                  {$t('common.delete')}
                </ElButton>
              )
            }}
          </ElPopconfirm>
        ) : (
          <ElButton type="primary" plain size="small" onClick={() => (row.isEditDel = false)}>
            {$t('common.add')}
          </ElButton>
        )}
      </div>
    )
  }
]);
// #endregion
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <ElCard class="sm:flex-1-hidden card-wrapper" body-class="ht50">
      <template #header>
        <div class="flex items-center justify-between">
          <p>{{ $t('table.setting') }}</p>
          <TableHeaderOperation
            id="TableHeader"
            v-model:columns="columns"
            tableof="TableHeaderDTO"
            :loading="loading"
            @refresh="getTableHeader"
          >
            <ElButton type="primary" @click="handleAdd()">
              <template #icon>
                <icon-ic-round-plus class="text-icon" />
              </template>
              {{ $t('common.add') }}
            </ElButton>

            <template #suffix>
              <ElButton>{{ $t('预览') }}</ElButton>
              <ElButton>{{ $t('查看json') }}</ElButton>
              <ElButton type="primary" @click="submit">{{ $t('common.confirm') }}</ElButton>
            </template>
          </TableHeaderOperation>
        </div>
      </template>

      <ElForm ref="formRef" :inline="true" :model="formData" size="large" :show-label="false">
        <ElFormItem prop="sortKey" :label="$t('table.sorts')">
          <SortCascader v-model="formData.sorts" :columns="formData.columns" class="w-200"></SortCascader>
        </ElFormItem>
      </ElForm>

      <VueDraggable
        v-model="formData.columns as TableColumn[]"
        class="h-[calc(100%-50px)]"
        target="tbody"
        filter=".handle"
        :animation="150"
      >
        <ElTable
          v-loading="submitLoading || loading"
          height="100%"
          border
          class="sm:h-full"
          :data="formData.columns || []"
          :row-class-name="rowClassName"
          row-key="id"
        >
          <ElTableColumn v-for="col in columns" :key="col.prop" v-bind="col" />
        </ElTable>
      </VueDraggable>
    </ElCard>
  </div>
</template>

<style>
.el-table .info-row {
  --el-table-tr-bg-color: var(--el-color-info-light-9);
}
</style>
