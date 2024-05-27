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
    return this.getPaginatedProjects(projectParams, this.baseUrl + 'projects');

  }

  getProject(id: number) {
    const project = [...this.projectCache.values()]
      .reduce((array, element) => array.concat(element.result), [])
      .find((project: Project) => project.id === id);
    if(project) {
      return of(project);
    }
    return this.http.get<Project>(this.baseUrl + 'projects/' + id);
  }

  getPaginatedProjects(projectParams: ProjectParams, url: string) {
    var response = this.projectCache.get(Object.values(projectParams).join('-'));
    if(response) {
      return of(response);
    }

    let params = getPaginationHeaders(projectParams.pageNumber, projectParams.pageSize);
    params = params.append('orderBy', projectParams.orderBy.toString());
    params = params.append('hasUser', projectParams.hasUser.toString());

    return getPaginatedResult<Project[]>(url, params, this.http)
      .pipe(map(response => {
        this.projectCache.set(Object.values(projectParams).join('-'), response);
        return response;
      }));
  }

  updateProject(id: number, project: Project) {
    return this.http.put(this.baseUrl + 'projects/' + id, project).pipe(map(() => {
      const index = this.projects.indexOf(project);
      this.projects[index] = project;
    }));
  }

  deleteProject(id: number) {
    return this.http.delete(this.baseUrl + 'projects/' + id);
  }

  deleteFile(fileId: number) {
    return this.http.delete(this.baseUrl + 'projects/delete-file/'+ fileId);
  }
}
