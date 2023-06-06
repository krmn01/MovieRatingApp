// eslint-disable-next-line vue/multi-word-component-names
/* eslint-disable */
<template>

    <div class="col-md-12">
        <div class="container">
        <h3 class="e-shop-font">Logowanie</h3>
        
        <div class="card">
            <div class="card-body">
                <!---email label-->
                <div class="form-group">
                    <label for="email">E-mail:</label>
                    <input v-model="user.email" ref="email" type="text" class="form-control" placeholder="Wprowadź email" name="email"/>
                </div>
                <!---password label-->
                 <div class="form-group">
                    <label for="password">Hasło:</label>
                    <input v-model="user.password" ref="password" type="password" class="form-control" placeholder="Wprowadź hasło" name="password"/>
                </div>   

                <div class="form-group form-check">
                     <label class="form-check-label">
                        <input class="form-check-input" type="checkbox" name="remember">Zapamiętaj hasło
                     </label>
                </div>

                <div class="clearfix">
                    <button type="button" class="signIn" v-on:click="login">Zaloguj się</button>
                    <button type="button" class="signUp" v-on:click="signup">Zarejestruj się</button> 
                </div>
            </div>
        </div>
        
        </div>
    </div>


</template>


<script>
import axios from 'axios';
import Swal from 'sweetalert2';


export default{
    data(){
        return{
            user:{
                email:"",
                password:""
            }
        }
    },

    methods:{
        
        signup(){
            this.$router.push({name: 'Register'});
        },
        login(){
            if(this.checkValidation()){
                
                const request = {
                    email: this.user.email,
                    password: this.user.password
                }; 

                axios.post(this.hostname + "authenticate/login", request)
                .then(response => {
                    if(response.data.success == true)
                    {
                        localStorage.setItem('token', JSON.stringify(response.data.accessToken))
                        response.data.accessToken = "";
                        localStorage.setItem('userId', JSON.stringify(response.data.userId))
                        localStorage.setItem('userName', JSON.stringify(response.data.userName))

                        console.log(response.data)
                        window.location.href = "/movies";
                        
                    }
                    
                })
                .catch(error => {
                    Swal.fire(error.message);
                })
            }
        },
        checkValidation(){
            if(!this.user.email){
                this.$refs.email.focus();
                Swal.fire("Podaj prawidłowy email!");
                return;
            }
            if(!this.user.password){
                this.$refs.password.focus();
                Swal.fire("Podaj hasło!");
                return;
            }
            return true;
        }
    }

}


</script>

<style scoped>
    .container{
        max-width:360px;
    }

    button{
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