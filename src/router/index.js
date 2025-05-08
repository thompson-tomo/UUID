import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/home.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      title: 'UUID - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/getting-started',
    name: 'GettingStarted',
    component: () => import('../views/gettingstarted.vue'),
    meta: {
      title: 'Getting Started - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/api-reference',
    name: 'APIReference',
    component: () => import('../views/apireference.vue'),
    meta: {
      title: 'API Reference - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/examples',
    name: 'Examples',
    component: () => import('../views/examples.vue'),
    meta: {
      title: 'Examples - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/serialization',
    name: 'Serialization',
    component: () => import('../views/serialization.vue'),
    meta: {
      title: 'Serialization - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/performance',
    name: 'Performance',
    component: () => import('../views/performance.vue'),
    meta: {
      title: 'Performance - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/security',
    name: 'Security',
    component: () => import('../views/security.vue'),
    meta: {
      title: 'Security - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/migration-guide',
    name: 'MigrationGuide',
    component: () => import('../views/migrationguide.vue'),
    meta: {
      title: 'Migration Guide - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/faq',
    name: 'FAQ',
    component: () => import('../views/faq.vue'),
    meta: {
      title: 'FAQ - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/404',
    name: 'NotFound',
    component: () => import('../views/errors/404.vue'),
    meta: {
      title: 'Page Not Found - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/500',
    name: 'ServerError',
    component: () => import('../views/errors/500.vue'),
    meta: {
      title: 'Server Error - High-Performance .NET UUID Generator for Modern Systems'
    }
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/404'
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  document.title = to.meta.title || 'High-Performance .NET UUID Generator for Modern Systems'
  next()
})

export default router