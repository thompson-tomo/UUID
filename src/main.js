import { createApp } from 'vue'
import { createHead } from '@vueuse/head'
import App from './App.vue'
import router from './router'
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'prismjs/themes/prism-tomorrow.css'

const app = createApp(App)
const head = createHead()

app.use(head)
app.use(router)
app.mount('#app')