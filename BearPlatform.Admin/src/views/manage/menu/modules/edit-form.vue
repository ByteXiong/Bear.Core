<script setup lang="tsx">
import { computed, watch } from 'vue';
import { useForm, useRequest } from 'alova/client';
import type { UpdateMenuParam } from '@/api/globals';
import { IconTypeEnum, LayoutType, MenuTypeEnum } from '@/api/apiEnums';
import { useForm as useCommonForm } from '@/hooks/common/form';
import { getLocalIcons } from '@/utils/icon';
import { getEnumValue } from '@/utils/common';
import createComponent from '@/utils/createComponent';
import SvgIcon from '@/components/custom/svg-icon.vue';
import { $t } from '@/locales';
import I18nDrawer from '@/components/i18n/i18n-drawer.vue';
import TreeSelect from './tree-select.vue';
defineOptions({
  name: 'MenuEditForm'
});
type FormType = UpdateMenuParam & { path: string };

const visible = defineModel<boolean>('visible', {
  required: true,
  default: false
});

const { formRef, validate, restoreValidation } = useCommonForm();
// 规则验证获取对象
// const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormType;
const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {};

interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

// const localIcons = getLocalIcons();

// const localIconOptions = localIcons.map(item => ({
//   // label: () => (
//   //   <div class="flex-y-center gap-16px">
//   //     <SvgIcon localIcon={item} class="text-icon" />
//   //     <span>{item}</span>
//   //   </div>
//   // ),
//   label: item,
//   value: item
// }));

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    (form.id ? Apis.Menu.put_update : Apis.Menu.post_add)({
      data: form,
      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
      id: null,
      parentId: null,
      menuType: MenuTypeEnum.Menu,
      iconType: IconTypeEnum.iconify,
      status: true,
      keepAlive: false,
      constant: false,
      hideInMenu: false,
      multiTab: false,
      buttons: [],
      querys: []
    } as unknown as FormType,
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
    Apis.Menu.get_getinfo({
      params: { id },
      transform: res => {
        updateForm(res.data);
      }
    }),
  { force: true, immediate: false }
);
const title = computed(() => {
  return formData.value.id ? $t('common.edit') : $t('common.add');
});
// 打开
const openForm = async (parentId: string | null, id?: string) => {
  if (id) {
    await getInfo(id);
  } else {
    formData.value.parentId = parentId;
  }
};

const closeForm = () => {
  visible.value = false;
  restoreValidation();
  resetFrom();
};

function addQuery(index: number) {
  formData.value.querys?.splice(index + 1, 0, {
    id: '',
    parentId: null,
    status: true,
    key: null,
    value: null
  });
}
function removeQuery(index: number) {
  formData.value.querys?.splice(index, 1);
}
watch(
  () => formData.value.name,
  () => {
    if (formData.value.name) {
      formData.value.path = `/${formData.value.name}`;
    } else {
      formData.value.path = '';
    }
  }
);
const openI180n = async () => {
  const { on, unmount } = await createComponent(I18nDrawer, { visible: true });
  on('update:visible', res => {
    if (!res) {
      unmount();
    }
  });
};

// // 高亮
// // 把主页的菜单全传入
// const props = defineProps<{
//   allPages: string[];
// }>();

// const pageOptions = computed(() => {
//   const allPages = [...props.allPages];

//   if (formData.value.name && !allPages.includes(formData.value.name)) {
//     allPages.unshift(formData.value.name);
//   }

//   const opts: CommonType.Option[] = allPages.map(page => ({
//     label: page,
//     value: page
//   }));

//   return opts;
// });

defineExpose({
  openForm
});
</script>

