import axios from 'axios'
import type { Message } from '../types/message'
import type { User } from '../types/user'

const API_URL = 'http://localhost:5233'


export async function getMessages(): Promise<Message[]> {
  const response = await axios.get<Message[]>(`${API_URL}/messages`)
  return response.data;
}

export async function sendMessage(inputMessage: Message): Promise<void> {
  await axios.post(`${API_URL}/messages`, inputMessage)
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


export async function getUsers(): Promise<User[]>{
  const response = await axios.get<User[]>(`${API_URL}/users`)
  return response.data;
}

export async function addMessage(inputUser: User): Promise<void> {
  await axios.post(`${API_URL}/users`, inputUser)
}