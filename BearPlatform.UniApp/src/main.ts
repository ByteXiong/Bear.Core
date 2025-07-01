import { createSSRApp } from 'vue';
import App from './App.vue';
import { setupRouter } from './router';
import { setupStore } from './stores';
import './styles/main.css';
import 'uno.css';

export function createApp() {
  const app = createSSRApp(App);

  setupRouter(app);
  setupStore(app);

  return {
    app,
  };
}
