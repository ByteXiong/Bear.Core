<script lang="ts" setup>
/**
 * @name Iconify
 * @desc 图标组件 使用 iconify + tailwindcss
 * 更多图标库 https://icon-sets.iconify.design/
 * @docs https://github.com/egoist/tailwindcss-icons
 * @example <Iconify icon="i-mdi-account-box" size="32" color="green" />
 */

interface Props {
  icon: string
  size?: string | number
  color?: string
}
const props = withDefaults(defineProps<Props>(), {
  size: 'inherit',
});

const iconSize = computed(() => (Number.isNaN(Number(props.size)) ? `${props.size}` : `${props.size}rpx`));

const iconStyle = computed(() => {
  const styles: Record<string, string> = {};
  if (props.color) {
    styles.color = props.color;
  }
  if (props.size) {
    styles.fontSize = iconSize.value;
  }
  return styles;
});
</script>

<template>
  <text class="iconify" :class="[props.icon]" :style="iconStyle" />
</template>

<style lang="scss" scoped>
.iconify {
  display: inline-block;
  line-height: 1;
}
</style>
