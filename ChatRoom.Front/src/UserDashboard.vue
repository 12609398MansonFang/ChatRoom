<template>
    <div class="PageContainer w-full h-screen flex justify-center items-center gap-10">

        <div class="RoomContainer w-1/5 h-3/5 shadow-2xl shadow-black flex flex-col gap-4 p-4">
            <h1 class="font-bold text-2xl">Chat Rooms ---- {{ rId }}</h1>
            <div class="RoomInterface h-full w-full shadow-md shadow-black p-2 flex flex-col gap-2">
                <button class="w-full shadow-md shadow-black p-2 flex hover:bg-lime-200" @click="handleRoomClick(0)">Reset</button>
                <button class="w-full shadow-md shadow-black p-2 flex hover:bg-lime-200" v-for="room in rooms" :key="room.roomId" @click="handleRoomClick(room.roomId)">
                {{ room.roomName }} - {{ room.roomDescription }} - {{ room.roomId }}
                </button>
            </div>
        </div>

        <div class="MessageContainer w-2/5 h-3/5 shadow-2xl shadow-black flex flex-col justify-between gap-4 p-4">

            <div class="flex justify-between">
                <h1 class="font-bold text-2xl">Messages</h1>
                <h1>Welcome {{ getUserName(prop.propId) }}</h1>
                <button class="text-black p-1 shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleLogOutClick">Log Out</button>
            </div>

            <div class="MessageDisplay h-full shadow-md shadow-black p-2 overflow-y-scroll" v-if="rId == 0"></div>

            <div class="MessageDisplay h-full shadow-md shadow-black p-2 overflow-y-scroll" v-else>

                <div class="MessageText" v-for="message in messages" :key="message.messageId">
                    <div :class="{'flex text-right justify-end w-full': checkMessage(message), 'flex justify-start text-left w-full': !checkMessage(message)}">
                        <div class="flex flex-col">

                            <p class="text-xs mt-1"> {{ getUserName(message.messageUserId) }}</p>

                            <div :class="{'flex justify-between items-center gap-2': true}">
                                <div v-if="checkMessage(message)">
                                    <button class="DeleteText text-slate-400 hover:text-slate-700" @click="handleDeleteMessageClick(message.messageId)">delete</button>
                                </div>
                                <p :class="{'bg-emerald-300 p-1 rounded-sm min-w-1/3': checkMessage(message), 'bg-red-300 p-1 rounded-sm min-w-1/3': !checkMessage(message)}">{{ message.messageText }}</p>
                            </div>

                        </div>
                    </div>
                </div>
                
            </div>

            <div class="MessageInterface flex justify-between shadow-md shadow-black p-1" v-if="rId == 0">
                <input class="InputBar w-3/4 px-2" v-model="inputValue" placeholder="Type Something...." @keydown.enter="handleSendMessageClick" :disabled="rId == 0"/>
                <button class="SendButton flex text-slate-500 p-1 shadow-sm shadow-black" >Send</button>
            </div>

            <div class="MessageInterface flex justify-between shadow-md shadow-black p-1" v-else>
                <input class="InputBar w-3/4 px-2" v-model="inputValue" placeholder="Type Something...." @keydown.enter="handleSendMessageClick" />
                <button class="SendButton flex text-black p-1 shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleSendMessageClick">Send</button>
            </div>

        </div>

        <div class="NotificationContainer w-1/5 h-3/5 shadow-2xl shadow-black gap-4 flex flex-col p-4">
            <h1 class="font-bold text-2xl">Notifications</h1>
            <div class="NotificationInterface h-full w-full shadow-md shadow-black p-2">
                
            </div>

        </div>

    </div>



</template>

<script setup lang="ts">
import { ref, onMounted, defineProps, defineEmits } from 'vue'

import { sendMessage, getMessages, deleteMessage} from '../src/components/service'
import { getUsers } from '../src/components/service'
import { getRooms } from '../src/components/service'

import type { Message } from '../src/types/message'
import type { User } from '../src/types/user'
import type { Room } from '../src/types/room'

const prop = defineProps<{ propId: number }>()

let rooms = ref<any[]>([])

const rId = ref<number>(0)


const handleRoomClick = (Id: number) => {
    rId.value = Id
}




const inputValue = ref<string>('')


const emitCustomEvents = defineEmits(['logOut'])

let messages = ref<Message[]>([]) 
let users = ref<User[]>([])



onMounted(async () => {
    messages.value = (await getMessages()).map(message => ({
        messageId: message.messageId,
        messageUserId: message.messageUserId,
        messageText: message.messageText
    }))

    users.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail,
    userPassword: user.userPassword
    }))

    rooms.value = (await getRooms(prop.propId)).map(room => ({
        roomId: room.roomId,
        roomName: room.roomName,
        roomDescription: room.roomDescription
    }))
})

const getUserName = (userId: number) => {
    const username = users.value.find(u => u.userId === userId)
    return username ? username.userName : 'Unknown User'
}

const checkMessage = (message: Message) => {
    return prop.propId === message.messageUserId
}

const handleSendMessageClick = () => {
    let newMessage: Message

    if (inputValue.value.trim() !== '') {
        if (messages.value.length > 0) {
        const lastMessageId = messages.value[messages.value.length - 1].messageId
        newMessage = { messageId: lastMessageId + 1, messageUserId: prop.propId, messageText: inputValue.value }
        } else {
        newMessage = { messageId: 1, messageUserId: prop.propId, messageText: inputValue.value }
        }

        sendMessage(newMessage)
        messages.value.push(newMessage)
        inputValue.value = '' 
    } else {
        alert('Input Empty');
        inputValue.value = '' 
    }
}

const handleDeleteMessageClick = (messageId: number) => {
    deleteMessage(messageId);
    messages.value = messages.value.filter(m => m.messageId !== messageId) 
}

const handleLogOutClick = () => {
    emitCustomEvents('logOut')
}

</script>

<script lang="ts">
    import { defineComponent } from 'vue'
    export default defineComponent({name: 'UserDashboard'})
</script>