/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'

// Styles
import 'unfonts.css'
import axios from 'axios';
axios.defaults.baseURL = 'http://localhost:5186/api';
const app = createApp(App)

registerPlugins(app)

app.mount('#app')
