import { api } from '@/shared/api/public-api.config';
import { ZProjectsResponse } from '../model/project.schema';

export const getProjectsAll = async () => {
  const response = await api.get('projects', {
    params: { limit: 100 },
  });

  const parsed = ZProjectsResponse.safeParse(response.data);
  if (!parsed.success) {
    console.warn('GET /projects validation error:', parsed.error);
  }

  return parsed.data?.items ?? [];
};

export interface ProjectReportRow {
  projectId: string;
  projectNumber: string;
  projectName: string;
  budget: number;
  totalHours: number;
  totalCost: number;
  budgetUsagePercentage: number;
  isRisk: boolean;
  isOverrun: boolean;
}

export interface MonthlyProjectReportResult {
  rows: ProjectReportRow[];
  grandTotalHours: number;
  grandTotalCost: number;
  grandTotalBudget: number;
  grandBudgetUsagePercentage: number;
  isGrandRisk: boolean;
  isGrandOverrun: boolean;
}

export const getProjectReport = async (monthStr: string): Promise<MonthlyProjectReportResult> => {
  const [year, month] = monthStr.split('-').map(Number);
  
  const response = await api.get<MonthlyProjectReportResult>('/reports/projects', {
    params: { year, month },
  });
  
  return response.data;
};