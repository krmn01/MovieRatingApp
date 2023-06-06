<template>
  <div id="app">
    <header>
      <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container">
          <router-link class="navbar-brand" to="/movies">Movies</router-link>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
            aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto">
              <li class="nav-item">
                <router-link v-if="isLoggedIn && roles.includes('admin')" class="nav-link" to="/admin">Admin Panel</router-link>
              </li>
              <li class="nav-item">
                <router-link class="nav-link" to="/movies">Movies</router-link>
              </li>
              <li class="nav-item" v-if="isLoggedIn">
                <div class="nav-link dropdown" @mouseenter="showLogoutOptions" @mouseleave="hideLogoutOptions">
                  <span class="username" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                    <b>{{ username }}</b>
                  </span>
                  <ul class="dropdown-menu dropdown-menu-end" v-if="showLogout">
                    <li><a class="dropdown-item" @click="logout">Logout</a></li>
                  </ul>
                </div>
              </li>
              <li class="nav-item" v-else>
                <router-link class="nav-link" to="/login">Login</router-link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App',
  data() {
    return {
      isLoggedIn: false,
      username: '',
      roles: [],
      showLogout: false
    };
  },
  mounted() {
    this.checkLoginStatus();
  },
  methods: {
    checkLoginStatus() {
      const token = localStorage.getItem('token');
      if (token) {
        this.isLoggedIn = true;
        this.username = localStorage.getItem('userName').replace(/"/g, '');

        const tokenPayload = JSON.parse(atob(token.split('.')[1]));
        this.roles = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      }
    },
    showLogoutOptions() {
      this.showLogout = true;
    },
    hideLogoutOptions() {
      this.showLogout = false;
    },
    logout() {
      localStorage.removeItem('token');
      localStorage.removeItem('userName');
      this.isLoggedIn = false;
      this.username = '';
      this.roles = [];
      this.$router.push('/login');
    }
  },
  watch: {
    isLoggedIn() {
      this.checkLoginStatus();
    }
  }
};
</script>

<style>
header {
  background: #f8f9fa;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 10px 0;
}

.navbar {
  padding: 0;
}

.navbar-brand {
  font-size: 24px;
  font-weight: bold;
  color: #333;
}

.navbar-toggler {
  border: none;
  outline: none;
  padding: 0;
}

.navbar-toggler-icon {
  background-color: #333;
  width: 20px;
  height: 2px;
}

.navbar-nav {
  align-items: center;
}

.nav-item {
  margin-left: 10px;
}

.nav-link {
  color: #333;
  font-size: 18px;
  transition: all 0.3s ease;
}

.nav-link:hover {
  color: #f00;
}

.dropdown-menu {
  border: none;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.dropdown-item {
  color: #333;
  font-weight: bold;
  padding: 10px;
  transition: all 0.3s ease;
}

.dropdown-item:hover {
  color: #f00;
  background-color: #f8f9fa;
}
</style>
