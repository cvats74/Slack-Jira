export interface CreateProjectDto {
  name: string;
  description: string;
  startDate?: string;
  dueDate?: string;
  coverImageUrl?: string;
}

export interface UpdateProjectDto {
  name: string;
  description: string;
  startDate?: string;
  dueDate?: string;
  status: number;
}

export interface ProjectSummaryDto {
  id: string;
  name: string;
  status: string;
  dueDate?: string;
  memberCount: number;
  taskCount: number;
  progressPercentage: number;
}

export interface ProjectResponseDto {
  id: string;
  name: string;
  description: string;
  status: string;
  startDate?: string;
  dueDate?: string;
  coverImageUrl?: string;
  createdAt: string;
  ownerName: string;
  memberCount: number;
  taskCount: number;
  completedTaskCount: number;
}

// Standard API response wrapper
// Matches your C# ApiResponse<T>
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
  statusCode: number;
}