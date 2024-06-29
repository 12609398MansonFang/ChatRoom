<template>
    <div class="flex justify-center items-center gap-10">
    <DashBoardRoom class="w-1/5 h-3/5 shadow-2xl shadow-black" :currentUserId="dashboardUser?.valueOf()?? 0"/>
    <DashBoardMessage class="w-2/5 h-3/5 shadow-2xl shadow-black" :currentUserId="dashboardUser?.valueOf()?? 0"/>
    <DashBoardNotification class="w-1/5 h-3/5 shadow-2xl shadow-black"/>
    </div>
</template>

<script setup lang="ts">
import { ref, defineProps } from 'vue';
import DashBoardRoom from './DashBoardRoom.vue';
import DashBoardMessage from './DashBoardMessage.vue';
import DashBoardNotification from './DashBoardNotification.vue';

const prop = defineProps<{ currentUserId: number }>();
const dashboardUser = prop.currentUserId;


</script>

<script lang="ts">
import { defineComponent } from 'vue';
export default defineComponent({ name: 'DashBoard' });
</script>