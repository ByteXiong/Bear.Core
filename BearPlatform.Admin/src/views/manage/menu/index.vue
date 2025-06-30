<script setup lang="tsx">
import { computed, ref } from "vue";
import { useRequest } from "alova/client";
import { useRoute } from "vue-router";
import { useAppStore } from "@/store/modules/app";
import { $t } from "@/locales";
import "@/api";
import { VueDraggable } from 'vue-draggable-plus';
import type { MenuTreeDTO, TableColumn, TableView } from "@/api/globals";
import { IconType, MenuType, ViewTypeEnum } from "@/api/apiEnums";
import customRender from "@/utils/customRender";
import { useAuth } from "@/hooks/business/auth";
import EditForm from "./modules/edit-form.vue";
import "@/api";
import SvgIcon from "@/components/custom/svg-icon.vue";
import {
  ElButton,
  ElPopconfirm,
  ElTag,
  Sort,
  TableColumnCtx,
} from "element-plus";
const route = useRoute();
const configId = ref<string>("");
const router = route.path;
defineOptions({ name: "Menu" });

const  tableof= ref<string>("MenuTreeDTO");

const loadChange = (table: TableView) => {
  // tableView.value = table;
  columns.value =
    table?.columns
      ?.filter(item => item.isShow)
      .map(item => {
        return { ...item, checked: true } as UI.TableColumnCheck;
      }) || [];
columns.value.push(   {
    prop: "sort",
    label: "排序",
    width: 60,
    formatter: row => {
      return (
        <div class="handle">
          <icon-mdi-drag class="mr-8px h-full cursor-move text-icon" />
          {row.order}
        </div>
      );
    }
  })
    columns.value.push(  {
    prop: "use",
    label: "操作",
    align: "center",
    checked: true,
    width:230,
    formatter: (row) => (
      <div class="flex-center justify-end pr-10px">
       { row.menuType === MenuType.Directory?
        <ElButton
          type="primary"
          plain
          size="small"
          onClick={() => handleAddChildMenu(row)}
        >
          添加子菜单
        </ElButton>:null
      }
      { row.menuType === MenuType.Directory||  row.menuType === MenuType.Menu ?
        <ElButton
          type="primary"
          plain
          size="small"
          onClick={() => openForm(row.id)}
        >
          编辑
        </ElButton>:null
      }

        <ElPopconfirm
          title={$t("common.confirmDelete")}
          onConfirm={() => handleDelete([row.id])}
        >
          {{
            reference: () => (
              <ElButton type="danger" plain size="small">
                {$t("common.delete")}
              </ElButton>
            ),
          }}
        </ElPopconfirm>
      </div>
    ),
  });

  // if (dataTableConfig.value) dataTableConfig.value.sortList[table?.sortKey || 'id'] = table?.sortOrder || 'desc';
  // searchData.value = res.data?.tableColumns?.filter(item => item.searchType !== null) || [];
  // emit('loadChange', table);
};

const columns = ref<
  Array<Partial<TableColumnCtx<MenuTreeDTO>> & { checked?: boolean }>
