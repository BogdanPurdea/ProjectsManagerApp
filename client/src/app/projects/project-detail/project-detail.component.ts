import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Project } from 'src/app/_models/project';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProjectsService } from 'src/app/_services/projects.service';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  @ViewChild('projectTabs', { static: true }) projectTabs!: TabsetComponent;
  project!: Project;
  user!: User;
  activeTab!: TabDirective;


  constructor(private route: ActivatedRoute, private accountService: AccountService,
    private projectService: ProjectsService, private router: Router,
    private toastr: ToastrService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      if (user)
        this.user = user;
    })
  }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.project = data['project'];
    })

    this.route.queryParams.subscribe(params => {
      params['tab'] ? this.selectTab(params['tab']) :
        this.selectTab(0);
    })
  }

  selectTab(tabId: number) {
    this.projectTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
  }

  onDelete() {
    this.projectService.deleteProject(this.project.id);
  }

  updateProject() {
    this.projectService.updateProject(this.project.id, this.project).subscribe(() => {
      this.toastr.success('Project updated successfully');
    });
  }
}
