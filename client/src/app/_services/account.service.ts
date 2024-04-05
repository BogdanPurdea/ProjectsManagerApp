import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, catchError, map, tap, throwError } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      tap({
          next: (user: User) => {
            if (user) {
              this.setCurrentUser(user);
              this.presence.createHubConnection(user);
            }
          },
          error: (error) => {
            console.error('Login error:', error);
            return throwError(() => new Error('Login failed'));  // Rethrow error as Observable
          }
        })
    )
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      tap({
        next: (user: any) => {
          if (user) {
            this.setCurrentUser(user);
            this.presence.createHubConnection(user);
          }
        }, error: (error) => {
          console.error('Register error:', error);
          return throwError(() => new Error('Registration failed'));  // Rethrow error as Observable
        }
      })
    )
  }

  setCurrentUser(user: User) {
    if (user != null) {
      user.roles = [];
      const roles = this.getDecodedToken(user.token).role;
      Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    }
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split(".")[1]));
  }
}
