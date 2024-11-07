<template>
  <div class="layout-padding" style="text-align: center">
     <q-card v-ripple class="relative-position" inline v-for="preset in presets" v-bind:key="preset.name"  @click.native="applyPreset(preset)">
        <q-card-title align="center" class="text-tertiary bg-blue">
          {{ preset.name }}
        </q-card-title>
        <q-card-separator/>
        <q-card-main  align="center" style="width:200px">
              <div class="col-md-12" style="padding:10px;"   v-ripple>
                <q-icon size="50px" class="button-icons" v-bind:name="preset.icon"/>
              </div>
              <div class="col-md-12" style="padding:10px; vertical-aligment: center;">
                {{ preset.description }}
              </div>
        </q-card-main>
      </q-card>
    </div>
</template>

<script>

import {
} from 'quasar'

// import Presets from './presets-store'

export default {
  components: {
  },
  directives: {
  },
  methods: {
    refresher: function (done) {
      // var store = process.env.__STORE__

      // axios.get(store)
      //   .then(
      //     (response) => {
      //       context.commit('setState', response.data)
      //     }
      //   )
      //   .catch((response) => {
      //     console.log(response)
      //   })
      // this.$store.dispatch('reloadStore', layout)

      done()
    },
    // notify (message) {
    //   notify.create(message)
    // },
    applyPreset (preset) {
      var wallAction = preset.action
      var item = {
        historyDescription: 'Apply Preset ' + preset.description,
        wallAction: wallAction,
        httpResponse: {}
      }
      this.$store.dispatch('addToHistory', item)

      this.gearsVisible = true

      // var uriContent = 'data:application/octet-stream,' + encodeURIComponent(item.action)
      // window.open(uriContent, 'newXmlDoc')

      console.log(process.env.__API__)
      console.log(JSON.stringify(item, false, 2))
      var that = this
      console.log(item.wallAction)
      this.$axios.post(process.env.__API__ + '/zone', item.wallAction)
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
    presets () {
      console.log('Hello')

      return this.$store.getters.presets
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
