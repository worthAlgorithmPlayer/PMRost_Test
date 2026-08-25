import { api } from '@/shared/api/public-api.config';

import { ZTimeEntriesResponse } from '../model/time-entry.schema';
import type { TimeEntryFormType } from '../model/time-entry.schema';
import type { UpdateTimeEntryFormType } from '../model/time-entry.schema';
import { ZTimeEntryFilter, type TimeEntryFilterInputType } from '../model/time-entry.schema';




export const getTimeEntriesAll = async (filters: TimeEntryFilterInputType) => {
  const parsedFilters = ZTimeEntryFilter.parse(filters);

  const response = await api.get('time-entries', {
    params: parsedFilters,
  });

  const parsed = ZTimeEntriesResponse.safeParse(response.data);
  if (!parsed.success) {
    console.warn('GET /time-entries validation error:', parsed.error);
  }

  return parsed.data?.items ?? [];
};

export const createTimeEntry = async (payload: TimeEntryFormType) => {
  const response = await api.post('time-entries', payload);
  return response.data;
};

export const updateTimeEntry = async (data: UpdateTimeEntryFormType) => {
  const { id, hours, comment, version } = data;

  await api.put(`time-entries/${id}`, {
    hours,
    comment,
    version,
  });
};

export const deleteTimeEntry = async (id: string) => {
  const response = await api.delete(`time-entries/${id}`);
  return response.data;
};
