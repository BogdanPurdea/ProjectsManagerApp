import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { Project } from 'src/app/_models/project';
import { ProjectParams } from 'src/app/_models/projectParams';
import { ProjectsService } from 'src/app/_services/projects.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit{
  projects!: Project[];
  pagination!: Pagination;
  projectParams!: ProjectParams;

  constructor(private projectService: ProjectsService) {
    this.projectParams = this.projectService.getProjectParams();
  }

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects() {
    this.projectService.setProjectParams(this.projectParams);
    this.projectService.getProjects(this.projectParams).subscribe(response => {
      this.projects = response.result;
      this.pagination = response.pagination;
    })
  }

  resetFilters() {
    this.projectParams = this.projectService.resetProjectParams();
    this.loadProjects();
  }

  pageChanged(event: any) {
    this.projectParams.pageNumber = event.page;
    this.projectService.setProjectParams(this.projectParams);
    this.loadProjects();
  }
}
