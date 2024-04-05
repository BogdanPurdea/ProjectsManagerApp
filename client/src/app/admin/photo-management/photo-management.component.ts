import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit{
  photos!: Partial<Photo[]>;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe(photos => {
      this.photos = photos;
    });
  }

  approvePhoto(photo: Photo) {
    this.adminService.approvePhoto(photo.id).subscribe();
  }

  rejectPhoto(photo: Photo) {
    this.adminService.rejectPhoto(photo.id).subscribe();
  }
}
