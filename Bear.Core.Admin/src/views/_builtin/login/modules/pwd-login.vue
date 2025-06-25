<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRequest } from 'alova/client';
import { loginModuleRecord } from '@/constants/app';
import type { LoginParam } from '@/api/globals';
import { useAuthStore } from '@/store/modules/auth';
import { useRouterPush } from '@/hooks/common/router';
import { useForm, useFormRules } from '@/hooks/common/form';
import { $t } from '@/locales';
import '@/api';

defineOptions({ name: 'PwdLogin' });

const authStore = useAuthStore();
const { toggleLoginModule } = useRouterPush();
const { formRef, validate } = useForm();
const model = ref<LoginParam>({
  userName: 'admin',
  password: '123456',
  captchaId: '',
  captcha: ''
});

const {
  data: captchaData,
  send: getCaptcha,
  loading: captchaLoading
} = useRequest(
  () =>
    Apis.Login.get_captcha({
      transform: ({ data }) => {
        model.value.captchaId = data.captchaId || '';
        return data;
      }
    }),
  { immediate: true, force: true }
);

const rules = computed<Record<keyof LoginParam, App.Global.FormRule[]>>(() => {
  // inside computed to make locale ref, if not apply i18n, you can define it without computed
  const { formRules } = useFormRules();

  return {
    userName: formRules.userName,
    password: formRules.pwd,
    captchaId: [],
    captcha: []
  };
});

async function handleSubmit() {
  await validate();
  await authStore.login(model.value);
}

type AccountKey = 'super' | 'admin' | 'user';

interface Account {
  key: AccountKey;
  label: string;
  userName: string;
  password: string;
}

const accounts = computed<Account[]>(() => [
  {
    key: 'super',
    label: $t('page.login.pwdLogin.superAdmin'),
    userName: 'Super',
    password: '123456'
  },
  {
    key: 'admin',
    label: $t('page.login.pwdLogin.admin'),
    userName: 'Admin',
    password: '123456'
  },
  {
    key: 'user',
    label: $t('page.login.pwdLogin.user'),
    userName: 'User',
    password: '123456'
  }
]);

async function handleAccountLogin(account: Account) {
  await authStore.login({
    ...account,
    captchaId: ''
  } as unknown as LoginParam);
}
</script>

<template>
  <ElForm ref="formRef" :model="model" :rules="rules" size="large" :show-label="false" @keyup.enter="handleSubmit">
    <ElFormItem prop="userName">
      <ElInput v-model="model.userName" :placeholder="$t('page.login.common.userNamePlaceholder')" />
    </ElFormItem>
    <ElFormItem prop="password">
      <ElInput
        v-model="model.password"
        type="password"
        show-password-on="click"
        :placeholder="$t('page.login.common.passwordPlaceholder')"
      />
    </ElFormItem>

    <ElFormItem v-if="captchaData?.showCaptcha" prop="captcha">
      <ElCol :span="16">
        <ElInput v-model="model.captcha" show-password-on="click" :placeholder="$t('请输入验证码')" />
      </ElCol>
      <ElCol :span="8" class="flex-y-center pl-5px">
        <ElImage v-loading="captchaLoading" :src="captchaData?.img || ''" @click="getCaptcha"></ElImage>
      </ElCol>
    </ElFormItem>
    <ElSpace direction="vertical" :size="24" class="w-full" fill>
      <div class="flex-y-center justify-between">
        <ElCheckbox>{{ $t('page.login.pwdLogin.rememberMe') }}</ElCheckbox>
        <ElButton text @click="toggleLoginModule('reset-pwd')">
          {{ $t('page.login.pwdLogin.forgetPassword') }}
        </ElButton>
      </div>
      <ElButton type="primary" size="large" round block :loading="authStore.loginLoading" @click="handleSubmit">
        {{ $t('common.confirm') }}
      </ElButton>
      <div class="flex-y-center justify-between gap-12px">
        <ElButton class="flex-1" size="default" @click="toggleLoginModule('code-login')">
          {{ $t(loginModuleRecord['code-login']) }}
        </ElButton>
        <ElButton class="flex-1" size="default" @click="toggleLoginModule('register')">
          {{ $t(loginModuleRecord.register) }}
        </ElButton>
      </div>
      <ElDivider class="text-14px text-#666 !m-0">{{ $t('page.login.pwdLogin.otherAccountLogin') }}</ElDivider>
      <div class="flex-center gap-12px">
        <ElButton
          v-for="item in accounts"
          :key="item.key"
          size="default"
          type="primary"
          @click="handleAccountLogin(item)"
        >
          {{ item.label }}
        </ElButton>
      </div>
    </ElSpace>
  </ElForm>
</template>

<style scoped></style>
