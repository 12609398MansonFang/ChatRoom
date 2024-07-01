import axios from 'axios'
import type { Message } from '../types/message'
import type { User, UserGeneral} from '../types/user'
import type { Room, AddRemoveUserRoom } from '@/types/room'

const API_URL = 'http://localhost:5233'

// USERS

// - login using email and password as input 
export async function loginUser(inputEmail: string, inputPassword: string): Promise<any> {
  try {
    const response = await axios.get(`${API_URL}/users/login/email/password/${encodeURIComponent(inputEmail)}/${encodeURIComponent(inputPassword)}`)
    return response.data
  } catch (error) {
    console.error('Credentials Incorrect', error)
    throw error    
  }
}

// - get all general user information
export async function getUsers(): Promise<UserGeneral[]>{
  const response = await axios.get<UserGeneral[]>(`${API_URL}/users/all`)
  return response.data;
}

// - create an account 
export async function createUser(inputUser: User): Promise<any> {
  await axios.post(`${API_URL}/users/create`, inputUser)
}

// ROOMS

// - get the rooms specific to the user that just logged in
export async function getRooms(Id: number): Promise<Room[]> { 
  const response = await axios.get(`${API_URL}/rooms/user${Id}`)
  return response.data;
}

// - create a room with details that have been inserted
export async function createRoom(room: Room): Promise<void> {
  await axios.post(`${API_URL}/rooms/create`, room)
}

// - add users from rooms
export async function addUsers(addRemoveUserRoom : AddRemoveUserRoom): Promise<any> {
  await axios.put(`${API_URL}/rooms/add/member`, addRemoveUserRoom)
}

// - remove users from rooms
export async function kickUsers(addRemoveUserRoom : AddRemoveUserRoom): Promise<any> {
  await axios.post(`${API_URL}/rooms/remove/member`, addRemoveUserRoom)
}

// MESSAGES

// - get all messages
export async function getMessages(): Promise<Message[]> {
  const response = await axios.get<Message[]>(`${API_URL}/messages/all`)
  return response.data;
}

// - creates message 
export async function sendMessage(inputMessage: Message): Promise<void> {
  await axios.post(`${API_URL}/messages/create`, inputMessage)
}

// - deletes message
export async function deleteMessage(messageId: number): Promise<void> {
    await axios.delete(`${API_URL}/messages/delete${messageId}`);
}