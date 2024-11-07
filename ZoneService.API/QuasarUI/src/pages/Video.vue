<template>
 <div class="layout-padding" style="text-align: center">
    <q-card class="col-2">
        <q-card-title align="center" class="text-tertiary bg-blue">
          Videos Controls
        </q-card-title>
        <q-card-separator/>
        <q-card-main  align="center">
            <q-btn icon='fas fa-volume-up' rounded color='green' @click='postOpenVideo("cl.mp4")'></q-btn>
            <q-btn icon='fas fa-volume-down' rounded color='green' @click='postOpenVideo("cl.mp4")'></q-btn>
            <q-btn icon='fas fa-volume-mute' rounded color='green' @click='postOpenVideo("cl.mp4")'></q-btn>

            <q-btn icon='fas fa-step-backward' rounded color='green' @click='postPosition("00:00:00")'></q-btn>
            <q-btn icon='fas fa-play' rounded color='green' @click='postAction("play")'></q-btn>
            <q-btn icon='fas fa-step-backward' rounded color='green' @click='postPosition("00:00:00")'></q-btn>
            <q-btn icon='fas fa-pause' rounded color='green' @click='postAction("pause")'></q-btn>

              <!-- <div class='col-md-12" style="padding:10px;"   v-ripple>
                <q-icon size="50px" color="secondary"  v-bind:name="preset.icon"/>
              </div>
              <div class="col-md-12" style="padding:10px; vertical-aligment: center;">
                {{ preset.description }}
              </div> -->
        </q-card-main>
      </q-card>
    <q-card class="col-2">
        <q-card-title align="center" class="text-tertiary bg-blue">
          Videos
        </q-card-title>
        <q-card-media  align="center">
          <q-list>
          <q-list-header inset>Videos</q-list-header>
          <q-item v-ripple inline v-for="video in videos" v-bind:key="video.name"  @click.native="postOpenVideo(video.file)">
            <q-item-side icon="fa-file-video" inverted color="primary" />
              <q-item-main>
                <q-item-tile label>{{video.name}}</q-item-tile>
                <q-item-tile sublabel>English, 2016</q-item-tile>
              </q-item-main>
              <q-item-side right icon="fa-play" color="green" />
            </q-item>
          </q-list>
        </q-card-media>
      </q-card>
    </div>
</template>

<script>
import { QIcon, QBtn } from 'quasar'

export default {
  name: 'AlphaLayout',
  components: {
    QIcon,
    QBtn
  },
  props: [],
  computed: {
    videos () {
      return [
        {
          'file': 'cl.mp4',
          'name': 'Core Labs Video',
          'language': 'English'
        },
        {
          'file': 'virtualmente_french.mp4',
          'name': 'Virtual Mente',
          'language': 'French'
        }
      ]
    }
  },
  data () {
    var url = 'http://localhost:5000/api/VideoPlayer'
    return {
      postPosition (val) {
        this.$axios({
          method: 'post',
          url: url + '/position',
          data: { Position: val }
        })
          .then(response => {
            console.log(response)
          })
          .catch(request => {
            console.log(request)
          })
      },
      postAction (verb) {
        this.$axios({
          method: 'post',
          url: url + '/action',
          data: { Verb: verb }
        })
          .then(response => {
            console.log(response)
          })
          .catch(request => {
            console.log(request)
          })
      },
      postOpenVideo (videoFileName) {
        var option = {
          method: 'post',
          url: url + '/open',
          headers: {'content-type': 'application/json'}

        }
        option.data = '"' + videoFileName + '"'
        this.$axios(option)
          .then(response => {
            console.log(response)
          })
          .catch(request => {
            console.log(request)
          })
      }
    }
  }
}
</script>

<style scoped=true>
</style>
