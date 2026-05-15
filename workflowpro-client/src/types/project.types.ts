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
   progressPercentage: number;
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


//=================workItem=================
export interface WorkItemSummaryDto {
  id: string;
  title: string;
  status: string;
  priority: string;
  dueDate?: string;
  assigneeId?: string;
  assigneeName?: string;
  subTaskCount: number;
  commentCount: number;
  isOverdue: boolean;
}

export interface WorkItemResponseDto {
  id: string;
  title: string;
  description: string;
  status: string;
  priority: string;
  dueDate?: string;
  createdAt: string;
  estimatedHours?: number;
  actualHours?: number;
  projectId: string;
  projectName: string;
  reporterId: string;
  reporterName: string;
  assigneeId?: string;
  assigneeName?: string;
  subTasks: WorkItemSummaryDto[];
  commentCount: number;
}

export interface CreateWorkItemDto {
  title: string;
  description: string;
  priority: number;
  dueDate?: string;
  assigneeId?: string;
  estimatedHours?: number;
}
export interface ProjectMemberDto {
  userId: string;
  fullName: string;
  email: string;
  role: string;
  joinedAt: string;
}

export interface UpdateWorkItemStatusDto {
  status: number;
}