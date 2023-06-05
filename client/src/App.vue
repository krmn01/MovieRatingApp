<template>
  <div id="app">
    <nav>
      <router-link to="/login">Login</router-link>
      <router-link to="/movies">Movies</router-link>
    </nav>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App',
};
</script>

<style>
#app {
  text-align: center;
}

nav {
  background: #f0f0f0;
  padding: 10px;
}

nav a {
  margin-right: 10px;
}
</style>
