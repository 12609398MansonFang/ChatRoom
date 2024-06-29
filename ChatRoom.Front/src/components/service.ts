import axios from 'axios'
import type { Message } from '../types/message'
import type { User, UserGeneral } from '../types/user'
import type { Room } from '@/types/room'

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

// - get the rooms specific to the user that just logged in