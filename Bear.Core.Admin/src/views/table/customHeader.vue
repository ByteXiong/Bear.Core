<script lang="ts" setup>
import { ref } from 'vue';
import type { TableColumnCtx } from 'element-plus';
import type { TableColumn } from '@/api/globals';
import { SearchComponent } from '@/api/apiEnumEls';
import { ConditionalType } from '@/api/sqlSugar';
defineOptions({ name: 'CustomHeader', inheritAttrs: false });
type Props = {
  column: Partial<TableColumnCtx<TableColumn>> & TableColumn & { checked?: boolean };
};
defineProps<Props>();
const search = ref<Record<string, any>>({});
const confirm = () => {
  alert('确定');
};
</script>

<template>
  <!-- 注意：逻辑部分尽量不好写到这个组件内，因为这个组件是根据外面table循环创建的，在这里写逻辑会非常影响性能 -->
  <div class="inline-block" @click.stop>
    <ElPopover
      placement="bottom"
      :title="'查询条件：' + $t(`sqlSugar.${ConditionalType[column.searchType]}`)"
      width="300"
      trigger="click"
    >
      <!-- 动态传入组件 -->
      <component
        :is="SearchComponent[column.columnType]"
        :ref="`${column?.prop}Ref`"
        v-model="search[column.prop || '']"
        :column="column"
        :placeholder="$t('common.placeholder') + $t(column.label || '')"
        v-bind="$attrs"
      ></component>
      <!-- confirm 确定框-->
      <div class="flex justify-end">
        <ElButton type="primary" class="confirm" @click="confirm">确定</ElButton>
      </div>
      <!-- label 标题显示-->
      <template #reference>
        <div>
          <IconIcRoundSearch />
          {{ $t(column.label) }}
        </div>
      </template>
    </ElPopover>
  </div>
</template>

<style scoped></style>
