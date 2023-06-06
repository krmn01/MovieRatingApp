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
          > </star-rating> 
          <p class="ratingCount">( {{this.ratingCount}} ) </p>
          </div>

          <div v-if="isLoggedIn">
          <p> Twoja ocena: </p>
          </div>

          <div v-if="isLoggedIn" class="specificUsersRate stars">
            <star-rating
              v-bind:increment="1"
              v-bind:star-size="20"
              :rating="specificUsersRating"
              :read-only="false"
               @update:rating="setRating"
               @click="rateMovie"
            ></star-rating>
          </div>

          <img
            v-if="movie.imgSrc"
            :src="require(`@/assets/img/movies/${movie.imgSrc}`)"
            alt="Movie Image"
            class="movie-image" >

          <p class="movie-description">{{ movie.description }}</p>
        </div>

        <div class="comments">
          <h3>Komentarze {{movie.commentsCount}}</h3>
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
            <div v-if="isLoggedIn">
                <div class="add-comment">
                    <textarea class="form-control" v-model="newComment" rows="3" cols="30" placeholder="Zawartość komentarza..."></textarea>
                    <button class="btnbtn-dark" @click="addComment">Dodaj komentarz</button>
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
import Swal from 'sweetalert2';

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
        commentsCount: 0
      },
      rating: 0,
      ratingCount:0,
      specificUsersRating: 0,
      newComment: '',
      loading: true,
      isLoggedIn: false,
    };
  },
  mounted() {
    const movieId = this.$route.params.id;
    this.getMovieDetails(movieId);
    this.getSpecificUsersMovieRate(movieId);
  },
  methods: {
    async getMovieDetails(movieId) {
      try {
        const response = await axios.get(this.hostname + `Movie/get/${movieId}`);
        this.movie = response.data.movie;
        this.loading = false;
        this.rating = response.data.rating;
        this.ratingCount = response.data.ratingCount;
        if(this.movie.comments.length!=null) this.commentsCount = this.movie.comments.length;
        this.commentsCount = 0;
      } catch (error) {
        console.error(error);
      }
    },
      async getSpecificUsersMovieRate(movieId) {
      try {
        const token = localStorage.getItem('token').replace(/"/g, '');
        const response = await axios.get(this.hostname + `Movie/get/${movieId}/rate`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        this.specificUsersRating = response.data.movieRate.rating;
        this.isLoggedIn = true; 
      } catch (error) {
        console.error(error);
      }
    },

      async addComment() {
        try {
        const token = localStorage.getItem('token').replace(/"/g, '');
        
        if(this.newComment!="")
        {
          const comment = {
            MovieId: this.movie.id,
            Content: this.newComment
          };

        const response = await axios.post(this.hostname + 'Movie/comment/new', comment, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        if (response.status === 200) {
          this.newComment = '';
          this.getMovieDetails(this.movie.id);
        } else {
          Swal.fire('Something went wrong');
        }
        }else Swal.fire('Nie można wysłać pustego komentarza!');
      } catch (error) {
        console.error(error);
      }
    },

    async rateMovie() {
    try {
      const token = localStorage.getItem('token').replace(/"/g, '');

      const rate = {
        MovieId: this.movie.id,
        Rating: this.specificUsersRating,
      };

      axios
        .post(this.hostname + 'Movie/set/rate', rate, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        .then(response => {
          if (response.status === 200) {
            this.getMovieDetails(this.movie.id);
          } else {
            Swal.fire('Something went wrong');
          }
        })
        .catch(error => {
          console.error(error);
        });
    } catch (error) {
      console.error(error);
    }
  },

  setRating(rating){
      this.specificUsersRating= rating;
    }

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

.ratingCount{
  margin-top:15px;
  margin-left:10px;
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
