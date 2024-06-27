import axios from 'axios'
import type { Message } from '../types/message'
import type { User } from '../types/user'
import type { Room } from '@/types/room'

const API_URL = 'http://localhost:5233'

// Messages

export async function getMessages(): Promise<Message[]> {
  const response = await axios.get<Message[]>(`${API_URL}/messages`)
  return response.data;
}

export async function getUserMessages( roomId: number ): Promise<Message[]> {
  const response = await axios.get(`${API_URL}/messages/${roomId}`)
  return response.data;
}

export async function sendMessage(inputMessage: Message): Promise<void> {
  await axios.post(`${API_URL}/messages/create`, inputMessage)
}

export async function deleteMessage(messageId: number): Promise<void> {
  try {
    await axios.delete(`${API_URL}/messages/${messageId}`);
    console.log('Message deleted successfully!');
    // Perform any additional logic after successful deletion
  } catch (error) {
    console.error('Error deleting message:', error);
    // Handle error if deletion fails
  }
}

// Users

export async function getUsers(): Promise<User[]>{
  const response = await axios.get<User[]>(`${API_URL}/users`)
  return response.data;
}

export async function addUser(inputUser: User): Promise<void> {
  await axios.post(`${API_URL}/users`, inputUser)
}

export async function loginUser(loginUser: User): Promise<void> {
  await axios.post(`${API_URL}/users/login`, loginUser.userEmail, loginUser.userPassword)
}

// Rooms
export async function getRooms(Id: number): Promise<any[]> { 
  const response = await axios.get(`${API_URL}/rooms/${Id}`)
  return response.data;
}

export async function addRoom(inputRoom: Room): Promise<void> {
  await axios.post(`${API_URL}/rooms/create`, inputRoom)
}