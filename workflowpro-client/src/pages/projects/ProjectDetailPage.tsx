/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { projectApi } from '../../api/projectApi';
import { workItemApi } from '../../api/projectApi';
import { useAuth } from '../../context/AuthContext';
import type {
  ProjectResponseDto,
  WorkItemSummaryDto,
  CreateWorkItemDto,
  ProjectMemberDto
} from '../../types/project.types';

// Status columns for Kanban board
// Each column has a name and numeric value matching backend enum
const KANBAN_COLUMNS = [
  { 
    label: 'Todo', 
    status: 'ToDo', 
    statusValue: 1,
    color: '#6366f1',
    bg: '#eef2ff'
  },
  { 
    label: 'In Progress', 
    status: 'InProgress', 
    statusValue: 2,
    color: '#f59e0b',
    bg: '#fffbeb'
  },
  { 
    label: 'In Review', 
    status: 'InReview', 
    statusValue: 3,
    color: '#8b5cf6',
    bg: '#f5f3ff'
  },
  { 
    label: 'Done', 
    status: 'Done', 
    statusValue: 5,
    color: '#22c55e',
    bg: '#f0fdf4'
  },
];

// Priority colors for task cards
const PRIORITY_COLORS: Record<string, string> = {
  Low: '#22c55e',
  Medium: '#f59e0b',
  High: '#ef4444',
  Critical: '#7c3aed',
};

function ProjectDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();

  // Project data
  const [project, setProject] = 
    useState<ProjectResponseDto | null>(null);

  // All tasks for this project
  const [workItems, setWorkItems] = 
    useState<WorkItemSummaryDto[]>([]);

  const [isLoading, setIsLoading] = useState(true);

  // Create task modal
  const [showCreateTask, setShowCreateTask] = 
    useState(false);

  // Which column triggered the create modal
  // So we pre-set the status
  const [defaultStatus, setDefaultStatus] = useState<number>(1);

    //members
    const[members, setMembers] = useState<ProjectMemberDto[]>([]);
    
  // New task form
  const [newTask, setNewTask] = 
    useState<CreateWorkItemDto>({
      title: '',
      description: '',
      priority: 2,
    });

  const [isCreating, setIsCreating] = useState(false);

  // Load project and tasks when page opens
  useEffect(() => {
    if (id) {
      // eslint-disable-next-line react-hooks/immutability
      loadData();
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const loadData = async () => {
    try {
      setIsLoading(true);

      // Load project details and tasks in parallel
      // Promise.all = run both requests simultaneously
      // Faster than running one after another
      const [projectData, tasksData, memberData] = await Promise.all([
        projectApi.getById(id!),
        workItemApi.getbyProject(id!),
        projectApi.getMembers(id!)
      ]);

      setProject(projectData);
      setWorkItems(tasksData);
      setMembers(memberData);

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (error) {
      toast.error('Failed to load project');
      navigate('/projects');
    } finally {
      setIsLoading(false);
    }
  };

  // Filter tasks by status for each Kanban column
  const getTasksByStatus = (status: string) => {
    return workItems.filter(t => t.status === status);
  };

  // Calculate progress
  const totalTasks = workItems.length;
  const doneTasks = workItems.filter(
    t => t.status === 'Done'
  ).length;
  const progress = totalTasks > 0
    ? Math.round((doneTasks / totalTasks) * 100)
    : 0;

  // Open create modal for specific column
  const handleAddTask = (statusValue: number) => {
    setDefaultStatus(statusValue);
    setNewTask({ title: '', description: '', priority: 2 });
    setShowCreateTask(true);
  };

  const handleCreateTask = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!newTask.title.trim()) {
      toast.error('Task title is required');
      return;
    }

    setIsCreating(true);

    try {
      await workItemApi.create(id!, newTask);
      toast.success('Task created!');
      setShowCreateTask(false);
      // Refresh tasks
      const tasks = await workItemApi.getbyProject(id!);
      setWorkItems(tasks);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      toast.error(
        error.response?.data?.message || 
        'Failed to create task'
      );
    } finally {
      setIsCreating(false);
    }
  };

  // Move task to different status column
  const handleStatusChange = async (
    taskId: string,
    newStatusValue: number
  ) => {
    try {
      await workItemApi.updateStatus(id!, taskId, {
        status: newStatusValue
      });

      // Update local state immediately
      // Don't wait for API refetch — feels instant
      setWorkItems(prev =>
        prev.map(t => {
          if (t.id === taskId) {
            const newStatus = KANBAN_COLUMNS.find(
              c => c.statusValue === newStatusValue
            )?.status || t.status;
            return { ...t, status: newStatus };
          }
          return t;
        })
      );

      toast.success('Status updated');
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (error) {
      toast.error('Failed to update status');
    }
  };

  if (isLoading) {
    return (
      <div style={styles.loadingContainer}>
        <p>Loading project...</p>
      </div>
    );
  }

  if (!project) return null;

  return (
    <div style={styles.container}>

      {/* Navbar */}
      <nav style={styles.navbar}>
        <div style={styles.navLeft}>
          <button
            onClick={() => navigate('/projects')}
            style={styles.backBtn}
          >
            ← Projects
          </button>
          <span style={styles.navDivider}>/</span>
          <h1 style={styles.navTitle}>{project.name}</h1>
        </div>
        <span style={styles.navUser}>{user?.fullName}</span>
      </nav>

      <main style={styles.main}>

        {/* Project Header */}
        <div style={styles.projectHeader}>
          <div>
            <div style={styles.projectMeta}>
              <span style={styles.statusBadge}>
                {project.status}
              </span>
              <span style={styles.metaText}>
                by {project.ownerName}
              </span>
              <span style={styles.metaText}>
                {project.memberCount} members
              </span>
            </div>
            {project.description && (
              <p style={styles.description}>
                {project.description}
              </p>
            )}
          </div>

          {/* Progress */}
          <div style={styles.progressSection}>
            <div style={styles.progressHeader}>
              <span style={styles.progressLabel}>
                Progress
              </span>
              <span style={styles.progressValue}>
                {progress}%
              </span>
            </div>
            <div style={styles.progressBar}>
              <div style={{
                ...styles.progressFill,
                width: `${progress}%`
              }} />
            </div>
            <span style={styles.progressSub}>
              {doneTasks} of {totalTasks} tasks done
            </span>
          </div>
        </div>

        {/* Kanban Board */}
        <div style={styles.kanban}>
          {KANBAN_COLUMNS.map(column => {
            const columnTasks = getTasksByStatus(
              column.status
            );

            return (
              <div
                key={column.status}
                style={styles.column}
              >
                {/* Column Header */}
                <div style={styles.columnHeader}>
                  <div style={styles.columnTitleRow}>
                    <span style={{
                      ...styles.columnDot,
                      backgroundColor: column.color
                    }} />
                    <span style={styles.columnTitle}>
                      {column.label}
                    </span>
                    <span style={{
                      ...styles.columnCount,
                      backgroundColor: column.bg,
                      color: column.color
                    }}>
                      {columnTasks.length}
                    </span>
                  </div>
                  <button
                    onClick={() => 
                      handleAddTask(column.statusValue)}
                    style={styles.addTaskBtn}
                    title="Add task"
                  >
                    +
                  </button>
                </div>

                {/* Task Cards */}
                <div style={styles.taskList}>
                  {columnTasks.map(task => (
                    <TaskCard
                      key={task.id}
                      task={task}
                      columns={KANBAN_COLUMNS}
                      onStatusChange={handleStatusChange}
                    />
                  ))}

                  {/* Empty column message */}
                  {columnTasks.length === 0 && (
                    <div style={styles.emptyColumn}>
                      <p style={styles.emptyColumnText}>
                        No tasks
                      </p>
                    </div>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </main>

      {/* Create Task Modal */}
        {showCreateTask && (
            <div style={styles.modalOverlay}>
                <div style={styles.modal}>
                <h3 style={styles.modalTitle}>
                    Create New Task
                </h3>

                <form onSubmit={handleCreateTask}>
                    
                    {/* Title */}
                    <div style={styles.fieldGroup}>
                    <label style={styles.label}>
                        Task Title *
                    </label>
                    <input
                        type="text"
                        value={newTask.title}
                        onChange={e => setNewTask(
                        (prev: CreateWorkItemDto) => ({
                            ...prev, title: e.target.value
                        })
                        )}
                        placeholder="e.g. Build login screen"
                        style={styles.input}
                        autoFocus
                    />
                    </div>

                    {/* Description */}
                    <div style={styles.fieldGroup}>
                    <label style={styles.label}>
                        Description
                    </label>
                    <textarea
                        value={newTask.description}
                        onChange={e => setNewTask(
                        (prev: CreateWorkItemDto) => ({
                            ...prev, description: e.target.value
                        })
                        )}
                        placeholder="Task details..."
                        style={styles.textarea}
                        rows={3}
                    />
                    </div>

                    {/* Priority + Due Date row */}
                    <div style={styles.row}>
                    <div style={styles.fieldGroup}>
                        <label style={styles.label}>
                        Priority
                        </label>
                        <select
                        value={newTask.priority}
                        onChange={e => setNewTask(
                            (prev: CreateWorkItemDto) => ({
                            ...prev,
                            priority: Number(e.target.value)
                            })
                        )}
                        style={styles.select}
                        >
                        <option value={1}>Low</option>
                        <option value={2}>Medium</option>
                        <option value={3}>High</option>
                        <option value={4}>Critical</option>
                        </select>
                    </div>

                    <div style={styles.fieldGroup}>
                        <label style={styles.label}>
                        Due Date
                        </label>
                        <input
                        type="date"
                        value={newTask.dueDate || ''}
                        onChange={e => setNewTask(
                            (prev: CreateWorkItemDto) => ({
                            ...prev, dueDate: e.target.value
                            })
                        )}
                        style={styles.input}
                        />
                    </div>
                    </div>

                    {/* Assignee dropdown */}
                    <div style={styles.fieldGroup}>
                    <label style={styles.label}>
                        Assign To
                    </label>
                    <select
                        value={newTask.assigneeId || ''}
                        onChange={e => {
                        const val = e.target.value;
                        setNewTask(
                            (prev: CreateWorkItemDto) => ({
                            ...prev,
                            assigneeId: val || undefined
                            })
                        );
                        }}
                        style={styles.select}
                    >
                        <option value="">Unassigned</option>
                        {members.map(member => (
                        <option
                            key={member.userId}
                            value={member.userId}
                        >
                            {member.fullName} ({member.role})
                        </option>
                        ))}
                    </select>
                    </div>

                    {/* Buttons */}
                    <div style={styles.modalButtons}>
                    <button
                        type="button"
                        onClick={() => setShowCreateTask(false)}
                        style={styles.cancelBtn}
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        disabled={isCreating}
                        style={styles.submitBtn}
                    >
                        {isCreating ? 'Creating...' : 'Create Task'}
                    </button>
                    </div>

                </form>
                </div>
            </div>
        )}

      
    </div>
  );
}

// =============================================
// TASK CARD COMPONENT
// Separate component for each task card
// =============================================

interface TaskCardProps {
  task: WorkItemSummaryDto;
  columns: typeof KANBAN_COLUMNS;
  onStatusChange: (
    taskId: string,
    statusValue: number
  ) => void;
}

function TaskCard({ task, columns, onStatusChange }: TaskCardProps) {
  const [showMenu, setShowMenu] = useState(false);

  const priorityColor = PRIORITY_COLORS[task.priority] 
    || '#64748b';

  return (
    <div style={styles.taskCard}>

      {/* Priority indicator */}
      <div style={{
        ...styles.priorityBar,
        backgroundColor: priorityColor
      }} />

      <div style={styles.taskContent}>

        {/* Task title */}
        <p style={styles.taskTitle}>{task.title}</p>

        {/* Task meta */}
        <div style={styles.taskMeta}>

          {/* Priority badge */}
          <span style={{
            ...styles.priorityBadge,
            color: priorityColor,
            backgroundColor: priorityColor + '15'
          }}>
            {task.priority}
          </span>

          {/* Overdue warning */}
          {task.isOverdue && (
            <span style={styles.overdueBadge}>
              Overdue
            </span>
          )}

          {/* Due date */}
          {task.dueDate && (
            <span style={styles.dueDate}>
              📅 {new Date(task.dueDate)
                .toLocaleDateString()}
            </span>
          )}
        </div>
       
        

        {/* Footer: assignee + status controls */}
        <div style={styles.taskFooter}>

          {/* Assignee */}
          <span style={styles.assignee}>
            {task.assigneeName
              ? `👤 ${task.assigneeName}`
              : '👤 Unassigned'}
          </span>

          {/* Quick status change dropdown */}
          <select
            value={columns.find(
              c => c.status === task.status
            )?.statusValue || 1}
            onChange={e => onStatusChange(
              task.id,
              Number(e.target.value)
            )}
            style={styles.statusSelect}
            onClick={e => e.stopPropagation()}
          >
            {columns.map(col => (
              <option
                key={col.statusValue}
                value={col.statusValue}
              >
                {col.label}
              </option>
            ))}
          </select>
        </div>
      </div>
    </div>
  );
}

// =============================================
// STYLES
// =============================================
const styles = {
  loadingContainer: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
    color: '#64748b',
  },
  container: {
    minHeight: '100vh',
    backgroundColor: '#f8fafc',
  },
  navbar: {
    backgroundColor: 'white',
    padding: '0 24px',
    height: '56px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    boxShadow: '0 1px 3px rgba(0,0,0,0.08)',
    position: 'sticky' as const,
    top: 0,
    zIndex: 100,
  },
  navLeft: {
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
  },
  backBtn: {
    background: 'none',
    border: 'none',
    cursor: 'pointer',
    color: '#3b82f6',
    fontSize: '14px',
    fontWeight: '500',
    padding: '4px 8px',
  },
  navDivider: {
    color: '#cbd5e1',
    fontSize: '16px',
  },
  navTitle: {
    fontSize: '16px',
    fontWeight: '600',
    color: '#1e293b',
  },
  navUser: {
    fontSize: '13px',
    color: '#64748b',
  },
  main: {
    maxWidth: '1400px',
    margin: '0 auto',
    padding: '24px',
  },
  projectHeader: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '24px',
    marginBottom: '24px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    boxShadow: '0 1px 3px rgba(0,0,0,0.06)',
    gap: '24px',
  },
  projectMeta: {
    display: 'flex',
    alignItems: 'center',
    gap: '12px',
    marginBottom: '8px',
  },
  statusBadge: {
    backgroundColor: '#dbeafe',
    color: '#1d4ed8',
    padding: '3px 10px',
    borderRadius: '999px',
    fontSize: '12px',
    fontWeight: '500',
  },
  metaText: {
    fontSize: '13px',
    color: '#64748b',
  },
  description: {
    fontSize: '14px',
    color: '#475569',
    marginTop: '4px',
    maxWidth: '600px',
  },
  progressSection: {
    minWidth: '200px',
  },
  progressHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    marginBottom: '8px',
  },
  progressLabel: {
    fontSize: '13px',
    color: '#64748b',
    fontWeight: '500',
  },
  progressValue: {
    fontSize: '13px',
    fontWeight: '700',
    color: '#1e293b',
  },
  progressBar: {
    height: '8px',
    backgroundColor: '#e2e8f0',
    borderRadius: '999px',
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#3b82f6',
    borderRadius: '999px',
    transition: 'width 0.5s ease',
  },
  progressSub: {
    fontSize: '11px',
    color: '#94a3b8',
    marginTop: '6px',
    display: 'block',
  },
  kanban: {
    display: 'grid',
    gridTemplateColumns: 'repeat(4, 1fr)',
    gap: '16px',
    alignItems: 'start',
  },
  column: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '16px',
    boxShadow: '0 1px 3px rgba(0,0,0,0.06)',
    minHeight: '400px',
  },
  columnHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '16px',
    paddingBottom: '12px',
    borderBottom: '1px solid #f1f5f9',
  },
  columnTitleRow: {
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
  },
  columnDot: {
    width: '8px',
    height: '8px',
    borderRadius: '50%',
    display: 'inline-block',
  },
  columnTitle: {
    fontSize: '14px',
    fontWeight: '600',
    color: '#1e293b',
  },
  columnCount: {
    fontSize: '11px',
    fontWeight: '600',
    padding: '2px 7px',
    borderRadius: '999px',
  },
  addTaskBtn: {
    background: 'none',
    border: '1px solid #e2e8f0',
    borderRadius: '6px',
    width: '26px',
    height: '26px',
    cursor: 'pointer',
    fontSize: '16px',
    color: '#64748b',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    lineHeight: 1,
  },
  taskList: {
    display: 'flex',
    flexDirection: 'column' as const,
    gap: '10px',
  },
  emptyColumn: {
    padding: '24px',
    textAlign: 'center' as const,
    border: '2px dashed #e2e8f0',
    borderRadius: '8px',
  },
  emptyColumnText: {
    fontSize: '13px',
    color: '#94a3b8',
  },
  taskCard: {
    border: '1px solid #e2e8f0',
    borderRadius: '8px',
    overflow: 'hidden',
    backgroundColor: '#fafafa',
    cursor: 'pointer',
    transition: 'box-shadow 0.15s',
  },
  priorityBar: {
    height: '3px',
    width: '100%',
  },
  taskContent: {
    padding: '12px',
  },
  taskTitle: {
    fontSize: '14px',
    fontWeight: '500',
    color: '#1e293b',
    marginBottom: '8px',
    lineHeight: 1.4,
  },
  taskMeta: {
    display: 'flex',
    flexWrap: 'wrap' as const,
    gap: '6px',
    marginBottom: '10px',
  },
  priorityBadge: {
    fontSize: '11px',
    fontWeight: '500',
    padding: '2px 8px',
    borderRadius: '999px',
  },
  overdueBadge: {
    fontSize: '11px',
    fontWeight: '500',
    padding: '2px 8px',
    borderRadius: '999px',
    backgroundColor: '#fef2f2',
    color: '#dc2626',
  },
  dueDate: {
    fontSize: '11px',
    color: '#64748b',
  },
  taskFooter: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: '8px',
  },
  assignee: {
    fontSize: '12px',
    color: '#64748b',
  },
  statusSelect: {
    fontSize: '11px',
    border: '1px solid #e2e8f0',
    borderRadius: '4px',
    padding: '2px 4px',
    backgroundColor: 'white',
    color: '#374151',
    cursor: 'pointer',
  },
  modalOverlay: {
    position: 'fixed' as const,
    inset: 0,
    backgroundColor: 'rgba(0,0,0,0.5)',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    zIndex: 200,
    padding: '20px',
  },
  modal: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '32px',
    width: '100%',
    maxWidth: '500px',
  },
  modalTitle: {
    fontSize: '18px',
    fontWeight: '700',
    color: '#1e293b',
    marginBottom: '24px',
  },
  fieldGroup: {
    display: 'flex',
    flexDirection: 'column' as const,
    gap: '6px',
    marginBottom: '16px',
  },
  row: {
    display: 'grid',
    gridTemplateColumns: '1fr 1fr',
    gap: '12px',
  },
  label: {
    fontSize: '13px',
    fontWeight: '500',
    color: '#374151',
  },
  input: {
    padding: '9px 13px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
  },
  textarea: {
    padding: '9px 13px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
    resize: 'vertical' as const,
    fontFamily: 'inherit',
  },
  select: {
    padding: '9px 13px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
    backgroundColor: 'white',
  },
  modalButtons: {
    display: 'flex',
    gap: '12px',
    justifyContent: 'flex-end',
    marginTop: '8px',
  },
  cancelBtn: {
    background: 'transparent',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    padding: '9px 20px',
    fontSize: '14px',
    cursor: 'pointer',
    color: '#374151',
  },
  submitBtn: {
    backgroundColor: '#3b82f6',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    padding: '9px 20px',
    fontSize: '14px',
    fontWeight: '600',
    cursor: 'pointer',
  },
};

export default ProjectDetailPage;