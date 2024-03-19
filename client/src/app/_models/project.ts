export interface Project {
    id: number;
    projectName: string;
    projectDescription: string;
    creatorName: string;
    isFinished: boolean;
    isApproved: boolean;
    contributors: string[];
  }