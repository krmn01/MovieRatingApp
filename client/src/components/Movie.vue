<template>
  <div class="movie-details">
    <div v-if="loading" class="loading-message">Loading...</div>
    <div v-else>
      <div class="card">
        <div class="card-body">

          <h2>{{ movie.title }}</h2>

          <div class="stars">
          <star-rating
            v-bind:increment="0.01"
            v-bind:star-size="20"
            :rating="rating"
            :read-only="true"
          ></star-rating> 
          {{this.movie.ratingCount}}
          </div>

          <img
            v-if="movie.imgSrc"
            :src="require(`@/assets/img/movies/${movie.imgSrc}`)"
            alt="Movie Image"
            class="movie-image" >

          <p class="movie-description">{{ movie.description }}</p>
        </div>

        <div class="comments">
          <h3>Komentarze</h3>
          <div v-for="comment in movie.comments" :key="comment.id" class="card-body comment">
           <div class="comment-details">
              <div class="comment-row">
                  <div class="username"><b>{{ comment.userName }}</b></div>
                  <div class="spacer"></div>
                  <div class="date">{{ comment.formatedDataTime }}</div>
              </div>
              <p class="comment-content">{{ comment.content }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import StarRating from 'vue-star-rating';

export default {
  components: {
    StarRating,
  },
  data() {
    return {
          movie: {
        id: '',
        imgSrc: '',
        title: '',
        description: '',
        director: '',
        productionYear: '',
        comments: [],
      },
      rating: 0,
      ratingCount:0,
      loading: true,
    };
  },
  mounted() {
    const movieId = this.$route.params.id;
    this.getMovieDetails(movieId);
  },
  methods: {
    async getMovieDetails(movieId) {
      try {
        const response = await axios.get(this.hostname + `Movie/get/${movieId}`);
        this.movie = response.data.movie;
        this.loading = false;
        this.rating = response.data.rating;
        this.ratingCount = response.data.ratingCount;
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>

<style scoped>
.movie-details {
  padding: 20px;
}

.movie-image {
  width: 25%;
  height: 25%;
  object-fit: cover;
  margin-bottom: 10px;
}

.movie-description {
  font-size: 16px;
  margin-top:15px;
  margin-bottom: 10px;
}



.comments {
  margin-top: 20px;
}

.comment {
  margin-bottom: 10px;
  padding: 10px;
  background-color: #f0f0f0;
}

.stars {
  display: flex;
  justify-content: center;
  margin-bottom: 10px;
  margin-top:10px;
}

.comment-row {
  display: flex;
  justify-content: space-between;
}

.username {
  align-self: flex-start;
  margin-left:25px;
}

.date {
  align-self: flex-end;
  margin-right:25px;
}

.spacer {
  flex-grow: 1;
}
</style>
