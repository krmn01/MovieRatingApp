import { createRouter, createWebHistory } from 'vue-router';
import Login from './components/Login.vue';
import Movies from './components/Movies.vue';

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login,
  },
  {
    path: '/movies',
    name: 'Movies',
    component: Movies,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
