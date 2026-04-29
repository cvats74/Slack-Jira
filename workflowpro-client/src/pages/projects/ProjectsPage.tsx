import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { useAuth } from '../../context/AuthContext';
import { projectApi } from '../../api/projectApi';
import { type ProjectSummaryDto } from '../../types/project.types';

function ProjectsPage() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  // Projects data from API
  const [projects, setProjects] = 
    useState<ProjectSummaryDto[]>([]);

  // Loading state for initial fetch
  const [isLoading, setIsLoading] = useState(true);

  // Control create project modal visibility
  const [showCreateModal, setShowCreateModal] = useState(false);

  // New project form data
  const [newProject, setNewProject] = useState({
    name: '',
    description: '',
  });

  // Creating project loading state
  const [isCreating, setIsCreating] = useState(false);

  // Fetch projects when page loads
  useEffect(() => {
    // eslint-disable-next-line react-hooks/immutability
    fetchProjects();
  }, []); // [] = run once on mount

  const fetchProjects = async () => {
    try {
      setIsLoading(true);
      const data = await projectApi.getMyProjects();
      setProjects(data);
      
    }
     
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    catch (error) {
      toast.error('Failed to load projects');
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateProject = async (
    e: React.FormEvent
  ) => {
    e.preventDefault();

    if (!newProject.name.trim()) {
      toast.error('Project name is required');
      return;
    }

    setIsCreating(true);

    try {
      await projectApi.create({
        name: newProject.name,
        description: newProject.description,
      });

      toast.success('Project created!');
      
      // Reset form and close modal
      setNewProject({ name: '', description: '' });
      setShowCreateModal(false);
      
      // Refresh projects list
      fetchProjects();

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      toast.error(
        error.response?.data?.message || 
        'Failed to create project'
      );
    } finally {
      setIsCreating(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
    toast.success('Logged out successfully');
  };

  // Status to color mapping
  const getStatusColor = (status: string) => {
    const colors: Record<string, string> = {
      'Planning': '#6366f1',
      'Active': '#22c55e',
      'OnHold': '#f59e0b',
      'Completed': '#3b82f6',
      'Cancelled': '#ef4444',
    };
    return colors[status] || '#64748b';
  };

  return (
    <div style={styles.container}>

      {/* Navbar */}
      <nav style={styles.navbar}>
        <h1 style={styles.navTitle}>WorkFlow Pro</h1>
        <div style={styles.navRight}>
          <span style={styles.userName}>
            {user?.fullName}
          </span>
          <button 
            onClick={handleLogout}
            style={styles.logoutBtn}
          >
            Logout
          </button>
        </div>
      </nav>

      {/* Main Content */}
      <main style={styles.main}>

        {/* Page Header */}
        <div style={styles.pageHeader}>
          <div>
            <h2 style={styles.pageTitle}>My Projects</h2>
            <p style={styles.pageSubtitle}>
              {projects.length} project
              {projects.length !== 1 ? 's' : ''}
            </p>
          </div>
          <button
            onClick={() => setShowCreateModal(true)}
            style={styles.createBtn}
          >
            + New Project
          </button>
        </div>

        {/* Loading State */}
        {isLoading && (
          <div style={styles.centerMessage}>
            <p>Loading projects...</p>
          </div>
        )}

        {/* Empty State */}
        {!isLoading && projects.length === 0 && (
          <div style={styles.emptyState}>
            <p style={styles.emptyTitle}>
              No projects yet
            </p>
            <p style={styles.emptySubtitle}>
              Create your first project to get started
            </p>
            <button
              onClick={() => setShowCreateModal(true)}
              style={styles.createBtn}
            >
              Create Project
            </button>
          </div>
        )}

        {/* Projects Grid */}
        {!isLoading && projects.length > 0 && (
          <div style={styles.grid}>
            {projects.map(project => (
              <div
                key={project.id}
                style={styles.card}
                onClick={() => 
                  navigate(`/projects/${project.id}`)}
              >
                {/* Status Badge */}
                <div style={styles.cardHeader}>
                  <span style={{
                    ...styles.badge,
                    backgroundColor: 
                      getStatusColor(project.status) + '20',
                    color: getStatusColor(project.status),
                  }}>
                    {project.status}
                  </span>
                </div>

                {/* Project Name */}
                <h3 style={styles.cardTitle}>
                  {project.name}
                </h3>

                {/* Progress Bar */}
                <div style={styles.progressContainer}>
                  <div style={styles.progressBar}>
                    <div style={{
                      ...styles.progressFill,
                      width: `${project.progressPercentage}%`,
                    }} />
                  </div>
                  <span style={styles.progressText}>
                    {project.progressPercentage}%
                  </span>
                </div>

                {/* Stats */}
                <div style={styles.cardStats}>
                  <span style={styles.stat}>
                    👥 {project.memberCount} members
                  </span>
                  <span style={styles.stat}>
                    ✅ {project.taskCount} tasks
                  </span>
                </div>

                {/* Due Date */}
                {project.dueDate && (
                  <p style={styles.dueDate}>
                    Due: {new Date(project.dueDate)
                      .toLocaleDateString()}
                  </p>
                )}
              </div>
            ))}
          </div>
        )}
      </main>

      {/* Create Project Modal */}
      {showCreateModal && (
        <div style={styles.modalOverlay}>
          <div style={styles.modal}>
            <h3 style={styles.modalTitle}>
              Create New Project
            </h3>

            <form onSubmit={handleCreateProject}>
              <div style={styles.fieldGroup}>
                <label style={styles.label}>
                  Project Name *
                </label>
                <input
                  type="text"
                  value={newProject.name}
                  onChange={e => setNewProject(prev => ({
                    ...prev, name: e.target.value
                  }))}
                  placeholder="e.g. Mobile App Redesign"
                  style={styles.input}
                  autoFocus
                />
              </div>

              <div style={styles.fieldGroup}>
                <label style={styles.label}>
                  Description
                </label>
                <textarea
                  value={newProject.description}
                  onChange={e => setNewProject(prev => ({
                    ...prev, description: e.target.value
                  }))}
                  placeholder="What is this project about?"
                  style={styles.textarea}
                  rows={3}
                />
              </div>

              <div style={styles.modalButtons}>
                <button
                  type="button"
                  onClick={() => setShowCreateModal(false)}
                  style={styles.cancelBtn}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={isCreating}
                  style={styles.submitBtn}
                >
                  {isCreating 
                    ? 'Creating...' 
                    : 'Create Project'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

    </div>
  );
}

const styles = {
  container: {
    minHeight: '100vh',
    backgroundColor: '#f8fafc',
  },
  navbar: {
    backgroundColor: 'white',
    padding: '0 24px',
    height: '60px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    boxShadow: '0 1px 3px rgba(0,0,0,0.1)',
    position: 'sticky' as const,
    top: 0,
    zIndex: 100,
  },
  navTitle: {
    fontSize: '20px',
    fontWeight: '700',
    color: '#3b82f6',
  },
  navRight: {
    display: 'flex',
    alignItems: 'center',
    gap: '16px',
  },
  userName: {
    fontSize: '14px',
    color: '#374151',
    fontWeight: '500',
  },
  logoutBtn: {
    backgroundColor: 'transparent',
    border: '1px solid #d1d5db',
    borderRadius: '6px',
    padding: '6px 14px',
    fontSize: '13px',
    cursor: 'pointer',
    color: '#374151',
  },
  main: {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '32px 24px',
  },
  pageHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '32px',
  },
  pageTitle: {
    fontSize: '24px',
    fontWeight: '700',
    color: '#1e293b',
  },
  pageSubtitle: {
    fontSize: '14px',
    color: '#64748b',
    marginTop: '4px',
  },
  createBtn: {
    backgroundColor: '#3b82f6',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    padding: '10px 20px',
    fontSize: '14px',
    fontWeight: '600',
    cursor: 'pointer',
  },
  centerMessage: {
    textAlign: 'center' as const,
    padding: '60px',
    color: '#64748b',
  },
  emptyState: {
    textAlign: 'center' as const,
    padding: '80px 20px',
  },
  emptyTitle: {
    fontSize: '20px',
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: '8px',
  },
  emptySubtitle: {
    color: '#64748b',
    marginBottom: '24px',
  },
  grid: {
    display: 'grid',
    gridTemplateColumns: 
      'repeat(auto-fill, minmax(300px, 1fr))',
    gap: '20px',
  },
  card: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '20px',
    boxShadow: '0 1px 3px rgba(0,0,0,0.07)',
    cursor: 'pointer',
    transition: 'transform 0.15s, box-shadow 0.15s',
    border: '1px solid #e2e8f0',
  },
  cardHeader: {
    marginBottom: '12px',
  },
  badge: {
    display: 'inline-block',
    padding: '3px 10px',
    borderRadius: '999px',
    fontSize: '12px',
    fontWeight: '500',
  },
  cardTitle: {
    fontSize: '16px',
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: '16px',
  },
  progressContainer: {
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
    marginBottom: '14px',
  },
  progressBar: {
    flex: 1,
    height: '6px',
    backgroundColor: '#e2e8f0',
    borderRadius: '999px',
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#3b82f6',
    borderRadius: '999px',
    transition: 'width 0.3s ease',
  },
  progressText: {
    fontSize: '12px',
    color: '#64748b',
    minWidth: '32px',
  },
  cardStats: {
    display: 'flex',
    gap: '16px',
  },
  stat: {
    fontSize: '13px',
    color: '#64748b',
  },
  dueDate: {
    fontSize: '12px',
    color: '#94a3b8',
    marginTop: '10px',
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
    maxWidth: '480px',
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
  label: {
    fontSize: '13px',
    fontWeight: '500',
    color: '#374151',
  },
  input: {
    padding: '10px 14px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
  },
  textarea: {
    padding: '10px 14px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
    resize: 'vertical' as const,
    fontFamily: 'inherit',
  },
  modalButtons: {
    display: 'flex',
    gap: '12px',
    justifyContent: 'flex-end',
    marginTop: '24px',
  },
  cancelBtn: {
    backgroundColor: 'transparent',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    padding: '10px 20px',
    fontSize: '14px',
    cursor: 'pointer',
    color: '#374151',
  },
  submitBtn: {
    backgroundColor: '#3b82f6',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    padding: '10px 20px',
    fontSize: '14px',
    fontWeight: '600',
    cursor: 'pointer',
  },
};

export default ProjectsPage;