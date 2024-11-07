<template>
  <div class="relative-position">
    <q-card>
      <div class="row justify-center">
        <h5 justify-center>Console Input</h5>
      </div>
      <q-card-main class="row justify-center">
          <q-select v-for="(c,i) in consoles" v-bind:key="i" v-bind:class="[ 'col-md-3',  i > 0 ? 'offset-md-1' : '']"  @input="setConsoleKvmSource( i, c)"
            v-bind:stack-label="c.name + ' KVM Source'"
            inverted
            color="blue-grey"
            separator
            v-model="c.source"
            :options="kvmSources"
          />
      </q-card-main>
    </q-card>
    <q-card justify-center>
      <div class="row justify-center">
        <h5 justify-center>Wall Layout</h5>
      </div>
      <q-card-main>
        <AlphaLayout :layout='selectedLayout' v-on:window-click="onWindowClick" :videoSources="videoSources"/>
      </q-card-main>
      <q-modal ref="popover">
        <q-list separator>
          <q-item v-for="(s,i) in videoSources" v-bind:key="i"  @click.native="setSelectedSourceOnWindow( s, selectedWindow)">
            <q-icon  class="col-md-3" :name="s.icon"/><div class="col-md-6">{{s.name}}</div>
          </q-item>
        </q-list>
      </q-modal>
      <q-modal class="row justify-center shadow-10" ref="selectLayoutModal" v-model="showLayoutSelector" :content-css="{minWidth: '40vw', minHeight: '90vh'}">
        <q-modal-layout
          header-stsyle="min-height: 100px"
          content-class="{'bg-primary': isPrimary, 'some-class': someBoolean}"
          footer-class="bg-primary some-class"
          footer-style="{fontSize: '24px', fontWeight: 'bold'}"
        >
          <div slot="header" style="padding: 10px;text-align: center"><h5>Choose a video wall layout</h5></div>
          <div slot="footer" style="text-align: center; padding:10px;" >
            <q-btn style="text-align: center; padding: 10px;" icon="fa-check" rounded color="green" @click="showLayoutSelector = true">Cancel</q-btn>
          </div>
          <div class="row justify-center shadow-10" style="padding:20px;">
            <div class="col-md-12" v-for="l in layouts" style="margin: 10px;" :key="l.id" @click="selectLayout(l);" >
              <AlphaLayout :layout='l'></AlphaLayout>
            </div>
          </div>
        </q-modal-layout>
      </q-modal>
      <div class="row justify-center" style="margin-bottom:10px;">
        <q-btn class="col-md-3" style="text-align: center; padding: 10px;" icon="fa-hand-o-up" rounded color="primary" @click="showLayoutSelector = true">Select Layout</q-btn>
      </div>
      <div class="row justify-center" style="margin-bottom:10px;">
        <q-btn class="col-md-3" style="text-align: center; padding: 10px;" icon="fa-check" rounded color="green" @click="applyChanges('Apply Layout Changes')">Apply</q-btn>
      </div>

    </q-card>
  </div>
</template>

<script>

import {
  QIcon,
  QCardActions,
  QInnerLoading,
  QSpinnerGears,
  QCard,
  QList,
  QPopover,
  QItem,
  QItemSeparator,
  QItemTile,
  QItemMain,
  QListHeader,
  QCardTitle,
  QCardSeparator,
  QCardMain,
  QSelect,
  ActionSheet,
  QModal,
  QModalLayout,
  QItemSide,
  QBtn
} from 'quasar'
import AlphaLayout from 'components/AlphaLayout'

export default {
  components: {
    QIcon,
    QCardActions,
    QInnerLoading,
    QSpinnerGears,
    AlphaLayout,
    QCard,
    QList,
    QPopover,
    QItemSide,
    QItem,
    QItemSeparator,
    QItemMain,
    QItemTile,
    QListHeader,
    QCardTitle,
    QCardSeparator,
    QCardMain,
    QSelect,
    ActionSheet,
    QModal,
    QModalLayout,
    QBtn
  },
  created: function () {
  },
  methods: {
    setSelectedSourceOnWindow (source, window) {
      window.source = source
      this.$refs.popover.hide()
    },
    onWindowClick (window) {
      console.log('onWindowClick')
      console.log(window)

      console.log('Window Clicked')
      console.log(window)
      this.selectedWindow = window
      this.$refs.popover.show()

      //      this.showSourceSelector = true
    },
    setConsoleKvmSource (i, c) {
      var tlActions = [
        {
          'device': 'tlx24',
          'path': `tlx-${c.source}2con${i + 1}`
        }
      ]
      var wallAction = {
        'thinkLogicalConfig': {
          'thinkLogicalActions': tlActions
        }
      }

      this.applyWallAction(`"Set source for console ${i + 1}`, wallAction)
    },
    selectLayout: function (layout) {
      this.$store.dispatch('setSelectedLayout', layout)
      this.showLayoutSelector = false
      // this.$refs.selectLayoutModal.close()
    },
    applyWallAction (description, wallAction) {
      console.log(wallAction)
      var item = {
        historyDescription: description,
        wallAction: wallAction,
        httpResponse: {}
      }
      // this.http.put(wallAction)
      this.$store.dispatch('addToHistory', item)
      this.gearsVisible = true

      // var uriContent = 'data:application/octet-stream,' + encodeURIComponent(item.action)
      // window.open(uriContent, 'newXmlDoc')

      console.log(JSON.stringify(item, false, 2))
      var that = this
      console.log('!')
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
    },
    applyChanges: function (description) {
      var tlActions = []
      // add VX routes
      tlActions.push({
        'device': 'vx320',
        'path': 'alpha.txt'
      })

      // // add TLX routes
      // this.consoles.forEach(function (c, i) {
      //   if (c.source !== undefined) {
      //     console.log(c)
      //     var tlxAction = {
      //       'device': 'tlx',
      //       'path': `${c.source}2con${i + 1}`
      //     }
      //     tlActions.push(tlxAction)
      //   }
      // })

      // add window sources
      var layout = this.$store.getters.selectedLayout

      var alphaActions = layout.windows.map((window, i) => {
        return {
          id: i + 1,
          input: window.source.source,
          y: window.y,
          x: window.x,
          height: window.height,
          width: window.width
        }
      })

      var wallAction = {
        'presetFile': layout.presetFileName,
        'alphaConfig': {
          closeAll: true,
          alphaActions: alphaActions
        },
        'thinkLogicalConfig': {
          'thinkLogicalActions': tlActions
        }
      }

      this.applyWallAction(description, wallAction)
    }
  },
  computed: {
    selectedLayout () {
      return this.$store.getters.selectedLayout
    },
    consoles () {
      return this.$store.getters.consoles
    },
    layouts () {
      return this.$store.getters.layouts
    },
    kvmSources () {
      return this.$store.getters.kvmSources.map(function (d) {
        return {
          label: d.description,
          value: d.id,
          icon: d.icon
        }
      })
    },
    videoSources () {
      return this.$store.getters.videoSources
    }
  },
  data () {
    return {
      selectedWindow: undefined,
      gearsVisible: false,
      showLayoutSelector: false,
      showSourceSelector: false,
      classes: {
        width: 2,
        isFirst: function (i) {
          return i > 1
        }
      }
    }
  }
}
</script>

<style scoped>
   .layout-padding {
     padding:1px;
   }
</style>
