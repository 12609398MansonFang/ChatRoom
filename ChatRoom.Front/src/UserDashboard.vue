<template>
    <div class="PageContainer w-full h-screen flex justify-center items-center gap-10">

        <div class="RoomContainer w-1/5 h-3/5 shadow-2xl shadow-black flex flex-col gap-4 p-4">
            <div class="flex justify-between w-full">
                <h1 class="font-bold text-2xl w-full">Chat Rooms</h1>
                <div class="w-2/3" v-if="!showCreateRoom">
                    <button class="text-black p-1 w-full shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleCreateRoom">Create New</button>
                </div>
                <div class="w-2/3" v-else>
                    <button class="text-black p-1 w-full shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleCreateRoom">Existing</button>
                </div>

            </div>

            <div class="RoomInterface h-full w-full shadow-md shadow-black p-2 flex flex-col gap-2 overflow-y-scroll" v-if="!showCreateRoom">
                <button class="w-full shadow-md shadow-black p-2 flex hover:bg-lime-200" @click="handleRoomClick(0)">Reset</button>
                <div v-for="room in rooms" :key="room.roomId">

                    <div class="w-full shadow-md shadow-black flex justify-between bg-lime-300" v-if="room.roomId == rId">
                        <button class="w-2/3 text-sm text-black hover:text-white p-2">{{ room.roomName }}</button>
                        <button class="w-1/3 text-sm text-black hover:text-white p-2">Members</button>
                    </div>
                    <div class="w-full shadow-md shadow-black flex justify-between hover:bg-lime-200" v-else>
                        <button class="w-2/3 text-sm text-black hover:text-white p-2" @click="handleRoomClick(room.roomId)" >{{ room.roomName }}</button>
                        <button class="w-1/3 text-sm text-black hover:text-white p-2">Members</button>
                    </div>

                </div>
            </div>

            <div class="RoomInterface h-full w-full shadow-md shadow-black p-2 flex flex-col gap-2" v-else>

                <div class="Create flex flex-col h-full justify-center items-center gap-2">
                    <button class="Title font-bold text-l hover:text-slate-400">Create a Room</button>
                    <div class="NameContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Name:</h1>
                        <input class="LogInBarName px-2 shadow-sm shadow-black" v-model="inputRoomName" placeholder="Type Room Name"/>
                    </div>
                    <div class="DescriptionContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Desc:</h1>
                        <input class="LogInBarEmail px-2 shadow-sm shadow-black" v-model="inputRoomDescription" placeholder="Type Description"/>
                    </div>
                    <div class="PeopleContainer flex flex-col h-full w-full gap-2">
                        <h1 class="Title">Add Team:</h1>
                        <div class="flex flex-col h-full justify-center shadow-md shadow-black p-2 gap-1 overflow-y-scroll">
                            <div v-for="user in users" :key="user.userId">
                                <div class="shadow-sm shadow-black bg-lime-100" v-if="checkSelected(user)">
                                    <button class="w-full" v-if="user.userId != prop.propId" @click="toggleMemberSelection(user)">{{ user.userName }}</button>
                                </div>
                                <div class="shadow-sm shadow-black hover:bg-lime-100" v-if="!checkSelected(user)">
                                    <button class="w-full" v-if="user.userId != prop.propId" @click="toggleMemberSelection(user)">{{ user.userName }}</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <button class="CreateButton text-sm shadow-lg p-2 hover:bg-lime-200" @click="handleCreateRoomClick">Create Room</button>
                </div>

            </div>

        </div>

        <div class="MessageContainer w-2/5 h-3/5 shadow-2xl shadow-black flex flex-col justify-between gap-4 p-4">

            <div class="flex justify-between">
                <h1 class="font-bold text-2xl">Messages</h1>
                <h1>Welcome - {{ getUserName(prop.propId) }}</h1>
                <button class="text-black p-1 shadow-sm shadow-black hover:text-black hover:shadow-md hover:shadow-black" @click="handleLogOutClick">Log Out</button>
            </div>

            <div class="MessageDisplay h-full shadow-md shadow-black p-2 overflow-y-scroll">
                <div class="MessageText" v-for="message in userMessages" :key="message.messageId">
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

            <div class="overflow-y-scroll">
                <div class="NotificationInterface" v-for="room in rooms" :key="room.roomId">
                    {{ room.roomId }} - {{ room.roomName }}
                </div>
            </div>
        </div>

    </div>



</template>

<script setup lang="ts">
import { ref, onMounted, defineProps, defineEmits } from 'vue'

