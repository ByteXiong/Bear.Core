<script lang="ts" setup>
import { useTheme } from '@/composables/useTheme';

import { useUserStore } from '@/stores/modules/user';
import { getCache, setCache } from '@/utils/cache';

const list = ref([
  {
    title: '主题设置',
    icon: '/static/images/share.png',
    url: '/pages/my/share',
    isLink: false,
  },
  {
    title: '基本信息',
    icon: '/static/images/share.png',
    url: '/pages/my/share',
    isLink: true,
  },
  {
    title: '修改密码',
    icon: '/static/images/share.png',
    url: '/pages/my/share',
    isLink: true,
  },

]);
function handleLogin() {
  const userStore = useUserStore();
  userStore.logout();

  uni.reLaunch({
    url: '/',
  });
}
const { theme, toggleTheme } = useTheme();
theme.value = getCache('theme');
function changeTheme(e: any) {
  toggleTheme(e.value);
  setCache('theme', e.value);
}
const checked = ref(theme.value);
</script>

<template>
  <view>
    <!-- 菜单列表 -->
    <wd-cell-group>
      <wd-cell v-for="item, index in list" :key="index" :title="item.title" :to="item.url" :is-link="item.isLink">
        <template v-if="item.title === '主题设置'" #default>
          <wd-switch v-model="checked" size="25px" active-value="dark" inactive-value="light" @change="changeTheme" />
        </template>
      </wd-cell>
    </wd-cell-group>
    <view class="footer">
      <wd-button type="primary" @click="handleLogin">
        退出登录
      </wd-button>
    </view>
  </view>
</template>

<style lang="css">
.footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 20rpx;
  background-color: #fff;
  text-align: center;
}
</style>
