<script setup lang="tsx">
import { computed, ref } from 'vue';
import { ElOption, ElSelect } from 'element-plus';
import { $t } from '@/locales';
import { componentsConfig } from '../types';
import type { ComponentApi } from '../types';
defineOptions({ name: 'DynamicComponent', inheritAttrs: false });
const modelValue = defineModel<ComponentApi>('modelValue', {
  default: () =>
    ({
      component: 'ElInput',
      label: '输入框'
    }) as ComponentApi
});

const visible = defineModel<boolean>('visible', {
  default: false
});

// 定义事件
interface Emits {
  (e: 'Change', value: ComponentApi): void;
}
const emit = defineEmits<Emits>();

// 当前选择的组件名称
const currentComponent = ref('ElInput');

// 可用组件列表
const availableComponents = computed(() => {
  return Object.keys(componentsConfig);
});

// 当前组件的配置
const currentComponentApi = computed(() => {
  return componentsConfig[currentComponent.value] || null;
});

// 组件属性值
// const componentProps = ref<Record<string, any>>({});

// // 解析已有的值
// const parseExistingValue = () => {
//   if (!value.value) return;

//   try {
//     const parsed = JSON.parse(value.value);
//     if (parsed.component && componentsConfig[parsed.component]) {
//       currentComponent.value = parsed.component;
//       componentProps.value = parsed.props || {};
//     }
//   } catch (e) {}
// };

// // 更新组件值
// const updateComponentValue = () => {
//   const result = {
//     component: currentComponent.value,
//     props: componentProps.value
//   };
//   modelValue.value = JSON.stringify(result, null, 2);
// };

// // 监听组件变化
// watch(
//   currentComponent,
//   () => {
//     componentProps.value = {};
//     updateComponentValue();
//   },
//   { immediate: true }
// );

// 渲染实时预览组件
// const renderPreviewComponent = () => {
//   const componentName = currentComponent.value;
//   const props = componentProps.value;

//   switch (componentName) {
//     case 'ElInput':
//       return <ElInput {...props} />;
//     case 'ElSelect':
//       return (
//         <ElSelect {...props}>
//           <ElOption label="选项1" value="1" />
//           <ElOption label="选项2" value="2" />
//         </ElSelect>
//       );
//     case 'ElSwitch':
//       return <el-switch {...props} />;
//     default:
//       return <div>暂不支持预览该组件</div>;
//   }
// };
// // 初始化
// watch(
//   () => visible.value,
//   newValue => {
//     if (newValue) {
//       parseExistingValue();
//     }
//   },
//   { immediate: true }
// );

// 提交和关闭
const handleSubmit = () => {
  // updateComponentValue();
  emit('Change', modelValue.value);
  visible.value = false;
};

const handleClose = () => {
  visible.value = false;
};
</script>

<template>
  <ElDialog v-model="visible" :title="$t('插槽编辑')" :append-to-body="true" class="w-1400px">
    <ElRow :gutter="24">
      <!-- 左侧预览 -->
      <ElCol :span="12">
        <ElCard header="组件预览" shadow="hover" class="preview-card">
          <div class="preview-container">
            <component :is="modelValue.component" :common="modelValue.data" />
          </div>
          <div class="code-container mt-4">
            <pre>{{ modelValue }}</pre>
          </div>
        </ElCard>
      </ElCol>

      <!-- 右侧配置 -->
      <ElCol :span="12">
        <ElCard header="组件配置" shadow="hover">
          <ElForm :model="modelValue">
            <!-- 组件选择器 -->
            <ElFormItem label="选择组件">
              <ElSelect v-model="currentComponent" placeholder="请选择组件" class="w-full">
                <ElOption v-for="comp in availableComponents" :key="comp" :label="comp" :value="comp" />
              </ElSelect>
            </ElFormItem>

            <!-- 组件属性配置 -->
            <template v-if="currentComponentApi">
              <ElDivider>属性配置</ElDivider>
              <ElForm label-position="top">
                <ElFormItem v-for="item in currentComponentApi.attributes" :key="item.prop" :label="item.description">
                  {{ item.component }}
                  <component
                    :is="item.component"
                    v-model="modelValue.attributes[item.prop + '']"
                    :column="item"
                    :placeholder="$t('common.placeholder') + $t(item.description)"
                    v-bind="$attrs"
                  ></component>

                  <!-- <component :is="item.component" :column="item" /> -->
                </ElFormItem>
              </ElForm>
            </template>
          </ElForm>
        </ElCard>
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

<style lang="scss" scoped>
.preview-card {
  height: 100%;

  .preview-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 200px;
    padding: 20px;
    border: 1px dashed #dcdfe6;
    border-radius: 4px;
    background-color: #f8f9fa;
  }

  .code-container {
    padding: 10px;
    border-radius: 4px;
    background-color: #f5f7fa;
    overflow: auto;
    max-height: 200px;

    pre {
      margin: 0;
      font-family: monospace;
      white-space: pre-wrap;
    }
  }
}
</style>
