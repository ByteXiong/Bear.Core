<script setup lang="tsx">
import { ref } from 'vue';
import "@/api"
import { useForm, usePagination } from '@sa/alova/client';
import { ElButton, ElInput, Sort, TableColumnCtx } from 'element-plus';
import { I18nDTO, UpdateI18nParam } from '@/api/globals';
import { $t } from '@/locales';
import { useClipboard } from '@vueuse/core';
const { copy, isSupported } = useClipboard();
const visible = defineModel<boolean>('visible', {
  required: true,
  default: false
});

type  DataType= I18nDTO & { isEdit:boolean }
const keyWord = ref('')
const sortList = ref<Record<string, string>>({ id: "asc" })
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
  Apis.I18n.get_getpage({
      params: {
        PageIndex: upPageIndex,
        PageSize: upPageSize,
        SortList: sortList.value,
        Search: {},
        StartIndex: 0
      }
    }),
  {
    watchingStates: [keyWord, sortList],
    force: true,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 20, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: res => res.data?.pagerInfo?.totalRowCount,
    data: res => (res.data?.data) || []
  }
);



/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  (form,row) =>
    (row?.id?Apis.I18n.put_update : Apis.I18n.post_add)({
      data: row,
      transform: () => {
        row.isEdit =false
        window.$message?.success($t('common.updateSuccess'));
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
    } as UpdateI18nParam,
    // async middleware(_, next) {
    //   // validate().then(async () => {
    //   //   await next();
    //   // });
    // }
  }
);



const columns= ref<Array<Partial<TableColumnCtx<DataType>>>>([
  {
    prop: 'key',
    label: "翻译键",
    align: 'center',
    width: 100,
    formatter: row => {
      return (
        <div>
      {
          !row.isEdit?  <el-text  onDblclick={()=>handleCopy(row)}>{row.key}</el-text>:
          <div> <ElInput v-model={row.key} placeholder={$t('common.placeholder')}/> </div>

        }
        </div>
      )
    }
  },
  {
    prop: 'zhCn',
    label: "中文",
    align: 'center',
    formatter: row => {
      return (
        <div >
      {
          !row.isEdit?  <el-text onDblclick={()=>handleCopy(row)}>{row.zhCn}</el-text>:
          <div ><ElInput v-model={row.zhCn} placeholder={$t('common.placeholder')}/> </div>

        }
        </div>
      )
    }
  },
  {
    prop: 'enUs',
    label: "英语",
    align: 'center',
    formatter: row => {
      return (
      <div onDblclick={()=>handleCopy(row)}>
      {
          !row.isEdit?  <el-text onDblclick={()=>handleCopy(row)}>{row.enUs}</el-text>:
          <div ><ElInput v-model={row.enUs} placeholder={$t('common.placeholder')}/> </div>
        }
        </div>
      )
    }
  },
  {
    prop: "use",
    label: "操作",
    align: "center",
    formatter: (row) => (
      <div class="flex-center gap-8px">{
        !row.isEdit?  <ElButton
          type="primary"
          plain
          size="small"
          onClick={() => { row.isEdit = !row.isEdit}}
        >
        编辑
        </ElButton>
:
        <ElButton
          type="primary"
          plain
          size="small"
          onClick={() => {submit(row)}}
        >
          保存
        </ElButton>
      }

      </div>
    ),
  }
])
const sortChange = ({ prop, order }: Sort) => {
  sortList.value = {};
  sortList.value[prop] = order?.replace('ending', '');
};


const submit =(row:DataType )=> {
//  updateForm({...row})
 handleSubmit(row)
}
const openAdd=  () => {
 data.value.unshift({
   id: "",
   key: "",
   zhCn: "",
   enUs: "",
   source: "",
   count: 0,
   isEdit: true
 } as DataType);
}


async function handleCopy(row:DataType) {
  row.isEdit = false;
  if (!isSupported) {
    window.$message?.error('您的浏览器不支持Clipboard API');
    return;
  }

  if (!row.key) {
    window.$message?.error('请输入要复制的内容');
    return;
  }

  await copy(row.key);
  window.$message?.success(`复制成功：${row.key}`);
}

</script>


<template>
  <el-drawer v-model="visible" >
  <template #header>
    <h4>翻译列表</h4>
    <div class="flex items-center justify-between">
      <el-button  type="primary"  @click="openAdd" >{{$t("common.add")}}</el-button>
    </div>
  </template>
  <template #default>

    <div class="h-[calc(100%-50px)]">
      <ElTable
        v-loading="loading"
        height="100%"
        border
        class="sm:h-full"
        sortable
        :data="data"
        row-key="id"
        @sort-change="sortChange"
      >
        <ElTableColumn
          v-for="(col, index) in columns"
          :key="index"
          v-bind="col"
        />
      </ElTable>
    </div>


    <div class="mt-20px flex justify-end">
      <ElPagination
        layout="total,prev,pager,next,sizes"
        :current-page="page"
        :total="total"
        :page-size="pageSize"
        :page-sizes="[...new Set([15, 50, 200, 500, Math.min(total || 1000, 1000)])].sort((a, b) => a - b)"
        @current-change="val => page = val"
        @size-change="val => {
          pageSize = val;
          page = 1;
        }"
      />
      <!-- 删除重复的 ElPagination 组件 -->
    </div>
  </template>
  <template #footer>
    <div style="flex: auto">
      <el-button @click="visible=false">{{$t("common.cancel")}}</el-button>
    </div>
  </template>
</el-drawer>
</template>

