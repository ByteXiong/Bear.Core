<script setup lang="ts">
import { computed, ref } from 'vue';
import { $t } from '@/locales';
import { useForm, useFormRules } from '@/hooks/common/form';
import type { TableColumn } from '@/api/globals';
import { ColumnTypeEnum } from '@/api/apiEnums';
import {ConditionalType  } from '@/api/sqlSugar';
defineOptions({
  name: 'TableHeaderSearch'
});

interface Emits {
  (e: 'searchChange', searchParams: UI.SearchParams): void;
}

const emit = defineEmits<Emits>();

const { formRef, validate, restoreValidation } = useForm();

const formData = ref<Record<string, any>>({});

// const params = ref<Record<string, NaiveUI.SearchField>>({} as { [key: string]: NaiveUI.SearchField });

type RuleKey = Extract<keyof Api.SystemManage.UserSearchParams, 'userEmail' | 'userPhone'>;

const rules = computed<Record<RuleKey, App.Global.FormRule>>(() => {
  const { patternRules } = useFormRules(); // inside computed to make locale reactive

  return {
    userEmail: patternRules.email,
    userPhone: patternRules.phone
  };
});

interface Props {
  searchData: TableColumn[];
}

const props = defineProps<Props>();

async function reset() {
  await restoreValidation();
  emit('searchChange', {});
}

async function search() {
  await validate();
  const searchParams = {} as UI.SearchParams;
  Object.keys(formData.value).forEach(key => {
    searchParams[key] = {
      key,
      searchType: (props.searchData.find(item => item.label === key)?.searchType ||
      ConditionalType.Equal) as ConditionalType,
      value: formData.value[key]
    };
  });
  emit('searchChange', searchParams);
}
</script>

<template>
  <ElCard v-if="searchData.length > 0" :bordered="false" size="small" class="card-wrapper">
    <ElCollapse>
      <ElCollapseItem :title="$t('common.search')" name="user-search">
        <ElForm ref="formRef" :model="formData" :rules="rules" label-placement="right" :label-width="80">
          <ElRow :gutter="24">
            <ElCol v-for="(item, index) in searchData" :key="index" :lg="4" :md="8" :sm="12">
              <ElFormItem :label="$t(item.label || '')" :prop="item.prop || ''">
                <!--
 <DicSelect
                  v-if="item.columnType === ColumnTypeEnum.字典"
                  v-model="formData[item.prop || '']"
                  :group-by="item.columnTypeDetail || ''"
                  :placeholder="'请选择' + $t(item.label || '')"
                ></DicSelect>
                <EnumSelect
                  v-else-if="item.columnType === ColumnTypeEnum.枚举"
                  v-model="formData[item.prop || '']"
                  :group-by="item.columnTypeDetail || ''"
                  :placeholder="'请选择' + $t(item.label || '')"
                ></EnumSelect>
-->

                <ElInputNumber
                  v-if="item.columnType === ColumnTypeEnum.整数"
                  v-model="formData[item.prop || '']"
                  :placeholder="$t('common.placeholder') + $t(item.label || '')"
                ></ElInputNumber>
                <ElInput
                  v-else
                  v-model="formData[item.prop || '']"
                  :placeholder="$t('common.placeholder') + $t(item.label || '')"
                />
              </ElFormItem>
            </ElCol>
            <ElCol :lg="4" :md="8" :sm="12">
              <ElSpace class="w-full justify-end" alignment="end">
                <ElButton @click="reset">
                  <template #icon>
                    <icon-ic-round-refresh class="text-icon" />
                  </template>
                  {{ $t('common.reset') }}
                </ElButton>
                <ElButton type="primary" plain @click="search">
                  <template #icon>
                    <icon-ic-round-search class="text-icon" />
                  </template>
                  {{ $t('common.search') }}
                </ElButton>
              </ElSpace>
            </ElCol>
          </ElRow>
        </ElForm>
      </ElCollapseItem>
    </ElCollapse>
  </ElCard>
</template>

<style scoped></style>
