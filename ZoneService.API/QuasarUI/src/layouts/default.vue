<template>
  <div>
      <q-layout ref="layout" view="lHh Lpr lFf"  style="padding:0px;margin:0px">
      <q-layout-header>
        <q-tabs align="center">
          <q-route-tab icon="fa-users-crown fad" slot="title" to="/corelabs" replace label="Core Labs" />
          <q-route-tab icon="fa-dragon fad" slot="title" to="/presets" replace label="Presets" />
          <q-route-tab icon="fa-wand-magic fad" slot="title" to="/alpha" replace label="Alpha" />
          <q-route-tab icon="fa-book-spells fad" slot="title" to="/history" replace label="History" />
        </q-tabs>
      </q-layout-header>
  <q-page-container>
    <router-view />
  </q-page-container>
  <q-layout-footer>
    <q-toolbar-title>
        <div v-if="ENV!='Production'">
        ENV   : {{ API }} <br/>
        CONFIG: {{ STORE }}<br/>
        API   : {{ env }}<br/>
      </div>    </q-toolbar-title>
  </q-layout-footer>
      </q-layout>
  </div>
</template>

<script>
import { QTabs, QTab, QTabPane, QRouteTab } from 'quasar'

export default {
  components: {
    QTabs,
    QTab,
    QTabPane,
    QRouteTab
  },
  data () {
    return {
      API: process.env.__API__,
      STORE: process.env.__STORE__,
      ENV: process.env.__ENV__,
      X: process.env.__X__
    }
  }
}
</script>

<style>
.fa-alpha:before {
  font-size: 72;
  font-weight: 1000;
  content: "\03b1";
}
.layout-padding {
  /* max-width: 600px; */
  padding: 2px;
  background-image: "https://media.wired.com/photos/59338f9a2a38e4432c4ba0ab/master/pass/Iris-Ayasdi.png";
}
.q-card {
  padding: 1px;
  margin: 10px;
}
.q-card-container {
  padding: 1px;
  margin: 1px;
}
</style>
