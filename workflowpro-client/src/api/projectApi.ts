import axiosInstance from "./axiosInstance";

import type{
    CreateProjectDto,
  UpdateProjectDto,
  ProjectSummaryDto,
  ProjectResponseDto,
  ApiResponse,
  WorkItemSummaryDto,
  CreateWorkItemDto,
  WorkItemResponseDto,
  UpdateWorkItemStatusDto,
  ProjectMemberDto
} from '../types/project.types';
// import { data } from "react-router-dom";


export const projectApi = {

    //GET /api/Project
    getMyProjects : async () : Promise<ProjectSummaryDto[]> =>{
        const response = await axiosInstance.get<ApiResponse<ProjectSummaryDto[]>>(`/project`);
        return response.data.data;
    },

    //GET /api/Project/members
    getMembers : async (id : string) : Promise<ProjectMemberDto[]> =>{
        const response = await axiosInstance.get<ApiResponse<ProjectMemberDto[]>>(`/project/${id}/members`);
        return response.data.data;
    },

    //GET /api/Project/id
    getById : async ( id : string) : Promise<ProjectResponseDto> => {
        const response = await axiosInstance.get<ApiResponse<ProjectResponseDto>>(`/project/${id}`);
        return response.data.data 
    },

    //post /api/Project
    create : async (dto : CreateProjectDto) : Promise<CreateProjectDto> =>{
        const response = await axiosInstance.post<ApiResponse<ProjectResponseDto>>('/project',dto);
        return response.data.data;
    },

    //put /api/Project/id
    update : async(id : string , dto : UpdateProjectDto) : Promise<ProjectResponseDto> =>{
        const response = await  axiosInstance.put<ApiResponse<ProjectResponseDto>>(`/project/${id}`, dto);
        return response.data.data;
    },
    // DELETE /api/project/:id
    delete: async (id: string): Promise<void> => {
    await axiosInstance.delete(`/project/${id}`);
  },
};

export const workItemApi = {

 // GET /api/projects/{projectId}/workitems

 getbyProject : async (projectId : string) : Promise<WorkItemSummaryDto[]> => {

    const response = await axiosInstance.get<ApiResponse<WorkItemSummaryDto[]>>(`/projects/${projectId}/workitems`);
    return response.data.data;
 },
 // POST /api/projects/{projectId}/workitems
 create : async (projectId : string, dto : CreateWorkItemDto) : Promise<WorkItemResponseDto> => {

    const response = await axiosInstance.post<ApiResponse<WorkItemResponseDto>>(`/projects/${projectId}/workitems`, dto);
    return response.data.data;
 },

 // PATCH status
updateStatus: async (projectId: string, workItemId: string, dto: UpdateWorkItemStatusDto): Promise<WorkItemResponseDto> => {
  const response = await axiosInstance.patch<ApiResponse<WorkItemResponseDto>>(
    `/projects/${projectId}/workitems/${workItemId}/status`, dto
  );
  return response.data.data;
},
 //delete

 delete : async (projectId : string, workItemId : string) : Promise<void> => {
    await axiosInstance.delete(`/projects/${projectId}/workitems/${workItemId}`);
 }



}