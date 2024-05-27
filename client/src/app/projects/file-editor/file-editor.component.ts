import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Project } from 'src/app/_models/project';
import { ProjectFile } from 'src/app/_models/projectFile';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProjectsService } from 'src/app/_services/projects.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-file-editor',
  templateUrl: './file-editor.component.html',
  styleUrls: ['./file-editor.component.css']
})
export class FileEditorComponent implements OnInit{
  @Input() project!: Project;
  uploader!: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user!: User;

  constructor(private accountService: AccountService,
    private projectService: ProjectsService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        if(user)
          this.user = user;
      })
    }

  ngOnInit(): void {
    this.initializeUploader();
  }
  
  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  deleteFile(fileId: number) {
    this.projectService.deleteFile(fileId).subscribe(() => {
      this.project.files = this.project.files.filter(x => x.id != fileId);
    })
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'projects/add-file/' + this.project.id,
      authToken: 'Bearer' + this.user.token,
      isHTML5: true,
      allowedFileType: ['*'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const file: ProjectFile = JSON.parse(response);
        this.project.files.push(file);
      }
    };
  }
}
