/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // --- ÁREA DESLOGADA ---
    {
      path: '/login',
      name: 'login',
      component: () => import('@/pages/LoginPage.vue'),
      meta: { requiresAuth: false }, // Qualquer um pode acessar
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/pages/RegisterPage.vue'),
      meta: { requiresAuth: false },
    },

    // --- ÁREA LOGADA ---
    {
      path: '/',
      component: () => import('@/layouts/default.vue'),
      meta: { requiresAuth: true }, // Todas as rotas filhas exigem login!
      children: [
        {
          path: '',
          redirect: '/students'
        },
        {
          path: '/students',
          name: 'student',  
          component: () => import('@/pages/StudentPage.vue'), 
        },
        {
          path: '/courses',
          name: 'course',  
          component: () => import('@/pages/CoursePage.vue'), 
        },
      ],
    },
    // Rota para not found (404)
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('@/pages/NotFoundPage.vue'),
    },
  ],
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isLoggedIn = authStore.isAuthenticated;

  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const isLoginPage = to.path == '/login' || to.path == '/register';

  if (requiresAuth && !isLoggedIn) {
    next('/login');
  } else if (isLoginPage && isLoggedIn) {
    next('/');
  } else {
    next();
  }
});

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (localStorage.getItem('vuetify:dynamic-reload')) {
      console.error('Dynamic import error, reloading page did not fix it', err)
    } else {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

export default router
