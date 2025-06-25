<script setup lang="tsx">
import { computed, ref ,watch} from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useForm as useCommonForm, useFormRules } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { MenuButton, MenuQuery, UpdateMenuParam } from '@/api/globals';
import { getLocalIcons } from '@/utils/icon';
import SvgIcon from '@/components/custom/svg-icon.vue';
import { IconType, LayoutType, MenuType,Status } from '@/api/apiEnums';
import { getEnumValue } from '@/utils/common';
import createComponent from '@/utils/createComponent';
import I18nDrawer from '@/components/i18n/i18n-drawer.vue';
defineOptions({
  name: 'MenuEditForm'
});
type FormType = UpdateMenuParam &{path:string};

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useCommonForm();
// 规则验证获取对象
// const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormType;
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
    (form.id?Apis.Menu.put_update : Apis.Menu.post_add)({
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
      id: 0,
      parentId: 0,
      menuType: MenuType.Menu,
      iconType: IconType.iconify图标,
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
const openForm = async (parentId : string|null, id?: string ) => {
  visible.value = true;
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
const localIcons = getLocalIcons();
const localIconOptions = localIcons.map(item => ({
  label: () => (
    <div class="flex-y-center gap-16px">
      <SvgIcon localIcon={item} class="text-icon" />
      <span>{item}</span>
    </div>
  ),
  value: item
}));




function addButton(index: number) {
  formData.value.buttons?.splice(index + 1, 0, {
    code: '', desc: '',
    id: "",
    parentId: null,
    status: true
  });
}
function removeButton(index: number) {
  formData.value.buttons?.splice(index, 1);
}


function addQuery(index: number) {
  formData.value.querys?.splice(index + 1, 0, {
    id: "",
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
  ()=>formData.value.name,
  () => {
  if (formData.value.name) {
    formData.value.path = "/"+formData.value.name;
  }else{
    formData.value.path = '';
  }
});
const openI180n=async ()=>{
     const { instance, on, unmount } =  await  createComponent(I18nDrawer,{visible:true})
     on('update:visible', (res) => {
      console.log(res);

      if (!res) {
    unmount()
      }

     })
}
defineExpose({
  openForm
});
</script>

<template>
  <ElDialog v-model="visible" :title="title" preset="card" class="w-800px">
    <ElScrollbar class="h-480px pr-20px">
      <ElForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="100">
        <ElRow>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.menuType')" prop="menuType">
              <ElRadioGroup v-model="formData.menuType" >
                <ElRadio
                  v-for="item in getEnumValue(MenuType).filter(item => item !== MenuType.Button && item !== MenuType.Query)"
                  :key="item"
                  :value="item"
                  :label="$t( MenuType[item])"

                />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.menuName')" prop="title" class="flex-center justify-around">
              <ElInput v-model="formData.title" :placeholder="$t('page.manage.menu.form.menuName')"  class="flex-1"/>
              <ElText type="primary" @click="openI180n">国际化</ElText>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.routeName')" prop="name">
              <ElInput v-model="formData.name" :placeholder="$t('page.manage.menu.form.routeName')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.routePath')" prop="path">
              <ElInput v-model="formData.path" disabled :placeholder="$t('page.manage.menu.form.routePath')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.pathParam')" prop="pathParam">
              <ElInput v-model="formData.pathParam" :placeholder="$t('page.manage.menu.form.pathParam')" />
            </ElFormItem>
          </ElCol>
          <ElCol v-if="!formData.parentId" :span="12">
            <ElFormItem :label="$t('page.manage.menu.layout')" prop="layout">
              <ElSelect v-model="formData.layout" clearable :placeholder="$t('page.manage.menu.form.layout')">
                <ElOption
                  v-for="item in getEnumValue(LayoutType) "
                  :key="item"
                  :value="item"
                  :label="$t( LayoutType[item])"
                ></ElOption>
              </ElSelect>
            </ElFormItem>
          </ElCol>
          <ElCol v-if="formData.menuType === MenuType.Menu" :span="12">

            <ElFormItem :label="$t('page.manage.menu.page')" prop="page">
              <ElInput v-model="formData.component" :placeholder="$t('page.manage.menu.form.page')" />
              <!-- <ElSelect v-model="formData.component" clearable :placeholder="$t('page.manage.menu.form.page')">
                <ElOption v-for="{ label, value } in pageOptions" :key="value" :label="label" :value="value"></ElOption>
              </ElSelect> -->
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.order')" prop="order">
              <ElInputNumber v-model="formData.order" class="w-full" :placeholder="$t('page.manage.menu.form.order')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.iconTypeTitle')" prop="iconType">
              <ElRadioGroup v-model="formData.iconType">
                <ElRadio
                  v-for="item in getEnumValue(IconType) "
                  :key="item"
                  :value="item"
                  :label="$t( IconType[item])"
                />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.icon')" prop="icon">
              <template v-if="formData.iconType === IconType.iconify图标">
                <ElInput v-model="formData.icon" :placeholder="$t('page.manage.menu.form.icon')" class="flex-1">
                  <template #suffix>
                    <SvgIcon v-if="formData.icon" :icon="formData.icon" class="text-icon" />
                  </template>
                </ElInput>
              </template>
              <template v-if="formData.iconType ===2">
                <ElSelect
                  v-model="formData.icon as string"
                  :placeholder="$t('page.manage.menu.form.localIcon')"
                  :options="localIconOptions"
                />
              </template>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.menuStatus')" prop="status">
              <ElRadioGroup v-model="formData.status">
                <ElRadio :value="true" label="启用" />
                <ElRadio :value="false" label="禁用" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.keepAlive')" prop="keepAlive">
              <ElRadioGroup v-model="formData.keepAlive">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.constant')" prop="constant">
              <ElRadioGroup v-model="formData.constant">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.href')" prop="href">
              <ElInput v-model="formData.href" :placeholder="$t('page.manage.menu.form.href')" />
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.hideInMenu')" prop="hideInMenu">
              <ElRadioGroup v-model="formData.hideInMenu">
                <ElRadio :value="true" label="是" />
                <ElRadio :value="false" label="否" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <!-- <ElCol v-if="formData.hideInMenu" :span="12">
            <ElFormItem :label="$t('page.manage.menu.activeMenu')" prop="activeMenu">
              <ElSelect
                v-model="formData.activeMenu"
                :options="pageOptions"
                clearable
                :placeholder="$t('page.manage.menu.form.activeMenu')"
              >
                <ElOption v-for="{ label, value } in pageOptions" :key="value" :label="label" :value="value"></ElOption>
              </ElSelect>
            </ElFormItem>
          </ElCol> -->
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.multiTab')" prop="multiTab">
              <ElRadioGroup v-model="formData.multiTab">
                <ElRadio :value="true" :label="$t('common.yesOrNo.yes')" />
                <ElRadio :value="false" :label="$t('common.yesOrNo.no')" />
              </ElRadioGroup>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem :label="$t('page.manage.menu.fixedIndexInTab')" prop="fixedIndexInTab">
              <ElInputNumber
                v-model="formData.fixedIndexInTab"
                class="w-full"
                clearable
                :placeholder="$t('page.manage.menu.form.fixedIndexInTab')"
              />
            </ElFormItem>
          </ElCol>
          <ElCol :span="24">
            <ElFormItem :label="$t('page.manage.menu.query')" prop="query">
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
                      <ElInput v-model="item.key" :placeholder="$t('page.manage.menu.form.queryKey')" class="flex-1" />
                    </ElFormItem>
                  </ElCol>
                  <ElCol :span="8">
                    <ElFormItem :prop="['query', index.toString(), 'value']">
                      <ElInput
                        v-model="item.value"
                        :placeholder="$t('page.manage.menu.form.queryValue')"
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
          <ElCol :span="24">
            <ElFormItem :label-col="{ span: 4 }" :label="$t('page.manage.menu.button')" prop="buttons">
              <ElButton v-if="formData.buttons && formData.buttons?.length === 0" class="w-full border-dashed" @click="addButton(-1)">
                <template #icon>
                  <icon-carbon-add class="align-sub text-icon" />
                </template>
                <span class="ml-8px">{{ $t('common.add') }}</span>
              </ElButton>
              <template v-else>
                <div v-for="(item, index) in formData.buttons" :key="index" class="flex gap-3">
                  <ElCol :span="8">
                    <ElFormItem :prop="['buttons', index.toString(), 'code']">
                      <ElInput
                        v-model="item.code"
                        :placeholder="$t('page.manage.menu.form.buttonCode')"
                        class="flex-1"
                      ></ElInput>
                    </ElFormItem>
                  </ElCol>
                  <ElCol :span="7">
                    <ElFormItem :prop="['buttons', index.toString(), 'desc']">
                      <ElInput
                        v-model="item.desc"
                        :placeholder="$t('page.manage.menu.form.buttonDesc')"
                        class="flex-1"
                      ></ElInput>
                      <ElText type="primary" @click="()=>{openI180n()}">国际化</ElText>
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
                      <ElButton @click="addButton(index)">
                        <template #icon>
                          <icon-ic:round-plus class="align-sub text-icon" />
                        </template>
                      </ElButton>
                      <ElButton @click="removeButton(index)">
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
