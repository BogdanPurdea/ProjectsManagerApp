import { Component, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Project } from 'src/app/_models/project';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProjectsService } from 'src/app/_services/projects.service';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})
export class ProjectEditComponent implements OnInit {
  @ViewChild('editForm') editForm!: NgForm;
  project!: Project;
  user!: User;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService,   
    private projectsService: ProjectsService,
    private toastr: ToastrService,
    private route: ActivatedRoute) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        if(user)
          this.user = user;
      });
      this.projectsService.getProject
    }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.project.id = params['project.id'];
    })
    this.loadProject();
  }

  loadProject() {
    this.projectsService.getProject(this.project.id).subscribe(project => {
      this.project = project;
    });
  }

  updateProject() {
    this.projectsService.updateProject(this.project.id).subscribe(() => {
      this.toastr.success('Project information updated successfully');
      this.editForm.reset(this.project);
    });
  }
}
