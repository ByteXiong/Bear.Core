<script lang="ts" setup>
import printJS from 'print-js';

defineOptions({ name: 'PrintPage' });

function printTable() {
  printJS({
    printable: [
      { name: 'Bear', wechat: 'honghuangdc', remark: '欢迎来技术交流' },
      { name: 'Bear', wechat: 'honghuangdc', remark: '欢迎来技术交流' }
    ],
    properties: ['name', 'wechat', 'remark'],
    type: 'json'
  });
}
function printImage() {
  printJS({
    printable: [
      'https://i.loli.net/2021/11/24/1J6REWXiHomU2kM.jpg',
      'https://i.loli.net/2021/11/24/1J6REWXiHomU2kM.jpg'
    ],
    type: 'image',
    header: 'Multiple Images',
    imageStyle: 'width:100%;'
  });
}
</script>

<template>
  <div class="h-full">
    <ElCard header="打印" class="card-wrapper">
      <ElButton type="primary" class="mr-10px" @click="printTable">打印表格</ElButton>
      <ElButton type="primary" @click="printImage">打印图片</ElButton>
      <template #footer>
        <GithubLink label="printJS：" link="https://github.com/crabbly/Print.js" class="mt-10px" />
      </template>
    </ElCard>
  </div>
</template>

<style scoped></style>
