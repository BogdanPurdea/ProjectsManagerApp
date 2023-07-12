import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Project } from '../_models/project';
import { ProjectParams } from '../_models/projectParams';
import { map, of } from 'rxjs';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  baseUrl = environment.apiUrl;
  projects: Project[] = [];
  projectCache = new Map();
  projectParams: ProjectParams = new ProjectParams();

  constructor(private http: HttpClient) { }

  getProjectParams() {
    return this.projectParams;
  }

  setProjectParams(params: ProjectParams) {
    this.projectParams = params;
  }

  resetProjectParams() {
    this.projectParams = new ProjectParams();
    return this.projectParams;
  }

  getProjects(projectParams: ProjectParams) {
    var response = this.projectCache.get(Object.values(projectParams).join('-'));
    if(response) {
      return of(response);
    }

    let params = getPaginationHeaders(projectParams.pageNumber, projectParams.pageSize);
    params = params.append('orderBy', projectParams.orderBy.toString());

    return getPaginatedResult<Project[]>(this.baseUrl + 'projects', params, this.http)
      .pipe(map(response => {
        this.projectCache.set(Object.values(projectParams).join('-'), response);
        return response;
      }));
  }
}
