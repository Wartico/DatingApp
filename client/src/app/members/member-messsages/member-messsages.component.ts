import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messsages',
  templateUrl: './member-messsages.component.html',
  styleUrls: ['./member-messsages.component.css'],
})
export class MemberMesssagesComponent implements OnInit {
  @Input() username: string;
  messages: Message[];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.messageService.getMessageThread(this.username).subscribe(messages => {
      this.messages = messages;
    });
  }
}
