<template>
    <div class="RoomContainer flex flex-col p-2 gap-4">

        <div class="ContainerTitle flex justify-between items-center shadow-md shadow-black p-2">
            <h1 class="font-bold text-2xl">Chat Rooms</h1>
            <button class="shadow-md shadow-black hover:shadow-lg hover:shadow-black p-1" v-if="!showCreateRoom" @click="handleCreateRoomClick">Create New</button>
            <button class="shadow-md shadow-black hover:shadow-lg hover:shadow-black p-1" v-else @click="handleCreateRoomClick">Existing</button>
        </div>

        <div class="ContainerInterface shadow-md shadow-black h-5/6">
            <div class="ContainerInterface h-full w-full p-2 flex flex-col gap-2" v-if="showCreateRoom">

                <h1 class="font-bold text-lg"> Create a Room</h1>
                <div class="NameContainer flex justify-between w-full gap-2">
                    <h1 class="Title">Name:</h1>
                    <input class="CreateName px-2 shadow-sm shadow-black" v-model="createRoomName" placeholder="Type Room Name"/>
                </div>
                <div class="DescriptionContainer flex justify-between w-full gap-2">
                    <h1 class="Title">Desc:</h1>
                    <input class="CreateDescription px-2 shadow-sm shadow-black" v-model="createRoomDescription" placeholder="Type Description"/>
                </div>
                <div class="PeopleContainer flex flex-col h-full w-full gap-2">
                    <h1 class="Title">Add Team:</h1>
                    <div class="flex flex-col justify-center items-start shadow-md shadow-black py-1 gap-1 overflow-y-scroll">

                        <div class="flex w-full justify-center" v-for="user in userGeneral" :key="user.userId">
                            <div class="hover:bg-lime-200 w-full" v-if="!checkSelected(user)">
                                <button class="w-full px-1" v-if="user.userId != prop.currentUserId" @click="handleSelectMember(user)"> {{ user.userName }} </button>
                            </div>
                            <div class="bg-lime-300 w-full" v-if="checkSelected(user)"> 
                                <button class="w-full px-1" v-if="user.userId != prop.currentUserId" @click="handleSelectMember(user)"> {{ user.userName }} </button>
                            </div>                          

                        </div>

                    </div>
                </div>
                <button class="CreateButton text-sm shadow-md shadow-black p-2 hover:bg-lime-200" @click="handleAddRoomClick">Create Room</button>



            </div>
            <div class="ContainerInterface h-full w-full p-2 flex flex-col gap-4 overflow-y-scroll" v-else>
                <h1 class="font-bold text-lg"> Select a Room</h1>
                
                <button class="w-full shadow-md shadow-black p-2 flex hover:bg-lime-200" @click="handleResetRoomClick(0)">Reset</button>
                <div v-for="room in rooms" :key="room.roomId">
                    <div>
                        <div class="w-full shadow-md shadow-black flex justify-between bg-lime-300" v-if="room.roomId == rId ">
                            <button class="w-2/3 text-sm text-black hover:text-white p-2 flex" @click="handleSelectRoomClick(room)">{{ room.roomName }}</button>
                            <button class="w-1/3 text-sm text-black hover:text-white p-2" @click="handleRoomPreviewClick(room)" >Members</button>
                        </div>
                        
                        <div class="w-full shadow-md shadow-black flex justify-between hover:bg-lime-200" v-else>
                            <button class="w-2/3 text-sm text-black hover:text-white p-2 flex" @click="handleSelectRoomClick(room)">{{ room.roomName }}</button>
                            <button class="w-1/3 text-sm text-black hover:text-white p-2" @click="handleRoomPreviewClick(room)" >Members</button>
                        </div>

                        <div class="w-full shadow-sm shadow-black" v-if="room.roomId === previewRoomId">
                            <div class=" bg-lime-100 w-full p-2">
                                <div class="flex justify-between" v-for="(memberId, index) in room.roomMembers" :key="index">
                                    <div class="text-sm text-black">{{ getMemberDetails(memberId) }}</div>
                                    <div class="text-sm text-black">{{ getAdmin(memberId, room) }}</div>  
                                </div>
                            </div>

                            <div class="PreviewButton" v-if="room.roomAdmin == prop.currentUserId">
                                <div class="flex justify-between py-1 px-2 bg-lime-50">
                                    <button class="hover:text-white" @click="handleAddMember">ADD</button>
                                    <button class="hover:text-white" @click="handleKickMember">KICK</button>
                                </div>
                                <div class="AddMember" v-if="showAddInterface">
                                    <p class="font-bold px-2">Pick Who You Want To Join The Group:</p>
                                    <div class="flex justify-center" v-for="user in getUserNotInRoom(room)" :key="user.userId">

                                        <div class="bg-lime-200 px-2" v-if="checkAddSelected(user)">
                                            <button @click="handleUserToBeAdded(user)">{{ user.userName }}</button>
                                        </div>
                                        <div class="hover:bg-lime-200 px-2" v-else>
                                            <button @click="handleUserToBeAdded(user)">{{ user.userName }}</button>
                                        </div>
                                    </div>
                                    <div class="flex justify-center bg-lime-50">
                                        <button class="hover:text-white" @click="handleAddMemberSubmit(room)">Done</button>
                                    </div>

                                </div>


                                <div class="KickMember" v-if="showKickInterface">
                                    <p class="font-bold px-2">Pick Who You Want Out The Group:</p>

                                    <div class="flex justify-center" v-for="user in getUserInRoom(room)" :key="user.userId">

                                        <div class="bg-lime-200 px-2" v-if="checkKickSelected(user)">
                                            <button @click="handleUserToBeKicked(user)">{{ user.userName }}</button>
                                        </div>

                                        <div class="hover:bg-lime-200 px-2" v-else>
                                            <button @click="handleUserToBeKicked(user)">{{ user.userName }}</button>
                                        </div>

                                    </div>

                                    <div class="flex justify-center bg-lime-50">
                                        <button class="hover:text-white" @click="handleKickMemberSubmit(room)">Done</button>
                                    </div>

                                </div>
                            </div>
                        </div>
                        
                    </div>
                </div>
                
            </div>
        </div>

    </div>
