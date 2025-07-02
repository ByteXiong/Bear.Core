<script setup lang="ts">
import type { I18nDTO } from '@/api/globals';
import { usePagination } from 'alova/client';

const keyWord = ref('');
const sortList = ref<Record<string, any>>({ id: 'desc' });
const list = ref<I18nDTO[]>([]);
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload,
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
    Apis.I18n.get_getpage({
      params: {
        PageIndex: upPageIndex,
        PageSize: upPageSize,
        SortList: sortList.value,
        Search: {},
        StartIndex: 0,
      },
    }),
  {
    // watchingStates: [keyWord, sortList],
    force: true,
    immediate: false,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 20, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: res => res.data?.pagerInfo?.totalRowCount,
    data: (res) => {
      // res的类型
      console.log(res, typeof res);
        list.value.push(...res.data?.data || []);
      return res.data?.data || [];
    },
  },
);

onLoad(() => {
  list.value = [];
  reload();
});
// 页面下拉时触发，与 onLoad 等生命周期函数平级
onPullDownRefresh(() => {
  list.value = [];
  page.value = 1;
  reload();
  console.log('refresh');
});
onReachBottom(() => {
  if (page.value < Math.ceil((total.value || 0) / pageSize.value)) {
    // page.value += 1;
    page.value = page.value + 1;
  }

  console.log('触底了');
});
</script>

<template>
  <view>
    <text>{{ page }}</text>
    <text>{{ list.length }}</text>
    <wd-input v-model="keyWord" />
    <wd-cell-group>
      <wd-cell v-for="item, index in list" :key="index" vertical>
        <wd-text :text="item.key || ''" bold />
        <wd-text :text="item.zhCn || ''" />
        <wd-text :text="item.enUs || ''" />
      </wd-cell>
    </wd-cell-group>
    <wd-loadmore :state="loading ? 'loading' : 'finished'" />
    <!-- <web-view src="https://bear.js.org" /> -->
  </view>
</template>
