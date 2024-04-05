import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { ProjectsService } from '../_services/projects.service';
import { Project } from '../_models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectDetailedResolver implements Resolve<Project> {

  constructor(private projectService: ProjectsService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Project> {
    return this.projectService.getProject(Number(route.paramMap.get('id')));
  }
}
