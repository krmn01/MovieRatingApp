import { createRouter, createWebHistory } from 'vue-router';
import Login from './components/Login.vue';
import Movies from './components/Movies.vue';
import Movie from './components/Movie.vue';

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
  {
    path: '/movie/:id',
    name: 'Movie',
    component: Movie,
    props: true
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
