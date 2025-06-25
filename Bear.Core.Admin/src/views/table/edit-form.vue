<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import { useForm as useElForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import { ColumnTypeEnum, ViewTypeEnum } from '@/api/apiEnums';
import { alovaInstance } from '@/api';
import type { TableColumn, TableView } from '@/api/globals';
defineOptions({
  name: 'TableEditForm'
});
const route = useRoute();

interface Props {
  tableof: string;
  router: string;
  configId: string;
  infoUrl: string;
  submitUrl: string;
}
const { tableof, configId, router, infoUrl, submitUrl } = defineProps<Props>();

// const tableView = ref<TableView>();
const formColumns = ref<Array<TableColumn>>([]);
const loadChange = (table: TableView) => {
  formColumns.value = table?.columns?.filter(item => item.isShow) || [];
  // searchData.value = res.data?.tableColumns?.filter(item => item.searchType !== null) || [];
};

type FormDataType = Record<string, any>;

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useElForm();
// const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {};
interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    alovaInstance.Post(submitUrl, form, {
      // Apis.TableColumn.post_submit_configid_tableof({
      //   pathParams: { configId: configId || '', tableof: tableof || '' },

      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as FormDataType,
    async middleware(_, next) {
      validate().then(async () => {
        await next();
      });
    }
  }
);
/** 获取详情 */
const { send: getInfo } = useRequest(
  id =>
    // Apis.TableColumn.get_getform_configid_tableof({
    //   pathParams: { configId: configId.value || '', tableof: tableof.value || '' },
    alovaInstance.Get(infoUrl, {
      params: { id },
      transform: (res: any) => {
        updateForm(res.data || {});
      }
    }),
  { force: true, immediate: false }
);
const title = computed(() => {
  return formData.value.id ? $t('common.edit') : $t('common.add');
});
// 打开
const openForm = async (id: string|null) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  }
};
const closeForm = () => {
  // 关闭页面 清空formData
  visible.value = false;
  restoreValidation();
  resetFrom();
};

defineExpose({
  openForm
});

onMounted(() => {
  // renderWangEditor();
});
</script>

<template>
  <ElDialog
    v-model="visible"
    :title="title + $t(route.meta.i18nKey || route.meta.title || '')"
    preset="card"
    class="w-800px"
    @after-leave="closeForm"
  >
    <ElScrollbar class="h-480px pr-20px">
      <ElForm ref="formRef" :model="formData" label-placement="left" :rules="rules" :label-width="80">
        <ElFormItem
          v-for="(item, index) in formColumns"
          :key="index"
          span="24 s:12 m:6"
          :label="$t(item.label || '')"
          :prop="item.prop || ''"
          class="pr-24px"
        >
          <ElInputNumber
            v-if="item.columnType === ColumnTypeEnum.整数"
            v-model="formData[item.prop || '']"
          ></ElInputNumber>
          <ElInputNumber
            v-else-if="item.columnType === ColumnTypeEnum.小数"
            v-model:value="formData[item.prop || '']"
            :step="0.01"
          ></ElInputNumber>

          <DictSelect
            v-else-if="item.columnType === ColumnTypeEnum.字典"
            v-model:value="formData[item.prop || '']"
            :dict-id="item.columnTypeDetail || ''"
          ></DictSelect>
          <EnumSelect
            v-else-if="item.columnType === ColumnTypeEnum.枚举"
            v-model:value="formData[item.prop || '']"
            :group-by="item.columnTypeDetail || ''"
            :placeholder="$t('common.placeholder') + $t(item.label || '')"
          ></EnumSelect>

          <ElInput
            v-else-if="item.columnType === ColumnTypeEnum.TexTarea文本"
            v-model:value="formData[item.prop || '']"
            type="textarea"
            :autosize="{ minRows: 1, maxRows: 7 }"
          />

          <WangEditor
            v-else-if="item.columnType === ColumnTypeEnum.富文本"
            v-model:value="formData[item.prop || '']"
          ></WangEditor>

          <ElInput
            v-else
            v-model:value="formData[item.prop || '']"
            :placeholder="$t('common.placeholder') + $t(item.label || '')"
          />
        </ElFormItem>
      </ElForm>
    </ElScrollbar>
    <template #footer>
      <ElSpace :size="16" class="float-right">
        <TableFormSetting
          :tableof="tableof"
          :config-id="configId"
          :router="router"
          :view-type="ViewTypeEnum.编辑"
          @load-change="loadChange"
        ></TableFormSetting>
        <ElButton @click="visible = false">{{ $t('common.cancel') }}</ElButton>
        <ElButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</ElButton>
      </ElSpace>
    </template>
  </ElDialog>
</template>
