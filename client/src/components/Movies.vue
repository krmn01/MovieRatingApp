// eslint-disable-next-line vue/multi-word-component-names
/* eslint-disable */
<template>
  <div>
    <h1>Wszystkie Filmy</h1>
    <div class="card-body" v-for="movie in movies" :key="movie.id">
      <div class="movie-info">
        <img :src="movie.ImgSrc" alt="Movie Image" class="movie-image">
        <div class="movie-details">
          <p class="movie-title">{{ movie.title }}</p>
          <p class="movie-rate">{{ movie.rate }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      movies: [] 
    };
  },
  mounted() {
    this.getAllMovies();
  },
  methods: {
    async getAllMovies() {
      try {
        const response = await axios.get(this.hostname+'api/Movie/all');
        this.movies = response.data;
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
  width: 100px;
  height: 200px;
  margin-right: 10px;
}

.movie-details {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-grow: 1;
}

.movie-title {
  font-weight: bold;
}

.movie-rate {
  margin-left: 10px;
}
</style>