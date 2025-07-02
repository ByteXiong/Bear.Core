// import { defineStore } from 'pinia';

interface AppState {
  sys?: string | number
}

export const useAppStore = defineStore('app-store', {
  state: (): AppState => ({}),
  getters: {},
  actions: {},
});
