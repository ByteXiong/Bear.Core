<script setup lang="tsx">
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import type { ColumnCls, TableColumnCtx } from 'element-plus';
import {
  ElButton,
  ElCheckbox,
  ElInput,
  ElMessage,
  ElMessageBox,
  ElOption,
  ElPopconfirm,
  ElSelect,
  ElSwitch
} from 'element-plus';
import { useForm, useRequest } from '@sa/alova/client';
import { VueDraggable } from 'vue-draggable-plus';
import { Hide, View } from '@element-plus/icons-vue';
import { ColumnTypeEnum, OrderTypeEnum } from '@/api/apiEnums';
import { ConditionalType } from '@/api/sqlSugar';
import type { TableColumn, TableView, UpdateTableViewParam } from '@/api/globals';
import { getEnumValue } from '@/utils/common';
import createComponent from '@/utils/createComponent';
import AllDictSelect from '@/components/select/all-dict-select.vue';
import AllEnumSelect from '@/components/select/all-enum-select.vue';
import I18nDrawer from '@/components/i18n/i18n-drawer.vue';
import { $t } from '@/locales';
import MonacoCode from '../modules/monaco-code.vue';
defineOptions({ name: 'TableView' });
const route = useRoute();
const router = useRouter();
const configId = ref<string>((route.query.configId as string) || '');
const tableof = ref<string>((route.query.tableof as string) || '');
const routerUrl = ref<string>((route.query.router as string) || '');

// #endregion
// #region 提交视图(TableView)
const formRef = ref();
// const { formRef, validate, restoreValidation } = useElForm();
// // 规则验证获取对象
// const { defaultRequiredRule, patternRules } = useFormRules();
// type RuleKey = keyof UpdateTableViewParam;
// const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {
//   sortKey: defaultRequiredRule,
//   sortOrder: defaultRequiredRule
// };

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
        // eslint-disable-next-line @typescript-eslint/no-use-before-define
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
// #endregion

// formData.value.configId = configId.value;
// formData.value.tableof = tableof.value;
// formData.value.router = routerUrl.value;

const { loading, send: getTableHeader } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.TableView.get_getedit({
      params: {
        ConfigId: configId.value || '',
        Tableof: tableof.value || '',
        Router: routerUrl.value || ''
      },
      transform: res => {
        updateForm(res.data as UpdateTableViewParam);
        // if (!res.success) {
        //   ElMessageBox.confirm(`列表页模型不存在`, '首次加载请创建模型', {
        //     confirmButtonText: 'OK',
        //     cancelButtonText: 'Cancel',
        //     type: 'warning'
        //   })
        //     .then(() => {
        //       // if you want to disable its autofocus
        //       // autofocus: false,

        //       submitView();
        //     })
        //     .catch(() => {
        //       router.back();
        //     });
        // } else {

        // }

        // start();
        // return res.data || [];
      }
    }),
  {
    force: true,
    immediate: true
  }
);

//= ===========================================设置头部=================================
//= ===========================================设置头部结束=================================
function renderColumnType(row: TableColumn) {
  switch (row.columnType) {
    case ColumnTypeEnum.枚举:
      return <AllEnumSelect v-model={row.columnTypeDetail} />;
    case ColumnTypeEnum.字典:
      return <AllDictSelect v-model={row.columnTypeDetail} />;
    case ColumnTypeEnum.时间:
      return <ElInput type="text" v-model={row.columnTypeDetail} placeholder="请输入yyyy-MM-dd HH:mm:ss格式" />;
    case ColumnTypeEnum.单图:
    case ColumnTypeEnum.多图:
    case ColumnTypeEnum.文件:
      return <ElInput type="text" v-model={row.columnTypeDetail} placeholder="请输入图片前缀URL" />;
    default:
      return null;
  }
}
const columns = ref<Array<Partial<TableColumnCtx<TableColumn>> & { checked?: boolean }>>([
  // {
  //   label: '序列',
  //   align: 'center',
  //   checked: false,
  //   width: 50,
  //   formatter: (_, index) => index + 1
  //   // formatter: (_, _, _, index) => index + 1
  // },
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
          <ElSelect
            class="w-140px"
            v-model={row.columnType}
            placeholder="请选择"
            clearable
            onChange={() => {
              row.isEditDel = false;
            }}
          >
            {getEnumValue(ColumnTypeEnum).map(item => (
              <ElOption label={ColumnTypeEnum[item]} value={item} />
            ))}
          </ElSelect>
          {renderColumnType(row)}
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
        {/* //<MonacoCode loading={submitLoading.value} code={row.attrs}>
         // {" "}
        //</MonacoCode> */}
        <ElButton type="primary" onClick={() => openMonacoCode(row)} plain={row.attrs === ''} loading={loading.value}>
          重写插槽
        </ElButton>
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

function handleAdd() {
  formData.value?.columns?.push({
    isCustom: true
  } as TableColumn);
}
const rowClassName = ({ row }: { row: TableColumn; rowIndex: number }) => {
  if (row.isEditDel) return 'info-row';
  return '';
};

const openMonacoCode = async (row: TableColumn) => {
  // 函数式组件 instance 为 ref , on 为 emit , unmount 为 destroy
  const { on, unmount } = await createComponent(MonacoCode, { modelValue: row.attrs, visible: true });

  on('Change', (code: string) => {
    row.attrs = code;
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
      <!--  :rules="rules" -->
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
