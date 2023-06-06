<template>
  <div class="col-md-12">
    <div class="container">
      <h3 class="e-shop-font">Rejestracja</h3>

      <div class="card">
        <div class="card-body">
            <!-- Username label -->
          <div class="form-group">
            <label for="username">Nazwa użytkownika:</label>
            <input v-model="user.username" ref="username" type="text" class="form-control" placeholder="Wprowadź nazwę użytkownika" name="username" :maxlength="15" :minlength="3" />
          </div>
          
          <!-- Email label -->
          <div class="form-group">
            <label for="email">E-mail:</label>
            <input v-model="user.email" ref="email" type="email" class="form-control" placeholder="Wprowadź email" name="email"/>
          </div>

          <!-- Password label -->
          <div class="form-group">
            <label for="password">Hasło:</label>
            <input v-model="user.password" ref="password" type="password" class="form-control" placeholder="Wprowadź hasło" name="password"/>
          </div>

          <!-- Password confirmation label -->
          <div class="form-group">
            <label for="passwordConfirmation">Potwierdź hasło:</label>
            <input v-model="user.passwordConfirmation" ref="passwordConfirmation" type="password" class="form-control" placeholder="Potwierdź hasło" name="passwordConfirmation"/>
          </div>

          <div class="clearfix">
            <button type="button" class="signUp" v-on:click="register">Zarejestruj się</button>
            <button type="button" class="signIn" v-on:click="login">Masz konto? Zaloguj się</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import Swal from 'sweetalert2';

export default {
  data() {
    return {
      user: {
        email: '',
        password: '',
        passwordConfirmation: '',
        username: ''
      }
    };
  },
  methods: {
    login() {
      this.$router.push({ name: 'Login' });
    },
    register() {
      if (this.checkValidation()) {
        const request = {
          email: this.user.email,
          password: this.user.password,
          passwordConfirmation: this.user.passwordConfirmation,
          username: this.user.username
        };

        axios.post(this.hostname + 'authenticate/register', request)
          .then(response => {
            if (response.data.success == true) {
                Swal.fire("Konto założone!");
                this.login();
            }
          })
          .catch(error => {
            Swal.fire(error.message);
          });
      }
    },
    checkValidation() {
      if (!this.user.email) {
        this.$refs.email.focus();
        Swal.fire('Podaj prawidłowy email!');
        return false;
      }
      if (!this.user.password) {
        this.$refs.password.focus();
        Swal.fire('Podaj hasło!');
        return false;
      }
      if (this.user.password !== this.user.passwordConfirmation) {
        this.$refs.passwordConfirmation.focus();
        Swal.fire('Hasła się nie zgadzają!');
        return false;
      }
      if (!this.user.username) {
        this.$refs.username.focus();
        Swal.fire('Podaj nazwę użytkownika!');
        return false;
      }
      if (this.user.username.length < 3 || this.user.username.length > 15) {
        this.$refs.username.focus();
        Swal.fire('Nazwa użytkownika powinna mieć od 3 do 15 znaków!');
        return false;
      }
      return true;
    }
  }
};
</script>

<style scoped>
.container {
  max-width: 360px;
}

button {
  background-color: blue;
  color: white;
  padding: 14px 20px;
  margin: 8px 0;
  border: none;
  cursor: pointer;
  width: 100%;
  opacity: 0.9;
}
</style>
