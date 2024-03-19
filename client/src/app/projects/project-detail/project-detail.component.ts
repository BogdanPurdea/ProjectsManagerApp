import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
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
export class ProjectDetailComponent implements OnInit{
@ViewChild('projectTabs', {static: true}) projectTabs!: TabsetComponent;
project!: Project;
activeTab!: TabDirective;

constructor(private route: ActivatedRoute, 
  private projectService: ProjectsService, private router: Router) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
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
}
