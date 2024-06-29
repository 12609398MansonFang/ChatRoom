<template>
  <div class="w-full h-screen">
    <button @click="handleLogIn" v-if="loggedIn">Log Out</button>
    <Login v-if="!loggedIn" @loggedIn="handleLogIn" @userId="handleId"/>
    <DashBoard class="w-full h-full" v-else :currentUserId="userId?.valueOf()?? 0"/>
  </div>
</template>

<script setup lang="ts">

import Login from './Login.vue'
import DashBoard from './DashBoard.vue'
import { ref } from 'vue'

const loggedIn = ref<boolean>(false)
const userId = ref<number>(0)

const handleLogIn = () => {
  loggedIn.value = !loggedIn.value
}

const handleId = (userNumber: number) => {
  userId.value = userNumber
}

</script>
