<template>
  <div>
    <div class="search-container">
      <input type="text" v-model="searchQuery" >
      <button @click="searchMovies">Szukaj</button>
    </div>

    <div v-if="movies && movies.length > 0">
    <div class="card" v-for="movie in movies" :key="movie.id">
      <router-link :to="{ name: 'Movie', params: { id: movie.id } }" class="card-body">
        <div class="movie-info">
          <img :src="require(`@/assets/img/movies/${movie.imgSrc}`)" alt="Movie Image" class="movie-image">
          <div class="movie-details">
            <p class="movie-title">{{ movie.title }}</p>
            <star-rating  v-bind:increment="0.01" :rating="movie.rate" :read-only="true" :star-size="20"></star-rating>
            <p class="movie-year">{{ movie.productionYear }}</p>
            <p class="movie-director">{{ movie.director }}</p>
          </div>
        </div>
      </router-link>
    </div>
    </div>
     <div v-else>
        <p>Nic nie znaleziono.</p>
      </div>

  </div>
</template>

<script>
import axios from 'axios';
import StarRating from 'vue-star-rating';

export default {
  components: {
    StarRating
  },
  data() {
    return {
       searchQuery: '',
      movies: []
    };
  },
  mounted() {
    this.getAllMovies();
  },
  methods: {
    async searchMovies() {
      try {
        const response = await axios.get(this.hostname + `Movie/search/${this.searchQuery}`);
        this.movies = response.data;
        for (let i = 0; i < this.movies.length; i++) {
          const movie = this.movies[i];
          let rate = 0;

          movie.rating.forEach((x) => {
            rate += x.rating;
          });

          const count = movie.rating.length;
          movie.rate = parseFloat(rate) / parseFloat(count);
          movie.count = count;
        }
      } catch (error) {
        console.error(error);
      }
    },
    async getAllMovies() {
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
          movie.rate = parseFloat(rate) / parseFloat(count);
          movie.count = count;
        }
      } catch (error) {
        console.error(error);
      }
    }
  }
};
</script>

<style scoped>
.movie-info {
  display: flex;
  align-items: center;
}

.movie-image {
  width: 20%;
  height: 20%;
  margin-right: 10px;
}

.movie-details {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  flex-grow: 1;
  margin-top: 10px;
}

.movie-title {
  font-weight: bold;
  font-size: 64px;
  margin: 0;
  text-align: center;
}

.movie-director {
  font-size: 36px;
}

.movie-year {
  font-size: 24px;
}

.movie-rate {
  margin-left: 10px;
}
</style>
