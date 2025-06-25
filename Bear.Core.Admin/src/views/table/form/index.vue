<script setup lang="tsx">
import { useForm, useRequest } from '@sa/alova/client';
import { useRoute, useRouter } from 'vue-router';
import { ref } from 'vue';
import type { TableColumnCtx } from 'element-plus';
import { ElButton, ElCheckbox, ElMessageBox, ElOption, ElPopconfirm, ElSelect } from 'element-plus';
import { VueDraggable } from 'vue-draggable-plus';
import { $t } from '@/locales';
import { ColumnTypeEnum } from '@/api/apiEnums';
import type { TableForm, TableFormItem } from '@/api/globals';
import { getEnumValue } from '@/utils/common';
import MonacoCode from '../modules/monaco-code.vue';
defineOptions({ name: 'UserManage' });
const route = useRoute();
const router = useRouter();
const configId = ref(route.query.configId as string);
const tableof = ref(route.query.tableof as string);
const routerUrl = ref(route.query.router as string);
// #region 排序
const { send: submitSort } = useForm(
  (_, row) =>
    Apis.TableFormItem.put_setsort({
      data: row,
      transform: ({ data }) => {
        window.$message?.success('保存成功！');
        Object.assign(row, data);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: []
  }
);

// #endregion

// #region 提交视图(TableForm)

// type FormDataType = typeof tableForm.value;

// const { formRef, validate, restoreValidation } = useElForm();
// // 规则验证获取对象
// const { defaultRequiredRule, patternRules } = useFormRules();
// type RuleKey = keyof FormDataType;
// const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {
//   sortKey: defaultRequiredRule,
//   sortOrder: defaultRequiredRule
// };

const {
  form: tableForm,
  send: submitView,
  updateForm
} = useForm(
  from =>
    from.id
      ? Apis.TableForm.put_update({
          data: from,
          transform: res => {
            window.$message?.success('保存成功！');
            tableForm.value.id = res.data;
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            getTableHeader();
          }
        })
      : Apis.TableForm.post_add({
          data: from,
          transform: res => {
            window.$message?.success('保存成功！');
            tableForm.value.id = res.data;
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            getTableHeader();
          }
        }),

  {
    immediate: false,
    resetAfterSubmiting: false,
    initialForm: {} as TableForm,
    async middleware(_, next) {
      await next();
    }
  }
);
// #endregion

const { loading, send: getTableHeader } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.TableForm.get_gettableform({
      params: {
        ConfigId: configId.value,
        Tableof: tableof.value,
        Router: routerUrl.value
      },
      transform: res => {
        if (!res.success) {
          ElMessageBox.confirm(`表单模型不存在`, '首次加载请创建模型', {
            confirmButtonText: 'OK',
            cancelButtonText: 'Cancel',
            type: 'warning'
          })
            .then(() => {
              // if you want to disable its autofocus
              // autofocus: false,
              tableForm.value.configId = configId.value;
              tableForm.value.tableof = tableof.value;
              tableForm.value.router = routerUrl.value;
              submitView();
            })
            .catch(() => {
              router.back();
            });
        } else {
          updateForm(res.data || {});
        }

        // start();
        return res.data || [];
      }
    }),
  {
    force: true,
    immediate: true
  }
);

// #region  行操作
const { send: submit, loading: submitLoading } = useForm(
  (_, row) =>
    Apis.TableFormItem.put_setformitem({
      data: row,
      transform: () => {
        window.$message?.success('保存成功！');

        // Object.assign(row, data);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as TableFormItem,
    async middleware(_, next) {
      await next();
    }
  }
);

const { send: handleDelete } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  ids =>
    Apis.TableFormItem.delete_delete({
      data: ids,
      transform: () => {
        window.$message?.success('删除成功！');
        getTableHeader();
      }
    }),
  {
    force: true,
    immediate: false
  }
);

const setSort = () => {
  tableForm.value?.items?.forEach((item, index) => {
    item.sort = index + 1;
  });

  submitSort(
    tableForm.value?.items
      ?.filter(x => x.id)
      ?.map(item => {
        return { sort: item.sort, id: item.id };
      })
  );
};
//= ===========================================设置头部=================================

const columns = ref<Array<Partial<TableColumnCtx<TableFormItem>> & { checked?: boolean }>>([
  {
    type: 'selection',
    label: $t('table.selection'),
    align: 'center',
    checked: true,
    width: 50,
    selectable: row => (row.id as any) !== '0'
  },
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
        <div>
          <el-text>{row.label}</el-text>
          <el-text type="primary" class="ml-8px">
            翻译
          </el-text>
        </div>
      );
    }
  },
  {
    prop: 'isShow',
    label: $t('table.isShow'),
    align: 'center',
    width: 100,
    checked: true,
    formatter: row => {
      return (
        <ElCheckbox
          checked={row.isShow as boolean}
          checked-value={true}
          v-model={row.isShow}
          onChange={() => {
            submit(row);
          }}
          disabled={submitLoading.value}
        >
          {row.isShow ? '显示' : '隐藏'}
        </ElCheckbox>
      );
    }
  },
  {
    prop: 'componentType',
    label: $t('table.columnType'),
    align: 'center',
    checked: true,
    formatter: row => {
      return (
        <div>
          <ElSelect
            v-model={row.componentType}
            placeholder="请选择"
            onChange={() => {
              submit(row);
            }}
            clearable
          >
            {getEnumValue(ColumnTypeEnum).map(item => (
              <ElOption label={ColumnTypeEnum[item]} value={item} />
            ))}
          </ElSelect>
        </div>
      );
    }
  },
  {
    prop: 'use',
    label: $t('table.use'),
    align: 'center',
    checked: true,
    formatter: row => (
      <div class="flex-center gap-8px">
        <MonacoCode
          loading={submitLoading.value}
          code={row.attrs}
          onChange={code => {
            row.attrs = code;
            submit(row);
          }}
        >
          {' '}
        </MonacoCode>
        {(row.id as unknown as string) !== '0' ? (
          <ElPopconfirm title={$t('common.confirmDelete')} onConfirm={() => handleDelete([row.id])}>
            {{
              reference: () => (
                <ElButton type="danger" plain>
                  {$t('common.delete')}
                </ElButton>
              )
            }}
          </ElPopconfirm>
        ) : null}
      </div>
    )
  }
]);
const checkedRowKeys = ref<number[]>([]);

