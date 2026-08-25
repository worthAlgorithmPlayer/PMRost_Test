import { z } from 'zod';

export const ZProject = z.object({
  id: z.string(),
  number: z.string(),
  name: z.string(),
  budget: z.number(),
  startDate: z.string().optional().nullable(),
  endDate: z.string().optional().nullable(),
});

export const ZProjectsResponse = z.object({
  items: z.array(ZProject),
  totalCount: z.number(),
});

export const ZProjectReportItem = z.object({
  projectId: z.string(),
  projectName: z.string(),
  totalHours: z.number(),
  totalCost: z.number(),
  budget: z.number(),
});

export const ZProjectReportResponse = z.array(ZProjectReportItem);

export type ProjectType = z.infer<typeof ZProject>;
export type ProjectReportItemType = z.infer<typeof ZProjectReportItem>;