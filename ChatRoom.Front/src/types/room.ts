export interface Room {
    roomId: any
    roomName: any | null
    roomDescription: any | null
    roomMembers: any | null
    roomAdmin: any | null
}

export interface AddRemoveUserRoom {
    roomId: any
    roomMembers: any | null
}