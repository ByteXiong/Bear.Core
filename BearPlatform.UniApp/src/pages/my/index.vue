<script setup lang="ts">
import type { JwtUserInfo } from '@/api/globals';
import { ref } from 'vue';
import { useUserStore } from '@/stores/modules/user';

const userStore = useUserStore();
const menuList = ref([
  {
    title: '分享好友',
    icon: '/static/images/share.png',
    url: '/pages/open-wx',
  },
  {
    title: '点赞',
    icon: '/static/images/star.png',
    url: `/pages/open-link?link=${encodeURI('https://bear.js.org/')}`,
  },
  {
    title: '意见反馈',
    icon: '/static/images/feedback.png',
    url: '/pages/open-wx',
  },
  {
    title: '我要合作',
    icon: '/static/images/cooperate.png',
    url: '/pages/open-wx',
  },
]);
const list = ref([
  {
    title: '设置',
    icon: '/static/images/share.png',
    url: '/pages/my/setting',
  },
  {
    title: '功能介绍',
    icon: '/static/images/share.png',
    url: `/pages/open-link?link=${encodeURI('https://bear.js.org/')}`,
  },
  {
    title: '开源地址',
    icon: '/static/images/share.png',
    url: `/pages/open-link?link=${encodeURI('https://bear.js.org/')}`,
    // query: {
    //  ,
    // },
  },
  {
    title: '帮助中心',
    icon: '/static/images/share.png',
    url: `/pages/open-link?link=${encodeURI('https://bear.js.org/')}`,
  },
  {
    title: '关于我们',
    icon: '/static/images/share.png',
    url: '/pages/open-wx',
  },
  {
    title: '合作伙伴',
    icon: '/static/images/share.png',
    url: '/pages/open-wx',
  },
]);
function handleClick(item: any) {
  //   console.log(item);
  uni.navigateTo({
    url: item.url,
  });
}

function handleLogin() {
  uni.navigateTo({
    url: '/pages/login/index',
    query: {
      redirect: '/pages/my/index',
      tabBar: 'true',
    },
  });
}
const userInfo = computed<JwtUserInfo | null>(() => userStore.userInfo);
</script>

<template>
  <view class="my-page">
    <!-- 用户信息区域 -->
    <view class="user">
      <view class="user-bg" />
      <view class="user-info">
        <wd-img :width="60" :height="60" src="/src/static/logo.png" />
        <wd-text
          v-if="!userStore.loggedIn" text="请登录" @click="handleLogin"
        />
        <wd-text v-else :text="userInfo?.user.nickName || ''" />
      <!-- 功能列表 -->
      </view>
      <wd-card class="user-menu">
        <wd-grid clickable>
          <wd-grid-item v-for="item, index in menuList" :key="index" use-slot @click="handleClick(item)">
            <view class="flex flex-col items-center justify-center ">
              <wd-img :width="45" :height="45" :src="item.icon" class="p-2 menu-img" />
              <wd-text
                bold
                :text="item.title"
              />
            </view>
          </wd-grid-item>
        </wd-grid>
      </wd-card>

      <!-- 菜单列表 -->
      <wd-cell-group>
        <wd-cell v-for="item, index in list" :key="index" :title="item.title" is-link @click="handleClick(item)" />
      </wd-cell-group>
    </view>
  </view>
</template>

<style scoped lang="scss">
.my-page {
  min-height: 100vh;
  background-color: #f5f5f5;
  padding-bottom: 20px;
}
.user {
  position: relative;
}
.user-bg {
  top: 0;
  width: 100%;
  position: absolute;
  height: 25vh;
  background: linear-gradient(180deg, #eef5f4, #91a7ff);
  filter: blur(10px);
  transform: scale(1.1);
}
.user-info {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 40px 0 20px;
}
.user-menu {
  z-index: 100;
  position: relative;
  background-color: #fff;
}
.menu-img {
  background-color: #f1f3f5;
  // 圆角
  border-radius: 10px;
  margin: 2rpx;
}
</style>
