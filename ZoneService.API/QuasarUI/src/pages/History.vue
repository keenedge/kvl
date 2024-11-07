<template>
  <div class="layout-padding relative-position" >
    <q-card v-for="item in history" :key="item.id">
      <q-item>
        <q-item-side>
            <div v-if="hasErrors(item)" style="color: red;">
              <q-icon name="fa-times" size="25px"/>
            </div>
            <div v-else style="color: green">
              <q-icon name="fa-check" size="25px"/>
            </div>
        </q-item-side>
        <q-item-main>
          <q-item-tile label>{{item.historyDescription}}</q-item-tile>
          <q-item-tile label>{{item.errorMessage}}</q-item-tile>
          <q-item-tile v-if="item.httpResponse" sublabel>{{`${item.httpResponse.statusText} (${item.httpResponse.status})`}}</q-item-tile>
        </q-item-main>
      </q-item>
      <q-item>
      <q-collapsible icon="explore" label="Request">
        <div>
          <pre>{{formatAction( item.wallAction ) }}</pre>
        </div>
      </q-collapsible>
      </q-item>
      <q-item>
        <q-collapsible icon="explore" :label="`Response`">
          <div>
            <pre v-for="(e,i) in item.httpResponse.data" width="30" style="width: 100%; overflow: auto; white-space: pre-wrap" :key="i">{{e}}</pre>
          </div>
        </q-collapsible>
      </q-item>
    </q-card>
    <q-inner-loading :visible="gearsVisible">
      <q-spinner-gears size="50px" color="primary"></q-spinner-gears>
    </q-inner-loading>
  </div>
</template>

<script>
import {
  QCard,
  QInnerLoading,
  QSpinnerGears,
  QScrollArea,
  QCollapsible,
  QIcon,
  QBtn,
  QInput,
  QList,
  QListHeader,
  QItem,
  QItemSeparator,
  QItemSide,
  QItemMain,
  QItemTile,
  QChip,
  QPopover,
  QModal
} from 'quasar'

import moment from 'moment'

export default {
  components: {
    QCard,
    QInnerLoading,
    QSpinnerGears,
    QScrollArea,
    QCollapsible,
    QIcon,
    QBtn,
    QInput,
    QList,
    QListHeader,
    QItem,
    QItemSeparator,
    QItemSide,
    QItemMain,
    QItemTile,
    QChip,
    QPopover,
    QModal
  },
  computed: {
    history () {
      return this.$store.state.history
    }
  },
  methods: {
    hasErrors (item) {
      return (item.httpResponse.status >= 400) || (item.errorMessage != null)
    },
    applyHistory (item) {
      var that = this
      this.gearsVisible = true

      // var uriContent = 'data:application/octet-stream,' + encodeURIComponent(item.action)
      // window.open(uriContent, 'newXmlDoc')

      item.result = ''
      this.$axios.post(process.env.__API__ + 'zone', item.action)
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
    },
    formatResponse (response) {
      return JSON.stringify(response, null, 2)
    },
    formatAction (action) {
      return JSON.stringify(action, null, 2)
    },
    toTime (t) {
      var m = moment(Date.now() - t)

      return m.format('mm:ss') + ' ago'
    }
  },
  data () {
    return {
      gearsVisible: false,
      jsonModal: false
    }
  }
}
</script>

<style scoped>
  .layout-padding {
     padding:1px;
  }
  .scrollabletextbox {
    min-height:500px;
    height:100%;
    width:100%;
    font-family: Verdana, Tahoma, Arial, Helvetica, sans-serif;
    font-size: 82%;
    overflow:scroll;
  }
</style>
