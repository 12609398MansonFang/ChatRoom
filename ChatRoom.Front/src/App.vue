<template>
  <div>
    <UserLogIn v-if="!loggedIn" @loggedIn="handleLogIn" @userId="handleId"/>
    <UserDashboard v-else :propId="userId?.valueOf()?? 0" @logOut="handleLogIn"/>
  </div>
</template>


<script setup lang="ts">
  import UserDashboard from './UserDashboard.vue'
  import UserLogIn from './UserLogIn.vue'
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