</template>

<script setup lang="ts">
import { ref, defineProps, onMounted, defineEmits } from 'vue'
import { getUsers, getRooms, createRoom, addUsers, kickUsers } from './components/service'
import type { AddRemoveUserRoom, Room } from './types/room'
import type { UserGeneral } from './types/user'

onMounted(async () => {
    rooms.value = (await getRooms(prop.currentUserId)).map(room => ({
        roomId: room.roomId,
        roomName: room.roomName,
        roomDescription: room.roomDescription,
        roomMembers: room.roomMembers,
        roomAdmin: room.roomAdmin
    })),

    userGeneral.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
    }))
})

const prop = defineProps<{ currentUserId: number }>()
const emitCustomEvents = defineEmits(['currentRoomId'])

let userGeneral = ref<UserGeneral[]>([])

const createRoomName = ref<string>('')
const createRoomDescription = ref<string>('')
const createRoomMembers = ref<number[]>([])

let rooms = ref<Room[]>([])
let rId = ref<number>(0)

const showCreateRoom = ref<boolean>(false)
const previewRoomId = ref<number | null>(null)
const showAddInterface = ref<boolean>(false)
const showKickInterface = ref<boolean>(false)

const usersToAdd = ref<number[]>([])
const usersToKick = ref<number[]>([])

const handleCreateRoomClick = () => {
    showCreateRoom.value = !showCreateRoom.value
}

const handleResetRoomClick = (Id: number) => {
    rId.value = Id
    emitCustomEvents('currentRoomId', rId.value)
}

const handleSelectRoomClick = (room: Room) => {
    rId.value = room.roomId
    emitCustomEvents('currentRoomId', rId.value)
}

const handleRoomPreviewClick = (room: Room) => {
    if (previewRoomId.value === room.roomId) {
        previewRoomId.value = null
        showAddInterface.value = false
        showKickInterface.value = false
    } else {
        previewRoomId.value = room.roomId
        showAddInterface.value = false
        showKickInterface.value = false
    }
}

const getMemberDetails = (memberId: Number) => {
    const member = userGeneral.value.find(user => user.userId === memberId)
    return member ? `${member.userName}` : 'Unknown Member'
}

const getAdmin = (memberId: number, room: Room) => {
    return room.roomAdmin === memberId ? 'admin' : 'standard'
}

