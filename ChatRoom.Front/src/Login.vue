<template>
    <div class="LogInPage h-screen w-screen flex justify-center items-center">
        <div class="LogInContainer h-1/2 w-2/3 flex p-2 shadow-2xl shadow-emerald-800">
            <div class="ContainerDisplay w-3/5 flex flex-col">
                <p v-for="user in userGeneral" :key="user.userId">
                    {{user.userId}} : {{user.userName}} :  {{ user.userEmail }}
                </p>
            </div>
            <div class="ContainerInputs w-2/5 flex flex-col justify-center items-center gap-6">
                <div class="LogIn flex flex-col justify-center items-center gap-2">
                    <h1 class="Title font-bold text-xl">Login To Your Account</h1>
                    <div class="EmailContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Email:</h1>
                        <input class="LogInBarEmail px-2 shadow-lg" v-model="inputLoginEmail" placeholder="Type Email" :disabled="showCreateAnAccount"/>
                    </div>
                    <div class="PasswordContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Pass:</h1>
                        <input class="LogInBarPassword px-2 shadow-lg" v-model="inputLoginPassword" placeholder="Type Password" :disabled="showCreateAnAccount">
                    </div>
                    <button class="LogInButton text-sm shadow-lg p-2 hover:text-white" @click="handleLogInClick" v-if="!showCreateAnAccount">Log In</button>
                    <button class="LogInButton text-sm shadow-lg p-2 text-slate-400 hover:text-slate-400" v-else>Log In</button>
                </div>

                <div class="Create flex flex-col justify-center items-center gap-2">
                    <button class="Title font-bold text-l hover:text-slate-400" @click="handleCreateAnAccount" v-if="!showCreateAnAccount">Create An Account</button>
                    <button class="Title font-bold text-l hover:text-slate-400" @click="handleCreateAnAccount" v-if="showCreateAnAccount">Back To Log In</button>
                    <div class="NameContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Name:</h1>
                        <input class="LogInBarName px-2 shadow-lg" v-model="inputCreateUserName" placeholder="Type Username"/>
                    </div>
                    <div class="EmailContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Email:</h1>
                        <input class="LogInBarEmail px-2 shadow-lg" v-model="inputCreateEmail" placeholder="Type Email"/>
                    </div>
                    <div class="PasswordContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Pass:</h1>
                        <input class="LogInBarPassword px-2 shadow-lg" v-model="inputCreatePassword" placeholder="Type Password"/>
                    </div>
                    <button class="CreateButton text-sm shadow-lg p-2 hover:text-white " v-if="showCreateAnAccount" @click="handleCreateUserClick">Create Account</button>
                </div>

            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, defineEmits, onMounted } from 'vue'
import { loginUser, createUser, getUsers } from './components/service'
import type { User, UserGeneral } from './types/user'

let userGeneral = ref<UserGeneral[]>([])

const uId = ref<number>(null)
const showCreateAnAccount = ref <boolean>(false)

const emitCustomEvents = defineEmits(['loggedIn', 'userId'])

const inputLoginEmail = ref('')
const inputLoginPassword = ref('')

const inputCreateUserName = ref('')
const inputCreateEmail = ref('')
const inputCreatePassword = ref('')

onMounted(async () => {
    userGeneral.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail
    }))
})

const handleLogInClick = () => {
    loginUser(inputLoginEmail.value, inputLoginPassword.value).then(response => {
        if (response && response.userId) {
            uId.value = response.userId
            emitCustomEvents('loggedIn')
            emitCustomEvents('userId', uId.value)
        } else {
            alert('Credentials Incorrect Try Again')
            inputLoginEmail.value = ""
            inputLoginPassword.value = ""
        }
    }).catch(error => {
        alert('Credentials Error Try Again')
        inputLoginEmail.value = ""
        inputLoginPassword.value = ""
    });
}

const handleCreateAnAccount = () => {
    showCreateAnAccount.value = !showCreateAnAccount.value
}

const handleCreateUserClick = async () => {
    let newUser: User
    if(inputCreateUserName.value.trim() !== '' && inputCreateEmail.value.trim() !== '' && inputCreatePassword.value.trim() !== ''){
        if(userGeneral.value.length > 0){
            const lastUserId = userGeneral.value[userGeneral.value.length - 1].userId
            newUser = {userId: lastUserId + 1, userName: inputCreateUserName.value, userEmail: inputCreateEmail.value, userPassword: inputCreatePassword.value}
        } else {
            newUser = {userId: 1, userName: inputCreateUserName.value, userEmail: inputCreateUserName.value, userPassword: inputCreatePassword.value}
        }
        try {
            const response = await createUser(newUser)
            if (response.status === 201){
                userGeneral.value.push(newUser)
            }
        } catch (error) {
            alert('An error occurred while creating the user.')
        }
        inputCreateUserName.value = ''
        inputCreateEmail.value = ''
        inputCreatePassword.value = ''
    } else {
        alert('Inputs Empty')
        inputCreateUserName.value = ''
        inputCreateEmail.value = ''
        inputCreatePassword.value = ''     
    }
}

</script>


<script lang="ts">
    import { defineComponent} from 'vue'
    export default defineComponent({name: 'LogIn'})
</script>