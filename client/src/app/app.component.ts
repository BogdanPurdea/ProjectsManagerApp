import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Projects App';

  constructor(private accountService: AccountService, private presence: PresenceService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userJson = localStorage.getItem('user');
    const user: User = userJson ? JSON.parse(userJson) : null;
    if(user) {
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
  }
}
