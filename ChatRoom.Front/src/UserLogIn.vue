<template>
    <div class="LogInPage h-screen w-screen flex justify-center items-center">
        <div class="LogInContainer h-1/2 w-2/3 flex p-2 shadow-2xl shadow-emerald-800">
            <div class="ContainerDisplay w-3/5">
                {{logInMessage}}
            </div>
            <div class="ContainerInputs w-2/5 flex flex-col justify-center items-center gap-6">
                <div class="LogIn flex flex-col justify-center items-center gap-2">
                    <h1 class="Title font-bold text-xl">Login To Your Account</h1>
                    <div class="EmailContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Email:</h1>
                        <input class="LogInBarEmail px-2 shadow-lg" v-model="logInEmail" placeholder="Type Email" :disabled="showCreateAnAccount"/>
                    </div>
                    <div class="PasswordContainer flex justify-between w-full gap-2">
                        <h1 class="Title">Pass:</h1>
                        <input class="LogInBarPassword px-2 shadow-lg" v-model="logInPassword" placeholder="Type Password" :disabled="showCreateAnAccount"/>
                    </div>
                    <button class="LogInButton text-sm shadow-lg p-2 hover:text-white" @click="handleLogInClick" v-if="!showCreateAnAccount">Log In</button>
                    <button class="LogInButton text-sm shadow-lg p-2 text-slate-400 hover:text-slate-400" v-else>Log In</button>
                </div>

                <div class="Create flex flex-col justify-center items-center gap-2">
                    <button class="Title font-bold text-l hover:text-slate-400" @click="handleCreateAnAccount">Create An Account</button>
                    <div class="NameContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Name:</h1>
                        <input class="LogInBarName px-2 shadow-lg" v-model="createUsername" placeholder="Type Username"/>
                    </div>
                    <div class="EmailContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Email:</h1>
                        <input class="LogInBarEmail px-2 shadow-lg" v-model="createEmail" placeholder="Type Email"/>
                    </div>
                    <div class="PasswordContainer flex justify-between w-full gap-2" v-if="showCreateAnAccount">
                        <h1 class="Title">Pass:</h1>
                        <input class="LogInBarPassword px-2 shadow-lg" v-model="createPassword" placeholder="Type Password"/>
                    </div>
                    <button class="CreateButton text-sm shadow-lg p-2 hover:text-white " v-if="showCreateAnAccount" @click="handleAddUserClick">Create Account</button>
                </div>

            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, defineEmits} from 'vue'
import type { User } from './types/user'
import { addUser, getUsers, loginUser } from './components/service'


let users = ref<User[]>([])

const logInEmail = ref<string>('')
const logInPassword = ref<string>('')
const logInMessage = ref<string>('')

const createUsername = ref<string>('')
const createEmail = ref<string>('')
const createPassword = ref<string>('')



const showCreateAnAccount = ref<boolean>(false)
const emitCustomEvents = defineEmits(['loggedIn', 'userId'])


onMounted(async () => {
    users.value = (await getUsers()).map(user => ({
    userId: user.userId,
    userName: user.userName,
    userEmail: user.userEmail,
    userPassword: user.userPassword
    }))
})

const handleLogInClick = () => {
    if (logInEmail.value.trim() !== '' && logInPassword.value.trim() !== ''){
        const findUser = users.value.find(user => user.userEmail === logInEmail.value.trim())
        if (findUser){
            if (logInPassword.value.trim() == findUser.userPassword){
                console.log("Credentials Correct" + findUser.userEmail + " - " + findUser.userPassword)
                logInMessage.value = `Welcome ${findUser.userName}`
                emitCustomEvents('loggedIn')
                emitCustomEvents('userId', findUser.userId)
            } else {
                console.log("Credentials Correct" + " - " + findUser.userPassword)
                return logInMessage.value = 'Password Incorrect'
            }
        } else {
            console.log("User Not Found")
            return logInMessage.value = 'User Does Not Exist'
        }
    } else {
        console.log("INPUTS EMPTY")
        return logInMessage.value = 'Incomplete Inputs'
    }
    logInEmail.value = ''
    logInPassword.value = ''
}

const handleCreateAnAccount = () => {
    showCreateAnAccount.value = !showCreateAnAccount.value
}

const handleAddUserClick = () => {
    let newUser: User
    if(createUsername.value.trim() !== '' && createEmail.value.trim() !== '' && createPassword.value.trim() !== ''){
        if (users.value.length > 0){
            const lastUserId = users.value[users.value.length - 1].userId
            newUser = {userId: lastUserId + 1, userName: createUsername.value, userEmail: createEmail.value, userPassword: createPassword.value}
        } else {
            newUser = {userId: 1, userName: createUsername.value, userEmail: createEmail.value, userPassword: createPassword.value}
        }
        addUser(newUser)
        users.value.push(newUser)
        createUsername.value = ''
        createEmail.value = ''
        createPassword.value = ''
    } else {
        alert('Input Empty')
        createUsername.value = ''
        createEmail.value = ''
        createPassword.value = ''
    }
}

</script>


<script lang="ts">
    import { defineComponent } from 'vue'
    export default defineComponent({name: 'UserLogIn'})
</script>