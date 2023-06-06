import { createRouter, createWebHistory } from 'vue-router';
import Login from './components/Login.vue';
import Movies from './components/Movies.vue';
import Movie from './components/Movie.vue';
import Register from './components/Register.vue';
import Admin from './components/Admin.vue';
import Unauthorized from './components/Unauthorized.vue';
import jwtDecode from 'jwt-decode';

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
  },
  {
    path: '/register',
    name: 'Register',
    component: Register,
  },
  {
    path: '/unauthorized',
    name: 'Unauthorized',
    component: Unauthorized
  },
  {
    path: '/admin',
    name: 'Admin',
    component: Admin,
    meta: { requiresAuth: true, requiredRole: 'admin' },
    
    beforeEnter: (to, from, next) => {
      const token = localStorage.getItem('token');
    
      if (to.meta.requiresAuth && token) {
        const cleanedToken = token.replace(/"/g, '');
        const decodedToken = jwtDecode(cleanedToken);
        console.log('Decoded token:', decodedToken);
    
        const userRoles = Array.isArray(decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
          ? decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
          : [];
        console.log('User roles:', userRoles);
    
        const requiredRoles = Array.isArray(to.meta.requiredRole) ? to.meta.requiredRole : [to.meta.requiredRole];
        console.log('Required roles:', requiredRoles);
    
        const hasRequiredRole = requiredRoles.every(role => userRoles.includes(role));
        console.log('Has required role:', hasRequiredRole);
    
        if (hasRequiredRole) {
          next();
        } else {
          // Redirect to unauthorized page or show an error
          next('/unauthorized');
        }
      } else if (to.meta.requiresAuth && !token) {
        // Redirect to login page when token is empty
        next('/login');
      } else {
        next();
      }
    }
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
