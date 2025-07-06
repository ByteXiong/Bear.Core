import type { ComponentApi } from '.';

const ElInputConfig: ComponentApi = {
  component: 'ElInput',
  label: '输入框',
  data: [
    {
      prop: 'type',
      label: '输入框类型',
      component: 'ElSelect',
      placeholder: '请选择类型',
      options: [
        { label: 'text', value: 'text' },
        { label: 'textarea', value: 'textarea' },
        { label: 'password', value: 'password' },
        { label: 'number', value: 'number' }
      ]
    },
    {
      prop: 'placeholder',
      label: '占位文本',
      component: 'ElInput',
      placeholder: '请输入占位文本'
    },
    {
      prop: 'maxlength',
      label: '最大长度',
      component: 'ElInputNumber',
      placeholder: '请输入最大长度'
    },
    {
      prop: 'minlength',
      label: '最小长度',
      component: 'ElInputNumber',
      placeholder: '请输入最小长度'
    },
    {
      prop: 'showWordLimit',
      label: '显示字数统计',
      component: 'ElSelect',
      placeholder: '是否显示输入字数统计',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'clearable',
      label: '可清空',
      component: 'ElSelect',
      placeholder: '是否可清空',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'showPassword',
      label: '显示密码切换',
      component: 'ElSelect',
      placeholder: '是否显示切换密码图标',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'disabled',
      label: '禁用状态',
      component: 'ElSelect',
      placeholder: '是否禁用',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'size',
      label: '输入框尺寸',
      component: 'ElSelect',
      placeholder: '输入框尺寸',
      options: [
        { label: 'large', value: 'large' },
        { label: 'default', value: 'default' },
        { label: 'small', value: 'small' }
      ]
    },
    {
      prop: 'prefixIcon',
      label: '头部图标',
      component: 'ElInput',
      placeholder: '输入框头部图标'
    },
    {
      prop: 'suffixIcon',
      label: '尾部图标',
      component: 'ElInput',
      placeholder: '输入框尾部图标'
    },
    {
      prop: 'rows',
      label: '文本域行数',
      component: 'ElInputNumber',
      placeholder: 'textarea 行数'
    },
    {
      prop: 'autosize',
      label: '高度自适应',
      component: 'ElSelect',
      placeholder: 'textarea 高度自适应',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'readonly',
      label: '只读状态',
      component: 'ElSelect',
      placeholder: '是否只读',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'modelValue',
      label: '绑定值',
      component: 'ElInput',
      placeholder: '输入框的值'
    },
    {
      prop: 'formId',
      label: '表单ID',
      component: 'ElInput',
      placeholder: '表单对象ID'
    },
    {
      prop: 'formItem',
      label: '表单项',
      component: 'ElInput',
      placeholder: '表单项对象'
    },
    {
      prop: 'validateEvent',
      label: '验证事件',
      component: 'ElSelect',
      placeholder: '是否触发表单验证',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'inputStyle',
      label: '输入框样式',
      component: 'ElInput',
      placeholder: '输入框的样式'
    },
    {
      prop: 'resize',
      label: '调整大小',
      component: 'ElSelect',
      placeholder: '控制是否能被用户缩放',
      options: [
        { label: 'none', value: 'none' },
        { label: 'both', value: 'both' },
        { label: 'horizontal', value: 'horizontal' },
        { label: 'vertical', value: 'vertical' }
      ]
    },
    {
      prop: 'autocomplete',
      label: '自动完成',
      component: 'ElSelect',
      placeholder: '原生 autocomplete 属性',
      options: [
        { label: 'on', value: 'on' },
        { label: 'off', value: 'off' }
      ]
    },
    {
      prop: 'name',
      label: '原生属性',
      component: 'ElInput',
      placeholder: '原生 name 属性'
    },
    {
      prop: 'id',
      label: '原生ID',
      component: 'ElInput',
      placeholder: '原生 id 属性'
    },
    {
      prop: 'label',
      label: '标签文本',
      component: 'ElInput',
      placeholder: '标签文本'
    },
    {
      prop: 'tabindex',
      label: 'Tab索引',
      component: 'ElInputNumber',
      placeholder: '原生 tabindex 属性'
    },
    {
      prop: 'step',
      label: '步长',
      component: 'ElInputNumber',
      placeholder: '原生 step 属性'
    },
    {
      prop: 'min',
      label: '最小值',
      component: 'ElInputNumber',
      placeholder: '原生 min 属性'
    },
    {
      prop: 'max',
      label: '最大值',
      component: 'ElInputNumber',
      placeholder: '原生 max 属性'
    },
    {
      prop: 'spellcheck',
      label: '拼写检查',
      component: 'ElSelect',
      placeholder: '原生 spellcheck 属性',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'wrap',
      label: '换行',
      component: 'ElSelect',
      placeholder: '原生 wrap 属性',
      options: [
        { label: 'soft', value: 'soft' },
        { label: 'hard', value: 'hard' }
      ]
    },
    {
      prop: 'autofocus',
      label: '自动聚焦',
      component: 'ElSelect',
      placeholder: '原生 autofocus 属性',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'form',
      label: '表单属性',
      component: 'ElInput',
      placeholder: '原生 form 属性'
    },
    {
      prop: 'list',
      label: '数据列表',
      component: 'ElInput',
      placeholder: '原生 list 属性'
    },
    {
      prop: 'pattern',
      label: '正则表达式',
      component: 'ElInput',
      placeholder: '原生 pattern 属性'
    },
    {
      prop: 'required',
      label: '必填',
      component: 'ElSelect',
      placeholder: '原生 required 属性',
      options: [
        { label: '是', value: true },
        { label: '否', value: false }
      ]
    },
    {
      prop: 'title',
      label: '标题',
      component: 'ElInput',
      placeholder: '原生 title 属性'
    },
    {
      prop: 'enterkeyhint',
      label: '回车键提示',
      component: 'ElSelect',
      placeholder: '原生 enterkeyhint 属性',
      options: [
        { label: 'enter', value: 'enter' },
        { label: 'done', value: 'done' },
        { label: 'go', value: 'go' },
        { label: 'next', value: 'next' },
        { label: 'previous', value: 'previous' },
        { label: 'search', value: 'search' },
        { label: 'send', value: 'send' }
      ]
    },
    {
      prop: 'inputmode',
      label: '输入模式',
      component: 'ElSelect',
      placeholder: '原生 inputmode 属性',
      options: [
        { label: 'none', value: 'none' },
        { label: 'text', value: 'text' },
        { label: 'tel', value: 'tel' },
        { label: 'url', value: 'url' },
        { label: 'email', value: 'email' },
        { label: 'numeric', value: 'numeric' },
        { label: 'decimal', value: 'decimal' },
        { label: 'search', value: 'search' }
      ]
    }
  ],
  events: [
    {
      event: 'update:modelValue',
      label: '值更新事件',
      description: '当输入框的值发生变化时触发'
    },
    {
      event: 'input',
      label: '输入事件',
      description: '当输入框内容发生变化时触发'
    },
    {
      event: 'change',
      label: '值改变事件',
      description: '当输入框失去焦点且值发生变化时触发'
    },
    {
      event: 'focus',
      label: '获得焦点事件',
      description: '当输入框获得焦点时触发'
    },
    {
      event: 'blur',
      label: '失去焦点事件',
      description: '当输入框失去焦点时触发'
    },
    {
      event: 'clear',
      label: '清空事件',
      description: '当用户点击清空按钮时触发'
    },
    {
      event: 'compositionstart',
      label: '输入法开始事件',
      description: '当输入法编辑器开始新的输入合成时触发'
    },
    {
      event: 'compositionupdate',
      label: '输入法更新事件',
      description: '当输入法编辑器输入合成更新时触发'
    },
    {
      event: 'compositionend',
      label: '输入法结束事件',
      description: '当输入法编辑器完成输入合成时触发'
    },
    {
      event: 'keydown',
      label: '按键按下事件',
      description: '当按下键盘按键时触发'
    },
    {
      event: 'keyup',
      label: '按键释放事件',
      description: '当释放键盘按键时触发'
    },
    {
      event: 'keypress',
      label: '按键事件',
      description: '当按下有字符值的按键时触发'
    },
    {
      event: 'paste',
      label: '粘贴事件',
      description: '当用户粘贴内容时触发'
    },
    {
      event: 'cut',
      label: '剪切事件',
      description: '当用户剪切内容时触发'
    },
    {
      event: 'copy',
      label: '复制事件',
      description: '当用户复制内容时触发'
    },
    {
      event: 'select',
      label: '选择事件',
      description: '当用户选择文本时触发'
    },
    {
      event: 'selectstart',
      label: '开始选择事件',
      description: '当用户开始选择文本时触发'
    },
    {
      event: 'scroll',
      label: '滚动事件',
      description: '当输入框内容滚动时触发'
    },
    {
      event: 'resize',
      label: '大小改变事件',
      description: '当输入框大小发生变化时触发'
    },
    {
      event: 'invalid',
      label: '无效输入事件',
      description: '当输入框验证失败时触发'
    },
    {
      event: 'search',
      label: '搜索事件',
      description: '当用户点击搜索按钮时触发'
    },
    {
      event: 'enter',
      label: '回车事件',
      description: '当用户按下回车键时触发'
    }
  ]
};

export default ElInputConfig;
