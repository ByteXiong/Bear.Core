<script setup lang="ts">
const pageQuery = ref<Record<string, any> | undefined>(undefined);
onLoad((query) => {
  pageQuery.value = query;
  console.log(pageQuery.value?.link);
});
const link = computed(() => {
  return pageQuery.value?.link;
});
</script>

<template>
  <view>
    <web-view :src="link" />
  </view>
</template>
