<template>
  <div class='PageContainer flex justify-center py-10 h-screen gap-5'>
    <div class='UserContainer flex flex-col justify-end bg-slate-100 w-1/5 h-3/4 p-5'>
        <div class="UserDisplay" v-for="user in users" :key="user.userId">
          <p class="Users">{{ user.userId }} - {{ user.userName }} - {{ user.userEmail }}</p>
        </div>
    </div>

    <div class='MessageContainer flex flex-col justify-end bg-slate-100 w-3/5 h-3/4 p-5' >

      <div class="MessageDisplay py-2 mb-4 h-1/5 flex flex-col overflow-y-scroll">
        <div class="MessageText flex justify-between items-center px-2" v-for="message in messages" :key="message.messageId">
          <p class="TextContainer w-3/4 flex flex-col justify-end">{{ message.messageText }}</p>    
          <button @click="handleDeleteClick(message.messageId)" class="DeleteButton font-light" >Delete</button>
        </div>
      </div>

      <div class="MessageInterface flex justify-between">
        <input class="InputBar w-3/4 px-2" v-model="inputValue" placeholder="Type Something...." @keydown.enter="handleSendClick"/>
        <button class="SendButton flex items-center bg-slate-300 p-1" @click="handleSendClick">Click Me</button>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted} from 'vue'
import { getMessages, getUsers } from '../src/components/service'
import { sendMessage } from '../src/components/service'
import { deleteMessage } from '../src/components/service'
import type { Message } from '../src/types/message' 
import type { User } from '../src/types/user'

let messages = ref<Message[]>([]) 
let users = ref<User[]>([])

const inputValue = ref<string>('')

onMounted(async () => {
  messages.value = (await getMessages()).map(message => ({
      messageId: message.messageId,
      messageText: message.messageText,
  }))

  users.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
  }))
})


const handleSendClick = () => {
  let newMessage: Message

  if (inputValue.value.trim() !== '') {
    if (messages.value.length > 0) {
      const lastMessageId = messages.value[messages.value.length - 1].messageId;
      newMessage = { messageId: lastMessageId + 1, messageText: inputValue.value };
    } else {
      newMessage = { messageId: 1, messageText: inputValue.value };
    }

    sendMessage(newMessage);
    messages.value.push(newMessage);
    inputValue.value = ''; // Clear the input field
  } else {
    alert('Input Empty');
    inputValue.value = ''; // Clear the input field even if the input is empty
  }
}



const handleDeleteClick = async (messageId: number) => {
  await deleteMessage(messageId);
  messages.value = messages.value.filter(m => m.messageId !== messageId) 
}


</script>
