<template>
    <div class="RoomContainer flex flex-col p-2 gap-4">

        <div class="ContainerTitle h-1/6 flex justify-between items-center shadow-md shadow-black p-2">
            <h1 class="font-bold text-xl">Chat Rooms</h1>
            <button v-if="!showCreateRoom" @click="handleCreateRoom">Create New</button>
            <button v-else @click="handleCreateRoom">Existing</button>
        </div>

        <div class="ContainerInterface shadow-md shadow-black h-5/6">
            <div class="ContainerInterface" v-if="showCreateRoom">
            Interface for Create
            </div>
            <div class="ContainerInterface h-full w-full p-2 flex flex-col gap-2 overflow-y-scroll" v-else>
                <button class="w-full shadow-md shadow-black p-2 flex hover:bg-lime-200">Reset</button>
                <div v-for="room in rooms" :key="room.roomId">
                    <div class="w-full shadow-md shadow-black flex justify-between hover:bg-lime-200">
                        <button class="w-2/3 text-sm text-black hover:text-white p-2 flex">{{ room.roomName }}</button>
                        <button class="w-1/3 text-sm text-black hover:text-white p-2">Members</button>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script setup lang="ts">
import { ref, defineProps, onMounted, defineEmits } from 'vue';
import { getRooms } from './components/service';
import type { Room } from './types/room';

const prop = defineProps<{ currentUserId: number }>();


let rooms = ref<Room[]>([]);
const showCreateRoom = ref<boolean>(false);


onMounted(async () => {
    rooms.value = (await getRooms(prop.currentUserId)).map(room => ({
        roomId: room.roomId,
        roomName: room.roomName,
        roomDescription: room.roomDescription,
        roomMembers: room.roomMembers,
        roomAdmin: room.roomAdmin
    }));
});

const handleCreateRoom = () => {
    showCreateRoom.value = !showCreateRoom.value;
};



</script>

<script lang="ts">
import { defineComponent } from 'vue';
export default defineComponent({ name: 'DashBoardRoom' });
</script>