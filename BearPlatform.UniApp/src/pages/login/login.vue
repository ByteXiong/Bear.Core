<script setup lang="ts">
import { ref } from 'vue';
import { useToast } from 'wot-design-uni';

const { show: showToast } = useToast();
const form = ref({
  username: 'admin',
  password: '123456',
});

function handleSubmit() {
  console.log(form.value);
  

  if (!form.value.username || !form.value.password) {
    showToast({ msg: '账号密码不能为空' });
    return;
  }
  // 调用登录API
  showToast({ msg: '登录成功' });
}
</script>

<template>
  <view class="login-container">
    <view class="form-wrapper">
      <view class="logo-wrapper">
        <image
          src="/src/static/logo.png"
          style="width: 100rpx; height: 100rpx"
        />
        <span class="title"> Bear.UniApp</span>
      </view>
      <wd-form :model="form">
        <wd-input
          v-model="form.username"
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
</template>

<style lang="scss">
.login-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  min-height: 100vh;
  padding: 0 60rpx;
}

.form-wrapper {
  width: 100%;
  //边框
  border-radius: 20rpx;
  //阴影
  box-shadow: 0 0 50rpx rgba(0, 0, 0, 0.1);
  .wd-input {
    margin: 30rpx 0;
  }
  padding: 40rpx;
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
</style>
