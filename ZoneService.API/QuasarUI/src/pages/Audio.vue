<template>
  <div class="layout-padding" style="text-align: center">
      <q-card v-ripple class="relative-position" inline v-for="cue in audioCues" v-bind:key="cue.id"  @click="applyAudioCue(cue)">
        <q-card-title align="center" class="text-tertiary bg-blue">
          {{ cue.id }}
        </q-card-title>
        <q-card-separator/>
        <q-card-main  align="center" style="width:200px">
              <div class="col-md-12" style="padding:10px;"   v-ripple>
                <!-- <q-icon size="50px" color="secondary"  v-bind:name="preset.icon"/> -->
              </div>
              <div class="col-md-12" style="padding:10px; vertical-aligment: center;">
                {{ cue.zone }} {{ cue.description }} {{ cue.action }}
              </div>
        </q-card-main>
      </q-card>
    </div>
  </template>

<script>

import {
  Ripple,
  QCard,
  QCardTitle,
  QCardMain,
  QCardSeparator,
  QIcon
} from 'quasar'

export default {
  components: {
    Ripple,
    QCard,
    QCardTitle,
    QCardMain,
    QCardSeparator,
    QIcon
  },
  directives: {
    Ripple
  },
  methods: {
    // notify (message) {
    //   notify.create(message)
    // },
    applyAudioCue (cue) {
      var wallAction = {
        audioConfig: {
          audioActions: [
            cue.cue
          ]
        }
      }
      var item = {
        historyDescription: 'Apply Audio Cur ' + cue.description,
        wallAction: wallAction,
        httpResponse: {}
      }
      this.$store.dispatch('addToHistory', item)

      this.gearsVisible = true

      console.log(process.env.__API__)
      console.log(JSON.stringify(item, false, 2))
      var that = this
      this.$axios.post(process.env.__API__ + 'zone', item.wallAction)
        .then((response) => {
          item.httpResponse = response
          that.gearsVisible = false
        })
        .catch((error) => {
          // var response = 'Post Error'
          // response = response + error
          // item.result = response
          // that.gearsVisible = false
          if (error.response) {
            // The request was made and the server responded with a status code
            // that falls out of the range of 2xx
            item.httpRequest = error.response
            item.httpResponse = error.request
            item.errorMessage = error.message
          } else if (error.request) {
            // The request was made but no response was received
            // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
            // http.ClientRequest in node.js
            item.httpRequest = null
            item.httpResponse = error.request
            item.errorMessage = error.message
          } else {
            // Something happened in setting up the request that triggered an Error
            console.log('Error', error.message)
            item.httpResponse = null
            item.errorMessage = error.message
          }
          console.log(error.config)
          that.gearsVisible = false
        })
    }
  },
  computed: {
    audioCues () {
      return this.$store.getters.audioCues
    }
  },
  data () {
    return {
    }
  }
}
</script>

<style>
</style>
