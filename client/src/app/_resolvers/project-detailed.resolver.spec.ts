import { TestBed } from '@angular/core/testing';

import { ProjectDetailedResolver } from './project-detailed.resolver';

describe('ProjectDetailedResolver', () => {
  let resolver: ProjectDetailedResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(ProjectDetailedResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
