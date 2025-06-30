<script setup lang="tsx">
import type * as monaco from 'monaco-editor';
import { ref } from 'vue';
import { $t } from '@/locales';
import customRender from '@/utils/customRender';
const language = ref('javascript');

// 根据提示，将 defineModelValue 替换为 defineModel
const value = defineModel<string>({
  default: ''
});
const visible =defineModel<boolean>('visible', {
  default: false
})
interface Emits {
  (e: 'Change', value: string): void;

}
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
const title = ref('插槽编辑');

const handleSubmit = () => {
  emit('Change', value.value);
  visible.value = false;

};
const handleClose = () => {
  visible.value = false;
};
</script>

<template>
  <ElDialog v-model="visible" :title="title" :append-to-body="true" class="w-1400px" >
    <ElRow :gutter="24">
      <ElCol :span="24">
        <MonacoEditor
          v-bind="$attrs"
          v-model:value="value"
          :language="language"
          width="100%"
          height="650px"
          @editor-mounted="editorMounted"
        ></MonacoEditor>
      </ElCol>
    </ElRow>

    <template #footer>
      <ElSpace :size="16" class="float-right">
        <ElButton @click="handleClose">{{ $t('common.cancel') }}</ElButton>
        <ElButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</ElButton>
      </ElSpace>
    </template>
  </ElDialog>
</template>

<style lang="less" scoped></style>
