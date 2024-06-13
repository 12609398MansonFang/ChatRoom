<template>
  <div class='PageContainer flex justify-center py-10 h-screen gap-5'>
    <div class='UserContainer flex flex-col justify-end bg-slate-100 w-1/5 h-3/4 p-5'>

        <div class="UserDisplay" v-for="user in users" :key="user.userId">
          <button class="Users hover:bg-orange-400" @click="handleUserClick(user)">{{ user.userId }} : {{ user.userName }} - {{ user.userEmail }}</button>
        </div>

        <div class="UserCreate flex flex-col justify-between">
          <h1>Enter Name:</h1>
          <input class="InputBarName px-2" v-model="inputName" placeholder="Type Name......"/>
          <h1>Enter Email:</h1>
          <input class="InputBarEmail px-2" v-model="inputEmail" placeholder="Type Email......"/>
          <button class="bg-slate-300" @click="handleAddUserClick">Create</button>

        </div>


    </div>

    <div class='MessageContainer flex flex-col justify-end bg-slate-100 w-3/5 h-3/4 p-5' >

      <div class="MessageDisplay py-2 overflow-y-scroll">

        <div class="MessageText flex justify-between" v-for="message in messages" :key="message.messageId">
          <div :class="{'flex justify-end text-right w-full': checkMessage(message), 'flex justify-start text-left w-full': !checkMessage(message)}">
            <div class="flex flex-col">
              <p> {{getUserName(message.messageUserId)}}</p>
              <p :class="{'bg-emerald-300 p-2 rounded-xl': checkMessage(message), 'bg-red-300 p-2 rounded-xl': !checkMessage(message)}">{{ message.messageText }}</p>
            </div>
          </div>
        </div>


        
      </div>

      <div class="MessageInterface flex justify-between">

        <input class="InputBar w-3/4 px-2" v-model="inputValue" placeholder="Type Something...." @keydown.enter="handleSendMessageClick"/>
        <button class="SendButton flex items-center bg-slate-300 p-1" @click="handleSendMessageClick">Send</button>
        
      </div>

    </div>

    <div>Current Account Selected: {{ currentUser }}</div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted} from 'vue'
import { addUser, getMessages, getUsers } from '../src/components/service'
import { sendMessage } from '../src/components/service'
import { deleteMessage } from '../src/components/service'
import type { Message } from '../src/types/message' 
import type { User } from '../src/types/user'


let messages = ref<Message[]>([]) 
let users = ref<User[]>([])
let currentUser = ref<number>(0)
let messageCheck = ref<boolean>(false)

const inputValue = ref<string>('')
const inputName = ref<string>('')
const inputEmail = ref<string>('')

onMounted(async () => {
  messages.value = (await getMessages()).map(message => ({
      messageId: message.messageId,
      messageUserId: message.messageUserId,
      messageText: message.messageText
  }))

  users.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
  }))
})


const handleSendMessageClick = () => {
  let newMessage: Message

  if (inputValue.value.trim() !== '') {
    if (messages.value.length > 0) {
      const lastMessageId = messages.value[messages.value.length - 1].messageId
      newMessage = { messageId: lastMessageId + 1, messageUserId: currentUser.value, messageText: inputValue.value }
    } else {
      newMessage = { messageId: 1, messageUserId: currentUser.value, messageText: inputValue.value }
    }

    sendMessage(newMessage)
    messages.value.push(newMessage)
    inputValue.value = '' 
  } else {
    alert('Input Empty');
    inputValue.value = '' 
  }
}

const handleDeleteMessageClick = async (messageId: number) => {
  await deleteMessage(messageId);
  messages.value = messages.value.filter(m => m.messageId !== messageId) 
}

const handleAddUserClick = () => {
  let newUser: User

  if (inputName.value.trim() !== '' && inputEmail.value.trim() !== ''){
    if (users.value.length > 0){
      const lastUserId = users.value[users.value.length - 1].userId
      newUser = {userId: lastUserId + 1, userName: inputName.value, userEmail: inputEmail.value}
    } else {
      newUser = {userId: 1, userName: inputName.value, userEmail: inputEmail.value}
    }

    addUser(newUser)
    users.value.push(newUser)
    inputName.value = ''
    inputEmail.value = ''
  } else {
    alert('Input Empty');
    inputName.value = '' 
    inputEmail.value = ''   
  }
}

const handleUserClick = (user: User) => {
  currentUser.value = user.userId
}

const checkMessage = (message: Message) => {
  return currentUser.value === message.messageUserId
}

const getUserName = (userId: number) => {
  const username = users.value.find(u => u.userId === userId)
  return username ? username.userName : 'Unknown User'
}


</script>
