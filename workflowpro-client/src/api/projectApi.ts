import axiosInstance from "./axiosInstance";

import type{
    CreateProjectDto,
  UpdateProjectDto,
  ProjectSummaryDto,
  ProjectResponseDto,
  ApiResponse
} from '../types/project.types';


export const projectApi = {

    //GET /api/Project
    getMyProjects : async () : Promise<ProjectSummaryDto[]> =>{
        const response = await axiosInstance.get<ApiResponse<ProjectSummaryDto[]>>('/project');
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