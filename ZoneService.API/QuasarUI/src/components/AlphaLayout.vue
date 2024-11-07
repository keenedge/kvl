<template>
  <div class="relative-position">
    <svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 13660 3072" preserveAspectRatio="xMidYMid meet" class="absolute-position" width="100%">
      <g>
        <rect :x="x" :y="y" :width="width" :height="height" stroke="lightslategray" fill="lightslategray" stroke-width="3"/>
        <g v-for='w in layout.windows' :key='w.id' v-on:click="onWindowClick(w)">
          <rect :x="w.x" :y="w.y" :width="w.width" :height="w.height" stroke="gray" fill="silver" stroke-width="25"/>
          <text :x="Math.trunc(w.width/2 + w.x)" :y="Math.trunc(w.height/2 + w.y)" class="svg-text">
            {{w.source.name}}
          </text>
        </g>
      </g>
    </svg>
  </div>
</template>

<script>

import {
  QPopover,
  QModal,
  QItem,
  QList
} from 'quasar'

export default {
  name: 'AlphaLayout',
  components: {
    QModal,
    QPopover,
    QItem,
    QList
  },
  props: [
    'layout',
    'videoSources'
  ],
  methods: {
    onWindowClick (window) {
      this.$emit('window-click', window)
    }
  },
  data () {
    return {
      selectedWindow: undefined,
      x: 0,
      y: 0,
      height: 3072,
      width: 13660
    }
  }
}
</script>

<style scoped=true>
        .svg-text {
          font-family: Verdana;
          font-size: 25em;
          fill: black;
          text-anchor:middle;
          alignment-baseline: middle
        }
</style>
