<script setup lang="ts">
import type { TableColumnCtx } from 'element-plus';
import { $t } from '@/locales';

defineOptions({ name: 'TableHeaderOperation' });

interface Props {
  loading?: boolean;
}

defineProps<Props>();

interface Emits {
  (e: 'refresh'): void;
}

const emit = defineEmits<Emits>();

const columns = defineModel<UI.TableColumnCheck[] | (Partial<TableColumnCtx<any>> & { checked?: boolean })[]>(
  'columns',
  {
    default: () => []
  }
);

function refresh() {
  emit('refresh');
}
</script>

<template>
  <ElSpace direction="horizontal" wrap justify="end" class="lt-sm:w-200px">
    <slot name="prefix"></slot>
    <slot name="default"></slot>
    <ElButton @click="refresh">
      <template #icon>
        <icon-mdi-refresh class="text-icon" :class="{ 'animate-spin': loading }" />
      </template>
      {{ $t('common.refresh') }}
    </ElButton>
    <TableColumnSetting v-model:columns="columns" />
    <slot name="suffix"></slot>
  </ElSpace>
</template>

<style scoped></style>