function handleAdd() {
  tableForm.value?.items?.push({
    formId: tableForm.value?.id,
    isCustom: true
  } as TableFormItem);
}
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <ElCard class="sm:flex-1-hidden card-wrapper" body-class="ht50">
      <template #header>
        <div class="flex items-center justify-between">
          <p>{{ $t('配置') }}</p>
          <TableHeaderOperation
            id="TableHeader"
            v-model:columns="columns"
            tableof="TableHeaderDTO"
            :disabled-delete="checkedRowKeys.length === 0"
            :loading="loading"
            @refresh="getTableHeader"
          >
            <div class="flex-1">
              <ElButton ghost type="primary" @click="handleAdd()">
                <template #icon>
                  <icon-ic-round-plus class="text-icon" />
                </template>
                {{ $t('common.add') }}
              </ElButton>
              <!--       -->
              <ElPopconfirm :title="$t('common.confirmDelete')" @confirm="handleDelete(checkedRowKeys)">
                <template #reference>
                  <ElButton type="danger" plain :disabled="checkedRowKeys.length === 0">
                    <template #icon>
                      <icon-ic-round-delete class="text-icon" />
                    </template>
                    {{ $t('common.batchDelete') }}
                  </ElButton>
                </template>
              </ElPopconfirm>
            </div>
          </TableHeaderOperation>
        </div>
      </template>
      <!--  :rules="rules" -->
      <ElForm
        ref="formRef"
        :inline="true"
        :model="tableForm"
        size="large"
        :show-label="false"
        @keyup.enter="submitView"
      >
        <!--
 <ElFormItem prop="sortKey" label="排序字段">
          <ElSelect
            v-model="tableForm.sortKey"
            :loading="loading"
            clearable
            placeholder="请选择"
            class="w-200px"
            @change="submitView"
          >
            <ElOption
              v-for="(item, index) in tableForm.columns"
              :key="index"
              :label="item.label || ''"
              :value="item.prop || ''"
            ></ElOption>
          </ElSelect>
        </ElFormItem>

        <ElFormItem prop="sortOrder" label="排序方式">
          <ElSelect
            v-model="tableForm.sortOrder"
            clearable
            :loading="loading"
            placeholder="请选择"
            class="w-200px"
            @change="submitView"
          >
            <ElOption
              v-for="(item, index) in getEnumValue(OrderTypeEnum)"
              :key="index"
              :label="OrderTypeEnum[item]"
              :value="OrderTypeEnum[item]"
            ></ElOption>
          </ElSelect>
        </ElFormItem>
-->
      </ElForm>

      <VueDraggable
        v-model="tableForm.items as TableFormItem[]"
        class="h-[calc(100%-50px)]"
        target="tbody"
        filter=".handle"
        :animation="150"
        @update="setSort"
      >
        <ElTable
          v-loading="submitLoading || loading"
          height="100%"
          border
          class="sm:h-full"
          :data="tableForm.items || []"
          row-key="id"
          @selection-change="checkedRowKeys = $event.map((item:TableFormItem) => item.id)"
        >
          <ElTableColumn v-for="col in columns" :key="col.prop" v-bind="col" />
        </ElTable>
      </VueDraggable>
    </ElCard>
  </div>
</template>

<style lang="scss" scoped>
:deep(.el-card) {
  .ht50 {
    height: calc(100% - 50px);
  }
}
</style>
