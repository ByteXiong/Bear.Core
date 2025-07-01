<script setup lang="ts">
import { ref } from 'vue';

const user = ref({
  avatar: '/static/logo.png',
  name: '管理员用户',
});

function navigateTo(url: string) {
  uni.navigateTo({ url });
}
</script>

<template>
  <view class="my-container">
    <!-- 用户信息栏 -->
    <view class="user-info">
      <wd-avatar size="large" :src="user.avatar" round />
      <text class="username">
        {{ user.name }}
      </text>
    </view>

    <!-- 功能列表 -->
    <wd-cell-group>
      <wd-cell
        title="账号设置"
        value=""
        is-link
        @click="navigateTo('/pages/my/settings')"
      />
      <wd-cell
        title="关于我们"
        value="v1.0.0"
        is-link
        @click="navigateTo('/pages/about')"
      />
    </wd-cell-group>
  </view>
</template>

<style lang="scss">
.my-container {
  padding: 40rpx 30rpx;

  .user-info {
    display: flex;
    align-items: center;
    padding: 40rpx;
    background: #fff;
    border-radius: 16rpx;
    margin-bottom: 30rpx;

    .username {
      margin-left: 30rpx;
      font-size: 36rpx;
      font-weight: 500;
    }
  }
}
</style>
