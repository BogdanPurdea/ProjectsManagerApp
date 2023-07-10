import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(userName: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + userName + '?roles=' + roles, {});
  }

  getPhotosForApproval() {
    return this.http.get<Partial<Photo[]>>(this.baseUrl + 'admin/photos-to-moderate');
  }

  approvePhoto(id: number) {
    return this.http.put(this.baseUrl + 'admin/approve-photo/' + id, {});
  }

  rejectPhoto(id: number) {
    return this.http.put(this.baseUrl + 'admin/reject-photo/' + id, {});
  }
}
