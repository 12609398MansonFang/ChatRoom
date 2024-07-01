<template>
  <div class="w-full h-screen">
    <Login v-if="!loggedIn" @loggedIn="handleLogIn" @userId="handleId"/>

    <div class="w-full h-full flex flex-col justify-center items-center" v-else>

      <div class="w-4/6 flex justify-between shadow-lg shadow-black gap-48 p-2">
        <h1 class="font-bold text-xl p-2">Welcome - {{ getUserName() }}</h1>
        <button class="shadow-md shadow-black hover:shadow-xl hover:shadow-black p-2" @click="handleLogIn" >Log Out</button>
      </div>

      <DashBoard class="w-full h-5/6" :currentUserId="userId?.valueOf()?? 0"/>
    </div>

  </div>
</template>

<script setup lang="ts">
import Login from './Login.vue'
import DashBoard from './DashBoard.vue'
import { ref, onMounted } from 'vue'
import { getUsers } from './components/service'
import type { UserGeneral } from './types/user'

let userGeneral = ref<UserGeneral[]>([])

onMounted(async () => {
    userGeneral.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
    }))
})



const loggedIn = ref<boolean>(false)
const userId = ref<number>(0)

const handleLogIn = () => {
  loggedIn.value = !loggedIn.value
}

const handleId = (userNumber: number) => {
  userId.value = userNumber
}

const getUserName = () => {
  const name = userGeneral.value.find(user => user.userId === userId.value)
  return name ? `${name.userName}` : 'Unknown Member'
}

</script>
