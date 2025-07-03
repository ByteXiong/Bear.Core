<script setup lang="tsx">
import { ref } from 'vue';
import { useRoute } from 'vue-router';
import type { TableColumnCtx } from 'element-plus';
import { ElButton, ElPopconfirm, ElTag } from 'element-plus';
import { useRequest } from 'alova/client';
import '@/api';
import { VueDraggable } from 'vue-draggable-plus';
import type { MenuTreeDTO } from '@/api/globals';
import { MenuTypeEnum } from '@/api/apiEnums';
import createComponent from '@/utils/createComponent';
import { $t } from '@/locales';
import { MenuTypeEl } from '../../../api/apiEnumEls';
import EditForm from './modules/edit-form.vue';
const route = useRoute();
defineOptions({ name: 'MenuComponent' });

/** 获取数据 */
const {
  send: getData,
  data,
  loading
} = useRequest(
  () =>
    Apis.Menu.get_gettree({
      transform: res => {
        const forch = (item: MenuTreeDTO, parent?: MenuTreeDTO) => {
          item.path = `/${item.name}`;
          item.routeName = parent ? `${parent?.routeName}_${item.name}` : item.name;
          item.routePath = parent ? parent?.path + item?.path : item.path;
          if (item.children) {
            item.children.forEach((child: MenuTreeDTO) => {
              forch(child, item);
            });
          }
        };
        res.data.forEach((item: MenuTreeDTO) => {
          forch(item, undefined);
        });
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);
// 删除
const { send: handleDelete } = useRequest(
  ids =>
    Apis.Menu.delete_delete({
      data: ids,
      transform: res => {
        window.$message?.success('删除成功！');
        getData();
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

const checkedRowKeys = ref<string[]>([]);

const openForm = async (id?: string | null) => {
  const { instance, on, unmount } = await createComponent(EditForm, { visible: true });
  instance?.openForm(null, id);
  on('refresh', () => {
    getData();
  });

  on('update:visible', res => {
    if (!res) {
      unmount();
    }
  });
};

const handleAddChildMenu = async (row: MenuTreeDTO) => {
  const { instance, on, unmount } = await createComponent(EditForm, { visible: true });
  instance?.openForm(row.id, null);
  on('refresh', () => {
    getData();
  });

  on('update:visible', res => {
    if (!res) {
      unmount();
    }
  });
};

function onStart() {
  console.log('开始拖拽');
}

const columns = ref<UI.TableColumnCheck[] | (Partial<TableColumnCtx<MenuTreeDTO>> & { checked?: boolean })[]>([
  { type: 'selection', width: 48, checked: true, label: $t('table.selection') },
  { prop: 'routeName', label: $t('menu.routeName'), checked: true, minWidth: 120 },
  {
    prop: 'menuType',
    label: $t('menu.menuType'),
    width: 90,
    checked: true,
    formatter: row => {
      return <ElTag type={MenuTypeEl[row.menuType]}>{MenuTypeEnum[row.menuType]}</ElTag>;
    }
  },

  { prop: 'path', label: $t('menu.routePath'), checked: true, minWidth: 120 },
  {
    prop: 'status',
    label: $t('menu.menuStatus'),
    width: 80,
    checked: true,
    formatter: row => {
      if (row.status === undefined) {
        return '';
      }

      return <ElTag type={row.status ? 'success' : 'danger'}>{row.status ? '启用' : '禁用'}</ElTag>;
    }
  },
  {
    prop: 'hideInMenu',
    label: $t('menu.hideInMenu'),
    width: 80,
    checked: true,
    formatter: row => {
      return <ElTag type={row.hideInMenu ? 'danger' : 'success'}>{row.hideInMenu ? '是' : '否'}</ElTag>;
    }
  },
  { prop: 'order', label: $t('menu.order'), checked: true, width: 60 },
  {
    prop: 'use',
    label: '操作',
    align: 'center',
    checked: true,
    width: 230,
    formatter: row => (
      <div class="flex-center justify-end pr-10px">
        {row.menuType === MenuTypeEnum.Directory ? (
          <ElButton type="primary" plain size="small" onClick={() => handleAddChildMenu(row)}>
            {$t('menu.addChildren')}
          </ElButton>
        ) : null}
        {row.menuType === MenuTypeEnum.Directory || row.menuType === MenuTypeEnum.Menu ? (
          <ElButton type="primary" plain size="small" onClick={() => openForm(row.id)}>
            {$t('common.edit')}
          </ElButton>
        ) : null}

        <ElPopconfirm title={$t('common.confirmDelete')} onConfirm={() => handleDelete([row.id])}>
          {{
            reference: () => (
              <ElButton type="danger" plain size="small">
                {$t('common.delete')}
              </ElButton>
            )
          }}
        </ElPopconfirm>
      </div>
    )
  }
]);
</script>

<template>
  <div class="min-h-500px flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <ElCard class="sm:flex-1-hidden card-wrapper" body-class="ht50">
      <template #header>
        <div class="flex items-center justify-between">
          <p>{{ $t(route.meta.i18nKey || route.meta.title || '') }}</p>

          <TableHeaderOperation
            v-model:columns="columns"
            :disabled-delete="checkedRowKeys.length === 0"
            :loading="loading"
            @refresh="getData"
          >
            <!-- v-if="hasAuth(dbConfig.addUrl)" -->
            <ElButton ghost type="primary" @click="openForm(null)">
              <template #icon>
                <icon-ic-round-plus class="text-icon" />
              </template>
              {{ $t('common.add') }}
            </ElButton>
            <!--       -->
            <ElPopconfirm :title="$t('common.confirmDelete')" @confirm="handleDelete(checkedRowKeys)">
              <template #reference>
                <ElButton type="danger" plain :disabled="checkedRowKeys.length == 0">
                  <template #icon>
                    <icon-ic-round-delete class="text-icon" />
                  </template>
                  {{ $t('common.batchDelete') }}
                </ElButton>
              </template>
            </ElPopconfirm>
          </TableHeaderOperation>
        </div>
      </template>
      <div class="h-[calc(100%-50px)]">
        <VueDraggable
          v-model="data"
          class="h-[calc(100%-50px)]"
          target="tbody"
          filter=".handle"
          :animation="150"
          @update="onStart"
        >
          <ElTable
            v-loading="loading"
            height="100%"
            border
            class="sm:h-full"
            sortable
            :data="data"
            row-key="id"
            default-expand-all
          >
            <ElTableColumn v-for="(col, index) in columns.filter(item => item.checked)" :key="index" v-bind="col" />
          </ElTable>
        </VueDraggable>
      </div>
    </ElCard>
  </div>
</template>

<style scoped></style>
