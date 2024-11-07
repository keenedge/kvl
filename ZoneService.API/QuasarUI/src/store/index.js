import Vue from 'vue'
import Vuex from 'vuex'

import axios from 'axios'

// import example from './module-example'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: {
    'selectedLayout': {
      'svg': 'statics/layout-1.svg',
      'windows': []
    },
    'test': '"Hello!',
    'consoles': [],
    'layouts': [],
    'kvm-sources': [],
    'video-sources': [],
    'presets': [],
    'corelabs': [],
    'audioCues': [],
    'history': []
  },
  getters: {
    test (state) {
      return state.test
    },
    consoles (state) {
      return state.consoles
    },
    presets (state) {
      return state.presets
    },
    corelabs (state) {
      return state.corelabs
    },
    kvmSources (state) {
      return state['kvm-sources']
    },
    videoSources (state) {
      return state['video-sources']
    },
    selectedLayout (state) {
      if (state.selectedLayout === undefined) {
        return state.layouts[0]
      } else {
        return state.selectedLayout
      }
    },
    layouts (state) {
      return state.layouts
    },
    actions (state) {
      return state.actions
    },
    audioCues (state) {
      return state.audioCues
    }
  },
  mutations: {
    addToHistory (state, payload) {
      payload.time = Date.now()
      payload.result = ''
      state.history.unshift(payload)
    },
    setSelectedLayout (state, payload) {
      state.selectedLayout = payload
    },
    setInitialState (state, payload) {
      var layouts = payload.layouts
      for (var i = 0; i < layouts.length; i++) {
        var layout = layouts[i]
        for (var j = 0; j < layout.windows.length; j++) {
          var w = layout.windows[j]
          console.log(w)
          var windowTop = w.y
          var windowLeft = w.x
          if (w.hasOwnProperty('anchorX')) {
            var anchorX = w.anchorX
            windowLeft = windowLeft - ((w.width / 2) * (anchorX + 1))
          }
          if (w.hasOwnProperty('anchorY')) {
            var anchorY = w.anchorY
            windowTop = windowTop - ((w.height / 2) * (anchorY + 1))
          }
          w.x = Math.trunc(windowLeft)
          w.y = Math.trunc(windowTop)
          w.anchorX = null
          w.anchorY = null
        }
      }

      state.layouts = layouts
      state.consoles = payload.consoles
      state.audioCues = payload.audioCues
      state.presets = payload.presets
      state.corelabs = payload.corelabs
      state['kvm-sources'] = payload['kvm-sources']
      state['video-sources'] = payload['video-sources']
      state.selectedLayout = state.layouts[0]

      console.log(state.layouts)
    }
  },
  actions: {
    addToHistory (context, payload) {
      context.commit('addToHistory', payload)
    },
    setSelectedLayout (context, payload) {
      context.commit('setSelectedLayout', payload)
    },
    loadInitialState (context) {
      var store = process.env.__STORE__

      axios.get(store)
        .then(
          (response) => {
            context.commit('setInitialState', response.data)
          }
        )
        .catch((response) => {
          console.log(response)
        })
    }
  }
})

store.dispatch('loadInitialState')
export default store
