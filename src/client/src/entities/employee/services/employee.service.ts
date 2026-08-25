import { api } from '@/shared/api/public-api.config.ts';
import { ZEmployeesResponse } from '../model/employee.schema';

export const getEmployeesAll = async () => {
  const response = await api.get('employees', {
    params: { limit: 100 },
  });

  const parsed = ZEmployeesResponse.safeParse(response.data);
  if (!parsed.success) {
    console.warn('GET /employees validation error:', parsed.error);
  }

  return parsed.data?.items ?? [];
};