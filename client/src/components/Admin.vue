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
                <button @click="clickOnEditMovie(movie)" class="btn btn-primary">Edit</button>
                <button @click="deleteMovie(movie)" class="btn btn-danger">Delete</button>
              </div>
            </div>
          </li>
        </ul>
      </div>
    </div>

    <h2 v-if="editingMovie">Editing {{ currentlyEditedMovie.title }}</h2>        
    <div v-if="editingMovie" class="card">
      <div class="card-body">
        <form class="edit-movie-form">
         
          <div class="form-group">
            <label for="title">Tytuł:</label>
            <input ref="title" v-model="currentlyEditedMovie.title" id="title" class="form-control" />
          </div>
          
          <div class="form-group">
            <label for="imgSrc">Image source:</label>
            <input ref="imgSrc" v-model="currentlyEditedMovie.imgSrc" id="imgSrc" class="form-control" />
          </div>

          <div class="form-group">
            <label for="director">Reżyser:</label>
            <input ref="director" v-model="currentlyEditedMovie.director" id="director" class="form-control" />
          </div>  

           <div class="form-group">
            <label for="description">Opis:</label>
            <input ref="description" type="textarea" v-model="currentlyEditedMovie.description" id="description" class="form-control" />
          </div>

           <div class="form-group">
            <label for="productionYear">Rok produkcji:</label>
            <input ref="productionYear" v-model="currentlyEditedMovie.productionYear" id="productionYear" class="form-control" />
          </div>

            <div class="button-group"> 
          <button type="submit" @click="editMovie" class="btn btn-success">Zapisz</button>
          <button type="submit" @click="clickOnEditMovie(movie)" class="btn btn-danger">Anuluj</button>
          </div>
        </form>
      </div>
    </div>

    <h2 v-if="addingMovie">Add New Movie</h2>
    <div v-if="addingMovie" class="card">
      <div class="card-body">
        <form  class="add-movie-form">
         
          <div class="form-group">
            <label for="title">Tytuł:</label>
            <input ref="title" v-model="newMovie.title" id="title" class="form-control" />
          </div>
          
          <div class="form-group">
            <label for="imgSrc">Image source:</label>
            <input ref="imgSrc" v-model="newMovie.imgSrc" id="imgSrc" class="form-control" />
          </div>

          <div class="form-group">
            <label for="director">Reżyser:</label>
            <input ref="director" v-model="newMovie.director" id="director" class="form-control" />
          </div>  

           <div class="form-group">
            <label for="description">Opis:</label>
            <input ref="description" type="textarea" v-model="newMovie.description" id="description" class="form-control" />
          </div>

           <div class="form-group">
            <label for="productionYear">Rok produkcji:</label>
            <input ref="productionYear" v-model="newMovie.productionYear" id="productionYear" class="form-control" />
          </div>

            <div class="button-group"> 
          <button type="submit" @click="addMovie" class="btn btn-success">Dodaj film</button>
          <button type="submit" @click="clickOnCancelAddingMovie" class="btn btn-danger">Anuluj</button>
          </div>
        </form>
      </div>
    </div>
    <div v-if="!addingMovie"> 
         <button @click="clickOnAddMovie" class="btn btn-primary">Dodaj film</button>
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
        imgSrc: '',
        director:'',
        description:'',
        productionYear:'',
        rating:[],
        comments:[],
      },
      editingMovie:false,
      currentlyEditedMovie:{},
      addingMovie:false
    };
  },
  methods: {
    validate(movie){
         if (!movie.title) {
    this.$refs.title.focus();
    Swal.fire('Podaj tytuł!');
    return;
  }

  if (!movie.imgSrc) {
    this.$refs.imgSrc.focus();
    Swal.fire('Podaj źródło obrazu!');
    return;
  }

  if (!movie.imgSrc.endsWith('.png')) {
    this.$refs.imgSrc.focus();
    Swal.fire('Źródło obrazu powinno kończyć się rozszerzeniem .png!');
    return;
  }

  if (!movie.director) {
    this.$refs.director.focus();
    Swal.fire('Podaj reżysera!');
    return;
  }

  const directorRegex = /^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+\s[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$/;
  if (!directorRegex.test(movie.director)) {
    this.$refs.director.focus();
    Swal.fire('Reżyser powinien mieć imię i nazwisko (np. "Imię Nazwisko")!');
    return;
  }

  if (!movie.description) {
    this.$refs.description.focus();
    Swal.fire('Podaj opis!');
    return;
  }

  if (!movie.productionYear) {
    this.$refs.productionYear.focus();
    Swal.fire('Podaj rok produkcji!');
    return;
  }

  const productionYear = Number(movie.productionYear);
  if (isNaN(productionYear) || productionYear < 1920 || productionYear > 2023) {
    this.$refs.productionYear.focus();
    Swal.fire('Rok produkcji powinien być liczbą między 1920 a 2023!');
    return;
  }
      return true;
    },

    async clickOnEditMovie(movie) {
       this.editingMovie = !this.editingMovie;
       this.currentlyEditedMovie = movie;
    },
    async deleteMovie(movie) {
    Swal.fire({
    title: 'Potwierdzenie',
    text: `Czy na pewno chcesz usunąć film ${movie.title}?`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Tak',
    cancelButtonText: 'Anuluj',
    reverseButtons: true
  }).then(async (result) => {
    if (result.isConfirmed) {
      try {
        const token = localStorage.getItem('token').replace(/"/g, '');
       const response = await axios.delete(`${this.hostname}Movie/delete/${movie.id}`, {
         headers: {
        Authorization: `Bearer ${token}`
      }
     });
        if(response.status === 200)
        {
        const index = this.movies.findIndex((m) => m.id === movie.id);
        if (index !== -1) {
          this.movies.splice(index, 1);
        }
        Swal.fire('Usunięto', 'Film został pomyślnie usunięty.', 'success');
        }
      } catch (error) {
        console.error(error);
        Swal.fire('Błąd', 'Wystąpił błąd podczas usuwania filmu.', 'error');
      }

      Swal.fire('Usunięto', 'Film został pomyślnie usunięty.', 'success');
    } else if (result.dismiss === Swal.DismissReason.cancel) {
      Swal.fire('Anulowano', 'Anulowano usuwanie filmu.', 'info');
    }
  });
    },  
    async clickOnAddMovie()
    {
        this.addingMovie=true;
    },
    async clickOnCancelAddingMovie()
    {
        this.addingMovie=false;
    },

    async editMovie()
    {
        if(this.validate(this.currentlyEditedMovie)){
                
        try {
        const token = localStorage.getItem('token').replace(/"/g, '');
       const response = await axios.put(`${this.hostname}Movie/update/this`,this.currentlyEditedMovie, {
         headers: {
        Authorization: `Bearer ${token}`
      }
     });
        if(response.status === 200)
        {
            const index = this.movies.findIndex((m) => m.id === this.currentlyEditedMovie.id);
            this.movies[index] = this.currentlyEditedMovie;
            Swal.fire("Zedytowano film!");
        }
        }catch (error) {
      console.error(error);
      Swal.fire("Wystąpił błąd podczas edytowania filmu.");
    }
    }
    },

    async addMovie() {
      if(this.validate(this.newMovie))
      {
        try {
        const token = localStorage.getItem('token').replace(/"/g, '');
      const response = await axios.post(this.hostname + "Movie/add/new", this.newMovie, {
        headers: {
          Authorization: `Bearer ${token}`
        },
      });
      
      if (response.status === 200) {
        const newMovie = {
          title: this.newMovie.title,
          imgSrc: this.newMovie.imgSrc,
          director: this.newMovie.director,
          description: this.newMovie.description,
          productionYear: this.newMovie.productionYear,
        };

        this.movies.push(newMovie);
        Swal.fire('Dodano film ' + this.newMovie.title + '!');

        
        this.newMovie = {
          title: '',
          imgSrc: '',
          director: '',
          description: '',
          productionYear: '',
        };
      } else {
        Swal.fire("Coś poszło nie tak");
      }
    } catch (error) {
      console.error(error);
      Swal.fire("Wystąpił błąd podczas dodawania filmu.");
    }
      }
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