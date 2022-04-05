export interface Message {
    id: number;
    sednerId: number;
    senderUsername: string,
    senderPhotoUrl: string;
    recipientId: number;
    recipientUsername: string;
    recipientPhotUrl: string;
    content: string;
    dateRead?: Date;
    messageSent: Date;
}