<template>
  <ElDialog v-model="visible" :title="title" preset="card" class="w-800px">
    <ElScrollbar class="h-480px pr-20px">
      <!-- {{ localIcons.length }} -->
      <ElForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="100">
        <ElRow>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.parent')" prop="menuType">
              <TreeSelect
                v-model="formData.parentId"
                val-key="id"
                :types="[MenuTypeEnum.Directory]"
                :placeholder="$t('common.placeholderSelect')"
              ></TreeSelect>
            </ElFormItem>
          </ElCol>

          <ElCol :span="12">{{ formData.parentId }}</ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.iconType')" prop="iconType">
              <ElRadioGroup v-model="formData.iconType">
                <ElRadio
                  v-for="item in getEnumValue(IconTypeEnum)"
                  :key="item"
                  :value="item"
                  :label="$t(IconTypeEnum[item])"
                />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.icon')" prop="icon">
              <template v-if="formData.iconType === IconTypeEnum.iconify">
                <ElInput
                  v-model="formData.icon"
                  :placeholder="$t('common.placeholder') + $t('menu.icon')"
                  class="flex-1"
                >
                  <template #suffix>
                    <SvgIcon v-if="formData.icon" :icon="formData.icon" class="text-icon" />
                  </template>
                </ElInput>
              </template>
              <template v-if="formData.iconType === IconTypeEnum.local">
                <ElSelect
                  v-model="formData.icon as string"
                  :placeholder="$t('common.placeholder') + $t('menu.icon')"
                  clearable
                  filterable
                >
                  <ElOption v-for="(item, index) in getLocalIcons()" :key="index" :label="item" :value="item">
                    <div class="flex-y-center gap-16px">
                      <SvgIcon :local-icon="item" class="text-icon" />
                      <span>{{ item }}</span>
                    </div>
                  </ElOption>
                  <template #label="{ value }">
                    <div class="flex items-center">
                      <SvgIcon :local-icon="value" class="mr-1 text-icon" />
                      <span>{{ value }}</span>
                    </div>
                  </template>
                </ElSelect>
              </template>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.menuType')" prop="menuType">
              <ElRadioGroup v-model="formData.menuType">
                <ElRadio
                  v-for="item in getEnumValue(MenuTypeEnum).filter(item => item !== MenuTypeEnum.Query)"
                  :key="item"
                  :value="item"
                  :label="$t(MenuTypeEnum[item])"
                />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.page')" prop="page">
              <ElInput v-model="formData.component" :placeholder="$t('common.placeholder') + $t('menu.page')"   :disabled="formData.menuType !== MenuTypeEnum.Menu"/>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.menuName')" prop="title" class="flex-center justify-around">
              <ElInput
                v-model="formData.title"
                :placeholder="$t('common.placeholder') + $t('menu.menuName')"
                class="flex-1"
              />
              <ElText
                type="primary"
                @click="
                  () => {
                    openI180n();
                  }
                "
              >
                {{ $t('common.i18n') }}
              </ElText>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.routeName')" prop="name">
              <ElInput v-model="formData.name" :placeholder="$t('common.placeholder') + $t('menu.routeName')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.pathParam')" prop="pathParam">
              <ElInput v-model="formData.pathParam" :placeholder="$t('common.placeholder') + $t('menu.pathParam')" />
            </ElFormItem>
          </ElCol>

          <ElCol v-if="!formData.parentId" :span="12">
            <ElFormItem :label="$t('menu.layout')" prop="layout">
              <ElSelect v-model="formData.layout" clearable :placeholder="$t('common.placeholder') + $t('menu.layout')">
                <ElOption
                  v-for="item in getEnumValue(LayoutType)"
                  :key="item"
                  :value="item"
                  :label="$t(LayoutType[item])"
                ></ElOption>
              </ElSelect>
            </ElFormItem>
          </ElCol>

          <ElCol :span="12">
            <ElFormItem :label="$t('menu.order')" prop="order">
              <ElInputNumber
                v-model="formData.order"
                class="w-full"
                :placeholder="$t('common.placeholder') + $t('menu.order')"
              />
            </ElFormItem>
          </ElCol>

          <ElCol :span="12">
            <ElFormItem :label="$t('menu.menuStatus')" prop="status">
              <ElRadioGroup v-model="formData.status">
                <ElRadio :value="true" label="启用" />
                <ElRadio :value="false" label="禁用" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.keepAlive')" prop="keepAlive">
              <ElRadioGroup v-model="formData.keepAlive">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.constant')" prop="constant">
              <ElRadioGroup v-model="formData.constant">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.href')" prop="href">
              <ElInput v-model="formData.href" :placeholder="$t('common.placeholder') + $t('menu.href')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.hideInMenu')" prop="hideInMenu">
              <ElRadioGroup v-model="formData.hideInMenu">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>

          <ElCol  :span="12">
            <ElFormItem :label="$t('menu.activeMenu')" prop="activeMenu"   >
              <TreeSelect  :disabled="formData.hideInMenu"
                v-model="formData.activeMenu as string"
                :types="[MenuTypeEnum.Directory, MenuTypeEnum.Menu]"
                val-key="name"
                clearable
                :placeholder="$t('common.placeholderSelect') + $t('menu.activeMenu')"
              ></TreeSelect>
            </ElFormItem>
          </ElCol>

          <ElCol :span="12">
            <ElFormItem :label="$t('menu.multiTab')" prop="multiTab">
              <ElRadioGroup v-model="formData.multiTab">
                <ElRadio :value="true" :label="$t('common.yesOrNo.yes')" />
                <ElRadio :value="false" :label="$t('common.yesOrNo.no')" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('menu.fixedIndexInTab')" prop="fixedIndexInTab">
              <ElInputNumber
                v-model="formData.fixedIndexInTab"
                class="w-full"
                clearable
                :placeholder="$t('common.placeholder') + $t('menu.fixedIndexInTab')"
              />
            </ElFormItem>
          </ElCol>
          <ElCol :span="24">
            <ElFormItem :label="$t('menu.query')" prop="query">
              <ElButton v-if="formData.querys?.length === 0" class="w-full border-dashed" @click="addQuery(-1)">
                <template #icon>
                  <icon-carbon-add class="align-sub text-icon" />
                </template>
                <span class="ml-8px">{{ $t('common.add') }}</span>
              </ElButton>
              <template v-else>
                <div v-for="(item, index) in formData.querys" :key="index" class="flex gap-3">
                  <ElCol :span="8">
                    <ElFormItem :prop="['query', index.toString(), 'key']">
                      <ElInput
                        v-model="item.key"
                        :placeholder="$t('common.placeholder') + $t('menu.queryKey')"
                        class="flex-1"
                      />
                    </ElFormItem>
                  </ElCol>
                  <ElCol :span="8">
                    <ElFormItem :prop="['query', index.toString(), 'value']">
                      <ElInput
                        v-model="item.value"
                        :placeholder="$t('common.placeholder') + $t('menu.queryValue')"
                        class="flex-1"
                      />
                    </ElFormItem>
                  </ElCol>
                  <ElCol :span="4">
                    <ElSwitch
                      v-model="item.status"
                      class="ml-2"
                      width="60"
                      inline-prompt
                      active-text="启用"
                      inactive-text="禁用"
                    />
                  </ElCol>
                  <ElCol :span="4">
                    <ElSpace class="ml-12px">
                      <ElButton @click="addQuery(index)">
                        <template #icon>
                          <icon-ic:round-plus class="align-sub text-icon" />
                        </template>
                      </ElButton>
                      <ElButton @click="removeQuery(index)">
                        <template #icon>
                          <icon-ic-round-remove class="align-sub text-icon" />
                        </template>
                      </ElButton>
                    </ElSpace>
                  </ElCol>
                </div>
              </template>
            </ElFormItem>
          </ElCol>
        </ElRow>
      </ElForm>
    </ElScrollbar>
    <template #footer>
      <ElSpace :size="16" class="float-right">
        <ElButton @click="closeForm">{{ $t('common.cancel') }}</ElButton>
        <ElButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</ElButton>
      </ElSpace>
    </template>
  </ElDialog>
</template>

<style scoped></style>