const handleSelectMember = (user: UserGeneral) => {
    if (createRoomMembers.value.includes(user.userId)){
        createRoomMembers.value = createRoomMembers.value.filter(Id => Id !== user.userId)
    } else {
        createRoomMembers.value.push(user.userId)
    }
}

const checkSelected = (user: UserGeneral) => {
    if (createRoomMembers.value.includes(user.userId)){
        return true
    } else {
        return false    
    }
}

const handleAddRoomClick = () => {
    let newRoom: Room
    if (createRoomName.value.trim() !== '' && createRoomDescription.value.trim() !== '' && createRoomMembers.value.length > 0){
        if(rooms.value.length > 0){
            createRoomMembers.value.push(prop.currentUserId)
            const lastRoomId = rooms.value[rooms.value.length-1].roomId
            newRoom = {roomId: lastRoomId + 1, roomName: createRoomName.value, roomDescription: createRoomDescription.value, roomMembers: createRoomMembers.value, roomAdmin: prop.currentUserId }
        } else {
            newRoom = {roomId: 1, roomName: createRoomName.value, roomDescription: createRoomDescription.value, roomMembers: createRoomMembers.value, roomAdmin: prop.currentUserId }
        }
        createRoom(newRoom)
        rooms.value.push(newRoom)
        createRoomName.value =''
        createRoomDescription.value =''
        createRoomMembers.value = []
    } else {
        alert('Input Empty')
        createRoomName.value =''
        createRoomDescription.value =''
        createRoomMembers.value = []
    }
}

const handleAddMember = () => {
    usersToAdd.value = []
    showKickInterface.value = false
    showAddInterface.value = !showAddInterface.value
}

const handleKickMember = () => {
    usersToKick.value = []
    showAddInterface.value = false
    showKickInterface.value = !showKickInterface.value
}

const getUserNotInRoom = (room: Room) => {
    return userGeneral.value.filter(user => !room.roomMembers.includes(user.userId)) 
}

const getUserInRoom = (room: Room) => {
    return userGeneral.value.filter(user => room.roomMembers.includes(user.userId) && user.userId !== room.roomAdmin)
}

const handleUserToBeKicked = (user: UserGeneral) => {
    if (usersToKick.value.includes(user.userId)){
        usersToKick.value = usersToKick.value.filter(Id => Id !== user.userId)
    } else {
        usersToKick.value.push(user.userId)
    }
}

const checkKickSelected = (user: UserGeneral) => {
    if (usersToKick.value.includes(user.userId)){
        return true     
    } else {
        return false
    }
}

const handleUserToBeAdded = (user: UserGeneral) => {
    if (usersToAdd.value.includes(user.userId)) {
        usersToAdd.value = usersToAdd.value.filter(Id => Id !== user.userId)
    } else {
        usersToAdd.value.push(user.userId)
    }
}

const checkAddSelected = (user: UserGeneral) => {
    if (usersToAdd.value.includes(user.userId)){
        return true
    } else {
        return false
    }
}

const handleKickMemberSubmit = (room: Room) => {
    let usersToRemove: AddRemoveUserRoom 
    usersToRemove = { roomId: room.roomId,roomMembers: usersToKick.value }

    kickUsers(usersToRemove)


    const roomIndex = rooms.value.findIndex(r => r.roomId === room.roomId)
    if (roomIndex !== -1) {
        rooms.value[roomIndex].roomMembers = rooms.value[roomIndex].roomMembers.filter((userId: number) => !usersToKick.value.includes(userId)
        )
        rooms.value[roomIndex] = { ...rooms.value[roomIndex] }
    }

    usersToKick.value = []
    showKickInterface.value = false
}

const handleAddMemberSubmit = (room: Room) => {
    let newUsers: AddRemoveUserRoom
    newUsers = { roomId: room.roomId, roomMembers: usersToAdd.value}

    addUsers(newUsers)


    const roomIndex = rooms.value.findIndex(r => r.roomId === room.roomId)
    
    if (roomIndex !== -1) {

        rooms.value[roomIndex].roomMembers.push(...usersToAdd.value)
        
        rooms.value[roomIndex] = { ...rooms.value[roomIndex] }
    }

    usersToAdd.value = []
    showAddInterface.value = false
}



</script>

<script lang="ts">
import { defineComponent } from 'vue';
export default defineComponent({ name: 'DashBoardRoom' });
</script>