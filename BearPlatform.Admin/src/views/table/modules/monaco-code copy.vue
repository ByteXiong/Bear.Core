<script setup lang="tsx">
import type * as monaco from 'monaco-editor';
import { ref } from 'vue';
import { $t } from '@/locales';
import customRender from '@/utils/customRender';
const language = ref('javascript');
interface Props {
  loading?: boolean;
  code?: string | null;
}

const { code } = defineProps<Props>();

interface Emits {
  (e: 'Change', value: string): string;
}
const value = ref<string>('');
const emit = defineEmits<Emits>();
const columns = ref<Array<any>>([
  {
    type: 'selection',
    align: 'center'
  },
  {
    key: 'name',
    title: 'name',
    align: 'center'
    // render: row => {
    //   return h('p', Enum.MenuType[row.id]);
    // }
  }
]);

const editorMounted = (editor: monaco.editor.IStandaloneCodeEditor) => {
  editor.onDidChangeModelContent(() => {
    // const value = editor.getValue();
    // columns.value[2] = customRender(value || '', h, naive)[0];
  });
};
const visible = ref(false);
const title = ref('插槽编辑');

const handleSubmit = () => {
  visible.value = false;
  console.log(value.value);
  emit('Change', value.value);
};

const closeModal = () => {
  visible.value = false;
};
const handClick = () => {
  visible.value = true;
  value.value = code || ``;
};
const handlTest = () => {
  try {
    columns.value[2] = customRender(value.value);
  } catch (e) {
    console.error(e);
  }
};

const tableData = ref([
  {
    id: 1,
    name: '张三'
  },
  {
    id: 2,
    name: '李四'
  }
]);
</script>

<template>
  <ElButton type="primary" :ghost="!code || code.length == 0" :loading="loading" plain @click="handClick" >重写插槽</ElButton>
  <ElDialog v-model="visible" :title="title" :append-to-body="true" class="w-1400px">
    <ElRow :gutter="24">
      <ElCol :span="8">
        {{ columns }}
        <!--
 <NDataTable
          :columns="columns"
          :data="tableData"
          size="small"
          remote
          :row-key="row => row.id"
          :loading="loading"
        />
-->
      </ElCol>
      <ElCol :span="16">
        <!-- <ElScrollbar class="h-650px pr-20px"> -->
        <MonacoEditor
          v-model:value="value"
          :language="language"
          width="100%"
          height="650px"
          @editor-mounted="editorMounted"
        ></MonacoEditor>
        <!-- </ElScrollbar> -->
      </ElCol>
    </ElRow>

    <template #footer>
      <ElSpace :size="16" class="float-right">
        <ElButton type="primary" :loading="loading" @click="handlTest">{{ $t('测试代码') }}</ElButton>

        <ElButton @click="closeModal">{{ $t('common.cancel') }}</ElButton>
        <ElButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</ElButton>
      </ElSpace>
    </template>
  </ElDialog>
</template>

<style lang="less" scoped></style>
