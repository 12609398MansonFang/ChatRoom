<template>
    <div>
        DashboardMessages - {{ prop.currentUserId }}
        CurrentRoom - 
    </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';

const prop = defineProps<{ currentUserId: number}>();
</script>

<script lang="ts">
import { defineComponent } from 'vue';
export default defineComponent({ name: 'DashBoardMessage' });
</script>