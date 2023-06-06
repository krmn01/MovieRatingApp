<template>
  <div>
    <h1>Unauthorized Access</h1>
    <p>You are not authorized to access this page.</p>
  </div>
</template>

<script>
export default {
  created() {
    const token = localStorage.getItem('token');
    if(token){this.$router.push('/movies');}
    else this.$router.push('/login');
  }
};
</script>