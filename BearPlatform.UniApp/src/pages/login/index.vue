<script setup lang="ts">
import type { LoginParam } from '@/api/globals';
import omit from 'lodash-es/omit';
import { ref } from 'vue';
import { useToast } from 'wot-design-uni';
import { useUserStore } from '@/stores/modules/user';

const pageQuery = ref<Record<string, any> | undefined>(undefined);
onLoad((query) => {
  pageQuery.value = query;
});

const router = useRouter();
const { show: showToast } = useToast();
const form = ref<LoginParam>({
  userName: 'admin',
  password: '123456',
  captcha: '',
  captchaId: '',
});

function handleSubmit() {
  console.log(form.value);

  if (!form.value.userName || !form.value.password) {
    showToast({ msg: '账号密码不能为空' });
    return;
  }
  useUserStore().login(form.value).then(() => {
    // Toast('登录成功', { duration: 1500 });
    setTimeout(() => {
      if (unref(pageQuery)?.redirect) {
        // 如果有存在redirect(重定向)参数，登录成功后直接跳转
        const params = omit(unref(pageQuery), ['redirect', 'tabBar']);
        if (unref(pageQuery)) {
          // 这里replace方法无法跳转tabbar页面故改为replaceAll

          unref(pageQuery)?.tabBar === 'true'
            ? router.replaceAll({ name: unref(pageQuery)?.redirect, params })
            : router.replace({ name: unref(pageQuery)?.redirect, params });
        }
      } else {
        // 不存在则返回上一页
        router.back();
      }
    }, 1500);
  });
  // 调用登录API
  showToast({ msg: '登录成功' });
}
</script>

<template>
  <view class="container">
    <view class="liquid-bg">
      <view class="liquid" />
      <view class="liquid" />
      <view class="liquid" />
    </view>

    <view class="login-container">
      <view class="form-wrapper">
        <view class="logo-wrapper">
          <wd-img :width="60" :height="60" src="/src/static/logo.png" />
          <span class="title"> Bear.UniApp</span>
        </view>
        <wd-form :model="form">
          <wd-input
            v-model="form.userName"
            label="账号"
            placeholder="请输入管理员账号"
            clearable
          />
          <wd-input
            v-model="form.password"
            label="密码"
            is-password
            placeholder="请输入密码"
            clearable
          />
          <wd-button
            type="primary"
            native-type="submit"
            block
            custom-class="login-btn"
            @click="handleSubmit"
          >
            立即登录
          </wd-button>
        </wd-form>
      </view>
    </view>
  </view>
</template>

<style lang="scss">
.container {
  position: relative;
  width: 100vw; // Changed from 100vh to 100vw
  height: 100vh;
  overflow: hidden;
}
.login-container {
  display: flex;
  position: absolute;
  flex-direction: column;
  justify-content: center;
  align-items: center; /* 新增水平居中 */
  left: 50%; /* 新增定位 */
  top: 50%; /* 新增定位 */
  transform: translate(-50%, -50%); /* 新增居中修正 */
  box-shadow: 0 0 50rpx rgba(0, 0, 0, 0.1);
  width: 80%; /* 设置相对宽度 */
  min-height: 40%; /* 最小高度保证 */
  background-color: rgba(255, 255, 255, 0.5);
  border-radius: 20rpx;
  .wd-input {
    margin: 30rpx 0;
  }
}

.login-btn {
  margin-top: 60rpx;
}
.logo-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 60rpx;
  .title {
    color: #465cff;
    font-size: 46rpx;
    font-weight: bold;
    margin-left: 20rpx;
  }
}

/* 新增流体动画样式 */
.liquid-bg {
  width: 100%;
  height: 100vh;
  background: linear-gradient(to right, #eebefa, #b197fc, #66d9e8, #4dabf7, #faa2c1);
  background-size: 400% 400%;
  animation: rotate 10s ease infinite;
}

@keyframes rotate {
  0% {
    background-position: 0% 0%;
  }
  50% {
    background-position: 100% 100%;
  }
  100% {
    background-position: 0% 0%;
  }
}
</style>