import { sendMessage, getMessages, deleteMessage, getUserMessages} from '../src/components/service'
import { getUsers } from '../src/components/service'
import { getRooms, addRoom } from '../src/components/service'

import type { Message } from '../src/types/message'
import type { User } from '../src/types/user'
import type { Room } from '../src/types/room'

onMounted(async () => {
    rooms.value = (await getRooms(prop.propId)).map(room => ({
        roomId: room.roomId,
        roomName: room.roomName,
        roomDescription: room.roomDescription
    }))

    messages.value = (await getMessages()).map(message => ({
        messageId: message.messageId,
        messageUserId: message.messageUserId,
        messageRoomId: message.messageRoomId,
        messageText: message.messageText
    }))

    users.value = (await getUsers()).map(user => ({
        userId: user.userId,
        userName: user.userName,
        userEmail: user.userEmail,
        userPassword: user.userPassword
    }))
})

//Rooms
let rooms = ref<any[]>([])
let userMessages = ref<Message[]>([]) 

const inputRoomName = ref<string>('')
const inputRoomDescription= ref<string>('')
const inputRoomMembers = ref<number[]>([])


const showCreateRoom = ref<boolean>(false)

const prop = defineProps<{ propId: number }>()
const rId = ref<number>(0)

const handleRoomClick = async (roomId: number) => {
    rId.value = roomId
    userMessages.value = await getUserMessages(rId.value)
}

const handleCreateRoom = () => {
    showCreateRoom.value = !showCreateRoom.value
}

const toggleMemberSelection  = (user: User) => {
    if (inputRoomMembers.value.includes(user.userId)) {
        inputRoomMembers.value = inputRoomMembers.value.filter(Id => Id !== user.userId)
    } else {
        inputRoomMembers.value.push(user.userId)
    }
}

const checkSelected = (user: User) => {
    if (inputRoomMembers.value.includes(user.userId)) {
        return true
    } else {
        return false 
    }
}

const handleCreateRoomClick = () => {
    let newRoom: Room
    if (inputRoomName.value.trim() !== '' && inputRoomDescription.value.trim() !== '' && inputRoomMembers.value.length > 0) {
        if (rooms.value.length > 0){
            inputRoomMembers.value.push(prop.propId)
            const lastRoomId = rooms.value[rooms.value.length -1].roomId
            newRoom = {roomId: lastRoomId + 1, roomName: inputRoomName.value, roomDescription: inputRoomDescription.value, roomMembers: inputRoomMembers.value}
        } else {
            newRoom = {roomId: 1, roomName: inputRoomName.value, roomDescription: inputRoomDescription.value, roomMembers: inputRoomMembers.value}

        }
        addRoom(newRoom)
        rooms.value.push(newRoom)
        inputRoomName.value = ''
        inputRoomDescription.value = ''
        inputRoomMembers.value = []
    } else {
        alert('Input Empty')
        inputRoomName.value = ''
        inputRoomDescription.value = ''
        inputRoomMembers.value = []
    }
}
    

//Messages
let messages = ref<Message[]>([]) 
let users = ref<User[]>([])

const inputValue = ref<string>('')

const getUserName = (userId: number) => {
    const username = users.value.find(u => u.userId === userId)
    return username ? username.userName : 'Unknown User'
}

const checkMessage = (message: Message) => {
    return prop.propId === message.messageUserId
}

const handleDeleteMessageClick = (messageId: number) => {
    deleteMessage(messageId);
    messages.value = messages.value.filter(m => m.messageId !== messageId)
    userMessages.value = userMessages.value.filter(m => m.messageId !== messageId)
}

const handleSendMessageClick = () => {
    let newMessage: Message

    if (inputValue.value.trim() !== '') {
        if (messages.value.length > 0) {
        const lastMessageId = messages.value[messages.value.length - 1].messageId
        newMessage = { messageId: lastMessageId + 1, messageUserId: prop.propId, messageRoomId: rId.value, messageText: inputValue.value }
        } else {
        newMessage = { messageId: 1, messageUserId: prop.propId, messageRoomId: rId.value, messageText: inputValue.value }
        }

        sendMessage(newMessage)
        messages.value.push(newMessage)
        userMessages.value.push(newMessage)
        inputValue.value = '' 
    } else {
        alert('Input Empty');
        inputValue.value = '' 
    }
}



const emitCustomEvents = defineEmits(['logOut'])

const handleLogOutClick = () => {
    emitCustomEvents('logOut')
}

</script>

<script lang="ts">
    import { defineComponent } from 'vue'
    export default defineComponent({name: 'UserDashboard'})
</script>