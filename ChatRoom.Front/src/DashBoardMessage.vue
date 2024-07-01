<template>
    <div class="MessageContainer flex flex-col p-2 gap-4">

        <div class="ContainerTitle flex justify-between items-center shadow-md shadow-black p-2">
            <h1 class="font-bold text-2xl">Messages</h1>
        </div>

        <div class="ContainerDisplay h-full shadow-md shadow-black p-2 overflow-y-scroll" v-if="prop.currentRoomId == 0">
            Select A Room
        </div>

        <div class="ContainerDisplay h-full shadow-md shadow-black p-2 overflow-y-scroll" v-else>
            <div class="flex flex-col w-full gap-2">
                <div v-for="message in messages" :key="message.messageId">
                    <div v-if="checkMessageForRoom(message)">
                        <div class="flex w-full justify-end" v-if="checkMessageForUser(message)">
                            <div class="max-w-1/2">
                                <div class="flex justify-between">
                                    <button class="hover:text-slate-100 text-xs" @click="handleDeleteMessage(message)">Delete</button>
                                    <p class="text-sm">{{ getName(message) }}</p>
                                </div>
                                <p class="shadow-sm shadow-black p-1 bg-lime-200">{{ message.messageUserId }} - {{ message.messageText }} --- {{ message.messageRoomId }}</p>
                            </div>
                        </div>
                        <div class="flex w-full justify-start" v-else>
                            <div class="max-w-1/2">
                                <div class="flex">
                                    <p class="text-sm">{{ getName(message) }}</p>
                                </div>
                                <p class="shadow-sm shadow-black p-1 bg-rose-300">{{ message.messageUserId }} - {{ message.messageText }} --- {{ message.messageRoomId }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="MessageInterface flex justify-between shadow-md shadow-black p-1" v-if="prop.currentRoomId == 0">
            <input class="InputBar w-3/4 px-2" placeholder="Type Something...." :disabled="prop.currentRoomId == 0"/>
            <button class="SendButton flex text-slate-500 p-1 shadow-sm shadow-black" >Send</button>
        </div>

        <div class="MessageInterface flex justify-between shadow-md shadow-black p-1" v-else>
            <input class="InputBar w-3/4 px-2" v-model="messageInput" placeholder="Type Something...." @keydown.enter="handleSendMessage"/>
            <button class="SendButton flex text-black p-1 shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleSendMessage">Send</button>
        </div>

    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, defineProps } from 'vue'
import { getMessages, getUsers, sendMessage, deleteMessage} from './components/service';
import type { Message } from '../src/types/message'
import type { UserGeneral } from './types/user'

const prop = defineProps<{ currentUserId: number, currentRoomId: number}>()


let messages = ref<Message[]>([])
let userGeneral = ref<UserGeneral[]>([])
const messageInput = ref<string>('')

onMounted(async () => {
    messages.value = (await getMessages()).map(message => ({
        messageId: message.messageId,
        messageUserId: message.messageUserId,
        messageRoomId: message.messageRoomId,
        messageText: message.messageText
    })),

    userGeneral.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
    }))
})

const checkMessageForRoom = (message: Message) => {
    return prop.currentRoomId === message.messageRoomId
}

const checkMessageForUser = (message: Message) => {
    return prop.currentUserId === message.messageUserId
}

const getName = (message: Message) => {
    const name = userGeneral.value.find(user => user.userId === message.messageUserId)
    return name ? `${name.userName}` : 'Unknown Member'
}

const handleSendMessage = () => {
    let newMessage: Message

    if( messageInput.value.trim() !== ''){
        if (messages.value.length > 0){
            const lastMessageId = messages.value[messages.value.length - 1].messageId
            newMessage = { messageId: lastMessageId + 1, messageUserId: prop.currentUserId, messageRoomId: prop.currentRoomId, messageText: messageInput.value }
        } else {
            newMessage = { messageId: 1, messageUserId: prop.currentUserId, messageRoomId: prop.currentRoomId, messageText: messageInput.value }
        }
        sendMessage(newMessage)
        messages.value.push(newMessage)
        messageInput.value = ''
    } else {
        alert('Input Empty')
        messageInput.value = ''
    }
}

const handleDeleteMessage = (message: Message) => {
    deleteMessage(message.messageId)
    messages.value = messages.value.filter(m => m.messageId !== message.messageId)
}

</script>

<script lang="ts">
import { defineComponent } from 'vue'
export default defineComponent({ name: 'DashBoardMessage' })
</script>