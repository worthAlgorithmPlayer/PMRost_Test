import { z } from 'zod';

export const ZEmployee = z.object({
  id: z.string(),
  name: z.string(),
  department: z.string().optional().nullable(),
});

export const ZEmployeesResponse = z.object({
  items: z.array(ZEmployee),
  totalCount: z.number(),
});

export type EmployeeType = z.infer<typeof ZEmployee>;