import { Component } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Component({
  selector: 'app-root',
  template: `
  `,
  styles: []
})
export class AppComponent {
  title = 'Frontend';
  connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/messageHub")
    .build();
  ngOnInit() {
    this.connection.on("MessageReceived", (message) => {
      console.log(message);
    });
    this.connection.start().catch(err => document.write(err));
  }
}
