<template>
  <div id="q-app">
    <router-view />
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
body {
  background-color: black;
}

.q-layout {
  background-color: rgb(2, 26, 26)
}

.q-card {

  background-color: #2196f3 ;
  color:silver;
  border-radius: 10px;
  font-weight: lighter;
}
.q-card-title {
  background-color: #2196f3;
  color: lightyellow;
  font-weight: bold;
}

.q-card-container {

  background-color: rgb(2, 26, 26);
}

.button-icons {
  color: silver
}
</style>
