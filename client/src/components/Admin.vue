<template>
  <div class="admin-panel">
    <h1>Admin Panel</h1>
    <div class="card">
      <div class="card-body">
        <h2 class="card-title">Movies</h2>
        <ul class="list-group">
          <li v-for="movie in movies" :key="movie.id" class="list-group-item">
            <div class="movie-title">
              <h3>{{ movie.title }}</h3>
              <div class="button-group">
                <button @click="editMovie(movie)" class="btn btn-primary">Edit</button>
                <button @click="deleteMovie(movie)" class="btn btn-danger">Delete</button>
              </div>
            </div>
          </li>
        </ul>
      </div>
    </div>

    <h2 v-if="editingMovie">Editing {{ currentlyEditedMovie.title }}</h2>        


    <h2 v-if="addingMovie">Add New Movie</h2>
    <div v-if="addingMovie" class="card">
      <div class="card-body">
        <form @submit.prevent="addMovie" class="add-movie-form">
          <div class="form-group">
            <label for="title">Title:</label>
            <input v-model="newMovie.title" id="title" class="form-control" />
          </div>
          <div class="form-group">
            <label for="genre">Genre:</label>
            <input v-model="newMovie.genre" id="genre" class="form-control" />
          </div>
          <button type="submit" class="btn btn-success">Add Movie</button>
        </form>
      </div>
    </div>
    <div v-if="!addingMovie"> 
         <button @click="clickOnAddMovie" class="btn btn-primary">Add new Movie</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import Swal from 'sweetalert2';

export default {
  data() {
    return {
      movies: [], // Array to store the list of movies
      newMovie: {
        title: '',
        genre: ''
      },
      editingMovie:false,
      currentlyEditedMovie:{},
      addingMovie:false
    };
  },
  methods: {
    async editMovie(movie) {
       this.editingMovie = !this.editingMovie;
       this.currentlyEditedMovie = movie;
    },
    async deleteMovie(movie) {
        Swal.fire("Czy na pewno chcesz usunąć film "+movie.title+"?");
    },  
    async clickOnAddMovie()
    {
        this.addingMovie=true;
    },

    async addMovie() {
      // Handle add movie functionality
    }
  },
  async created() {
    try {
        const response = await axios.get(this.hostname + 'Movie/all');
        this.movies = response.data;

        for (let i = 0; i < this.movies.length; i++) {
          const movie = this.movies[i];
          let rate = 0;

          movie.rating.forEach((x) => {
            rate += x.rating;
          });

          const count = movie.rating.length;
          if(count>0){movie.rate = parseFloat(rate) / parseFloat(count);}
          else movie.rate = 0;
          movie.count = count;
        }
      } catch (error) {
        console.error(error);
      }
    }
};
</script>

<style scoped>
.admin-panel {
  display: flex;
  flex-direction: column;
  align-items: center;
}

h1 {
  margin-bottom: 20px;
}

h2 {
  margin-bottom: 10px;
}

.card {
  margin-bottom: 20px;
  width: 80%;
  max-width: 400px;
}

.card-title {
  font-size: 24px;
  font-weight: bold;
  margin-bottom: 10px;
}

.list-group-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.add-movie-form {
  margin-top: 20px;
  width: 80%;
  max-width: 400px;
}

.form-group {
  margin-bottom: 10px;
}

.form-control {
  width: 100%;
}

.btn {
  margin-top: 10px;
}
</style>