>([
  // {
  //   type: "selection",
  //   label: "多选",
  //   align: "center",
  //   checked: true,
  //   width: 50,
  // },
  // {
  //   prop: "menuType",
  //   label: "菜单类型",
  //   align: "center",
  //   checked: true,
  //   width: 100,
  //   formatter: row => {
  //       const tagMap: Record<MenuType, UI.ThemeColor> = {
  //         [MenuType.Menu]: 'success',
  //         [MenuType.Directory] : 'primary',
  //         [MenuType.Button]: "warning",
  //         [MenuType.Query]: "danger"
  //       };
  //       return <ElTag type={tagMap[row.menuType]}>{MenuType[row.menuType]}</ElTag>;
  //     }
  // },
  // {
  //   prop: "title",
  //   label: "菜单名称",
  //   align: "center",
  //   checked: true,
  //   width: 150,
  //   formatter: row =>{
  //     return $t(row.i18nKey ||row.title ||"" );
  //   }
  // },
  // {
  //   prop: "iconType",
  //   label: $t("page.manage.menu.icon"),
  //   width: 100,
  //   formatter: (row) => {
  //     const icon = row.iconType === IconType.iconify图标 ? row.icon : undefined;

  //     const localIcon =
  //       row.iconType === IconType.本地图标 ? row.icon : undefined;

  //     return (
  //       <div class="flex-center">
  //         <SvgIcon icon={icon} localIcon={localIcon} class="text-icon" />
  //       </div>
  //     );
  //   },
  // },
  // { prop: "name", label: $t("page.manage.menu.routeName"), minWidth: 120 },
  // { prop: "path", label: $t("page.manage.menu.routePath"), minWidth: 120 },
  // {
  //   prop: "status",
  //   label: $t("page.manage.menu.menuStatus"),
  //   width: 80,
  //   formatter: (row) => {
  //     if (row.status) {
  //       return <ElTag type="success">启用</ElTag>;
  //     } else {
  //       return <ElTag type="warning">禁用</ElTag>;
  //     }
  //   },
  // },
  // // {
  // //   prop: 'hideInMenu',
  // //   label: $t('page.manage.menu.hideInMenu'),
  // //   width: 80,
  // //   formatter: row => {
  // //     const hide: CommonType.YesOrNo = row.hideInMenu ? 'Y' : 'N';

  // //     const tagMap: Record<CommonType.YesOrNo, UI.ThemeColor> = {
  // //       Y: 'danger',
  // //       N: 'info'
  // //     };

  // //     const label = $t(yesOrNoRecord[hide]);

  // //     return <ElTag type={tagMap[hide]}>{label}</ElTag>;
  // //   }
  // // },
  // // { prop: 'parentId', label: $t('page.manage.menu.parentId'), width: 90 },
  // { prop: "order", label: $t("page.manage.menu.order"), width: 60 },
  // {
  //   prop: "use",
  //   label: "操作",
  //   align: "center",
  //   checked: true,
  //   formatter: (row) => (
  //     <div class="flex-center gap-8px">
  //      { row.menuType === MenuType.Directory?
  //       <ElButton
  //         type="primary"
  //         plain
  //         size="small"
  //         onClick={() => handleAddChildMenu(row)}
  //       >
  //         添加子菜单
  //       </ElButton>:null
  //     }
  //     { row.menuType === MenuType.Directory||  row.menuType === MenuType.Menu ?
  //       <ElButton
  //         type="primary"
  //         plain
  //         size="small"
  //         onClick={() => openForm(row.id)}
  //       >
  //         编辑
  //       </ElButton>:null
  //     }

  //       <ElPopconfirm
  //         title={$t("common.confirmDelete")}
  //         onConfirm={() => handleDelete([row.id])}
  //       >
  //         {{
  //           reference: () => (
  //             <ElButton type="danger" plain size="small">
  //               {$t("common.delete")}
  //             </ElButton>
  //           ),
  //         }}
  //       </ElPopconfirm>
  //     </div>
  //   ),
  // },
]);
/** 获取数据 */
const {
  send: getData,
  data,
  loading,
} = useRequest(
  () =>
    Apis.Menu.get_gettree({
      transform: (res) => {
        return res.data;
      },
    }),
  {
    force: true,
    immediate: true,
  },
);
// 删除
const { send: handleDelete } = useRequest(
  (ids) =>
    Apis.Menu.delete_delete({
      data: ids,
      transform: (res) => {
        window.$message?.success("删除成功！");
        getData();
        return res.data;
      },
    }),
  { force: true, immediate: false },
);

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

const checkedRowKeys = ref<string[]>([]);

// 打开编辑/新增
const editFormRef = ref();
const openForm = (id?: string | null) => {
  editFormRef.value?.openForm(null, id);
};

const handleAddChildMenu = (row: MenuTreeDTO) => {
  editFormRef.value?.openForm(row.id, null);
};

function onStart() {
  console.log('开始拖拽')
}


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
          <template #prefix>
            <TableViewSetting
              :config-id="configId"
              :tableof="tableof"
              :router="router"
              @load-change="loadChange"
            ></TableViewSetting>
          </template>
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
      class="h-[calc(100%-50px)]"
        target="tbody"
        filter=".handle"
        :animation="150"
        @update="onStart"
        v-model="data"
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
        <ElTableColumn
          v-for="(col, index) in columns"
          :key="index"
          v-bind="col"
        />
      </ElTable>
      </VueDraggable>
    </div>
    <EditForm ref="editFormRef" @refresh="getData" />
  </ElCard>

</div>
</template>

<style scoped></style>
