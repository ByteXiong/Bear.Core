<script setup lang="ts">
import { onHide, onLaunch, onShow } from '@dcloudio/uni-app';
import { useUserStore } from '@/stores/modules/user';

onLaunch(() => {
  console.log('App Launch');
});
onShow(async () => {
  const userStore = useUserStore();
  await userStore.initUserInfo();
  console.log('App Show');
});
onHide(() => {
  console.log('App Hide');
});
</script>

<style>
</style>
